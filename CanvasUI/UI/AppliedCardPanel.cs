using DG.Tweening;
using Hashira.Cards.Effects;
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

        [SerializeField]
        private SetupCardVisual _setupCardVisual;

        private Dictionary<Type, SetupCardVisual> _appliedCardDict;

        private PlayerDataManager _playerEffectManager;

        protected override void Awake()
        {
            base.Awake();
            _appliedCardDict = new Dictionary<Type, SetupCardVisual>();
            _playerEffectManager = PlayerDataManager.Instance;
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
                SetupCardVisual cardUI = Instantiate(_setupCardVisual, _contentTransform);
                cardUI.VisualSetup(cardEffect.CardSO);
                cardUI.transform.localScale = Vector3.one * 0.9f;
                _appliedCardDict.Add(cardEffect.GetType(), cardUI);
            }
        }

        private void HandleEffectAddedEvent(CardEffect cardEffect)
        {
            Type effectType = cardEffect.GetType();
            if (_appliedCardDict.TryGetValue(effectType, out var ui))
            {
                int count = cardEffect.stack;
                ui.VisualSetup(cardEffect.CardSO);
            }
            else
            {
                SetupCardVisual cardUI = Instantiate(_setupCardVisual, _contentTransform);
                cardUI.VisualSetup(cardEffect.CardSO);
                cardUI.transform.localScale = Vector3.one * 0.9f;
                _appliedCardDict.Add(cardEffect.GetType(), cardUI);
            }
        }

        public void Close()
        {
            SetActive(false);
            if (_playerEffectManager != null) _playerEffectManager.EffectAddedEvent -= HandleEffectAddedEvent;
            foreach (var card in _appliedCardDict.Values)
            {
                if (card != null)
                {
                    Destroy(card.gameObject);
                }
            }
            _appliedCardDict.Clear();
        }

        private void OnDestroy()
        {
            _canvasGroup.DOKill();
            Close();
        }
    }
}
