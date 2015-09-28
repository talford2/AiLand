using UnityEngine;

public class Soldier : BaseNPC
{
	public BaseState<Soldier> State { get; set; }

	public Animator AnimationController;

	public SoldierSteering Steering { get; set; }

	public Vector3 Velocity { get; set; }

	public SightSensor SightSensor { get; set; }

	public HearingSensor HearingSensor;

	public float ShootAttackRadius = 3;

	public float Speed { get; set; }

    public float TargetSpeed { get; set; }

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

	public override IState GetState()
	{
		return State;
	}

    public override void NPCUpdate()
    {
        base.NPCUpdate();
        Speed = Mathf.Lerp(Speed, TargetSpeed, Time.deltaTime);
    }
}
