using DG.Tweening;
using UnityEngine;

namespace Hashira
{
    public static class DOTweenUtility
    {
        public static Tween Clear(this Tween tween)
        {
            if (tween != null && tween.IsActive())
                tween.Kill();

            return tween;
        }
    }
}
