using UnityEngine;

namespace Hashira.CanvasUI
{
    public abstract class UIBase : MonoBehaviour
    {
        private RectTransform _rectTransform;
        public RectTransform RectTransform
        {
            get
            {
                if (_rectTransform == null)
                {
                    _rectTransform = transform as RectTransform;
                }
                return _rectTransform;
            }
        }

        protected virtual void Awake()
        {
            UIManager.Instance.AddUI(this);
        }
    }
}
