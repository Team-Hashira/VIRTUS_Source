using System.Collections.Generic;
using UnityEngine;

namespace Hashira.Entities.Components
{
    public class EntityPartsCollider : MonoBehaviour, IEntityComponent
    {
        [field: SerializeField] public List<Collider2D> Colliders { get; private set; }

        public void Initialize(Entity entity)
        {

        }

        public Collider2D[] GetRandomCollider(int count)
        {
            Collider2D[] result = Colliders.ToArray();
            int lastIdx = result.Length;
            while (lastIdx >= 0)
            {
                int index = Random.Range(0, lastIdx);
                Collider2D temp = result[index];
                result[index] = result[lastIdx - 1];
                result[lastIdx - 1] = temp;
                lastIdx--;
            }
            return result;
        }
        public Collider2D GetRandomCollider()
        {
            int index = Random.Range(0, Colliders.Count);
            return Colliders[index];
        }
    }
}
