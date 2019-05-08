using UnityEngine;
using UnityEngine.UI;

namespace Source
{
    public class PopupController : MonoBehaviour
    {
        #region InspectorFields

        [SerializeField] private GameObject _successPopUp;
        [SerializeField] private GameObject _failurePopUp;
        [SerializeField] private Button _continueBtn;
        [SerializeField] private Button _tryAgainBtn;

        #endregion
        
        #region PublicMethods

        public void Initialize()
        {
            _continueBtn.onClick.AddListener(SuccessAction);
            _tryAgainBtn.onClick.AddListener(FailureAction);
            HideFailure();
            HideSuccess();
        }

        public void ShowSuccess()
        {
            _successPopUp.SetActive(true);
        }

        public void HideSuccess()
        {
            _successPopUp.SetActive(false);
        }

        public void ShowFailure()
        {
            _failurePopUp.SetActive(true);
        }

        public void HideFailure()
        {
            _failurePopUp.SetActive(false);
        }

        #endregion

        #region PrivateMethods

        private void SuccessAction()
        {
            GameManager.Instance.RestartAback();
            HideSuccess();
        }

        private void FailureAction()
        {
            HideFailure();
        }

        #endregion
    }
}