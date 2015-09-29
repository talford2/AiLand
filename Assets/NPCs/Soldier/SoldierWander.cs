﻿using UnityEngine;

public class SoldierWander : BaseState<Soldier>
{
    private float hearInterval;
    private float hearCooldown;

    private float seeInterval;
    private float seeCooldown;

    private bool useArriveForce;

    private readonly NpcPath npcPath;

    private Vector3 wanderDestination;

    public SoldierWander(Soldier npc) : base(npc)
    {
        Debug.Log("Wander");
        npcPath = new NpcPath(NPC);
        wanderDestination = GetWanderPosition();

        hearInterval = 0.3f;
        seeInterval = 0.3f;

        NPC.TargetSpeed = 0.5f;
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

    private void SeeTarget(Transform target)
    {
        if (target != NPC.transform)
            NPC.State = new SoldierChase(NPC, target);
    }

    private void HearTarget(Transform target)
    {
        if (target != NPC.transform)
            NPC.State = new SoldierSeek(NPC, target.position);
    }

    private Vector3 GetWanderPosition()
    {
        var wanderDistance = 10f;
        var wanderRadius = 5f;
        var randomInCircle = Random.insideUnitCircle*wanderRadius;

        var wanderPosition = NPC.transform.position + NPC.transform.forward*wanderDistance + new Vector3(randomInCircle.x, 0f, randomInCircle.y);

        if (!npcPath.PathExistsTo(wanderPosition))
        {
            var wanderRay = new Ray(Utility.AtHeight(NPC.transform.position, 1f), Utility.AtHeight(wanderPosition, 1f) - Utility.AtHeight(NPC.transform.position, 1f));
            RaycastHit wanderHit;
            if (Physics.Raycast(wanderRay, out wanderHit, wanderDistance))
            {
                var remainingDistance = wanderDistance - wanderHit.distance;
                wanderPosition = wanderHit.point + Vector3.Reflect(wanderRay.direction, wanderHit.normal).normalized*remainingDistance;
            }
        }
        return wanderPosition;
    }

    private void CheckSensors()
    {
        seeCooldown -= Time.deltaTime;
        if (seeCooldown < 0f)
        {
            NPC.SightSensor.Detect(SeeTarget);
            seeCooldown = seeInterval;
        }
        hearCooldown -= Time.deltaTime;
        if (hearCooldown < 0f)
        {
            NPC.HearingSensor.Detect(HearTarget);
            hearCooldown = hearInterval;
        }
    }

    public override void UpdateState()
    {
        CheckSensors();

        npcPath.Update(wanderDestination);

        useArriveForce = npcPath.IsFinalPathPoint();

        NPC.Velocity += GetSteeringForce() * Time.deltaTime;

        // Locomotion
        var targetForward = Utility.AtHeight(npcPath.GetCurrentPathTargetPosition(), 0f) - Utility.AtHeight(NPC.transform.position, 0f);

        NPC.transform.rotation = Quaternion.Lerp(NPC.transform.rotation, Quaternion.LookRotation(targetForward), 5f*Time.deltaTime);
        NPC.AnimationController.SetBool("IsAim", false);
        NPC.AnimationController.SetFloat("Speed", NPC.Velocity.magnitude);

        NPC.AnimationController.SetFloat("HorizontalSpeed", Vector3.Dot(targetForward.normalized * NPC.Speed, NPC.transform.right));
        NPC.AnimationController.SetFloat("VerticalSpeed", Vector3.Dot(targetForward.normalized * NPC.Speed, NPC.transform.forward));

        npcPath.SetLastDestination(wanderDestination);
        if (npcPath.HasArrived())
        {
            wanderDestination = GetWanderPosition();
        }
    }
}
