using UnityEngine;

public class SoldierSeek : BaseState<Soldier>
{
	private readonly NpcPath npcPath;

	private bool useArriveForce;

	private Vector3 SeekPoint;

    private float seeInterval;
    private float seeCooldown;

    private float hearInterval;
    private float hearCooldown;

	public SoldierSeek(Soldier npc, Vector3 seekPoint) : base(npc)
	{
		Debug.Log("Seek");
		SeekPoint = seekPoint;

	    seeInterval = 0.3f;
	    hearInterval = 0.3f;

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

    private void SeeTarget(Transform target)
	{
		NPC.State = new SoldierChase(NPC, target);
	}

	private void HearTarget(Transform target)
	{
		SeekPoint = target.position;
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

	public override void Update()
	{
		base.Update();

        CheckSensors();

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
