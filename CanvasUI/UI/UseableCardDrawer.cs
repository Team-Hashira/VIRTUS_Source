using DG.Tweening;
using Hashira.Cards;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Hashira.CanvasUI
{
    public class UseableCardDrawer : UIBase
    {
        private readonly static int _GlitchValueHash = Shader.PropertyToID("_Value");

        [SerializeField] private int _cardCount = 4;

        [field: SerializeField] public bool IsLockMode { get; private set; }
        [SerializeField] private Transform _dragCardTrm;
        [SerializeField] private CardSpreader _cardSpreader;
        [SerializeField] private Image _cardSelectBackgroundPanel;
        [SerializeField] private CanvasGroup _cardLockBackgroundPanel;
        [SerializeField] private CustomButton _useButton, _cancelButton;
        [SerializeField] private ChildrenMaterialController _buttonMaterial;
        private CanvasGroup _buttonGroup;
        private Tween _buttonMaterialTween;

        private List<UseableCardUI> _useableCardUIList;
        private List<int> _fixedCardIndexList;

        private UseableCardUI _currentSelectedCard;

        private Coroutine _fixedDisable;

        protected override void Awake()
        {
            base.Awake();
            _useButton.gameObject.SetActive(false);
            _cancelButton.gameObject.SetActive(false);

            _buttonGroup = _buttonMaterial.GetComponent<CanvasGroup>();
        }

        public void CardDraw(bool isMaintainFixedCard = false)
        {
            if (_fixedDisable != null)
            {
                StopCoroutine(_fixedDisable);
                foreach (int fixedIndex in _fixedCardIndexList)
                    CardManager.Instance.UnFixationCard(_useableCardUIList[fixedIndex]);
                _fixedDisable = null;
            }

            int fixedCardCount = CardManager.Instance.FixedCardList.Count;
            _fixedCardIndexList = CardManager.Instance.FixedCardList.Select(pair => pair.second).ToList();

            List<CardSO> cardSOList = CardManager.Instance.GetRandomCardList(_cardCount);

            _cardSpreader.ClearCard();
            _cardSpreader.CardSpread(cardSOList);
            _useableCardUIList = _cardSpreader.GetCardList<UseableCardUI>();

            int index = 0;
            foreach (UseableCardUI useableCardUI in _useableCardUIList)
            {
                useableCardUI.SetCard(this, index);
                DOTween.To(() => 0.5f, value => useableCardUI.MaterialController.SetValue(_GlitchValueHash, value), 0, 0.75f).SetEase(Ease.InSine);

                index++;
            }

            if (isMaintainFixedCard)
            {
                foreach (int fixedIndex in _fixedCardIndexList)
                {
                    CardManager.Instance.FixationCard(_useableCardUIList[fixedIndex], false);
                }
            }
            else
                _fixedDisable = StartCoroutine(FixedDisableCoroutine(0.3f, 0.4f));
        }

        private IEnumerator FixedDisableCoroutine(float sahkeDelay, float UnFixationDelay)
        {
            foreach (int fixedIndex in _fixedCardIndexList)
            {
                _useableCardUIList[fixedIndex].ActiveLockIcon(true);
                CardManager.Instance.FixationCard(_useableCardUIList[fixedIndex], false);
            }

            yield return new WaitForSeconds(sahkeDelay);
            foreach (int fixedIndex in _fixedCardIndexList)
                _useableCardUIList[fixedIndex].ShakeLockIcon();
            yield return new WaitForSeconds(UnFixationDelay);

            foreach (int fixedIndex in _fixedCardIndexList)
            {
                _useableCardUIList[fixedIndex].ActiveLockIcon(false);
                CardManager.Instance.UnFixationCard(_useableCardUIList[fixedIndex]);
            }

            _fixedDisable = null;
        }

        public void CardSelect(UseableCardUI useableCardUI)
        {
            _currentSelectedCard = useableCardUI;

            useableCardUI.transform.SetParent(_dragCardTrm);
            _cardSelectBackgroundPanel.DOKill();
            _cardSelectBackgroundPanel.DOFade(0.7f, 0.2f);
            _cardSelectBackgroundPanel.raycastTarget = true;
            _cardSpreader.ExitCard(useableCardUI);
            DOTween.To(() => 0.4f, value => useableCardUI.MaterialController.SetValue(_GlitchValueHash, value), 0, 0.3f).SetEase(Ease.InSine);

            StartCoroutine(DelayButtonEnable(0.2f));
            _useButton.OnClickEvent += HandleCardUseEvent;
            _cancelButton.OnClickEvent += HandleSelectCancelEvent;
        }

        private void HandleCardUseEvent()
            => _currentSelectedCard.CardUse();
        private void HandleSelectCancelEvent()
            => CardSelectCancel(_currentSelectedCard);

        public void CardSelectCancel(UseableCardUI useableCardUI)
        {
            _cardSelectBackgroundPanel.DOKill();
            _cardSelectBackgroundPanel.DOFade(0f, 0.2f);
            _cardSelectBackgroundPanel.raycastTarget = false;
            if (useableCardUI != null)
            {
                useableCardUI.ActiveSelectMode(false);
                useableCardUI.transform.SetParent(transform);
                _cardSpreader.EnterCard(useableCardUI, useableCardUI.Index);
            }
            ActiveUseButton(false);
            _useButton.OnClickEvent -= HandleCardUseEvent;
            _cancelButton.OnClickEvent -= HandleSelectCancelEvent;
        }

        private void ActiveUseButton(bool active)
        {
            _useButton.gameObject.SetActive(active);
            _cancelButton.gameObject.SetActive(active);
            _buttonGroup.blocksRaycasts = active;
            _buttonGroup.interactable = active;
        }

        private IEnumerator DelayButtonEnable(float delay)
        {
            yield return new WaitForSeconds(delay);
            ActiveUseButton(true);
            if (_buttonMaterialTween != null && _buttonMaterialTween.IsActive()) _buttonMaterialTween.Kill();
            _buttonMaterialTween = DOTween.To(() => 1f, value =>
            {
                _buttonGroup.alpha = 1 - value;
                _buttonMaterial.SetValue(_GlitchValueHash, value);
            }, 0, 0.3f);
        }

        public void ActiveLockMode(bool active)
        {
            IsLockMode = active;
            _cardLockBackgroundPanel.DOKill();
            _cardLockBackgroundPanel.DOFade(active ? 1f : 0f, 0.1f);
            _cardLockBackgroundPanel.blocksRaycasts = active;
            CardLockMode(active);
        }

        public void CardLockMode(bool active)
        {
            _useableCardUIList.ForEach(card => card.OnLockMode(active));
        }

        private void OnDestroy()
        {
            if (_buttonMaterialTween != null && _buttonMaterialTween.IsActive()) _buttonMaterialTween.Kill();
        }
    }
}
