using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceShooter
{
    public class ImpactEffect : MonoBehaviour
    {
        [SerializeField] private float _lifeTime;

        private float _time;

        private void Update()
        {
            if (_time < _lifeTime)
            {
                _time += Time.deltaTime;
            }

            else
            {
                Destroy(gameObject);
            }
        }
    }
}

