using UnityEngine;

public class SoldierFloo : BaseState<Soldier>
{
	public SoldierFloo(Soldier npc) : base(npc)
	{
		Debug.Log("Flee");
	}
}
