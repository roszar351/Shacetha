using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunAwayNode : Node
{
    private Enemy _ai;

    public RunAwayNode(Enemy ai)
    {
        this._ai = ai;
    }

    public override NodeState Execute()
    {
        myNodeState = NodeState.RUNNING;

        _ai.MoveAway();

        return myNodeState;
    }
}
