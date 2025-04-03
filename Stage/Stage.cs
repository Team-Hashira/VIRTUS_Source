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
        [HideInInspector] public string name;
        public float delay = 1;
        public Enemy[] enemies;
        [FormerlySerializedAs("ClearEvent")]
        public UnityEvent ClearEvent;

        private int _enemyCount = 0;
        private Stage _owner;

        public void Init(Stage stage)
        {
            _enemyCount = enemies.Length;
            _owner = stage;

            foreach (Enemy enemy in enemies)
            {
                enemy.GetEntityComponent<EntityHealth>().OnDieEvent += HandleEnemyCounting;
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
                for (int i = 0; i < enemies.Length; i++)
                {
                    if (enemies[i] is Boss boss)
                    {
                        _owner.StartCoroutine(DelaySpawn(enemies[i], 0f));
                    }
                    else
                    {
                        spawnEffectList.Add(PopCore.Pop(EffectPoolType.EnemySpawnMark, enemies[i].transform.position, Quaternion.identity));
                        _owner.StartCoroutine(DelaySpawn(enemies[i], 1f));
                        yield return waitForSeconds;
                    }
                }   
            }
            else
            {
                for (int i = 0; i < enemies.Length; i++)
                {
                    enemies[i].gameObject.SetActive(false);
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
        [field:SerializeField] public Vector2 Scale { get; private set; } = new Vector2(15, 15);
        [field:SerializeField] public Vector2 Center { get; private set; } = Vector2.zero;
        [Space]
        [SerializeField] private Transform _playerSpawnPoint;
        [SerializeField] private Portal[] _portals;
        public Wave[] waves;
        [SerializeField] private bool _useCanvas;
        [ToggleField(nameof(_useCanvas)), SerializeField] private Transform _stagePanel;

        protected StageGenerator _stageGenerator;
        public int CurrentEnemiesCount => EnemyList.Count;
        public int CurrentWaveCount { get; private set; } = 0;
        //public UnityEvent OnAllClearEvent;

        public event Action OnWaveChanged;

        public List<Enemy> EnemyList { get; private set; } = new List<Enemy>();

        public Wave GetCurrentWave() => waves[CurrentWaveCount];

#if UNITY_EDITOR

        private void OnValidate()
        {
            for (int i = 0; i < waves.Length; i++)
                waves[i].name = $"Wave{i}";
        }
#endif

        public Enemy[] GetEnabledRandomEnemies(int count)
        {
            List<Enemy> curEnemyList = GetEnabledEnemies().ToList();

            int finalCount = Mathf.Clamp(count, 0, curEnemyList.Count);
            Enemy[] finalEnemies = new Enemy[finalCount];

            for (int i = 0; i < finalCount; i++)
            {
                int random = UnityEngine.Random.Range(0, curEnemyList.Count);

                finalEnemies[i] = curEnemyList[random];
                curEnemyList.RemoveAt(random);
            }

            return finalEnemies;
        }

        public Enemy[] GetEnabledEnemies()
        {
            return EnemyList.Where(x => x.gameObject.activeSelf.Equals(true)).ToArray();
        }

        private void Awake()
        {
            // 적 리스트 초기화
            for (int i = 0; i < waves.Length; i++)
                EnemyList.AddRange(waves[i].enemies);

            foreach (var portal in _portals)
                portal.gameObject.SetActive(false);

            if (_useCanvas)
                UIManager.Instance.AddGameCanvas(_stagePanel);
        }

        private void OnDestroy()
        {
            if (_useCanvas && _stagePanel != null)
                Destroy(_stagePanel.gameObject);
        }

        private void Start()
        {
            // 이벤트 구독
            for (int i = 0; i < waves.Length; i++)
            {
                waves[i].Init(this);
                StartCoroutine(waves[i].SetActiveAllEnemies(false));
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

        public void SetPlayerPosToSpawnPoint()
        {
            PlayerManager.Instance.Player.transform.position = _playerSpawnPoint.position;
        }

        public void ClearStage()
        {
            Cost.AddCost(15);
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
                for (int j = 0; j < waves[i].enemies.Length; j++)
                {
                    if (waves[i].enemies[j] == null) return;
                    var pos = waves[i].enemies[j].gameObject.transform.position + new Vector3(0, 1);
                    Handles.Label(pos, $"wave {i}");
                }
            }

            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(Center, Scale*2);
            Gizmos.DrawLine(new Vector3(-Scale.x, Center.y), new Vector3(Scale.x, Center.y));
            Gizmos.DrawLine(new Vector3(Center.x, -Scale.y), new Vector3(Center.x, Scale.y));
            Gizmos.color = Color.white;
        }
#endif
    }
}