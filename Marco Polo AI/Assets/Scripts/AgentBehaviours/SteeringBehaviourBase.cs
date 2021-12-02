using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))] //Requires the GameObject to have the Player script attached to it
public abstract class SteeringBehaviourBase : MonoBehaviour
{
    public abstract Vector3 Calculate(); //A method to calculate the movement of the player objects
}
