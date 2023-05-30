using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceShooter
{
    public class ResultPanelController : SingletonBase<ResultPanelController>
    {
        [SerializeField] private Text _textKills;
        [SerializeField] private Text _textScore;
        [SerializeField] private Text _textTime;
        [SerializeField] private Text _textMultiplier;

        [SerializeField] private Text _result;

        [SerializeField] private Text _buttonNextText;

        private bool _success;

        private void Start()
        {
            gameObject.SetActive(false);
        }

        public void ShowResults(PlayerStatistics statistics, bool success)
        {
            gameObject.SetActive(true);

            _success = success;

            _result.text = success ? "Win" : "Lose";
            _buttonNextText.text = success ? "Next" : "Restart";

            _textKills.text =  "Kills: " + statistics.NumKills.ToString();
            _textScore.text = "Score: " + statistics.Score.ToString();
            _textTime.text = "Time: " + statistics.Time.ToString();
            _textMultiplier.text = "Multiplier " + statistics.Multiplier.ToString();

            Time.timeScale = 0;
        }

        public void ShowAllResults()
        {
            Debug.Log("SHOW ALL RESULTS");
            gameObject.SetActive(true);
            _buttonNextText.text = "Return";
            _result.text = "All statistics";

            _textKills.text = "AllKills: " + PlayerPrefs.GetInt("AllKills").ToString();
            _textScore.text = "Best Score: " + PlayerPrefs.GetInt("MaxScore").ToString();
            _textTime.text = "Time: " + PlayerPrefs.GetInt("BestTime").ToString();

            Time.timeScale = 0;
        }

        public void OnButtonNextAction()
        {
            gameObject.SetActive(false);

            Time.timeScale = 1;

            if (_success)
            {
                LevelSequenceController.Instance.NextLevel();
            }

            else
            {
                LevelSequenceController.Instance.RestartLevel();
            }
        }

        public void OnClosePanelStatistics()
        {
            gameObject.SetActive(false);

            Time.timeScale = 1;
        }
    }
}

