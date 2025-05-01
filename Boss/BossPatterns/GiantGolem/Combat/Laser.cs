using Crogen.CrogenPooling;
using DG.Tweening;
using Hashira.Combat;
using UnityEngine;

namespace Hashira.Bosses.Patterns.GiantGolem
{
    public class Laser : MonoBehaviour, IPoolingObject
    {
        public string OriginPoolType { get; set; }
        GameObject IPoolingObject.gameObject { get; set; }
        
        [SerializeField] private EffectPoolType _muzzleVFXPoolType;
        [SerializeField] private EffectPoolType _hitVFXPoolType;
        
        private IPoolingObject _hitVFXObject;
        
        [SerializeField] private Transform _damageCasterHandleTransform;
        [SerializeField] private LineRenderer _lineRenderer;
        
        private BoxDamageCaster2D _damageCaster;
        private AttackVisualizer _attackVisualizer;
        private Vector2 _endPosition;
        private Vector2 _attackDirection;

        public bool isPenetrate;
        private bool _isAttacking;

        private Sequence _visualizerSequence;
        private Sequence _attackSequence;

        public bool IsAttacking
        {
            get => _isAttacking;
            private set
            {
                _isAttacking = value;
                _lineRenderer.gameObject.SetActive(_isAttacking);
                if (_isAttacking == true)
                {
                    _hitVFXObject = gameObject.Pop(_hitVFXPoolType, _endPosition, Quaternion.identity);
                    gameObject.Pop(_muzzleVFXPoolType, transform.position, Quaternion.identity);
                    CameraManager.Instance.ShakeCamera(10, 20, 0.25f);
                }
                else
                    _hitVFXObject?.Push();
            }
        }
        [SerializeField] private LayerMask _whatIsTarget;
        
        private void Awake()
        {
            _attackVisualizer = _damageCasterHandleTransform.GetComponentInChildren<AttackVisualizer>();
            _damageCaster = _attackVisualizer.DamageCaster as BoxDamageCaster2D;
            _attackVisualizer.InitDamageCastVisualSign();
        }
        
        public void OnPop()
        {
            IsAttacking = false;
            _attackVisualizer.SetAlpha(1);
            _attackVisualizer.SetDamageCastValue(0);
        }

        public void OnPush()
        {
            StopAllCoroutines();
            _visualizerSequence?.Kill();
            _attackSequence?.Kill();
        }

        private void Update()
        {
            if (IsAttacking)
            {
                CastDamage();
            }
        }

        public Tween Blink(float delay) => _attackVisualizer.Blink(delay);
        public void SetAlpha(float alpha) => _attackVisualizer.SetAlpha(alpha);
        
        public Sequence ShowVisualizer(float duration)
        {
            _visualizerSequence = DOTween.Sequence();
            _visualizerSequence.Append(_attackVisualizer.SetDamageCastValue(1, duration));
            return _visualizerSequence;
        }
        public Sequence StartAttack(float duration = 1f)
        {
            _attackSequence = DOTween.Sequence();
            _attackSequence
                .AppendCallback(() =>
                {
                    _attackVisualizer.SetDamageCastValue(0);
                    _lineRenderer.widthMultiplier = 1;
                    IsAttacking = true;
                })
                .Append(DOTween.To(x=>_lineRenderer.widthMultiplier = x, 1, 0, duration).SetEase(Ease.InQuint))
                .AppendCallback(() =>
                {
                    IsAttacking = false;
                    this.Push();
                });

            return _attackSequence;
        }
        public void SetAttackDirection(Vector2 direction)
        {
            _attackDirection = direction;
            float distance = 1000f;
            _endPosition = _attackDirection * distance;
            if (isPenetrate == false)
            {
                RaycastHit2D hit = Physics2D.Raycast(transform.position, _attackDirection, 1000, _whatIsTarget);
                if (hit.transform)
                {
                    distance = Vector2.Distance(transform.position, hit.point);
                    _endPosition = hit.point;
                }
            }
            
            _damageCasterHandleTransform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(_attackDirection.y, _attackDirection.x) * Mathf.Rad2Deg);
            _damageCasterHandleTransform.localScale = new Vector3(distance, 1, 1);
            
            _lineRenderer.SetPosition(0, transform.position);
            _lineRenderer.SetPosition(1, _endPosition);
        }

        private void CastDamage()
        {
            _damageCaster.CastDamage(AttackInfo.defaultOneDamage, popupText:false);
        }
    }
}
