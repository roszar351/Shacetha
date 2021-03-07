using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Used to keep an object's world position constant, needed if an object is a child of another object and 
// by moving the parent this object would as well but need constant world position.
public class KeepConstantPosition : MonoBehaviour
{
    private Vector3 _targetPosition;

    // Update is called once per frame
    void Update()
    {
        if (_targetPosition != null)
            transform.position = _targetPosition;
    }

    public void UpdatePosition(Vector3 newPosition)
    {
        _targetPosition = newPosition;
    }
}
