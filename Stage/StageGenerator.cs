using Hashira.Bosses;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace Hashira.StageSystem
{
    public class StageGenerator : MonoSingleton<StageGenerator>
    {
        public static int currentFloorIdx = 0;
        public static int currentStageIdx = 0;

        [SerializeField] private FloorSO[] floors;
        private Stage _currentStage;

        public event Action OnFloorUpEvent;
        public event Action OnNextStageEvent;

        public Stage GetCurrentStage() => _currentStage;
        public int GetCurrentEnemiesCount() => _currentStage.CurrentEnemiesCount;

        public void GenerateStage()
        {
            if (floors.Length == 0) return;
            _currentStage = Instantiate(GetCurrentStageData().GetRandomStageType(1)[0].GetRandomStage(), transform);
            //_currentStage.OnAllClearEvent.AddListener(() =>
            //{
            //    if (currentFloorIdx == floors.Length - 1 && currentStageIdx == floors[currentFloorIdx].Length - 1)
            //    {
            //        // 임시
            //        GameManager.Instance.GameOver();
            //    }
            //    else
            //        ClearStage();
            //});
        }

        public StageTypeListSO GetCurrentStageData()
        {
            return floors[currentFloorIdx][currentStageIdx];
        }

        public StageTypeListSO GetNextStageData(int addStage = 1)
        {
            int stage = currentStageIdx + addStage;
            int floor = currentFloorIdx;

            if (stage >= floors[floor].stages.Length)
            {
                stage -= floors[floor].stages.Length;
                floor++;
            }
            return floors[floor][stage];
        }

        public bool IsCurrentBossStage(out Boss boss)
        {
            return IsBossState(GetCurrentStage(), out boss);
        }

        public bool IsBossState(Stage stage, out Boss boss)
        {
            boss = stage?.EnemyList.FirstOrDefault(x => x is Boss) as Boss;
            return boss != null;
        }

        public void NextStage(StageTypeSO stages)
        {
            bool isNextFloor = false;
            ++currentStageIdx;
            if (currentStageIdx >= floors[currentFloorIdx].stages.Length)
            {
                OnFloorUpEvent?.Invoke();
                currentFloorIdx++;
                currentStageIdx = 0;
                if (currentFloorIdx >= floors.Length)
                {
                    Debug.Log("Clear");
                    //SceneLoadingManager.LoadScene("ClearScene");
                }
                else
                {
                    isNextFloor = true;
                }
            }
            if (isNextFloor)
            {
                SceneLoadingManager.LoadScene(SceneName.CardSendScene);
            }
            else
            {
                if (stages.isSceneChange)
                    SceneLoadingManager.LoadScene(stages.sceneName);
                else
                    StartCoroutine(NextStageCoroutine(stages));
            }
        }

        private IEnumerator NextStageCoroutine(StageTypeSO stages)
        {
            Destroy(_currentStage.gameObject);
            yield return new WaitForEndOfFrame();
            _currentStage = Instantiate(stages.GetRandomStage(), transform);
        }

        //private IEnumerator ClearStageCoroutine()
        //{
        //    yield return new WaitForSeconds(1f);

        //    MainScreenEffect.OnAlpha(0, 0.75f, Ease.InSine);
        //    MainScreenEffect.OnGlitch(1f, 0.5f, Ease.InCubic);
        //    MainScreenEffect.OnScaling(0.5f, 1.15f, Ease.Unset);

        //    yield return new WaitForSeconds(1.5f);

        //    bool isNextFloor = false;
        //    ++currentStageIdx;
        //    if (currentStageIdx >= floors[currentStageIdx].stages.Length)
        //    {
        //        OnFloorUpEvent?.Invoke();
        //        currentFloorIdx++;
        //        currentStageIdx = 0;
        //        if (currentFloorIdx >= floors.Length)
        //        {
        //            Debug.Log("Clear");
        //            //SceneLoadingManager.LoadScene("ClearScene");
        //        }
        //        else
        //        {
        //            isNextFloor = true;
        //        }
        //    }

        //    OnNextStageEvent?.Invoke();
        //    GameManager.Instance.ClearStage();
        //    Cost.AddCost(Cost.ClearCost + Cost.GetBonusCost());
        //    Destroy(_currentStage.gameObject);
        //    if (isNextFloor)
        //        SceneLoadingManager.LoadScene(SceneName.CardSendScene);
        //    else
        //        SceneLoadingManager.LoadScene(SceneName.CardSelectScene);
        //}

        public static void ResetStage()
        {
            currentStageIdx = 0;
            currentFloorIdx = 0;
        }
    }
}
