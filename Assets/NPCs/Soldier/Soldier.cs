using UnityEngine;

public class Soldier : MonoBehaviour
{
	public BaseState<Soldier> State { get; set; }

	public Animator AnimationController;

	public SoldierSteering Steering { get; set; }

	public Vector3 Velocity { get; set; }

	public SightSensor sightSensor { get; set; }

    public HearingSensor hearingSensor { get; set; }

	public Transform Target;

	public float MaxSpeed { get; set; }

	void Awake()
	{
		Steering = new SoldierSteering(this);

		sightSensor = GetComponent<SightSensor>();
		sightSensor.SeeTarget += SeeTarget;

	    hearingSensor = GetComponent<HearingSensor>();
	    hearingSensor.HearTarget += HearTarget;
	}

	private void SeeTarget(Transform target)
	{
		Debug.Log("I saw something, I'm going to chase it");
		Target = target;
		State = new SoldierChase(this);
	}

    private void HearTarget(Transform target)
    {
        Debug.Log("I heard something.");
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
