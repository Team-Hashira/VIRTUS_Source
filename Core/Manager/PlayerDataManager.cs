using Hashira.Accessories;
using Hashira.Cards;
using Hashira.Cards.Effects;
using Hashira.Core;
using Hashira.EffectSystem;
using Hashira.StageSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Hashira
{
    public class PlayerDataManager : DonDestroyMonoSingleton<PlayerDataManager>
    {
        public List<CardEffect> CardEffectList { get; private set; } = new List<CardEffect>();

        public event Action<CardEffect> EffectAddedEvent;
        public event Action<CardEffect> EffectRemovedEvent;

        public int Health { get; private set; } = 6;
        public int MaxHealth { get; private set; } = 6;

        public int KillCount { get; private set; }
        public int BossKillCount { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            SetHealth(6, 6);
        }

        public void AddKillCount(bool isBoss = false)
        {
            if (isBoss)
                BossKillCount++;
            else
                KillCount++;
        }

        public void SetHealth(int health, int maxHealth)
        {
            Health = health;
            MaxHealth = maxHealth;
        }

        public bool IsMaxStackEffect(CardSO cardSO)
        {
            if (cardSO.maxOverlapCount < 0) return false;
            CardEffect cardEffect = CardEffectList.Find(effect => effect.CardSO == cardSO);
            if (cardEffect != null)
                return cardSO.maxOverlapCount == cardEffect.stack;
            else
                return false;
        }

        public void AddEffect(CardEffect cardEffect)
        {
            if (cardEffect == null) return;
            if (IsMaxStackEffect(cardEffect.CardSO)) return;

            if (CardEffectList.Contains(cardEffect) == false)
                CardEffectList.Add(cardEffect);
            cardEffect.stack++;

            EffectAddedEvent?.Invoke(cardEffect);
        }

        public void RemoveEffect(CardEffect cardEffect)
        {
            if (CardEffectList.Contains(cardEffect))
            {
                cardEffect.stack--;
                if (cardEffect.stack == 0)
                    CardEffectList.Remove(cardEffect);
            }
                
            EffectRemovedEvent?.Invoke(cardEffect);
        }

        public void SetEffectStack(CardSO cardSO, int stack)
        {
            CardEffect cardEffect = CardEffectList.Find(effect => effect.CardSO == cardSO);
            if (cardSO != null && cardEffect != null)
            {
                cardEffect.stack = stack;
            }
            else
                Debug.Log($"{cardSO.className} was not found");
        }

        public int GetAdditionalNeedCost(CardSO cardSO)
        {
            foreach (CardEffect cardEffect in CardEffectList)
            {
                if (cardEffect.CardSO == cardSO)
                {
                    return cardEffect.GetAdditionalNeedCost();
                }
            }
            return 0;
        }

        public string GetCardDescription(CardSO cardSO)
        {
            foreach (CardEffect cardEffect in CardEffectList)
            {
                if (cardEffect.CardSO == cardSO)
                {
                    return cardEffect.GetCardDescription();
                }
            }
            return cardSO.cardDescriptions[0];
        }

        public int GetCardStack(CardSO cardSO)
        {
            foreach (CardEffect cardEffect in CardEffectList)
            {
                if (cardEffect.CardSO == cardSO)
                {
                    return cardEffect.stack;
                }
            }
            return 0;
        }

        public void ResetData()
        {
            SetHealth(6, 6);
            ResetPlayerData();
            CardManager.Instance.ClearCardList();
            Cost.ResetCost();
            Accessory.ResetAccessory();
            StageGenerator.ResetStage();
        }
        public void ResetPlayerData()
        {
            KillCount = 0;

            bool isInGame = PlayerManager.Instance != null;

            if (isInGame)
                PlayerManager.Instance.SetCardEffectList(CardEffectList);
        }
        public void ResetPlayerCardEffect(List<CardSO> exceptionCardSO = null, bool useDisable = false)
        {
            if (useDisable)
            {
                foreach (CardEffect cardEffect in CardEffectList)
                {
                    cardEffect.Disable();
                }
            }

            if (exceptionCardSO == null)
                CardEffectList = new List<CardEffect>();
            else
            {
                List<CardEffect> exceptionCardEffectList = new List<CardEffect>();
                foreach (CardEffect cardEffect in CardEffectList)
                {
                    if (exceptionCardSO.Contains(cardEffect.CardSO))
                        exceptionCardEffectList.Add(cardEffect);
                }

                CardEffectList = new List<CardEffect>();

                foreach (CardEffect cardEffect in exceptionCardEffectList)
                {
                    CardEffectList.Add(cardEffect);
                }
            }
        }
    }
}
