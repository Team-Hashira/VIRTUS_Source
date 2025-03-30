using Crogen.CrogenPooling;
using Hashira.Core.EventSystem;
using Hashira.Core.StatSystem;
using Hashira.EffectSystem.Effects;
using Hashira.Entities;
using Hashira.Projectiles;
using UnityEngine;

namespace Hashira.Cards.Effects
{
    public class BloodFrenzyCard : CardEffect
    {
        private int[] _needCostByStack = new int[] { 1, 2, 3 };
        protected override int[] _NeedCostByStack => _needCostByStack;

        private int _killCount;
        private int[] _needKillCountByStack = new int[] { 7, 6, 4, 4 };

        private bool _isBloodFrenzy;
        private const float _BloodFrenzyDuration = 10f;
        private float _lastEnableBloodFrenzyTime;

        private int[] _bleedingDamageByStack = new int[] { 5, 5, 5, 10 };

        private IPoolingObject _bloodFrenzyModeEffect;

        private StatElement _attackSpeedStat;
        private StatElement _speedStat;

        public override void Enable()
        {
            _killCount = 0;
            _isBloodFrenzy = false;
            _lastEnableBloodFrenzyTime = -100;

            GameEventChannel.AddListener<KillEnemyEvent>(HandleKillEnemyEvent);

            StatDictionary statDictionary = player.GetEntityComponent<EntityStat>().StatDictionary;
            _speedStat = statDictionary[StatName.Speed];
            _attackSpeedStat = statDictionary[StatName.AttackSpeed];
        }

        private void HandleKillEnemyEvent(KillEnemyEvent killEnemyEvent)
        {
            _killCount++;

            if (_killCount == _needKillCountByStack[stack - 1])
            {
                _killCount = 0;
                _lastEnableBloodFrenzyTime = Time.time;
            }
        }

        public override void Disable()
        {
            GameEventChannel.RemoveListener<KillEnemyEvent>(HandleKillEnemyEvent);

            if (_isBloodFrenzy)
                BloodFrenzyEnable(false);
        }

        public override void Update()
        {
            if (_isBloodFrenzy && _lastEnableBloodFrenzyTime + _BloodFrenzyDuration < Time.time)
            {
                // 피의 축제 시작
                _isBloodFrenzy = false;
                BloodFrenzyEnable(false);
            }
            else if (_isBloodFrenzy == false && _lastEnableBloodFrenzyTime + _BloodFrenzyDuration > Time.time)
            {
                // 피의 축제 끝
                _isBloodFrenzy = true;
                BloodFrenzyEnable(true);
            }
        }

        private void BloodFrenzyEnable(bool enable)
        {
            if (enable)
            {
                _speedStat.AddModify("BloodParty", 3f, EModifyMode.Percent, EModifyLayer.Default);
                _attackSpeedStat.AddModify("BloodParty", 20f, EModifyMode.Percent, EModifyLayer.Default);
                GameEventChannel.AddListener<ProjectileAfterHitEvent>(HandleHitEvent);
                GameEventChannel.AddListener<ProjectileShootEvent>(HandleShootEvent);
                _bloodFrenzyModeEffect = PopCore.Pop(EffectPoolType.BloodFrenzyMode, player.transform);
            }
            else
            {
                _speedStat.RemoveModify("BloodParty", EModifyLayer.Default);
                _attackSpeedStat.RemoveModify("BloodParty", EModifyLayer.Default);
                GameEventChannel.RemoveListener<ProjectileAfterHitEvent>(HandleHitEvent);
                GameEventChannel.RemoveListener<ProjectileShootEvent>(HandleShootEvent);
                _bloodFrenzyModeEffect.Push();
            }
        }

        private void HandleShootEvent(ProjectileShootEvent projectileShootEvent)
        {
            TrailData trailData = new TrailData()
            {
                startColor = Color.red,
                endColor = Color.red,
            };
            projectileShootEvent.projectile.SetVisual(Color.red, trailData);
        }

        private void HandleHitEvent(ProjectileAfterHitEvent projectileHitEvent)
        {
            RaycastHit2D raycastHit = projectileHitEvent.hitInfo.raycastHit;
            // Effect를 받을 수 있는 애만
            if (raycastHit.transform.TryGetComponent(out Entity entity))
            {
                if (entity.TryGetEntityComponent(out EntityEffector entityEffector))
                {
                    Bleeding bleeding = new Bleeding();
                    bleeding.Setup(_bleedingDamageByStack[stack - 1], 0.25f, 3.1f);
                    entityEffector.AddEffect(bleeding);

                    // 비주얼 효과
                    PopCore.Pop(EffectPoolType.BloodFrenzyHit, raycastHit.point, Quaternion.identity);
                    CameraManager.Instance.ShakeCamera(3, 3, 0.2f);
                }
            }
        }
    }
}
