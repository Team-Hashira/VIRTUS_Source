#if UNITY_EDITOR
using UnityEditor;

namespace Hashira.Bosses.Editor
{
    [CustomEditor(typeof(Boss))]
    public class BossEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        }
    }
}

#endif