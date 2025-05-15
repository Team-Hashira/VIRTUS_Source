using Hashira.Core;
using Hashira.Entities;
using Hashira.Players;
using System;
using System.Collections.Generic;
using System.Linq;
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
        }

        private void Start()
        {
            PlayerManager.Instance.OnCardEffectEnableEvent += HandleCardEffectEnableEvent;
            _playerHealth = _player.GetEntityComponent<EntityHealth>();
            _playerHealth.OnHealthChangedEvent += HandleHealthChangedEvent;
            HandleCardEffectEnableEvent(false);
        }

        // Start에서 실행
        private void HandleCardEffectEnableEvent(bool isReEnable)
        {
            SetMaxHealth(_playerHealth.MaxHealth, isReEnable);
        }

        public void HandleHealthChangedEvent(int prev, int current)
        {
            SetHealthImage(current, current < prev);
        }

        private void SetHealthImage(int health, bool isAnimation)
        {
            for (int i = 0; i < _healthImageList.Count; i++)
            {
                _healthImageList[i]?.SetLevel(Mathf.Clamp(health - i * 2, 0, 2), isAnimation);
            }
        }

        public void SetMaxHealth(int maxHealth, bool isReEnable)
        {
            List<PlayerHealthUI> playerHealthUI = _healthImageList.ToList();
            foreach (PlayerHealthUI healthUI in playerHealthUI)
            {
                Destroy(healthUI.gameObject);
            }
            _healthImageList.Clear();

            while (_healthImageList.Count * 2 < maxHealth)
            {
                PlayerHealthUI healthImage = Instantiate(_healthUI, transform);
                _healthImageList.Add(healthImage);
            }

            if (isReEnable == false)
            {
                int addedMaxHealth = Mathf.Clamp(maxHealth - PlayerDataManager.Instance.MaxHealth, 0, int.MaxValue);
                _playerHealth.ApplyRecovery(addedMaxHealth);
            }

            SetHealthImage(_playerHealth.Health, false);
        }

        private void OnDestroy()
        {
            if (_playerHealth != null)
                _playerHealth.OnHealthChangedEvent -= HandleHealthChangedEvent;
            if (PlayerManager.Instance) PlayerManager.Instance.OnCardEffectEnableEvent -= HandleCardEffectEnableEvent;
        }
    }
}
