using Hashira.Entities;

namespace Hashira.EffectSystem
{
    public abstract class Effect
    {
        public string name;
        public bool Visable { get; private set; } = true;

        public Entity entity;
        public EntityEffector entityEffector;
        public EntityStat entityStat;

        public virtual void Enable()
        {

        }

        public virtual void Update()
        {
        }

        public virtual void Disable()
        {
        }
    }
}
