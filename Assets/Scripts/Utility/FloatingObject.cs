using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script for a floating object that floats up and down
public class FloatingObject : MonoBehaviour
{
    [SerializeField] private float maxYOffset = 1f;
    [SerializeField] private float floatStep = .1f;

    private Vector3 _initialPosition;

    private void Start()
    {
        _initialPosition = transform.position;
    }

    void FixedUpdate()
    {
        transform.position += new Vector3(0, floatStep, 0);
        if (transform.position.y >= _initialPosition.y + maxYOffset)
        {
            transform.position = _initialPosition + new Vector3(0, maxYOffset, 0);
            floatStep *= -1;
        }
        if (transform.position.y <= _initialPosition.y - maxYOffset)
        {
            transform.position = _initialPosition - new Vector3(0, maxYOffset, 0);
            floatStep *= -1;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position + new Vector3(0, maxYOffset, 0), .1f);
        Gizmos.DrawSphere(transform.position - new Vector3(0, maxYOffset, 0), .1f);
    }
}
