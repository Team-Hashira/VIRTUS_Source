using Hashira.CanvasUI.Wells;
using Hashira.CanvasUI;
using Hashira.Entities.Interacts;
using Hashira.Players;
using UnityEngine;
using Hashira.CanvasUI.Stores;

namespace Hashira.Object
{
    public class Store : KeyInteractObject
    {
        private ToggleDomain toggleDomain;
        private Collider2D _collider2D;

        protected override void Awake()
        {
            base.Awake();
            _collider2D = GetComponent<Collider2D>();
        }

        public override void Interaction(Player player)
        {
            base.Interaction(player);

            toggleDomain = Hashira.CanvasUI.UIManager.Instance.GetDomain<ToggleDomain>();
            StoreUI storeUI = toggleDomain.OpenUI("StoreUI") as StoreUI;
        }
    }
}
