using DG.Tweening;
using Hashira.Bosses.BillboardClasses;
using Hashira.MainScreen;
using UnityEngine;

namespace Hashira.Bosses.Patterns
{
    public class GiantGoblinStampPattern : BossPattern
    {
        [SerializeField] private float _jumpPower = 2f;
        [SerializeField] private float _jumpDuration = 1f;
        private float _bottom;
        private LayerMask _whatIsGround;

        public override void OnStart()
        {
            base.OnStart();
            SetBottom();

            _whatIsGround = Boss.BillboardValue<LayerMaskValue>("WhatIsGround").Value;

            var selfHeight = Transform.localScale.y;
            var jumpEndPos = new Vector2(Player.transform.position.x, _bottom + selfHeight);

            Transform.DOJump(jumpEndPos, _jumpPower , 1, _jumpDuration).OnComplete(() =>
            {
                MainScreenEffect.OnLocalMoveScreenSide(Pathfind.DirectionType.Down);
                CameraManager.Instance.ShakeCamera(10, 5, 0.25f);
                EndPattern();
            });
        }

        private void SetBottom()
        {
            Physics2D.Raycast(Transform.position, -Vector2.up, 10, _whatIsGround);
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
        }
    }
}
