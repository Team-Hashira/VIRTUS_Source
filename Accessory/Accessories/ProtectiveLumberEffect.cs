using Crogen.CrogenPooling;
using Hashira.Bosses;
using Hashira.Combat;
using Hashira.Core.DamageHandler;
using Hashira.EffectSystem;
using Hashira.EffectSystem.Effects;
using Hashira.Enemies;
using Hashira.Entities;
using System;
using UnityEngine;

namespace Hashira.Accessories.Effects
{
    public class ProtectiveLumberPassive : AccessoryPassiveEffect, IUpdatableEffect
    {
        private EntityHealth _ownerHealth;

        private ShieldHandler _shieldHandler;

        [SerializeField]
        [Tooltip("활성화 될때 까지의 딜레이")]
        private float _beforeActivateDelay = 15;
        private float _activeTimer;

        private bool _isActive = false;

        private IPoolingObject _protectedLumberShield;

        public ProtectiveLumberPassive()
        {
            _shieldHandler = new ShieldHandler(1, true);
            _shieldHandler.SetOrderInLayer(-500);

            _shieldHandler.OnBreakEvent += HandleOnBreak;
        }

        private void HandleOnBreak(AttackInfo info)
        {
            BreakShield();
        }

        public void BreakShield()
        {
            PopCore.Pop(EffectPoolType.LumberShieldBrokeVFX, _owner.transform.position, Quaternion.identity);
            SetActiveShield(false);
            _activeTimer = 0;
        }

        public override void Initialize(Entity owner)
        {
            base.Initialize(owner);
            _ownerHealth = owner.GetEntityComponent<EntityHealth>();
            _ownerHealth.OnHealthChangedEvent += HandleOnHealthChanged;

            _protectedLumberShield = PopCore.Pop(EffectPoolType.ProtectiveLumberVFX, owner.transform);
            _protectedLumberShield.gameObject.SetActive(false);

            _activeTimer = 0;
            _isActive = false;
        }

        private void HandleOnHealthChanged(int previous, int current)
        {
            if(current < previous)
            {
                _activeTimer = 0;
            }
        }

        public void OnUpdate()
        {
            if (_isActive)
                return;
            _activeTimer += Time.deltaTime;
            if (_activeTimer >= _beforeActivateDelay)
            {
                SetActiveShield(true);
            }
        }

        public void SetActiveShield(bool isActive)
        {
            _isActive = isActive;
            _protectedLumberShield.gameObject.SetActive(isActive);
            if (isActive)
                _ownerHealth.AddDamageHandler(EDamageHandlerLayer.First, _shieldHandler);
        }
    }

    public class ProtectiveLumberActive : AccessoryActiveEffect, IInitializeOnNextStage
    {
        private EntityEffector _ownerEffector;

        private ShieldHandler _shieldHandler;
        private AddDamageHandlerEffect _addDamageHandler;

        [Tooltip("보스의 공격을 막았을때 보스가 그로기 되는 시간")]
        [SerializeField]
        private float _bossGroggyDuration = 3f;
        [Tooltip("쉴드가 유지되는 시간")]
        [SerializeField]
        private float _shieldDuration = 1f;
        [Tooltip("스테이지마다 사용가능한 횟수")]
        [SerializeField]
        private int _useableCount = 1;
        private int _currentCount;

        private IPoolingObject _protectedLumberShield;

        public ProtectiveLumberActive()
        {
            _shieldHandler = new ShieldHandler(1, true);
            _shieldHandler.SetOrderInLayer(-500);

            _addDamageHandler = new AddDamageHandlerEffect();
            _addDamageHandler.Setup(_shieldDuration, _shieldHandler, EDamageHandlerLayer.First);
            _addDamageHandler.OnTimeOutEvent += HandleOnTimeOut;
            _shieldHandler.OnBreakEvent += HandleOnBreak;

            _currentCount = _useableCount;
        }

        private void HandleOnBreak(AttackInfo info)
        {
            if (info.attacker is Boss boss)
            {
                boss.OnGroggy(_bossGroggyDuration);
            }
            else if (info.attacker is Enemy enemy)
            {
                var health = enemy.GetEntityComponent<EntityHealth>();
                health.ApplyDamage(_owner.MakeAttackInfo(health.Health));
            }
            BreakShield();
        }

        private void HandleOnTimeOut(Effect effect)
        {
            BreakShield();
        }

        public override void Initialize(Entity owner)
        {
            base.Initialize(owner);
            _protectedLumberShield = PopCore.Pop(EffectPoolType.ProtectiveLumberVFX, owner.transform);
            _protectedLumberShield.gameObject.SetActive(false);
            _ownerEffector = owner.GetEntityComponent<EntityEffector>();
        }

        public override void OnActivate()
        {
            if (_currentCount > 0)
            {
                _currentCount--;
                _ownerEffector.AddEffect(_addDamageHandler);
                _protectedLumberShield.gameObject.SetActive(true);
            }
        }

        public void OnNextStage()
        {
            _currentCount = _useableCount;
            _shieldHandler.Reset();
            BreakShield();
        }

        public void BreakShield()
        {
            PopCore.Pop(EffectPoolType.LumberShieldBrokeVFX, _owner.transform.position, Quaternion.identity);
            _protectedLumberShield.gameObject.SetActive(false);
        }
    }
}
