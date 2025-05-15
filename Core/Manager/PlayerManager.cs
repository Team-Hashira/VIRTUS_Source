using Hashira.Cards;
using Hashira.Cards.Effects;
using Hashira.Players;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Hashira.Core
{
    public class PlayerManager : MonoSingleton<PlayerManager>
    {
        private Player _player;
        public Player Player
        {
            get
            {
                if (_player == null)
                {
                    _player = FindFirstObjectByType<Player>();
                }
                return _player;
            }
        }

        private List<CardEffect> _cardEffectList;

        public event Action<bool> OnCardEffectEnableEvent;

        public void SetCardEffectList(List<CardEffect> cardEffectList, bool isEventSender)
        {
            _cardEffectList = cardEffectList;
            if (_cardEffectList != null)
            {
                foreach (var cardEffect in _cardEffectList)
                {
                    cardEffect.player = Player;
                    if (false == cardEffect.IsEnable) cardEffect.Enable();
                }
            }

            if (isEventSender)
                OnCardEffectEnableEvent?.Invoke(false);
        }

        public void ReEnableCardEffect()
        {
            if (_cardEffectList != null)
            {
                foreach (var cardEffect in _cardEffectList)
                {
                    cardEffect.Disable();
                    cardEffect.Enable();
                }
            }
            OnCardEffectEnableEvent?.Invoke(true);
        }

        private void Update()
        {
            if (_cardEffectList != null)
            {
                foreach (var cardEffect in _cardEffectList)
                {
                    cardEffect.Update();
                }
            }
        }

        private void OnDestroy()
        {
            if (_cardEffectList == null) return;

            foreach (var cardEffect in _cardEffectList)
            {
                cardEffect.Disable();
            }
        }
    }
}
