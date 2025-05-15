using Crogen.CrogenPooling;
using System;
using UnityEngine;

namespace Hashira.StageSystem
{
    public static class StagePopCore
    {
        public static IPoolingObject Pop(Enum type, Vector3 pos, Quaternion rot)
        {
            return PopCore.Pop(type, StageGenerator.Instance.GetCurrentStage().transform, pos, rot);
        }
    }
}