using UnityEngine;
using System.Collections;

public class SoldierIdle : BaseState<Soldier>
{
	public SoldierIdle(Soldier npc) : base(npc)
	{
		Debug.Log("Idle");

	}

	public override void Update()
	{
		if (NPC.Target != null)
		{
			NPC.State = new SoldierChase(NPC);
		}
		base.Update();
	}
}
