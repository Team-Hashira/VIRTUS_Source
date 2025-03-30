using Hashira.Core.StatSystem;
using Hashira.Enemies.Components;
using Hashira.Entities;
using Hashira.Entities.Components;
using Hashira.FSM;
using System;
using UnityEngine;

namespace Hashira.Enemies.Goblin.DefaultGoblin
{
    public class DefaultGoblinAttackState : EntityState
    {
        private EnemyMover _enemyMover;
        private DamageCaster2D _damageCaster;

        private StatElement _damageElement;

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
            _enemyMover.StopImmediately();
            _entityAnimator.OnAnimationTriggeredEvent += HandleOnAnimationTriggeredEvent;
        }

        private void HandleOnAnimationTriggeredEvent(EAnimationTriggerType trigger, int count)
        {
            if (trigger == EAnimationTriggerType.Trigger)
            {
                _damageCaster.CastDamage(_damageElement.IntValue, false);
            }
            if (trigger == EAnimationTriggerType.End)
            {
                _entityStateMachine.DelayedChangeState("Chase");
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
        }
    }
}
