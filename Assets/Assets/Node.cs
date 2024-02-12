using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    //whether the node is not an obstacle
    public bool walkable = true;

    // position in the actual game "world"
    public Vector3 worldPosition;

    // gCost is distance from start node to this node
    // hCost is estimated distance from this node to target node
    public int gCost, hCost;

    // Node we came from to this node (for retracing steps)
    public Node parentNode;

    // the node x and y coordinate in the grid
    public int x, y;

    public Node(int x,int y)
    {
        this.x = x;
        this.y = y;
    }

    public int FCost
    {
        get
        {
            return gCost + hCost;
        }
    }
}
