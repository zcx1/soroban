using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AssetsCore
{
    [ExecuteInEditMode]
    public class LocalizeText : MonoBehaviour
    {

        public string Key;
        public bool UseList = true;

        void Awake()
        {
            if (string.IsNullOrEmpty(Key)) Key = name;
            
            UpdateText();
            
            Localization.OnChanged += UpdateText;
        }

        void OnDestroy()
        {
            Localization.OnChanged -= UpdateText;
        }

        public void UpdateText(string key)
        {
            Key = key;

            UpdateText();
        }

        public void UpdateText()
        {
            if (!Localization.HasKey(Key)) return;

            // локализация UI.Text
            {
                var text = GetComponent<Text>();
                if (text) text.text = Key.Tr();
            }

            // локализация TextMesh
            {
                var text = GetComponent<TextMesh>();
                if (text) text.text = Key.Tr();
            }

			// локализация TextMeshProUGUI
			{
				var text = GetComponent<TextMeshProUGUI>();
				if (text) text.text = Key.Tr();
			}
		}
    }
}