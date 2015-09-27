using UnityEngine;

public class SoldierIdle : BaseState<Soldier>
{
	public SoldierIdle(Soldier npc) : base(npc)
	{
		Debug.Log("Idle");
	    NPC.hearingSensor.HearTarget += HearTarget;
	}

	public override void Update()
	{
		if (NPC.Target != null)
		{
			NPC.State = new SoldierSeek(NPC, NPC.Target.transform.position);
		}
		base.Update();
	}

    private void HearTarget(Transform target)
    {
        NPC.State = new SoldierSeek(NPC, target.position);
    }
}
