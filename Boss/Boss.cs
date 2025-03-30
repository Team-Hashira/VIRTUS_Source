using Hashira.Bosses.BillboardClasses;
using Hashira.Bosses.Patterns;
using Hashira.Core;
using Hashira.Enemies;
using Hashira.Entities;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Hashira.Bosses
{
    [Serializable]
    public class BossPatternPair
    {
        [Delayed] public string patternName;
        public bool enable = true;
        [SerializeReference] public BossPattern pattern;
    }

    [Serializable]
    public class BillboardPair
    {
        [Delayed] public string valueName;
        [Delayed] public string typeName;
        [SerializeReference] public BillboardValue billboardValue;

        public BillboardPair()
        {
            valueName = string.Empty;
            typeName = string.Empty;
            billboardValue = null;
        }
    }

    public class Boss : Enemy
    {
        [field:SerializeField] public LayerMask whatIsGround { get; private set; }
        [SerializeField] private Tilemap _currentMap;
        [field:SerializeField] public string BossName { get; private set; }
        [field:SerializeField] public string BossDisplayName { get; private set; }
        [Range(1, 10)] public int maxPhase = 1;
        [field:SerializeField] public float PatternPickDelay { get; private set; } = 1.85f;

        public BillboardPair[] billboard;
        public BossPatternPair[] bossPatterns;
        public BossPattern CurrentBossPattern { get; private set; }

        public float CurrentMaxGroggyTime { get; set; }

        #region Initialize Boss
        protected override void AfterIntiialize()
        {
            base.AfterIntiialize();
        }
        protected override void Awake()
        {
            base.Awake();
            for (int i = 0; i < bossPatterns.Length; i++)
                bossPatterns[i].pattern.Init(this);
        }
        protected override void InitializeComponent()
        {
            base.InitializeComponent();

            _entityHealth = GetEntityComponent<EntityHealth>();
            _entityStat = GetEntityComponent<EntityStat>();
        }
        #endregion

        protected override void HandleDieEvent(Entity _)
        {
            Debug.Log("보스 죽음");
            PlayerDataManager.Instance.AddKillCount(true);
            _entityStateMachine.ChangeState("Dead");
        }

        #region Pattern Utils
        public BossPattern GetRandomBossPattern()
        {
            var index = UnityEngine.Random.Range(0, bossPatterns.Length);

            // TODO : 일단은 디버그하려고 요렇게 했음(나중에 최적화 땜시 수정할 듯)
            if (bossPatterns[index].enable == false)
                return GetRandomBossPattern();

            return bossPatterns[index].pattern;
        }
        public T GetBossPattern<T>() where T : BossPattern
        {
            return bossPatterns.FirstOrDefault(x => x.pattern.GetType() == typeof(T))?.pattern as T;
        }
        public void SetCurrentBossPattern(BossPattern bossPattern)
        {
            CurrentBossPattern = bossPattern;
        }
        public T BillboardValue<T>(string valueName) where T : BillboardValue
        {
            for (int i = 0; i < billboard.Length; i++)
            {
                if (billboard[i].valueName == valueName)
                    return billboard[i].billboardValue as T;
            }
            return null;
        }

        public float GetGroundFloorPosY()
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up, 1000, whatIsGround);
            return hit.point.y;
        }
        public Vector2[] GetGroundFloorPositions()
        {
            float y = GetGroundFloorPosY();

            //TileBase[] tiles = _currentMap.Get

            //return;

            return null;
        }
        #endregion


#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (bossPatterns == null) return;
            for (int i = 0; i < bossPatterns.Length; i++)
                bossPatterns[i].pattern?.OnDrawGizmos(transform);
        }
        private void OnValidate()
        {
            for (int i = 0; i < billboard.Length; i++)
            {
                Type type = Type.GetType($"Hashira.Bosses.BillboardClasses.{billboard[i].typeName}Value");
                for (int j = 0; j < i; j++)
                {
                    if (billboard[i].billboardValue == billboard[j].billboardValue)
                    {
                        billboard[i].billboardValue = Activator.CreateInstance(type) as BillboardValue;
                    }
                }
                if (billboard[i].billboardValue == null || billboard[i].billboardValue.GetType() != type)
                {

                    try
                    {
                        billboard[i].billboardValue = Activator.CreateInstance(type) as BillboardValue;
                    }
                    catch (Exception)
                    {
                    }
                }
            }

            for (int i = 0; i < bossPatterns.Length; i++)
            {
                Type type = Type.GetType($"Hashira.Bosses.Patterns.{bossPatterns[i].patternName}");
                for (int j = 0; j < i; j++)
                {
                    if (bossPatterns[i].pattern == bossPatterns[j].pattern)
                    {
                        bossPatterns[i].pattern = Activator.CreateInstance(type) as BossPattern;
                    }
                }
                if (bossPatterns[i].pattern == null || bossPatterns[i].pattern.GetType() != type)
                {
                    try
                    {
                        bossPatterns[i].pattern = Activator.CreateInstance(type) as BossPattern;
                    }
                    catch (Exception)
                    {
                    }
                }
            }
        }
#endif
    }
}