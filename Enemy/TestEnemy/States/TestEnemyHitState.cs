using Hashira.Entities;
using Hashira.FSM;
using UnityEngine;

namespace Hashira.Enemies.TestEnemy
{
    public class TestEnemyHitState : EntityState
    {
        private EntityHealth _entityHealth;

        public TestEnemyHitState(Entity entity, StateSO stateSO) : base(entity, stateSO)
        {
            _entityHealth = entity.GetEntityComponent<EntityHealth>();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            if (!_entityHealth.IsKnockback)
                _entityStateMachine.ChangeState("Idle");
        }
    }
}
