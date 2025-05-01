using Hashira.Core.EventSystem;
using Hashira.Core.StatSystem;
using Hashira.Entities;
using System;
using UnityEngine;

namespace Hashira.Cards.Effects
{
    public class MagicStoneCard : CardEffect
    {
        private StatDictionary _statDictionary;
        private StatElement _attackPowerStat;
        private StatElement _attackSpeedStat;

        public override void Enable()
        {
            foreach (CardEffect cardEffect in PlayerDataManager.Instance.CardEffectList)
            {
                if (cardEffect is MagicCardEffect magicCardEffect)
                {
                    magicCardEffect.SetMultiplier(0.7f);
                }
            }

            _statDictionary = player.GetEntityComponent<EntityStat>().StatDictionary;
            _attackPowerStat = _statDictionary[StatName.AttackPower];
            _attackSpeedStat = _statDictionary[StatName.AttackSpeed];
            _attackPowerStat.AddModify("MagicStoneCard", -30f, EModifyMode.Percent, EModifyLayer.Default);
            _attackSpeedStat.AddModify("MagicStoneCard", -20f, EModifyMode.Percent, EModifyLayer.Default);

            GameEventChannel.AddListener<ProjectileAfterHitEvent>(HandleHitEvent);
        }

        private void HandleHitEvent(ProjectileAfterHitEvent projectileHitEvent)
        {
            if (projectileHitEvent.hitInfo.damageable != null)
            {
                foreach (CardEffect cardEffect in PlayerDataManager.Instance.CardEffectList)
                {
                    if (cardEffect is MagicCardEffect magicCardEffect)
                    {
                        magicCardEffect.DelayDown(5f, EModifyMode.Percent);
                    }
                }
            }
        }

        public override void Disable()
        {
            GameEventChannel.RemoveListener<ProjectileAfterHitEvent>(HandleHitEvent);
            _attackPowerStat.RemoveModifyOverlap("MagicStoneCard", EModifyLayer.Default);
            _attackSpeedStat.RemoveModifyOverlap("MagicStoneCard", EModifyLayer.Default);
            foreach (CardEffect cardEffect in PlayerDataManager.Instance.CardEffectList)
            {
                if (cardEffect is MagicCardEffect magicCardEffect)
                {
                    magicCardEffect.SetMultiplier(1f);
                }
            }
        }

        public override void Update()
        {

        }
    }
}
