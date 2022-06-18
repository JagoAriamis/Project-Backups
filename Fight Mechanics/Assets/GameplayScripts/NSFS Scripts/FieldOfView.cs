using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public float viewRadius;

    [Range(0, 360)]
    public float viewAngle;

    public LayerMask layerMask;
    public List<Transform> targetDistanceList = new List<Transform>();


    // Should dynamically return targets based on either of the return target methods
    //public delegate void ReturnTargets(string distanceType, params Transform[] targets);

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FindTargetCheck(0.1f));
    }

    IEnumerator FindTargetCheck(float delay)
    {
        while(true)
        {
            yield return new WaitForSeconds(delay);
            FindTargets();
        }
    }

    void FindTargets()
    {
        targetDistanceList.Clear();

        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, layerMask);

        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            Transform target = targetsInViewRadius[i].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < viewAngle / 2)
            {
                targetDistanceList.Add(target);
                PartyListSortByDist();
            }
        }
    }

    void PartyListSortByDist()
    {
        // Condition = Method name (sort) and type parameter for what will be sorted. In this case, the transforms in the List
        targetDistanceList.Sort((Transform a, Transform b) => // => = Lambda expression. Calls the delegate Sort method from the List class without having to write my own (I think?)
        {
            // Executable = Using my own distance calculation, for every transform in the List, return the transforms in order of closest to furthest
            return DistanceToTarget(transform.position, a.transform.position).CompareTo(DistanceToTarget(transform.position, b.transform.position));
        });
    }

    public Vector3 AngleDirection(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    public float DistanceToTarget(Vector3 a, Vector3 b) // We don't need the Y co-ordinate, so to optimise, it's been removed
    {
        float x = a.x - b.x;
        float z = a.z - b.z;

        float distance = x * x + z * z;

        return distance;
    }
}
