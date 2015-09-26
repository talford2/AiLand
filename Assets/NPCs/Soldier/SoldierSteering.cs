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
}
