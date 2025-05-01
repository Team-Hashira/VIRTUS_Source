using Hashira.Core.StatSystem;
using Hashira.Enemies.Components;
using Hashira.Entities;
using Hashira.Entities.Components;
using Hashira.FSM;
using System;
using UnityEngine;

namespace Hashira.Enemies.Slime
{
    public class SlimeDashAttackState : EntityState
    {
        private Slime _slime;

        private EntityRenderer _entityRenderer;
        private EnemyMover _enemyMover;

        private StatElement _attackPowerElement;
        private StatElement _dashSpeedElement;

        private readonly int _setupEndTriggerHash = Animator.StringToHash("SetupEnd");
        private bool _isDashTriggered = false;

        public SlimeDashAttackState(Entity entity, StateSO stateSO) : base(entity, stateSO)
        {
            _slime = entity as Slime;

            _entityRenderer = entity.GetEntityComponent<EntityRenderer>();
            _enemyMover = entity.GetEntityComponent<EnemyMover>();
            var stat = entity.GetEntityComponent<EntityStat>();
            _attackPowerElement = stat.StatDictionary[StatName.AttackPower];
            _dashSpeedElement = stat.StatDictionary[StatName.DashSpeed];
        }

        public override void OnEnter()
        {
            base.OnEnter();

            _isDashTriggered = false;

            _entityAnimator.OnAnimationTriggeredEvent += HandleOnAnimationTriggeredEvent;
            _enemyMover.StopImmediately();
        }

        private void HandleOnAnimationTriggeredEvent(EAnimationTriggerType triggerType, int count)
        {
            if (triggerType == EAnimationTriggerType.Trigger)
            {
                _entityAnimator.SetParam(_setupEndTriggerHash);
                _isDashTriggered = true;
            }
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            if (_isDashTriggered)
            {
                _enemyMover.SetMovement(new Vector2(_entityRenderer.FacingDirection * _dashSpeedElement.Value, 0));
                _slime.DamageCaster.CastDamage(_attackPowerElement.IntValue);
            }
            if (_enemyMover.IsWallOnFront())
            {
                _entityStateMachine.ChangeState("Idle");
            }
        }

        public override void OnExit()
        {
            _entityAnimator.OnAnimationTriggeredEvent -= HandleOnAnimationTriggeredEvent;
            base.OnExit();
        }
    }
}
