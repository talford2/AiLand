using UnityEngine;
using System.Collections;

public class SoldierWonder : BaseState<Soldier>
{
	public SoldierWonder(Soldier npc) : base(npc)
	{
		Debug.Log("Wonder");
	}
}
