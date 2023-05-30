using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace SpaceShooter
{
    public class CircleArea : MonoBehaviour
    {
        [SerializeField] private float _radius;
        public float Radius => _radius;

        public Vector2 GetRandomInsideZone()
        {
            return (Vector2)transform.position + UnityEngine.Random.insideUnitCircle * _radius;
        }

        #if UNITY_EDITOR
        private static Color GizmoColor = new Color(0, 1, 0, 0.3f);

        private void OnDrawGizmosSelected()
        {
            Handles.color = GizmoColor;
            Handles.DrawSolidDisc(transform.position, transform.forward, _radius);
        }
        #endif
    }
}

