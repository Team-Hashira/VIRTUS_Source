using DG.Tweening;
using Hashira.Accessories;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Hashira.CanvasUI.Accessories
{
    public class AccessorySlot : MonoBehaviour
    {
        [SerializeField] private EAccessoryType _accessoryType;
        [SerializeField] private Image _iconImage;

        public AccessorySO Accessory { get; private set; }
        private Tween _iconTween;
        
        public void Init(AccessorySO accessory)
        {
            Accessory = accessory;
            _iconImage.rectTransform.localScale = Vector3.one;
            _iconTween?.Kill(true);

            if (Accessory == null)
            {
                _iconTween = _iconImage.rectTransform.DOScale(Vector3.one, 0.5f)
                    .OnStart(()=>_iconImage.rectTransform.localScale = Vector3.zero)
                    .SetLoops(-1, LoopType.Restart);
            }
            else
            {
                _iconImage.sprite = accessory.sprite;
            }
        }
    }
}
