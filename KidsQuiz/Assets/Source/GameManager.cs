using AssetsCore;
using LitJson;
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
    [SerializeField] private GameObject _popupPref;

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

    private void OnApplicationQuit()
    {
        SaveManager.SaveSettings();
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
        var data = Resources.Load<TextAsset>("Localization").text;
        var root = JsonMapper.ToObject(data);
        LoadLocalization(root);

        InitializeMainMenu();
        InitializeSettings();
        InitializeAbackContainer();
        InitializePopup();
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

    #endregion
}