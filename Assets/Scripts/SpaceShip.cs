using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceShooter
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class SpaceShip : Destructible
    {
        [Header("Space ship")]
        [SerializeField] private float _mass;

        /// <summary>
        /// Толкающая вперед сила
        /// </summary>
        [SerializeField] private float _thrust;
        [SerializeField] private float _startThrust;
        [SerializeField] private float _maxThrust;
        /// <summary>
        /// Вращающая сила
        /// </summary>
        [SerializeField] private float _mobility;
        /// <summary>
        /// Максимальная линейная скорость
        /// </summary>
        [SerializeField] private float _maxLinearVelocity;

        public float CurrentVelocity => _maxLinearVelocity;
        /// <summary>
        /// Максимальная вращ скорость. В град/сек
        /// </summary>
        [SerializeField] private float _maxAngularVelocity;
        public float MaxAngularVelocity => _maxAngularVelocity;

        [SerializeField] private Sprite _previewImage;
        public Sprite PreviewImage => _previewImage;
        /// <summary>
        /// Сохраненная ссылка на ригид
        /// </summary>
        private Rigidbody2D _rigidbody2D;

        #region Public API
        /// <summary>
        /// Управление линейной тягой -1.0 до 1.0
        /// </summary>
        public float ThrustControl { get; set; }

        /// <summary>
        /// Управление вращательной тягой -1.0 до 1.0
        /// </summary>
        public float TorqueControl { get; set; }

        [SerializeField] private Turret[] _turrets;

        #endregion

        #region Unity Event
        protected override void Start()
        {
            base.Start();
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _rigidbody2D.mass = _mass;

            _rigidbody2D.inertia = 1;

            InitValues();
        }

        private void FixedUpdate()
        {
            UpdateRigidbody();
            UpdateEnergyRegen();

            if (HitPoints <= 0)
            {
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// Метод добавления сил к кораблю для движения
        /// </summary>
        private void UpdateRigidbody()
        {
            _rigidbody2D.AddForce(_thrust * ThrustControl * transform.up * Time.fixedDeltaTime, ForceMode2D.Force); // толкаем корабль 

            _rigidbody2D.AddForce(-_rigidbody2D.velocity * (_thrust / _maxLinearVelocity) * Time.fixedDeltaTime, ForceMode2D.Force); // торможение корабля

            _rigidbody2D.AddTorque(TorqueControl * _mobility * Time.fixedDeltaTime, ForceMode2D.Force); //вращаем корабль

            _rigidbody2D.AddTorque(-_rigidbody2D.angularVelocity * (_mobility / _maxAngularVelocity) * Time.fixedDeltaTime, ForceMode2D.Force); //замедление вращения
        }

        #endregion

        public void Fire(TurretMode _turretMode)
        {
            foreach (var turret in _turrets)
            {
                if (turret._TurretMode == _turretMode)
                {
                    turret.Fire();
                }
            }
        }


        [SerializeField] private int _maxEnergy;
        [SerializeField] private int _maxAmmo;
        [SerializeField] private int _energyRegenPerSecond;

        private float _currentEnergy;
        private int _currentAmmo;

        public void AddEnergy(int _energy)
        {
            _currentEnergy = Mathf.Clamp(_currentEnergy + _energy, 0, _maxEnergy);
        }

        public void AddAmmo(int _ammo)
        {
            _currentAmmo = Mathf.Clamp(_currentAmmo + _ammo, 0, _maxAmmo);
        }

        public void AddAcceleration(float thrust)
        {
            _thrust = Mathf.Clamp(_thrust + thrust, 0, _maxThrust);
        }

        public void InitAcceleration()
        {
            _thrust = _startThrust;
        }

        public bool DrawAmmo(int _count)
        {
            if (_count == 0) return true;

            if (_currentAmmo >= _count)
            {
                _currentAmmo -= _count;
                return true;
            }

            return false;
        }

        public bool DrawEnergy(int _count)
        {
            if (_count == 0) return true;

            if (_currentEnergy >= _count)
            {
                _currentEnergy -= _count;
                return true;
            }

            return false;
        }

        private void InitValues()
        {
            _currentEnergy = _maxEnergy;
            _currentAmmo = _maxAmmo;
        }

        private void UpdateEnergyRegen()
        {
            _currentEnergy += (float)_energyRegenPerSecond * Time.deltaTime;
            _currentEnergy = Mathf.Clamp(_currentEnergy, 0, _maxEnergy);
        }

        public void AssignWeapon(TurretPropetries props)
        {
            foreach (Turret weapon in _turrets)
            {
                weapon.AssignLoadout(props);
            }
        }
    }

}
