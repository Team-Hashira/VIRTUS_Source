using Doryu.CustomAttributes;
using UnityEngine;

namespace Hashira.StageSystem
{
    [CreateAssetMenu(menuName = "SO/Level/Floor")]
    public class FloorSO : ScriptableObject
    {
        [VisibleInspectorSO]
        public StageTypeListSO[] stages;
        
        public int Length => stages.Length;
        public StageTypeListSO this[int index] => stages[index];
    }
}
