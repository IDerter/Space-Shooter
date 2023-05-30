using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceShooter
{
    public class Player : SingletonBase<Player>
    {
        [SerializeField] private int _amountLives;
        [SerializeField] private SpaceShip _ship;
        [SerializeField] private GameObject _playerShipPrefab;
        public SpaceShip ActiveShip => _ship;

        [SerializeField] private CameraContoller _cameraController;
        [SerializeField] private MovementController _movementController;

        [SerializeField] private GameObject _explosionParicle;
        [SerializeField] private GameObject _shipParticle;
        [SerializeField] private GameObject _particlePlayerPrefab;
        [SerializeField] private AudioSource _audioDie;
        [SerializeField] private float _timeUndestructible = 5f;
        [SerializeField] private PowerUpStats _powerInvulnerability;

        protected override void Awake()
        {
            base.Awake();

            if(_ship != null)
            {
                Destroy(_ship.gameObject);
            }
        }

        private void Start()
        {
            Respawn();
           // _ship?.EventOnDeath.AddListener(OnShipDeath);
        }

        private void OnShipDeath()
        {
            _amountLives--;
            var _explosion = Instantiate(_explosionParicle, _ship.transform);
            _explosion.transform.parent = null;

            var audioDie = Instantiate(_audioDie, _ship.transform);
            audioDie.transform.parent = null;

            if (_amountLives > 0)
            {
                Invoke("Respawn", 2f);
            }
            else
            {
                LevelSequenceController.Instance.FinishCurrentLevel(false);
            }
            Destroy(_ship.gameObject);
        }

        private void Respawn()
        {
            if (LevelSequenceController.PlayerShip != null)
            {
                var newPlayerShip = Instantiate(LevelSequenceController.PlayerShip);
                _ship = newPlayerShip.GetComponent<SpaceShip>();

                var newPlayerParticle = Instantiate(_particlePlayerPrefab);
                newPlayerParticle.transform.SetParent(_ship.transform);

                Debug.Log("indestructible");
                var powerUp = Instantiate(_powerInvulnerability, _ship.transform);
                powerUp.transform.parent = null;

                _shipParticle = newPlayerParticle;

                _cameraController.SetTarget(_ship.transform);
                _movementController.SetTargetShip(_ship);
                _ship?.EventOnDeath.AddListener(OnShipDeath);
            }
        }
        
        private void InDestructible()
        {
            StartCoroutine(DisableDestructible());
        }

        private void SetAcceleration()
        {
            StartCoroutine(SetDefaultAcceleration());
        }

        private void SetDecreaseSize()
        {
            _ship.transform.localScale -= new Vector3(0.2f, 0.2f, 0.2f);
            _shipParticle.transform.localScale -= new Vector3(0.02f, 0.02f);
            Debug.Log("DECREASESIZE!!");
        }

        private IEnumerator DisableDestructible()
        {
            _ship.IsInDestructible = true;
            Debug.Log("¬ Àﬁ◊¿≈Ã Õ≈–¿«–”ÿ»ÃŒ—“‹!");
            yield return new WaitForSeconds(_timeUndestructible);
            Debug.Log("¬€ Àﬁ◊¿≈Ã Õ≈–¿«–”ÿ»ÃŒ—“‹!");
            _ship.IsInDestructible = false;
        }

        private IEnumerator SetDefaultAcceleration()
        {
            yield return new WaitForSeconds(5f);
            _ship.InitAcceleration();
        }

        private void OnEnable()
        {
            PowerUpStats.OnDestroyPowerUpInvulnerability += InDestructible;
            PowerUpStats.OnDestroyPowerUpAcceleration += SetAcceleration;
            PowerUpStats.OnDestroyPowerUpIncreaseCircle += SetDecreaseSize;
        }

        private void OnDisable()
        {
            PowerUpStats.OnDestroyPowerUpInvulnerability -= InDestructible;
            PowerUpStats.OnDestroyPowerUpAcceleration -= SetAcceleration;
            PowerUpStats.OnDestroyPowerUpIncreaseCircle -= SetDecreaseSize;
        }

        #region Score
        public int Score { get; private set; }

        public int NumKills { get; private set; }

        public void AddKill()
        {
            NumKills++;
        }

        public void AddScore(int num)
        {
            Score += num;
        }
        #endregion
    }
}

