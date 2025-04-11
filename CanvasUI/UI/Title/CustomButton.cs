using DG.Tweening;
using Doryu.CustomAttributes;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Hashira.CanvasUI
{
    public class CustomButton : UIBase, IClickableUI, IHoverableUI
    {
        private readonly static int _ValueShaderHash = Shader.PropertyToID("_Value");

        [FormerlySerializedAs("_isUseMove")]
        public bool isUseMove;
        [SerializeField, ToggleField(nameof(isUseMove))] private float _moveDistance;
        [SerializeField, ToggleField(nameof(isUseMove))] private float _moveDuration;

        [FormerlySerializedAs("_isUseSize")]
        public bool isUseSize;
        [SerializeField, ToggleField(nameof(isUseSize))] private Vector2 _sizeModify;
        [SerializeField, ToggleField(nameof(isUseSize))] private float _sizeDuration;

        [FormerlySerializedAs("_isUseLight")]
        public bool isUseLight;
        [SerializeField, ToggleField(nameof(isUseLight))] private Image _image;
        [SerializeField, ToggleField(nameof(isUseLight))] private TextMeshProUGUI _text;
        [SerializeField, ToggleField(nameof(isUseLight))] private Color lightColor = Color.white;
        [SerializeField, ToggleField(nameof(isUseLight))] private float _lightOffDuration;

        [FormerlySerializedAs("_isUseGlitch")]
        public bool isUseGlitch;
        [SerializeField, ToggleField(nameof(isUseGlitch))] private ChildrenMaterialController _childrenMaterialController;
        [SerializeField, ToggleField(nameof(isUseGlitch))] private float _glitchDuration;

        [SerializeField] private bool _isDonUseTimeScale = false;

        private Color _defaultColor;
        private Vector2 _defaultSize;
        private Color _defaultTextColor;
        private Vector2 _defaultAnchoredPos;
        private Tween _moveTween;
        private Tween _sizeTween;
        private Tween _materialTween;
        private Sequence _colorSeq;

        public event Action OnClickEvent;
        public event Action<bool> OnHoverEvent;

        private bool _onHoverEvent = true;
        private bool _onClickEvent = true;

        protected override void Awake()
        {
            base.Awake();
            _defaultSize = RectTransform.sizeDelta;
            if (isUseLight)
            {
                _defaultColor = _image.color;
                if (_text != null) _defaultTextColor = _text.color;
            }
            _defaultAnchoredPos = RectTransform.anchoredPosition;
        }

        public void SetText(string text)
        {
            if (_text != null) _text.text = text;
        }

        public void ActiveHoverEvent(bool active)
        {
            OnCursorExit();
            _onHoverEvent = active;
        }
        public void ActiveClickEvent(bool active)
        {
            OnClickEnd(true);
            OnClickEnd(false);
            _onClickEvent = active;
        }

        public void OnClick(bool isLeft)
        {
            if (_onClickEvent == false) return;

            if (isLeft)
                OnClickEvent?.Invoke();
        }

        public void OnClickEnd(bool isLeft)
        {
            if (_onClickEvent == false) return;

        }

        public void OnCursorEnter()
        {
            if (_onHoverEvent == false) return;
            OnHoverEvent?.Invoke(true);
            if (isUseMove)
            {
                if (_moveTween != null && _moveTween.IsActive()) _moveTween.Kill();
                _moveTween = RectTransform.DOAnchorPosX(_defaultAnchoredPos.x + _moveDistance, _moveDuration).SetEase(Ease.OutExpo).SetUpdate(_isDonUseTimeScale);
            }
            if (isUseSize)
            {
                if (_sizeTween != null && _sizeTween.IsActive()) _sizeTween.Kill();
                _sizeTween = RectTransform.DOSizeDelta(_defaultSize + _sizeModify, _sizeDuration).SetEase(Ease.OutExpo).SetUpdate(_isDonUseTimeScale);
            }

            Light();
            Glitch();
        }

        private void Light()
        {
            if (isUseLight == false) return;
            if (_colorSeq != null && _colorSeq.IsActive()) _colorSeq.Kill();
            if (_text != null) _text.color = lightColor;
            _image.color = lightColor;
        }

        private void Glitch()
        {
            if (isUseGlitch == false) return;
            _childrenMaterialController.SetValue(_ValueShaderHash, 1f);
            if (_materialTween != null && _materialTween.IsActive()) _materialTween.Kill();
            _materialTween = DOTween.To(() => 1f, value => _childrenMaterialController.SetValue(_ValueShaderHash, value), 0f, _glitchDuration).SetUpdate(_isDonUseTimeScale);
        }

        public void OnCursorExit()
        {
            if (_onHoverEvent == false) return;
            OnHoverEvent?.Invoke(false);
            if (isUseLight)
            {
                if (_colorSeq != null && _colorSeq.IsActive()) _colorSeq.Kill();
                _colorSeq = DOTween.Sequence().SetUpdate(_isDonUseTimeScale);
                _colorSeq.Append(_image.DOColor(_defaultColor, _lightOffDuration));
                if (_text != null) _colorSeq.Join(_text.DOColor(_defaultTextColor, _lightOffDuration));
            }

            if (isUseMove)
            {
                if (_moveTween != null && _moveTween.IsActive()) _moveTween.Kill();
                _moveTween = RectTransform.DOAnchorPosX(_defaultAnchoredPos.x, _moveDuration).SetUpdate(_isDonUseTimeScale);
            }

            if (isUseSize)
            {
                if (_sizeTween != null && _sizeTween.IsActive()) _sizeTween.Kill();
                _sizeTween = RectTransform.DOSizeDelta(_defaultSize, _sizeDuration).SetUpdate(_isDonUseTimeScale);
            }
        }

        private void OnDestroy()
        {
            if (_moveTween != null && _moveTween.IsActive()) _moveTween.Kill();
            if (_colorSeq != null && _colorSeq.IsActive()) _colorSeq.Kill();
            if (_materialTween != null && _materialTween.IsActive()) _materialTween.Kill();
            if (_sizeTween != null && _sizeTween.IsActive()) _sizeTween.Kill();
        }
    }
}
