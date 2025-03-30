using Hashira.Entities;
using Hashira.FSM;
using UnityEngine;

namespace Hashira.Bosses.States
{
    public class BossGroggyState : EntityState
    {
        private Boss _boss;
        private float _currentGroggyTime = 0f;

        public BossGroggyState(Entity entity, StateSO stateSO) : base(entity, stateSO)
        {
            _boss = entity as Boss;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            _currentGroggyTime = Time.time;
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            if (_currentGroggyTime + _boss.CurrentMaxGroggyTime < Time.time)
                _entityStateMachine.ChangeState("Idle");
        }

        public override void OnExit()
        {
            base.OnExit();
        }
    }
}
