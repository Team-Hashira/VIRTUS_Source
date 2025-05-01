using DG.Tweening;
using Hashira.Cards.Effects;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Hashira.CanvasUI.CardUseScene
{
    public class PlayerHealButton : UIBase, IClickableUI
    {
        [SerializeField] private int _HealNeedCost = 3;

        [SerializeField] private TextMeshProUGUI _rerollCostText, _playerHealthText;

        private void Start()
        {
            _rerollCostText.text = $"{_HealNeedCost}";
            PlayerDataManager.Instance.EffectAddedEvent += HandleEffectAddedEvent;
            PlayerHealthUpdate();
        }

        private void HandleEffectAddedEvent(CardEffect effect)
        {
            if (effect is HealthStatCard healthStatCard)
            {
                int addedHealth = 1 + healthStatCard.stack;
                addedHealth = PlayerDataManager.Instance.DefaultMaxHealth + addedHealth - PlayerDataManager.Instance.MaxHealth;
                PlayerDataManager.Instance.SetHealth(
                    PlayerDataManager.Instance.Health + addedHealth,
                    PlayerDataManager.Instance.MaxHealth + addedHealth);
                PlayerHealthUpdate();
            }
        }

        public void PlayerHealthUpdate()
        {
            _playerHealthText.text = $"{PlayerDataManager.Instance.Health}/{PlayerDataManager.Instance.MaxHealth}";
        }

        public void OnClick(bool isLeft)
        {
            if (isLeft == false) return;

            int maxHealth = PlayerDataManager.Instance.MaxHealth;
            int health = PlayerDataManager.Instance.Health;
            if (health >= maxHealth)
            {
                PopupTextManager.Instance.PopupText("최대 체력입니다.", Color.white);
            }
            else if (Cost.TryRemoveCost(_HealNeedCost))
            {
                health++;
                PlayerDataManager.Instance.SetHealth(health, maxHealth);
                PlayerHealthUpdate();
            }
            else
            {
                PopupTextManager.Instance.PopupText("코스트가 부족합니다.", Color.red);
            }
        }

        public void OnClickEnd(bool isLeft)
        {

        }

        private void OnDestroy()
        {
            if (PlayerDataManager.Instance != null) PlayerDataManager.Instance.EffectAddedEvent -= HandleEffectAddedEvent;
        }
    }
}
