using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sequence : Node
{
    protected List<Node> childNodes = new List<Node>();

    public Sequence(List<Node> nodes)
    {
        this.childNodes = nodes;
    }

    public override NodeState Execute()
    {
        bool isAnyChildRunning = false;
        foreach (var node in childNodes)
        {
            switch (node.Execute())
            {
                case NodeState.RUNNING:
                    isAnyChildRunning = true;
                    break;
                case NodeState.FAILURE:
                    myNodeState = NodeState.FAILURE;
                    return myNodeState;
                case NodeState.SUCCESS:
                    break;
                default:
                    break;
            }
        }

        myNodeState = isAnyChildRunning ? NodeState.RUNNING : NodeState.SUCCESS;
        return myNodeState;
    }
}
