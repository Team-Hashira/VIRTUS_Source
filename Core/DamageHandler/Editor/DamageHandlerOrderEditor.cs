using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Hashira.Core.DamageHandler
{
    [EditorWindowTitle(title = "Damage Handler Order")]
    public class DamageHandlerOrderEditor : EditorWindow
    {
        private List<EDamageHandlerLayer> _layerOrderList;

        private ReorderableList _reorderableList;

        [MenuItem("Game Setting/Edit Damage Handler Order")]
        public static void ShowWindow()
        {
            EditorWindow window = EditorWindow.GetWindow(typeof(DamageHandlerOrderEditor));
            window.Show();
            window.minSize = new Vector2(300, 300);
            window.maxSize = new Vector2(300, 300);
        }

        private void OnEnable() 
        {
            _layerOrderList = DamageHandlerOrder.OrderList;
            _reorderableList = new ReorderableList(_layerOrderList, typeof(EDamageHandlerLayer), true, true, false, false)
            {
                drawHeaderCallback = (Rect rect) =>
                {
                    EditorGUI.LabelField(rect, "Damage Handler Sorting Layer");
                },

                drawElementCallback = (Rect rect, int index, bool isActive, bool isForced) =>
                {
                    GUI.enabled = false;
                    _layerOrderList[index] = (EDamageHandlerLayer)EditorGUI.EnumPopup(rect, _layerOrderList[index]);
                    GUI.enabled = true;
                },

                onChangedCallback = (ReorderableList list) =>
                {
                    DamageHandlerOrder.Reorder(_layerOrderList);
                },
            };
        }

        private void OnGUI()
        {
            _reorderableList.DoLayoutList();
            if(GUILayout.Button("Initialize"))
            {
                DamageHandlerOrder.Initialize();
            }
        }
    }
}
