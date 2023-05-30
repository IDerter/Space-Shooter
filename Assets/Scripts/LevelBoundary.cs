using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceShooter
{
    public class LevelBoundary : SingletonBase<LevelBoundary>
    {
        [SerializeField] private float _radiusLevel;
        public float RadiusLevel => _radiusLevel;

        public enum Mode
        {
            Limit,
            Teleport,
            Wall
        }

        [SerializeField] private Mode _limitMode;
        public Mode LimitMode => _limitMode;

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            UnityEditor.Handles.color = Color.green;
            UnityEditor.Handles.DrawWireDisc(transform.position, transform.forward, _radiusLevel);
        }
    }
#endif
}

