using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeNode : Node
{
    private Transform origin;
    private Transform target;
    private float range;

    public RangeNode(Transform origin, Transform target, float range)
    {
        this.origin = origin;
        this.target = target;
        this.range = range;
    }

    public override NodeState Execute()
    {
        float distance = Vector3.Distance(origin.position, target.position);
        myNodeState = distance <= range ? NodeState.SUCCESS : NodeState.FAILURE;

        return myNodeState;
    }
}
