using Crogen.CrogenPooling;
using Hashira.Core.MoveSystem;
using Hashira.Core.StatSystem;
using Hashira.EffectSystem.Effects;
using Hashira.Enemies.Components;
using Hashira.Entities;
using Hashira.Entities.Components;
using Hashira.FSM;
using Hashira.Players;
using Hashira.Visualizers;
using UnityEngine;

namespace Hashira.Enemies.Bee.CommonBee
{
    public class CommonBeeAttackState : EntityState
    {
        private CommonBee _bee;
        private EnemyMover _enemyMover;
        private EntityEffector _entityEffector;

        private StatElement _attackPowerElement;
        private StatElement _dashSpeedElement;

        private Player _target;

        private BoxVisualizer _visualizer;

        private Vector2 _direction;
        private Vector2 _destination;
        private float _distanceThresholdSqr = 1f;

        private bool _isDashStarted = false;

        private readonly int _setupEndHash = Animator.StringToHash("SetupEnd");


        public CommonBeeAttackState(Entity entity, StateSO stateSO) : base(entity, stateSO)
        {
            _bee = entity as CommonBee;
            _enemyMover = entity.GetEntityComponent<EnemyMover>();
            _entityEffector = entity.GetEntityComponent<EntityEffector>();

            var stat = _entity.GetEntityComponent<EntityStat>();
            _dashSpeedElement = stat.StatDictionary[StatName.DashSpeed];
            _attackPowerElement = stat.StatDictionary[StatName.AttackPower];
        }

        public override void OnEnter()
        {
            base.OnEnter();
            _enemyMover.SetActiveMoveProcessor<ApplyVelocityProcessor>(true);
            _enemyMover.SetActiveMoveProcessor<XSmoothProcessor>(false);
            _enemyMover.SetActiveMoveProcessor<YSmoothProcessor>(false);

            _target = _entityStateMachine.GetShareVariable<Player>("Target");

            _direction = _target.transform.position - _entity.transform.position;
            _direction.Normalize();

            _destination = (Vector2)_target.transform.position + _direction * 2f;

            _entityAnimator.OnAnimationTriggeredEvent += HandleOnAnimationTriggeredEvent;

            _enemyMover.StopImmediately(true);

            _visualizer = PopCore.Pop(WorldUIPoolType.BoxVisualizer) as BoxVisualizer;
            _visualizer.Visualize(_entity.transform.position, _destination, 1.3f, -1f, 0.2f, MathEx.OutCubic);
        }

        private void HandleOnAnimationTriggeredEvent(EAnimationTriggerType triggerType, int count)
        {
            if(triggerType == EAnimationTriggerType.End)
            {
                _entityAnimator.SetParam(_setupEndHash, true);
            }
            if (triggerType == EAnimationTriggerType.Trigger)
            {
                _visualizer.Blink(0, 0.1f, OnComplete: () => _visualizer.Fold(0.1f));
                _enemyMover.SetMovement(_direction * _dashSpeedElement.Value);
                _isDashStarted = true;
            }
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            if (_isDashStarted)
            {
                _bee.DamageCaster.CastDamage(_attackPowerElement.IntValue, false);
            }
            float distSqr = (_entity.transform.position - (Vector3)_destination).sqrMagnitude;
            if (distSqr < _distanceThresholdSqr)
            {
                var stun = new Stun();
                stun.Setup(1f);
                _entityEffector.AddEffect(stun);
                _isDashStarted = false;
            }
        }

        public override void OnExit()
        {
            base.OnExit();
            _entityAnimator.OnAnimationTriggeredEvent -= HandleOnAnimationTriggeredEvent;
            _entityAnimator.SetParam(_setupEndHash, false);
            _enemyMover.SetActiveMoveProcessor<ApplyVelocityProcessor>(false);
            _enemyMover.SetActiveMoveProcessor<XSmoothProcessor>(true);
            _enemyMover.SetActiveMoveProcessor<YSmoothProcessor>(true);
            _enemyMover.StopImmediately(true);
        }
    }
}
