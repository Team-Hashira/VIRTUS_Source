using System;

namespace Hashira.Items
{
    [Serializable]
    public abstract class ItemEffect
    {
        public ItemSO ItemSO { get; private set; }
        public virtual void Initialize(ItemSO itemSO)
        {
            ItemSO = itemSO;
        }
    }
}
