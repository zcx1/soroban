using Source;
using Source.Helper;
using UnityEngine;

public class GameManager : Singleton<GameManager>, IDestroyableSingleton
{
    #region InspectorFields

    [SerializeField] private GameObject _mainMenuPref;
    [SerializeField] private GameObject _abackContainerPref;
    [SerializeField] private GameObject _abackPref;
    [SerializeField] private GameObject _settingsPref;

    #endregion

    #region PrivateFields

    private MainMenuController _mainMenuController;
    private AbackController _abackController;
    private GameObject _abackContainerGO;
    private GameObject _abackGO;
    private EGameState _gameState;
    private SettingsManager _settingsManager;
    private GameUIController _gameUICOntroller;

    #endregion

    #region Properties

    public bool CanDestroyed => false;

    public EGameState GameState
    {
        get => _gameState;
        set
        {
            _gameState = value;
            switch (value)
            {
                case EGameState.IN_MENU:
                {
                    _mainMenuController.gameObject.SetActive(true);
                    _settingsManager.gameObject.SetActive(false);
                    HideAback();
                    break;
                }

                case EGameState.IN_SHOW_NUMBER:
                {
                    _mainMenuController.gameObject.SetActive(false);
                    ShowAback();
                    break;
                }

                case EGameState.IN_MAKE_NUMBER:
                {
                    _mainMenuController.gameObject.SetActive(false);
                    ShowAback();
                    break;
                }
                case EGameState.IN_SETTINGS:
                {
                    _mainMenuController.gameObject.SetActive(false);
                    _settingsManager.gameObject.SetActive(true);
                    _settingsManager.Initialize();
                    break;
                }
            }
        }
    }

    #endregion

    #region UnityMethods

    private void Start()
    {
        Initialize();
        GameState = EGameState.IN_MENU;
    }

    #endregion

    #region PublicMethods

    public void RestartAback()
    {
        HideAback();
        ShowAback();
    }

    #endregion

    #region PrivateMethods

    private void Initialize()
    {
        SaveManager.LoadSettings();
        InitializeMainMenu();
        InitializeSettings();
        InitializeAbackContainer();
    }


    private void InitializeMainMenu()
    {
        var mainMenuGO = Instantiate(_mainMenuPref);
        _mainMenuController = mainMenuGO.GetComponent<MainMenuController>();
    }

    private void InitializeSettings()
    {
        var settingsGO = Instantiate(_settingsPref);
        _settingsManager = settingsGO.GetComponent<SettingsManager>();
    }

    private void InitializeAbackContainer()
    {
        _abackContainerGO = Instantiate(_abackContainerPref);
        _gameUICOntroller = _abackContainerGO.GetComponent<GameUIController>();
    }

    private void ShowAback()
    {
        _abackContainerGO.SetActive(true);

        _abackGO = Instantiate(_abackPref, AbackContainer.Instance.AbackSpawnPoint);
        _abackController = _abackGO.GetComponent<AbackController>();
        _abackController.ColumnCount = Configurations.ColumnCount;

        _gameUICOntroller.Initialize();
        _abackController.Initialize();
    }

    private void HideAback()
    {
        _abackContainerGO.SetActive(false);
        Destroy(_abackGO);
    }

    #endregion
}