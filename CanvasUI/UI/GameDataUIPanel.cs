using DG.Tweening;
using Hashira.StageSystem;
using UnityEngine;
using UnityEngine.UI;

namespace Hashira.CanvasUI
{
    public class GameDataUIPanel : UIBase, IToggleUI
    {
        [field: SerializeField] public string Key { get; set; }
        [SerializeField] private Slider _waveSlider;
        
        private Stage _currentStage;

        private CanvasGroup _canvasGroup;

        protected override void Awake()
        {
            base.Awake();
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        private void SetVisable(bool visable, float duration = 1f)
        {
            _canvasGroup.interactable = visable;
            _canvasGroup.blocksRaycasts = visable;
            _canvasGroup.DOFade(visable ? 1 : 0, duration);
        }
        
        private void Start()
        {
            Open();
        }

        

        public void Open()
        {
            SetVisable(true);
            _waveSlider.value = 0f;
            _currentStage = StageGenerator.Instance.GetCurrentStage();
            StageGenerator.Instance.OnNextStageEvent += HandleNextStageEvent;
            _currentStage.OnWaveChangedEvent += HandleWaveSliderUpdate;
            _currentStage.OnAllClearEvent += HandleStageClear;
        }

        public void Close()
        {
            SetVisable(false);
            StageGenerator.Instance.OnNextStageEvent -= HandleNextStageEvent;
            _currentStage.OnWaveChangedEvent -= HandleWaveSliderUpdate;
            _currentStage.OnAllClearEvent -= HandleStageClear;
        }
        
        private void HandleWaveSliderUpdate(int cur, int max) => _waveSlider.DOValue(cur / (float)max, 0.1f);
        private void HandleStageClear() => _waveSlider.DOValue(1f, 0.1f);

        private void HandleNextStageEvent()
        {
            _currentStage = StageGenerator.Instance.GetCurrentStage();
        }

        private void OnDestroy()
        {
            _canvasGroup?.DOKill();
            UIManager.Instance?.RemoveUI(this);
        }
    }
}
