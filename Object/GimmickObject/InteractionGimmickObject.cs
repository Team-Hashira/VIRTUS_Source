using Doryu.CustomAttributes;
using Hashira.Entities.Interacts;
using Hashira.GimmickSystem;
using Hashira.Players;
using UnityEngine;
using UnityEngine.Events;

namespace Hashira.Object
{
    public class InteractionGimmickObject : KeyInteractObject, IGimmickObject
    {
        [field:SerializeField] public GimmickSO GimmickSO { get; set; }
        [SerializeField] private bool _onlyOnce = false;
        private bool _isWorked = false;
        [SerializeField, ToggleField(nameof(_onlyOnce))] private bool _canDestroy = false;
        [SerializeField, ToggleField(nameof(_canDestroy))] private float _destroyDelay = 3f;
        [SerializeField] private UnityEvent _interactionEvent;


        public override void Interaction(Player player)
        {
            if (_onlyOnce && _isWorked) return;
            
            base.Interaction(player);
            _isWorked = true;
            _interactionEvent?.Invoke();
            GimmickSO?.OnGimmick(this);
            
            if (_canDestroy) Destroy(gameObject, _destroyDelay);
        }
    }
}
