using Hashira.Entities;
using UnityEngine;

namespace Hashira.Players
{
    public class PlayerMover : EntityMover
    {
        public bool CanRolling => _currentDelayTime >= rollingDelay;
        public float rollingDelay = 1;
        private float _currentDelayTime = 0;

        private float _lastVelocityValue;
        private EntityRenderer _entityRenderer;

		private void Update()
		{
            if(_currentDelayTime < rollingDelay)
                _currentDelayTime += Time.deltaTime;
		}

		public override void Initialize(Entity entity)
        {
            base.Initialize(entity);
            _entityRenderer = entity.GetEntityComponent<EntityRenderer>();
        }

        public void OnDash()
        {
            _currentDelayTime = 0;
		}

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            Vector3 rigidVelocity = Rigidbody2D.linearVelocity;
            if (Vector3.ProjectOnPlane(rigidVelocity.normalized, transform.up) != Vector3.zero)
            {
                _entityRenderer.LookTarget(transform.position + rigidVelocity);
            }
        }

        public void OnCollision(Collision2D collision)
        {
        }
    }
}