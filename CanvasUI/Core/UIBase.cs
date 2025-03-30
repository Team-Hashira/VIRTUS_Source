using UnityEngine;

namespace Hashira.CanvasUI
{
    public abstract class UIBase : MonoBehaviour
    {
        public RectTransform RectTransform => transform as RectTransform;

        protected virtual void Awake()
        {
            UIManager.Instance.AddUI(this);
        }
    }
}
