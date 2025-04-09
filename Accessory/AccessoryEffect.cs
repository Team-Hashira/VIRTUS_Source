using Hashira.Entities;
using Hashira.Entities.Components;
using Hashira.Items;
using System;
using UnityEngine;

namespace Hashira.Accessories
{
    public abstract class AccessoryEffect : ItemEffect<AccessoryEffect>
    {
        protected Entity _owner;

        public AccessorySO AccessorySO { get; private set; }

        public override void Initialize(ItemSO<AccessoryEffect> itemSO)
        {
            base.Initialize(itemSO);
            AccessorySO = itemSO as AccessorySO;
        }
           
        public virtual void Initialize(Entity owner) 
        {
            _owner = owner;
        }

        public virtual void OnAccessoryTypeChange(EAccessoryType accessoryType)
        {

        }

        /// <summary>
        /// Accessory를 가지고 있는 주체에서 Update 사이클에 맞춰 실행하는 함수입니다.
        /// </summary>
        public virtual void PassiveSkill() { }

        /// <summary>
        /// Accessory를 가지고 있는 주체에서 Active 스킬 키를 눌렀을떄 실행하는 함수입니다.
        /// </summary>
        public virtual void ActiveSkill() { }
    }
}
