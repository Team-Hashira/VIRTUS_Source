using DG.Tweening;
using UnityEngine;

namespace Hashira
{
    public static class FadeController
    {
        private static CanvasGroup _fadeCanvasGroup;
        
        public static void Fade(bool fadeOut, float duration = 0.2f)
        {
            _fadeCanvasGroup ??= GameObject.Find("Canvas/FadeImage").GetComponent<CanvasGroup>();
            var target = fadeOut ? 1f : 0f;
            _fadeCanvasGroup.alpha = 1-target;
            _fadeCanvasGroup
                .DOFade(target, duration)
                .SetUpdate(true);
        }
    }
}
