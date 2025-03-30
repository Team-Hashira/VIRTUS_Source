using Hashira.Entities;

namespace Hashira.Bosses
{
    public class BossMover : EntityMover
    {
        public override void Initialize(Entity entity)
        {
            base.Initialize(entity);
        }

        public float GetDownDistance()
        {
            return _downDistance;
        }

#if UNITY_EDITOR
        protected override void OnDrawGizmos()
        {
            base.OnDrawGizmos();
        }
#endif
    }
}
