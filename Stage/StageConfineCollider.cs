using Unity.Cinemachine;
using UnityEngine;

namespace Hashira.StageSystem
{
    public class StageConfineCollider : MonoBehaviour
    {
        [SerializeField] private PolygonCollider2D _confineCollider;
        [SerializeField] private BoxCollider2D _sideColliderR;
        [SerializeField] private BoxCollider2D _sideColliderL;

        [SerializeField] private float _xOffset = 2f;
        private CinemachineConfiner2D _cinemachineConfiner;
        
        public void SetConfine(Vector2 min, Vector2 max)
        {
            _confineCollider.points = new Vector2[]
            {
                new(min.x+_xOffset, max.y),
                new(min.x+_xOffset, min.y),
                new(max.x-_xOffset, min.y),
                new(max.x-_xOffset, max.y)
            };
            
            _sideColliderR.size = new Vector2(1, 10000);
            _sideColliderL.size = new Vector2(1, 10000);
            
            _sideColliderR.offset = new Vector2(max.x-0.5f, 0);
            _sideColliderL.offset = new Vector2(min.x+0.5f, 0);
            
            _cinemachineConfiner ??= FindFirstObjectByType<PlayerCamera>().CinemachineConfiner2D;
            _cinemachineConfiner.BoundingShape2D = _confineCollider;
        }
    }
}
