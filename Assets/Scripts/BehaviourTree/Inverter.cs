using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inverter : Node
{
    protected Node childNode;

    public Inverter(Node node)
    {
        this.childNode = node;
    }

    public override NodeState Execute()
    {
        switch (childNode.Execute())
        {
            case NodeState.RUNNING:
                myNodeState = NodeState.RUNNING;
                break;
            case NodeState.FAILURE:
                myNodeState = NodeState.SUCCESS;
                break;
            case NodeState.SUCCESS:
                myNodeState = NodeState.FAILURE;
                break;
            default:
                break;
        }

        return myNodeState;
    }
}
