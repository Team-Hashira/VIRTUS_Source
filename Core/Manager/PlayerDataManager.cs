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
    public class PlayerDataManager : DontDestroyMonoSingleton<PlayerDataManager>
    {
        public Dictionary<CardSO, CardEffect> CardEffectDictionary { get; private set; } = new Dictionary<CardSO, CardEffect>();

        public event Action<CardEffect> EffectAddedEvent;
        public event Action<CardEffect> EffectRemovedEvent;

        public int Health { get; private set; } = 6;
        public int MaxHealth { get; private set; } = 6;
        public int DefaultMaxHealth { get; private set; } = 6;

        public int KillCount { get; private set; }
        public int BossKillCount { get; private set; }

        [SerializeField]
        private AccessorySetSO _allAccessorySet;

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

        /// <summary>
        /// 해당 카드의 효과 stack이 Max인지 검사합니다.
        /// </summary>
        /// <param name="cardSO"></param>
        /// <returns></returns>
        public bool IsMaxStackEffect(CardSO cardSO)
        {
            if (cardSO.maxOverlapCount < 0) return false;

            if (CardEffectDictionary.TryGetValue(cardSO, out CardEffect cardEffect))
                return cardSO.maxOverlapCount == cardEffect.stack;
            else
                return false;
        }

        /// <summary>
        /// CardEffect를 하나 추가합니다.
        /// </summary>
        /// <param name="cardEffect"></param>
        public void AddEffect(CardSO cardSO)
        {
            if (IsMaxStackEffect(cardSO)) return;

            if (CardEffectDictionary.TryGetValue(cardSO, out CardEffect cardEffect) == false)
            {
                cardEffect = cardSO.GetEffectInstance<CardEffect>();
                CardEffectDictionary.Add(cardSO, cardEffect);
            }

            cardEffect.stack++;

            if (PlayerManager.Instance != null)
            {
                cardEffect.Disable();
                cardEffect.Enable();
            }

            EffectAddedEvent?.Invoke(cardEffect);
        }

        /// <summary>
        /// CardEffect를 하나 제거합니다.
        /// </summary>
        /// <param name="cardEffect"></param>
        public void RemoveEffect(CardSO cardSO)
        {
            if (CardEffectDictionary.TryGetValue(cardSO, out CardEffect cardEffect))
            {
                cardEffect.stack--;

                if (PlayerManager.Instance != null)
                {
                    cardEffect.Disable();
                    cardEffect.Enable();
                }

                if (cardEffect.stack == 0)
                    CardEffectDictionary.Remove(cardSO);
            }

            EffectRemovedEvent?.Invoke(cardEffect);
        }

        /// <summary>
        /// 카드의 stack을 바꾸고 재실행해서 적용합니다.
        /// </summary>
        /// <param name="cardSO"></param>
        /// <param name="stack"></param>
        public void SetEffectStack(CardSO cardSO, int stack)
        {
            CardEffect cardEffect = CardEffectDictionary[cardSO];
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

        /// <summary>
        /// Card의 설명을 가져옵니다.
        /// </summary>
        /// <param name="cardSO"></param>
        /// <returns></returns>
        public string GetCardDescription(CardSO cardSO)
        {
            if (CardEffectDictionary.TryGetValue(cardSO, out CardEffect cardEffect))
                return cardEffect.GetCardDescription();
            else
                return cardSO.cardDescriptions[0];
        }

        /// <summary>
        /// Card의 중첩 수를 가져옵니다.
        /// </summary>
        /// <param name="cardSO"></param>
        /// <returns></returns>
        public int GetCardStack(CardSO cardSO)
        {
            if (CardEffectDictionary.TryGetValue(cardSO, out CardEffect cardEffect))
                return cardEffect.stack;
            else
                return 0;
        }

        /// <summary>
        /// Card 강화에 필요한 Cost를 반환합니다.
        /// </summary>
        /// <param name="cardSO"></param>
        /// <returns></returns>
        public int GetCardNeedCost(CardSO cardSO)
        {
            if (IsMaxStackEffect(cardSO))
                return 0;
            else
            {
                if (CardEffectDictionary.TryGetValue(cardSO, out CardEffect cardEffect))
                    return cardSO.needCost[cardEffect.stack];
                else
                    return cardSO.needCost[0];
            }
        }

        /// <summary>
        /// 플레이어의 모든 데이터를 초기화합니다.
        /// </summary>
        public void ResetData()
        {
            SetHealth(DefaultMaxHealth, DefaultMaxHealth);
            KillCount = 0;
            BossKillCount = 0;
            CardManager.Instance.ClearCardList();
            Cost.ResetCost();
            Accessory.ResetAccessory();
            StageGenerator.ResetStage();
            _allAccessorySet.ResetAll();
            ResetCardEffectList(null);
        }

        /// <summary>
        /// 카드효과 리스트 초기화
        /// </summary>
        /// <param name="exceptionCardSO"></param>
        public void ResetCardEffectList(List<CardSO> exceptionCardSO)
        {
            if (exceptionCardSO == null)
            {
                foreach (CardSO cardSO in CardEffectDictionary.Keys.ToList())
                {
                    CardEffectDictionary[cardSO].stack = 0;
                }
                CardEffectDictionary = new Dictionary<CardSO, CardEffect>();
            }
            else
            {
                foreach (CardSO cardSO in CardEffectDictionary.Keys.Except(exceptionCardSO))
                {
                    CardEffectDictionary[cardSO].stack = 0;
                    CardEffectDictionary.Remove(cardSO);
                }
            }
        }
        /// <summary>
        /// 모든 카드 해제
        /// </summary>
        public void CardDisable()
        {
            foreach (CardEffect cardEffect in CardEffectDictionary.Values)
            {
                cardEffect.Disable();
            }
        }
    }
}
