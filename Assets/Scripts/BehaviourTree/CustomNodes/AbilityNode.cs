using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityNode : Node
{
    private Enemy _ai;

    public AbilityNode(Enemy ai)
    {
        this._ai = ai;
    }

    public override NodeState Execute()
    {
        bool abilityUsed = _ai.UseAbility();
        if (abilityUsed)
            myNodeState = NodeState.RUNNING;
        else
            myNodeState = NodeState.FAILURE;

        return myNodeState;
    }
}
