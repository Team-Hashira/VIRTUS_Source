using Hashira.Bosses.BillboardClasses;
using System.Collections;
using UnityEngine;

namespace Hashira.Bosses.Patterns
{
    [System.Serializable]
    public class GiantGoblinAttackPattern : BossPattern
    {
        [SerializeField] private float _attackDelay = 0.1f;

        public override void OnStart()
        {
            base.OnStart();
            Boss.StartCoroutine(CoroutineAttackDelay());
        }
        
        private IEnumerator CoroutineAttackDelay()
        {
            yield return new WaitForSeconds(_attackDelay);
            AttackByDistance();
        }
        
        private void AttackByDistance()
        {
            float closeAttackRange = Boss.BillboardValue<FloatValue>("CloseAttackRange").Value;
            float playerToDistance = Mathf.Abs(Player.transform.position.x - Transform.position.x);

            // 플레이어가 멀리있으면
            if (playerToDistance > closeAttackRange)
            {
                if (Random.Range(0f, 1f) < 0.5f)
                    EndPattern<GiantGoblinDashPattern>();
                else
                    EndPattern<GiantGoblinJumpPattern>();
            }
            else
            {
                EndPattern<GiantGoblinSwingPattern>();
            }
        }

        public override void OnEnd()
        {
            base.OnEnd();
        }
    }
}
