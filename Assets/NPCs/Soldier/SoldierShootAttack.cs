using System.Collections.Generic;
using UnityEngine;

public class SoldierShootAttack : BaseState<Soldier>
{
	private Transform attackTarget;

    private float neighborInterval;
    private float neighborCooldown;
    private List<Transform> neighbors;

	public float AttackInterval = 0.4f;
	private float attackCooldown = 0;

    private readonly NpcPath npcPath;

	public SoldierShootAttack(Soldier npc, Transform target) : base(npc)
	{
	    Name = "Shoot";
		Debug.Log("Shoot");
		attackTarget = target;

		NPC.TargetSpeed = 0.5f;
		attackCooldown = Random.Range(0, AttackInterval);

        npcPath = new NpcPath(NPC);
        neighbors = new List<Transform>();
    }

	private Vector3 GetSteeringForce()
	{
		var sqrMaxSpeed = NPC.Speed * NPC.Speed;
		var steerForce = Vector3.zero;

        steerForce += NPC.Steering.SeparationForce(neighbors, NPC.NeighborSensor.Distance);
        if (steerForce.sqrMagnitude > sqrMaxSpeed)
            return NPC.Speed * steerForce.normalized;

        steerForce += NPC.Steering.ArriveForce(npcPath.GetCurrentPathTargetPosition());
		if (steerForce.sqrMagnitude > sqrMaxSpeed)
			return NPC.Speed * steerForce.normalized;

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

    private void CheckSensors()
    {
        neighborCooldown -= Time.deltaTime;
        if (neighborCooldown < 0f)
        {
            neighbors = new List<Transform>();
            NPC.NeighborSensor.Detect(DetectNeighbor);
            neighborCooldown = neighborInterval;
        }
    }

	public override void UpdateState()
	{
		if (attackTarget == null)
		{
			NPC.State = new SoldierWander(NPC);
			return;
		}
		if (NPC.IsDistanceGreaterThan(attackTarget.position, NPC.ShootAttackRadius))
		{
			if (attackTarget != null)
			{
				NPC.State = new SoldierChase(NPC, attackTarget);
			}
			else
			{
				NPC.State = new SoldierWander(NPC);
			}
		}

        var toTarget = attackTarget.position - NPC.transform.position;
        var destination = attackTarget.position - toTarget.normalized * (NPC.ShootAttackRadius - 1f);

        npcPath.Update(destination);

		NPC.Velocity += GetSteeringForce() * Time.deltaTime;

        var targetForward = Utility.AtHeight(attackTarget.position, 0f) - Utility.AtHeight(NPC.transform.position, 0f);
        NPC.transform.rotation = Quaternion.Lerp(NPC.transform.rotation, Quaternion.LookRotation(targetForward), 5f * Time.deltaTime);

		// Locomotion
		NPC.AnimationController.SetBool("IsAim", true);
		NPC.AnimationController.SetFloat("Speed", NPC.Velocity.magnitude);
		NPC.AnimationController.SetFloat("HorizontalSpeed", Vector3.Dot(targetForward.normalized * NPC.Speed, NPC.transform.right));
		NPC.AnimationController.SetFloat("VerticalSpeed", Vector3.Dot(targetForward.normalized * NPC.Speed, NPC.transform.forward));

        npcPath.SetLastDestination(destination);

		attackCooldown -= Time.deltaTime;
		if (attackCooldown <= 0)
		{
			NPC.AnimationController.SetTrigger("Shoot");

			RaycastHit hit;
			if (Physics.SphereCast(NPC.transform.position + Vector3.up, 0.2f, NPC.transform.forward, out hit, 50f))
			{
				var hitDetectable = hit.collider.GetComponent<Detectable>();
				if (hitDetectable != null)
				{
					var soldier = hitDetectable.Target.GetComponent<Soldier>();
					soldier.Die();
				}
				else
				{
					Debug.Log("Missed!");
				}
			}

			Debug.Log(NPC.name + " shoot");
			attackCooldown = AttackInterval;
		}
	}
}
