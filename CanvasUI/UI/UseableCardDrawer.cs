using Crogen.CrogenPooling;
using DG.Tweening;
using Hashira.Cards;
using Hashira.Core;
using System;
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

        [SerializeField] private int _interval = 10;
        [SerializeField] private int _spreadMovingSpeed = 10;

        [field: SerializeField] public Transform CardUsePos { get; private set; }
        [field: SerializeField] public TextMeshProUGUI CardUseHint { get; private set; }
        [field: SerializeField] public Transform DragCardTrm { get; private set; }

        private List<UseableCardUI> _useableCardUILsit = new List<UseableCardUI>();

        public void CardDraw(bool isMaintainFixedCard = false)
        {
            int fixedCardCount = CardManager.Instance.FixedCardList.Count;
            _useableCardUILsit.ForEach(cardUI =>
            {
                cardUI.Push();
            });
            _useableCardUILsit.Clear();
            List<CardSO> cardSOList = CardManager.Instance.GetRandomCardList(_cardCount);
            for (int i = 0; i < cardSOList.Count && _useableCardUILsit.Count < cardSOList.Count; i++)
            {
                UseableCardUI useableCardUI = gameObject.Pop(UIPoolType.UseableCardUI, transform) as UseableCardUI;
                useableCardUI.SetCard(cardSOList[i], this);
                DOTween.To(() => 0.5f, value => useableCardUI.ChildrenMaterialController.SetValue(_GlitchValueHash, value), 0, 0.75f).SetEase(Ease.InSine);
                EnterSpread(useableCardUI);

                if (isMaintainFixedCard && i < fixedCardCount)
                {
                    CardManager.Instance.FixationCard(useableCardUI, false);
                }
            }
        }

        public void OnCardUsed(CardSO cardSO)
        {
            if (_useableCardUILsit.Count == 0)
            {
                CardDraw();
            }
        }

        public void ExitSpread(UseableCardUI useableCardUI)
        {
            _useableCardUILsit.Remove(useableCardUI);
            DOTween.To(() => 0.4f, value => useableCardUI.ChildrenMaterialController.SetValue(_GlitchValueHash, value), 0, 0.3f).SetEase(Ease.InSine);
        }

        public void EnterSpread(UseableCardUI useableCardUI)
        {
            _useableCardUILsit.Add(useableCardUI);
        }

        private void Update()
        {
            int cardCount = _useableCardUILsit.Count;
            float interval = (float)_interval / 1920 * Screen.width * ((1920f / 1080) / ((float)Screen.width / Screen.height));
            for (int i = 0; i < cardCount; i++)
            {
                Vector2 currentPos = _useableCardUILsit[i].transform.position;
                Vector2 targetPos = transform.position + Vector3.right * (i + 0.5f - (float)cardCount / 2) * interval;
                _useableCardUILsit[i].transform.position = Vector3.Lerp(currentPos, targetPos, Time.deltaTime * _spreadMovingSpeed);
            }
        }
    }
}
