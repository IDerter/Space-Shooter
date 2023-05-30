using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceShooter
{
    public class CameraContoller : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private Transform _target;
        [SerializeField] private float _interpolationLinear;
        [SerializeField] private float _interpolationAngular;
        [SerializeField] private float _cameraZOffset;
        [SerializeField] private float _forwardOffset;

        private void FixedUpdate()
        {
            if (_target == null || _camera == null) return;

            Vector2 camPos = _camera.transform.position;
            Vector2 targetPos = _target.position + _target.transform.up * _forwardOffset;

            Vector2 newCamPos = Vector2.Lerp(camPos, targetPos, _interpolationLinear * Time.deltaTime);
            _camera.transform.position = new Vector3(newCamPos.x, newCamPos.y, _cameraZOffset);

            if(_interpolationAngular > 0)
            {
                _camera.transform.rotation = Quaternion.Slerp(_camera.transform.rotation,
                                            _target.rotation, _interpolationAngular * Time.deltaTime);
            }
        }

        public void SetTarget(Transform newTarget)
        {
            _target = newTarget;
        }
    }
}

