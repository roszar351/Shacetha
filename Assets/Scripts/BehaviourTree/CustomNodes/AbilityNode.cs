using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityNode : Node
{
    private Enemy ai;

    public AbilityNode(Enemy ai)
    {
        this.ai = ai;
    }

    public override NodeState Execute()
    {
        bool abilityUsed = ai.UseAbility();
        if (abilityUsed)
            myNodeState = NodeState.RUNNING;
        else
            myNodeState = NodeState.FAILURE;

        return myNodeState;
    }
}
