using Hashira.Entities;
using Hashira.FSM;

namespace Hashira.Players
{
    public class PlayerIdleState : PlayerGroundState
    {
		public PlayerIdleState(Entity entity, StateSO stateSO) : base(entity, stateSO)
        {

		}

        public override void OnEnter()
        {
            base.OnEnter();
            _playerMover.StopImmediately();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            if (_player.InputReader.XMovement != 0)
            {
                _entityStateMachine.ChangeState("Walk");
            }
        }
    }
}