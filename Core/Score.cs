using System;

namespace Hashira
{
    public static class Score
    {
        public static int CurrentScore { get; private set; }
        public static event Action<int> OnScoreChangedEvent;

        public static void AddScore(int value)
        {
            int prevCost = CurrentScore;
            CurrentScore += value;

            if (prevCost != CurrentScore)
                OnScoreChangedEvent?.Invoke(CurrentScore);
        }
        public static void RemoveScore(int value)
        {
            int prevCost = CurrentScore;
            CurrentScore -= value;
            if (CurrentScore < 0)
                CurrentScore = 0;

            if (prevCost != CurrentScore)
                OnScoreChangedEvent?.Invoke(CurrentScore);
        }
        //웬만하면 이거 쓰기
        public static bool TryRemoveScore(int value)
        {
            if (CurrentScore >= value)
            {
                CurrentScore -= value;
                OnScoreChangedEvent?.Invoke(CurrentScore);
                return true;
            }
            else
            {
                return false;
            }
        }

        public static void ResetScore()
        {
            CurrentScore = 0;
        }
    }
}
