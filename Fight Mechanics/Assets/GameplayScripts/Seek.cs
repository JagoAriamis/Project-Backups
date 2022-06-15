using UnityEngine;

public class Seek : SteeringBehaviourBase
{
    // The pos to seek to
    public Transform SeekPos;

    public override Vector3 Calculate()
    {
        Player player = GetComponent<Player>();
        Vector3 DesiredVelocity = (SeekPos.position - transform.position).normalized * player.MaxSpeed;

        return (DesiredVelocity - player.Velocity);
    }
}
