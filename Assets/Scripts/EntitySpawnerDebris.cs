using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceShooter
{
    public class EntitySpawnerDebris : MonoBehaviour
    {
        [SerializeField] private Destructible[] _debrisPrefab;

        [SerializeField] private CircleArea _area;

        [SerializeField] private int _numDebris;

        [SerializeField] private float _randomSpeed;

        private void Start()
        {
            for (int i = 0; i < _numDebris; i++)
            {
                SpawnDebris();
            }
        }

        private void SpawnDebris()
        {
            int index = Random.Range(0, _debrisPrefab.Length);

            GameObject debris = Instantiate(_debrisPrefab[index].gameObject);

            debris.transform.position = _area.GetRandomInsideZone();
            debris.GetComponent<Destructible>().EventOnDeath.AddListener(OnDebrisDead);
        }

        private void OnDebrisDead()
        {
            SpawnDebris();
        }
    }
}

