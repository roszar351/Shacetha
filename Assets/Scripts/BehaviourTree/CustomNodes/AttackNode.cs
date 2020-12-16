using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackNode : Node
{
    private Enemy ai;

    public AttackNode(Enemy ai)
    {
        this.ai = ai;
    }

    public override NodeState Execute()
    {
        ai.Attack();
        myNodeState = NodeState.RUNNING;

        return myNodeState;
    }
}
