using Hashira.CanvasUI;
using Hashira.Players;
using UnityEngine;

namespace Hashira.Entities.Interacts
{
    public class Well : KeyInteractObject
    {
        private ToggleDomain toggleDomain;

        public override void Interaction(Player player)
        {
            base.Interaction(player);

            toggleDomain = Hashira.CanvasUI.UIManager.Instance.GetDomain<ToggleDomain>();
            toggleDomain.OpenUI("WellUI");
        }
    }
}
