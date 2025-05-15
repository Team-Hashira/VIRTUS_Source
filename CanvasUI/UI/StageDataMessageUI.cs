using DG.Tweening;
using Hashira.StageSystem;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Hashira.CanvasUI
{
    public class StageDataMessageUI : UIBase, IToggleUI
    {
        public string Key { get; set; } = nameof(StageDataMessageUI);

        [SerializeField] private TextMeshProUGUI _stageText;
        private CanvasGroup _canvasGroup;
        private Image _image;
        
        private Sequence _activeSequence;

        private readonly int _glitchValueID = Shader.PropertyToID("_Value");
        
        protected override void Awake()
        {
            base.Awake();
            _canvasGroup = GetComponent<CanvasGroup>();
            _image = GetComponent<Image>();
            _image.material = Instantiate(_image.material);
        }

        private void Start()
        {
            // StageGenerator가 먼저 실행되어버림
            HandleStageTextUpdate();
            StageGenerator.Instance.OnGeneratedStageEvent += HandleStageTextUpdate;
        }

        private void OnDestroy()
        {
            if (StageGenerator.Instance)
                StageGenerator.Instance.OnGeneratedStageEvent -= HandleStageTextUpdate;

            _activeSequence?.Kill();
        }
        
        private void HandleStageTextUpdate()
        {
            bool isTutorial = StageGenerator.Instance.gameObject.activeSelf == false;
            string text = isTutorial ? "테스트 구역" : $"{StageGenerator.currentFloorIdx + 1}층 {StageGenerator.currentStageIdx + 1}번 구역";
            _stageText.text = text;
            Open();
        }

        public void Open()
        {
            gameObject.SetActive(true);
            
            _activeSequence?.Kill(true);
            _activeSequence = DOTween.Sequence();
            _activeSequence.OnStart(() =>
            {
                RectTransform.localScale = Vector3.one;
                _canvasGroup.alpha = 0f;
            });

            _activeSequence
                .Append(DOTween.To(x => _image.material.SetFloat(_glitchValueID, x), 0f, 1f, 1.2f))
                .Join(_canvasGroup.DOFade(1f, 1.2f))
                .AppendInterval(0.5f)
                .Append(RectTransform.DOScaleY(0f, 0.35f).SetEase(Ease.OutCubic))
                .Join(DOTween.To(x => _image.material.SetFloat(_glitchValueID, x), 1f, 0f, 0.35f));
        }

        public void Close()
        {
            _activeSequence?.Kill(true);
            gameObject.SetActive(false);
        }
    }
}
 