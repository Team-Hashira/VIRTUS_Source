using Crogen.CrogenPooling;
using DG.Tweening;
using Hashira.Cards.Effects;
using Hashira.EffectSystem;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Hashira.CanvasUI
{
    public class AppliedCardPanel : UIBase, IToggleUI
    {
        [field: SerializeField]
        public string Key { get; set; }

        [SerializeField]
        private CanvasGroup _canvasGroup;

        [SerializeField]
        private Transform _contentTransform;

        private Dictionary<Type, AppliedCardUI> _appliedCardDict;

        private PlayerDataManager _playerEffectManager;

        protected override void Awake()
        {
            base.Awake();
            _appliedCardDict = new Dictionary<Type, AppliedCardUI>();
        }

        private void Start()
        {
            _playerEffectManager = PlayerDataManager.Instance;
            Close();
        }

        public void SetActive(bool isActive, float duration = 0.5f)
        {
            float alpha = isActive ? 1f : 0;
            _canvasGroup.DOFade(alpha, duration);
            _canvasGroup.interactable = isActive;
            _canvasGroup.blocksRaycasts = isActive;
        }

        public void Open()
        {
            SetActive(true);
            _playerEffectManager.EffectAddedEvent += HandleEffectAddedEvent;
            List<CardEffect> cardEffectList = _playerEffectManager.CardEffectList;

            foreach (var cardEffect in cardEffectList)
            {
                if (cardEffect.stack <= 0)
                    continue;
                AppliedCardUI cardUI = gameObject.Pop(UIPoolType.AppliedCardUI, _contentTransform) as AppliedCardUI;
                cardUI.Initialize(cardEffect);
                _appliedCardDict.Add(cardEffect.GetType(), cardUI);
            }
        }

        private void HandleEffectAddedEvent(CardEffect cardEffect)
        {
            Type effectType = cardEffect.GetType();
            if (_appliedCardDict.TryGetValue(effectType, out var ui))
            {
                int count = cardEffect.stack;
                ui.UpdateCount(count);
            }
            else
            {
                AppliedCardUI cardUI = gameObject.Pop(UIPoolType.AppliedCardUI, _contentTransform) as AppliedCardUI;
                cardUI.Initialize(cardEffect);
                _appliedCardDict.Add(cardEffect.GetType(), cardUI);
            }
        }

        public void Close()
        {
            SetActive(false);
            if (_playerEffectManager != null) _playerEffectManager.EffectAddedEvent -= HandleEffectAddedEvent;
            foreach (var card in _appliedCardDict.Values)
            {
                card?.Push();
            }
            _appliedCardDict.Clear();
        }

        private void OnDestroy()
        {
            Close();
            _canvasGroup.DOKill();
        }
    }
}
