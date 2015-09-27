using UnityEngine;

public class Soldier : MonoBehaviour
{
	public BaseState<Soldier> State { get; set; }

	public Animator AnimationController;

	public SoldierSteering Steering { get; set; }

	public Vector3 Velocity { get; set; }

	private SightSensor sightSensor;

	private HearingSensor hearingSensor;

	public Transform Target;

	public float ShootAttackRadius = 3;

	public float MaxSpeed { get; set; }

	void Awake()
	{
		Steering = new SoldierSteering(this);
		sightSensor = GetComponent<SightSensor>();
		hearingSensor = GetComponent<HearingSensor>();

		sightSensor.SeeTarget += SeeTarget;
		hearingSensor.HearTarget += HearTarget;
	}

	private void HearTarget(Transform target)
	{
		if (State != null)
		{
			State.HearTarget(target);
		}
	}

	private void SeeTarget(Transform target)
	{
		if (State != null)
		{
			State.SeeTarget(target);
		}
	}

	public bool IsDistanceGreaterThan(float distance)
	{
		return (Target.position - transform.position).sqrMagnitude > (distance * distance);
	}

	void Start()
	{
		State = new SoldierIdle(this);
	}

	void Update()
	{
		State.Update();
	}
}
