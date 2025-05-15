using Crogen.CrogenPooling;
using Hashira.Core.EventSystem;
using Hashira.Core.StatSystem;
using Hashira.Entities;
using Hashira.Players;
using Hashira.Projectiles;
using Hashira.StageSystem;
using Hashira.VFX;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Hashira.Accessories.Effects
{
    public class PrecisionScrollPassive : AccessoryPassiveEffect, IUpdatableEffect
    {
        [SerializeField]
        [Tooltip("높아질수록 경로가 더욱 세세히 그려짐")]
        private int _resolution = 32;

        private EntityStat _ownerStat;
        private Attacker _attacker;
        private ParabolaDrawer _drawer;

        public override void Initialize(Entity owner)
        {
            base.Initialize(owner);
            _ownerStat = owner.GetEntityComponent<EntityStat>();
            _attacker = (owner as Player)?.Attacker;
            _drawer = PopCore.Pop(EffectPoolType.PrecisionParabolaDrawer, _attacker.transform).gameObject.GetComponent<ParabolaDrawer>();
        }

        public void OnUpdate()
        {
            _drawer.DrawLine(
                _attacker.transform.position,
                _attacker.transform.eulerAngles.z * Mathf.Deg2Rad,
                _ownerStat.StatDictionary[StatName.ProjectileSpeed].Value,
                _owner.transform.up * Physics2D.gravity * 25f * 0.5f,
                _resolution);
        }
    }

    public class PrecisionScrollActive : AccessoryActiveEffect, IUpdatableEffect, IInitializeOnNextStage
    {
        private Player _player;

        [SerializeField]
        [Tooltip("몇발까지 유도할것인가")]
        private int _maxBulletCount = 10;
        private int _currentBulletCount;
        [SerializeField]
        [Tooltip("한 스테이지당 몇번 사용")]
        private int _useableCount = 1;
        private int _useableCounter;

        private EntityHealth _closesetTarget;
        private Dictionary<Projectile, EntityHealth> _targetDictionary;
        private PrecisionScrollActiveVFX _activeVFX;

        public PrecisionScrollActive()
        {
            _targetDictionary = new Dictionary<Projectile, EntityHealth>();
        }

        public override void Initialize(Entity owner)
        {
            base.Initialize(owner);
            _player = owner as Player;
            GameEventChannel.RemoveListener<ProjectileAfterHitEvent>(HandleHitEvent);
            GameEventChannel.RemoveListener<ProjectileShootEvent>(HandleShootEvent);
            _closesetTarget = null;
            _activeVFX = PopCore.Pop(EffectPoolType.PrecisionScrollActiveVFX) as PrecisionScrollActiveVFX;
            _activeVFX.Initialize(_player);
            _activeVFX.SetActive(false);
        }

        private void HandleShootEvent(ProjectileShootEvent evt)
        {
            if (!TryCaptureClosestTarget())
                return;
            _currentBulletCount--;
            if (_currentBulletCount <= 0)
            {
                GameEventChannel.RemoveListener<ProjectileShootEvent>(HandleShootEvent);
                _activeVFX.SetActive(false);
            }
            evt.projectile.IsPenetrateWall = true;
            evt.projectile.UseGravity = false;
            _targetDictionary.Add(evt.projectile, _closesetTarget);
        }

        private void HandleHitEvent(ProjectileAfterHitEvent evt)
        {
            if (_targetDictionary.TryGetValue(evt.projectile, out EntityHealth health))
            {
                if (evt.hitInfo.damageable != (IDamageable)health)
                {
                    evt.projectile.SetLifeCount(EProjectileUndyingMode.Penetration, 1);
                }
                else
                {
                    evt.projectile.IsPenetrateWall = false;
                    evt.projectile.UseGravity = true;
                    _targetDictionary.Remove(evt.projectile);
                }
            }
        }

        public override void OnActivate()
        {
            if (_useableCounter > 0 && _currentBulletCount == 0)
            {
                _useableCounter--;
                _currentBulletCount = _maxBulletCount;
                GameEventChannel.AddListener<ProjectileAfterHitEvent>(HandleHitEvent);
                GameEventChannel.AddListener<ProjectileShootEvent>(HandleShootEvent);
            }
        }

        public void OnUpdate()
        {
            foreach (var projectile in _targetDictionary.Keys)
            {
                EntityHealth targetHealth = _targetDictionary[projectile];
                if (targetHealth == null)
                {
                    projectile.IsPenetrateWall = false;
                    projectile.UseGravity = true;
                    _targetDictionary.Remove(projectile);
                    continue;
                }

                Transform target = targetHealth.transform;

                Vector3 targetDir = (target.position - projectile.transform.position).normalized;
                Vector3 newDir = Vector3.Slerp(projectile.transform.forward, targetDir, Time.deltaTime * projectile.Speed);
                projectile.Redirection(newDir);
            }

            if (_closesetTarget == null)
                _activeVFX.SetActive(false);
            if (_currentBulletCount > 0)
            {
                if (!TryCaptureClosestTarget())
                    return;
                _activeVFX.SetActive(true);
                _activeVFX.UpdateTarget(_closesetTarget.transform);
            }

        }

        public void OnNextStage()
        {
            _currentBulletCount = 0;
            _useableCounter = _useableCount;
        }

        private bool TryCaptureClosestTarget()
        {
            Vector3 curMousePos = Camera.main.ScreenToWorldPoint(_player.InputReader.MousePosition);
            _closesetTarget =
                StageGenerator.Instance.GetCurrentStage().GetEnabledEnemies().OrderBy(enemy => (enemy.transform.position - curMousePos).sqrMagnitude).FirstOrDefault()?.GetEntityComponent<EntityHealth>();
            return _closesetTarget != null;
        }
    }
}
