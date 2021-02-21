using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackNode : Node
{
    private Enemy _ai;

    public AttackNode(Enemy ai)
    {
        this._ai = ai;
    }

    public override NodeState Execute()
    {
        _ai.Attack();
        myNodeState = NodeState.RUNNING;

        return myNodeState;
    }
}
