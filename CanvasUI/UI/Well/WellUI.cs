using System;
using UnityEngine;

namespace Hashira.CanvasUI.Well
{
    public class WellUI : UIBase, IToggleUI
    {
        [field: SerializeField] public string Key { get; set; }

        [SerializeField] private CustomButton _exitButton;

        public void Open()
        {
            gameObject.SetActive(true);
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }

        protected override void Awake()
        {
            base.Awake();
            _exitButton.OnClickEvent += HandleExitClickEvent;
        }

        private void Start()
        {
            Close();
        }

        private void HandleExitClickEvent()
        {
            Close();
        }

        private void OnDestroy()
        {
            UIManager.Instance.RemoveUI(this);
        }
    }
}
