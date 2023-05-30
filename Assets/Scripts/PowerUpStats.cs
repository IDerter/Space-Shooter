using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SpaceShooter
{
    public class PowerUpStats : PowerUp
    {
        public enum EffectType
        {
            AddAmmo,
            AddEnergy,
            Invulnerability,
            Acceleration,
            IncreaseCircle
        }

        [SerializeField] private EffectType _effectType;
        [SerializeField] private float _value;
        [SerializeField] private AudioSource _audioPickUp;

        public static Action OnDestroyPowerUpInvulnerability;
        public static Action OnDestroyPowerUpAcceleration;
        public static Action OnDestroyPowerUpIncreaseCircle;

        protected override void OnPickedUp(SpaceShip ship)
        {
            if (ship == Player.Instance.ActiveShip)
            {
                var obj = Instantiate(_audioPickUp, ship.transform);
                obj.transform.parent = null;

                if (_effectType == EffectType.AddAmmo)
                {
                    ship.AddAmmo((int)_value);
                }

                if (_effectType == EffectType.AddEnergy)
                {
                    ship.AddEnergy((int)_value);
                    Debug.Log("addEnergy");
                }

                if (_effectType == EffectType.Invulnerability)
                {
                    ship.IsInDestructible = true;
                    OnDestroyPowerUpInvulnerability.Invoke();
                }

                if (_effectType == EffectType.Acceleration)
                {
                    ship.AddAcceleration(_value);
                    OnDestroyPowerUpAcceleration.Invoke();
                }

                if (_effectType == EffectType.IncreaseCircle)
                {
                    Debug.Log("increaseCircle");
                    OnDestroyPowerUpIncreaseCircle.Invoke();
                }
            }
        }

      
    }
}

