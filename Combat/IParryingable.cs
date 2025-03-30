using UnityEngine;

namespace Hashira.Combat
{
    public interface IParryingable
    {
        public Transform Owner { get; set; }
        public bool IsParryingable { get; set; }
        public void Parrying(LayerMask whatIsNewTargetLayer, Transform owner, bool isChargedParrying);
    }
}
