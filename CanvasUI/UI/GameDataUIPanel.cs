using Hashira.StageSystem;
using System;
using TMPro;
using UnityEngine;

namespace Hashira.CanvasUI
{
    public class GameDataUIPanel : UIBase, IToggleUI
    {
        [field: SerializeField] public string Key { get; set; }

        [SerializeField] private TextMeshProUGUI _stageText;

        private Stage _currentStage;

        private void Start()
        {
            Open();
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }

        public void Open()
        {
            gameObject.SetActive(true);
            StageGenerator.Instance.OnNextStageEvent += HandleNextStageEvent;
            _currentStage = StageGenerator.Instance.GetCurrentStage();
            if (_currentStage != null) _currentStage.OnWaveChanged += StageTextUpdate;
            StageTextUpdate();
        }

        private void HandleNextStageEvent()
        {
            _currentStage.OnWaveChanged -= StageTextUpdate;
            _currentStage = StageGenerator.Instance.GetCurrentStage();
            _currentStage.OnWaveChanged += StageTextUpdate;
        }

        private void StageTextUpdate()
        {
            bool isTutorial = StageGenerator.Instance.gameObject.activeSelf == false;
            string text;
            if (isTutorial)
            {
                text = "테스트 구역";
            }
            else
            {
                Stage stage = StageGenerator.Instance.GetCurrentStage();
                int waveCount = stage.waves.Length;
                text = $"{StageGenerator.currentFloorIdx + 1}층 {StageGenerator.currentStageIdx + 1}번 구역\n" +
                       $"{Mathf.Clamp(stage.CurrentWaveCount + 1, 0, waveCount)}/{waveCount}웨이브";
            }
            _stageText.text = text;
        }
    }
}
