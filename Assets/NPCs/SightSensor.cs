﻿using System;
using System.Collections;
using UnityEngine;

public class SightSensor : MonoBehaviour
{
	public float Distance;
	public float FieldOfView;
	public float DetectFrequency;

	public event Action<Transform> SeeTarget;

	private void Start()
	{
		StartCoroutine(Detect(1f));
	}

	private IEnumerator Detect(float delay)
	{
		yield return new WaitForSeconds(delay);
		Debug.Log("CHECK");
		var detectedObjects = Physics.OverlapSphere(transform.position, Distance, LayerMask.GetMask("Detectable"));
		foreach (var detected in detectedObjects)
		{
			Debug.Log("DETECTED: " + detected.name);

			if (SeeTarget != null)
			{
				SeeTarget(detected.transform);
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