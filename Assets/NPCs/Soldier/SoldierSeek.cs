using UnityEngine;
using System.Collections;

public class SoldierSeek : BaseState<Soldier>
{
	public SoldierSeek(Soldier npc) : base(npc)
	{
		Debug.Log("Seek");
	}
}
