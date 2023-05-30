using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SpaceShooter
{
    /// <summary>
    /// ������������ �������. �������� � ������ � LevelBoundary, ���� ������� ������� �� �����.
    /// �������� �� ������, ������� ���������� ����������
    /// </summary>
    public class LevelBoundaryLimiter : MonoBehaviour
    {
        private void Update()
        {
            if (LevelBoundary.Instance == null) return;

            var _levelBoundary = LevelBoundary.Instance;
            var _radius = _levelBoundary.RadiusLevel;

            if(transform.position.magnitude > _radius)
            {
                if(_levelBoundary.LimitMode == LevelBoundary.Mode.Limit)
                {
                    transform.position = transform.position.normalized * _radius;
                }

                if (_levelBoundary.LimitMode == LevelBoundary.Mode.Teleport)
                {
                    transform.position = -transform.position.normalized * _radius;
                }

                if (_levelBoundary.LimitMode == LevelBoundary.Mode.Wall)
                {
                    Destroy(gameObject);
                    Debug.Log("Wall");
                }
            }
        }
    }
}

