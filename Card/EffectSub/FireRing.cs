using Crogen.CrogenPooling;
using Hashira.Combat;
using Hashira.Core;
using Hashira.Enemies;
using Hashira.Entities;
using Hashira.LightingControl;
using Hashira.StageSystem;
using UnityEngine;

namespace Hashira
{
    public class FireRing : MonoBehaviour, IPoolingObject
    {
        [SerializeField] private SpriteRenderer _visual;
        [SerializeField] private ParticleSystem _ringParticle;
        [SerializeField] private ParticleSystem _startBurstParticle;
        [SerializeField] private ParticleSystem _endBurstParticle;
        [SerializeField] private float _rotationSpeed;
        [SerializeField] private float _damageDelay;
        [SerializeField] private float _radius;
        [SerializeField] private LayerMask _whatIsTarget;
        [SerializeField] private float _delayPush = 1f;

        public string OriginPoolType { get; set; }
        GameObject IPoolingObject.gameObject { get; set; }

        private int _damage;
        private int _burstDamage;
        private float _lastDamageTime;
        private float _spawnTime;
        private float _duration;

        public void OnPop()
        {
            _lastDamageTime = Time.time;
            _spawnTime = Time.time;
            _startBurstParticle.Play();
            _ringParticle.Play();
        }

        public void OnPush()
        {

        }

        public void Init(int damage, int burstDamage, float duration)
        {
            _visual.enabled = true;
            _damage = damage;
            _burstDamage = burstDamage;
            _duration = duration;
        }

        private void Awake()
        {

        }

        private void Update()
        {
            _visual.transform.Rotate(new Vector3(0, 0, _rotationSpeed * Time.deltaTime));

            if (_visual.enabled && _lastDamageTime + _damageDelay < Time.time)
            {
                _lastDamageTime = Time.time;
                RaycastHit2D[] raycastHits = Physics2D.CircleCastAll(transform.position, _radius, Vector2.zero, 0, _whatIsTarget);
                foreach (RaycastHit2D raycastHit in raycastHits)
                {
                    if (raycastHit.transform.TryGetComponent(out IDamageable damageable))
                    {
                        AttackInfo attackInfo = new AttackInfo(_damage, Vector2.zero, EAttackType.Fire);
                        damageable.ApplyDamage(attackInfo, raycastHit);
                        Vector3 dir = raycastHit.transform.position - transform.position;
                        PopCore.Pop(EffectPoolType.FireRingHitEffect, raycastHit.transform.position, Quaternion.Euler(0, 0, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 90f));
                        CameraManager.Instance.ShakeCamera(4, 4, 0.2f);
                    }
                }
            }

            if (_spawnTime + _duration < Time.time)
            {
                _ringParticle.Stop();
                if (_visual.enabled)
                {
                    foreach (Enemy enemy in StageGenerator.Instance.GetCurrentStage().GetEnabledEnemies())
                    {
                        if (enemy.TryGetEntityComponent(out EntityHealth health))
                        {
                            AttackInfo attackInfo = new AttackInfo(_burstDamage, Vector2.zero, EAttackType.Fire);
                            health.ApplyDamage(attackInfo);
                        }
                    }
                    _endBurstParticle.Play();
                    CameraManager.Instance.ShakeCamera(8, 12, 0.3f);
                    LightingController.Aberration(1, 0.4f);
                }
                _visual.enabled = false;
                if (_spawnTime + _duration + _delayPush < Time.time)
                    this.Push();
            }
        }



        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, _radius);
        }
    }
}
