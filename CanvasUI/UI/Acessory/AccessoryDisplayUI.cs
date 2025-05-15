using Hashira.Accessories;
using UnityEngine;
using UnityEngine.UI;

namespace Hashira.CanvasUI
{
    public class AccessoryDisplayUI : MonoBehaviour
    {
        [SerializeField]
        private Image _accessoryImage;

        public void Display(AccessorySO accessory)
        {
            _accessoryImage.sprite = accessory?.sprite;
            if (_accessoryImage.sprite == null)
                _accessoryImage.color = Color.clear;
            else
                _accessoryImage.color = Color.white;
        }
    }
}
