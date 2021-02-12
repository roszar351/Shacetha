using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunAwayNode : Node
{
    private Enemy ai;
    private Transform origin;
    private Transform target;
    private float distanceForSuccess;

    public RunAwayNode(Enemy ai, Transform origin, Transform target, float distanceForSuccess = 10f)
    {
        this.ai = ai;
        this.origin = origin;
        this.target = target;
        this.distanceForSuccess = distanceForSuccess;
    }

    public override NodeState Execute()
    {
        float distance = Vector3.Distance(origin.position, target.position);
        myNodeState = distance > distanceForSuccess ? NodeState.SUCCESS : NodeState.RUNNING;
        if(myNodeState == NodeState.RUNNING)
        {
            ai.SetTarget(target);
            ai.MoveAway();
        }

        return myNodeState;
    }
}
