using Crogen.CrogenPooling;
using Doryu.CustomAttributes;
using Hashira.Bosses;
using Hashira.Core;
using Hashira.Enemies;
using Hashira.Entities;
using Hashira.Entities.Interacts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Hashira.StageSystem
{
    [Serializable]
    public class Wave
    {
        [Serializable]
        public struct EnemyPair
        {
            public Enemy enemy;
            public Transform Transform => enemy.transform;  
            public GameObject GameObject => enemy.gameObject;  
            public bool ignore;
        }
        
        [HideInInspector] public string name;
        public float delay = 1;
        public EnemyPair[] enemyPairs;
        [FormerlySerializedAs("ClearEvent")]
        public UnityEvent ClearEvent;

        private int _enemyCount = 0;
        private Stage _owner;

        public void Init(Stage stage)
        {
            _enemyCount = enemyPairs.Count(x => x.ignore == false);
            _owner = stage;

            if (_enemyCount == 0)
            {
                ClearEvent?.Invoke();
                _owner.AddWaveCount();
                return;
            }
            
            foreach (EnemyPair pair in enemyPairs)
            {
                if (pair.ignore == false)
                    pair.enemy.GetEntityComponent<EntityHealth>().OnDieEvent += HandleEnemyCounting;
            }
        }

        private void HandleEnemyCounting(Entity entity)
        {
            --_enemyCount;
            _owner.EnemyList.Remove(entity as Enemy);
            if (_enemyCount > 0) return;

            ClearEvent?.Invoke();
            _owner.AddWaveCount();
        }

        public IEnumerator SetActiveAllEnemies(bool active, float delay = 0)
        {
            if (active)
            {
                WaitForSeconds waitForSeconds = new WaitForSeconds(delay);
                List<IPoolingObject> spawnEffectList = new List<IPoolingObject>();
                for (int i = 0; i < enemyPairs.Length; i++)
                {
                    // 이전에 미리 켜둔 놈이라면 굳이 건들지 않는다.
                    if (enemyPairs[i].GameObject.activeSelf)
                        continue;
                    
                    if (enemyPairs[i].enemy is Boss boss)
                    {
                        _owner.StartCoroutine(DelaySpawn(enemyPairs[i].enemy, 0f));
                    }
                    else
                    {
                        spawnEffectList.Add(PopCore.Pop(EffectPoolType.EnemySpawnMark, enemyPairs[i].Transform.position, Quaternion.identity));
                        _owner.StartCoroutine(DelaySpawn(enemyPairs[i].enemy, 1f));
                        yield return waitForSeconds;
                    }
                }   
            }
            else
            {
                for (int i = 0; i < enemyPairs.Length; i++)
                {
                    enemyPairs[i].GameObject.SetActive(false);
                }
            }
        }
        public IEnumerator DelaySpawn(Enemy enemy, float delay)
        {
            yield return new WaitForSeconds(delay);
            enemy.gameObject.SetActive(true);
            PopCore.Pop(EffectPoolType.EnemySpawnEffect, enemy.transform.position, Quaternion.identity);
        }
    }

    public class Stage : MonoBehaviour
    {
        [field:SerializeField] public Camera ScreenCamera { get; private set; }
        [field:SerializeField] public Vector2 Scale { get; private set; } = new Vector2(15, 15);
        [field:SerializeField] public Vector2 Center { get; private set; } = Vector2.zero;
        [Space]
        [SerializeField] private Transform _playerSpawnPoint;
        [SerializeField] private Portal[] _portals;
        public Wave[] waves;
        [SerializeField] private bool _useCanvas;
        [ToggleField(nameof(_useCanvas)), SerializeField] private Transform _stagePanel;

        public int CurrentEnemiesCount => EnemyList.Count;
        public int CurrentWaveCount { get; private set; } = 0;
        //public UnityEvent OnAllClearEvent;

        public event Action OnWaveChanged;

        public List<Enemy> EnemyList { get; private set; } = new List<Enemy>();
        public List<Enemy> IgnoredEnemyList { get; private set; } = new List<Enemy>();

        public Wave GetCurrentWave() => waves[CurrentWaveCount];

#if UNITY_EDITOR

        private void OnValidate()
        {
            for (int i = 0; i < waves.Length; i++)
                waves[i].name = $"Wave{i}";
        }
#endif

        public Enemy[] GetEnabledEnemies()
        {
            return EnemyList.Where(x => x.gameObject.activeSelf.Equals(true)).ToArray();
        }

        private void Awake()
        {
            // 적 리스트 초기화
            foreach (var wave in waves)
            {
                Enemy[] ignoreEnemies = wave.enemyPairs.Where(x => x.ignore == false).Select(x => x.enemy).ToArray();
                Enemy[] enemies = wave.enemyPairs.Select(x => x.enemy).ToArray();
                
                EnemyList.AddRange(enemies);
                IgnoredEnemyList.AddRange(ignoreEnemies);
            }

            foreach (var portal in _portals)
                portal.gameObject.SetActive(false);

            if (_useCanvas)
                UIManager.Instance.AddGameCanvas(_stagePanel);
        }

        private void Update()
        {
            if (ScreenCamera == null) return;

            ScreenCamera.orthographicSize = Mathf.Max(Scale.x, Scale.y);
            ScreenCamera.transform.position = new Vector3(Center.x, Center.y, -10f);
        }

        private void OnDestroy()
        {
            if (_useCanvas && _stagePanel != null)
                Destroy(_stagePanel.gameObject);
        }

        private void Start()
        {
            // 이벤트 구독
            foreach (var wave in waves)
            {
                wave.Init(this);
                StartCoroutine(wave.SetActiveAllEnemies(false));
            }

            SetPlayerPosToSpawnPoint();
            AddWaveCount(0);
        }

        public void AddWaveCount(int value = 1)
        {
            StartCoroutine(CoroutineAddWaveCount(value));
        }

        private IEnumerator CoroutineAddWaveCount(int value)
        {
            CurrentWaveCount += value;
            if (CurrentWaveCount >= waves.Length)
            {
                ClearStage();
                //OnAllClearEvent?.Invoke();
            }
            else
            {
                OnWaveChanged?.Invoke();
                yield return new WaitForSeconds(waves[CurrentWaveCount].delay);
                StartCoroutine(waves[CurrentWaveCount].SetActiveAllEnemies(true, 0.1f));
            }
        }

        private void SetPlayerPosToSpawnPoint()
        {
            PlayerManager.Instance.Player.transform.position = _playerSpawnPoint.position;
        }

        private void ClearStage()
        {
            List<StageTypeSO> stageTypeSOList = StageGenerator.Instance.GetNextStageData().GetRandomStageType(_portals.Length);
            for (int i = 0; i < stageTypeSOList.Count; i++)
            {
                _portals[i].gameObject.SetActive(true);
                _portals[i].Init(stageTypeSOList[i]);
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            for (int i = 0; i < waves.Length; i++)
            {
                for (int j = 0; j < waves[i].enemyPairs.Length; j++)
                {
                    if (waves[i].enemyPairs[j].enemy == null) continue;
                    var pos = waves[i].enemyPairs[j].Transform.position + new Vector3(0, 1);
                    Handles.Label(pos, $"wave {i}");

                    if (waves[i].enemyPairs[j].ignore)
                    {
                        Gizmos.color = new Color(1, 0, 0, 0.5f);
                        Gizmos.DrawSphere(waves[i].enemyPairs[j].Transform.position, 1);
                        Gizmos.color = Color.white;
                    }
                }
            }

            if (_playerSpawnPoint != null)
                Handles.Label(_playerSpawnPoint.position + new Vector3(0, 1), $"PlayerSpawnPoint");
            
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(Center, Scale*2);
            Gizmos.DrawLine(new Vector3(-Scale.x, Center.y), new Vector3(Scale.x, Center.y));
            Gizmos.DrawLine(new Vector3(Center.x, -Scale.y), new Vector3(Center.x, Scale.y));
            Gizmos.color = Color.white;
        }
#endif
    }
}