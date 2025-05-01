using DG.Tweening;
using Hashira.Cards.Effects;
using Hashira.Entities.Interacts;
using Hashira.Object;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;

namespace Hashira.CanvasUI.Wells
{
    public class WellUI : UIBase, IToggleUI
    {
        private readonly int _valueShaderHash = Shader.PropertyToID("_Value");

        [field: SerializeField] public string Key { get; set; }

        [SerializeField] private CustomButton _exitButton;
        [SerializeField] private CardScrollView _cardScrollView;
        [SerializeField] private WellCardUpgradeView _wellCardUpgradeView;
        [SerializeField] private CanvasGroup _selectView;
        private ChildrenMaterialController _selectViewMat;
        private ChildrenMaterialController _cardUpgradeViewMat;

        private Sequence _selectViewSeq;

        private List<SelecrableCard> _selecrableCardList;
        private SelecrableCard _selectedCard;
        private Vector3 _cardScale;

        private Well _well;

        public void Init(Well well)
        {
            _well = well;
            _well.CanInteraction = false;
        }

        public void Open()
        {
            gameObject.SetActive(true);
            _selectViewMat.SetValue(_valueShaderHash, 0f);

            UIManager.Instance.AddPauseMenu(this);
        }

        public void EventEnd()
        {
            _well.EventEnd();
            Close();
        }

        private IEnumerator CreateCardCoroutine()
        {
            yield return null;
            _cardScrollView.ActiveGrid(false);
            _selectViewMat.ReFindMaterial();
            if (_selectViewSeq != null && _selectViewSeq.IsActive()) _selectViewSeq.Kill();
            _selectViewSeq = DOTween.Sequence().SetUpdate(true);
            _selectViewSeq.Append(DOTween.To(() => 1f, value => _selectViewMat.SetValue(_valueShaderHash, value), 0f, 0.2f));
        }

        public void Close()
        {
            gameObject.SetActive(false);
            if (_well != null) _well.CanInteraction = true;
            Disable();
            UIManager.Instance.RemovePauseMenu(this);
        }

        public void Enable()
        {
            List<CardEffect> effectList = PlayerDataManager.Instance.CardEffectList;
            if (effectList.Count != 0)
            {
                _cardScrollView.ActiveGrid(true);
                _cardScrollView.CreateCard(effectList.Select(cardEffect => cardEffect.CardSO).ToList(), true);
                StartCoroutine(CreateCardCoroutine());
            }

            _selecrableCardList = _cardScrollView.GetCardList<SelecrableCard>();
            _selecrableCardList.ForEach(cardVisual =>
            {
                cardVisual.OnCardSelectEvent += HandleCardClickEvent;
            });
            _selectView.blocksRaycasts = true;
            _selectView.interactable = true;

            _selectView.alpha = 1;
        }

        private void HandleCardClickEvent(SelecrableCard selecrableCard)
        {
            Debug.Log(selecrableCard.CardSO.displayName);
            _cardScale = selecrableCard.transform.localScale;
            _selectedCard = selecrableCard;
            selecrableCard.ActiveHoverEvent(false);
            selecrableCard.ActiveClickEvent(false);
            selecrableCard.transform.SetParent(_wellCardUpgradeView.transform, true);
            _wellCardUpgradeView.Init(selecrableCard, this);

            RectTransform selectedCardRectTrm = _selectedCard.transform as RectTransform;
            Vector2 center = new Vector2(0.5f, 0.5f);
            Vector3 pos = selectedCardRectTrm.position;
            selectedCardRectTrm.anchorMin = center;
            selectedCardRectTrm.anchorMax = center;
            selectedCardRectTrm.position = pos;

            _selectViewMat.ReFindMaterial();

            if (_selectViewSeq != null && _selectViewSeq.IsActive()) _selectViewSeq.Kill();
            _selectViewSeq = DOTween.Sequence().SetUpdate(true);
            _selectViewSeq
                .Append(_selectView.DOFade(0, 0.2f))
                .Join(DOTween.To(() => 0f, value => _selectViewMat.SetValue(_valueShaderHash, value), 1f, 0.2f))
                .Join(selectedCardRectTrm.DOAnchorPos(new Vector2(0, 50), 0.4f).SetEase(Ease.OutQuart))
                .Join(selectedCardRectTrm.DOScale(_cardScale * 1.2f, 0.4f).SetEase(Ease.OutQuart))
                .AppendCallback(() =>
                {
                    _wellCardUpgradeView.transform.GetChild(0).gameObject.SetActive(true);
                    Disable();
                    _cardUpgradeViewMat.ReFindMaterial();
                    _cardUpgradeViewMat.SetValue(_valueShaderHash, 0f);
                });
        }

        public void Disable()
        {
            _selectView.blocksRaycasts = false;
            _selectView.interactable = false;
            _selecrableCardList.ForEach(cardVisual =>
            {
                cardVisual.OnCardSelectEvent -= HandleCardClickEvent;
            });
            _cardScrollView.ClearCard();
        }

        protected override void Awake()
        {
            base.Awake();
            _exitButton.OnClickEvent += HandleExitClickEvent;
            _selecrableCardList = new List<SelecrableCard>();
            _selectViewMat = _selectView.GetComponent<ChildrenMaterialController>();
            _cardUpgradeViewMat = _wellCardUpgradeView.GetComponent<ChildrenMaterialController>();
        }

        private void Start()
        {
            Close();
        }

        private void HandleExitClickEvent()
        {
            Close();
        }

        private void OnDestroy()
        {
            UIManager.Instance?.RemoveUI(this);
        }
    }
}
