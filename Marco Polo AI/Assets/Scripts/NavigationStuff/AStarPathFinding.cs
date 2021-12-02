using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarPathFinding : MonoBehaviour
{
    Graph graph;
    PathFollow pathFollow;
 
    public Transform StartPosition;
    public Transform TargetPosition;

    private void Start()
    {
        // References the object containing the graph data. Object won't know what's going on without it. This is because the graph object is separate from the object that's following
        graph = GameObject.Find("GraphManager").GetComponent<Graph>();
        pathFollow = GetComponent<PathFollow>();
    }

    // Get path follow script and pass in its list of nodes

    private void Update()
    {
        pathFollow.nodes = FindPath(StartPosition.position, TargetPosition.position);
    }

    public List<Nodes> FindPath(Vector3 StartPos, Vector3 TargetPos) 
    {
        // Gets the node closest to the starting position
        Nodes StartNode = graph.NodeFromWorldPos(StartPos);

        // Gets the node closest to the target position
        Nodes TargetNode = graph.NodeFromWorldPos(TargetPos);

        // List of nodes that are yet to be visited
        List<Nodes> OpenList = new List<Nodes>();

        // List of nodes that have already been checked/visited
        HashSet<Nodes> ClosedList = new HashSet<Nodes>();

        // Add the starting node to the open list
        OpenList.Add(StartNode);

        while (OpenList.Count > 0)
        {
            Nodes CurrentNode = OpenList[0]; // Create a node and set it to the first index in the open list

            for (int i = 0; i < OpenList.Count; i++) 
            {
                // If the F Cost is less than OR equal to the F Cost of the current node
                if (OpenList[i].FCost < CurrentNode.FCost || OpenList[i].FCost == CurrentNode.FCost && OpenList[i].HCost < CurrentNode.HCost)
                {
                    // Set the current node to that node
                    CurrentNode = OpenList[i];
                }
            }

            OpenList.Remove(CurrentNode); // Remove that node from the open list
            ClosedList.Add(CurrentNode); // Then add it to the closed list

            // If we've found the target node
            if (CurrentNode == TargetNode)
            {
                // Get the shortest path to it
                return GetShortestPath(StartNode, TargetNode);
            }

            // Loop through each node that is adjacent to the current node
            foreach (Nodes NeighbourNode in graph.GetNeighbourNode(CurrentNode))
            {
                // Check if the neighbour node is a wall or is already contained in the closed list
                if (!NeighbourNode.IsObstacle || ClosedList.Contains(NeighbourNode))
                {
                    continue;
                }

                // Get the F Cost of that neighbour node
                int MoveCost = CurrentNode.GCost + GetManHanDist(CurrentNode, NeighbourNode);

                // If the F Cost is greater than the G Cost or the neighbour node is not in the open list..
                if (MoveCost < NeighbourNode.GCost || !OpenList.Contains(NeighbourNode))
                {
                    NeighbourNode.GCost = MoveCost; // Set G Cost to the F Cost
                    NeighbourNode.HCost = GetManHanDist(NeighbourNode, TargetNode); // Set the H Cost
                    NeighbourNode.Parent = CurrentNode; // Set the parent of the node to retrace path

                    // If the neighbour node is not in the open list, add it
                    if (!OpenList.Contains(NeighbourNode))
                        OpenList.Add(NeighbourNode);
                }
            }
        }

        // Reaching this point means that all nodes in the open list have been searched
        return null;
    }

    // To retrace the shortest path
    public List<Nodes> GetShortestPath(Nodes StartNode, Nodes EndNode)
    {
        // Sequential path of nodes
        List<Nodes> ShortestPath = new List<Nodes>();
        
        // Assign the current node as the target node
        Nodes CurrentNode = EndNode;

        // Loop through each of the parent nodes, adding each one while it is not the start node
        while (CurrentNode != StartNode)
        {
            ShortestPath.Add(CurrentNode);
            CurrentNode = CurrentNode.Parent;
        }

        // Flip the list to get the correct order
        ShortestPath.Reverse();

        // Set the shortest path both in this script and in the graph script
        return graph.ShortestPath = ShortestPath;
    }

    int GetManHanDist(Nodes nodeA, Nodes nodeB)
    {
        int x = Mathf.Abs(nodeA.NodeX - nodeB.NodeX);
        int y = Mathf.Abs(nodeA.NodeY - nodeB.NodeY);

        return x + y;
    }
}
