using Hashira.Core;
using UnityEngine;

namespace Hashira.CanvasUI.Option
{
    public class InGamePopupUIController : MonoBehaviour
    {
        [SerializeField] private InputReaderSO _inputReader;
        [SerializeField] private PauseMenuPanel _pausePanel;
        [SerializeField] private OptionPanel _optionPanel;

        private void Awake()
        {
            _inputReader.OnPauseEvent += HandlePauseEvent;
        }

        private void HandlePauseEvent()
        {
            if (GameManager.Instance != null && GameManager.Instance.IsGameOver) return;

            if (_pausePanel.IsOpened)
            {
                if (_optionPanel.IsOpened)
                {
                    _optionPanel.Close();
                    _optionPanel.SaveData();
                }
                else
                {
                    _pausePanel.Close();
                }
            }
            else
            {
                _pausePanel.Open();
            }
        }

        private void OnDestroy()
        {
            _inputReader.OnPauseEvent -= HandlePauseEvent;
        }
    }
}
