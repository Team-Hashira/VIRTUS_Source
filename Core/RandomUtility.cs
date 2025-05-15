using Hashira.StageSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hashira.Core
{
    public static class RandomUtility
    {
        public static bool RollChance(float percent)
        {
            float random = Random.Range(0f, 100f);
            return random < percent;
        }

        public static bool RollChance01(float percent)
        {
            float random = Random.Range(0f, 1f);
            return random < percent;
        }

        public static T GetRandomElement<T>(this List<T> list)
        {
            int index = Random.Range(0, list.Count);
            return list[index];
        }

        public static T GetRandomElement<T>(this T[] arr)
        {
            int index = Random.Range(0, arr.Length);
            return arr[index];
        }
    }
}
