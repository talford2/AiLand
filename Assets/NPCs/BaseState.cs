using UnityEngine;

public abstract class BaseState<T> : IState
{
	public float IntervalTime = 0.2f;
	private float intervalCooldown = 0;

	private T npc;

	public T NPC
	{
		get
		{
			return npc;
		}
	}

	public BaseState(T npc)
	{
		this.npc = npc;
	}

	public virtual void UpdateState() { }

	public virtual void FrameUpdate()
	{
		UpdateState();

		intervalCooldown -= Time.deltaTime;
		if (intervalCooldown < 0)
		{
			IntervalUpdate();
			intervalCooldown = IntervalTime;
		}
	}

	public virtual void IntervalUpdate() { }
}

public interface IState
{
	void FrameUpdate();
}