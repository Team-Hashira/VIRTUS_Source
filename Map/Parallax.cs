using Hashira.Combat;
using UnityEngine;

namespace Hashira
{
    public class Parallax : MonoBehaviour
    {
        //[SerializeField] private Vector2 _weight;

        [SerializeField] private float _depth;
        [SerializeField] private float _moveDistance;

        private Vector3 _originPosition;
        private Vector3 _offset;

        private void Start()
        {
            _originPosition = transform.position;
            _offset = Vector3.zero;
        }

        private void Update()
        {
            Vector3 targetPos = Vector3.Lerp(_originPosition, Camera.main.transform.position, _depth);
            targetPos.z = 0;
            transform.position = targetPos + _offset;

            if (Mathf.Abs(transform.position.x - Camera.main.transform.position.x) > _moveDistance)
            {
                _offset += Vector3.right * _moveDistance * Mathf.Sign(Camera.main.transform.position.x - transform.position.x);
            }
        }
    }
}
