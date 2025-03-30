using Hashira.Entities;
using UnityEngine;

namespace Hashira.Players
{
    public class PlayerMover : EntityMover
    {
        public bool CanRolling => _currentDelayTime >= rollingDelay;
        public float rollingDelay = 1;
        private float _currentDelayTime = 0;
		public bool IsSprint { get; private set; } = false;

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

		public void OnSprintToggle()
		{
			IsSprint = !IsSprint;
		}

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            Vector3 rigidVelocity = Rigidbody2D.linearVelocity;
            _lastVelocityValue = rigidVelocity.magnitude;
            if (rigidVelocity.x != 0)
            {
                _entityRenderer.LookTarget(transform.position + rigidVelocity);
            }
        }

        public void OnCollision(Collision2D collision)
        {
        }
    }
}