using Crogen.CrogenPooling;
using Hashira.Combat;
using Hashira.Core.DamageHandler;
using Hashira.Enemies;
using Hashira.Entities;
using Hashira.Items;
using Hashira.StageSystem;
using System.Linq;
using UnityEngine;

namespace Hashira.Accessories.Effects
{
    public class LifeRingPassive : AccessoryPassiveEffect
    {
        private EntityHealth _ownerHealth;

        private LifeRingHandler _lifeRingHandler;

        public LifeRingPassive()
        {
            _lifeRingHandler = new LifeRingHandler();
            _lifeRingHandler.OnHandlerCalledEvent += HandleOnHandlerCalledEvent;
        }

        public override void Initialize(Entity owner)
        {
            base.Initialize(owner);
            _ownerHealth = owner.GetEntityComponent<EntityHealth>();
            _ownerHealth.AddDamageHandler(EDamageHandlerLayer.First, _lifeRingHandler);
        }

        private void HandleOnHandlerCalledEvent()
        {
            PopCore.Pop(EffectPoolType.LifeRingPassiveEffect, _owner.transform.position, Quaternion.identity);
            Accessory.UnequipAccessory(EAccessoryType.Passive);
        }

        public override void OnUnequip()
        {
            _ownerHealth.RemoveDamageHandler(_lifeRingHandler);
        }
    }

    public class LifeRingActive : AccessoryActiveEffect
    {
        private EntityHealth _ownerHealth;

        [SerializeField]
        [Tooltip("데미지")]
        private int _damage = 10;
        [SerializeField]
        [Tooltip("회복량")]
        private int _recoverAmount = 1;
        [SerializeField]
        [Tooltip("게임 내에서 사용 가능한 횟수")]
        private int _useableCount = 2;
        private int _counter = 0;

        public override void Initialize(Entity owner)
        {
            base.Initialize(owner);
            _ownerHealth = owner.GetEntityComponent<EntityHealth>();
        }

        public override void OnActivate()
        {
            var enemies = StageGenerator.Instance.GetCurrentStage().GetEnabledEnemies();
            if (enemies != null && enemies.Length == 0)
                return;
            Enemy targetEnemy = enemies.OrderBy(enemy => (enemy.transform.position - _owner.transform.position).sqrMagnitude).First();
            targetEnemy.GetEntityComponent<EntityHealth>().ApplyDamage(new AttackInfo(_damage, attackType: EAttackType.Default), popUpText: true);
            PopCore.Pop(EffectPoolType.BeAbsorbEffect, targetEnemy.transform.position, Quaternion.identity);
            PopCore.Pop(EffectPoolType.AbsorbEffect, _owner.transform.position, Quaternion.identity);
            _ownerHealth.ApplyRecovery(_recoverAmount);

            _counter++;
            if (_counter >= _useableCount)
            {
                Accessory.UnequipAccessory(EAccessoryType.Active);
            }
        }

        public override void Reset()
        {
            _counter = 0;
        }
    }
}
