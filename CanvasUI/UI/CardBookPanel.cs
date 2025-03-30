using Crogen.CrogenPooling;
using DG.Tweening;
using Hashira.Cards;
using Hashira.Core;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Hashira.CanvasUI
{
    public class CardBookPanel : UIBase, IToggleUI
    {
        [SerializeField]
        private Transform _contentTrm;
        [SerializeField]
        private RectTransform _scrollView;
        [SerializeField]
        private CardBookInfoPanel _cardBookInfoPanel;
        [SerializeField]
        private CanvasGroup _canvasGroup;

        private float _startRight;
        private float _endRight = Screen.width * 0.4f;

        [field: SerializeField]
        public string Key { get; set; }

        private List<BookCardUI> _cardList;

        public void Open()
        {
            SetActive(true);
            var list = CardManager.Instance.GetCardList();
            foreach (var card in list)
            {
                BookCardUI bookCardUI = gameObject.Pop(UIPoolType.BookCardUI, _contentTrm) as BookCardUI;
                bookCardUI.Initialize(card);
                _cardList.Add(bookCardUI);
            }
        }

        public void Close()
        {
            SetActive(false, 0);
            _cardList.ForEach(x => x.Push());
            _cardList.Clear();
        }

        private void Update()
        {
            //if(Input.GetKeyDown(KeyCode.Escape))
            //{
            //    Open();
            //}
            //if(Input.GetKeyDown(KeyCode.Alpha1))
            //{
            //    Close();
            //}
        }

        protected override void Awake()
        {
            base.Awake();
            _cardList = new List<BookCardUI>();
            Close();
            _cardBookInfoPanel.OnToggleEvent += HandleOnToggleEvent;
            _startRight = _scrollView.offsetMax.x;
        }

        private void HandleOnToggleEvent(bool isOpen)
        {
            float right = isOpen ? -_endRight : _startRight;
            Vector2 destination = new Vector2(right, _scrollView.offsetMax.y);
            DOTween.To(() => _scrollView.offsetMax, v => _scrollView.offsetMax = v, destination, 0.4f);
        }

        public void SetActive(bool isActive, float duration = 0.5f)
        {
            float alpha = isActive ? 1f : 0;
            _canvasGroup.DOFade(alpha, duration);
            _canvasGroup.interactable = isActive;
            _canvasGroup.blocksRaycasts = isActive;
        }
    }
}
