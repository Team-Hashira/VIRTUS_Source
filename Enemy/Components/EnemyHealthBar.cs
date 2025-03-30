using Hashira.Entities;
using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace Hashira.Enemies.Components
{
    public class EnemyHealthBar : MonoBehaviour, IEntityComponent
    {
        private Enemy _enemy;
        private EntityHealth _entityHealth;

        [SerializeField]
        private Transform _pivotTransform;


        public void Initialize(Entity entity)
        {
            _enemy = entity as Enemy;
            _entityHealth = entity.GetEntityComponent<EntityHealth>();

            _entityHealth.OnHealthChangedEvent += HandleOnHealthChangedEvent;
        }

        private void HandleOnHealthChangedEvent(int previous, int current)
        {
            float ratio = current != 0 ? current / (float)_entityHealth.MaxHealth : 0;
            _pivotTransform.localScale = new Vector3(ratio, 1);
        }
    }
}
