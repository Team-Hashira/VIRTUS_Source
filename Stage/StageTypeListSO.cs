using AYellowpaper.SerializedCollections;
using Hashira.StageSystem;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Hashira
{
    [CreateAssetMenu(fileName = "StageTypeList", menuName = "SO/Level/StageTypeList")]
    public class StageTypeListSO : ScriptableObject
    {
        [SerializeField] private SerializedDictionary<StageTypeSO, float> _stageTypePercent;

        public List<StageTypeSO> GetRandomStageType(int count)
        {
            List<StageTypeSO> resultTypeSOList = new List<StageTypeSO>();
            List<StageTypeSO> stageTypeSOList = _stageTypePercent.Keys.ToList();

            for (int i = 0; i < count; i++)
            {
                if (stageTypeSOList.Count == 0) break;

                // 전체 가중치
                float totalValue = 0;
                foreach (StageTypeSO stageTypeSO in stageTypeSOList)
                    totalValue += _stageTypePercent[stageTypeSO];

                // 가중치에 비례한 Random뽑기
                float random = Random.Range(0f, totalValue);
                float currentValue = 0;
                foreach (StageTypeSO stageTypeSO in stageTypeSOList)
                {
                    currentValue += _stageTypePercent[stageTypeSO];
                    if (random <= currentValue)
                    {
                        resultTypeSOList.Add(stageTypeSO);
                        break;
                    }
                }

                //뽑힌애 제외
                stageTypeSOList.Remove(resultTypeSOList[i]);
            }

            return resultTypeSOList;
        }
    }
}
