using UnityEngine;

namespace Hashira.StageSystem
{
    public class EndStagePiece : Stage
    {
        [SerializeField] private Vector2 _offset, _size;
        [SerializeField] private LayerMask _whatIsPlayer;

        private void Update()
        {
            Collider2D[] colliders = new Collider2D[1];
            colliders = Physics2D.OverlapBoxAll(transform.position + (Vector3)_offset, 
                _size * (Vector2)transform.localScale, 0, _whatIsPlayer);
            if (colliders.Length != 0)
            {
                //_stageGenerator.Clear();
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(transform.position + (Vector3)_offset, 
                _size * (Vector2)transform.localScale);
        }
    }
}
