using Hashira.Bosses;
using Hashira.Cards;
using Hashira.Core;
using Hashira.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Hashira.StageSystem
{
    public class StageGenerator : MonoSingleton<StageGenerator>
    {
        public static int currentFloorIdx = 0;
        public static int currentStageIdx = 0;
        public static int CurrentStageCount => Instance.floors[currentFloorIdx].stages.Length;

        [SerializeField] private FloorSO[] floors;
        private Stage _currentStage;

        public event Action OnFloorUpEvent;
        public event Action OnNextStageEvent;
        public event Action OnGeneratedStageEvent;

        public Stage GetCurrentStage() => _currentStage;
        public void SetCurrentStage(Stage stage) => _currentStage = stage;
        public int GetCurrentEnemiesCount() => _currentStage.CurrentEnemiesCount;

        public void GenerateStage(StageTypeSO stages = null)
        {
            if (floors.Length == 0) return;
            if (stages == null)
            {
                StageTypeSO stageSO = GetCurrentStageData().GetRandomStageType(1)[0];
                if (stageSO.isSceneChange)
                {
                    EntityHealth entityHealth = PlayerManager.Instance.Player.GetEntityComponent<EntityHealth>();
                    PlayerDataManager.Instance.SetHealth(entityHealth.Health, entityHealth.MaxHealth);
                    SceneLoadingManager.LoadScene(stageSO.sceneName);
                }
                else
                {
                    _currentStage = Instantiate(stageSO.GetRandomStage(), transform);
                }
            }
            else
            {
                _currentStage = Instantiate(stages.GetRandomStage(), transform);
            }

            PlayerManager.Instance.Player.Rotate(0, 0);

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
            OnGeneratedStageEvent?.Invoke();
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

        public bool IsCurrentBossStage()
        {
            return IsBossStage(GetCurrentStage());
        }

        public bool IsBossStage(Stage stage)
        {
            Boss boss = stage?.EnemyList.FirstOrDefault(x => x is Boss) as Boss;
            return boss != null;
        }

        public void NextStage(StageTypeSO stages)
        {
            bool isNextFloor = false;
            ++currentStageIdx;
            OnNextStageEvent?.Invoke();
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
                ResetStage();
                SceneLoadingManager.LoadScene(SceneName.CardSendScene);
            }
            else
            {
                if (stages.isSceneChange)
                {
                    EntityHealth entityHealth = PlayerManager.Instance.Player.GetEntityComponent<EntityHealth>();
                    PlayerDataManager.Instance.SetHealth(entityHealth.Health, entityHealth.MaxHealth);
                    SceneLoadingManager.LoadScene(stages.sceneName);
                }
                else
                {
                    StartCoroutine(NextStageCoroutine(stages));
                }
            }
        }

        private IEnumerator NextStageCoroutine(StageTypeSO stages)
        {
            Destroy(_currentStage.gameObject);
            yield return new WaitForEndOfFrame();
            GenerateStage(stages);
            yield return null;
            List<CardSO> cardSOList = PlayerDataManager.Instance.CardEffectList.Select(cardEffect => cardEffect.CardSO).ToList();
            PlayerManager.Instance.ReEnableCardEffect();
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
