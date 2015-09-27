using UnityEngine;

public class SoldierShootAttack : BaseState<Soldier>
{
	public float AttackInterval = 0.4f;
	private float attackCooldown = 0;

	public SoldierShootAttack(Soldier npc) : base(npc)
	{
		Debug.Log("Shoot");
		NPC.AnimationController.SetBool("IsAim", true);
		NPC.AnimationController.SetFloat("Speed", 0);
		NPC.AnimationController.SetFloat("VerticalSpeed", 0);
		NPC.AnimationController.SetFloat("HorizontalSpeed", 0);
	}

	public override void Update()
	{
		if (NPC.IsDistanceGreaterThan(NPC.ShootAttackRadius))
		{
			if (NPC.Target != null)
			{
				NPC.State = new SoldierChase(NPC, NPC.Target);
			}
			else
			{
				NPC.State = new SoldierIdle(NPC);
			}
		}
		base.Update();

		attackCooldown -= Time.deltaTime;
		if (attackCooldown <= 0)
		{
			NPC.AnimationController.SetTrigger("Shoot");
			attackCooldown = AttackInterval;
		}
	}
}
