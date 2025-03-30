using Crogen.CrogenPooling;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Hashira
{
    public class ElectricZone : PushLifetime
    {
        [Header("===========ElectricZone===========")]
        [SerializeField] private DamageCaster2D _damageCaster;
        [SerializeField] private Light2D _light2D;
        [SerializeField] private float _delay;
        [SerializeField] private int _damage;
        private float _intensity;

        private void Awake()
        {
            _intensity = _light2D.intensity;
        }

        public override void OnPop()
        {
            base.OnPop();
            _light2D.intensity = _intensity;
        }

        private void Update()
        {
            if (CooldownUtillity.CheckCooldown("ElectricZoneDelay", _delay, true))
            {
                _damageCaster.CastDamage(_damage, attackType: Entities.EAttackType.Electricity);
                CooldownUtillity.StartCooldown("ElectricZoneDelay");
            }

            if (_isDead)
            {
                _light2D.intensity -= _intensity * Time.deltaTime * 0.2f;
            }
        }
    }
}
