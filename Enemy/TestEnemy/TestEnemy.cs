using Hashira.Entities;
using Hashira.Entities.Components;
using System;
using UnityEngine;

namespace Hashira.Enemies.TestEnemy
{
    public class TestEnemy : Enemy
    {
        protected override void AfterIntiialize()
        {
            base.AfterIntiialize();
            GetEntityComponent<EntityHealth>().OnDieEvent += HandleOnDieEvent;
        }

        private void HandleOnDieEvent(Entity _)
        {
            GetEntityComponent<EntityStateMachine>().ChangeState("Dead");
        }
    }
}
