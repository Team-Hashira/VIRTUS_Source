using Hashira.Items;
using UnityEngine;

namespace Hashira.Accessories
{
    [CreateAssetMenu(fileName = "AccessorySO", menuName = "SO/AccessorySO")]
    public class AccessorySO : ItemSO
    {
        [field: TextArea]
        [field: SerializeField]
        public override string Description { get; set; }
    }
}
