using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunAwayNode : Node
{
    private Enemy ai;

    public RunAwayNode(Enemy ai)
    {
        this.ai = ai;
    }

    public override NodeState Execute()
    {
        myNodeState = NodeState.RUNNING;

        ai.MoveAway();

        return myNodeState;
    }
}
