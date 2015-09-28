using UnityEngine;

public class Soldier : MonoBehaviour
{
	public BaseState<Soldier> State { get; set; }

	public Animator AnimationController;

	public SoldierSteering Steering { get; set; }

	public Vector3 Velocity { get; set; }

	public SightSensor SightSensor { get; set; }

	public HearingSensor HearingSensor;

	public float ShootAttackRadius = 3;

	public float MaxSpeed { get; set; }

	void Awake()
	{
		Steering = new SoldierSteering(this);
		SightSensor = GetComponent<SightSensor>();
		HearingSensor = GetComponent<HearingSensor>();
	}

	public bool IsDistanceGreaterThan(Vector3 position, float distance)
	{
		return (position - transform.position).sqrMagnitude > (distance * distance);
	}

	void Start()
	{
		//State = new SoldierIdle(this);
        State = new SoldierWander(this);
	}

	void Update()
	{
		State.Update();
	}
}
