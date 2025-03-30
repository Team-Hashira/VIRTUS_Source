using Hashira.Entities;
using Hashira.Items;
using System.Collections.Generic;
using UnityEngine;

namespace Hashira.Core
{
    public static class EnumUtility
    {
        public static readonly Dictionary<EAttackType, Color> AttackTypeColorDict 
            = new Dictionary<EAttackType, Color>()
            {
                { EAttackType.Default, new Color(1, 1, 1, 1)},
                { EAttackType.Fixed, new Color(0.5f, 0.5f, 0.5f, 1)},
                { EAttackType.Fire, new Color(1, 0.2f, 0.2f, 1)},
                { EAttackType.Electricity, new Color(0.9f, 0.35f, 0.9f, 1)},
                { EAttackType.Bleeding, new Color(0.6f, 0, 0, 1)},
            };
    }
}
