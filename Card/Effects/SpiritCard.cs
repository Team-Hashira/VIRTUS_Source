using Crogen.CrogenPooling;
using Hashira.Core.StatSystem;
using Hashira.Entities;

namespace Hashira.Cards.Effects
{
    public class SpiritCard : CardEffect
    {
        private SpiritFamiliar _spiritFamiliar;
        private StatElement _attackPowerStat;

        public override void Enable()
        {
            base.Enable();
            _attackPowerStat = player.GetEntityComponent<EntityStat>().StatDictionary[StatName.AttackPower];

            if (stack < 5)
            {
                for (int i = 0; i < stack; i++)
                {
                    _spiritFamiliar = PopCore.Pop(FamiliarPoolType.Spirit) as SpiritFamiliar;
                    _spiritFamiliar.Init(() => _attackPowerStat.IntValue, i, 3f, false);
                }
            }
            else
            {
                _spiritFamiliar = PopCore.Pop(FamiliarPoolType.Spirit) as SpiritFamiliar;
                _spiritFamiliar.Init(() => _attackPowerStat.IntValue * 2, 0, 1f, true);
            }
        }

        public override void Disable()
        {
            base.Disable();
            _spiritFamiliar?.Push();
        }
    }
}
