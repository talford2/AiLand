using UnityEngine;

public class SoldierChase : BaseState<Soldier>
{
	public float TargetExpirationTime = 1;

	private float targetExpirationCooldown = 0;

	private readonly NpcPath npcPath;

	private bool useArriveForce;

	public SoldierChase(Soldier npc, Transform chaseTransform) : base(npc)
	{
		Debug.Log("Chase");
		NPC.Target = chaseTransform;
		npcPath = new NpcPath(NPC);
		targetExpirationCooldown = TargetExpirationTime;
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

		npcPath.Update(NPC.Target.position);
		useArriveForce = npcPath.IsFinalPathPoint();

		NPC.Velocity += GetSteeringForce() * Time.deltaTime;

		// Locomotion
		var targetForward = Utility.AtHeight(npcPath.GetCurrentPathTargetPosition(), 0f) - Utility.AtHeight(NPC.transform.position, 0f);

		NPC.transform.rotation = Quaternion.Lerp(NPC.transform.rotation, Quaternion.LookRotation(targetForward), Time.deltaTime);
		NPC.AnimationController.SetBool("IsAim", false);
		NPC.AnimationController.SetFloat("Speed", NPC.Velocity.magnitude);

		NPC.AnimationController.SetFloat("HorizontalSpeed", Vector3.Dot(targetForward, NPC.transform.right));
		NPC.AnimationController.SetFloat("VerticalSpeed", Vector3.Dot(targetForward, NPC.transform.forward));

		npcPath.SetLastDestination(NPC.Target.position);

		if (NPC.Target != null && !NPC.IsDistanceGreaterThan(3))
		{
			Debug.Log("A");
			NPC.State = new SoldierShootAttack(NPC);
		}
		else
		{
			Debug.Log("B");
		}

		// Target expires start seeking
		targetExpirationCooldown -= Time.deltaTime;
		if (targetExpirationCooldown <= 0)
		{
			var lastSeenPoint = NPC.Target.position;
			NPC.Target = null;
			NPC.State = new SoldierSeek(NPC, lastSeenPoint);
		}
	}

	public override void SeeTarget(Transform target)
	{
		targetExpirationCooldown = TargetExpirationTime;
		NPC.Target = target;
		base.SeeTarget(target);
	}
}
