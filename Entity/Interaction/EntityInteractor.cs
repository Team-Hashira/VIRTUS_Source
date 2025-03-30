using Hashira.Entities.Interacts;
using Hashira.Players;
using System.Linq;
using UnityEngine;

namespace Hashira.Entities
{
    public class EntityInteractor : MonoBehaviour, IEntityComponent
    {
        public bool CanInteract { get; private set; }
        public IInteractable Interactable { get; private set; }
        public IHoldInteractable HoldInteractable { get; private set; }

        [SerializeField] private LayerMask _whatIsInteractable;
        [SerializeField] private float _radius;

        private Entity _entity;

        private bool _isClicked = false;
        private bool _isHolding = false;
        private float _holdTime = 0.2f;
        private float _holdStartTime = 0;

        public void Initialize(Entity entity)
        {
            _entity = entity;
        }

        public void Interact(bool isDown)
        {
            if (Interactable == null) return;
            if (_isClicked == isDown) return;

            _isClicked = isDown;
            if (isDown)
                _holdStartTime = Time.time;
            else
            {
                if (_isHolding == false)
                    Interactable?.Interaction(_entity as Player);
                else
                {
                    _isHolding = false;
                    HoldInteractable?.HoldInteractionEnd();
                }
            }
        }

        private void TargetInteractableUpdate()
        {
            RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, _radius, Vector2.zero, 0, _whatIsInteractable);
            IInteractable interactable = null;
            if (hits.Length > 0)
            {
                RaycastHit2D[] hitOrder = hits.ToList()
                    .Where(hit =>
                    {
                        hit.transform.TryGetComponent(out IInteractable interactable);
                        return interactable.CanInteraction;
                    })
                    .OrderBy(hit => (hit.transform.position - transform.position).sqrMagnitude).ToArray();
                if (hitOrder.Length > 0)
                {
                    RaycastHit2D hit = hitOrder[0];
                    interactable = hit.transform.GetComponent<IInteractable>();
                }
            }

            if (Interactable == interactable) return;

            Interactable?.OffInteractable();
            if (_isHolding) HoldInteractable?.HoldInteractionEnd();
            Interactable = interactable;
            HoldInteractable = interactable as IHoldInteractable;
            Interactable?.OnInteractable();

            _isClicked = false;
            _isHolding = false;
        }

        private void Update()
        {
            TargetInteractableUpdate();

            if (_isClicked && _holdStartTime + _holdTime < Time.time && _isHolding == false)
            {
                _isHolding = true;
                HoldInteractable?.HoldInteractionStart(_entity);
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, _radius);
        }
    }
}
