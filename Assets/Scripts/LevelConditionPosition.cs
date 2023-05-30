using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceShooter
{
    public class LevelConditionPosition : MonoBehaviour, ILevelCondition
    {
        [SerializeField] private Transform _transformFinal;
        [SerializeField] private float _radiusFinal = 2f;

        private bool _isReached;
        bool ILevelCondition.IsCompleted
        {
            get
            {
                if (Player.Instance != null && Player.Instance.ActiveShip != null)
                {
                    if ((_transformFinal.position - Player.Instance.ActiveShip.transform.position).magnitude <= _radiusFinal)
                    {
                        float dist = (_transformFinal.position - Player.Instance.ActiveShip.transform.position).magnitude;
                        Debug.Log(dist);
                        _isReached = true;
                    }
                    
                }
                return _isReached;
            }
        }
    }
}

