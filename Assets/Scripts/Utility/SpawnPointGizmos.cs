using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointGizmos : MonoBehaviour
{
    [SerializeField] private float radius;
    [SerializeField] private Color color;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = color;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
