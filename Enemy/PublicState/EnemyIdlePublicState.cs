using Hashira.Enemies.Components;
using Hashira.Entities;
using Hashira.FSM;
using Hashira.Players;
using UnityEngine;

namespace Hashira.Enemies.PublicStates
{
    public class EnemyIdlePublicState : EntityState
    {
        private Enemy _enemy;
        private EnemyDetector _enemyDectector;
        private EnemyMover _enemyMover;

        private float _idleTimer = 0;
        private float _additionalTime = 0;
        public float IdleTime { get; set; }
        public string TargetState { get; set; } = "Patrol";

        public EnemyIdlePublicState(Entity entity, StateSO stateSO) : base(entity, stateSO)
        {
            _enemy = entity as Enemy;
            _enemyDectector = entity.GetEntityComponent<EnemyDetector>();
            _enemyMover = entity.GetEntityComponent<EnemyMover>();

            IdleTime = 2f;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            _enemyMover.StopImmediately();
            _additionalTime = Random.Range(0, 0.5f);
            _idleTimer = 0;
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            Player player = _enemyDectector.DetectPlayer();
            if (player != null)
            {
                _entityStateMachine.SetShareVariable("Target", player);
                _entityStateMachine.ChangeState(TargetState);
                return;
            }
            _idleTimer += Time.deltaTime;
            if(_idleTimer + _additionalTime > IdleTime)
            {
                _entityStateMachine.ChangeState(TargetState);
            }
        }
    }
}
