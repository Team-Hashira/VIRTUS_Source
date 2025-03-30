using Hashira.Entities;
using Hashira.FSM;

namespace Hashira.Bosses.States
{
    public class BossDeadState : EntityState
    {
        public BossDeadState(Entity entity, StateSO stateSO) : base(entity, stateSO)
        {
        }
    }
}
