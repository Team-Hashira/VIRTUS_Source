using Hashira.Entities;
using UnityEngine;

namespace Hashira.Core.MoveSystem
{
    public abstract class MoveProcessor
    {
        protected EntityMover _entityMover;

        public virtual void Initialize(EntityMover entityMover)
        {
            _entityMover = entityMover;
        }

        public abstract Vector2 ProcessMove(Vector2 movement);
    }
}
