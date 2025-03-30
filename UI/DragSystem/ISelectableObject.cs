using UnityEngine;

namespace Hashira.UI.DragSystem
{
    public interface ISelectableObject
    {
        public void OnSelectStart();
        public void OnSelectEnd();
    }
}
