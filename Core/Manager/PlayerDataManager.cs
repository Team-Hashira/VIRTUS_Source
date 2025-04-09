using Hashira.Accessories;
using Hashira.Cards;
using Hashira.Cards.Effects;
using Hashira.Core;
using Hashira.StageSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Hashira
{
    public class PlayerDataManager : DonDestroyMonoSingleton<PlayerDataManager>
    {
        private Dictionary<Type, CardEffect> _cardEffectDictionary = new Dictionary<Type, CardEffect>();
        public List<CardEffect> CardEffectList => _cardEffectDictionary.Values.ToList();

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
            if (_cardEffectDictionary.TryGetValue(cardSO.GetEffectType(), out CardEffect effect))
                return cardSO.maxOverlapCount == effect.stack;
            else
                return false;
        }

        public void AddEffect(CardEffect cardEffect)
        {
            if (cardEffect == null) return;

            Type type = cardEffect.GetType();

            if (IsMaxStackEffect(cardEffect.CardSO)) return;

            if (_cardEffectDictionary.TryGetValue(type, out CardEffect effect))
                effect.stack += cardEffect.stack;
            else
                _cardEffectDictionary[type] = cardEffect;

            EffectAddedEvent?.Invoke(_cardEffectDictionary[type]);
        }

        public void RemoveEffect(CardEffect cardEffect)
        {
            Type type = cardEffect.GetType();

            if (cardEffect != null && _cardEffectDictionary.TryGetValue(type, out CardEffect effect))
            {
                effect.stack -= cardEffect.stack;
                if (effect.stack == 0)
                    _cardEffectDictionary.Remove(type);

                EffectRemovedEvent?.Invoke(effect);
            }
            else
                Debug.Log($"Effect {type.Name} was not found");
        }

        public void SetEffectStat(CardSO cardSO, int stack)
        {
            Type type = cardSO.GetEffectType();

            if (cardSO != null && _cardEffectDictionary.TryGetValue(type, out CardEffect effect))
            {
                effect.stack = stack;
            }
            else
                Debug.Log($"Effect {type.Name} was not found");
        }

        public int GetAdditionalNeedCost(CardSO cardSO)
        {
            foreach (CardEffect cardEffect in _cardEffectDictionary.Values)
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
            foreach (CardEffect cardEffect in _cardEffectDictionary.Values)
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
            foreach (CardEffect cardEffect in _cardEffectDictionary.Values)
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

            ResetPlayerCardEffect(useDisable: isInGame);

            if (isInGame)
                PlayerManager.Instance.SetCardEffectList(CardEffectList);
        }
        public void ResetPlayerCardEffect(List<CardSO> exceptionCardSO = null, bool useDisable = false)
        {
            if (useDisable)
            {
                foreach (CardEffect cardEffect in _cardEffectDictionary.Values)
                {
                    cardEffect.Disable();
                }
            }

            if (exceptionCardSO == null)
                _cardEffectDictionary = new Dictionary<Type, CardEffect>();
            else
            {
                List<CardEffect> exceptionCardEffectList = new List<CardEffect>();
                foreach (CardEffect cardEffect in _cardEffectDictionary.Values)
                {
                    if (exceptionCardSO.Contains(cardEffect.CardSO))
                        exceptionCardEffectList.Add(cardEffect);
                }

                _cardEffectDictionary = new Dictionary<Type, CardEffect>();

                foreach (CardEffect cardEffect in exceptionCardEffectList)
                {
                    _cardEffectDictionary.Add(cardEffect.GetType(), cardEffect);
                }
            }
        }
    }
}
