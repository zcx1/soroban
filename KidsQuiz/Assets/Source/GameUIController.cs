using System;
using Source.Helpers;
using UnityEngine;
using UnityEngine.UI;

namespace Source
{
    public class GameUIController : Singleton<GameUIController>, IDestroyableSingleton
    {
        #region InspectorFields

        [SerializeField] private Button _backButton;
        [SerializeField] private Button _checkButton;
        [SerializeField] private Text _answerMakeNumber;
        [SerializeField] private InputField _questionShowNumber;

        #endregion

        #region Properties

        public bool CanDestroyed => true;

        #endregion

        #region PublicFields

        public Action OnTrueAnswer;
        public Action OnFalseAnswer;

        #endregion

        #region PrivateFields

        private AbackController _abackController;
        private string _defaultMakeNumberText = string.Empty;

        #endregion

        #region PublicMethods

        public void Initialize()
        {
            _backButton.onClick.RemoveAllListeners();
            _backButton.onClick.AddListener(BackAction);

            _checkButton.onClick.RemoveAllListeners();
            _checkButton.onClick.AddListener(CheckNumberAction);

            _questionShowNumber.characterLimit = Configurations.ColumnCount;
            _abackController = FindObjectOfType<AbackController>();
            _abackController.gameObject.SetActive(true);
            DisableCheckButton();

            if (_defaultMakeNumberText == string.Empty)
            {
                _defaultMakeNumberText = _answerMakeNumber.text;
            }

            _questionShowNumber.text = string.Empty;
            HideUI();
        }

        public void ShowAnswerMakeNumber(int answer)
        {
            _answerMakeNumber.gameObject.SetActive(true);
            _answerMakeNumber.text = _defaultMakeNumberText + " " + answer;
            _abackController.gameObject.SetActive(false);
            Invoke("HideUI", Configurations.ShowingTime);
            Invoke("ShowAback", Configurations.ShowingTime);
            Invoke("EnableCheckButton", Configurations.ShowingTime);
        }

        public void ShowQuestionShowNumber()
        {
            EnableCheckButton();
            _questionShowNumber.gameObject.SetActive(true);
            _abackController.gameObject.SetActive(false);
        }

        public void ShowAback()
        {
            _abackController.gameObject.SetActive(true);
        }

        public void HideAback()
        {
            _abackController.gameObject.SetActive(false);
        }


        public void HideUI()
        {
            _answerMakeNumber.gameObject.SetActive(false);
            _questionShowNumber.gameObject.SetActive(false);
        }

        public void DisableCheckButton()
        {
            _checkButton.enabled = false;
        }

        #endregion

        #region PrivateMethods

        private void BackAction()
        {
            GameManager.Instance.GameState = EGameState.IN_MENU;
            CancelInvoke("ShowAback");
            CancelInvoke("EnableCheckButton");
            CancelInvoke("HideUI");
        }

        private void CheckNumberAction()
        {
            bool isTrueAnw = false;
            switch (GameManager.Instance.GameState)
            {
                case EGameState.IN_MAKE_NUMBER:
                {
                    isTrueAnw = _abackController.CheckAnswer();
                    break;
                }
                case EGameState.IN_SHOW_NUMBER:
                {
                    try
                    {
                        var answ = Convert.ToInt32(_questionShowNumber.text);
                        isTrueAnw = _abackController.CheckAnswer(answ);
                    }
                    catch (Exception ex)
                    {
                        Debug.Log(ex);
                    }

                    break;
                }
            }

            _questionShowNumber.text = string.Empty;
            if (isTrueAnw)
            {
                OnTrueAnswer.Invoke();
                return;
            }

            OnFalseAnswer.Invoke();
        }


        private void EnableCheckButton()
        {
            _checkButton.enabled = true;
        }

        #endregion
    }
}