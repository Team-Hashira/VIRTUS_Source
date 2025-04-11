using Crogen.CrogenPooling;
using Hashira.Enemies;
using Hashira.Enemies.Components;
using Hashira.Entities;
using Hashira.Entities.Components;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hashira.Bosses.Patterns
{
    public class GiantGolemSpawnEnemyPattern : GiantGolemPattern
    {
        [SerializeField] private Enemy _enemyPrefab;
        [SerializeField] private Transform[] _spawnPoints;
        [SerializeField] private int _enemyCount = 0;

        public override void OnStart()
        {
            base.OnStart();
            EntityAnimator.OnAnimationTriggeredEvent += OnAnimationTriggeredHandle;
        }

        private void OnAnimationTriggeredHandle(EAnimationTriggerType triggertype, int count)
        {
            if (triggertype == EAnimationTriggerType.Trigger)
                Boss.StartCoroutine(SpawnEnemies(0.1f));
        }

        public override void OnEnd()
        {
            EntityAnimator.OnAnimationTriggeredEvent -= OnAnimationTriggeredHandle;
            base.OnEnd();
        }

        private IEnumerator SpawnEnemies(float delay = 0)
        {
            WaitForSeconds waitForSeconds = new WaitForSeconds(delay);
            foreach (var spawnPoint in _spawnPoints)
            {
                PopCore.Pop(EffectPoolType.EnemySpawnMark, spawnPoint.transform.position, Quaternion.identity);
                Boss.StartCoroutine(DelaySpawn(spawnPoint.position, 1f));
                yield return waitForSeconds;
            }
        }
        
        private IEnumerator DelaySpawn(Vector2 pos, float delay)
        {
            yield return new WaitForSeconds(delay);
            var enemy = GameObject.Instantiate(_enemyPrefab, pos, Quaternion.identity);
            PopCore.Pop(EffectPoolType.EnemySpawnEffect, enemy.transform.position, Quaternion.identity);
            ++_enemyCount;
            enemy.GetEntityComponent<EnemyHealth>().OnDieEvent += OnCountingEnemyCountHandle;
        }

        private void OnCountingEnemyCountHandle(Entity entity)
        {
            --_enemyCount;
            if (_enemyCount <= 0)
                EndPattern();
        }
    }
}
