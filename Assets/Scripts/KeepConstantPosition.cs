using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
