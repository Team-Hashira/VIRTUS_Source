using Crogen.CrogenPooling;
using Hashira.Enemies;
using Hashira.Entities.Components;
using Hashira.StageSystem;
using System.Collections;
using UnityEngine;

namespace Hashira.Cards.Effects
{
    public class MeteorCard : MagicCardEffect
    {
        protected override float DelayTime => 5;

        [SerializeField] private int[] _meteorCountByStack = { 1, 1, 2 };
        [SerializeField] private int[] _meteorDamageByStack = { 20, 30, 30 };

        private Coroutine _meteorCoroutine;

        public override void Disable()
        {
            base.Disable();
            if (_meteorCoroutine != null && player != null) player.StopCoroutine(_meteorCoroutine);
        }

        public override void OnUse()
        {
            _meteorCoroutine = player.StartCoroutine(MeteorGenerateCoroutine());
        }

        private IEnumerator MeteorGenerateCoroutine()
        {
            for (int i = 0; i < _meteorCountByStack[stack - 1]; i++)
            {
                CreateMeteor();
                yield return new WaitForSeconds(0.2f);
            }
        }

        private void CreateMeteor()
        {
            Enemy[] enemies = StageGenerator.Instance.GetCurrentStage().GetEnabledEnemies();

            if (enemies != null && enemies.Length == 0) return;

            Enemy enemy = enemies[Random.Range(0, enemies.Length)];

            Vector3 pos;
            if (enemy.TryGetEntityComponent(out EntityPartsCollider entityPartsCollider))
                pos = entityPartsCollider.GetRandomCollider().transform.position;
            else
                pos = enemy.transform.position;

            Meteor meteor = PopCore.Pop(CardSubPoolType.Meteor, pos, Quaternion.identity) as Meteor;
            meteor.Init(pos, _meteorDamageByStack[stack - 1]);
        }
    }
}
