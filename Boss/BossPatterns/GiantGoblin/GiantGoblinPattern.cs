using Hashira.Bosses.BillboardClasses;
using UnityEngine;

namespace Hashira.Bosses.Patterns
{
    public class GiantGoblinPattern : BossPattern
    {
        public override void OnStart()
        {
            base.OnStart();
        }

        public override void OnEnd()
        {
            base.OnEnd();
        }

        public void AttackByDistance()
        {
            float closeAttackRange = Boss.BillboardValue<FloatValue>("CloseAttackRange").Value;
            float playerToDistance = Mathf.Abs(Player.transform.position.x - Transform.position.x);

            // 플레이어가 멀리있으면
            if (playerToDistance > closeAttackRange)
            {
                EndPattern<GiantGoblinDashPattern>();
            }
            else
            {
                EndPattern<GiantGoblinSwingPattern>();
            }
        }
    }
}
