using Hashira.Bosses;
using Hashira.Cards;
using Hashira.Combat;
using Hashira.Core;
using Hashira.Entities;
using System;
using System.Collections;
using Doryu.CustomAttributes;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Hashira.StageSystem
{
    public class StageGenerator : MonoSingleton<StageGenerator>
    {
        public static int currentFloorIdx = 0;
        public static int currentStageIdx = 0;
        public static int CurrentStageCount => Instance.floors[currentFloorIdx].stages.Length;

        [VisibleInspectorSO]
        [SerializeField] private FloorSO[] floors;
        private Stage _currentStage;

        public event Action OnFloorUpEvent;
        public event Action OnNextStageEvent;
        public event Action OnGeneratedStageEvent;
        [SerializeField] private FollowTarget _followTarget;
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

            //_followTarget?.SetTarget(_currentStage.transform);
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
                    SceneLoadingManager.LoadScene(SceneName.TitleScene);
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
            PlayerManager.Instance.ReEnableCardEffect();
        }

        public static void ResetStage()
        {
            currentStageIdx = 0;
            currentFloorIdx = 0;
        }
    }
}
