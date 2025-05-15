using Crogen.CrogenPooling;
using Doryu.CustomAttributes;
using Hashira.Bosses;
using Hashira.CanvasUI;
using Hashira.Core;
using Hashira.Enemies;
using Hashira.Entities;
using Hashira.Entities.Interacts;
using Hashira.GimmickSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

namespace Hashira.StageSystem
{
    [Serializable]
    public class Wave : IGimmickObject
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
        public UnityEvent ClearEvent;
        [field: SerializeField] public GimmickSO GimmickSO { get; set; }

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
                if (pair.ignore == false && pair.enemy.TryGetComponent(out EntityHealth entityHealth))
                {
                    entityHealth.OnDieEvent += HandleEnemyCounting;
                }
            }
        }

        private void HandleEnemyCounting(Entity entity)
        {
            --_enemyCount;
            _owner.EnemyList.Remove(entity as Enemy);
            if (_enemyCount > 0) return;

            ClearEvent?.Invoke();
            GimmickSO?.OnGimmick(this);
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
            Transform effectTrm = PopCore.Pop(EffectPoolType.EnemySpawnEffect, _owner.Tilemap.transform).gameObject.transform;
            effectTrm.position = enemy.transform.position;
        }
    }

    public class Stage : MonoBehaviour
    {
        [field: SerializeField] public Camera ScreenCamera { get; private set; }
        [field: SerializeField] public Vector2 Scale { get; private set; } = new Vector2(15, 15);
        [field: SerializeField] public Vector2 Center { get; private set; } = Vector2.zero;
        [field: SerializeField] public Tilemap Tilemap { get; private set; }
        public List<Vector2Int> AirTileList { get; private set; }
        public List<Vector2Int> GroundTileList { get; private set; }
        [Space]
        [SerializeField] private Transform _playerSpawnPoint;
        [SerializeField] private Portal[] _portals;
        [field: SerializeField] public Transform[] EventTrms { get; private set; }
        public Wave[] waves;
        [SerializeField] private bool _useCanvas;
        [ToggleField(nameof(_useCanvas)), SerializeField] private Transform _stagePanel;
        public UnityEvent StartEvent;

        private float _cardSelectEventPercent = 20f;

        public int CurrentEnemiesCount => EnemyList.Count;
        public int CurrentWaveCount { get; private set; } = 0;

        public event Action<int, int> OnWaveChangedEvent;
        public event Action OnAllClearEvent;

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
            return EnemyList?.Where(x => x != null && x.gameObject.activeSelf && x.IsEnable)?.ToArray();
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
            foreach (var eventTrm in EventTrms)
                eventTrm.gameObject.SetActive(false);

            if (_useCanvas)
            {
                CanvasUI.UIManager.Instance.AddGameCanvas(_stagePanel);
            }

            StartEvent?.Invoke();

            AirTileList = new List<Vector2Int>();
            GroundTileList = new List<Vector2Int>();
            for (int i = -(int)Scale.y; i < Scale.y; i++)
            {
                for (int j = -(int)Scale.x; j < Scale.x; j++)
                {
                    Vector2Int pos = new Vector2Int(j, i);
                    TileBase tileBase = Tilemap.GetTile((Vector3Int)pos);
                    if (tileBase == null)
                        AirTileList.Add(pos);
                    else
                        GroundTileList.Add(pos);
                }
            }

            UpdateScreenCamera();
        }

        private void Update()
        {
            //UpdateScreenCamera();
        }

        private void UpdateScreenCamera()
        {
            if (ScreenCamera == null) return;
            ScreenCamera.orthographicSize = Mathf.Max(Scale.x, Scale.y);
            ScreenCamera.transform.position = new Vector3(Center.x, Center.y, -10f);            
        }
        
        private void OnDestroy()
        {
            PlayerManager.Instance.Player.transform.parent = null;
                
            var components = PlayerManager.Instance.Player.GetComponents<Behaviour>();

            foreach (var com in components)
                com.enabled = true;
            
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
                OnAllClearEvent?.Invoke();
            }
            else
            {
                OnWaveChangedEvent?.Invoke(CurrentWaveCount, waves.Length);
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
            StartCoroutine(DelayClearStageCoroutine(1f));
        }

        private IEnumerator DelayClearStageCoroutine(float delay)
        {
            yield return new WaitForSeconds(delay);

            float cardSelectRandomValue = Random.Range(0f, 100f);
            if (cardSelectRandomValue < _cardSelectEventPercent)
                Hashira.CanvasUI.UIManager.Instance.GetDomain<ToggleDomain>().OpenUI(nameof(StageCardSelectUI));
            else
                OpenPortalAndEvent();
        }

        public void OpenPortalAndEvent()
        {
            if (_portals.Length == 0) return;
            List<StageTypeSO> stageTypeSOList = StageGenerator.Instance.GetNextStageData().GetRandomStageType(_portals.Length);
            for (int i = 0; i < stageTypeSOList.Count; i++)
            {
                _portals[i].gameObject.SetActive(true);
                _portals[i].Init(stageTypeSOList[i]);
            }
            for (int i = 0; i < EventTrms.Length; i++)
            {
                EventTrms[i].gameObject.SetActive(true);
            }
        }

        
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            // foreach (var item in AirTileList)
            // {
            //     Gizmos.DrawSphere(new Vector3(item.x, item.y), 0.2f);
            // }

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
            Gizmos.matrix = Matrix4x4.TRS(Center, transform.rotation, Vector3.one);
            Gizmos.DrawWireCube(Center, Scale * 2);
            Gizmos.DrawLine(new Vector3(-Scale.x, Center.y), new Vector3(Scale.x, Center.y));
            Gizmos.DrawLine(new Vector3(Center.x, -Scale.y), new Vector3(Center.x, Scale.y));
            Gizmos.matrix = Matrix4x4.TRS(Vector2.zero, Quaternion.identity, Vector3.one);
            Gizmos.color = Color.white;
        }
#endif
    }
}