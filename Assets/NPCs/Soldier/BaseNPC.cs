using UnityEngine;
using System.Collections;

public abstract class BaseNPC : MonoBehaviour
{
	void Start() { }

	void Update()
	{
		GetState().FrameUpdate();
		NPCUpdate();
	}

	public virtual void NPCUpdate() { }

	public abstract IState GetState();
}
