using Doryu.CustomAttributes;
using Hashira.GimmickSystem;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Hashira.Object
{
    public class AreaGimmickObject : MonoBehaviour, IGimmickObject
    {
        [field: SerializeField, Tooltip("Only Enter")] public GimmickSO GimmickSO { get; set; }
        [SerializeField] private UnityEvent _enterEvent;
        [SerializeField] private UnityEvent _exitEvent;
        [SerializeField] private bool _onlyOnce = false;
        private bool _isWorked = false;
        [SerializeField, ToggleField(nameof(_onlyOnce))] private bool _canDestroy = false;
        [SerializeField, ToggleField(nameof(_canDestroy))] private float _destroyDelay = 3f;
        [SerializeField] private Vector2 _size = Vector2.one;
        [SerializeField] private LayerMask _whatIsTarget;

        
        private Collider2D _hitTarget;

        private void FixedUpdate()
        {
            var target = Physics2D.OverlapBox(transform.position, _size, transform.eulerAngles.z, _whatIsTarget);
            if (_hitTarget != null && _hitTarget == target)
            {
                return;
            }

            if (target != _hitTarget)
            {
                if (_hitTarget == null)
                {
                    OnGimmick();
                    _enterEvent?.Invoke();
                }
                else
                    _exitEvent?.Invoke();

                _hitTarget = target;
            }
        }

        private void OnGimmick()
        {
            if (_onlyOnce && _isWorked) return;
            
            GimmickSO?.OnGimmick(this);
            _isWorked = true;
            
            if (_canDestroy) Destroy(gameObject, _destroyDelay);
        }

        private void OnDrawGizmos()
        {
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireCube(Vector3.zero, _size);
            Gizmos.color = Color.white;
            Gizmos.matrix = Matrix4x4.identity;
        }
        
        
    }
}
