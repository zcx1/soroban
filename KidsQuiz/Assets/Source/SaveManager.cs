using UnityEngine;

namespace Source
{
    public class SaveManager
    {
        private const string COLUMN_COUNT_KEY = "column_count";
        private const string SHOWING_TIME_KEY = "showing_time";
        private const string IS_PLAY_NUSIC_KEY = "is_play_music";

        public static void SaveSettings()
        {
            PlayerPrefs.SetInt(COLUMN_COUNT_KEY, Configurations.ColumnCount);
            PlayerPrefs.SetFloat(SHOWING_TIME_KEY, Configurations.ShowingTime);
            PlayerPrefs.SetInt(IS_PLAY_NUSIC_KEY, Configurations.IsMusic ? 1 : 0);
            PlayerPrefs.Save();
        }

        public static void LoadSettings()
        {
            var columnCount = PlayerPrefs.GetInt(COLUMN_COUNT_KEY);
            if (columnCount == 0)
            {
                Configurations.ColumnCount = 1;
                Configurations.ShowingTime = 3f;
                Configurations.IsMusic = true;
                return;
            }

            Configurations.ColumnCount = columnCount;
            Configurations.ShowingTime = PlayerPrefs.GetFloat(SHOWING_TIME_KEY);
            Configurations.IsMusic = PlayerPrefs.GetInt(IS_PLAY_NUSIC_KEY) == 1;
        }
    }
}