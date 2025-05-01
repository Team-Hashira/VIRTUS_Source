using Crogen.CrogenPooling;
using Hashira.Core;
using Hashira.Core.EventSystem;
using Hashira.Core.StatSystem;
using Hashira.Entities;
using Hashira.Entities.Components;
using Hashira.Players;
using Hashira.Projectiles;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Hashira
{
    public class Attacker : MonoBehaviour
    {
        private Player _player;
        [SerializeField] private InputReaderSO _input;
        [SerializeField] private LayerMask _whatIsTarget;
        [SerializeField] private RingIcon _ringIcon;
        [SerializeField] private float _wallChecker;
        private float _lastAttackTime;
        private int _burstBulletCount = 1;
        private ProjectilePoolType _projectilePoolType = ProjectilePoolType.Bullet;

        private bool _isPressed;
        private bool _isCharging;
        private bool _isChargeMode;
        private float _maxChargeTime;
        private float _maxChargeDamageMultiply = 3f;
        private float _chargeStartTime = 0.2f;
        private float _lastPressedTime;

        private StatElement _attackPowerStat;
        private StatElement _attackSpeedStat;
        private StatElement _bulletSpeedStat;

        private SpriteRenderer _spriteRenderer;

        public event Action<bool> OnChargeEnableEvent;

        private void Awake()
        {
            _player = PlayerManager.Instance.Player;

            _input.OnAttackEvent += HandleAttackEvent;
            _lastAttackTime = Time.time;

            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            EntityStat playerStat = _player.GetEntityComponent<EntityStat>();

            _attackPowerStat = playerStat.StatDictionary[StatName.AttackPower];
            _attackSpeedStat = playerStat.StatDictionary[StatName.AttackSpeed];
            _bulletSpeedStat = playerStat.StatDictionary[StatName.ProjectileSpeed];
        }

        private void Update()
        {
            if (_isPressed)
            {
                if (_isChargeMode)
                {
                    if (_isCharging == false && _lastPressedTime + _chargeStartTime < Time.time)
                    {
                        _isCharging = true;
                        OnChargeEnableEvent?.Invoke(true);
                        _ringIcon.SetActive(true);
                    }
                    if (_isCharging)
                        _ringIcon.SetAmount((Time.time - (_lastPressedTime + _chargeStartTime)) / _maxChargeTime);
                }
                else if (_lastAttackTime + 1 / _attackSpeedStat.Value < Time.time)
                {
                    _lastAttackTime = Time.time;
                    Shoot(true);
                }
            }

            bool isFliped = transform.parent.eulerAngles.z > 90 && transform.parent.eulerAngles.z < 270;
            if (isFliped ^ _spriteRenderer.flipY)
            {
                _spriteRenderer.flipY = isFliped;
            }
        }

        public void SetChargeMode(float maxChargeTime)
        {
            _isChargeMode = true;
            _maxChargeTime = maxChargeTime;
        }

        public void AddBurstBullets()
            => _burstBulletCount++;
        public void RemoveBurstBullets()
            => _burstBulletCount--;

        private void HandleAttackEvent(bool isDown)
        {
            _isPressed = isDown;
            if (_isChargeMode)
            {
                if (isDown)
                {
                    _lastPressedTime = Time.time;
                }
                else if (_isCharging)
                {
                    _isCharging = false;
                    _ringIcon.SetActive(false);
                    OnChargeEnableEvent?.Invoke(false);
                    float amount = (Time.time - (_lastPressedTime + _chargeStartTime)) / _maxChargeTime;
                    _lastAttackTime = Time.time;
                    Shoot(true, Mathf.CeilToInt(Mathf.Lerp(_attackPowerStat.IntValue, _attackPowerStat.IntValue * _maxChargeDamageMultiply, amount)));
                }
                else if (_lastAttackTime + 1 / _attackSpeedStat.Value < Time.time)
                {
                    _lastAttackTime = Time.time;
                    Shoot(true);
                }
            }
        }

        public void Shoot(bool isPlayerInput = false, int damage = -1)
        {
            RaycastHit2D wallHit = Physics2D.Raycast(transform.parent.position, transform.right, _wallChecker, _whatIsTarget);
            Vector3 shootPoint = wallHit == default ? transform.position : (Vector3)wallHit.point - transform.right * 0.3f;
            float angle = _burstBulletCount * 5;
            List<Projectile> createdProjectileList = new List<Projectile>();
            for (int i = 0; i < _burstBulletCount; i++)
            {
                Vector2 direction = Quaternion.Euler(0, 0, -angle / 2 + angle * (i + 0.5f) / _burstBulletCount) * transform.right;
                if (damage == -1)
                    damage = _attackPowerStat.IntValue;
                Projectile projectile = gameObject.Pop(_projectilePoolType, shootPoint, Quaternion.identity) as Projectile;
                projectile.Init(_whatIsTarget, direction, _bulletSpeedStat.Value, damage, _player.transform, true, 5);

                // 사격 이펙트
                ParticleSystem particleSystem = gameObject.Pop(EffectPoolType.BulletShootSparkleEffect, shootPoint, projectile.transform.rotation).gameObject.GetComponent<ParticleSystem>();
                var mianModule = particleSystem.main;
                mianModule.startRotation = -transform.eulerAngles.z * Mathf.Deg2Rad;

                // 이벤트 발행
                var evt = InGameEvents.ProjectileShootEvent;
                evt.projectile = projectile;
                evt.isPlayerInput = isPlayerInput;
                GameEventChannel.RaiseEvent(evt);

                CameraManager.Instance.ShakeCamera(Mathf.Log10(projectile.damage * 10) * 4f, Mathf.Log10(projectile.damage * 10) * 4f, 0.15f);

                createdProjectileList.Add(projectile);
            }
            SoundManager.Instance.PlaySFX("PlayerShoot", _player.transform.position, 1f);
        }

        public ProjectilePoolType SetProjectile(ProjectilePoolType projectilePoolType)
        {
            ProjectilePoolType prev = _projectilePoolType;
            _projectilePoolType = projectilePoolType;
            return prev;
        }


        private void OnDestroy()
        {
            _input.OnAttackEvent -= HandleAttackEvent;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.parent.position, transform.parent.position + transform.right * _wallChecker);
        }
    }
}
