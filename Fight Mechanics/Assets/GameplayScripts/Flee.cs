using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flee : SteeringBehaviourBase
{
    // The pos to flee from
    public Transform FleePos;

    public override Vector3 Calculate()
    {
        Player player = GetComponent<Player>();
        Vector3 DesiredVelocity = (transform.position - FleePos.position).normalized * player.MaxSpeed;

        return (DesiredVelocity - player.Velocity);
    }
}
