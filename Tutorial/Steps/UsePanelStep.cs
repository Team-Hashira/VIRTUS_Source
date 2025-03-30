using Doryu.CustomAttributes;
using Hashira.Pathfind;
using System;
using UnityEngine;

namespace Hashira.Tutorials
{
    [Serializable]
    public struct PanelRect
    {
        public float top, down, left, right;
        public DirectionType pivot;
        [Header("Unused if Pivot is Zero")]
        public Vector2 size;

        private float _Top => 1080f - top;
        private float _Down => down;
        private float _Left => left;
        private float _Right => 1920f - right;

        public Vector2 GetPosition()
        {
            switch (pivot)
            {
                case DirectionType.Zero:
                    return GetCenterPosition();
                case DirectionType.Up:
                    return new Vector2(1920f / 2, _Top);
                case DirectionType.UpperRight:
                    return new Vector2(_Right, _Top);
                case DirectionType.Right:
                    return new Vector2(_Right, 1080f / 2);
                case DirectionType.LowerRight:
                    return new Vector2(_Right, _Down);
                case DirectionType.Down:
                    return new Vector2(1920f / 2, _Down);
                case DirectionType.LowerLeft:
                    return new Vector2(_Left, _Down);
                case DirectionType.Left:
                    return new Vector2(_Left, 1080f / 2);
                case DirectionType.UpperLeft:
                    return new Vector2(_Left, _Top);
                default:
                    return Vector2.zero;
            }
        }

        public Vector2 GetSize()
        {
            return size;
        }

        public Vector2 GetCenterPosition()
        {
            Vector2 panelSize = GetSize();

            if (pivot == DirectionType.Zero)
            {
                return Vector2.zero;
            }
            else
            {
                // 피봇 위치에 따라 중앙 위치를 계산
                Vector2 pivotPos = GetPosition();
                Vector2 finalPos = Vector2.zero;

                switch (pivot)
                {
                    case DirectionType.Up:
                        finalPos = new Vector2(pivotPos.x, pivotPos.y - panelSize.y * 0.5f);
                        break;
                    case DirectionType.UpperRight:
                        finalPos = new Vector2(pivotPos.x - panelSize.x * 0.5f, pivotPos.y - panelSize.y * 0.5f);
                        break;
                    case DirectionType.Right:
                        finalPos = new Vector2(pivotPos.x - panelSize.x * 0.5f, pivotPos.y);
                        break;
                    case DirectionType.LowerRight:
                        finalPos = new Vector2(pivotPos.x - panelSize.x * 0.5f, pivotPos.y + panelSize.y * 0.5f);
                        break;
                    case DirectionType.Down:
                        finalPos = new Vector2(pivotPos.x, pivotPos.y + panelSize.y * 0.5f);
                        break;
                    case DirectionType.LowerLeft:
                        finalPos = new Vector2(pivotPos.x + panelSize.x * 0.5f, pivotPos.y + panelSize.y * 0.5f);
                        break;
                    case DirectionType.Left:
                        finalPos = new Vector2(pivotPos.x + panelSize.x * 0.5f, pivotPos.y);
                        break;
                    case DirectionType.UpperLeft:
                        finalPos = new Vector2(pivotPos.x + panelSize.x * 0.5f, pivotPos.y - panelSize.y * 0.5f);
                        break;
                    default:
                        finalPos = pivotPos;
                        break;
                }
                finalPos -= new Vector2(1920f * 0.5f, 1080f * 0.5f);

                return finalPos;
            }
        }
    }

    public class UsePanelStep : TutorialStep
    {
        [SerializeField]
        private bool _ifPanelStillExist;

        [ToggleField(nameof(_ifPanelStillExist))]
        [SerializeField]
        private string _existKey;

        [ToggleField(nameof(_ifPanelStillExist))]
        [SerializeField]
        private bool _doRelocate;

        [ToggleField(nameof(_ifPanelStillExist), false)]
        [SerializeField]
        private string _newKey;

        [SerializeField]
        private PanelRect _panelRect;

        private Vector2 _panelPosition;
        private Vector2 _panelSize;
        [SerializeField]
        protected float _duration;

        [SerializeField]
        private bool _onEndClosePanel;

        protected TutorialPanel _panel;


        public override void Initialize(TutorialManager tutorialManager)
        {
            base.Initialize(tutorialManager);
            _panelPosition = _panelRect.GetCenterPosition();
            _panelSize = _panelRect.GetSize();
        }

        public override void OnEnter()
        {
            base.OnEnter();
            if (_ifPanelStillExist)
            {
                _panel = _tutorialManager.GetTutorialPanel(_existKey);
                if (_panel == null)
                {
                    _panel = _tutorialManager.GenerateTutorialPanel(_existKey);
                    _panel.Open(_panelPosition, _panelSize, _duration);
                }
                else if (_doRelocate)
                    _panel.Relocate(_panelPosition, _panelSize, _duration);
            }
            else
            {
                _panel = _tutorialManager.GenerateTutorialPanel(_newKey);
                _panel.Open(_panelPosition, _panelSize, _duration);
            }
        }

        public override void OnExit()
        {
            base.OnExit();
            if (_onEndClosePanel)
                _panel.Close();
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(Vector2.zero, new Vector2(1920f, 1080f));
            Gizmos.DrawWireCube(_panelRect.GetCenterPosition(), _panelRect.GetSize());
        }
#endif
    }
}
