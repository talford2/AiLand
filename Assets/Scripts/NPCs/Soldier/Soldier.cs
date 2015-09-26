using UnityEngine;
using System.Collections;

public class Soldier : MonoBehaviour
{
	public BaseState<Soldier> State { get; set; }

	void Start()
	{
		State = new SoldierIdle(this);
	}

	void Update()
	{
		State.Update();
	}
}
