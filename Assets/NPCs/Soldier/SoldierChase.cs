using UnityEngine;

public class SoldierChase : BaseState<Soldier>
{
    private Transform chaseTransform;

    private readonly NpcPath npcPath;

    private bool useArriveForce;

	public SoldierChase(Soldier npc, Transform chaseTransform) : base(npc)
	{
		Debug.Log("Chase");
	    this.chaseTransform = chaseTransform;
        npcPath = new NpcPath(NPC);
	}

    private Vector3 GetSteeringForce()
    {
        var sqrMaxSpeed = NPC.MaxSpeed * NPC.MaxSpeed;
        var steerForce = Vector3.zero;

        if (useArriveForce)
        {
            steerForce += NPC.Steering.ArriveForce(npcPath.GetCurrentPathTargetPosition());
            if (steerForce.sqrMagnitude > sqrMaxSpeed)
                return NPC.MaxSpeed * steerForce.normalized;
        }

        steerForce += NPC.Steering.SeekForce(npcPath.GetCurrentPathTargetPosition());
        if (steerForce.sqrMagnitude > sqrMaxSpeed)
            return NPC.MaxSpeed * steerForce.normalized;

        return steerForce;
    }

    public override void Update()
    {
        base.Update();

        npcPath.Update(chaseTransform.position);
        useArriveForce = npcPath.IsFinalPathPoint();

        NPC.Velocity += GetSteeringForce() * Time.deltaTime;

        // Locomotion
        var targetForward = npcPath.GetCurrentPathTargetPosition() - NPC.transform.position;

        NPC.transform.rotation = Quaternion.Lerp(NPC.transform.rotation, Quaternion.LookRotation(targetForward), Time.deltaTime);
        NPC.AnimationController.SetBool("IsAim", false);
        NPC.AnimationController.SetFloat("Speed", NPC.Velocity.magnitude);

        NPC.AnimationController.SetFloat("HorizontalSpeed", Vector3.Dot(targetForward, NPC.transform.right));
        NPC.AnimationController.SetFloat("VerticalSpeed", Vector3.Dot(targetForward, NPC.transform.forward));

        npcPath.SetLastDestination(chaseTransform.position);
    }
}
