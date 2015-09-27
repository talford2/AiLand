using UnityEngine;

public class SoldierWander : BaseState<Soldier>
{
	public SoldierWander(Soldier npc) : base(npc)
	{
		Debug.Log("Wander");
	}
}
