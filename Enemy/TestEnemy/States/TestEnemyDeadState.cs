using Crogen.CrogenPooling;
using Hashira.Entities;
using Hashira.Entities.Components;
using Hashira.FSM;
using System;
using UnityEngine;

namespace Hashira.Enemies.TestEnemy
{
    public class TestEnemyDeadState : EntityState
    {
        public TestEnemyDeadState(Entity entity, StateSO stateSO) : base(entity, stateSO)
        {
            _entityAnimator.OnAnimationTriggeredEvent += HandleOnTrigger;
        }

        private void HandleOnTrigger(EAnimationTriggerType type, int count)
        {
            if (type == EAnimationTriggerType.End)
                GameObject.Destroy(_entity.gameObject);
        }
    }
}
