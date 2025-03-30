using Hashira.Entities;
using Hashira.Entities.Components;
using UnityEngine;

namespace Hashira.FSM
{
    public abstract class EntityState
    {
        protected Entity _entity;
        public StateSO StateSO { get; private set; }
        protected EntityStateMachine _entityStateMachine;
        protected EntityAnimator _entityAnimator;

        public EntityState(Entity entity, StateSO stateSO)
        {
            _entity = entity;
            StateSO = stateSO;
            _entityStateMachine = entity.GetEntityComponent<EntityStateMachine>();
            _entityAnimator = entity.GetEntityComponent<EntityAnimator>();
        }

        public virtual void OnEnter()
        {
            _entityAnimator?.ClearAnimationTriggerDictionary();
            _entityAnimator?.SetParam(StateSO.animatorParam, true);
        }

        public virtual void OnUpdate() { }

        public virtual void OnExit()
        {
            _entityAnimator?.SetParam(StateSO.animatorParam, false);
        }
    }
}
