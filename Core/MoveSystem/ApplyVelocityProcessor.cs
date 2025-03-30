using UnityEngine;

namespace Hashira.Core.MoveSystem
{
    public class ApplyVelocityProcessor : MoveProcessor
    {
        public override Vector2 ProcessMove(Vector2 movement)
        {
            return _entityMover.ToMove;
        }
    }
}
