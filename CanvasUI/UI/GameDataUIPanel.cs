using Hashira.StageSystem;
using TMPro;
using UnityEngine;

namespace Hashira.CanvasUI
{
    public class GameDataUIPanel : UIBase, IToggleUI
    {
        [field: SerializeField] public string Key { get; set; }

        [SerializeField] private TextMeshProUGUI _stageText;

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
            StageGenerator.Instance.GetCurrentStage().OnNextWave += StageTextUpdate;
            StageTextUpdate();
        }

        private void StageTextUpdate()
        {
            Stage stage = StageGenerator.Instance.GetCurrentStage();
            int waveCount = stage.waves.Length;
            bool isTutorial = StageGenerator.Instance.gameObject.activeSelf == false;
            string text;
            if (isTutorial)
            {
                text = "테스트 구역";
            }
            else
            {
                text = $"{StageGenerator.currentFloorIdx + 1}층 {StageGenerator.currentStageIdx + 1}번 구역\n" +
                       $"{Mathf.Clamp(stage.CurrentWaveCount + 1, 0, waveCount)}/{waveCount}웨이브";
            }
            _stageText.text = text;
        }
    }
}
