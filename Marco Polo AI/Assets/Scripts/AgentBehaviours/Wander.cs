using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wander : SteeringBehaviourBase
{
    // The radius of the constraining circle
    public float WanderRadius = 10f;
    
    // The distance in front of the agent the circle is moved
    public float WanderDistance = 10f;

    // The amount of random displacement in a single second 
    public float WanderJitter = 1f;

    // The angle that represents the point on the circle
    public float WanderAngle = 0.0f; 

    // The actual target position; initialised to a random value within the WanderRadius using WanderAngle
    Vector3 WanderTarget = Vector3.zero;

    void Start()
    {
        WanderAngle = Random.Range(0.0f, Mathf.PI * 2);
        WanderTarget = new Vector3(Mathf.Cos(WanderAngle), 0, Mathf.Sin(WanderAngle)) * WanderRadius;
    }
    public override Vector3 Calculate()
    {
        WanderAngle += Random.Range(-WanderJitter, WanderJitter);

        WanderTarget = new Vector3(Mathf.Cos(WanderAngle), 0, Mathf.Sin(WanderAngle)) * WanderRadius;

        Vector3 targetLocal = WanderTarget;
        Vector3 targetWorld = transform.position + WanderTarget;

        targetWorld += transform.forward * WanderDistance;

        return targetWorld - transform.position;
    }
}
