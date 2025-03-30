using Hashira.Core.MoveSystem;
using Hashira.Enemies;
using Hashira.Enemies.Components;
using UnityEngine;

namespace Hashira.Enemies
{
    public class AirEnemy : Enemy
    {
        protected EnemyMover _enemyMover;
        protected override void AfterIntiialize()
        {
            base.AfterIntiialize();
            _enemyMover = GetEntityComponent<EnemyMover>();
            _enemyMover.SetGravity(false);
            _enemyMover.RemoveMoveProcessor<ApplyVelocityProcessor>();
            _enemyMover.AddMoveProcessor<XSmoothProcessor>();
            _enemyMover.AddMoveProcessor<YSmoothProcessor>();
        }
    }
}