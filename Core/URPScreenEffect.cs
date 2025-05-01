using DG.Tweening;
using System;
using UnityEngine;

namespace Hashira
{
    public static class URPScreenEffect
    {
        private static readonly int LoadingFadeValueID = Shader.PropertyToID("_LoadingFadeValue");
        
        public static void Fade(bool fadeIn, float duration, Action onComplete = null)
        {
            float startValue = fadeIn ? 1f : 0f;
            float endValue = fadeIn ? 0f : 1f;
            
            DOTween.To(x=> Shader.SetGlobalFloat(LoadingFadeValueID, x), startValue, endValue, duration)
                .SetUpdate(true)
                .OnComplete(()=>onComplete?.Invoke());
        }
    }
}