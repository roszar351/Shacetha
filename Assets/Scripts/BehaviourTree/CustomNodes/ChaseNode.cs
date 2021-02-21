using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseNode : Node
{
    private Enemy _ai;
    private Transform _origin;
    private Transform _target;
    private float _distanceForSuccess;

    public ChaseNode(Enemy ai, Transform origin, Transform target, float distanceForSuccess = 0.1f)
    {
        this._ai = ai;
        this._origin = origin;
        this._target = target;
        this._distanceForSuccess = distanceForSuccess;
    }

    public override NodeState Execute()
    {
        float distance = Vector3.Distance(_origin.position, _target.position);
        myNodeState = distance < _distanceForSuccess ? NodeState.SUCCESS : NodeState.RUNNING;
        if(myNodeState == NodeState.RUNNING)
        {
            _ai.SetTarget(_target);
            _ai.Move();
        }

        return myNodeState;
    }
}
