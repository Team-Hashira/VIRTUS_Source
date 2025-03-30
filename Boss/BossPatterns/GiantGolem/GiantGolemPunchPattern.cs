using DG.Tweening;
using Hashira.Bosses.BillboardClasses;
using Hashira.Bosses.Patterns.GiantGolem;
using Hashira.MainScreen;
using UnityEngine;

namespace Hashira.Bosses.Patterns
{
    public class GiantGolemPunchPattern : GiantGolemPattern
    {
        [SerializeField] private float _handMaxPositionY = 1;
        [SerializeField] private int _punchCount = 5;
        [SerializeField] private float _handSpeed = 1f;
        [SerializeField] private float _handFollowDuration = 1f;
        private int _currentPunchCount = 0;
        
        private GiantGolemHand _currentHand;

        private bool _followPlayer;

        private float _originRotate;
        
        public override void OnStart()
        {
            base.OnStart();
            _giantGolemEye.LookAtPlayerDirection = true;
            _currentPunchCount = 0;
            _originRotate = Transform.eulerAngles.z;
            OnPunch();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            if (_followPlayer)
            {
                _currentHand.MoveToPlayer(Player, _handSpeed, true, false);
            }
        }

        private void OnPunch()
        {
            ++_currentPunchCount;
            if (_currentPunchCount >= _punchCount)
            {
                EndPattern();
                return;
            }

            float handLToPlayerDis = Vector2.Distance(Player.transform.position, _handL.transform.position);
            float handRToPlayerDis = Vector2.Distance(Player.transform.position, _handR.transform.position);
            _currentHand = handLToPlayerDis > handRToPlayerDis ? _handR : _handL;

            Sequence seq = DOTween.Sequence();
            seq.AppendCallback(()=>_followPlayer=true);
            seq.Append(_currentHand.transform.DOMoveY(_handMaxPositionY, _handFollowDuration));
            //seq.AppendInterval(_handFollowDuration);
            seq.AppendCallback(()=>_followPlayer=false);
            seq.Join(_currentHand.transform.DOMoveY(Boss.GetGroundFloorPosY()+2, 0.45f)).SetEase(Ease.InBack, 3);
            seq.Join(_currentHand.transform.DOShakeRotation(0.25f, new Vector3(0, 0, 3.5f), 30, 90f, false));
            seq.AppendCallback(() =>
            {
                _currentHand.SetActiveEffect<PunchHandEffect>(false);
                MainScreenEffect.OnScaling(1);
                MainScreenEffect.OnRotate(_originRotate-_currentHand.transform.position.x/3);
                CameraManager.Instance.ShakeCamera(17, 10, 0.35f);
            });
            seq.AppendInterval(1.55f);
            seq.Append(_currentHand.ResetToOriginPosition(1.25f).SetEase(Ease.InQuart));
            seq.OnComplete(OnPunch);
        }

        public override void OnEnd()
        {
            base.OnEnd();
            _giantGolemEye.LookAtPlayerDirection = false;
            MainScreenEffect.OnRotate(_originRotate, 1f);
        }

        public override void OnDrawGizmos(Transform transform)
        {
            base.OnDrawGizmos(transform);
            Gizmos.DrawLine(new Vector3(-1000, _handMaxPositionY), new Vector3(1000, _handMaxPositionY));
        }
    }
}
