using System;
using UnityEngine;

namespace Hashira.CanvasUI
{
    public delegate void OnClickEvent();

    public interface IClickableUI : IUserInterface
    {
        public void OnClick(bool isLeft);
        public void OnClickEnd(bool isLeft);
    }
}
