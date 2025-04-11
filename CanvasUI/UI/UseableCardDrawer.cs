using DG.Tweening;
using Hashira.Cards;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Hashira.CanvasUI
{
    public class UseableCardDrawer : UIBase
    {
        private readonly static int _GlitchValueHash = Shader.PropertyToID("_Value");

        [SerializeField] private int _cardCount = 4;

        [field: SerializeField] public bool IsLockMode { get; private set; }
        [field: SerializeField] public Transform CardUsePos { get; private set; }
        [field: SerializeField] public TextMeshProUGUI CardUseHint { get; private set; }
        [SerializeField] private Transform _dragCardTrm;
        [SerializeField] private CardSpreader _cardSpreader;
        [SerializeField] private Image _cardLockBackgroundPanel, _cardSelectBackgroundPanel;

        private List<UseableCardUI> _useableCardUIList;

        public void CardDraw(bool isMaintainFixedCard = false)
        {
            int fixedCardCount = CardManager.Instance.FixedCardList.Count;

            List<CardSO> cardSOList = CardManager.Instance.GetRandomCardList(_cardCount);

            _cardSpreader.ClearCard();
            _cardSpreader.CardSpread(cardSOList);
            _useableCardUIList = _cardSpreader.GetCardList<UseableCardUI>();

            int index = 0;
            foreach (UseableCardUI useableCardUI in _useableCardUIList)
            {
                useableCardUI.SetCard(this);
                DOTween.To(() => 0.5f, value => useableCardUI.ChildrenMaterialController.SetValue(_GlitchValueHash, value), 0, 0.75f).SetEase(Ease.InSine);

                if (isMaintainFixedCard && index < fixedCardCount)
                {
                    CardManager.Instance.FixationCard(useableCardUI, false);
                }
                index++;
            }
        }

        public void CardSelect(UseableCardUI useableCardUI)
        {
            useableCardUI.transform.SetParent(_dragCardTrm);
            _cardSelectBackgroundPanel.DOKill();
            _cardSelectBackgroundPanel.DOFade(0.6f, 0.2f);
            _cardSelectBackgroundPanel.raycastTarget = true;
            _cardSpreader.ExitCard(useableCardUI);
            DOTween.To(() => 0.4f, value => useableCardUI.ChildrenMaterialController.SetValue(_GlitchValueHash, value), 0, 0.3f).SetEase(Ease.InSine);
        }

        public void CardSelectCancel(UseableCardUI useableCardUI)
        {
            _cardSelectBackgroundPanel.DOKill();
            _cardSelectBackgroundPanel.DOFade(0f, 0.2f);
            _cardSelectBackgroundPanel.raycastTarget = false;
            if (useableCardUI != null)
            {
                useableCardUI.transform.SetParent(transform);
                _cardSpreader.EnterCard(useableCardUI);
            }
        }

        public void ActiveLockMode(bool active)
        {
            IsLockMode = active;
            _cardLockBackgroundPanel.DOKill();
            _cardLockBackgroundPanel.DOFade(active ? 0.6f : 0f, 0.2f);
            _cardLockBackgroundPanel.raycastTarget = active;
            CardLockMode(active);
        }

        public void CardLockMode(bool active)
        {
            _useableCardUIList.ForEach(card => card.OnLockMode(active));
        }
    }
}
