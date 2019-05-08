using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using AssetsCore;
using LitJson;

// TODO: Maybe refactor
namespace AssetsCore
{
    public class Localization
    {
        public static string Current
        {
            get { return _current; }
            set { SetCurrent(value); }
        }

        public static bool IsDefault { get { return Current == DefaultLanguage; } }

        public static event Action OnChanged;

        public static List<string> Keys
        {
            get
            {
                return Default.Keys.ToList();
            }
        }

        public static string CurrentId { get; private set; }

        private const string DefaultLanguage = "English";
        private const string LocalizationPath = "Localization/";

        private static string _current;
        private static Dictionary<string, string> Strings;
        private static Dictionary<string, string> Default;

        /* */

        public static string Get(string key, string defaultValue = null)
        {
            return GetValue(key, defaultValue);
        }

        public static void Set(string key, string value)
        {
            Strings[key] = value;
        }

        public static bool HasKey(string key)
        {
            return Strings.ContainsKey(key);
        }

        private static Dictionary<string, string> IDS;

        /* */

        public static void Check(List<string> supportLocalizations)
        {
            var pattern = Load(DefaultLanguage);

            foreach (var it in supportLocalizations)
            {
                if (it == DefaultLanguage) continue;

                var compare = Load(it);
                if (compare.Count < 1) continue;

                foreach (var key in pattern.Keys.Where(key => !compare.ContainsKey(key)))
                {
                    Debug.LogError(string.Format("Localization: ({0}) {1} not found", it, key));
                }
            }
        }

        /* */

        static Localization()
        {
            // TODO: Remove this shortcuts later
            IDS = new Dictionary<string, string>() {
                {"English", "en"},
                {"Russian", "ru"},
                {"Ukrainian", "ukr"},
                {"German", "de"},
                {"French", "fr"},
                {"Italian", "it"},
                {"Spanish", "es"},
                {"Portuguese", "pt"},
                {"Arabic", "ar"},
                {"Turkish", "tr"},
                {"Vietnamese", "vn"},
                {"Japanese", "jp"}
            };

            Default = Load("English");

            string current = Application.systemLanguage.ToString();

            SetCurrent(current);
        }

        private static string GetValue(string key, string defaultValue = null)
        {
            if (Strings.ContainsKey(key))
            {
                string ret = Strings[key];
                ret = ret.Replace("\\n", "\n");
                return ret;
            }


            if (defaultValue != null) return defaultValue;

            return Default.ContainsKey(key) ? Default[key] : key + "_" + CurrentId;
        }

        private static void SetCurrent(string locale)
        {
            _current = locale;

            CurrentId = IDS.ContainsKey(_current) ? IDS[_current] : "en";

            Strings = Load(_current);

            NotifyChanged();
        }

        internal static void NotifyChanged()
        {
            if (OnChanged != null) OnChanged.Invoke();
        }

        private static Dictionary<string, string> Load(string locale)
        {
            var result = new Dictionary<string, string>();

            var assets = Resources.LoadAll<TextAsset>(LocalizationPath + locale);
            Resources.UnloadUnusedAssets();

            foreach (var it in assets)
            {
                DoLoad(result, it.text);
            }

            return result;
        }

        private static void DoLoad(Dictionary<string, string> result, string data)
        {
            var root = JsonMapper.ToObject(data);
            if (root == null) return;

            foreach (JsonData it in (IList)root)
            {
                result[(string)it["id"]] = (string)it["value"];
            }
        }

        private static TextAsset GetData(string fileName)
        {
            var asset = (TextAsset)Resources.Load(fileName);
            Resources.UnloadUnusedAssets();

            return asset;
        }
    }
}

public static class LocalizationUtil
{
    public static string Tr(this string key, string defaultValue = null)
    {
        return Localization.Get(key, defaultValue);
    }

    public static string Tr(this string self, string defaultValue, params object[] args)
    {
        return string.Format(Localization.Get(self, defaultValue), args);
    }
}