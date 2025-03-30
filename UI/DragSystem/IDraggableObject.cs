using UnityEngine;

namespace Hashira.UI.DragSystem
{
    public interface IDraggableObject
    {
        public bool CanDrag { get; }
        public Vector2 DragStartPosition { get; set; }
        public Vector2 DragEndPosition { get; set; }
        public RectTransform RectTransform { get; set; }
        public void OnDragStart();
        public void OnDragging(Vector2 curPos);
        public void OnDragEnd(Vector2 curPos);
    }
}