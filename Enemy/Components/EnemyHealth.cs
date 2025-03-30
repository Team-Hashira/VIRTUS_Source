using Hashira.Core;
using Hashira.EffectSystem.Effects;
using Hashira.Entities;
using Hashira.Entities.Components;
using System;
using UnityEngine;

namespace Hashira.Enemies.Components
{
    public class EnemyHealth : EntityHealth
    {
        private EntityEffector _entityEffector;

        [SerializeField]
        private float _stunTime = 0.5f;
        [SerializeField]
        private float _stunThresholdPercent = 60f;
        private float _stunThresholdValue; // Percent에 따라 초기화 단계에서 미리 계산됨.

        public override void AfterInit()
        {
            base.AfterInit();
            _entityEffector = Owner.GetEntityComponent<EntityEffector>();
            _stunThresholdValue = (MaxHealth / 100f) * _stunThresholdPercent;

            OnHealthChangedEvent += HandleOnHealthChange;
        }

        private void HandleOnHealthChange(int previous, int current)
        {
            if (_stunThresholdValue > current)
            {
                if(Owner.TryGetEntityComponent<EntityStateMachine>(out var stateMachine))
                {
                    stateMachine.SetShareVariable("Target", PlayerManager.Instance.Player);
                }
                Stun stunEffect = new Stun();
                stunEffect.Setup(_stunTime);
                _entityEffector.AddEffect(stunEffect);
                OnHealthChangedEvent -= HandleOnHealthChange;
            }
        }
    }
}
