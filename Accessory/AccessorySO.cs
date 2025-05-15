using Hashira.Items;
using UnityEngine;

namespace Hashira.Accessories
{
    [CreateAssetMenu(fileName = "AccessorySO", menuName = "SO/Accessory/AccessorySO")]
    public class AccessorySO : ItemSO
    {
        [TextArea]
        public string passiveDescription, activeDescription;
        public override string Description { get; }

    }
}
