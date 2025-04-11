using Hashira.Core.StatSystem;
using Hashira.Entities;
using Hashira.Entities.Components;
using Hashira.FSM;
using UnityEngine;

namespace Hashira.Players
{
    public class PlayerWalkState : PlayerGroundState
    {
		private StatElement _speedStat;

		public PlayerWalkState(Entity entity, StateSO stateSO) : base(entity, stateSO)
        {
			_playerMover = entity.GetEntityComponent<PlayerMover>();
			_speedStat = entity.GetEntityComponent<EntityStat>().StatDictionary[StatName.Speed];
			_entityRenderer = entity.GetEntityComponent<EntityRenderer>();
		}

		public override void OnEnter()
        {
            base.OnEnter();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            float movement = _player.InputReader.XMovement;
			float look = _entityRenderer.FacingDirection;

			if (movement != 0)
            {
                if (_speedStat != null)
                    movement *= _speedStat.Value;
                _playerMover.SetMovement(_player.transform.right * movement);

			}
            else
                _entityStateMachine.ChangeState("Idle");

		}

        public override void OnExit()
        {
            base.OnExit();
        }
    }
}