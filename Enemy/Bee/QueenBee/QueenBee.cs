using Crogen.CrogenPooling;
using Hashira.Core.MoveSystem;
using Hashira.Core.StatSystem;
using Hashira.Entities;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Hashira.Enemies.Bee.QueenBee
{
    public class QueenBee : AirEnemy
    {
        [field: SerializeField]
        public EntityPoolType CommonBee { get; private set; }

        [field: SerializeField]
        public float EvasionRange { get; private set; }

        [SerializeField]
        private List<Entity> _beeList;

        protected override void AfterIntiialize()
        {
            base.AfterIntiialize();
            EntityHealth.OnDieEvent += HandleOnDieEvent;

            _enemyMover.GetMoveProcessor<XYSmoothProcessor>().Speed = 6f;
            _enemyMover.GetMoveProcessor<XYSmoothProcessor>().OnlyOut = true;
        }

        private void HandleOnDieEvent(Entity entity)
        {
            for (int i = 0; i < _beeList.Count; i++)
            {
                var stat = entity.GetEntityComponent<EntityStat>();
                stat.StatDictionary[StatName.Speed].AddModify("QueenBee", 50f, EModifyMode.Percent, EModifyLayer.Default);
                stat.StatDictionary[StatName.DashSpeed].AddModify("QueenBee", 50f, EModifyMode.Percent, EModifyLayer.Default);
            }
        }

        public void AddBee(Entity entity)
        {
            _beeList.Add(entity);
            entity.GetEntityComponent<EntityHealth>().OnDieEvent += HandleOnCommonBeeDieEvent;
        }

        private void HandleOnCommonBeeDieEvent(Entity entity)
        {
            _beeList.Remove(entity);
        }
    }
}
