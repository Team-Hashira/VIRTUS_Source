using Crogen.CrogenPooling;
using UnityEngine;

namespace Hashira
{
    public class PopupTextManager : MonoSingleton<PopupTextManager>
    {
        [SerializeField] private RectTransform _popupTextTrm;

        public void PopupText(string text, Color color)
        {
            PopupText popupText = PopCore.Pop(UIPoolType.PopupText, _popupTextTrm) as PopupText;
            popupText.transform.localScale = Vector3.one;
            popupText.Init(text, color);
        }
    }
}
