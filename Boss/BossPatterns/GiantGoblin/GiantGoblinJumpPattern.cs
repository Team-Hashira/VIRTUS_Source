using DG.Tweening;
using UnityEngine;

namespace Hashira.Bosses.Patterns
{
    public class GiantGoblinJumpPattern : GiantGoblinPattern
    {
        [SerializeField] private float _jumpPower = 2f;
        [SerializeField] private float _jumpDuration = 1f;

        public override void OnStart()
        {
            base.OnStart();

            var jumpEndPos = new Vector2(Player.transform.position.x, Transform.position.y);

            Transform.DOJump(jumpEndPos, _jumpPower , 1, _jumpDuration).SetEase(Ease.InSine).OnComplete(AttackByDistance);
        }
    }
}
