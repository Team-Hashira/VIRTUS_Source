using UnityEngine;

namespace Hashira.Core.MoveSystem
{
    public class YSmoothProcessor : MoveProcessor
    {
        public float Speed { get; set; } = 2f;

        public bool OnlyIn { get; set; } = false;
        public bool OnlyOut { get; set; } = false;

        public override Vector2 ProcessMove(Vector2 movement)
        {
            float y = Mathf.Lerp(movement.y, _entityMover.ToMove.y, Time.fixedDeltaTime * Speed);
            if (OnlyIn && movement.x > _entityMover.ToMove.y)
                y = _entityMover.ToMove.y;
            else if (OnlyOut && movement.y < _entityMover.ToMove.y)
                y = _entityMover.ToMove.y;
            movement = new Vector2(movement.x, y);
            return movement;
        }
    }
}
