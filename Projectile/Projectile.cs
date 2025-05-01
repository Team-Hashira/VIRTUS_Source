using Crogen.CrogenPooling;
using Hashira.Combat;
using Hashira.Core.EventSystem;
using Hashira.Entities;
using Hashira.StageSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hashira.Projectiles
{
    public class TrailData
    {
        public float time = -1;
        public float width = -1;
        public Color? startColor = null;
        public Color? endColor = null;
    }

    public enum EProjectileUndyingMode
    {
        Reflection,
        penetration
    }

    public class Projectile : ProjectileBase, IPoolingObject
    {
        public bool IsDead { get; set; } = false;
        public bool IsEventSender { get; private set; }
        public float MoveDistance { get; set; }
        public LayerMask WhatIsTarget { get; protected set; }
        [SerializeField] protected float _pushDelay;
        [SerializeField] protected bool _canMultipleAttacks;
        [SerializeField] protected SpriteRenderer _spriteRenderer;
        [SerializeField] protected TrailRenderer _trailRenderer;
        protected float _gravity = 3f;
        protected float _yVelocity;
        public float Speed => movement.magnitude;

        private Dictionary<EProjectileUndyingMode, int> _lifeCountDictionary;

        public void SetLifeCount(EProjectileUndyingMode eProjectileUndyingMode, int count)
        {
            _lifeCountDictionary[eProjectileUndyingMode] += count;
        }
        public int GetLifeCount(EProjectileUndyingMode eProjectileUndyingMode)
        {
            return _lifeCountDictionary[eProjectileUndyingMode];
        }
        public bool TryGetCurrentLifeCountType(out EProjectileUndyingMode projectileUndyingMode)
        {
            foreach (EProjectileUndyingMode eProjectileUndyingMode in _lifeCountDictionary.Keys)
            {
                if (_lifeCountDictionary[eProjectileUndyingMode] > 0)
                {
                    projectileUndyingMode = eProjectileUndyingMode;
                    return true;
                }
            }
            projectileUndyingMode = EProjectileUndyingMode.Reflection;
            return false;
        }

        public Transform Owner { get; set; }

        public EAttackType attackType;

        [HideInInspector] public int damage;
        [HideInInspector] public Vector3 movement;

        private Vector3 _defaultScale;
        private Color _defaultSpriteColor;
        private TrailData _defaultTrailData;
        private Sprite _defaultSprite;

        private HashSet<Transform> _stayTransformSet;
        private RaycastHit2D[] _currentHit2D;

        protected virtual void Awake()
        {
            DefaultSetting();
        }

        private void DefaultSetting()
        {
            _defaultScale = transform.localScale;
            _defaultSpriteColor = _spriteRenderer.color;
            _defaultSprite = _spriteRenderer.sprite;
            _defaultTrailData = new TrailData();
            _defaultTrailData.startColor = _trailRenderer.startColor;
            _defaultTrailData.endColor = _trailRenderer.endColor;
            _defaultTrailData.time = _trailRenderer.time;
            _defaultTrailData.width = _trailRenderer.widthMultiplier;
        }


        // 데미지 처리 후
        private void AfterDamageCast(HitInfo info, int damage)
        {
            if (IsEventSender)
            {
                var projectileHitEvent = InGameEvents.ProjectileAfterHitEvent;
                projectileHitEvent.hitInfo = info;
                projectileHitEvent.projectile = this;
                projectileHitEvent.appliedDamage = damage;
                GameEventChannel.RaiseEvent(projectileHitEvent);
            }
        }

        // 데미지 처리 전
        private void BeginDamageCast(HitInfo info)
        {
            if (IsEventSender)
            {
                var projectileBeginHitEvent = InGameEvents.ProjectileBeginHitEvent;
                projectileBeginHitEvent.hitInfo = info;
                projectileBeginHitEvent.projectile = this;
                GameEventChannel.RaiseEvent(projectileBeginHitEvent);
            }
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
        }

        protected override void Move()
        {
            if (IsDead) return;
            // 중력
            movement -= Owner.up * _gravity * Time.fixedDeltaTime * 25f;
            transform.right = movement.normalized;

            _projectileCollider2D.CheckCollision(WhatIsTarget, out _currentHit2D, movement * Time.fixedDeltaTime);

            HashSet<Transform> prevTransformSet = _stayTransformSet;
            _stayTransformSet = new HashSet<Transform>(_currentHit2D.Length);
            for (int i = 0; i < _currentHit2D.Length; i++)
            {
                _stayTransformSet.Add(_currentHit2D[i].transform);
            }
            if (_currentHit2D.Length > 0)
            {
                float moveDistance = movement.magnitude * Time.fixedDeltaTime;
                MoveDistance += moveDistance;
                Vector3 startPos = transform.position;
                foreach (RaycastHit2D raycastHit in _currentHit2D)
                {
                    HitInfo hitInfo = new HitInfo();
                    hitInfo.raycastHit = raycastHit;
                    hitInfo.damageable = raycastHit.transform.GetComponent<IDamageable>();
                    hitInfo.entity = raycastHit.transform.GetComponent<Entity>();

                    bool isStayTransform = prevTransformSet.Contains(raycastHit.transform);
                    if (isStayTransform == false || hitInfo.damageable == null)
                    {
                        MoveDistance += raycastHit.distance - moveDistance;
                        // 데미지
                        BeginDamageCast(hitInfo);
                        int finalDamage = CalculateDamage(damage);
                        AttackInfo attackInfo = new AttackInfo(finalDamage, Vector2.zero, attackType);
                        if (hitInfo.damageable != null)
                            hitInfo.damageable.ApplyDamage(attackInfo, raycastHit);
                        AfterDamageCast(hitInfo, finalDamage);
                        OnHited(hitInfo);

                        // 이동
                        moveDistance = raycastHit.distance;

                        bool dieable = true;
                        bool isReflected = false;
                        foreach (var undyingMode in _lifeCountDictionary.Keys)
                        {
                            if (_lifeCountDictionary[undyingMode] > 0)
                            {
                                _lifeCountDictionary[undyingMode]--;
                                if (undyingMode == EProjectileUndyingMode.Reflection)
                                    isReflected = true;
                                dieable = false;
                                break;
                            }
                        }

                        if (isReflected)
                            break;

                        if (dieable)
                        {
                            StartCoroutine(DelayPushCoroutine(_pushDelay));
                            break;
                        }
                    }
                }
                transform.position = startPos + movement.normalized * moveDistance;
            }
            else
            {
                // 이동
                transform.position += movement * Time.fixedDeltaTime;
                MoveDistance += movement.magnitude * Time.fixedDeltaTime;
            }
        }

        /// <summary>
        /// OnPush전 죽기은 당시 실행
        /// </summary>
        protected virtual void OnDie()
        {
            IsDead = true;
            _spriteRenderer.color = Color.clear;
        }

        public IEnumerator DelayPushCoroutine(float time)
        {
            OnDie();
            yield return new WaitForSeconds(time);
            this.Push();
        }

        public virtual int CalculateDamage(float damage)
        {
            return Mathf.CeilToInt(damage);
        }

        public void Redirection(Vector2 direction, float speed = -1f)
        {
            movement = direction.normalized * Speed;
            damage = Mathf.CeilToInt(damage * 0.8f);
        }

        protected virtual void OnHited(HitInfo hitInfo) { }

        public virtual void Init(LayerMask whatIsTarget, Vector3 direction, float speed, int damage, Transform owner, bool isEventSender = true, float gravity = 0)
        {
            this.damage = damage;
            IsEventSender = isEventSender;
            WhatIsTarget = whatIsTarget;
            MoveDistance = 0;
            _gravity = gravity;

            transform.right = direction;
            movement = transform.right * speed;

            Owner = owner;
            _yVelocity = 0;

            _stayTransformSet = new HashSet<Transform>();

            _lifeCountDictionary = new()
            {
                { EProjectileUndyingMode.Reflection, 0},
                { EProjectileUndyingMode.penetration, 0},
            };

            SetVisual(_defaultSpriteColor, _defaultTrailData, _defaultSprite, 2);
        }

        public void SetVisual(Color? bulletColor = null, TrailData trailData = null, Sprite bulletSprite = null, float scaleMultiplier = 1)
        {
            if (bulletSprite != null)
                _spriteRenderer.sprite = bulletSprite;
            if (bulletColor != null)
                _spriteRenderer.color = bulletColor.Value;
            if (trailData != null)
            {
                if (trailData.startColor != null)
                    _trailRenderer.startColor = trailData.startColor.Value;
                if (trailData.endColor != null)
                    _trailRenderer.endColor = trailData.endColor.Value;
                if (trailData.width != -1)
                    _trailRenderer.widthMultiplier = trailData.width;
                if (trailData.time != -1)
                    _trailRenderer.time = trailData.time;
            }
            _trailRenderer.widthMultiplier *= scaleMultiplier;
            transform.localScale = _defaultScale * scaleMultiplier;
        }


        #region Pooling
        public override void OnPop()
        {
            IsDead = false;
            StageGenerator.Instance.OnNextStageEvent += HandleNextStage;
        }

        private void HandleNextStage()
        {
            this.Push();
        }

        public override void OnPush()
        {
            StageGenerator.Instance.OnNextStageEvent -= HandleNextStage;
        }
        #endregion
    }
}
