using Hashira.Core;
using Hashira.Players;
using UnityEngine;
using UnityEngine.UI;

namespace Hashira.CanvasUI
{
    public class StaminaUI : MonoBehaviour
    {
        [SerializeField] private Image _staminaBar;

        private Player _player;

        private void Awake()
        {
            _player = PlayerManager.Instance.Player;
            _player.OnStaminaChangedEvent += HandleStaminaChangedEvent;
            HandleStaminaChangedEvent(_player.MaxStamina, _player.MaxStamina);
        }

        private void HandleStaminaChangedEvent(int prevValue, int newValue)
        {
            _staminaBar.fillAmount = (float)newValue / _player.MaxStamina;
        }

        private void OnDestroy()
        {
            _player.OnStaminaChangedEvent -= HandleStaminaChangedEvent;
        }
    }
}
