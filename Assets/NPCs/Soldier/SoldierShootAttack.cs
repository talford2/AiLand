﻿using UnityEngine;

public class SoldierShootAttack : BaseState<Soldier>
{
    private Transform attackTarget;
    public float AttackInterval = 0.4f;
    private float attackCooldown = 0;

    public SoldierShootAttack(Soldier npc, Transform target) : base(npc)
    {
        Debug.Log("Shoot");
        attackTarget = target;

        NPC.TargetSpeed = 0f;
    }

    private Vector3 GetSteeringForce()
    {
        var sqrMaxSpeed = NPC.Speed*NPC.Speed;
        var steerForce = Vector3.zero;

        steerForce += NPC.Steering.ArriveForce(attackTarget.position);
        if (steerForce.sqrMagnitude > sqrMaxSpeed)
            return NPC.Speed*steerForce.normalized;

        steerForce += NPC.Steering.SeekForce(attackTarget.position);
        if (steerForce.sqrMagnitude > sqrMaxSpeed)
            return NPC.Speed*steerForce.normalized;

        return steerForce;
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
        NPC.transform.rotation = Quaternion.Lerp(NPC.transform.rotation, Quaternion.LookRotation(targetForward), 5f*Time.deltaTime);

        NPC.Velocity += GetSteeringForce()*Time.deltaTime;

        // Locomotion
        NPC.AnimationController.SetBool("IsAim", true);
        NPC.AnimationController.SetFloat("Speed", NPC.Velocity.magnitude);
        NPC.AnimationController.SetFloat("HorizontalSpeed", Vector3.Dot(targetForward.normalized*NPC.Speed, NPC.transform.right));
        NPC.AnimationController.SetFloat("VerticalSpeed", Vector3.Dot(targetForward.normalized*NPC.Speed, NPC.transform.forward));

        attackCooldown -= Time.deltaTime;
        if (attackCooldown <= 0)
        {
            NPC.AnimationController.SetTrigger("Shoot");
            attackCooldown = AttackInterval;
        }
    }
}
