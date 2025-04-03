using UnityEngine;

namespace Hashira.Core.MoveSystem
{
    public class XSmoothProcessor : MoveProcessor
    {
        public float Speed { get; set; } = 2f;

        public bool OnlyIn { get; set; } = false;
        public bool OnlyOut { get; set; } = false;

        public override Vector2 ProcessMove(Vector2 movement)
        {
            float x = Mathf.Lerp(movement.x, _entityMover.ToMove.x, Time.fixedDeltaTime * Speed);
            if (OnlyIn && movement.x > _entityMover.ToMove.x)
                x = _entityMover.ToMove.x;
            else if (OnlyOut && movement.x < _entityMover.ToMove.x)
                x = _entityMover.ToMove.x;
            movement = new Vector2(x, movement.y);
            return movement;
        }
    }
}
