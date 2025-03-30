using Hashira.Core.StatSystem;
using Hashira.Entities;
using Hashira.Entities.Components;
using Hashira.FSM;
using System;
using UnityEngine;

namespace Hashira.Players
{
    [Obsolete]
	public class PlayerRunState : PlayerGroundState
	{
		private StatElement _sprintSpeedStat;


		public PlayerRunState(Entity entity, StateSO stateSO) : base(entity, stateSO)
		{
            // 사용 안함(StatName.SprintSpeed)
            _sprintSpeedStat = entity.GetEntityComponent<EntityStat>().StatDictionary[StatName.SprintSpeed];
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
				if (_sprintSpeedStat != null)
					movement *= _sprintSpeedStat.Value;
				_playerMover.SetMovement(new Vector2(movement, 0));
			}
			else
				_entityStateMachine.ChangeState("Idle");

			if(!_playerMover.IsSprint || Mathf.Sign(movement) != Mathf.Sign(look))
				_entityStateMachine.ChangeState("Walk");
		}

		public override void OnExit()
		{
			base.OnExit();
		}
	}
}
