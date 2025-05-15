using Crogen.CrogenPooling;
using UnityEngine;

namespace Hashira.Cards.Effects
{
    public class HurricaneCard : MagicCardEffect
    {
        protected float[] _delayTimeByStack = { 18f, 15f, 15f, 12f, 12f };
        protected int[] _damageByStack = { 20, 20, 25, 25, 25 };
        protected override float DelayTime => _delayTimeByStack[stack - 1];

        public override void OnUse()
        {
            Collider2D[] collider2D = Physics2D.OverlapCircleAll(player.transform.position, 300f, 1 << LayerMask.NameToLayer("Enemy"));
            if (collider2D.Length > 0)
            {
                Hurricane hurricaneFirst = PopCore.Pop
                    (CardSubPoolType.Hurricane, collider2D[0].transform.position, Quaternion.identity) as Hurricane;
                hurricaneFirst.Init(_damageByStack[stack - 1]);
                if (IsMaxStack)
                {
                    Hurricane hurricaneSecond = PopCore.Pop
                        (CardSubPoolType.Hurricane, collider2D[collider2D.Length >= 2 ? 1 : 0].transform.position, Quaternion.identity) as Hurricane;
                    hurricaneSecond.Init(_damageByStack[stack - 1]);
                }
            }
        }
    }
}
