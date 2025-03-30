using System;
using UnityEngine;

namespace Hashira.Entities.Components
{
    public class EntityShield : MonoBehaviour, IEntityComponent
    {
        private Entity _entity;

        private EntityRenderer _entityRenderer;

        [SerializeField]
        private Vector2 _offset;

        private void OnValidate()
        {
            transform.localPosition = _offset;
        }

        public void Initialize(Entity entity)
        {
            _entity = entity;

            _entityRenderer = entity.GetEntityComponent<EntityRenderer>();
        }

        private void Update()
        {
            transform.position = _entity.transform.position + new Vector3(_offset.x * _entityRenderer.FacingDirection, _offset.y);
            transform.rotation = Quaternion.Euler(0, 90 - 90 * _entityRenderer.FacingDirection, 0);
        }
    }
}
