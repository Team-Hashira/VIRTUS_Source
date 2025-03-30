using Crogen.CrogenPooling;
using Hashira.Core;
using Hashira.Core.EventSystem;
using Hashira.Core.StatSystem;
using Hashira.Entities;
using Hashira.Entities.Components;
using Hashira.FSM;
using Hashira.Players;
using System;
using UnityEngine;

namespace Hashira.Enemies
{
    public abstract class Enemy : Entity
    {
        public EntityRenderer EntityRenderer { get; private set; }
        protected EntityHealth _entityHealth;
        protected EntityStat _entityStat;
        protected EntityStateMachine _entityStateMachine;


        //Test
        [SerializeField] private EffectPoolType _dieEffect;

        
        [field: SerializeField]
        public LayerMask WhatIsPlayer { get; private set; }

        protected override void Awake()
        {
            base.Awake();

            _entityHealth.OnDieEvent += HandleDieEvent;

            // 사용 안함 (StatName.FieldOfView)
            //_fovElement = _entityStat.StatDictionary[StatName.FieldOfView];
        }

        protected virtual void HandleDieEvent(Entity _)
        {
            var killEnemyEvent = InGameEvents.KillEnemyEvent;
            GameEventChannel.RaiseEvent(killEnemyEvent);
            gameObject.Pop(_dieEffect, transform.position, Quaternion.identity);
            PlayerDataManager.Instance.AddKillCount();
            SoundManager.Instance.PlaySFX("EnemyDie", transform.position, 1f);
            Destroy(gameObject);
        }

        protected override void InitializeComponent()
        {
            base.InitializeComponent();

            EntityRenderer = GetEntityComponent<EntityRenderer>();
            _entityHealth = GetEntityComponent<EntityHealth>();
            _entityStat = GetEntityComponent<EntityStat>();
            _entityStateMachine = GetEntityComponent<EntityStateMachine>();
        }

        
    }
}