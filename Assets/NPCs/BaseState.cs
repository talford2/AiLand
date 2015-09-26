using UnityEngine;
using System.Collections;

public abstract class BaseState<T>
{
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

	public virtual void Update() { }
}