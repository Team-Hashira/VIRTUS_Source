using Hashira.Players;

namespace Hashira.Entities.Interacts
{
    public interface IInteractable
    {
		public bool CanInteraction { get; set; }
		public void Interaction(Player entity);
        public void OnInteractable();
        public void OffInteractable();
    }
    public interface IHoldInteractable
    {
		public bool CanInteraction { get; set; }
		public void HoldInteractionStart(Entity entity);
        public void HoldInteractionEnd();
    }
}
