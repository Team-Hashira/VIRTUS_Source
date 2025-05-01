using Crogen.CrogenPooling;
using Hashira.Enemies;
using Hashira.Entities;
using Hashira.Entities.Components;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hashira.Bosses.Patterns
{
    [System.Serializable]
    public class GiantGolemSpawnEnemyPattern : GiantGolemPattern
    {
        [SerializeField] private Enemy _enemyPrefab;
        [SerializeField] private Transform[] _spawnPoints;
        private List<Entity> _currentEnemyList = new List<Entity>();

        public override bool CanStart()
        {
            return _currentEnemyList.Count <= 0;
        }

        public override void OnStart()
        {
            base.OnStart();
            EntityAnimator.OnAnimationTriggeredEvent += OnAnimationTriggeredHandle;
        }

        private void OnAnimationTriggeredHandle(EAnimationTriggerType triggertype, int count)
        {
            if (triggertype == EAnimationTriggerType.Trigger)
                Boss.StartCoroutine(SpawnEnemies(0.1f));
            
            if (triggertype == EAnimationTriggerType.End)
                EndPattern();
        }

        public override void OnEnd()
        {
            EntityAnimator.OnAnimationTriggeredEvent -= OnAnimationTriggeredHandle;
            base.OnEnd();
        }
 
        public override void OnDie()
        {
            DestroyAllEnemy();
            base.OnDie();
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
            var enemy = GameObject.Instantiate(_enemyPrefab, pos, Quaternion.Euler(0, 0, pos.x > 0 ? 90 : -90));
            _currentEnemyList.Add(enemy);
            enemy.EntityHealth.OnDieEvent += dieEnemy => _currentEnemyList.Remove(dieEnemy);
            PopCore.Pop(EffectPoolType.EnemySpawnEffect, enemy.transform.position, Quaternion.identity);
        }

        private void DestroyAllEnemy()
        {
            for (int i = 0; i < _currentEnemyList.Count; i++)
                GameObject.Destroy(_currentEnemyList[i].gameObject);
            
            _currentEnemyList.Clear();
        }
    }
}
