using Crogen.CrogenPooling;
using Hashira.Combat;
using Hashira.Enemies;
using Hashira.Entities;
using Hashira.Entities.Components;
using Hashira.StageSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hashira.Cards.Effects
{
    public class LightningShockCard : MagicCardEffect
    {
        private int[] _needCostByStack = new int[] { 1, 1, 1, 3 };
        protected override int[] _NeedCostByStack => _needCostByStack;

        protected override float DelayTime => 8;

        private int[] _lightningCountByStack = { 4, 5, 6, 8, 8 };
        private int[] _damageByStack = { 20, 20, 20, 30, 30 };

        public override void Enable()
        {
            base.Enable();
        }

        public override void Use()
        {
            base.Use();
            player.StartCoroutine(CoroutineCreateLightnings());
        }

        private IEnumerator CoroutineCreateLightnings()
        {
            Dictionary<Enemy, int> enemyHitCount = new Dictionary<Enemy, int>();
            for (int i = 0; i < _lightningCountByStack[stack - 1]; i++)
            {
                yield return new WaitForSeconds(0.1f);
                CreateLightning(enemyHitCount);
            }
        }

        private void CreateLightning(Dictionary<Enemy, int> enemyHitCount)
        {
            Enemy[] enemieList = StageGenerator.Instance.GetCurrentStage().GetEnabledEnemies();

            if (enemieList.Length == 0) return;

            Enemy enemy = enemieList[Random.Range(0, enemieList.Length)];

            if (enemyHitCount.ContainsKey(enemy))
                enemyHitCount[enemy]++;
            else
                enemyHitCount[enemy] = 0;

            int damage = _damageByStack[stack - 1];
            if (stack >= 5) damage += enemyHitCount[enemy] * 10;

            Vector3 pos;
            if (enemy.TryGetEntityComponent(out EntityPartsCollider entityPartsCollider))
                pos = entityPartsCollider.GetRandomCollider().transform.position;
            else
                pos = enemy.transform.position;

            RaycastHit2D hit2D = new RaycastHit2D()
            {
                point = pos
            };
            AttackInfo attackInfo = new AttackInfo(damage, Vector2.zero, EAttackType.Electricity);
            enemy.GetEntityComponent<EntityHealth>().ApplyDamage(attackInfo, hit2D);

            CameraManager.Instance.ShakeCamera(4, 15, 0.1f);
            PopCore.Pop(EffectPoolType.LightningVFX, pos, Quaternion.identity);
        }
    }
}
