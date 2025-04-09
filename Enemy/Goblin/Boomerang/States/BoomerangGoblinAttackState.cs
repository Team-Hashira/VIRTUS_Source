using Crogen.CrogenPooling;
using Hashira.Core.StatSystem;
using Hashira.Enemies.Components;
using Hashira.Entities;
using Hashira.Entities.Components;
using Hashira.FSM;
using Hashira.Players;
using Hashira.Visualizers;
using System;
using UnityEngine;

namespace Hashira.Enemies.Goblin.BoomerangGoblin
{
    public class BoomerangGoblinAttackState : EntityState
    {
        private BoomerangGoblin _boomerangGoblin;
        private EnemyMover _enemyMover;
        private EntityHealth _entityHealth;
        private ProjectilePoolType _boomerang;

        private GoblinBoomerang _currentBoomerang;
        private Player _target;

        private StatElement _damageElement;

        private Vector3 _targetPosition;
        private Vector3 _direction;

        private readonly int _recallTriggerHash = Animator.StringToHash("Recall");

        private BoxVisualizer _visualizer;


        public BoomerangGoblinAttackState(Entity entity, StateSO stateSO) : base(entity, stateSO)
        {
            _boomerangGoblin = (entity as BoomerangGoblin);
            _boomerang = _boomerangGoblin.Boomerang;
            _enemyMover = entity.GetEntityComponent<EnemyMover>();
            _entityHealth = entity.GetEntityComponent<EntityHealth>();

            var stat = entity.GetEntityComponent<EntityStat>();
            _damageElement = stat.StatDictionary[StatName.AttackPower];
        }

        public override void OnEnter()
        {
            base.OnEnter();
            if (!_enemyMover.IsGrounded)
            {
                _entityStateMachine.ChangeState("Chase");
                return;
            }

            _enemyMover.StopImmediately();
            _target = _entityStateMachine.GetShareVariable<Player>("Target");
            _targetPosition = _target.transform.position;
            _direction = _target.transform.position - _entity.transform.position;
            _direction.Normalize();
            _visualizer = PopCore.Pop(WorldUIPoolType.BoxVisualizer) as BoxVisualizer;
            _visualizer.Visualize(_entity.transform.position, _targetPosition + _direction, 1f, 1f, 0.1f, MathEx.OutCubic);

            _entityAnimator.OnAnimationTriggeredEvent += HandleOnAnimationTriggeredEvent;
            _entityHealth.OnDieEvent += HandleOnDieEvent;
        }

        private void HandleOnDieEvent(Entity entity)
        {
            if(_visualizer != null && _visualizer.gameObject.activeSelf) 
                _visualizer.Fold(0.06f);
        }

        private void HandleOnAnimationTriggeredEvent(EAnimationTriggerType trigger, int count)
        {
            if (trigger == EAnimationTriggerType.Trigger)
            {
                if (count == 1)
                {
                    _visualizer.Blink(0.05f, 0.3f);
                }
                if (count == 2)
                {
                    SoundManager.Instance.PlaySFX("BoomerangSwing", _entity.transform.position, 1f);
                    _currentBoomerang = PopCore.Pop(_boomerang, _entity.transform.position, Quaternion.identity) as GoblinBoomerang;
                    _currentBoomerang.Initialize(_boomerangGoblin, _targetPosition + _direction);
                    _currentBoomerang.OnReturnEvent += HandleOnReturnEvent;
                }
            }
            if (trigger == EAnimationTriggerType.End)
            {
                _entityStateMachine.DelayedChangeState("Chase");
            }
        }

        private void HandleOnReturnEvent()
        {
            _entityAnimator.SetParam(_recallTriggerHash);
            _currentBoomerang.OnReturnEvent -= HandleOnReturnEvent;
        }

        public override void OnExit()
        {
            _entityAnimator.OnAnimationTriggeredEvent -= HandleOnAnimationTriggeredEvent;
            _entityHealth.OnDieEvent -= HandleOnDieEvent;
            base.OnExit();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
        }
    }
}
