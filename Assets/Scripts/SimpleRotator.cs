using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleRotator : MonoBehaviour
{
    [SerializeField] private Transform objectToRotate;
    [SerializeField] private float rotateStep;

    void FixedUpdate()
    {
        objectToRotate.localEulerAngles = objectToRotate.localEulerAngles + new Vector3(0,0,rotateStep);
    }
}
