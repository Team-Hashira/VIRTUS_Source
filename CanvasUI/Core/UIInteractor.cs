using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace Hashira.CanvasUI
{
    public class UIInteractor : MonoBehaviour
    {
        [SerializeField]
        private GraphicRaycaster _rayCatster;
        private EventSystem _eventSystem;

        private void Awake()
        {
            _eventSystem = EventSystem.current;
        }

        public bool IsOnUI(Vector3 cursorPosition, out GameObject ui)
        {
            ui = null;
            PointerEventData evtData = new PointerEventData(_eventSystem)
            {
                position = cursorPosition
            };

            List<RaycastResult> results = new List<RaycastResult>();
            _rayCatster.Raycast(evtData, results);
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
            PointerEventData evtData = new PointerEventData(_eventSystem)
            {
                position = UIManager.MousePosition
            };

            List<RaycastResult> results = new List<RaycastResult>();
            _rayCatster.Raycast(evtData, results);

            if (results.Count != 0)
            {
                ui = results[0].gameObject;
                return true;
            }
            return false;
        }
    }
}
