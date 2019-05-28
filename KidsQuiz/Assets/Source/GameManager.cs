using LitJson;
using Source;
using Source.Helpers;
using UnityEngine;
using UnityEngine.Android;

public class GameManager : Singleton<GameManager>, IDestroyableSingleton
{
    #region InspectorFields

    [SerializeField] private GameObject _mainMenuPref;
    [SerializeField] private GameObject _abackContainerPref;
    [SerializeField] private GameObject _abackPref;
    [SerializeField] private GameObject _settingsPref;
    [SerializeField] private GameObject _popupPref;
    [SerializeField] private GameObject _instructionPrefs;

    #endregion

    #region PrivateFields

    private MainMenuController _mainMenuController;
    private PopupController _popupController;
    private AbackController _abackController;
    private GameObject _abackContainerGO;
    private GameObject _abackGO;
    private EGameState _gameState;
    private SettingsManager _settingsManager;
    private GameUIController _gameUIController;
    private InstructionsController _instructionsController;
    private int _winCounter = -1;

    #endregion

    #region Properties

    public bool CanDestroyed => false;

    public EGameState GameState
    {
        get => _gameState;
        set
        {
            BackgroundController.Instance.ChangeBackground();
            _gameState = value;
            switch (value)
            {
                case EGameState.IN_MENU:
                {
                    _mainMenuController.gameObject.SetActive(true);
                    _settingsManager.gameObject.SetActive(false);
                    _instructionsController.gameObject.SetActive(false);
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
                case EGameState.IN_INSTRUCTIONS:
                {
                    _mainMenuController.gameObject.SetActive(false);
                    _instructionsController.gameObject.SetActive(true);
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

    private void OnApplicationQuit()
    {
        SaveManager.SaveSettings();
    }

    #endregion

    #region PrivateMethods

    private void Initialize()
    {
        InitializePermissions();
        InitializeAds();
        SaveManager.LoadSettings();
        var data = Resources.Load<TextAsset>("Localization").text;
        var root = JsonMapper.ToObject(data);
        LoadLocalization(root);

        InitializeMainMenu();
        InitializeSettings();
        InitializeAbackContainer();
        InitializeInstructions();
        InitializePopup();
        AudioManager.Instance.Initialize();
    }

    private void InitializeAds()
    {
        AdsManager.Instance.Initialize();
    }

    private void LoadLocalization(JsonData root)
    {
        foreach (JsonData data in root["localization"])
        {
            var id = data["id"].ToString();

            if (id != Localization.CurrentId)
            {
                continue;
            }

            foreach (var key in data["value"].Keys)
            {
                Localization.Set(key, data["value"][key].ToString());
            }
        }
    }

    private void InitializePermissions()
    {
        if (Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
        }
        else
        {
            Permission.RequestUserPermission(Permission.FineLocation);
        }
    }

    private void InitializeMainMenu()
    {
        var mainMenuGO = Instantiate(_mainMenuPref);
        _mainMenuController = mainMenuGO.GetComponent<MainMenuController>();
    }

    private void InitializePopup()
    {
        var popupGO = Instantiate(_popupPref);
        _popupController = popupGO.GetComponent<PopupController>();
        _popupController.OnSuccessClick += OnSuccessPopupClick;
        _popupController.OnFailureClick += OnFailurePupupClick;
        _popupController.Initialize();
    }

    private void InitializeInstructions()
    {
        var instructionsGO = Instantiate(_instructionPrefs);
        _instructionsController = instructionsGO.GetComponent<InstructionsController>();
        _instructionsController.Initialize();
    }

    private void OnFailurePupupClick()
    {
        switch (GameState)
        {
            case EGameState.IN_SHOW_NUMBER:
            {
                _gameUIController.HideUI();
                _gameUIController.ShowAback();
                break;
            }

            case EGameState.IN_MAKE_NUMBER:
            {
                _gameUIController.HideAback();
                break;
            }
        }

        _gameUIController.DisableCheckButton();
        _abackController.ShowAnswer();
        _popupController.HideFailure();
    }

    private void OnSuccessPopupClick()
    {
        RestartAback();
        _popupController.HideSuccess();
    }

    private void InitializeSettings()
    {
        var settingsGO = Instantiate(_settingsPref);
        _settingsManager = settingsGO.GetComponent<SettingsManager>();
    }

    private void InitializeAbackContainer()
    {
        _abackContainerGO = Instantiate(_abackContainerPref);
        _gameUIController = _abackContainerGO.GetComponent<GameUIController>();

        _gameUIController.OnTrueAnswer += OnTrueAnswer;
        _gameUIController.OnFalseAnswer += OnFalseAnswer;
    }

    private void OnFalseAnswer()
    {
        _popupController.ShowFailure();
    }

    private void OnTrueAnswer()
    {
        _popupController.ShowSuccess();
    }

    private void ShowAback()
    {
        _winCounter++;
        if (_winCounter == 3)
        {
            AdsManager.Instance.ShowInterstitial();
            _winCounter = -1;
        }

        _abackContainerGO.SetActive(true);

        _abackGO = Instantiate(_abackPref, AbackContainer.Instance.AbackSpawnPoint);
        _abackController = _abackGO.GetComponent<AbackController>();
        _abackController.ColumnCount = Configurations.ColumnCount;

        _gameUIController.Initialize();
        _abackController.Initialize();
    }

    private void HideAback()
    {
        _abackContainerGO.SetActive(false);
        Destroy(_abackGO);
    }

    private void RestartAback()
    {
        HideAback();
        ShowAback();
    }

    #endregion
}