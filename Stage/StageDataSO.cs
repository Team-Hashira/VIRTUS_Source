using UnityEngine;

namespace Hashira.StageSystem
{
    [CreateAssetMenu(fileName = "StageData", menuName = "SO/StageData")]
    public class StageDataSO : ScriptableObject
    {
        public string stageName;
        public Stage[] stagePrefabs;

        public Stage GetRandomStage()
        {
            return stagePrefabs[Random.Range(0, stagePrefabs.Length)];
        }
    }
}
