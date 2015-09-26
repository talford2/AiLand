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
        Debug.Log("RUN SEEK UPDATE!");
        NPC.Velocity += GetSteeringForce()*Time.deltaTime;
        Debug.Log(NPC.Velocity);
        
        // Locomotion
        var targetForward = NPC.transform.position - NPC.transform.position;

        NPC.transform.rotation = Quaternion.Lerp(NPC.transform.rotation, Quaternion.LookRotation(targetForward), Time.deltaTime);
        NPC.AnimationController.SetBool("IsAim", true);
        NPC.AnimationController.SetFloat("Speed", NPC.Velocity.magnitude);

        NPC.AnimationController.SetFloat("HorizontalSpeed", Vector3.Dot(targetForward, NPC.transform.right));
        NPC.AnimationController.SetFloat("VerticalSpeed", Vector3.Dot(targetForward, NPC.transform.forward));
    }
}
