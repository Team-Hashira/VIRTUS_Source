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

        private List<CardEffect> _cardEffectList = new();

        public event Action<bool> OnCardEffectEnableEvent;

        public void SetCardEffectList(List<CardEffect> cardEffects, bool isEventSender = false)
        {
            _cardEffectList = cardEffects;
            for (int i = 0; i < _cardEffectList.Count; i++)
            {
                _cardEffectList[i].player = Player;
                _cardEffectList[i].Enable();
            }

            if (isEventSender)
                OnCardEffectEnableEvent?.Invoke(false);
        }

        public void ReEnableCardEffect()
        {
            for (int i = 0; i < _cardEffectList.Count; i++)
            {
                _cardEffectList[i].Disable();
                _cardEffectList[i].Enable();
            }
            OnCardEffectEnableEvent?.Invoke(true);
        }

        private void Update()
        {
            for (int i = 0; i < _cardEffectList.Count; i++)
            {
                _cardEffectList[i].Update();
            }
        }

        private void OnDisable()
        {
            for (int i = 0; i < _cardEffectList.Count; i++)
            {
                _cardEffectList[i].Disable();
            }
        }
    }
}
