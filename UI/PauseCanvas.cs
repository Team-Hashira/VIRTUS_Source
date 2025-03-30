using DG.Tweening;
using System;
using UnityEngine;

namespace Hashira.UI
{
    public class PauseCanvas : MonoBehaviour
    {
        [SerializeField] private InputReaderSO _inputReader;

        private CanvasGroup _canvasGroup;
        private bool _isFading = false;

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            _inputReader.OnPauseEvent += HandleOnPause;

            _canvasGroup.alpha = 0f;
            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            _inputReader.OnPauseEvent -= HandleOnPause;
        }

        private void HandleOnPause()
        {
            if (_isFading == true) return;

            bool fadeIn = gameObject.activeSelf;
            float duration = 0.35f;
            Sequence seq = DOTween.Sequence();
            seq.AppendCallback(() =>
            {
                _isFading = true;
                _canvasGroup.interactable = false;
                gameObject.SetActive(true);
            });
            seq.Append(_canvasGroup.DOFade(fadeIn ? 0 : 1, duration));
            seq.AppendCallback(() =>
            {
                _canvasGroup.interactable = true;
                gameObject.SetActive(!fadeIn);
                _isFading = false;
            });
        }
    }
}
