using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckBoolNode : Node
{
    private Enemy _ai;
    private int _whichBoolVal;

    public CheckBoolNode(Enemy ai, int whichBool = 0)
    {
        this._ai = ai;
        _whichBoolVal = whichBool;
    }

    public override NodeState Execute()
    {
        if (_ai.CheckBool(_whichBoolVal))
            myNodeState = NodeState.SUCCESS;
        else
            myNodeState = NodeState.FAILURE;

        return myNodeState;
    }
}
