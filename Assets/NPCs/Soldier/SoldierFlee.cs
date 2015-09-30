using UnityEngine;

public class SoldierFloo : BaseState<Soldier>
{
	public SoldierFloo(Soldier npc) : base(npc)
	{
	    Name = "Flee";
		Debug.Log("Flee");
	}
}
