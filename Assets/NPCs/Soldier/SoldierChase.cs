using UnityEngine;

public class SoldierChase : BaseState<Soldier>
{
	public SoldierChase(Soldier npc) : base(npc)
	{
		Debug.Log("Chase");
	}
}
