using Hashira.Core.StatSystem;
using Hashira.Enemies.Components;
using Hashira.Entities;
using Hashira.FSM;
using Hashira.Players;
using UnityEngine;

namespace Hashira.Enemies.PublicStates
{
    public class EnemyPatrolPublicState : EntityState
    {
        private int _facingDirection;

        private Enemy _enemy;
        private EnemyDetector _enemyDetector;
        private EnemyMover _enemyMover;
        private EntityStat _entityStat;
        private EntityRenderer _entityRenderer;

        private StatElement _speedElement;

        public string TargetState { get; set; } = "Chase";


        public EnemyPatrolPublicState(Entity entity, StateSO stateSO) : base(entity, stateSO)
        {
            _enemy = entity as Enemy;
            _enemyDetector = entity.GetEntityComponent<EnemyDetector>();
            _enemyMover = entity.GetEntityComponent<EnemyMover>();
            _entityStat = entity.GetEntityComponent<EntityStat>();
            _entityRenderer = entity.GetEntityComponent<EntityRenderer>();

            _speedElement = _entityStat.StatDictionary[StatName.Speed];
            _facingDirection = 1;
            _facingDirection *= Random.Range(-2, 2) >= 0 ? 1 : -1;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            _facingDirection *= -1;
            _entityRenderer.LookTarget(_facingDirection);
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            Vector3 moveDir = new Vector3(_facingDirection * _speedElement.Value, 0);
            _enemyMover.SetMovement(_entity.transform.rotation * moveDir);
            Player player = _entityStateMachine.GetShareVariable<Player>("Target");
            if (player != null)
            {
                _entityStateMachine.ChangeState(TargetState);
                return;
            }
            else
            {
                player = _enemyDetector.DetectPlayer();
                if (player != null)
                {
                    _entityStateMachine.SetShareVariable("Target", player);
                    _entityStateMachine.ChangeState(TargetState);
                    return;
                }
            }
            if (_enemyMover.IsWallOnFront())
            {
                //Debug.Log("벽 발견");
                //if(_enemyMover.IsWallJumpable())
                //{
                //    _enemyMover.Jump();
                //}
                //else
                {
                    _entityStateMachine.ChangeState("Idle");
                }
            }
            if (_enemyMover.IsOnEdge())
            {
                _entityStateMachine.ChangeState("Idle");
            }
        }
    }
}
