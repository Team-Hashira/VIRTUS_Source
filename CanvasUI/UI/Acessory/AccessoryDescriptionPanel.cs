using Hashira.Accessories;
using TMPro;
using UnityEngine;

namespace Hashira.CanvasUI.Accessories
{
    public class AccessoryDescriptionPanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private TextMeshProUGUI _passiveDescriptionText;
        [SerializeField] private TextMeshProUGUI _activeDescriptionText;

        public void Init(AccessorySO accessory)
        {
            _nameText.SetText(accessory.displayName);
            _passiveDescriptionText.SetText(accessory.passiveDescription);
            _activeDescriptionText.SetText(accessory.activeDescription);
        }
    }
}
