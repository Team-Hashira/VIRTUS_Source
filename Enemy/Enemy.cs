using Crogen.CrogenPooling;
using Hashira.Core.EventSystem;
using Hashira.Core.StatSystem;
using Hashira.Entities;
using Hashira.Entities.Components;
using Hashira.StageSystem;
using UnityEngine;

namespace Hashira.Enemies
{
    public abstract class Enemy : Entity
    {
        public EntityRenderer EntityRenderer { get; private set; }
        public EntityHealth EntityHealth { get; private set; }
        protected EntityStat _entityStat;
        protected EntityStateMachine _entityStateMachine;

        public bool IsEnable { get; private set; } = true;

        //Test
        [SerializeField] protected EffectPoolType _dieEffect;
        [SerializeField] protected int _killCost = 1;


        [field: SerializeField]
        public LayerMask WhatIsPlayer { get; private set; }

        protected override void Awake()
        {
            base.Awake();

            EntityHealth.OnDieEvent += HandleDieEvent;

            // 사용 안함 (StatName.FieldOfView)
            //_fovElement = _entityStat.StatDictionary[StatName.FieldOfView];
        }

        protected virtual void Start()
        {
            if (EntityHealth != null)
            {
                EntityHealth.OnHitEvent += HandleHitEvent;
            }
        }

        protected void OnDisable()
        {
            if (EntityHealth != null)
            {
                EntityHealth.OnHitEvent -= HandleHitEvent;
            }
        }

        public void SetEnable(bool enable)
        {
            IsEnable = enable;
        }

        protected virtual void HandleDieEvent(Entity _)
        {
            var killEnemyEvent = InGameEvents.KillEnemyEvent;
            GameEventChannel.RaiseEvent(killEnemyEvent);
            PopCore.Pop(_dieEffect, transform.position, Quaternion.identity);
            PlayerDataManager.Instance.AddKillCount();
            SoundManager.Instance.PlaySFX("EnemyDie", transform.position, 1f);
            Destroy(gameObject);
            Cost.AddCost(_killCost);
        }

        private void HandleHitEvent(int currentHealth)
        {
            SoundManager.Instance.PlaySFX("BeShotSound", transform.position, 1f);
        }

        protected override void InitializeComponent()
        {
            base.InitializeComponent();

            EntityRenderer = GetEntityComponent<EntityRenderer>();
            EntityHealth = GetEntityComponent<EntityHealth>();
            _entityStat = GetEntityComponent<EntityStat>();
            _entityStateMachine = GetEntityComponent<EntityStateMachine>();
        }

        protected override void AfterIntiialize()
        {
            float stageAmount = (float)(StageGenerator.currentStageIdx + 1) / StageGenerator.CurrentStageCount;
            int floor = StageGenerator.currentFloorIdx;

            float value = (1 - Mathf.Cos(stageAmount * Mathf.PI)) / 2 + floor * 0.75f;

            _entityStat.StatDictionary[StatName.Health].AddModify("StageMultiplier", value * 100f, EModifyMode.Percent, EModifyLayer.Default);

            base.AfterIntiialize();
        }
    }
}