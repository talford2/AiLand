using UnityEngine;

public class SoldierIdle : BaseState<Soldier>
{
    private float closestSeenTargetDistanceSquared;
    private Transform closestSeenTarget;

    private float closestHeardTargetDistanceSquared;
    private Transform closestHeardTarget;

	public SoldierIdle(Soldier npc) : base(npc)
	{
		Debug.Log("Idle");
		IntervalTime = 0.3f;

	    NPC.TargetSpeed = 0f;
	}

    private void SeeTarget(Transform target)
    {
        if (target != NPC.transform)
        {
            var toTargetDistanceSquared = (target.position - NPC.transform.position).sqrMagnitude;
            if (toTargetDistanceSquared < closestSeenTargetDistanceSquared)
            {
                closestSeenTargetDistanceSquared = toTargetDistanceSquared;
                closestSeenTarget = target;
                //NPC.State = new SoldierChase(NPC, target);
            }
        }
    }

    private void HearTarget(Transform target)
    {
        if (target != NPC.transform)
        {
            var toTargetDistanceSquared = (target.position - NPC.transform.position).sqrMagnitude;
            if (toTargetDistanceSquared < closestHeardTargetDistanceSquared)
            {
                closestHeardTargetDistanceSquared = toTargetDistanceSquared;
                closestHeardTarget = target;
                //NPC.State = new SoldierSeek(NPC, target.position);
            }
        }
    }

    public override void IntervalUpdate()
    {
        closestSeenTargetDistanceSquared = Mathf.Infinity;
        closestSeenTarget = null;
        NPC.SightSensor.Detect(SeeTarget);

        closestHeardTargetDistanceSquared = Mathf.Infinity;
        closestHeardTarget = null;
        NPC.HearingSensor.Detect(HearTarget);

        HandleTarget();
    }

    private void HandleTarget()
    {
        if (closestSeenTarget != null)
        {
            NPC.State = new SoldierChase(NPC, closestSeenTarget);
        }
        else
        {
            if (closestHeardTarget != null)
                NPC.State = new SoldierSeek(NPC, closestHeardTarget.position);
        }
    }

    public override void UpdateState()
    {
        base.UpdateState();
        NPC.Speed = Mathf.Lerp(NPC.Speed, 0.0f, Time.deltaTime);

        // Locmotion
        NPC.AnimationController.SetBool("IsAim", false);
        NPC.AnimationController.SetFloat("Speed", NPC.Speed);

        NPC.AnimationController.SetFloat("HorizontalSpeed", NPC.Speed);
        NPC.AnimationController.SetFloat("VerticalSpeed", NPC.Speed);
    }
}
