using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nodes
{
    public int NodeX; // X pos in the node array
    public int NodeY; // Y pos in the node array

    public bool IsObstacle; // Returns true if a node is obstructed. False by default. !IsObstacle = true;
    public Vector3 Position; // World pos of a node

    public Nodes Parent; // Stores what node came previously to trace a path 

    public int GCost; // Cost of moving to the next square
    public int HCost; // Distance to the target from a node
    public int FCost { get { return GCost + HCost; } } // Returns GCost + HCost 

    public Nodes(bool _IsObstacle, Vector3 _Pos, int _NodeX, int _NodeY)
    {
        IsObstacle = _IsObstacle;
        Position = _Pos;
        NodeX = _NodeX;
        NodeY = _NodeY;
    }
}
