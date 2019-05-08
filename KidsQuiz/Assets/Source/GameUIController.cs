using System;
using Source.Helper;
using UnityEngine;
using UnityEngine.UI;

namespace Source
{
    public class GameUIController : Singleton<GameUIController>, IDestroyableSingleton
    {
        [SerializeField] private Button _backButton;
        [SerializeField] private Button _checkButton;
        [SerializeField] private Text _answerMakeNumber;
        [SerializeField] private InputField _questionShowNumber;


        public bool CanDestroyed => true;

        public Action OnTrueAnswer;
        public Action OnFalseAnswer;

        private AbackController _abackController;


        public void Initialize()
        {
            _backButton.onClick.RemoveAllListeners();
            _backButton.onClick.AddListener(BackAction);

            _checkButton.onClick.RemoveAllListeners();
            _checkButton.onClick.AddListener(CheckNumberAction);

            _questionShowNumber.characterLimit = Configurations.ColumnCount;
            _abackController = FindObjectOfType<AbackController>();
            _abackController.gameObject.SetActive(true);
            DisaableCheckButton();

            _questionShowNumber.text = string.Empty;
            HideUI();
        }

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

        public void ShowAnswerMakeNumber(int answer)
        {
            _answerMakeNumber.gameObject.SetActive(true);
            _answerMakeNumber.text = "Покажи число " + answer;
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

        private void HideUI()
        {
            _answerMakeNumber.gameObject.SetActive(false);
            _questionShowNumber.gameObject.SetActive(false);
        }

        private void ShowAback()
        {
            _abackController.gameObject.SetActive(true);
        }

        private void EnableCheckButton()
        {
            _checkButton.enabled = true;
        }

        private void DisaableCheckButton()
        {
            _checkButton.enabled = false;
        }
    }
}