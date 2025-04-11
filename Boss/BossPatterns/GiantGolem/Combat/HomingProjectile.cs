using Crogen.CrogenPooling;
using Hashira.Combat;
using Hashira.Projectiles;
using System;
using UnityEngine;

namespace Hashira.Bosses.Patterns.GiantGolem
{
    public class HomingProjectile : MonoBehaviour, IPoolingObject
    {
        [SerializeField] private EffectPoolType _dieEffectPoolType;
        
        public string OriginPoolType { get; set; }
        public GameObject gameObject { get; set; }

        public event Action<HomingProjectile> OnDieEvent;
        private DamageCaster2D _damageCaster;
        private Vector2 _direction;
        private float _speed;
        private float _duration;
        private float _currentTime = 0;
        
        private void Awake()
        {
            _damageCaster = GetComponent<DamageCaster2D>();
            _damageCaster.OnDamageCastSuccessEvent += OnDieHandle;
        }

        private void OnDestroy()
        {
            _damageCaster.OnDamageCastSuccessEvent -= OnDieHandle;
        }

        private void OnDieHandle(HitInfo info)
        {
            PopCore.Pop(_dieEffectPoolType, transform.position, Quaternion.identity);
            this.Push();
        }

        public void Init(Vector2 direction, float speed, float duration)
        {
            _direction = direction;
            _speed = speed;
            _duration = duration;
            _currentTime = Time.time;
        }
        
        public void OnPop()
        {
        }

        private void FixedUpdate()
        {
            _damageCaster.CastDamage(AttackInfo.defaultOneDamage, popupText:false);
            transform.position += (Vector3)_direction * _speed * Time.fixedDeltaTime;
            if (_currentTime + _duration < Time.time)
            {
                this.Push();   
            }
        }

        public void OnPush()
        {
            OnDieEvent?.Invoke(this);
        }
    }
}