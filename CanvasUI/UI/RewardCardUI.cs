using DG.Tweening;
using Hashira.Cards;
using System;
using System.Collections;
using UnityEngine;

namespace Hashira.CanvasUI
{
    public class RewardCardUI : UIBase, IClickableUI, IHoverableUI
    {
        private CardSO _cardSO;

        private RewardCardPanel _rewardCardPanel;

        [SerializeField]
        private Vector2 _defaultPosition;
        private Vector2 _defaultScale;

        private Sequence _setDefaultTween;
        private Sequence _hoverSequence;
        private Sequence _selectSequence;

        [SerializeField]
        private SetupCardVisual _cardVisualSetup;
        [SerializeField]
        private ChildrenMaterialController _childMaterialController;

        private readonly int _glitchValueHash = Shader.PropertyToID("_Value");
        private readonly int _glitchRangeHash = Shader.PropertyToID("_Range");
        private readonly int _glitchNoiseSizeHash = Shader.PropertyToID("_NoiseSize");

        private Tween _glitchTween;

        //[Header("Test")]
        //[Dependent]
        //[SerializeField]
        //private CardSO _input;


        public void Reload(CardSO cardSO)
        {
            _cardSO = cardSO;
            _cardVisualSetup.VisualSetup(cardSO);
            RectTransform.localScale = _defaultScale;
            RectTransform.anchoredPosition = Vector2.zero;
            StartCoroutine(ReloadCoroutine(_defaultPosition, 0.6f));
        }

        public void Initialize(RewardCardPanel panel)
        {
            _childMaterialController.SetValue(_glitchValueHash, 0f);
            _childMaterialController.SetValue(_glitchRangeHash, 1.77f);
            _childMaterialController.SetValue(_glitchNoiseSizeHash, 1.13f);
            _rewardCardPanel = panel;
            _defaultPosition = RectTransform.anchoredPosition;
            _defaultScale = RectTransform.localScale;
        }

        public void OnClick(bool isLeft)
        {
            if (isLeft)
            {
                _selectSequence = DOTween.Sequence();
                _selectSequence
                    .Append(RectTransform.DORotate(new Vector3(0, 360f), 0.3f, RotateMode.FastBeyond360))
                    .JoinCallback(() => _rewardCardPanel.Select(this))
                    .InsertCallback(_selectSequence.Duration() - 0.1f, _rewardCardPanel.Close)
                    //카드 선택으로
                    .InsertCallback(0.5f, CardSelectScene.Instance.StartCardUse);

                CardManager.Instance.AddCard(_cardSO);
            }
        }

        public void OnClickEnd(bool isLeft)
        {
        }

        public void OnCursorEnter()
        {
            _glitchTween.Clear();
            _glitchTween = DOTween.To(() => 0.6f,
                v => _childMaterialController.SetValue(_glitchValueHash, v), 0f, 0.15f);

            _hoverSequence.Clear();
            _hoverSequence = DOTween.Sequence();
            _hoverSequence.Append(RectTransform.DOScale(_defaultScale * new Vector2(1.3f, 1.3f), 0.2f));
        }

        public void OnCursorExit()
        {
            _hoverSequence.Clear();
            _hoverSequence = DOTween.Sequence();
            _hoverSequence.Append(RectTransform.DOScale(_defaultScale, 0.3f));
        }

        public void Wipe(int direction)
        {
            float x = direction == 1 ? Screen.width * 1.5f : -Screen.width * 0.5f;
            DOTween.To(() => _childMaterialController.GetValue(_glitchValueHash), v => _childMaterialController.SetValue(_glitchValueHash, v), 1f, 0.4f);
            RectTransform.DOAnchorPosX(x, 0.6f).SetEase(Ease.InBack);
        }

        private IEnumerator ReloadCoroutine(Vector2 destination, float duration, Action OnComplete = null)
        {
            Vector2 startPos = RectTransform.anchoredPosition;
            _childMaterialController.SetValue(_glitchValueHash, 1f);
            DOTween.To(() => _childMaterialController.GetValue(_glitchValueHash),
                v => _childMaterialController.SetValue(_glitchValueHash, v), 0, 0.7f);
            //Vector2 randomPos = Random.insideUnitCircle.normalized;
            //randomPos = destination + randomPos * (Screen.width * 0.5f);
            float toAdd = 1f / duration;
            float percent = 0;
            while (percent < 1f)
            {
                float t = MathEx.OutCirc(percent);
                //RectTransform.anchoredPosition = MathEx.Bezier(t, startPos, randomPos, destination);
                RectTransform.anchoredPosition = MathEx.Bezier(t, startPos, destination);
                yield return null;
                percent += Time.deltaTime * toAdd;
            }
            OnComplete?.Invoke();
        }

        private void OnDestroy()
        {
            _setDefaultTween?.Kill(true);
            _hoverSequence?.Kill(true);
            _selectSequence?.Kill(true);
        }
    }
}
