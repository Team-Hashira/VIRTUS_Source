using UnityEngine;

namespace Hashira.Core.MoveSystem
{
    public class XYSmoothProcessor : MoveProcessor
    {
        public float Speed { get; set; } = 2f;

        public bool OnlyIn { get; set; } = false;
        public bool OnlyOut { get; set; } = false;

        public override Vector2 ProcessMove(Vector2 movement)
        {
            movement = Vector2.Lerp(movement, _entityMover.ToMove, Time.fixedDeltaTime * Speed);
            if (OnlyIn && movement.sqrMagnitude > _entityMover.ToMove.sqrMagnitude)
                movement = _entityMover.ToMove;
            else if (OnlyOut && movement.sqrMagnitude < _entityMover.ToMove.sqrMagnitude)
                movement = _entityMover.ToMove;
            return movement;
        }
    }
}
