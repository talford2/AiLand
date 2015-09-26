using UnityEngine;
using System.Collections;

public class SoldierIdle : BaseState<Soldier>
{
	public SoldierIdle(Soldier npc) : base(npc)
	{
		Debug.Log("Idle");
	}
}
