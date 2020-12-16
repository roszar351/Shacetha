using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector : Node
{
    protected List<Node> childNodes = new List<Node>();

    public Selector(List<Node> nodes)
    {
        this.childNodes = nodes;
    }

    public override NodeState Execute()
    {
        foreach (var node in childNodes)
        {
            switch (node.Execute())
            {
                case NodeState.RUNNING:
                    myNodeState = NodeState.RUNNING;
                    return myNodeState;
                case NodeState.FAILURE:
                    break;
                case NodeState.SUCCESS:
                    myNodeState = NodeState.SUCCESS;
                    return myNodeState;
                default:
                    break;
            }
        }

        myNodeState = NodeState.FAILURE;
        return myNodeState;
    }
}
