using Hashira.CanvasUI;
using System;
using UnityEngine;

namespace Hashira
{
    public class StoreButton : MonoBehaviour
    {
        [SerializeField] private CustomButton _storeCustomButton;

        private void Awake()
        {
            _storeCustomButton.OnClickEvent += HandleClickEvent;
        }

        private void HandleClickEvent()
        {
            PopupTextManager.Instance.PopupText("아직 구현되지 않은 기능입니다.", Color.white);
        }
    }
}
