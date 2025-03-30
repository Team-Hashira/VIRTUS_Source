using Hashira.Core;
using Hashira.Entities;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Hashira.UI
{
    public class ProfileContainer : MonoBehaviour
    {
        [SerializeField] private Image _healthImage;
        private EntityHealth _playerHealth;

        private void Start()
        {
            _playerHealth = PlayerManager.Instance.Player.GetEntityComponent<EntityHealth>();
            _playerHealth.OnHealthChangedEvent += HandleHealthChange;
        }

        private void OnDestroy()
        {
            _playerHealth.OnHealthChangedEvent -= HandleHealthChange;
        }

        private void HandleHealthChange(int oldValue, int newValue)
        {
            _healthImage.fillAmount = newValue/(float)_playerHealth.MaxHealth;
        }
    }
}
