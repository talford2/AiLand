using UnityEngine;

public class SoldierShootAttack : BaseState<Soldier>
{
	public SoldierShootAttack(Soldier npc) : base(npc)
	{
		Debug.Log("Shoot");
		NPC.AnimationController.SetBool("IsAim", true);
		NPC.AnimationController.SetFloat("Speed", 0);
		NPC.AnimationController.SetFloat("VerticalSpeed", 0);
		NPC.AnimationController.SetFloat("HorizontalSpeed", 0);
		NPC.AnimationController.SetTrigger("Shoot");
	}
}
