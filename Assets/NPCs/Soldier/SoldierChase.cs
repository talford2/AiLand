using System.Collections.Generic;
using UnityEngine;

public class SoldierChase : BaseState<Soldier>
{
	private Transform chaseTarget;

    private float neighborInterval;
    private float neighborCooldown;

	private float seeInterval;
	private float seeCooldown;

    private List<Transform> neighbors;

	public float TargetExpirationTime = 1;

	private float targetExpirationCooldown = 0;

	private readonly NpcPath npcPath;

	private bool useArriveForce;

	public SoldierChase(Soldier npc, Transform chaseTransform) : base(npc)
	{
	    Name = "Chase";
		Debug.Log("Chase");
		chaseTarget = chaseTransform;
		npcPath = new NpcPath(NPC);
        neighbors = new List<Transform>();

		targetExpirationCooldown = TargetExpirationTime;
        neighborInterval = 0.1f;
        seeInterval = 0.3f;

		NPC.TargetSpeed = 1.0f;
	}

	private Vector3 GetSteeringForce()
	{
		var sqrMaxSpeed = NPC.Speed * NPC.Speed;
		var steerForce = Vector3.zero;

        steerForce += NPC.Steering.SeparationForce(neighbors, NPC.NeighborSensor.Distance);
	    if (steerForce.sqrMagnitude > sqrMaxSpeed)
            return NPC.Speed * steerForce.normalized;

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
        neighborCooldown -= Time.deltaTime;
        if (neighborCooldown < 0f)
        {
            neighbors = new List<Transform>();
            NPC.NeighborSensor.Detect(DetectNeighbor);
            neighborCooldown = neighborInterval;
        }
		seeCooldown -= Time.deltaTime;
		if (seeCooldown < 0f)
		{
			NPC.SightSensor.Detect(SeeTarget);
			seeCooldown = seeInterval;
		}
	}

	public override void UpdateState()
	{
		if (chaseTarget == null)
		{
			NPC.State = new SoldierWander(NPC);
			return;
		}

		CheckSensors();

		if (chaseTarget != null && !NPC.IsDistanceGreaterThan(chaseTarget.position, NPC.ShootAttackRadius))
		{
			NPC.State = new SoldierShootAttack(NPC, chaseTarget);
			return;
		}

        var toTarget = chaseTarget.position - NPC.transform.position;
	    var destination = chaseTarget.position - toTarget.normalized*(NPC.ShootAttackRadius - 1f);

		npcPath.Update(destination);
		useArriveForce = npcPath.IsFinalPathPoint();

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

		NPC.AnimationController.SetFloat("HorizontalSpeed", Vector3.Dot(targetForward.normalized * NPC.Speed, NPC.transform.right));
		NPC.AnimationController.SetFloat("VerticalSpeed", Vector3.Dot(targetForward.normalized * NPC.Speed, NPC.transform.forward));

        npcPath.SetLastDestination(destination);

		// Target expires start seeking
		targetExpirationCooldown -= Time.deltaTime;
		if (targetExpirationCooldown <= 0)
		{
			var lastSeenPoint = chaseTarget.position;
			chaseTarget = null;
			NPC.State = new SoldierSeek(NPC, lastSeenPoint);
		}

	}

    private void DetectNeighbor(Transform target)
    {
        if (target != NPC.transform)
        {
            if (!neighbors.Contains(target))
                neighbors.Add(target);
        }
    }

	private void SeeTarget(Transform target)
	{
		if (target != NPC.transform)
		{
			targetExpirationCooldown = TargetExpirationTime;
			chaseTarget = target;
		}
	}
}
