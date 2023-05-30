using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceShooter
{
    public class MovementController : MonoBehaviour
    {
        public enum ControlMode
        {
            Keyboard,
            Joystick
        }

        [SerializeField] private SpaceShip _targetShip;
        public void SetTargetShip(SpaceShip ship) => _targetShip = ship;

        [SerializeField] private VirtualJoystick _mobileJoystick;

        [SerializeField] private ControlMode _controlMode;

        [SerializeField] private PointerClickHold _mobileFirePrimary;
        [SerializeField] private PointerClickHold _mobileFireSecondary;

        private void Start()
        {
            if (Application.isMobilePlatform || _controlMode == ControlMode.Joystick)
            {
                _controlMode = ControlMode.Joystick;

                _mobileJoystick.gameObject.SetActive(true);
                _mobileFirePrimary.gameObject.SetActive(true);
                _mobileFireSecondary.gameObject.SetActive(true);
            }
            else
            {
                _controlMode = ControlMode.Keyboard;

                _mobileJoystick.gameObject.SetActive(false);
                _mobileFirePrimary.gameObject.SetActive(false);
                _mobileFireSecondary.gameObject.SetActive(false);
            }
        }

        private void Update()
        {
            if (_targetShip == null) return;

            if (_controlMode == ControlMode.Keyboard)
                ControlKeyboard();

            if (_controlMode == ControlMode.Joystick)
                ControlMobileJoystick();
        }

        private void ControlMobileJoystick()
        {
            var dir = _mobileJoystick.Value;
            _targetShip.ThrustControl = dir.y;
            _targetShip.TorqueControl = -dir.x;

            if (_mobileFirePrimary.IsHold)
            {
                _targetShip.Fire(TurretMode.Primary);
            }

            if (_mobileFireSecondary.IsHold)
            {
                _targetShip.Fire(TurretMode.Secondary);
            }
        }

        private void ControlKeyboard()
        {
            float thrust = 0;
            float torque = 0;

            if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
                thrust = 1.0f;
            if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
                thrust = -1.0f;
            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
                torque = 1.0f;
            if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
                torque = -1.0f;

            if (Input.GetKey(KeyCode.Space))
            {
                Debug.Log("Space");
                _targetShip.Fire(TurretMode.Primary);
            }

            if (Input.GetKey(KeyCode.X))
            {
                _targetShip.Fire(TurretMode.Secondary);
            }

            _targetShip.ThrustControl = thrust;
            _targetShip.TorqueControl = torque;
        }
    }
}
