using Hashira.Entities;
using Hashira.FSM;
using UnityEngine;

namespace Hashira.Enemies.Scarecrow
{
    public class DummyIdleState : EntityState
    {
        public DummyIdleState(Entity entity, StateSO stateSO) : base(entity, stateSO)
        {
        }
    }
}
