using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPointPatrol : MonoBehaviour
{
    [SerializeField] private float _radius;
    public float Radius => _radius;

    private static readonly Color GizmosColor = new Color(1, 0, 0, 0.3f);

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = GizmosColor;
        Gizmos.DrawSphere(transform.position, _radius);
    }
}
