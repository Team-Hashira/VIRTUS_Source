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
    public class PlayerDataManager : DontDestroyMonoSingleton<PlayerDataManager>
    {
        public List<CardEffect> CardEffectList { get; private set; } = new List<CardEffect>();

        public event Action<CardEffect> EffectAddedEvent;
        public event Action<CardEffect> EffectRemovedEvent;

        public int Health { get; private set; } = 6;
        public int MaxHealth { get; private set; } = 6;
        public int DefaultMaxHealth { get; private set; } = 6;

        public int KillCount { get; private set; }
        public int BossKillCount { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            SetHealth(DefaultMaxHealth, DefaultMaxHealth);
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

            if (PlayerManager.Instance != null)
            {
                cardEffect.Disable();
                cardEffect.Enable();
            }

            EffectAddedEvent?.Invoke(cardEffect);
        }

        public void RemoveEffect(CardEffect cardEffect)
        {
            if (CardEffectList.Contains(cardEffect))
            {
                cardEffect.stack--;

                if (PlayerManager.Instance != null)
                {
                    cardEffect.Disable();
                    cardEffect.Enable();
                }

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

                if (PlayerManager.Instance != null)
                {
                    cardEffect.Disable();
                    cardEffect.Enable();
                }
            }
            else
                Debug.Log($"{cardSO.className} was not found");
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

        public int GetCardNeedCost(CardSO cardSO)
        {
            foreach (CardEffect cardEffect in CardEffectList)
            {
                if (cardEffect.CardSO == cardSO)
                {
                    if (cardEffect.IsMaxStack == false)
                        return cardSO.needCost[cardEffect.stack];
                    else
                        return 0;
                }
            }
            return cardSO.needCost[0];
        }

        public void ResetData()
        {
            SetHealth(DefaultMaxHealth, DefaultMaxHealth);
            ResetPlayerData();
            CardManager.Instance.ClearCardList();
            Cost.ResetCost();
            Accessory.ResetAccessory();
            StageGenerator.ResetStage();
        }
        public void ResetPlayerData()
        {
            KillCount = 0;
            BossKillCount = 0;

            //bool isInGame = PlayerManager.Instance != null;

            //if (isInGame)
            //    PlayerManager.Instance.SetCardEffectList(CardEffectList);
        }
        public void ResetPlayerCardEffect(List<CardSO> exceptionCardSO = null, bool useDisable = false)
        {
            if (useDisable)
            {
                foreach (CardEffect cardEffect in CardEffectList)
                {
                    cardEffect.Disable();
                    cardEffect.stack = 0;
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
                    else
                        cardEffect.stack = 0;
                }

                CardEffectList = new List<CardEffect>();

                foreach (CardEffect cardEffect in exceptionCardEffectList)
                {
                    CardEffectList.Add(cardEffect);
                }
            }
        }
        public void CardDisable()
        {
            foreach (CardEffect cardEffect in CardEffectList)
            {
                cardEffect.Disable();
            }
        }
    }
}
