using Hashira.Core;
using Hashira.Entities;
using Hashira.Players;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Hashira.CanvasUI
{
    public class PlayerHealthController : MonoBehaviour
    {
        [SerializeField] private PlayerHealthUI _healthUI;
        private EntityHealth _playerHealth; 
        private Player _player;

        private List<PlayerHealthUI> _healthImageList;

        private void Awake()
        {
            _healthImageList = new List<PlayerHealthUI>();
            _player = PlayerManager.Instance.Player;
            PlayerManager.Instance.OnCardEffectEnableEvent += HandleCardEffectEnableEvent;
        }

        private void HandleCardEffectEnableEvent()
        {
            _playerHealth = _player.GetEntityComponent<EntityHealth>();
            _playerHealth.OnHealthChangedEvent += HandleHealthChangedEvent;
            SetMaxHealth(_playerHealth.MaxHealth);
        }

        public void HandleHealthChangedEvent(int prev, int current)
        {
            SetHealthImage(current, true);
        }

        private void SetHealthImage(int health, bool isAnimation)
        {
            for (int i = 0; i < _healthImageList.Count; i++)
            {
                _healthImageList[i].SetLevel(Mathf.Clamp(health - i * 2, 0, 2), isAnimation);
            }
        }

        public void SetMaxHealth(int maxHealth)
        {
            while (_healthImageList.Count * 2 < maxHealth)
            {
                PlayerHealthUI healthImage = Instantiate(_healthUI, transform);
                _healthImageList.Add(healthImage);
            }
            int addedMaxHealth = Mathf.Clamp(maxHealth - PlayerDataManager.Instance.MaxHealth, 0, int.MaxValue);
            _playerHealth.ApplyRecovery(addedMaxHealth);
            SetHealthImage(_playerHealth.Health, false);
        }
    }
}
