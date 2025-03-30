#if UNITY_EDTIOR
using UnityEngine;

namespace Hashira.LatestUI
{
    public class TestUI : UIBase, IHoverableUI, IClickableUI, IToggleUI
    {
        [field:SerializeField]
        public Collider2D Collider { get; set; }
        [field:SerializeField]
        public string Key { get; set; }

        public void Open()
        {
            Debug.Log("Open");
        }

        public void Close()
        {
            Debug.Log("Close");
        }

        public void OnClick()
        {
            Debug.Log("OnClick");
        }

        public void OnClickEnd()
        {
            Debug.Log("OnClickEnd");
        }

        public void OnCursorEnter()
        {
            Debug.Log("OnCursorEnter");
        }

        public void OnCursorExit()
        {
            Debug.Log("OnCursorExit");
        }
    }
}
#endif