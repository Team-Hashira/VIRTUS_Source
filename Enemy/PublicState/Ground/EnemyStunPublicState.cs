using Hashira.Enemies.Components;
using Hashira.Entities;
using Hashira.FSM;
using UnityEngine;

namespace Hashira.Enemies.PublicStates
{
    //StunEffect에 의해서만 들어와야하며 StunEffect에 의해 나가집니다.
    public class EnemyStunPublicState : EntityState
    {
        private EnemyMover _enemyMover;

        public EnemyStunPublicState(Entity entity, StateSO stateSO) : base(entity, stateSO)
        {
            _enemyMover = entity.GetEntityComponent<EnemyMover>();
        }

        public override void OnEnter()
        {
            base.OnEnter();
            _enemyMover.StopImmediately();
        }
    }
}
