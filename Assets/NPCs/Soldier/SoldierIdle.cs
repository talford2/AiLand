using UnityEngine;

public class SoldierIdle : BaseState<Soldier>
{
	public SoldierIdle(Soldier npc) : base(npc)
	{
		Debug.Log("Idle");
	}

    public override void HearTarget(Transform target)
    {
        NPC.State = new SoldierSeek(NPC, target.position);
    }
}
