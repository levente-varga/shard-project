using System;

namespace Shard
{
    public static class ScoreManager
    {
        public static int totalScorePoints = 0;
        public delegate void ScoreChangedDelegate(int newScore);
        public static event ScoreChangedDelegate OnScoreChanged;

        public static void UpdateScore(int points)
        {
            totalScorePoints += points;
            RaiseScoreChanged(totalScorePoints);
        }
        public static void RaiseScoreChanged(int newScore)
        {
            OnScoreChanged?.Invoke(newScore);
        }
        public static void ResetScore()
        {
            totalScorePoints = 0;
            OnScoreChanged?.Invoke(totalScorePoints);
        }
    }
}
