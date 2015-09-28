using UnityEngine;

public class SoldierChase : BaseState<Soldier>
{
	private Transform chaseTarget;

	private float seeInterval;
	private float seeCooldown;

	public float TargetExpirationTime = 1;

	private float targetExpirationCooldown = 0;

	private readonly NpcPath npcPath;

	private bool useArriveForce;

	public SoldierChase(Soldier npc, Transform chaseTransform) : base(npc)
	{
		Debug.Log("Chase");
		chaseTarget = chaseTransform;
		npcPath = new NpcPath(NPC);
		targetExpirationCooldown = TargetExpirationTime;
		seeInterval = 0.3f;
	}

	private Vector3 GetSteeringForce()
	{
		var sqrMaxSpeed = NPC.Speed * NPC.Speed;
		var steerForce = Vector3.zero;

		if (useArriveForce)
		{
			steerForce += NPC.Steering.ArriveForce(npcPath.GetCurrentPathTargetPosition());
			if (steerForce.sqrMagnitude > sqrMaxSpeed)
				return NPC.Speed * steerForce.normalized;
		}

		steerForce += NPC.Steering.SeekForce(npcPath.GetCurrentPathTargetPosition());
		if (steerForce.sqrMagnitude > sqrMaxSpeed)
			return NPC.Speed * steerForce.normalized;

		return steerForce;
	}

	private void CheckSensors()
	{
		seeCooldown -= Time.deltaTime;
		if (seeCooldown < 0f)
		{
			NPC.SightSensor.Detect(SeeTarget);
			seeCooldown = seeInterval;
		}
	}

	public override void UpdateState()
	{
		CheckSensors();

		if (chaseTarget != null && !NPC.IsDistanceGreaterThan(chaseTarget.position, NPC.ShootAttackRadius))
		{
			NPC.State = new SoldierShootAttack(NPC, chaseTarget);
			return;
		}

		npcPath.Update(chaseTarget.position);
		useArriveForce = npcPath.IsFinalPathPoint();

        NPC.Speed = Mathf.Lerp(NPC.Speed, 1.0f, Time.deltaTime);
        NPC.Velocity += GetSteeringForce() * Time.deltaTime;

		// Locomotion
		Vector3 targetForward;
		if (npcPath.IsFinalPathPoint())
		{
			targetForward = Utility.AtHeight(chaseTarget.position, 0f) - Utility.AtHeight(NPC.transform.position, 0f);
		}
		else
		{
			targetForward = Utility.AtHeight(npcPath.GetCurrentPathTargetPosition(), 0f) - Utility.AtHeight(NPC.transform.position, 0f);
		}

		NPC.transform.rotation = Quaternion.Lerp(NPC.transform.rotation, Quaternion.LookRotation(targetForward), 5f * Time.deltaTime);
		NPC.AnimationController.SetBool("IsAim", false);
		NPC.AnimationController.SetFloat("Speed", NPC.Velocity.magnitude);

		NPC.AnimationController.SetFloat("HorizontalSpeed", Vector3.Dot(targetForward, NPC.transform.right));
		NPC.AnimationController.SetFloat("VerticalSpeed", Vector3.Dot(targetForward, NPC.transform.forward));

		npcPath.SetLastDestination(chaseTarget.position);

		// Target expires start seeking
		targetExpirationCooldown -= Time.deltaTime;
		if (targetExpirationCooldown <= 0)
		{
			var lastSeenPoint = chaseTarget.position;
			chaseTarget = null;
			NPC.State = new SoldierSeek(NPC, lastSeenPoint);
		}

	}

	private void SeeTarget(Transform target)
	{
		targetExpirationCooldown = TargetExpirationTime;
		chaseTarget = target;
	}
}
