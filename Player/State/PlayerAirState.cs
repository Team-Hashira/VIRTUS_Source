using Crogen.CrogenPooling;
using Hashira.Core.StatSystem;
using Hashira.Entities;
using Hashira.FSM;
using UnityEngine;

namespace Hashira.Players
{
    public class PlayerAirState : EntityState
    {
        private readonly static int _LandingAnimationHash = Animator.StringToHash("Landing");
        private StatElement _speedStat;
        protected EntityMover _entityMover;

		private Player _player;

        private bool _isJumped = false;

        public PlayerAirState(Entity entity, StateSO stateSO) : base(entity, stateSO)
        {
            _player = entity as Player;
            _entityMover = entity.GetEntityComponent<EntityMover>();
            _speedStat = entity.GetEntityComponent<EntityStat>().StatDictionary[StatName.Speed];
		}

		public override void OnEnter()
		{
			base.OnEnter();
			_player.InputReader.OnJumpEvent += HandleJumpEvent;
		}

		public override void OnUpdate()
        {
            base.OnUpdate();

            float movement = _player.InputReader.XMovement;
            if (_speedStat != null)
                movement *= _speedStat.Value;

            _entityMover.SetMovement(_player.transform.right * movement);

            if (_entityMover.IsGrounded == true)
            {
                ParticleSystem landingEffect = PopCore.Pop(EffectPoolType.LandingSmoke, _player.transform.position - _player.transform.up * 0.4f, Quaternion.identity).gameObject.GetComponent<ParticleSystem>();
                var mainModule = landingEffect.main;
                mainModule.startRotation = _player.transform.eulerAngles.z * Mathf.Deg2Rad;
                _entityStateMachine.ChangeState("Idle");
            }
        }

		public override void OnExit()
        {
            base.OnExit();
            _entityAnimator.SetParam(_LandingAnimationHash);
			_player.InputReader.OnJumpEvent -= HandleJumpEvent;
            _isJumped = false;
		}

		protected virtual void HandleJumpEvent()
		{
            if (_isJumped == true) return;
            _isJumped = true;
			_entityMover.Jump();
		}
	}
}