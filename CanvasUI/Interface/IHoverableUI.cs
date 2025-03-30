using System;
using UnityEngine;

namespace Hashira.CanvasUI
{
    public interface IHoverableUI : IUserInterface
    {
        public void OnCursorEnter();
        public void OnCursorExit();
    }
}
