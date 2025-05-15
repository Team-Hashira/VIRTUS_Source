using Crogen.CrogenPooling;
using Hashira.Core;
using Hashira.MainScreen;
using Hashira.StageSystem;
using UnityEngine;

namespace Hashira.Cards.Effects
{
    public class ElectricBallCard : MagicCardEffect
    {
        protected override float DelayTime => 18f;

        [SerializeField] private float[] _duration = { 6f, 8f, 10f, 10f };
        [SerializeField] private int[] _damageByStack = { 10, 10, 10, 15 };

        public override void OnUse()
        {
            Vector2Int spawnPosIntFirst = StageGenerator.Instance.GetCurrentStage().AirTileList.GetRandomElement();
            Vector2 spawnPosFirst = new Vector2(spawnPosIntFirst.x + 0.5f, spawnPosIntFirst.y + 0.5f);

            ElectricBall electricBallFirst = PopCore.Pop
                (CardSubPoolType.ElectricBall, MainScreenEffect.GetLevelTransform()) as ElectricBall;
            electricBallFirst.transform.position = spawnPosFirst;
            electricBallFirst.Init(_damageByStack[stack - 1], _duration[stack - 1]);


            if (IsMaxStack)
            {
                Vector2Int spawnPosIntSecond = StageGenerator.Instance.GetCurrentStage().AirTileList.GetRandomElement();
                Vector2 spawnPosSecond = new Vector2(spawnPosIntSecond.x + 0.5f, spawnPosIntSecond.y + 0.5f);

                ElectricBall electricBallSecond = PopCore.Pop
                    (CardSubPoolType.ElectricBall, MainScreenEffect.GetLevelTransform()) as ElectricBall;
                electricBallSecond.transform.position = spawnPosSecond;
                electricBallSecond.Init(_damageByStack[stack - 1], _duration[stack - 1]);
            }
        }
    }
}
