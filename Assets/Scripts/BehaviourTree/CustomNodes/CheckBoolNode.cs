using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckBoolNode : Node
{
    private Enemy ai;
    private int whichBoolVal;

    public CheckBoolNode(Enemy ai, int whichBool = 0)
    {
        this.ai = ai;
        whichBoolVal = whichBool;
    }

    public override NodeState Execute()
    {
        if (ai.CheckBool(whichBoolVal))
            myNodeState = NodeState.SUCCESS;
        else
            myNodeState = NodeState.FAILURE;

        return myNodeState;
    }
}
