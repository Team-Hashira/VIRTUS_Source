using Doryu.CustomAttributes;
using Hashira.Combat;
using Hashira.GimmickSystem;
using UnityEngine;
using UnityEngine.Serialization;

namespace Hashira.Object
{
    public class HittableGimmickObject : MonoBehaviour, IGimmickObject, IDamageable
    {
        [field:SerializeField] public GimmickSO GimmickSO { get; set; }
        [SerializeField] private bool _onlyOnce = false;
        private bool _isWorked = false;
        [SerializeField, ToggleField(nameof(_onlyOnce))] private bool _canDestroy = false;
        [SerializeField, ToggleField(nameof(_canDestroy))] private float _destroyDelay = 3f;
        [SerializeField] private int _maxHealth = 1;
        private int _currentHealth = 0;

        public void ApplyDamage(AttackInfo attackInfo, RaycastHit2D raycastHit, bool popUpText = true)
        {
            if (_onlyOnce && _isWorked) return;

            _currentHealth += attackInfo.damage;
            if (_maxHealth > _currentHealth) return;
            GimmickSO.OnGimmick(this);
            _isWorked = true;
            _currentHealth = 0;
            
            if (_canDestroy) Destroy(gameObject, _destroyDelay);
        }
    }
}
