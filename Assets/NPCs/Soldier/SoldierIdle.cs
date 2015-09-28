using UnityEngine;

public class SoldierIdle : BaseState<Soldier>
{
	public SoldierIdle(Soldier npc) : base(npc)
	{
		Debug.Log("Idle");
		IntervalTime = 0.3f;
	}

	private void SeeTarget(Transform target)
	{
		NPC.State = new SoldierChase(NPC, target);
	}

	private void HearTarget(Transform target)
	{
		NPC.State = new SoldierSeek(NPC, target.position);
	}

	public override void IntervalUpdate()
	{
		NPC.SightSensor.Detect(SeeTarget);
		NPC.HearingSensor.Detect(HearTarget);
	}
}
