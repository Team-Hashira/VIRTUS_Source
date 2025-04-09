using DG.Tweening;
using Hashira.Cards;
using Hashira.StageSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Hashira.CanvasUI
{
    public class CardUsingUI : UIBase, IToggleUI
    {
        private readonly static int _GlitchValueHash = Shader.PropertyToID("_Value");

        private CanvasGroup _canvasGroup;
        [SerializeField] private UseableCardDrawer _useableCardDrower;
        [SerializeField] private PlayerImage _playerImage;
        [field: SerializeField] public string Key { get; set; }

        [SerializeField] private TextMeshProUGUI _stageProgressText;
        [SerializeField] private ChildrenMaterialController _cardAreaGlitchController;
        private CanvasGroup _cardAreaGroup;

        [SerializeField] private CardRerollButton _rerollBtn;
        [SerializeField] private CustomButton _stageBtn;
        [SerializeField] private TextMeshProUGUI _cardFixedcNeedCost;

        [SerializeField] private Transform _backgroundTrm;
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
            // 스테이지 스킵 디버그
            if (Input.GetKeyDown(KeyCode.P))
            {
                StageGenerator.currentStageIdx++;
                if (StageGenerator.currentStageIdx > 10)
                    StageGenerator.currentStageIdx = 10;

                int currentStageIdx = StageGenerator.currentStageIdx;
                string prevStageText = currentStageIdx == 0 ? "출발 구역" : $"{currentStageIdx}번 구역";
                string nextStageText = $"{currentStageIdx + 1}번 구역";
                _stageProgressText.text = $"{StageGenerator.currentFloorIdx + 1}층\n " +
                                            $"{prevStageText}  >>  {nextStageText}";
            }
        }

        public void Close()
        {
            _canvasGroup.alpha = 0;
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
            _backgroundTrm.gameObject.SetActive(false);

            CardManager.Instance.OnFixationCardEvent -= HandleFixationCardEvent;
        }

        private void HandleFixationCardEvent(int fixationCard)
        {
            _cardFixedcNeedCost.text = $"{CardManager.Instance.FixedCardNeedCost[fixationCard]}";
        }

        public void Open()
        {
            _backgroundTrm.gameObject.SetActive(true);
            _backgroundGlitchRenderer.gameObject.SetActive(true);

            int currentStageIdx = StageGenerator.currentStageIdx;
            string prevStageText = currentStageIdx == 0 ? "출발 구역" : $"{currentStageIdx}번 구역";
            string nextStageText = $"{currentStageIdx + 1}번 구역";
            _stageProgressText.text =   $"{StageGenerator.currentFloorIdx + 1}층\n " +
                                        $"{prevStageText}  >>  {nextStageText}";

            CardManager.Instance.OnFixationCardEvent += HandleFixationCardEvent;
            HandleFixationCardEvent(0);

            if (_openSeq != null && _openSeq.IsActive()) _openSeq.Kill();
            _openSeq = DOTween.Sequence();
            _openSeq.Append(DOTween.To(() => 1f, value => _backgroundGlitchRenderer.material
                .SetFloat(_GlitchValueHash, value), 0, 0.5f).SetEase(Ease.Linear));
            _openSeq.AppendCallback(() =>
            {
                _backgroundGlitchRenderer.gameObject.SetActive(false);
                _canvasGroup.alpha = 1;
                _canvasGroup.interactable = true;
                _canvasGroup.blocksRaycasts = true;
                _useableCardDrower.CardDraw();
            });
            _openSeq.Join(DOTween.To(() => 1f, value => _playerImage.material
                .SetFloat(_GlitchValueHash, value), 0.1f, 0.5f).SetEase(Ease.Linear));
            _openSeq.Join(DOTween.To(() => 1f, value => _cardAreaGlitchController.SetValue(_GlitchValueHash, value), 0, 0.5f).SetEase(Ease.Linear));
            _openSeq.Join(DOTween.To(() => 0f, value => _cardAreaGroup.alpha = value, 1, 0.5f).SetEase(Ease.Linear));
        }
    }
}
