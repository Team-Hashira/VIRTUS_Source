using Crogen.CrogenPooling;
using Hashira.Enemies;
using Hashira.Entities.Components;
using Hashira.StageSystem;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

namespace Hashira.Cards.Effects
{
    public class MeteorCard : MagicCardEffect
    {
        private int[] _needCostByStack = new int[] { 1 };
        protected override int[] _NeedCostByStack => _needCostByStack;

        protected override float DelayTime => 5;

        private int[] _meteorCountByStack = { 1, 1, 2 };
        private int[] _meteorDamageByStack = { 20, 30, 30 };

        public override void Enable()
        {
            base.Enable();
        }

        public override void Use()
        {
            base.Use();
            player.StartCoroutine(MeteorGenerateCoroutine());
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
            Enemy[] enemieList = StageGenerator.Instance.GetCurrentStage().GetEnabledEnemies();

            if (enemieList.Length == 0) return;

            Enemy enemy = enemieList[Random.Range(0, enemieList.Length)];

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
