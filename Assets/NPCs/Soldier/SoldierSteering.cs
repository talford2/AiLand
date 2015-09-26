using UnityEngine;

public class SoldierSteering : BaseState<Soldier>
{
    public SoldierSteering(Soldier npc) : base(npc)
    {
    }

    public Vector3 SeekForce(Vector3 position)
    {
        var desiredVelocity = (position - NPC.transform.position).normalized*NPC.MaxSpeed;
        return desiredVelocity - NPC.Velocity;
    }

    public Vector3 ArriveForce(Vector3 position)
    {
        var toPosition = position - NPC.transform.position;
        var distance = toPosition.magnitude;
        var deceleration = 0.6f;
        if (distance > 0f)
        {
            var speed = distance / deceleration;
            speed = Mathf.Min(speed, NPC.MaxSpeed);
            var desiredVelocity = toPosition * speed / distance;
            return desiredVelocity - NPC.Velocity;
        }
        return Vector3.zero;
    }
}
