using UnityEngine;
using UnityEngine.UI;

namespace Source
{
    public class Bone : MonoBehaviour
    {
        [SerializeField] private float _anchorMinX;
        [SerializeField] private float _anchorMinY;
        [SerializeField] private float _anchorMaxX;
        [SerializeField] private float _anchorMaxY;

        [SerializeField] private RectTransform _rectTransform;

        public Vector2 MinAnchor => new Vector2(_anchorMinX, _anchorMinY);
        public Vector2 MaxAnchor => new Vector2(_anchorMaxX, _anchorMaxY);

        public Vector2 DefaultMinAnchor { get; set; }
        public Vector2 DefaultMaxAnchor { get; set; }
        public bool IsInteractive { get; set; }

        private void Awake()
        {
            DefaultMinAnchor = _rectTransform.anchorMin;
            DefaultMaxAnchor = _rectTransform.anchorMax;
            InitializeUI();
        }

        public void InitializeUI()
        {
            GetComponent<Button>().onClick.AddListener(Move);
        }

        private void Move()
        {
            if (!IsInteractive)
                return;
            if (CheckDefaultPosition)
            {
                SetActivePosition();
            }
            else
            {
                SetDefaultPosition();
            }
        }

        public void SetActivePosition()
        {
            _rectTransform.anchorMin = MinAnchor;
            _rectTransform.anchorMax = MaxAnchor;
            _rectTransform.offsetMax = Vector2.zero;
            _rectTransform.offsetMin = Vector2.zero;
        }

        public void SetDefaultPosition()
        {
            _rectTransform.anchorMin = DefaultMinAnchor;
            _rectTransform.anchorMax = DefaultMaxAnchor;
            _rectTransform.offsetMax = Vector2.zero;
            _rectTransform.offsetMin = Vector2.zero;
        }

        public bool CheckDefaultPosition => _rectTransform.anchorMin == DefaultMinAnchor &&
                                            _rectTransform.anchorMax == DefaultMaxAnchor;

        public bool CheckActivePosition =>
            _rectTransform.anchorMin == MinAnchor && _rectTransform.anchorMax == MaxAnchor;
    }
}