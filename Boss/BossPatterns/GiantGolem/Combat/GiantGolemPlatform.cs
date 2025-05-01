using DG.Tweening;
using UnityEngine;

namespace Hashira.Bosses.Patterns.GiantGolem
{
    public class GiantGolemPlatform : MonoBehaviour
    {
        public Vector2 OriginPosition {get; private set;}

        private void Awake()
        {
            OriginPosition= transform.position;
        }

        public Tween MoveYRandom(float duration = 2f, float min = 0f, float max = 1f)
        {
            return MoveY(Random.Range(min, max), duration);
        }

        public Tween MoveY(float posY, float duration = 2f)
        {
            return transform.DOMoveY(posY, duration);
        }

        public Tween MoveToOrigin(float duration = 2f)
        {
            return transform.DOMove(OriginPosition, duration);
        }

        public Tween Shake(float duration = 0.25f, float strength = 15f, int vibrato = 10)
        {
            return transform.DOShakeRotation(duration, strength, vibrato, fadeOut:true)
                .SetEase(Ease.OutQuad)
                .OnComplete(() =>
            {
                transform.rotation = Quaternion.identity;
            });
        }
    }
}
