using Hashira.Pathfind;
using System;
using UnityEngine;

namespace Hashira.Core.EventSystem
{

    [Flags]
    public enum ESoundSource
    {
        Player = 1,
        Enemy = 1 << 1,
        Gun = 1 << 2,
        FootStep = 1 << 3,
        Melee = 1 << 4,
        Nature = 1 << 5,
    }
}
