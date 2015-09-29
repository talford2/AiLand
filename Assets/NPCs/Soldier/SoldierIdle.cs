using UnityEngine;

public class SoldierIdle : BaseState<Soldier>
{
	public SoldierIdle(Soldier npc) : base(npc)
	{
		Debug.Log("Idle");
		IntervalTime = 0.3f;

	    NPC.TargetSpeed = 0f;
	}

    private void SeeTarget(Transform target)
    {
        if (target != NPC.transform)
            NPC.State = new SoldierChase(NPC, target);
    }

    private void HearTarget(Transform target)
    {
        if (target != NPC.transform)
            NPC.State = new SoldierSeek(NPC, target.position);
    }

    public override void IntervalUpdate()
	{
		NPC.SightSensor.Detect(SeeTarget);
		NPC.HearingSensor.Detect(HearTarget);
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
