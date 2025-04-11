using Hashira.CanvasUI;
using System;
using UnityEngine;

namespace Hashira
{
    public class LockModeButton : MonoBehaviour
    {
        [SerializeField] private CustomButton _lockModeButton;
        [SerializeField] private UseableCardDrawer _useableCardDrawer;

        private void Awake()
        {
            _lockModeButton.OnClickEvent += HandleLockModeEvent;
        }

        private void HandleLockModeEvent()
        {
            if (_useableCardDrawer.IsLockMode)
            {
                _lockModeButton.isUseLight = true;
                _useableCardDrawer.ActiveLockMode(false);
            }
            else
            {
                _lockModeButton.isUseLight = false;
                _useableCardDrawer.ActiveLockMode(true);
            }
        }
    }
}
