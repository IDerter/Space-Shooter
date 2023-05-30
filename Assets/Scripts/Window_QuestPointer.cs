using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceShooter
{
    public class Window_QuestPointer : MonoBehaviour
    {
        [SerializeField] private Transform _target;

        private void Update()
        {
            var direction = _target.position - transform.position;
            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 20f;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }
}
