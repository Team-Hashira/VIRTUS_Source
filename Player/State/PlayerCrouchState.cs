using Hashira.Entities;
using Hashira.FSM;
using System.Collections;
using UnityEngine;

namespace Hashira.Players
{
    public class PlayerCrouchState : PlayerGroundState
    {
        public PlayerCrouchState(Entity entity, StateSO stateSO) : base(entity, stateSO)
        {

        }

        public override void OnEnter()
        {
            base.OnEnter();
			_playerMover.StopImmediately();
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        protected override void HandleJumpEvent()
        {
            _player.StartCoroutine(UnderJumpCoroutime());
        }

        private IEnumerator UnderJumpCoroutime()
        {
            _playerMover.UnderJump(true);
            yield return new WaitForSeconds(0.2f);
            _playerMover.UnderJump(false);
        }
    }
}
