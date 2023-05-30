using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceShooter
{
    public class EntitySpawner : MonoBehaviour
    {
        public enum SpawnMode
        {
            Start,
            Loop
        }

        [SerializeField] private Entity[] _entityPrefabs;
        [SerializeField] private CircleArea _area;
        [SerializeField] private SpawnMode _spawnMode;
        [SerializeField] private List<GameObject> _listEnemies;
        public List<GameObject> GetListEnemies => _listEnemies;

        [SerializeField] private int _numSpawns;
        [SerializeField] private float _respawnTime;
        private float _timer;


        private void Start()
        {
            if (_spawnMode == SpawnMode.Start)
            {
                SpawnEntities();
            }
        }

        private void Update()
        {
            if (_timer > 0)
            {
                _timer -= Time.deltaTime;
            }

            if (_spawnMode == SpawnMode.Loop && _timer <= 0)
            {
                SpawnEntities();
                _timer = _respawnTime;
            }
        }

        private void SpawnEntities()
        {
            for (int i = 0; i < _numSpawns; i++)
            {
                int index = Random.Range(0, _entityPrefabs.Length);
                GameObject _obj = Instantiate(_entityPrefabs[index].gameObject);

                _obj.transform.position = _area.GetRandomInsideZone();

                if (_obj.GetComponent<Destructible>()!= null)
                {
                    _listEnemies.Add(_obj);
                }
            }

        }
    }

}
