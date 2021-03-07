using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Describes a node in a behaviour tree
[System.Serializable]
public abstract class Node 
{
    protected NodeState myNodeState;

    // allow to check the node state but prevent changing it outside the node itself
    public NodeState nodeState { get { return myNodeState; } }

    public abstract NodeState Execute();
}

public enum NodeState
{ 
    RUNNING, FAILURE, SUCCESS,
}


