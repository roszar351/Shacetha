using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleNode : Node
{
    private Enemy _ai;

    public IdleNode(Enemy ai)
    {
        this._ai = ai;
    }

    public override NodeState Execute()
    {
        _ai.Idle();
        myNodeState = NodeState.RUNNING;

        return myNodeState;
    }
}
