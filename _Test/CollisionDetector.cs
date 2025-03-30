using UnityEngine;

namespace Hashira
{
    public class CollisionDetector : MonoBehaviour
    {
        private void OnCollisionEnter2D(Collision2D collision)
        {
            Debug.Log($"Collision Detect with {collision.gameObject.name}");
        }
    }
}
