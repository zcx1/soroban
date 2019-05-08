using UnityEngine;
using UnityEngine.UI;

namespace Source
{
    public class MainMenuController : MonoBehaviour
    {
        [SerializeField] private Button _showNumberButton;
        [SerializeField] private Button _makeNumberButton;
        [SerializeField] private Button _settings;
        [SerializeField] private Button _instruction;

        private void Awake()
        {
            InitializeUI();
        }

        private void InitializeUI()
        {
            _showNumberButton.onClick.AddListener(ShowNumberAction);
            _makeNumberButton.onClick.AddListener(MakeNumberAction);
            _settings.onClick.AddListener(SettingsAction);
        }

        private void ShowNumberAction()
        {
            GameManager.Instance.GameState = EGameState.IN_SHOW_NUMBER;
        }

        private void MakeNumberAction()
        {
            GameManager.Instance.GameState = EGameState.IN_MAKE_NUMBER;
        }

        private void SettingsAction()
        {
            GameManager.Instance.GameState = EGameState.IN_SETTINGS;
        }
    }
}