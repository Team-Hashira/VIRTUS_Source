using DG.Tweening;
using Hashira.Bosses.BillboardClasses;
using Hashira.Combat;
using Hashira.MainScreen;
using Hashira.Pathfind;
using System.Collections;
using UnityEngine;

namespace Hashira.Bosses.Patterns
{
    [System.Serializable]
    public class GiantGoblinDashPattern : BossPattern
    {
        [SerializeField] private float _dashDelay = 0.5f;
        [SerializeField] private float _dashDuration = 1.75f;
        [SerializeField] private float _wallCheckdistance = 1;
        private float _dashDirection;
        [SerializeField] private float _maxSpeed = 5;
        [SerializeField] private float _speedIncreaseDuration = 2f;
        [SerializeField] private DamageCaster2D _dashDamageCaster;
        private float _speedTime = 0;
        private LayerMask _whatIsGround;
        private bool _isDashStart = false;
        
        public override void OnStart()
        {
            base.OnStart();
            _speedTime = 0;
            _whatIsGround = Boss.BillboardValue<LayerMaskValue>("WhatIsGround").Value;
            _dashDirection = CheckPlayerPos();
            _isDashStart = false;
            Boss.StartCoroutine(CoroutineStartDash());
        }

        private IEnumerator CoroutineStartDash()
        {
            yield return new WaitForSeconds(_dashDelay);
            _isDashStart = true;
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
            if (_isDashStart == false) return;
            
            var currentSpeed = _maxSpeed;
            _speedTime += Time.deltaTime;
            if (_speedIncreaseDuration > _speedTime)
                currentSpeed = Mathf.Lerp(0, _maxSpeed, _speedTime/_speedIncreaseDuration);
            else
                _dashDamageCaster.CastDamage(
                    new AttackInfo(1, Vector2.right * _dashDirection * 10), 
                    Vector2.zero, false);
            
            if (_dashDuration < _speedTime) EndPattern();
            else Mover.SetMovement(Vector2.right * currentSpeed * _dashDirection);
            
            CheckWall();
        }

        private void CheckWall()
        {
            RaycastHit2D hit = Physics2D.Raycast(Transform.position, Vector2.right * _dashDirection, _wallCheckdistance, _whatIsGround);
            if (hit.transform != null)
            {
                Groggy(3f);
                Sequence seq = DOTween.Sequence();
                seq.Append(MainScreenEffect.OnLocalMoveScreenSide(_dashDirection > 0 ? DirectionType.Right : DirectionType.Left));
                seq.JoinCallback(()=>CameraManager.Instance.ShakeCamera(10, 30, 2.8f, Ease.InCirc));
                seq.AppendInterval(0.02f);
                seq.AppendCallback(() => MainScreenEffect.OnLocalMoveScreenSide(0));
            }
        }

        public override void OnEnd()
        {
            Mover.StopImmediately();
            base.OnEnd();
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
