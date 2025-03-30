using System;
using UnityEditor;
using UnityEngine;

namespace Hashira
{
    public static class Cost
    {
        //혹시몰라서 넣은 값
        private const int MaxCost = 1000;
        private const int StartCost = 10;
        public const int ClearCost = 15;

        public static int CurrentCost { get; private set; } = StartCost;
        public static event Action<int> OnCostChangedEvent;

        public static int GetBonusCost()
        {
            return CurrentCost / 10 * 2 - 1;
        }

        public static void AddCost(int value)
        {
            int prevCost = CurrentCost;
            CurrentCost += value;
            if (CurrentCost > MaxCost)
                CurrentCost = MaxCost;

            if (prevCost != CurrentCost)
                OnCostChangedEvent?.Invoke(CurrentCost);
        }
        public static void RemoveCost(int value)
        {
            int prevCost = CurrentCost;
            CurrentCost -= value;
            if (CurrentCost < 0)
                CurrentCost = 0;

            if (prevCost != CurrentCost)
                OnCostChangedEvent?.Invoke(CurrentCost);
        }
        //웬만하면 이거 쓰기
        public static bool TryRemoveCost(int value)
        {
            if (CurrentCost >= value)
            {
                CurrentCost -= value;
                OnCostChangedEvent?.Invoke(CurrentCost);
                return true;
            }
            else
            {
                return false;
            }
        }

        public static void ResetCost()
        {
            CurrentCost = StartCost;
        }
    }
}
