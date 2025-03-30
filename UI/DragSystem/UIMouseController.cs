using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Hashira.UI.DragSystem
{
    public class UIMouseController : MonoSingleton<UIMouseController>
    {
        public Canvas canvas;
        [SerializeField] private InputReaderSO _inputReader;
        private static GraphicRaycaster _graphicRaycaster; // UI가 포함된 Canvas에 연결된 GraphicRaycaster
        private static EventSystem _eventSystem;           // EventSystem 오브젝트
        private static Vector2 MousePosition { get; set; } 
        private IDraggableObject _currentDragObject;
        private ISelectableObject _currentSelectedObject;
        public ISelectableObject CurrentSelectedObject
        {
            get
            {
                if (CanSelect == false)
                    _currentSelectedObject = null;
                return _currentSelectedObject;
            }
        }

        public bool IsDragging { get; private set; } = false;

        public bool CanSelect { get; set; } = true;
        public bool CanDrag { get; private set; } = true;

        private void Awake()
        {
            _graphicRaycaster = canvas.GetComponent<GraphicRaycaster>();
            _eventSystem = EventSystem.current;

            _inputReader.OnClickEvent += HandleDrag;
            _inputReader.OnMouseMoveEvent += HandleSelect;
        }

        private void OnDestroy()
        {
            _inputReader.OnClickEvent -= HandleDrag;
            _inputReader.OnMouseMoveEvent -= HandleSelect;
        }

        private void HandleDrag(bool isMouseDown)
        {
            if (CanDrag == false) return;

            if (isMouseDown)
            {
                var rayCastResult = GetUIUnderCursor();
                if (rayCastResult == null) return;

                if (!rayCastResult[0].gameObject.TryGetComponent(out IDraggableObject draggableObject)) return;
                if (draggableObject.CanDrag == false) return;
                _currentDragObject = draggableObject;
                _currentDragObject.DragStartPosition = _currentDragObject.RectTransform.position;
                _currentDragObject?.OnDragStart();
                IsDragging = true;
            }
            else
            {
                if(_currentDragObject == null) return;
                _currentDragObject.DragEndPosition = _currentDragObject.RectTransform.position;
                _currentDragObject?.OnDragEnd(MousePosition);
                _currentDragObject = null;
                IsDragging = false;
            }
        }

        private void HandleSelect(Vector2 mousePos)
        {
            if (CanSelect == false) return;
            var rayCastResult = GetUIUnderCursor();
            if (rayCastResult == null) return;
            ISelectableObject selectableObject = null;

            for (int i = 0; i < rayCastResult.Count; i++)
            {
                selectableObject = rayCastResult[i].gameObject.GetComponent<ISelectableObject>();
                if (selectableObject != null) break;
            }

            if (selectableObject != _currentSelectedObject)
            {
                _currentSelectedObject?.OnSelectEnd();
                _currentSelectedObject = selectableObject;
                _currentSelectedObject?.OnSelectStart();
                return;
            }
        }

        public static List<RaycastResult> GetUIUnderCursor()
        {
            PointerEventData pointerEventData = new PointerEventData(_eventSystem)
            {
                position = MousePosition // 마우스 위치 설정
            };

            List<RaycastResult> results = new List<RaycastResult>();
            _graphicRaycaster.Raycast(pointerEventData, results);

            if (results.Count == 0 || results[0].gameObject == null) return null;

            return results;
        }

        private void Update()
        {
            // 마우스 위치 
            MousePosition = _inputReader.MousePosition;
            
            if (_currentDragObject == null) return;
            _currentDragObject.RectTransform.position = MousePosition;
            _currentDragObject.OnDragging(MousePosition);
        }

        public void ResetDrag()
        {
            _currentDragObject = null;
            IsDragging = false;
        }
    }
}