using System.Collections.Generic;
using UnityEngine;

namespace Hashira.CanvasUI
{
    public abstract class UIManagementDomain
    {
        protected HashSet<IUserInterface> _uiList;

        public UIManagementDomain()
        {
            _uiList = new HashSet<IUserInterface>();
        }

        public virtual void UpdateUI() { }

        public virtual void AddUI(IUserInterface uiInterface)
        {
            if (!_uiList.Contains(uiInterface))
                _uiList.Add(uiInterface);
        }

        public virtual void RemoveUI(IUserInterface uiInterface)
        {
            _uiList.Remove(uiInterface);
        }
    }
}
