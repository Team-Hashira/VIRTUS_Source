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
        {
            Vector3 rotatedOffset = RotateOffset(_edgeCheckerOffset, transform.rotation);
            return !Physics2D.OverlapBox(transform.position + rotatedOffset, _edgeCheckerSize, transform.eulerAngles.z, _whatIsGround);
        }

        public bool IsWallOnFront()
        {
            Vector3 rotatedOffset = RotateOffset(_wallCheckerOffset, transform.rotation);
            return Physics2D.OverlapBox(transform.position + rotatedOffset, _wallCheckerSize, transform.eulerAngles.z, _whatIsGround);
        }

        private Vector3 RotateOffset(Vector2 offset, Quaternion rotation)
        {
            offset.x *= _entityRenderer.FacingDirection;
            return rotation * offset;
        }

#if UNITY_EDITOR
        protected override void OnDrawGizmos()
        {
            base.OnDrawGizmos();

            float facingDir = _entityRenderer != null ? _entityRenderer.FacingDirection : 1;

            Quaternion rotation = transform.rotation;

            Vector3 edgeOffset = rotation * new Vector2(_edgeCheckerOffset.x * facingDir, _edgeCheckerOffset.y);
            Vector3 wallOffset = rotation * new Vector2(_wallCheckerOffset.x * facingDir, _wallCheckerOffset.y);

            Gizmos.color = Color.cyan;
            Gizmos.matrix = Matrix4x4.TRS(transform.position + edgeOffset, rotation, Vector3.one);
            Gizmos.DrawWireCube(Vector3.zero, _edgeCheckerSize);

            Gizmos.color = Color.magenta;
            Gizmos.matrix = Matrix4x4.TRS(transform.position + wallOffset, rotation, Vector3.one);
            Gizmos.DrawWireCube(Vector3.zero, _wallCheckerSize);

            Gizmos.matrix = Matrix4x4.identity;
        }
#endif

    }
}
