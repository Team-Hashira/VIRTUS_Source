using DG.Tweening;
using Hashira.CanvasUI;
using Hashira.Cards;
using Hashira.StageSystem;
using System.Collections.Generic;
using UnityEngine;

namespace Hashira
{
    public class StageCardSelectUI : UIBase, IToggleUI
    {
        private readonly static int _ValueShaderHash = Shader.PropertyToID("_Value");

        [SerializeField] private CardSetSO _cardSetSO;
        [SerializeField] private CardSpreader _cardSpreader;

        private ChildrenMaterialController _childrenMaterialController;
        private Sequence _cardGetVisualSeq;

        private List<SelecrableCard> _cardVisualList;

        public string Key { get; set; } = nameof(StageCardSelectUI);

        protected override void Awake()
        {
            base.Awake();
            _childrenMaterialController = GetComponent<ChildrenMaterialController>();
        }

        private void Start()
        {
            gameObject.SetActive(false);
        }

        public void Open()
        {
            Hashira.CanvasUI.UIManager.Instance.AddPauseMenu(this);
            gameObject.SetActive(true);

            List<CardSO> cardSO = _cardSetSO.GetRandomCardList(3, CardManager.Instance.GetCardList());

            RectTransform.sizeDelta = new Vector2(1000, 0);

            _cardGetVisualSeq.Clear();
            _cardGetVisualSeq = DOTween.Sequence().SetUpdate(true);
            _cardGetVisualSeq
                .Append(RectTransform.DOSizeDelta(new Vector2(1000, 600), 0.2f).SetEase(Ease.OutQuart))
                .Join(DOTween.To(() => 1, value =>
                {
                    _childrenMaterialController.SetValue(_ValueShaderHash, value);
                }, 0f, 0.2f))
                .AppendCallback(() =>
                {
                    _cardSpreader.CardSpread(cardSO);
                    _cardVisualList = _cardSpreader.GetCardList<SelecrableCard>();
                    foreach (SelecrableCard selecrableCard in _cardVisualList)
                    {
                        selecrableCard.transform.localScale = Vector3.one * 1.5f;
                        selecrableCard.OnCardSelectEvent += HandleCardSelectEvent;
                    }
                })
                .Append(DOTween.To(() => 1f, value =>
                {
                    foreach (SelecrableCard selecrableCard in _cardVisualList)
                    {
                        selecrableCard.MaterialController.SetValue(_ValueShaderHash, value);
                        selecrableCard.CanvasGroup.alpha = 1 - value;
                    }
                }, 0f, 0.2f));
        }

        private void HandleCardSelectEvent(SelecrableCard card)
        {
            if (_cardGetVisualSeq != null && _cardGetVisualSeq.IsActive()) _cardGetVisualSeq.Kill();
            _cardGetVisualSeq = DOTween.Sequence().SetUpdate(true);
            _cardGetVisualSeq
                .Append(DOTween.To(() => 1f, value =>
                {
                    foreach (SelecrableCard selecrableCard in _cardVisualList)
                    {
                        selecrableCard.CanvasGroup.alpha = value;
                        if (card == selecrableCard) continue;
                        selecrableCard.transform.localScale *= Mathf.Pow(value, 0.5f);
                    }
                }, 0f, 0.2f))
                .Join(RectTransform.DOSizeDelta(new Vector2(1000, 600), 0.3f).SetEase(Ease.OutQuart))
                .Join(DOTween.To(() => 1, value =>
                {
                    _childrenMaterialController.SetValue(_ValueShaderHash, value);
                }, 0f, 0.3f))
                .AppendCallback(() =>
                {
                    _cardSpreader.ClearCard();

                    CardManager.Instance.AddCard(card.CardSO);

                    Close();

                    CardGetVisual cardGetVisual = Hashira.CanvasUI.UIManager.Instance.GetDomain<ToggleDomain>()
                        .OpenUI(nameof(CardGetVisual)) as CardGetVisual;
                    cardGetVisual.SetCard(card.CardSO);
                });
            StageGenerator.Instance.GetCurrentStage().OpenPortalAndEvent();
        }

        public void Close()
        {
            Hashira.CanvasUI.UIManager.Instance.RemovePauseMenu(this);
            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            _cardGetVisualSeq.Clear();
        }
    }
}
