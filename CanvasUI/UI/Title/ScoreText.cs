using System;
using TMPro;
using UnityEngine;

namespace Hashira
{
    public class ScoreText : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _scoreText;

        private void Awake()
        {
            Score.OnScoreChangedEvent += UpdateScore;
            UpdateScore(Score.CurrentScore);
        }

        public void UpdateScore(int score)
        {
            _scoreText.text = $"Score : {score}";
        }

        private void OnDestroy()
        {
            Score.OnScoreChangedEvent -= UpdateScore;
        }
    }
}
