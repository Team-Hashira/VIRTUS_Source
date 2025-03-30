using UnityEngine;

namespace Hashira.Core.MoveSystem
{
    public class XSmoothProcessor : MoveProcessor
    {
        public float Speed { get; set; } = 2f;

        public override Vector2 ProcessMove(Vector2 movement)
        {
            float x = Mathf.Lerp(movement.x, _entityMover.ToMove.x, Time.fixedDeltaTime * Speed);
            movement = new Vector2(x, movement.y);
            return movement;
        }
    }
}
