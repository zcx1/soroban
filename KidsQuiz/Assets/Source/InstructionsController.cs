using UnityEngine;
using UnityEngine.UI;

namespace Source
{
    public class InstructionsController : MonoBehaviour
    {
        #region Inspector Fields

        [SerializeField] private Button _backButton;

        #endregion

        #region PublicMethods

        public void Initialize()
        {
            _backButton.onClick.AddListener(() => GameManager.Instance.GameState = EGameState.IN_MENU);
        }

        #endregion
    }
}