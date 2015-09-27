using UnityEngine;

public class SoldierSeek : BaseState<Soldier>
{
	private readonly NpcPath npcPath;

	private bool useArriveForce;

	private Vector3 SeekPoint;

	public SoldierSeek(Soldier npc, Vector3 seekPoint) : base(npc)
	{
		Debug.Log("Seek");
		SeekPoint = seekPoint;
		NPC.MaxSpeed = 1f;
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

	public override void SeeTarget(Transform target)
	{
		NPC.State = new SoldierChase(NPC, target);
	}

	public override void HearTarget(Transform target)
	{
		SeekPoint = target.position;
	}

	public override void Update()
	{
		base.Update();

		npcPath.Update(SeekPoint);
		useArriveForce = npcPath.IsFinalPathPoint();

		NPC.Velocity += GetSteeringForce() * Time.deltaTime;

		// Locomotion
		var targetForward = Utility.AtHeight(npcPath.GetCurrentPathTargetPosition(), 0f) - Utility.AtHeight(NPC.transform.position, 0f);

		NPC.transform.rotation = Quaternion.Lerp(NPC.transform.rotation, Quaternion.LookRotation(targetForward), Time.deltaTime);
		NPC.AnimationController.SetBool("IsAim", true);
		NPC.AnimationController.SetFloat("Speed", NPC.Velocity.magnitude);

		NPC.AnimationController.SetFloat("HorizontalSpeed", Vector3.Dot(targetForward, NPC.transform.right));
		NPC.AnimationController.SetFloat("VerticalSpeed", Vector3.Dot(targetForward, NPC.transform.forward));

		npcPath.SetLastDestination(SeekPoint);
	}
}
