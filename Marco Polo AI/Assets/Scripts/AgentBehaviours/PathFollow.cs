using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFollow : SteeringBehaviourBase
{
    public List<Nodes> nodes = new List<Nodes>();
    int currentNode;
    public Transform TargetPos;

    public override Vector3 Calculate()
    {
        return PathFollowing();
    }

    Vector3 PathFollowing()
    {
        Player player = GetComponent<Player>();
        currentNode = 0;

        if (nodes != null && nodes.Count > 1)
        {
            nodes.RemoveAt(0);
            SetNextWayPoint();
        }

        Vector3 targetPos = CurrentWayPoint();
        if (Vector3.Distance(transform.position, targetPos) > 0.1f)
        {
            Vector3 DesiredVelocity = (targetPos - transform.position).normalized * player.MaxSpeed;

            return DesiredVelocity - player.Velocity;
        }

        else
        {
            currentNode++;
        }

        return player.Velocity -= player.Velocity;
    }

    Vector3 CurrentWayPoint()
    {
        return nodes[currentNode].Position;
    }

    void SetNextWayPoint()
    { 
        if (nodes.Count > 0)
        {
            if (nodes[currentNode].Position != TargetPos.position)
            {
                nodes[currentNode].Position = nodes[0].Position; 
            }
        }
    }
}
