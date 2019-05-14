using System;
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
        [SerializeField] private Animator _failureAnimator;
        [SerializeField] private Animator _successAnimator;

        #endregion

        #region PublicFields

        public Action OnFailureClick;
        public Action OnSuccessClick;

        #endregion

        #region PublicMethods

        public void Initialize()
        {
            _continueBtn.onClick.AddListener(OnSuccessClick.Invoke);
            _tryAgainBtn.onClick.AddListener(OnFailureClick.Invoke);
            HideFailure();
            HideSuccess();
        }

        public void ShowSuccess()
        {
            _successPopUp.SetActive(true);
            _successAnimator.SetTrigger("PopupShow");
        }

        public void HideSuccess()
        {
            _successPopUp.SetActive(false);
        }

        public void ShowFailure()
        {
            _failurePopUp.SetActive(true);
            _failureAnimator.SetTrigger("PopupShow");
        }

        public void HideFailure()
        {
            _failurePopUp.SetActive(false);
        }

        #endregion
    }
}