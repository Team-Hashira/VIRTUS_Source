using Hashira.Entities;
using UnityEngine;

namespace Hashira.Enemies.Components
{
    public class EnemyMover : EntityMover
    {
        [Header("Ground check setting mk.2")]
        [SerializeField]
        private Vector2 _edgeCheckerSize;
        [SerializeField]
        private Vector2 _wallCheckerSize;
        [SerializeField]
        private Vector2 _edgeCheckerOffset;
        [SerializeField]
        private Vector2 _wallCheckerOffset;

        private EntityRenderer _entityRenderer;

        [SerializeField]
        private int _jumpableWallCount;

        public override void Initialize(Entity entity)
        {
            base.Initialize(entity);
            _entityRenderer = entity.GetEntityComponent<EntityRenderer>();
        }

        //protected override void FixedUpdate()
        //{
        //    base.FixedUpdate();
        //    Debug.Log(IsGrounded);
        //}

        public bool IsOnEdge()
            => !Physics2D.OverlapBox(transform.position + new Vector3(_edgeCheckerOffset.x * _entityRenderer.FacingDirection, _edgeCheckerOffset.y), _edgeCheckerSize, 0, _whatIsGround);
        public bool IsWallOnFront()
            => Physics2D.OverlapBox(transform.position + new Vector3(_wallCheckerOffset.x * _entityRenderer.FacingDirection, _wallCheckerOffset.y), _wallCheckerSize, 0, _whatIsGround);
        public bool IsWallJumpable()
            => !Physics2D.Raycast(transform.position + new Vector3(0, _jumpableWallCount), new Vector2(_entityRenderer.FacingDirection, 0), _checkerSize.x / 2 + 0.2f, _whatIsGround);

#if UNITY_EDITOR
        protected override void OnDrawGizmos()
        {
            base.OnDrawGizmos();
            float facingDir = 1;
            if (_entityRenderer != null)
                facingDir = _entityRenderer.FacingDirection;
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireCube(transform.position + new Vector3(_edgeCheckerOffset.x * facingDir, _edgeCheckerOffset.y), _edgeCheckerSize);
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireCube(transform.position + new Vector3(_wallCheckerOffset.x * facingDir, _wallCheckerOffset.y), _wallCheckerSize);
        }
#endif
    }
}
