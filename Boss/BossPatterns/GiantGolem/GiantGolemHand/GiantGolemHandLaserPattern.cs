using Crogen.CrogenPooling;
using DG.Tweening;
using Hashira.Bosses.Patterns.GiantGolem;
using UnityEngine;

namespace Hashira.Bosses.Patterns
{
    [System.Serializable]
    public class GiantGolemHandLaserPattern : GiantGolemHandPattern
    {
        private float _laserFireDelay = 2f;
        private float _laserFireTime;
        private float _laserAngle = 32.5f;
        [SerializeField] private int _laserCount = 3;
        private Laser[] _lasers;

        public override void Initialize(Boss boss)
        {
            base.Initialize(boss);
            _lasers = new Laser[_laserCount];
        }

        public override void OnStart()
        {
            base.OnStart();
            Transform.DOLocalMoveY(2, 1);
            _laserFireTime = Time.time;
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            if (_laserFireDelay + _laserFireTime < Time.time)
            {
                _laserFireTime = Time.time;
                FireLasers(Player.transform.position);
            }
        }

        private void FireLasers(Vector2 targetPosition)
        {
            Vector2 playerDir = (targetPosition - (Vector2)Transform.position).normalized;
            
            for (int i = 0; i < _laserCount; i++)
            {
                _lasers[i] = PopCore.Pop(ProjectilePoolType.EnemyLaser, Transform) as Laser;
                float angle = MathExtension.Remap(i, 0, _laserCount-1, -_laserCount/2, _laserCount/2) * _laserAngle;
                Vector2 direction = Quaternion.Euler(0, 0, angle) * playerDir;
                
                _lasers[i]?.SetAttackDirection(direction);
                _lasers[i]?.ShowVisualizer(0.85f)
                    .Join(DOTween.To(x => _lasers[i].SetAlpha(x), 0.3f, 1f, 0.85f).SetEase(Ease.InExpo))
                    .Append(_lasers[i].Blink(0.6f))
                    .Append(_lasers[i].StartAttack(duration: 0.25f));             
            }
        }
        
        public override void OnEnd()
        {
            foreach (var laser in _lasers)
            {
                laser?.Push();
            }

            Transform.DOKill(true);
            ReturnToOriginPosition();
            base.OnEnd();
        }
    }
}