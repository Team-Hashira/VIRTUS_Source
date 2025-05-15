using Hashira.Core.StatSystem;
using Hashira.Enemies.Components;
using Hashira.Entities;
using Hashira.Entities.Components;
using Hashira.FSM;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Hashira.Enemies.Goblin.DefaultGoblin
{
    public class DefaultGoblinAttackState : EntityState
    {
        private EnemyMover _enemyMover;
        private DamageCaster2D _damageCaster;

        private StatElement _damageElement;

        private bool _checkingDelay;
        private float _attackDelay;
        private float _delayTimer;

        public DefaultGoblinAttackState(Entity entity, StateSO stateSO) : base(entity, stateSO)
        {
            _damageCaster = (entity as DefaultGoblin).DamageCaster;
            _enemyMover = entity.GetEntityComponent<EnemyMover>();

            var stat = entity.GetEntityComponent<EntityStat>();
            _damageElement = stat.StatDictionary[StatName.AttackPower];
        }

        public override void OnEnter()
        {
            base.OnEnter();
            _checkingDelay = false;
            _attackDelay = Random.Range(0.3f, 1f);
            _delayTimer = 0;
            _enemyMover.StopImmediately();
            _entityAnimator.OnAnimationTriggeredEvent += HandleOnAnimationTriggeredEvent;
        }

        private void HandleOnAnimationTriggeredEvent(EAnimationTriggerType trigger, int count)
        {
            if (trigger == EAnimationTriggerType.Trigger)
            {
                SoundManager.Instance.PlaySFX("Swing", _entity.transform.position, 1f);
                _damageCaster.CastDamage(_entity.MakeAttackInfo(_damageElement.IntValue), popupText: false);
            }
            if (trigger == EAnimationTriggerType.End)
            {
                _checkingDelay = true;
            }
        }

        public override void OnExit()
        {
            _entityAnimator.OnAnimationTriggeredEvent -= HandleOnAnimationTriggeredEvent;
            base.OnExit();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            if (!_checkingDelay)
                return;
            _delayTimer += Time.deltaTime;
            if(_delayTimer >= _attackDelay)
            {
                _entityStateMachine.ChangeState("Chase");
            }
        }
    }
}
