using UnityEngine;

public class SoldierShootAttack : BaseState<Soldier>
{
    private Transform attackTarget;
    public float AttackInterval = 0.4f;
    private float attackCooldown = 0;

    public SoldierShootAttack(Soldier npc, Transform target) : base(npc)
    {
        Debug.Log("Shoot");
        attackTarget = target;
        NPC.AnimationController.SetBool("IsAim", true);
        NPC.AnimationController.SetFloat("Speed", 0);
        NPC.AnimationController.SetFloat("VerticalSpeed", 0);
        NPC.AnimationController.SetFloat("HorizontalSpeed", 0);
    }

    public override void UpdateState()
    {
        if (NPC.IsDistanceGreaterThan(attackTarget.position, NPC.ShootAttackRadius))
        {
            if (attackTarget != null)
            {
                NPC.State = new SoldierChase(NPC, attackTarget);
            }
            else
            {
                NPC.State = new SoldierIdle(NPC);
            }
        }

        var targetForward = Utility.AtHeight(attackTarget.position, 0f) - Utility.AtHeight(NPC.transform.position, 0f);
        NPC.transform.rotation = Quaternion.Lerp(NPC.transform.rotation, Quaternion.LookRotation(targetForward), 5f * Time.deltaTime);

        attackCooldown -= Time.deltaTime;
        if (attackCooldown <= 0)
        {
            NPC.AnimationController.SetTrigger("Shoot");
            attackCooldown = AttackInterval;
        }
    }
}
