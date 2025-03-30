using Hashira.Enemies;
using Hashira.Entities;
using System;
using UnityEngine;

namespace Hashira.Tutorials
{
    public class EnemyDeadCheckStep : TutorialStep
    {
        [SerializeField]
        private EntityHealth[] _entityHealths;

        private int _count;

        public override void OnEnter()
        {
            base.OnEnter();
            _count = _entityHealths.Length;
            foreach (var health in _entityHealths)
                health.OnDieEvent += HandleOnDieEvent;
        }

        private void HandleOnDieEvent(Entity entity)
        {
            entity.GetEntityComponent<EntityHealth>().OnDieEvent -= HandleOnDieEvent;
            _count--;
            if(_count <= 0)
                _tutorialManager.NextStep();
        }

        public override void OnExit()
        {
            base.OnExit();
        }
    }
}
