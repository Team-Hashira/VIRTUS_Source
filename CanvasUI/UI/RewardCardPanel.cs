using DG.Tweening;
using Hashira.Cards;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Hashira.CanvasUI
{
    public class RewardCardPanel : UIBase, IToggleUI
    {
        [field: SerializeField]
        public string Key { get; set; }

        [SerializeField]
        private float _space = 250f;
        [SerializeField]
        private Image _backgroundImage;
        [SerializeField]
        private RewardCardUI[] _rewardCards;

        private CanvasGroup _canvasGroup;

        protected override void Awake()
        {
            base.Awake();
            _canvasGroup = GetComponent<CanvasGroup>();

            for (int i = -1; i <= 1; i++)
            {
                float space = _space * i;
                _rewardCards[i + 1].RectTransform.anchoredPosition = new Vector3(space, 0);
            }

            SetActive(false, 0);

            foreach (var card in _rewardCards)
            {
                card.Initialize(this);
            }
        }

        public void Open()
        {
            SetActive(true, 0, 0.85f);
            _backgroundImage.DOFade(1, 0.1f);
            List<CardSO> cardList = CardManager.Instance.CardSetSO
                .GetRandomCardList(_rewardCards.Length, CardManager.Instance.GetCardList());
            for (int i = 0; i < _rewardCards.Length; i++)
            {
                if (i < cardList.Count)
                    _rewardCards[i].Reload(cardList[i]);
                else
                    _rewardCards[i].Reload(cardList[^1]);
            }
        }

        public void Close()
        {
            SetActive(false);
        }

        public void SetActive(bool isActive, float duration = 0.5f, float interactDelay = 0f)
        {
            float alpha = isActive ? 1f : 0;
            _canvasGroup.DOFade(alpha, duration);
            StartCoroutine(InteractDelayCoroutine(isActive, interactDelay));
        }

        private IEnumerator InteractDelayCoroutine(bool enable, float time)
        {
            yield return new WaitForSeconds(time);
            _canvasGroup.interactable = enable;
            _canvasGroup.blocksRaycasts = enable;
        }

        public void Select(RewardCardUI card)
        {
            foreach (var c in _rewardCards)
            {
                if (c == card)
                    continue;
                if (c.RectTransform.anchoredPosition.x > card.RectTransform.anchoredPosition.x)
                    c.Wipe(1);
                else
                    c.Wipe(-1);
            }
        }
    }
}
