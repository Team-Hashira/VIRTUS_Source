using JetBrains.Annotations;
using System.Collections.Generic;
using UnityEngine;

namespace Hashira.CanvasUI
{
    public class HoverableDomain : UIManagementDomain
    {
        private IHoverableUI _hoveredUI;

        public override void UpdateUI()
        {
            base.UpdateUI();
            if (_hoveredUI == null)
            {
                if (UIManager.UIInteractor.IsUIUnderCursor(out GameObject uiObject))
                {
                    if (uiObject.TryGetComponent(out IHoverableUI hoverable))
                    {
                        _hoveredUI = hoverable;
                        _hoveredUI.OnCursorEnter();
                    }
                }
            }
            else
            {
                if (!UIManager.UIInteractor.IsUIUnderCursor(out GameObject uiObject)
                    || !uiObject.TryGetComponent(out IHoverableUI hoverable)
                    || _hoveredUI != hoverable)
                {
                    CallOnCursorExit();
                }
            }
        }
        private void CallOnCursorExit()
        {
            _hoveredUI.OnCursorExit();
            _hoveredUI = null;
        }
    }
}