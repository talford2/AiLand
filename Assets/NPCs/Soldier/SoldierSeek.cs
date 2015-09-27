using UnityEngine;

public class SoldierSeek : BaseState<Soldier>
{
	private Vector3 lastTargetPosition;
	private Vector3[] path;
	private int curPathIndex;

	private bool useArriveForce;

	private Vector3 SeekPoint;

	public SoldierSeek(Soldier npc, Vector3 seekPoint) : base(npc)
	{
		Debug.Log("Seek");
		SeekPoint = seekPoint;
		NPC.MaxSpeed = 1f;
		lastTargetPosition = NPC.transform.position;
	}

	private Vector3 GetSteeringForce()
	{
		var sqrMaxSpeed = NPC.MaxSpeed * NPC.MaxSpeed;
		var steerForce = Vector3.zero;

		if (useArriveForce)
		{
			steerForce += NPC.Steering.ArriveForce(path[curPathIndex]);
			if (steerForce.sqrMagnitude > sqrMaxSpeed)
				return NPC.MaxSpeed * steerForce.normalized;
		}

		steerForce += NPC.Steering.SeekForce(path[curPathIndex]);
		if (steerForce.sqrMagnitude > sqrMaxSpeed)
			return NPC.MaxSpeed * steerForce.normalized;

		return steerForce;
	}

	public override void Update()
	{
		base.Update();

		// If target has changed position recalculate path
		var deltaTargetPos = NPC.Target.position - lastTargetPosition;
		if (deltaTargetPos.sqrMagnitude > 1f)
		{
			var navPath = new NavMeshPath();
			if (NavMesh.CalculatePath(NPC.transform.position, NPC.Target.position, NavMesh.AllAreas, navPath))
			{
				path = navPath.corners;
				curPathIndex = 0;
			}
			else
			{
				path = new[] { NPC.transform.position };
				curPathIndex = 0;
			}
		}

		// Increment path index on arriving at current path point
		var toCurPathPos = AtHeight(path[curPathIndex], 0f) - AtHeight(NPC.transform.position, 0f);
		if (toCurPathPos.sqrMagnitude < 0.1f)
		{
			curPathIndex++;
			useArriveForce = curPathIndex >= path.Length - 1;
		}

		// Arrive and avoid out of range indexes.
		if (curPathIndex > path.Length - 1)
		{
			Debug.Log("DESTINATION REACHED!");
			curPathIndex = path.Length - 1;
		}

		NPC.Velocity += GetSteeringForce() * Time.deltaTime;

		// Locomotion
		var targetForward = toCurPathPos;

		NPC.transform.rotation = Quaternion.Lerp(NPC.transform.rotation, Quaternion.LookRotation(targetForward), Time.deltaTime);
		NPC.AnimationController.SetBool("IsAim", true);
		NPC.AnimationController.SetFloat("Speed", NPC.Velocity.magnitude);

		NPC.AnimationController.SetFloat("HorizontalSpeed", Vector3.Dot(targetForward, NPC.transform.right));
		NPC.AnimationController.SetFloat("VerticalSpeed", Vector3.Dot(targetForward, NPC.transform.forward));

		lastTargetPosition = NPC.Target.position;
	}

	private Vector3 AtHeight(Vector3 postion, float height)
	{
		return new Vector3(postion.x, height, postion.z);
	}
}
