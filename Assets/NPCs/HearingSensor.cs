using System;
using System.Collections;
using UnityEngine;

public class HearingSensor : MonoBehaviour
{
	public float Distance;
	public float DetectFrequency;

	public event Action<Transform> HearTarget;

	private void Start()
	{
		StartCoroutine(Detect(1f));
	}

	private IEnumerator Detect(float delay)
	{
		yield return new WaitForSeconds(delay);
		Debug.Log("LISTEN");
		var detectedObjects = Physics.OverlapSphere(transform.position, Distance, LayerMask.GetMask("Detectable"));
		foreach (var detected in detectedObjects)
		{
			Debug.Log("DETECTED: " + detected.name);
			if (HearTarget != null)
			{
				HearTarget(detected.transform);
			}
		}
		StartCoroutine(Detect(DetectFrequency));
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.white;
		Gizmos.DrawWireSphere(transform.position, Distance);
	}
}
