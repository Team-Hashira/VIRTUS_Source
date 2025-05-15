using Crogen.CrogenPooling;
using DG.Tweening;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Hashira.Tutorials
{
    public class TutorialPanel : MonoBehaviour, IPoolingObject
    {
        public string OriginPoolType { get; set; }
        GameObject IPoolingObject.gameObject { get; set; }

        [SerializeField]
        private ChildrenMaterialController _materialController;
        [SerializeField]
        private RectTransform _backgroundImageRect;
        private Sequence _toggleSequence;

        [SerializeField]
        private TextMeshProUGUI _tutorialText;
        [SerializeField]
        private Image _tutorialImage;

        private readonly int _glitchValueHash = Shader.PropertyToID("_Value");

        public void Open(Vector2 position, Vector2 size, float duration = 0.5f, bool withTween = true, bool withGlitch = true)
        {
            _backgroundImageRect.anchoredPosition = position;
            Vector2 startSize = new Vector2(0, size.y * 0.1f);
            float halfDuration = duration * 0.5f;

            if (withTween)
            {
                _toggleSequence = DOTween.Sequence();
                _toggleSequence
                    .Append(DOVirtual.Float(startSize.x, size.x, halfDuration, value => _backgroundImageRect.sizeDelta = new Vector2(value, _backgroundImageRect.sizeDelta.y))).SetEase(Ease.OutCubic)
                    .Append(DOVirtual.Float(startSize.y, size.y, halfDuration, value => _backgroundImageRect.sizeDelta = new Vector2(_backgroundImageRect.sizeDelta.x, value))).SetEase(Ease.OutCubic);
            }
            if (withGlitch)
            {
                DOVirtual.Float(1f, 0, duration, value => _materialController.SetValue(_glitchValueHash, value));
            }
        }

        public void Relocate(Vector2 position, Vector2 size, float duration = 0.5f, bool withTween = true, bool withGlitch = true)
        {
            if (withTween)
            {
                _backgroundImageRect.DOAnchorPos(position, duration).SetEase(Ease.OutCubic);
                _backgroundImageRect.DOSizeDelta(size, duration).SetEase(Ease.OutCubic);
            }
            else
                _backgroundImageRect.sizeDelta = size;
            if (withGlitch)
            {
                DOVirtual.Float(0.5f, 0, duration, value => _materialController.SetValue(_glitchValueHash, value));
            }
        }

        public void Close(float duration = 0.5f, bool withTween = true, bool withGlitch = true)
        {
            Vector2 startSize = _backgroundImageRect.sizeDelta;
            float halfDuration = duration * 0.5f;

            DOVirtual.DelayedCall(duration, this.Push);
            if (withTween)
            {
                _toggleSequence = DOTween.Sequence();
                _toggleSequence
                    .Append(DOVirtual.Float(startSize.y, 0.3f, halfDuration, value => _backgroundImageRect.sizeDelta = new Vector2(_backgroundImageRect.sizeDelta.x, value))).SetEase(Ease.OutCubic)
                    .Append(DOVirtual.Float(startSize.x, 0, halfDuration, value => _backgroundImageRect.sizeDelta = new Vector2(value, _backgroundImageRect.sizeDelta.y))).SetEase(Ease.OutCubic);
            }
            if (withGlitch)
            {
                DOVirtual.Float(_materialController.GetValue(_glitchValueHash), 1f, duration * 0.5f, value => _materialController.SetValue(_glitchValueHash, value));
            }
        }

        public void SetText(string text, bool useTextBounds = false, bool withAnimation = true, float speed = 10f)
        {
            _tutorialText.text = text;
            if (useTextBounds)
            {
                _tutorialText.ForceMeshUpdate();
                _backgroundImageRect.sizeDelta = _tutorialText.textBounds.size;
            }
            if (withAnimation)
                _tutorialText.AnimateText(text, speed);
        }

        [Obsolete("일단 쓰지마세요.")]
        public void SetSprite(Sprite sprite, Vector2 size)
        {
            //_tutorialImage.sprite = sprite;
            //_tutorialImage.transform.localScale = size;
        }

        public void OnPop()
        {
            _tutorialText.text = "";
            _tutorialImage.sprite = null;
        }

        public void OnPush()
        {
            _toggleSequence?.Kill(true);
        }

        private void OnDestroy()
        {
            OnPush();
        }
    }
}
