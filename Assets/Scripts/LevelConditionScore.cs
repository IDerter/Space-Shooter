using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceShooter
{
    public class LevelConditionScore : SingletonBase<LevelConditionScore>, ILevelCondition
    {
        [SerializeField] private int _score;
        public int GoalScore => _score;

        private bool _reached;

        bool ILevelCondition.IsCompleted
        {
            get
            {
                if (Player.Instance != null && Player.Instance.ActiveShip != null)
                {
                    if (Player.Instance.Score >= _score)
                    {
                        _reached = true;
                        Debug.Log("Reached");
                    }
                }
                return _reached;
            }
        }
    }
}

