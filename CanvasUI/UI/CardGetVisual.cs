using DG.Tweening;
using Hashira.CanvasUI;
using Hashira.Cards;
using Hashira.StageSystem;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Hashira
{
    public class CardGetVisual : UIBase, IToggleUI
    {
        private readonly static int _ValueShaderHash = Shader.PropertyToID("_Value");

        [SerializeField] private SetupCardVisual _cardVisual;
        [SerializeField] private TextMeshProUGUI _name, _description;
        [SerializeField] private CanvasGroup _canvasGroup;

        private Queue<CardSO> _cardGetQueue;

        private ChildrenMaterialController _childrenMaterialController, _cardMat;
        private Sequence _cardGetVisualSeq;

        public string Key { get; set; } = nameof(CardGetVisual);

        protected override void Awake()
        {
            base.Awake();
            _childrenMaterialController = GetComponent<ChildrenMaterialController>();
            _cardGetQueue = new Queue<CardSO>();
            _cardMat = _canvasGroup.GetComponent<ChildrenMaterialController>();
        }

        private void Start()
        {
            Close();
            _name.alpha = 0;
            _description.alpha = 0;

            StageGenerator.Instance.OnNextStageEvent += GetHandleNextStageEvent;
        }

        private void GetHandleNextStageEvent()
        {
            _cardGetQueue.Clear();

            _cardGetVisualSeq.Clear();

            Close();
        }

        public void SetCard(CardSO cardSO)
        {
            _cardGetQueue.Enqueue(cardSO);
        }

        private void Update()
        {
            if (_cardGetQueue.Count > 0 && (_cardGetVisualSeq == null || _cardGetVisualSeq.IsActive() == false))
            {
                CardSO cardSO = _cardGetQueue.Dequeue();
                _cardVisual.VisualSetup(cardSO);
                _name.text = cardSO.displayName;
                _description.text = cardSO.Description;

                _cardGetVisualSeq = DOTween.Sequence();
                _cardGetVisualSeq
                    .Append(RectTransform.DOSizeDelta(new Vector2(1000, 600), 0.2f).SetEase(Ease.OutQuart))
                    .Join(DOTween.To(() => 1, value =>
                    {
                        _childrenMaterialController.SetValue(_ValueShaderHash, value);
                    }, 0f, 0.2f))
                    .Append(DOTween.To(() => 1, value =>
                    {
                        _canvasGroup.alpha = 1 - value;
                        _cardMat.SetValue(_ValueShaderHash, value);
                    }, 0f, 0.2f))
                    .Append(_name.DOFade(1f, 0.2f))
                    .Append(_description.DOFade(1f, 0.2f))
                    .AppendInterval(0.8f)
                    .Append(RectTransform.DOSizeDelta(new Vector2(1000, 0), 0.4f).SetEase(Ease.InExpo))
                    .Join(_name.DOFade(0f, 0.3f))
                    .Join(_description.DOFade(0f, 0.3f))
                    .Join(DOTween.To(() => 1, value =>
                    {
                        _canvasGroup.alpha = value;
                        _childrenMaterialController.SetValue(_ValueShaderHash, value);
                    }, 0f, 0.2f))
                    .AppendCallback(() =>
                    {
                        if (_cardGetQueue.Count == 0)
                            Close();
                    });
            }
        }

        public void Open()
        {
            RectTransform.sizeDelta = new Vector2(1000, 0);
            gameObject.SetActive(true);
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            _cardGetVisualSeq.Clear();

            if (StageGenerator.Instance != null) StageGenerator.Instance.OnNextStageEvent += GetHandleNextStageEvent;
        }
    }
}
