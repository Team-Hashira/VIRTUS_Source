using Hashira.Core.StatSystem;
using Hashira.Entities;
using Hashira.Entities.Components;
using Hashira.FSM;
using UnityEngine;

namespace Hashira.Players
{
    public class PlayerRollingState : EntityState
    {
        private StatElement _dashSpeedStat;
        private PlayerMover _playerMover;
        private EntityHealth _entityHealth;
        private EntityRenderer _entityRenderer;
        private Player _player;

        private float _moveDir;

        public PlayerRollingState(Entity entity, StateSO stateSO) : base(entity, stateSO)
        {
            _player = entity as Player;
            _playerMover = entity.GetEntityComponent<PlayerMover>(true);
            _entityRenderer = entity.GetEntityComponent<EntityRenderer>();
            _entityHealth = entity.GetEntityComponent<EntityHealth>();
            _dashSpeedStat = entity.GetEntityComponent<EntityStat>().StatDictionary[StatName.DashSpeed];
        }

        public override void OnEnter()
        {
            base.OnEnter();
            _moveDir = _entityRenderer.FacingDirection;

            var mainModule = _player.AfterImageParticle.main;
            mainModule.startRotationZ = -_moveDir * _player.transform.eulerAngles.z * Mathf.Deg2Rad;
            mainModule.startRotationY = (-90 + 90 * _moveDir) * Mathf.Deg2Rad;
            _player.AfterImageParticle.Play();

            _entityAnimator.OnAnimationTriggeredEvent += HandleAnimationTriggerEvent;
            _entityRenderer.SetArmActive(false);

            _entityHealth.ModifyEvasion(true);
        }

        private void HandleAnimationTriggerEvent(EAnimationTriggerType type, int count)
        {
            if (type == EAnimationTriggerType.End)
            {
                if (_player.InputReader.XMovement != 0)
                    _entityStateMachine.ChangeState("Walk");
                else
                    _entityStateMachine.ChangeState("Idle");
            }
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            _playerMover.SetMovement(_player.transform.right * _moveDir * _dashSpeedStat.Value);
            _entityRenderer.LookTarget(_player.transform.position + _player.transform.right * _moveDir);
        }

        public override void OnExit()
        {
            base.OnExit();
            _entityAnimator.OnAnimationTriggeredEvent -= HandleAnimationTriggerEvent;
            _entityRenderer.SetArmActive(true);
            _entityHealth.ModifyEvasion(false);
        }
    }
}