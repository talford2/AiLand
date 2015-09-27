using UnityEngine;

public class Soldier : MonoBehaviour
{
	public BaseState<Soldier> State { get; set; }

	public Animator AnimationController;

	public SoldierSteering Steering { get; set; }

	public Vector3 Velocity { get; set; }

	public SightSensor sightSensor { get; set; }

	public HearingSensor hearingSensor { get; set; }

	public float TargetExpirationTime = 2f;
	private float targetExpirationCooldown = 0;

	public Transform Target;

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

	void Start()
	{
		State = new SoldierIdle(this);
	}

	void Update()
	{
		targetExpirationCooldown -= Time.deltaTime;
		if (targetExpirationCooldown <= 0 && Target != null)
		{
			Debug.Log("I've lost the target");
			State = new SoldierSeek(this, Target.position);
			Target = null;
		}
		State.Update();
	}
}
