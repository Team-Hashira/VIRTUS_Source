using Hashira.CanvasUI;
using Hashira.CanvasUI.Wells;
using Hashira.Entities.Interacts;
using Hashira.Players;
using UnityEngine;

namespace Hashira.Object
{
    public class Well : KeyInteractObject
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
            WellUI wellUI = toggleDomain.OpenUI("WellUI") as WellUI;
            wellUI.Init(this);
            wellUI.Enable();
        }

        public void EventEnd()
        {
            _collider2D.enabled = false;
            OffInteractable();
        }
    }
}
