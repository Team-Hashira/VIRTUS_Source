using UnityEngine;

namespace Hashira.GimmickSystem
{
    public abstract class GimmickSO : ScriptableObject
    {
        public abstract void OnGimmick(IGimmickObject owner);
    }
}