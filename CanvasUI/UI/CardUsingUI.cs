using DG.Tweening;
using UnityEngine;

namespace Hashira.CanvasUI
{
    public class CardUsingUI : UIBase, IToggleUI
    {
        private readonly static int _GlitchValueHash = Shader.PropertyToID("_Value");

        private CanvasGroup _canvasGroup;
        [SerializeField] private UseableCardDrawer _useableCardDrower;
        [SerializeField] private PlayerImage _playerImage;
        [field: SerializeField] public string Key { get; set; }

        [SerializeField] private ChildrenMaterialController _cardAreaGlitchController;
        private CanvasGroup _cardAreaGroup;

        [SerializeField] private CardRerollButton _rerollBtn;
        [SerializeField] private CustomButton _stageBtn;

        [SerializeField] private SpriteRenderer _backgroundGlitchRenderer;
        private Sequence _openSeq;

        protected override void Awake()
        {
            base.Awake();
            _canvasGroup = GetComponent<CanvasGroup>();
            _cardAreaGroup = _cardAreaGlitchController.transform.GetComponent<CanvasGroup>();
            _cardAreaGroup.alpha = 1;

            _rerollBtn.SetCardDrower(_useableCardDrower);

            _stageBtn.OnClickEvent += () =>
            {
                //Stage 시작
                CardSelectScene.Instance.StartStage();
                Close();
            };

            Close();
        }

        private void Update()
        {
            //// 스테이지 스킵 디버그
            //if (Input.GetKeyDown(KeyCode.P))
            //{
            //    StageGenerator.currentStageIdx++;
            //    if (StageGenerator.currentStageIdx > 10)
            //        StageGenerator.currentStageIdx = 10;

            //    int currentStageIdx = StageGenerator.currentStageIdx;
            //    string prevStageText = currentStageIdx == 0 ? "출발 구역" : $"{currentStageIdx}번 구역";
            //    string nextStageText = $"{currentStageIdx + 1}번 구역";
            //    _stageProgressText.text = $"{StageGenerator.currentFloorIdx + 1}층\n " +
            //                                $"{prevStageText}  >>  {nextStageText}";
            //}
        }

        public void Close()
        {
            _canvasGroup.alpha = 0;
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
        }

        public void Open()
        {
            _backgroundGlitchRenderer.gameObject.SetActive(true);

            _openSeq.Clear();
            _openSeq = DOTween.Sequence();
            _openSeq.Append(DOTween.To(value => _backgroundGlitchRenderer.material
                .SetFloat(_GlitchValueHash, value), 1f, 0, 0.5f).SetEase(Ease.Linear));
            _openSeq.AppendCallback(() =>
            {
                _backgroundGlitchRenderer.gameObject.SetActive(false);
                _canvasGroup.alpha = 1;
                _canvasGroup.interactable = true;
                _canvasGroup.blocksRaycasts = true;
                _useableCardDrower.CardDraw();
            });
            _openSeq.Join(DOTween.To(value => _playerImage.material
                .SetFloat(_GlitchValueHash, value), 1f, 0.1f, 0.5f).SetEase(Ease.Linear));
            _openSeq.Join(DOTween.To(value => _cardAreaGlitchController.SetValue(_GlitchValueHash, value), 1f, 0, 0.5f).SetEase(Ease.Linear));
            _openSeq.Join(DOTween.To( value => _cardAreaGroup.alpha = value, 0, 1f, 0.5f).SetEase(Ease.Linear));
        }

        private void OnDestroy()
        {
            _openSeq?.Kill();
        }
    }
}
