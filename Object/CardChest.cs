using Hashira.CanvasUI;
using Hashira.Cards;
using Hashira.Items;
using Hashira.Players;
using UnityEngine;

namespace Hashira.Entities.Interacts
{
    public class CardChest : KeyInteractObject
    {
        [SerializeField] private CardSetSO _cardSetSO;

        public override void Interaction(Player player)
        {
            base.Interaction(player);

            CardSO cardSO = _cardSetSO.GetRandomCardList(1, CardManager.Instance.GetCardList())[0];
            CardManager.Instance.AddCard(cardSO);

            CardGetVisual cardGetVisual = Hashira.CanvasUI.UIManager.Instance.GetDomain<ToggleDomain>()
                .OpenUI(nameof(CardGetVisual)) as CardGetVisual;
            cardGetVisual.SetCard(cardSO);

            Destroy(gameObject);
        }
    }
}
