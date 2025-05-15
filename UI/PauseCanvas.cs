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

        private Sequence _seq;

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            _inputReader.OnPauseEvent += HandleOnPause;

            _canvasGroup.alpha = 0f;
            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            _seq?.Clear();
            _inputReader.OnPauseEvent -= HandleOnPause;
        }

        private void HandleOnPause()
        {
            if (_isFading == true) return;

            bool fadeIn = gameObject.activeSelf;
            float duration = 0.35f;
            _seq = DOTween.Sequence();
            _seq.AppendCallback(() =>
            {
                _isFading = true;
                _canvasGroup.interactable = false;
                gameObject.SetActive(true);
            });
            _seq.Append(_canvasGroup.DOFade(fadeIn ? 0 : 1, duration));
            _seq.AppendCallback(() =>
            {
                _canvasGroup.interactable = true;
                gameObject.SetActive(!fadeIn);
                _isFading = false;
            });
        }
    }
}
