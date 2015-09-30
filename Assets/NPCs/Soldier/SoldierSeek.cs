using System.Collections.Generic;
using UnityEngine;

public class SoldierSeek : BaseState<Soldier>
{
	private readonly NpcPath npcPath;

	private bool useArriveForce;

	private Vector3 SeekPoint;

    private float neighborInterval;
    private float neighborCooldown;

    private float seeInterval;
    private float seeCooldown;
    private float closestSeenTargetDistanceSquared;

    private float hearInterval;
    private float hearCooldown;
    private float closestHeardTargetDistanceSquared;

    private List<Transform> neighbors;

	public SoldierSeek(Soldier npc, Vector3 seekPoint) : base(npc)
	{
		Debug.Log("Seek");
		SeekPoint = seekPoint;

	    neighborInterval = 0.1f;
	    seeInterval = 0.3f;
	    hearInterval = 0.3f;

		npcPath = new NpcPath(NPC);
        neighbors = new List<Transform>();

	    NPC.TargetSpeed = 0.5f;
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
		    steerForce += NPC.Steering.ArriveForce(SeekPoint);//npcPath.GetCurrentPathTargetPosition());
			if (steerForce.sqrMagnitude > sqrMaxSpeed)
				return NPC.Speed * steerForce.normalized;
		}

		steerForce += NPC.Steering.SeekForce(npcPath.GetCurrentPathTargetPosition());
		if (steerForce.sqrMagnitude > sqrMaxSpeed)
			return NPC.Speed * steerForce.normalized;

		return steerForce;
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
            var toTargetDistanceSquared = (target.position - NPC.transform.position).sqrMagnitude;
            if (toTargetDistanceSquared < closestSeenTargetDistanceSquared)
            {
                closestSeenTargetDistanceSquared = toTargetDistanceSquared;
                NPC.State = new SoldierChase(NPC, target);
            }
        }
    }

    private void HearTarget(Transform target)
    {
        if (target != NPC.transform)
        {
            var toTargetDistanceSquared = (target.position - NPC.transform.position).sqrMagnitude;
            if (toTargetDistanceSquared < closestHeardTargetDistanceSquared)
            {
                closestHeardTargetDistanceSquared = toTargetDistanceSquared;
                SeekPoint = target.position;
            }
        }
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
            closestSeenTargetDistanceSquared = Mathf.Infinity;
            NPC.SightSensor.Detect(SeeTarget);
            seeCooldown = seeInterval;
        }
        hearCooldown -= Time.deltaTime;
        if (hearCooldown < 0f)
        {
            closestHeardTargetDistanceSquared = Mathf.Infinity;
            NPC.HearingSensor.Detect(HearTarget);
            hearCooldown = hearInterval;
        }
    }

	public override void UpdateState()
	{
        CheckSensors();

	    npcPath.Update(SeekPoint);
		useArriveForce = npcPath.IsFinalPathPoint();

        NPC.Velocity += GetSteeringForce() * Time.deltaTime;

		// Locomotion
		var targetForward = Utility.AtHeight(npcPath.GetCurrentPathTargetPosition(), 0f) - Utility.AtHeight(NPC.transform.position, 0f);

		NPC.transform.rotation = Quaternion.Lerp(NPC.transform.rotation, Quaternion.LookRotation(targetForward), Time.deltaTime);
		NPC.AnimationController.SetBool("IsAim", true);
		NPC.AnimationController.SetFloat("Speed", NPC.Velocity.magnitude);

        NPC.AnimationController.SetFloat("HorizontalSpeed", Vector3.Dot(targetForward.normalized * NPC.Speed, NPC.transform.right));
        NPC.AnimationController.SetFloat("VerticalSpeed", Vector3.Dot(targetForward.normalized * NPC.Speed, NPC.transform.forward));

		npcPath.SetLastDestination(SeekPoint);
	}
}
