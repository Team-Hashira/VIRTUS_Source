using Crogen.CrogenPooling;
using Hashira.Enemies.PublicStates;
using UnityEngine;

namespace Hashira.Enemies.Slime
{
    public class Slime : Enemy
    {
        [field: SerializeField]
        public DamageCaster2D DamageCaster { get; private set; }
        [field: SerializeField]
        public float DashAttackRange { get; private set; }
        [field: SerializeField]
        public ProjectilePoolType SlimeBall { get; private set; }

        protected override void AfterIntiialize()
        {
            base.AfterIntiialize();
            _entityStateMachine.GetState<EnemyChasePublicState>().TargetState = "DetermineAttack";
        }
    }
}
