using Hashira.Entities;
using Hashira.Items;
using System;
using UnityEngine;

namespace Hashira.Accessories.Effects
{
    public class AccessoryEffector : ItemEffect
    {
        protected Entity _owner;

        public AccessorySO AccessorySO { get; private set; }
        public EAccessoryType CurrentType { get; protected set; } = EAccessoryType.None;

        public bool IsApplied { get; private set; }

        public AccessoryEffect CurrentEffect { get; private set; }

        [SerializeReference]
        protected AccessoryPassiveEffect _accessoryPassiveEffect;
        [SerializeReference]
        protected AccessoryActiveEffect _accessoryActiveEffect;

        /// <summary>
        /// 객체 생성 시점에 실행되는 함수. 게임 시작할때 한번에 만들기 때문에 진짜 초기값이 필요한 경우만 사용
        /// </summary>
        /// <param name="itemSO"></param>
        public override void Initialize(ItemSO itemSO)
        {
            base.Initialize(itemSO);
            AccessorySO = itemSO as AccessorySO;
            Type passiveType = Type.GetType($"{GetType().Namespace}.{itemSO.name}Passive");
            Type activeType = Type.GetType($"{GetType().Namespace}.{itemSO.name}Active");
            _accessoryPassiveEffect = Activator.CreateInstance(passiveType) as AccessoryPassiveEffect;
            _accessoryActiveEffect = Activator.CreateInstance(activeType) as AccessoryActiveEffect;
        }

        /// <summary>
        /// Accessory.ApplyAll을 호출 했을때 호출 되는 함수. Player가 Initialize될 시점에 호출 됨.
        /// </summary>
        /// <param name="owner">아마 플레이어일거임</param>
        public virtual void OnApply(Entity owner, EAccessoryType accessoryType)
        {
            _owner = owner;
            IsApplied = true;
            if (accessoryType == EAccessoryType.Passive)
                CurrentEffect = _accessoryPassiveEffect;
            else if (accessoryType == EAccessoryType.Active)
                CurrentEffect = _accessoryActiveEffect;
            CurrentEffect.Initialize(owner);
        }

        /// <summary>
        /// 게임이 끝났을때 완전한 객체 초기화를 위해
        /// </summary>
        public virtual void Reset()
        {
            _accessoryPassiveEffect.Reset();
            _accessoryActiveEffect.Reset();
        }

        public virtual void OnUnequip()
        {
            IsApplied = false;
            CurrentEffect.OnUnequip();
        }

        /// <summary>
        /// Accessory를 가지고 있는 주체에서 Update 사이클에 맞춰 실행하는 함수입니다.
        /// </summary>
        public virtual void PassiveSkill()
        {
            _accessoryPassiveEffect.OnUpdate();
        }

        /// <summary>
        /// Accessory를 가지고 있는 주체에서 Active 스킬 키를 눌렀을떄 실행하는 함수입니다.
        /// </summary>
        public virtual void ActiveSkill()
        {
            _accessoryActiveEffect.OnActivate();
        }
    }
}
