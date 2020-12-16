using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseNode : Node
{
    private Enemy ai;
    private Transform origin;
    private Transform target;

    public ChaseNode(Enemy ai, Transform origin, Transform target)
    {
        this.ai = ai;
        this.origin = origin;
        this.target = target;
    }

    public override NodeState Execute()
    {
        float distance = Vector3.Distance(origin.position, target.position);
        myNodeState = distance < 0.1f ? NodeState.SUCCESS : NodeState.RUNNING;
        if(myNodeState == NodeState.RUNNING)
        {
            ai.SetTarget(target);
            ai.Move();
        }

        return myNodeState;
    }
}
