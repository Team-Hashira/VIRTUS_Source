using Hashira.Core.DamageHandler;
using Hashira.Entities;
using System;
using UnityEngine;

namespace Hashira.Accessories
{
    [Serializable]
    public class LifeRingAccessory : AccessoryEffect
    {
        public int height;
        private EntityHealth _ownerHealth;

        public EAccessoryType CurrentType { get; private set; } = EAccessoryType.None;
        private LifeRingHandler _lifeRingHandler;

        public override void Initialize(Entity owner)
        {
            base.Initialize(owner);
            _ownerHealth = owner.GetEntityComponent<EntityHealth>();
            _lifeRingHandler = new LifeRingHandler();
            _lifeRingHandler.OnHandlerCalledEvent += HandleOnHandlerCalledEvent;
        }

        private void HandleOnHandlerCalledEvent()
        {
            //여기서 부수는 코드. 일단 이거로 대체
            _ownerHealth.RemoveDamageHandler(_lifeRingHandler);
        }

        public override void OnAccessoryTypeChange(EAccessoryType accessoryType)
        {
            if (accessoryType == EAccessoryType.Passive)
            {
                _ownerHealth.AddDamageHandler(EDamageHandlerLayer.First, _lifeRingHandler);
            }
            else if(CurrentType == EAccessoryType.Passive)
            {
                _ownerHealth.RemoveDamageHandler(_lifeRingHandler);
            }
            CurrentType = accessoryType;
        }

        public override void ActiveSkill()
        {
            base.ActiveSkill();
            // 쪼옥(생명 흡수하는 소리)
        }
    }
}
