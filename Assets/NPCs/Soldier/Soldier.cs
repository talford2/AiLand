using UnityEngine;

public class Soldier : MonoBehaviour
{
	public BaseState<Soldier> State { get; set; }

	public Animator AnimationController;

    public Vector3 Velocity { get; set; }

    public float MaxSpeed { get; set; }

	void Start()
	{
		State = new SoldierIdle(this);
	}

	void Update()
	{
		State.Update();
	}
}
