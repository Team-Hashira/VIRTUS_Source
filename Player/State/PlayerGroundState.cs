using Hashira.Core.StatSystem;
using Hashira.Entities;
using Hashira.FSM;
using UnityEngine;

namespace Hashira.Players
{
    public class PlayerGroundState : EntityState
    {
        private readonly static int _JumpingAnimationHash = Animator.StringToHash("Jumping");
        protected PlayerMover _playerMover;
		protected EntityRenderer _entityRenderer;
		protected Player _player;

        public PlayerGroundState(Entity entity, StateSO stateSO) : base(entity, stateSO)
        {
            _player = entity as Player;
			_playerMover = entity.GetEntityComponent<PlayerMover>(true);
			_entityRenderer = entity.GetEntityComponent<EntityRenderer>();
		}

		public override void OnEnter()
        {
            base.OnEnter();

            _player.InputReader.OnJumpEvent += HandleJumpEvent;
            _player.InputReader.OnCrouchEvent += HandleCrouchEvent;
        }

        private void HandleDashEvent()
        {
            _entityStateMachine.ChangeState("Dash");
        }

        private void HandleCrouchEvent(bool isOn)
        {
            if (isOn)
                _entityStateMachine.ChangeState("Crouch");
            else
                _entityStateMachine.ChangeState("Idle");
        }

        protected virtual void HandleJumpEvent()
        {
            //SoundManager.Instance.PlaySFX("PlayerJump", _player.transform.position, 1f);
            _playerMover.Jump();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            if (_playerMover.IsGrounded == false)
            {
                _entityStateMachine.ChangeState("Air");
                _entityAnimator.SetParam(_JumpingAnimationHash);
            }
        }

        public override void OnExit()
        {
            base.OnExit();

            _player.InputReader.OnJumpEvent -= HandleJumpEvent;
            _player.InputReader.OnCrouchEvent -= HandleCrouchEvent;
            _player.InputReader.OnDashEvent -= HandleDashEvent;
        }
    }
}