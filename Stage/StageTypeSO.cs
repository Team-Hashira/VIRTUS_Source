using Doryu.CustomAttributes;
using UnityEngine;

namespace Hashira.StageSystem
{
    [CreateAssetMenu(fileName = "StageType", menuName = "SO/Level/StageType")]
    public class StageTypeSO : ScriptableObject
    {
        public string stageName;
        public Color stageColor;

        public bool isSceneChange;
        [ToggleField(nameof(isSceneChange))] public string sceneName;

        public Stage[] stagePrefabs;

        public Stage GetRandomStage()
        {
            return stagePrefabs[Random.Range(0, stagePrefabs.Length)];
        }
    }
}
