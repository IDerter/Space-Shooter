using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceShooter
{
    public class ScoreStats : MonoBehaviour
    {
        [SerializeField] private Text _textScore;

        [SerializeField] private Text _textGoalScore;

        private int _lastScore;

        private void Start()
        {
            _textGoalScore.text = "Collect coins: " + LevelConditionScore.Instance.GoalScore.ToString() + "\nAnd reach the final";
        }

        private void Update()
        {
            UpdateScore();
        }

        private void UpdateScore()
        {
            if (Player.Instance != null)
            {
                int _currentScore = Player.Instance.Score;

                if (_lastScore != _currentScore)
                {
                    _lastScore = _currentScore;

                    _textScore.text = "Score: " + _lastScore.ToString();
                }
            }
        }
    }

}
