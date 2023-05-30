using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceShooter
{
    public class Turret : MonoBehaviour
    {
        [SerializeField] private AudioSource _audioTurret;

        [SerializeField] private TurretMode _turretMode;
        public TurretMode _TurretMode => _turretMode;

        [SerializeField] private TurretPropetries _turretPropetries;

        private float _refireTimer;

        public bool CanFire => _refireTimer <= 0;

        [SerializeField] private SpaceShip _spaceShip;

        private void Start()
        {
            _audioTurret = transform.GetComponent<AudioSource>();
            _spaceShip = transform.root.GetComponent<SpaceShip>();
            Debug.Log(_spaceShip.name);
        }

        private void Update()
        {
            if (_refireTimer > 0)
                _refireTimer -= Time.deltaTime;
        }

        // public API
        public void Fire()
        {
            _spaceShip = transform.root.GetComponent<SpaceShip>();
            if (_turretPropetries == null) return;
            if (_refireTimer > 0) return;

            // если не хватает патронов для выстрела выходим из метода, иначе вычитаем из кол-ва патрон ammousage
            if (_spaceShip.DrawAmmo(_turretPropetries.AmmoUsage) == false) return;
            
            if (_spaceShip.DrawEnergy(_turretPropetries.EnergyUsage) == false) return;

            Projectile projectile = Instantiate(_turretPropetries.ProjectilePrefab).GetComponent<Projectile>();

            projectile.transform.position = transform.position;
            projectile.transform.up = transform.up;

            projectile.SetShooterParent(_spaceShip);

            _refireTimer = _turretPropetries.RateOfFire;

            {
                _audioTurret.Play();
            }
        }

        public void AssignLoadout(TurretPropetries properties)
        {
            if (_turretMode != properties._TurretMode) return;

            _refireTimer = 0;
            _turretPropetries = properties;
        }
    }
}

