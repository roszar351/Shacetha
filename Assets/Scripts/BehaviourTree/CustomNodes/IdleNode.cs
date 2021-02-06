using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleNode : Node
{
    private Enemy ai;

    public IdleNode(Enemy ai)
    {
        this.ai = ai;
    }

    public override NodeState Execute()
    {
        ai.Idle();
        myNodeState = NodeState.RUNNING;

        return myNodeState;
    }
}
