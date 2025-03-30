using System;
using UnityEngine;

namespace Hashira.CanvasUI
{
    public delegate void OnToggleEvent(bool isOpen);

    public interface IToggleUI : IUserInterface
    {
        public string Key { get; set; }

        public void Open();
        public void Close();
    }
}
