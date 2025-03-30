using UnityEngine;

namespace Hashira
{
    public enum EPopupUIName
    {
        ItemDataUI,
        TitleOption,

    }

    public interface IPopupUI
    {
        public EPopupUIName PopupUIName {  get; }
        public void Show();
        public void Hide();
    }
}
