using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeNode : Node
{
    private Transform _origin;
    private Transform _target;
    private float _range;

    public RangeNode(Transform origin, Transform target, float range)
    {
        this._origin = origin;
        this._target = target;
        this._range = range;
    }

    public override NodeState Execute()
    {
        if (_target == null || _origin == null)
            return NodeState.FAILURE;

        float distance = Vector3.Distance(_origin.position, _target.position);
        myNodeState = distance <= _range ? NodeState.SUCCESS : NodeState.FAILURE;

        return myNodeState;
    }
}
