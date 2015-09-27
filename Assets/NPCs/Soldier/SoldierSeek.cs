using UnityEngine;

public class SoldierSeek : BaseState<Soldier>
{
	//private Vector3 lastTargetPosition;
	//private Vector3[] path;
	//private int curPathIndex;

    private NpcPath npcPath;

	private bool useArriveForce;

	private Vector3 SeekPoint;

	public SoldierSeek(Soldier npc, Vector3 seekPoint) : base(npc)
	{
		Debug.Log("Seek");
		SeekPoint = seekPoint;
		NPC.MaxSpeed = 1f;
		npcPath = new NpcPath(NPC);
        npcPath.SetLastDestination(NPC.transform.position);
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

	    if (npcPath.HasDestinationChanged(SeekPoint))
	        npcPath.SetDestination(SeekPoint);
        npcPath.Update();
	    useArriveForce = npcPath.IsFinalPathPoint();
		
		NPC.Velocity += GetSteeringForce() * Time.deltaTime;

		// Locomotion
		var targetForward = npcPath.GetCurrentPathTargetPosition() - NPC.transform.position;

		NPC.transform.rotation = Quaternion.Lerp(NPC.transform.rotation, Quaternion.LookRotation(targetForward), Time.deltaTime);
		NPC.AnimationController.SetBool("IsAim", true);
		NPC.AnimationController.SetFloat("Speed", NPC.Velocity.magnitude);

		NPC.AnimationController.SetFloat("HorizontalSpeed", Vector3.Dot(targetForward, NPC.transform.right));
		NPC.AnimationController.SetFloat("VerticalSpeed", Vector3.Dot(targetForward, NPC.transform.forward));

		npcPath.SetLastDestination(SeekPoint);
	}

	
}
