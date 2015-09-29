using System.Collections.Generic;
using UnityEngine;

public class SoldierSteering : BaseState<Soldier>
{
    public SoldierSteering(Soldier npc) : base(npc)
    {
    }

    public Vector3 SeekForce(Vector3 position)
    {
        var desiredVelocity = (position - NPC.transform.position).normalized*NPC.Speed;
        return desiredVelocity - NPC.Velocity;
    }

    public Vector3 SeparationForce(List<Transform> neighbors, float distance)
    {
        var avoidSum = Vector3.zero;
        var distSqr = distance*distance;
        foreach (var neighbor in neighbors)
        {
            if (neighbor != null)
            {
                var fromNeighbour = NPC.transform.position - neighbor.position;
                if (fromNeighbour.sqrMagnitude > 0f && fromNeighbour.sqrMagnitude < distSqr)
                {
                    avoidSum += NPC.Speed*fromNeighbour.normalized/fromNeighbour.magnitude;
                }
            }
        }
        return avoidSum;
    }

    public Vector3 ArriveForce(Vector3 position)
    {
        var toPosition = position - NPC.transform.position;
        var distance = toPosition.magnitude;
        var deceleration = 0.6f;
        if (distance > 0f)
        {
            var speed = distance / deceleration;
            speed = Mathf.Min(speed, NPC.Speed);
            var desiredVelocity = toPosition * speed / distance;
            return desiredVelocity - NPC.Velocity;
        }
        return Vector3.zero;
    }
}
