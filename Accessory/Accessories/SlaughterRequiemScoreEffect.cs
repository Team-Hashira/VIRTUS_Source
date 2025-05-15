using Crogen.CrogenPooling;
using Hashira.Combat;
using Hashira.Core.EventSystem;
using Hashira.Core.StatSystem;
using Hashira.EffectSystem;
using Hashira.EffectSystem.Effects;
using Hashira.Entities;
using Hashira.Players;
using UnityEngine;
using static UnityEngine.ParticleSystem;

namespace Hashira.Accessories.Effects
{
    public class SlaughterRequiemScorePassive : AccessoryPassiveEffect, IInitializeOnNextStage, IUpdatableEffect
    {
        [SerializeField]
        [Tooltip("스택당 공격력 증가 퍼센트")]
        private float _increasePercent = 2;
        [SerializeField]
        [Tooltip("최대 스택")]
        private int _maxOverlapCount = 10;
        [SerializeField]
        [Tooltip("몇초마다 스택을 감소 시킬 것인지")]
        private float _passiveDuration = 2f;
        private ParticleSystem _particle;

        private EntityStat _ownerStat;

        private int _currentIncreaseCount = 0;
        private float _durationTimer = 0;

        private SpriteRenderer _lightRenderer;

        public override void Initialize(Entity owner)
        {
            base.Initialize(owner);
            GameEventChannel.RemoveListener<KillEnemyEvent>(HandleOnKillEnemyEvent); //Add해둔게 없으면 안빠짐. 솔직히 좀 구림
            GameEventChannel.AddListener<KillEnemyEvent>(HandleOnKillEnemyEvent);

            _ownerStat = owner.GetEntityComponent<EntityStat>();

            if (_particle == null)
            {
                _particle = PopCore.Pop(EffectPoolType.SlaughterRequiemEffect, owner.transform).gameObject.GetComponent<ParticleSystem>();
                _lightRenderer = _particle.transform.Find("Light").GetComponent<SpriteRenderer>();
            }

            _currentIncreaseCount = 0;
            _durationTimer = 0;
            UpdateVisual();
        }

        private void HandleOnKillEnemyEvent(KillEnemyEvent evt)
        {
            _durationTimer = 0;
            if (_currentIncreaseCount < _maxOverlapCount)
            {
                _currentIncreaseCount++;
                _ownerStat.StatDictionary[StatName.AttackPower].AddModify(nameof(SlaughterRequiemScorePassive), _increasePercent, EModifyMode.Percent, EModifyLayer.Default);
                UpdateVisual();
            }
        }

        public void OnUpdate()
        {
            if (_currentIncreaseCount > 0)
            {
                _durationTimer += Time.deltaTime;
                if (_durationTimer >= _passiveDuration)
                {
                    _durationTimer = 0;
                    _currentIncreaseCount--;
                    _ownerStat.StatDictionary[StatName.AttackPower].RemoveModifyOverlap(nameof(SlaughterRequiemScorePassive), EModifyLayer.Default);
                    UpdateVisual();
                }
            }
        }

        private void UpdateVisual()
        {
            _lightRenderer.color = new Color(_lightRenderer.color.r, _lightRenderer.color.g, _lightRenderer.color.b, 1);
            float ratio = _currentIncreaseCount / (float)_maxOverlapCount;
            Color transparent = new Color(1, 1, 1, Mathf.Lerp(0, 1f, ratio));
            _lightRenderer.color *= transparent;

            EmissionModule module = _particle.emission;
            module.rateOverTime = Mathf.Lerp(5f, 50f, ratio);
        }

        public override void OnUnequip()
        {
            _currentIncreaseCount = 0;
            _durationTimer = 0;
            _ownerStat.StatDictionary[StatName.AttackPower].RemoveModify(nameof(SlaughterRequiemScorePassive), EModifyLayer.Default);
            UpdateVisual();
        }

        public void OnNextStage()
        {
            _ownerStat.StatDictionary[StatName.AttackPower].RemoveModify(nameof(SlaughterRequiemScorePassive), EModifyLayer.Default);
            _currentIncreaseCount = 0;
            _durationTimer = 0;
            UpdateVisual();
        }
    }

    public class SlaughterRequiemScoreActive : AccessoryActiveEffect, IInitializeOnNextStage
    {
        [SerializeField]
        [Tooltip("사용할때 드는 체력")]
        private int _costHealth = 2;

        [SerializeField]
        [Tooltip("유지 시간")]
        private float _activeDuration = 10f;

        [SerializeField]
        [Tooltip("만약 피격당하지 않았다면 돌려받는 체력")]
        private int _refundAmount = 1;

        [SerializeField]
        [Tooltip("증가하는 스탯의 퍼센트")]
        private float _attackSpeedAmount = 20f, _damageAmount = 30f;

        [SerializeField]
        [Tooltip("스테이지당 사용 가능한 횟수")]
        private int _useableCount = 1;

        private int _useableCounter = 0;

        private bool _canRefund = false;

        private EntityHealth _ownerHealth;
        private EntityEffector _ownerEffector;
        private AttackPowerUp _attackPowerUp;
        private AttackSpeedUp _attackSpeedUp;

        private ParticleSystem _particle;
        private SpriteRenderer _lightRenderer;

        public SlaughterRequiemScoreActive()
        {
            _attackPowerUp = new AttackPowerUp();
            _attackSpeedUp = new AttackSpeedUp();
            _attackPowerUp.Setup(new StatModifier(_damageAmount, EModifyMode.Percent, true), _activeDuration, EModifyLayer.Last);
            _attackSpeedUp.Setup(new StatModifier(_attackSpeedAmount, EModifyMode.Percent, true), _activeDuration);

            _attackSpeedUp.OnTimeOutEvent += HandleOnTimeOut;
        }

        public override void Initialize(Entity owner)
        {
            base.Initialize(owner);
            _ownerEffector = owner.GetEntityComponent<PlayerEffector>();
            _ownerHealth = owner.GetEntityComponent<EntityHealth>();
            _ownerHealth.OnHitEvent += HandleOnHitEvent;
            if (_particle == null)
            {
                _particle = PopCore.Pop(EffectPoolType.SlaughterRequiemEffect, owner.transform).gameObject.GetComponent<ParticleSystem>();
                _lightRenderer = _particle.transform.Find("Light").GetComponent<SpriteRenderer>();
            }

        }

        public override void OnActivate()
        {
            if (_useableCounter < _useableCount && _ownerHealth.Health > _costHealth)
            {
                _ownerHealth.ApplyDamage(new AttackInfo(_costHealth));
                UpdateVisual(true);
                _ownerEffector.AddEffect(_attackSpeedUp);
                _ownerEffector.AddEffect(_attackPowerUp);

                _useableCounter++;
                _canRefund = true;
            }
        }

        private void HandleOnHitEvent(int hp)
        {
            _canRefund = false;
        }

        private void HandleOnTimeOut(Effect effect)
        {
            if (_canRefund)
            {
                _ownerHealth.ApplyRecovery(_refundAmount);
            }
            UpdateVisual(false);
        }

        private void UpdateVisual(bool isActive)
        {
            float multiplier = isActive ? 1f : 0f;
            _lightRenderer.color = new Color(_lightRenderer.color.r, _lightRenderer.color.g, _lightRenderer.color.b, 1 * multiplier);

            EmissionModule module = _particle.emission;
            module.rateOverTime = 50f * multiplier;
        }

        public void OnNextStage()
        {
            _useableCounter = 0;
            HandleOnTimeOut(null);
            _ownerEffector.RemoveEffect(_attackPowerUp);
            _ownerEffector.RemoveEffect(_attackSpeedUp);
        }
    }
}
