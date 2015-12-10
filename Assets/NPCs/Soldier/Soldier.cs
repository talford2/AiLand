using UnityEngine;

public class Soldier : BaseNPC
{
	public BaseState<Soldier> State { get; set; }

	public Animator AnimationController;

	public SoldierSteering Steering { get; set; }

	public Vector3 Velocity { get; set; }

    public NeighborSensor NeighborSensor { get; set; }

	public SightSensor SightSensor { get; set; }

	public HearingSensor HearingSensor { get; set; }

	public float ShootAttackRadius = 3;

	public float Speed { get; set; }

	public float TargetSpeed { get; set; }

	public GameObject Corpse;
    public GameObject DieSpawnPrefab;

	void Awake()
	{
		Steering = new SoldierSteering(this);
	    NeighborSensor = GetComponent<NeighborSensor>();
		SightSensor = GetComponent<SightSensor>();
		HearingSensor = GetComponent<HearingSensor>();
	    stateStyle = new GUIStyle
	    {
	        normal = {textColor = Color.yellow},
            alignment = TextAnchor.MiddleCenter
	    };
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
		Speed = Mathf.Lerp(Speed, TargetSpeed, 2f * Time.deltaTime);
	    screenPosition = Camera.main.WorldToScreenPoint(transform.position);
	}

    public void Die()
    {
        Debug.Log(name + " die");
        if (Corpse != null)
        {
            var corpse = Instantiate(Corpse);
            corpse.transform.position = transform.position;
            corpse.GetComponentInChildren<SkinnedMeshRenderer>().material = GetComponentInChildren<SkinnedMeshRenderer>().material;
            var liveParts = transform.FindChild("Ground").GetComponentsInChildren<Transform>();
            var deadParts = corpse.transform.FindChild("Ground").GetComponentsInChildren<Transform>();
            foreach (var livePart in liveParts)
            {
                foreach (var deadPart in deadParts)
                {
                    if (livePart.name == deadPart.name)
                    {
                        deadPart.transform.localPosition = livePart.transform.localPosition;
                        deadPart.transform.localRotation = livePart.transform.localRotation;
                    }
                }
            }
        }
        SpawnerManager.TriggerRandom(DieSpawnPrefab);
        Destroy(gameObject);
    }

    private Vector2 screenPosition;
    private GUIStyle stateStyle;

    private void OnGUI()
    {
        GUI.Label(new Rect(screenPosition.x - 50f, Screen.height - screenPosition.y - 50f, 100f, 30f), State.Name, stateStyle);
    }
}
