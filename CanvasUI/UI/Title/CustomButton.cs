using DG.Tweening;
using Doryu.CustomAttributes;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Hashira.CanvasUI
{
    public class CustomButton : UIBase, IClickableUI, IHoverableUI
    {
        private readonly static int _ValueShaderHash = Shader.PropertyToID("_Value");

        [SerializeField] private bool _isUseMove; 
        [SerializeField, ToggleField(nameof(_isUseMove))] private float _moveDistance;
        [SerializeField, ToggleField(nameof(_isUseMove))] private float _moveDuration;

        [SerializeField] private bool _isUseSize;
        [SerializeField, ToggleField(nameof(_isUseSize))] private Vector2 _sizeModify;
        [SerializeField, ToggleField(nameof(_isUseSize))] private float _sizeDuration;

        [SerializeField] private bool _isUseLight;
        [SerializeField, ToggleField(nameof(_isUseLight))] private Image _image;
        [SerializeField, ToggleField(nameof(_isUseLight))] private TextMeshProUGUI _text;
        [SerializeField, ToggleField(nameof(_isUseLight))] private float _lightOffDuration;

        [SerializeField] private bool _isUseGlitch;
        [SerializeField, ToggleField(nameof(_isUseGlitch))] private ChildrenMaterialController _childrenMaterialController;
        [SerializeField, ToggleField(nameof(_isUseGlitch))] private float _glitchDuration;

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

        protected override void Awake()
        {
            base.Awake();
            _defaultSize = RectTransform.sizeDelta;
            if (_isUseLight)
            {
                _defaultColor = _image.color;
                if (_text != null) _defaultTextColor = _text.color;
            }
            _defaultAnchoredPos = RectTransform.anchoredPosition;
        }

        public void OnClick(bool isLeft)
        {
            if (isLeft)
                OnClickEvent?.Invoke();
            Light();
        }

        public void OnClickEnd(bool isLeft)
        {

        }

        public void OnCursorEnter()
        {
            OnHoverEvent?.Invoke(true);
            if (_isUseMove)
            {
                if (_moveTween != null && _moveTween.IsActive()) _moveTween.Kill();
                _moveTween = RectTransform.DOAnchorPosX(_defaultAnchoredPos.x + _moveDistance, _moveDuration).SetEase(Ease.OutExpo).SetUpdate(_isDonUseTimeScale);
            }
            if (_isUseSize)
            {
                if (_sizeTween != null && _sizeTween.IsActive()) _sizeTween.Kill();
                _sizeTween = RectTransform.DOSizeDelta(_defaultSize + _sizeModify, _sizeDuration).SetEase(Ease.OutExpo).SetUpdate(_isDonUseTimeScale);
            }

            Light();
            Glitch();
        }

        private void Light()
        {
            if (_isUseLight == false) return;
            if (_colorSeq != null && _colorSeq.IsActive()) _colorSeq.Kill();
            if (_text != null) _text.color = Color.white;
            _image.color = Color.white;
        }

        private void Glitch()
        {
            if (_isUseGlitch == false) return;
            _childrenMaterialController.SetValue(_ValueShaderHash, 1f);
            if (_materialTween != null && _materialTween.IsActive()) _materialTween.Kill();
            _materialTween = DOTween.To(() => 1f, value => _childrenMaterialController.SetValue(_ValueShaderHash, value), 0f, _glitchDuration).SetUpdate(_isDonUseTimeScale);
        }
        
        public void OnCursorExit()
        {
            OnHoverEvent?.Invoke(false);
            if (_isUseLight)
            {
                if (_colorSeq != null && _colorSeq.IsActive()) _colorSeq.Kill();
                _colorSeq = DOTween.Sequence().SetUpdate(_isDonUseTimeScale);
                _colorSeq.Append(_image.DOColor(_defaultColor, _lightOffDuration));
                if (_text != null) _colorSeq.Join(_text.DOColor(_defaultTextColor, _lightOffDuration));
            }

            if (_isUseMove)
            {
                if (_moveTween != null && _moveTween.IsActive()) _moveTween.Kill();
                _moveTween = RectTransform.DOAnchorPosX(_defaultAnchoredPos.x, _moveDuration).SetUpdate(_isDonUseTimeScale);
            }

            if (_isUseSize)
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
