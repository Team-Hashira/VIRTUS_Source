using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Hashira.CanvasUI
{
    public class UIInteractor
    {
        private GraphicRaycaster _rayCaster;
        private EventSystem _eventSystem;

        public bool Interactable { get; set; } = true;

        public UIInteractor(GraphicRaycaster rayCaster)
        {
            _rayCaster = rayCaster;
            _eventSystem = EventSystem.current;
        }

        public bool IsOnUI(Vector3 cursorPosition, out GameObject ui)
        {
            ui = null;
            if (false == Interactable ) return false;
            
            PointerEventData evtData = new PointerEventData(_eventSystem)
            {
                position = cursorPosition
            };

            List<RaycastResult> results = new List<RaycastResult>();
            _rayCaster.Raycast(evtData, results);
            if(results.Count != 0)
            {
                ui = results[0].gameObject;
                return true;
            }
            return false;
        }

        public bool IsUIUnderCursor(out GameObject ui)
        {
            ui = null;
            if (false == Interactable) return false;

            PointerEventData evtData = new PointerEventData(_eventSystem)
            {
                position = UIManager.MousePosition
            };

            List<RaycastResult> results = new List<RaycastResult>();
            _rayCaster.Raycast(evtData, results);

            if (results.Count != 0)
            {
                ui = results[0].gameObject;
                return true;
            }
            return false;
        }
    }
}
