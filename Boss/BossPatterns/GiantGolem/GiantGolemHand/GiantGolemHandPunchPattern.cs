using DG.Tweening;
using Hashira.Bosses.Patterns.GiantGolem;
using Hashira.MainScreen;
using UnityEngine;

namespace Hashira.Bosses.Patterns
{
    [System.Serializable]
    public class GiantGolemHandPunchPattern : GiantGolemHandPattern
    {
        [SerializeField] private float _handMaxPositionY = 1;
        [SerializeField] private int _punchCount = 5;
        private int _currentPunchCount = 0;
        private bool _followPlayer;
        private float _originRotate;
        
        private GiantGolemPlatform _selectedPlatform;
        
        public override void OnStart()
        {
            base.OnStart();
            _giantGolemEye.LookAtPlayerDirection = true;
            _currentPunchCount = 0;
            SelectRandomPlatform();
            OnPunch();
        }

        private void SelectRandomPlatform()
        {
            float minDis = float.MaxValue;
            
            foreach (var platform in _giantGolemPlatformList.GetAllPlatforms())
            {
                float currentDis = Vector2.Distance(platform.transform.position, Player.transform.position);
                if (currentDis < minDis)
                {
                    minDis = currentDis;
                    _selectedPlatform = platform;
                }
            }
        }        
        
        public override void OnUpdate()
        {
            base.OnUpdate();
            if (_followPlayer)
            {
                MoveToTarget(Player.transform.position);
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

            
            // Sequence seq = DOTween.Sequence();
            // seq.AppendCallback(()=>_followPlayer=true);
            // seq.Append(Transform.DOMoveY(_handMaxPositionY, _handFollowDuration));
            // //seq.AppendInterval(_handFollowDuration);
            // seq.AppendCallback(()=>_followPlayer=false);
            // seq.Join(Transform.DOMoveY(Boss.GetGroundFloorPosY()+2, 0.45f)).SetEase(Ease.InBack, 3);
            // seq.Join(Transform.DOShakeRotation(0.25f, new Vector3(0, 0, 3.5f), 30, 90f, false));
            // seq.AppendCallback(() =>
            // {
            //     MainScreenEffect.OnScaling(1);
            //     MainScreenEffect.OnRotate(_originRotate-Transform.position.x/3);
            //     CameraManager.Instance.ShakeCamera(17, 10, 0.35f);
            // });
            // seq.AppendInterval(1.55f);
            // seq.Append(ReturnToOriginPosition(1.25f).SetEase(Ease.InQuart));
            // seq.OnComplete(OnPunch);
        }

        private void ReadyToAttack()
        {
            Sequence seq = DOTween.Sequence();
            
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
