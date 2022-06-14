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
        targetDistanceList.Sort(delegate(Transform a, Transform b) // Delegate = for callbacks and communicating information between two parties
        {
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

    float DistanceToTarget(Vector3 a, Vector3 b)
    {
        float x = a.x - b.x;
        float z = a.z - b.z;

        float distance = x * x + z * z;

        return distance;
    }
}
