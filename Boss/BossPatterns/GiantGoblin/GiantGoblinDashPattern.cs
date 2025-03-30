using Hashira.Bosses.BillboardClasses;
using UnityEngine;

namespace Hashira.Bosses.Patterns
{
    public class GiantGoblinDashPattern : BossPattern
    {
        [SerializeField] private float _wallCheckdistance = 1;
        private float _dashDirection;
        [SerializeField] private float _maxSpeed = 5;
        [SerializeField] private float _speedIncreaseDuration = 2f;
        [SerializeField] private AnimationCurve _speedIncreaseCurve;
        private float _speedTime = 0;
        private LayerMask _whatIsGround;

        public override void OnStart()
        {
            base.OnStart();
            _whatIsGround = Boss.BillboardValue<LayerMaskValue>("WhatIsGround").Value;
            _dashDirection = CheckPlayerPos();
        }

        private float CheckPlayerPos()
        {
            var dir = Mathf.Sign(Player.transform.position.x - Transform.position.x);
            EntityRenderer.LookTarget(dir);
            return dir;
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            if (_speedIncreaseDuration > _speedTime)
                _speedTime += Time.deltaTime;
            var currentSpeed = _speedIncreaseCurve.Evaluate(_speedTime/_speedIncreaseDuration) * _maxSpeed;

            Mover.SetMovement(Vector2.right * currentSpeed * _dashDirection);
            CheckWall();
        }

        private void CheckWall()
        {
            RaycastHit2D hit = Physics2D.Raycast(Transform.position, Vector2.right * _dashDirection, _wallCheckdistance, _whatIsGround);
            if (hit.transform != null)
                OnGroggy(3f);
        }

        public override void OnDrawGizmos(Transform transform)
        {
            base.OnDrawGizmos(transform);
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, transform.right * _wallCheckdistance);
            Gizmos.color = Color.white;
        }
    }
}
