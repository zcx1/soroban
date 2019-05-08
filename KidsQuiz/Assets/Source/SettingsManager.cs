using System;
using UnityEngine;
using UnityEngine.UI;

namespace Source
{
    public class SettingsManager : MonoBehaviour
    {
        #region InspectorFields

        [SerializeField] private Button _backButton;

        [SerializeField] private Slider _columnSlider;
        [SerializeField] private Text _columnText;

        [SerializeField] private Slider _timeSlider;
        [SerializeField] private Text _timeText;
        [SerializeField] private Toggle _musicToggle;

        #endregion

        #region PrivateFields

        private int _columnCount;

        #endregion

        #region UnityMethods

        #endregion

        #region PublicMethods

        public void Initialize()
        {
            _columnText.text = Configurations.ColumnCount.ToString();
            _columnSlider.value = Configurations.ColumnCount;
            _columnSlider.onValueChanged.AddListener(value => ChangeColumnValue((int) value));


            _timeText.text = Configurations.ShowingTime.ToString();
            _timeSlider.value = Configurations.ShowingTime;
            _timeSlider.onValueChanged.AddListener(value => ChangeShowingTime(value));

            _musicToggle.isOn = Configurations.IsMusic;
            _musicToggle.onValueChanged.AddListener(value => ChangeMusic(value));

            _backButton.onClick.AddListener(BackButtonAction);
        }

        #endregion

        #region PrivateMethods

        private void ChangeColumnValue(int value)
        {
            _columnText.text = value.ToString();
            Configurations.ColumnCount = value;
        }

        private void ChangeShowingTime(float value)
        {
            _timeText.text = Math.Round(value,  1).ToString();
            Configurations.ShowingTime = (float) Math.Round(value, 1);
        }

        private void ChangeMusic(bool value)
        {
            Configurations.IsMusic = value;
        }

        private void BackButtonAction()
        {
            SaveManager.SaveSettings();
            GameManager.Instance.GameState = EGameState.IN_MENU;
        }

        #endregion
    }
}