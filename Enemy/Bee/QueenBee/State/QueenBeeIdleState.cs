using Crogen.CrogenPooling;
using Hashira.Core;
using Hashira.Enemies.Components;
using Hashira.Entities;
using Hashira.FSM;
using Hashira.Players;
using System.Net.NetworkInformation;
using UnityEngine;

namespace Hashira.Enemies.Bee.QueenBee
{
    public class QueenBeeIdleState : EntityState
    {
        private QueenBee _queenBee;

        private EntityRenderer _entityRenderer;
        private EnemyMover _enemyMover;
        private EnemyDetector _enemyDetector;

        private Player _target;

        private bool _isEvasioned;

        private float _spawnTimer = 0;
        private float _spawnDelay = 7f;

        public QueenBeeIdleState(Entity entity, StateSO stateSO) : base(entity, stateSO)
        {
            _queenBee = entity as QueenBee;

            _entityRenderer = entity.GetEntityComponent<EntityRenderer>();
            _enemyMover = entity.GetEntityComponent<EnemyMover>();
            _enemyDetector = entity.GetEntityComponent<EnemyDetector>();

            _target = PlayerManager.Instance.Player;
            _entityStateMachine.SetShareVariable("Target", _target);

            _isEvasioned = false;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            _enemyMover.StopImmediately();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            _entityRenderer.LookTarget(_target.transform.position);
            if (!_isEvasioned)
            {
                if (_enemyDetector.IsTargetOnAttackRange(_target.transform, _queenBee.EvasionRange))
                {
                    _entityStateMachine.ChangeState("Evasion");
                    _isEvasioned = true;
                }
            }
            _spawnTimer += Time.deltaTime;
            if(_spawnTimer >= _spawnDelay)
            {
                CommonBee.CommonBee bee = PopCore.Pop(_queenBee.CommonBee, _entity.transform.position, Quaternion.identity) as CommonBee.CommonBee;
                Vector2 dir = Random.insideUnitCircle.normalized;
                bee.GetEntityComponent<EnemyMover>().SetMovement(dir * 6);

                _spawnTimer = 0;
            }
        }
    }
}
