using Crogen.CrogenPooling;
using Hashira.Combat;
using Hashira.Core.DamageHandler;
using Hashira.Enemies;
using Hashira.Entities;
using Hashira.StageSystem;
using System.Linq;
using UnityEngine;

namespace Hashira.Accessories.Effects
{
    public class LifeRingAccessory : AccessoryEffect
    {
        private EntityHealth _ownerHealth;

        private LifeRingHandler _lifeRingHandler;

        [Header("Active Skill Setting")]
        [SerializeField]
        private int _damage = 10;
        [SerializeField]
        private int _recoverAmount = 1;
        [SerializeField]
        private int _useableCount = 2;
        private int _counter = 0;

        public override void Initialize(Entity owner)
        {
            base.Initialize(owner);
            _ownerHealth = owner.GetEntityComponent<EntityHealth>();
            _lifeRingHandler = new LifeRingHandler();
            _lifeRingHandler.OnHandlerCalledEvent += HandleOnHandlerCalledEvent;
        }

        private void HandleOnHandlerCalledEvent()
        {
            PopCore.Pop(EffectPoolType.LifeRingPassiveEffect, _owner.transform.position, Quaternion.identity);
            Accessory.UnequipAccessory(EAccessoryType.Passive, true);
        }

        public override void OnAccessoryTypeChange(EAccessoryType accessoryType)
        {
            if (accessoryType == EAccessoryType.Passive)
            {
                _ownerHealth.AddDamageHandler(EDamageHandlerLayer.First, _lifeRingHandler);
            }
            else if (CurrentType == EAccessoryType.Passive)
            {
                _ownerHealth.RemoveDamageHandler(_lifeRingHandler);
            }
            base.OnAccessoryTypeChange(accessoryType);
        }

        public override void ActiveSkill()
        {
            var enemies = StageGenerator.Instance.GetCurrentStage().GetEnabledEnemies();
            if (enemies.Length == 0)
                return;
            Enemy targetEnemy = enemies.OrderBy(enemy => (enemy.transform.position - _owner.transform.position).sqrMagnitude).First();
            targetEnemy.GetEntityComponent<EntityHealth>().ApplyDamage(new AttackInfo(_damage, attackType: EAttackType.Default), popUpText: true);
            PopCore.Pop(EffectPoolType.BeAbsorbEffect, targetEnemy.transform.position, Quaternion.identity);
            PopCore.Pop(EffectPoolType.AbsorbEffect, _owner.transform.position, Quaternion.identity);
            _ownerHealth.ApplyRecovery(_recoverAmount);

            _counter++;
            if (_counter >= _useableCount)
            {
                Accessory.UnequipAccessory(EAccessoryType.Active, true);
            }
        }

        public override void Reset()
        {
            _counter = 0;
        }
    }
}
