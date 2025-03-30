using UnityEngine;

namespace Hashira.Core.MoveSystem
{
    public class YSmoothProcessor : MoveProcessor
    {
        public float Speed { get; set; } = 2f;

        public override Vector2 ProcessMove(Vector2 movement)
        {
            float y = Mathf.Lerp(movement.y, _entityMover.ToMove.y, Time.fixedDeltaTime * Speed);
            movement = new Vector2(movement.x, y);
            return movement;
        }
    }
}
