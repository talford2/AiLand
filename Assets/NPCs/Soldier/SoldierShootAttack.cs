using UnityEngine;

public class SoldierShootAttack : BaseState<Soldier>
{
	public SoldierShootAttack(Soldier npc) : base(npc)
	{
		Debug.Log("Shoot");
	}
}
