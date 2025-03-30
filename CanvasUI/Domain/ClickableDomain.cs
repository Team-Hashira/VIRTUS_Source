using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Hashira.CanvasUI
{
    public class ClickableDomain : UIManagementDomain
    {
        private IClickableUI _clickedUI;

        public override void UpdateUI()
        {
            base.UpdateUI();
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                if (UIManager.UIInteractor.IsUIUnderCursor(out GameObject uiObject))
                {
                    if (uiObject.TryGetComponent(out IClickableUI clickable))
                    {
                        _clickedUI = clickable;
                        _clickedUI.OnClick(true);
                    }
                }
            }
            else if (Mouse.current.leftButton.wasReleasedThisFrame)
            {
                if (_clickedUI == null)
                    return;
                if (UIManager.UIInteractor.IsUIUnderCursor(out GameObject uiObject))
                {
                    if (uiObject.TryGetComponent(out IClickableUI clickable))
                    {
                        if (clickable == _clickedUI)
                            _clickedUI.OnClickEnd(true);
                    }
                }
                _clickedUI = null;
            }


            if (Mouse.current.rightButton.wasPressedThisFrame)
            {
                if (UIManager.UIInteractor.IsUIUnderCursor(out GameObject uiObject))
                {
                    if (uiObject.TryGetComponent(out IClickableUI clickable))
                    {
                        _clickedUI = clickable;
                        _clickedUI.OnClick(false);
                    }
                }
            }
            else if (Mouse.current.rightButton.wasReleasedThisFrame)
            {
                if (_clickedUI == null)
                    return;
                if (UIManager.UIInteractor.IsUIUnderCursor(out GameObject uiObject))
                {
                    if (uiObject.TryGetComponent(out IClickableUI clickable))
                    {
                        if (clickable == _clickedUI)
                            _clickedUI.OnClickEnd(false);
                    }
                }
                _clickedUI = null;
            }
        }
    }
}
