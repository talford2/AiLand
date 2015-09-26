using UnityEngine;
using System.Collections;

public class SoldierFloo : BaseState<Soldier>
{
	public SoldierFloo(Soldier npc) : base(npc)
	{
		Debug.Log("Flee");
	}
}
