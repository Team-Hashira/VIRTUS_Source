using Hashira.Core;
using Hashira.Players;
using Hashira.StageSystem;
using UnityEngine;
using UnityEngine.UI;

namespace Hashira.CanvasUI
{
    public class StaminaUI : MonoBehaviour
    {
        [SerializeField] private Image _staminaBar;

        private Player _player;
        private StageGenerator _stageGenerator;
        private readonly int _valueID = Shader.PropertyToID("_Value");
        private readonly int _splitID = Shader.PropertyToID("_Split");
        
        private void Start()
        {
            _player = PlayerManager.Instance.Player;
            _stageGenerator = StageGenerator.Instance; 
            _player.OnStaminaChangedEvent += HandleStaminaChangedEvent;
            _stageGenerator.OnGeneratedStageEvent += HandleUpdateStageData;
            _staminaBar.material.SetFloat(_splitID, _player.MaxStamina);
            HandleStaminaChangedEvent(_player.CurrentStamina, _player.MaxStamina);
        }

        private void HandleUpdateStageData() => HandleStaminaChangedEvent(_player.CurrentStamina, _player.MaxStamina);

        private void HandleStaminaChangedEvent(int prevValue, int newValue)
        {
            _staminaBar.material.SetFloat(_splitID, _player.MaxStamina);
            _staminaBar.material.SetFloat(_valueID, (float)newValue / _player.MaxStamina);
        }

        private void OnDestroy()
        {
            _player.OnStaminaChangedEvent -= HandleStaminaChangedEvent;
            _stageGenerator.OnGeneratedStageEvent -= HandleUpdateStageData;
        }
    }
}
