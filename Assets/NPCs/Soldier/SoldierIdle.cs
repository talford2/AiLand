using UnityEngine;

public class SoldierIdle : BaseState<Soldier>
{
    private float seeInterval;
    private float seeCooldown;

    private float hearInterval;
    private float hearCooldown;

	public SoldierIdle(Soldier npc) : base(npc)
	{
		Debug.Log("Idle");
	    seeInterval = 0.3f;
	    hearInterval = 0.3f;
	}

    public void SeeTarget(Transform target)
    {
        NPC.State = new SoldierChase(NPC, target);
    }

    public void HearTarget(Transform target)
    {
        NPC.State = new SoldierSeek(NPC, target.position);
    }

    private void CheckSensors()
    {
        seeCooldown -= Time.deltaTime;
        if (seeCooldown < 0f)
        {
            NPC.SightSensor.Detect(SeeTarget);
            seeCooldown = seeInterval;
        }
        hearCooldown -= Time.deltaTime;
        if (hearCooldown < 0f)
        {
            NPC.HearingSensor.Detect(HearTarget);
            hearCooldown = hearInterval;
        }
    }

    public override void Update()
    {
        base.Update();

        CheckSensors();
    }
}
