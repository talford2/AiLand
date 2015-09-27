using System;
using System.Collections;
using UnityEngine;

public class SightSensor : MonoBehaviour
{
	public float Distance;
	public float FieldOfView;
	public float DetectFrequency;

	public event Action<Transform> SeeTarget;

	public Transform TransformObject;

	private void Start()
	{
		StartCoroutine(Detect(1f));
	}

	private IEnumerator Detect(float delay)
	{
		yield return new WaitForSeconds(delay);
		//Debug.Log("LOOK");
		var detectedObjects = Physics.OverlapSphere(transform.position, Distance, LayerMask.GetMask("Detectable"));
		foreach (var detected in detectedObjects)
		{
			if (SeeTarget != null)
			{
				var toDetected = detected.transform.position - TransformObject.position;
				var angleTo = Vector3.Angle(TransformObject.forward, toDetected);
				if (angleTo < FieldOfView)
				{
					var sightRay = new Ray(TransformObject.position, toDetected);
					RaycastHit sightHit;
					if (Physics.Raycast(sightRay, out sightHit, Distance))
					{
						Debug.Log("DETECTED: " + detected.name);
						SeeTarget(detected.transform);
					}
				}
			}
		}
		StartCoroutine(Detect(DetectFrequency));
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.white;
		Gizmos.DrawWireSphere(TransformObject.position, Distance);

        var fovLeft = Quaternion.Euler(0, -FieldOfView * 0.5f, 0f);
        var fovRight = Quaternion.Euler(0, FieldOfView * 0.5f, 0f);
        Gizmos.DrawLine(TransformObject.position, TransformObject.position + fovLeft * TransformObject.forward * Distance);
        Gizmos.DrawLine(TransformObject.position, TransformObject.position + fovRight * TransformObject.forward * Distance);
	}
}
