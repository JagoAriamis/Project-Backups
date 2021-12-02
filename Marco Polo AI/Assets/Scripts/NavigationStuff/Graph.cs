using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour
{
    // Layers to mark as obstacles
    public LayerMask ObstacleLayer;

    // A Vector2 to store the size of the graph in the world
    public Vector2 GraphWorldSize;

    // Defines how big each node square will be
    public float NodeRadius;

    // Defines the distance that each node will have between one another
    public float NodeDistance;

    // An array of nodes
    Nodes[,] graph;

    // The completed path that will be drawn after the algorithm finishes
    public List<Nodes> ShortestPath;

    float NodeDiameter;
    int GraphSizeX, GraphSizeY;

    private void Start()
    {
        NodeDiameter = NodeRadius * 2;

        // Divide the graph's world size by node diameter to get the graph size in array units
        GraphSizeX = Mathf.RoundToInt(GraphWorldSize.x / NodeDiameter);
        GraphSizeY = Mathf.RoundToInt(GraphWorldSize.y / NodeDiameter);
        DrawGraph();
    }

    void DrawGraph()
    {
        graph = new Nodes[GraphSizeX, GraphSizeY];

        // Get world pos of the graph from the bottom left
        Vector3 BottomLeft = transform.position - Vector3.right * GraphWorldSize.x / 2 - Vector3.forward * GraphWorldSize.y / 2;

        // Nested for loop, looping through array of nodes. i = x, j = y
        for (int i = 0; i < GraphSizeX; i++)
        {
            for (int j = 0; j < GraphSizeY; j++)
            {
                // Get the world coordinates from the bottom left of the graph
                Vector3 WorldPoint = BottomLeft + Vector3.right * (i * NodeDiameter + NodeRadius) + Vector3.forward * (j * NodeDiameter + NodeRadius);
                bool Obstacle = true; 

                // Collision check against current node and its position. If anything obstructs it, the statement returns false
                if (Physics.CheckSphere(WorldPoint, NodeRadius, ObstacleLayer))
                    Obstacle = false;

                // Create a new node in the array
                graph[i, j] = new Nodes(Obstacle, WorldPoint, i, j);
            }
        }
    }

    // Draws the graph in pretty colours for debug purposes
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(GraphWorldSize.x, 1, GraphWorldSize.y));

        if (graph != null) // Check if graph is not empty
        {
            foreach(Nodes node in graph)
            {
                if (node.IsObstacle)
                {
                    Gizmos.color = Color.clear;
                }

                else
                {
                    Gizmos.color = Color.red;
                }

                if (ShortestPath != null) // Check if shortest path is not empty
                {
                    if (ShortestPath.Contains(node))
                        Gizmos.color = Color.blue;
                }

                Gizmos.DrawCube(node.Position, Vector3.one * (NodeDiameter - NodeDistance)); // Draw a node at the position of the node
            }
        }
    }

    // Gets the closest node from a Vector3 world position
    public Nodes NodeFromWorldPos(Vector3 WorldPosition)
    {
        float XPoint = ((WorldPosition.x + GraphWorldSize.x / 2) / GraphWorldSize.x);
        float YPoint = ((WorldPosition.z + GraphWorldSize.y / 2) / GraphWorldSize.y);

        XPoint = Mathf.Clamp01(XPoint);
        YPoint = Mathf.Clamp01(YPoint);

        int x = Mathf.RoundToInt((GraphSizeX - 1) * XPoint);
        int y = Mathf.RoundToInt((GraphSizeY - 1) * YPoint);

        return graph[x, y];
    }

    public List<Nodes> GetNeighbourNode(Nodes node)
    {
        List<Nodes> NeighbourNodes = new List<Nodes>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if ( x == 0 && y == 0 )
                {
                    continue;
                }

                int CheckX = node.NodeX + x;
                int CheckY = node.NodeY + y;

                if (CheckX >= 0 && CheckX < GraphSizeX && CheckY >= 0 && CheckY < GraphSizeY)
                {
                    NeighbourNodes.Add(graph[CheckX, CheckY]);
                }
            }
        }

        return NeighbourNodes;
    }
}
