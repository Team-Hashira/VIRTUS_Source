using Hashira.Enemies.Components;
using Hashira.Entities;
using Hashira.FSM;
using UnityEngine;

namespace Hashira.Enemies.TestEnemy
{
    public class TestEnemyIdleState : EntityState
    {
        private float _idleTime = 2f;
        private float _idleTimer = 0;

        private EnemyMover _enemyMover;

        public TestEnemyIdleState(Entity entity, StateSO stateSO) : base(entity, stateSO)
        {
            _enemyMover = entity.GetEntityComponent<EnemyMover>();
        }

        public override void OnEnter()
        {
            base.OnEnter();
            _enemyMover.StopImmediately();
            _idleTimer = 0;
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            _idleTimer += Time.deltaTime;
            if(_idleTimer >= _idleTime)
            {
                _entityStateMachine.ChangeState("Patrol");
            }
        }
    }
}
