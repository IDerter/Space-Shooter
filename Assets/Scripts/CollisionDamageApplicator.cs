using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceShooter
{
    public class CollisionDamageApplicator : MonoBehaviour
    {
        public static string _ignoreTag = "WorldBoundary";

        [SerializeField] private float _velocityDamageModifier;

        [SerializeField] private float _damageConstant;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.transform.tag == _ignoreTag) return;

            var destructible = transform.root.GetComponent<Destructible>();
            if (destructible != null)
            {
                destructible.ApplyDamage((int)_damageConstant + 
                                        (int) (_velocityDamageModifier * collision.relativeVelocity.magnitude));
            }
        }
    }
}

