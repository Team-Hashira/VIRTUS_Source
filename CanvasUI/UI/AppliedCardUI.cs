using Crogen.CrogenPooling;
using DG.Tweening;
using Hashira.Cards.Effects;
using Hashira.EffectSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Hashira.CanvasUI
{
    public class AppliedCardUI : UIBase, IHoverableUI, IPoolingObject
    {
        public string OriginPoolType { get; set; }
        GameObject IPoolingObject.gameObject { get; set; }

        [SerializeField]
        private Image _innerBackground, _foreIconImage, _backIconImage;
        [SerializeField]
        private TextMeshProUGUI _cardNameText, _cardCountText;

        [SerializeField]
        private Gradient _countGradient;

        private CardEffect _cardEffect;

        protected override void Awake()
        {
            base.Awake();
        }

        public void Initialize(CardEffect cardEffect)
        {
            _cardEffect = cardEffect;
            _foreIconImage.sprite = cardEffect.CardSO.sprite;
            _backIconImage.sprite = cardEffect.CardSO.sprite;
            _cardNameText.text = cardEffect.CardSO.displayName;
            _innerBackground.color = CardColor.GetColorByType(cardEffect.CardSO.cardType);
            UpdateCount(cardEffect.stack);
        }

        public void UpdateCount(int count)
        {
            float ratio = count / _cardEffect.CardSO.maxOverlapCount;
            Color color = _countGradient.Evaluate(ratio);
            string rgbHex = color.ToRGBHex();
            float size = 20f * ratio;
            size += 30f;
            if (_cardEffect.CardSO.maxOverlapCount > 0 && _cardEffect.CardSO.maxOverlapCount <= count)
            {
                _cardCountText.text = $"<size={size}pt><color=#{rgbHex}>MAX</color></size>";
            }
            else
                _cardCountText.text = $"<size={size}pt><color=#{rgbHex}>{count}</color></size> 중첩";
        }

        public void OnCursorEnter()
        {
            (_innerBackground.transform as RectTransform).DOAnchorPosX(65, 0.4f);
        }

        public void OnCursorExit()
        {
            (_innerBackground.transform as RectTransform).DOAnchorPosX(256, 0.4f);
        }

        public void OnPop()
        {
        }

        public void OnPush()
        {
        }
    }
}
