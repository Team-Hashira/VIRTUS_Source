using Hashira.CanvasUI;
using Hashira.Cards;
using TMPro;
using UnityEngine;

namespace Hashira
{
    public class CardDataArea : MonoBehaviour
    {
        [SerializeField] private SetupCardVisual _setupCardVisual;
        [SerializeField] private TextMeshProUGUI _name, _description, _cost;

        public void SetCard(CardSO cardSO)
        {
            _setupCardVisual.VisualSetup(cardSO);
            _cost.text = $"{3}C";
            _name.text = cardSO.displayName;
            _description.text = cardSO.Description;
        }
    }
}
