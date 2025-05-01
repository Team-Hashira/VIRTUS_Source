using Crogen.CrogenPooling;
using Hashira.Core;
using Hashira.Enemies;
using Hashira.Entities;
using Hashira.Entities.Components;
using Hashira.Players;
using Hashira.Projectiles;
using Hashira.Projectiles.Player;
using Hashira.StageSystem;
using System;
using UnityEngine;
using PlayerBullet = Hashira.Projectiles.Player.PlayerBullet;

namespace Hashira
{
    public class SpiritFamiliar : MonoBehaviour, IPoolingObject
    {
        [SerializeField] private float _attackDelay;
        [SerializeField] private LayerMask _whatIsTarget;
        [SerializeField] private LayerMask _whatIsOnlyTarget;
        private float _lastAttackTime;
        private Player _player;
        private EntityRenderer _playerRenderer;
        private Func<int> _damageFunc;

        private int _index;
        private Vector3 _offset;
        private Vector3 _defaultScale;
        private bool _isElite;

        public string OriginPoolType { get; set; }
        GameObject IPoolingObject.gameObject { get; set; }

        private void Awake()
        {
            _defaultScale = transform.localScale;
        }

        public void Init(Func<int> damageFunc, int index, float delay, bool isElite)
        {
            _isElite = isElite;
            _index = index;
            _damageFunc = damageFunc;
            _lastAttackTime = Time.time + index * 0.4f;
            _player = PlayerManager.Instance.Player;
            _playerRenderer = _player.GetEntityComponent<EntityRenderer>();
            _offset = Quaternion.Euler(0, 0, -_index * 18f) * Vector3.left;
            if (isElite) transform.localScale = _defaultScale * 3;
            else transform.localScale = _defaultScale;
        }

        private void Update()
        {
            if (_lastAttackTime + _attackDelay * (_isElite ? 0.5f : 1f) < Time.time)
            {
                _lastAttackTime = Time.time;
                Attack();
            }

            Vector3 offset = _offset;
            offset.x *= _playerRenderer.FacingDirection;
            Vector3 targetPos = _player.transform.position;
            transform.position = Vector3.Lerp(transform.position, targetPos + offset, Time.deltaTime * 10f);
        }

        private void Attack()
        {
            Enemy nearEnemy = null;
            float minDistance = 1000000f;
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(_player.InputReader.MousePosition);
            mousePos.z = 0;
            Entity[] entities = StageGenerator.Instance.GetCurrentStage().GetEnabledEnemies();
            if (entities != null)
            {
                foreach (Enemy enemy in entities)
                {
                    float distance = Vector3.Distance(mousePos, enemy.transform.position);
                    if (minDistance > distance)
                    {
                        minDistance = distance;
                        nearEnemy = enemy;
                    }
                }
            }
            if (nearEnemy != null)
            {
                Vector3 nearEnemyPos;
                if (nearEnemy.TryGetEntityComponent(out EntityPartsCollider entityPartsCollider))
                    nearEnemyPos = entityPartsCollider.GetRandomCollider().transform.position;
                else
                    nearEnemyPos = nearEnemy.transform.position;
                Vector3 dir = (nearEnemyPos - transform.position).normalized;

                int damage = Mathf.CeilToInt(_damageFunc.Invoke() * (_isElite ? 1.5f : 1f));
                PlayerBullet PlayerBullet = PopCore.Pop(ProjectilePoolType.Bullet, transform.position, Quaternion.identity) as PlayerBullet;
                PlayerBullet.Init(_isElite ? _whatIsOnlyTarget : _whatIsTarget, dir, 30, damage, PlayerManager.Instance.Player.transform, false, 0);

                Color color = new Color(0.5f, 0.8f, 1f);
                TrailData trailData = new TrailData();
                trailData.startColor = color;
                trailData.endColor = color;
                PlayerBullet.SetVisual(color, trailData);
            }
        }

        public void OnPop()
        {

        }

        public void OnPush()
        {

        }
    }
}
