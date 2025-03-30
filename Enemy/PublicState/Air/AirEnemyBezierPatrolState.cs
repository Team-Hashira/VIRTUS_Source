using Hashira.Core.StatSystem;
using Hashira.Enemies.Components;
using Hashira.Entities;
using Hashira.FSM;
using Hashira.Players;
using System.Collections.Generic;
using UnityEngine;

namespace Hashira.Enemies.PublicStates
{
    public class AirEnemyBezierPatrolState : EntityState
    {
        private EnemyDetector _enemyDetector;
        private EnemyMover _enemyMover;

        private StatElement _speedElement;

        public List<Vector2> BezierPositionList { get; set; }
        private Vector2 _destination;
        private float _percent;

        public string TargetState { get; set; } = "Chase";


        public AirEnemyBezierPatrolState(Entity entity, StateSO stateSO) : base(entity, stateSO)
        {
            BezierPositionList = new List<Vector2>();

            _enemyDetector = entity.GetEntityComponent<EnemyDetector>();
            _enemyMover = entity.GetEntityComponent<EnemyMover>();
            var stat = entity.GetEntityComponent<EntityStat>();
            _speedElement = stat.StatDictionary[StatName.Speed];
        }

        public override void OnEnter()
        {
            base.OnEnter();
            _percent = 0;
            _destination = BezierPositionList[BezierPositionList.Count - 1];
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            Vector3 toMove = MathEx.Bezier(Mathf.Clamp01(_percent), BezierPositionList);
            Vector2 direction = toMove - _entity.transform.position;
            _enemyMover.SetMovement(direction.normalized * _speedElement.Value);
            _percent += Time.deltaTime;
            if (Vector2.Distance(_entity.transform.position, _destination) <= 0.3f)
            {
                _entityStateMachine.ChangeState("Idle");
            }
            Player player = _enemyDetector.DetectPlayer();
            if (player != null)
            {
                _entityStateMachine.SetShareVariable("Target", player);
                _entityStateMachine.ChangeState(TargetState);
            }
        }

        public override void OnExit()
        {
            base.OnExit();
            BezierPositionList.Reverse();
        }
    }
}
