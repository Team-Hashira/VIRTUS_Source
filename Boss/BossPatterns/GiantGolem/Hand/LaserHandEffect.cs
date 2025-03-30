using Hashira.Bosses.Patterns.GiantGolem;
using Hashira.Combat;
using Hashira.Entities.Components;
using UnityEngine;

namespace Hashira.Bosses.Patterns
{
    public class LaserHandEffect : MonoBehaviour, IGiantGolemHandEffect
    {
        [SerializeField] private ParticleSystem _attackVisualizerParticle;
        [SerializeField] private ParticleSystem _endPointParticle;
        [SerializeField] private Transform _damageCasterScaleHandler;
        [field:SerializeField] public AttackVisualizer AttackVisualizer { get; private set; }
        [SerializeField] private LayerMask _whatIsGround;
        [SerializeField] private float _laserAttackDelay = 0.1f;
        [SerializeField] private bool _isFlipX = false;
        private float _currentAttackTime = 0;

        [SerializeField] private LineRenderer _lineRenderer;

        private bool _currentActive;
        private GiantGolemHand _giantGolemHand;
        private Vector2 _finalDirection;
        
        public void Init(GiantGolemHand giantGolemHand)
        {
            _lineRenderer.useWorldSpace = true;
            _giantGolemHand = giantGolemHand;
        }

        private void LateUpdate()
        {
            SetEndPoint();
            _finalDirection = _isFlipX ? new Vector2(-_giantGolemHand.HandNormalizedDirection.x, _giantGolemHand.HandNormalizedDirection.y) : _giantGolemHand.HandNormalizedDirection;
            _damageCasterScaleHandler.right = _finalDirection;
            
            if (_currentAttackTime + _laserAttackDelay < Time.time && _currentActive)
            {
                _currentAttackTime = Time.time;
                SoundManager.Instance.PlaySFX("GiantGolemLaser", transform.position, 1, UnityEngine.Random.Range(0.9f, 0.5f));
                AttackVisualizer.DamageCaster.CastDamage(1, false);
            }
        }

        public void ShowVisualizer()
        {
            _attackVisualizerParticle.Play(true);
        }
        
        private void SetEndPoint()
        {
            RaycastHit2D hit = Physics2D.Raycast(
                transform.position,
                _finalDirection, 
                1000, _whatIsGround);
            if (hit.transform != null)
            {
                float laserLength = Vector3.Distance(hit.point, transform.position)/2f;
                _damageCasterScaleHandler.localScale = new Vector3(laserLength, 1, 1);
                _lineRenderer.SetPosition(0, transform.position);
                _lineRenderer.SetPosition(1, hit.point);
                _endPointParticle.transform.position = hit.point;
            }
        }

        public void SetActive(bool active)
        {
            _currentActive = active;
            if (active)
            {
                _endPointParticle.Play(true);
                _lineRenderer.gameObject.SetActive(true);
            }
        }

        public void OnAnimatorTrigger(EAnimationTriggerType triggerType, int count)
        {
            if (_currentActive == false && triggerType == EAnimationTriggerType.End)
            {
                _endPointParticle.Stop(true);
                _lineRenderer.gameObject.SetActive(false);
            }
        }
    }
}
