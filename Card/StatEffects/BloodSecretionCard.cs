using Hashira.Core.EventSystem;
using Hashira.Core.StatSystem;
using Hashira.Entities;
using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace Hashira.Cards.Effects
{
    public class BloodSecretionCard : CardEffect
    {
        [SerializeField] private readonly float[] _damageUpValue = { 8, 16, 30 };

        private StatElement _attackPowerStat;

        private int _hitStack;

        public override void Enable()
        {
            base.Enable();
            _attackPowerStat = player.GetEntityComponent<EntityStat>().StatDictionary[StatName.AttackPower];

            GameEventChannel.AddListener<PlayerHitEvent>(HandlePlayerHitEvent);
        }

        private void HandlePlayerHitEvent(PlayerHitEvent playerHitEvent)
        {
            _hitStack++;
            _hitStack %= 4;
            SetStat();
        }

        private void SetStat()
        {
            _attackPowerStat.RemoveModify(nameof(BloodSecretionCard), EModifyLayer.Default);
            if (_hitStack > 0)
            {
                _attackPowerStat.AddModify(nameof(BloodSecretionCard), _damageUpValue[_hitStack - 1], EModifyMode.Percent, EModifyLayer.Default, false);
            }
        }

        public override void Disable()
        {
            base.Disable();
            GameEventChannel.RemoveListener<PlayerHitEvent>(HandlePlayerHitEvent);
        }

        public override void Reset()
        {
            _hitStack = 0;
            SetStat();
        }
    }
}
