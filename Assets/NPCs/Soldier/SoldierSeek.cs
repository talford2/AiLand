using UnityEngine;

public class SoldierSeek : BaseState<Soldier>
{
	public SoldierSeek(Soldier npc) : base(npc)
	{
		Debug.Log("Seek");
	    NPC.MaxSpeed = 1f;
	}

    private Vector3 GetSteeringForce()
    {
        var sqrMaxSpeed = NPC.MaxSpeed*NPC.MaxSpeed;
        var steerForce = Vector3.zero;

        steerForce += NPC.Steering.ArriveForce(NPC.Target.position);
        if (steerForce.sqrMagnitude > sqrMaxSpeed)
            return NPC.MaxSpeed*steerForce.normalized;

        steerForce += NPC.Steering.SeekForce(NPC.Target.position);
        if (steerForce.sqrMagnitude > sqrMaxSpeed)
            return NPC.MaxSpeed*steerForce.normalized;

        return steerForce;
    }

    public override void Update()
    {
        base.Update();

        NPC.Velocity += GetSteeringForce()*Time.deltaTime;
        
        // Locomotion
        NPC.transform.rotation = Quaternion.LookRotation(NPC.Target.position - NPC.transform.position);
        NPC.AnimationController.SetBool("IsAim", true);
        NPC.AnimationController.SetFloat("Speed", NPC.Velocity.magnitude);
    }
}
