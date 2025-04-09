using System;
using UnityEngine;

namespace Hashira.Items
{
    [Serializable]
    public abstract class ItemEffect<T> where T : ItemEffect<T>
    {
        public ItemSO<T> ItemSO { get; private set; }
        public virtual void Initialize(ItemSO<T> itemSO)
        {
            ItemSO = itemSO;
        }
    }
}
