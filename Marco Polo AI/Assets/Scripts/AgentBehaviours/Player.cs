using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Vector3 Velocity;

    SteeringBehaviourBase steeringBehaviour;

    public float Mass = 1f;
    public float MaxSpeed = 1f;
    public float MaxForce = 1f;
    public float MaxTurnRate = 1f;

    // Start is called before the first frame update
    void Start()
    {
        steeringBehaviour = GetComponent<SteeringBehaviourBase>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 steeringForce = steeringBehaviour.Calculate();

        Vector3 Acceleration = steeringForce / Mass;

        Velocity += Acceleration * Time.deltaTime;

        Velocity = Vector3.ClampMagnitude(Velocity, MaxSpeed);

        if (Velocity != Vector3.zero)
        {
            transform.position += Velocity * Time.deltaTime;

            transform.forward = Velocity.normalized;
        }
    }
}
