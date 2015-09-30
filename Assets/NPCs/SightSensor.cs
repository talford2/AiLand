using System;
using UnityEngine;

public class SightSensor : MonoBehaviour
{
	public float Distance;
	public float FieldOfView;

	public Transform TransformObject;

	public void Detect(Action<Transform> action)
	{
		var candidates = Physics.OverlapSphere(transform.position, Distance, LayerMask.GetMask("Detectable"));
		foreach (var candidate in candidates)
		{
			var toDetected = candidate.transform.position - TransformObject.position;
			var angleTo = Vector3.Angle(TransformObject.forward, toDetected);
			if (angleTo < FieldOfView)
			{
				var sightRay = new Ray(TransformObject.position, toDetected);
				RaycastHit sightHit;
				if (Physics.Raycast(sightRay, out sightHit, Distance))
				{
					//Debug.Log(gameObject.name + " SEES " + sightHit.collider.name);

					if (sightHit.collider == candidate)
					{
						//Debug.Log("1:::" + gameObject.name + " SEES " + sightHit.collider.name);
						action(candidate.GetComponent<Detectable>().Target);
					}
					else
					{
						//Debug.Log("2:::" + gameObject.name + " SEES " + sightHit.collider.name);
						//Debug.Log("SEE else: " + sightHit.collider.name);
					}
				}
				else
				{

				}
			}
		}
	}

    private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere(TransformObject.position, Distance);

		var fovLeft = Quaternion.Euler(0, -FieldOfView * 0.5f, 0f);
		var fovRight = Quaternion.Euler(0, FieldOfView * 0.5f, 0f);
		Gizmos.DrawLine(TransformObject.position, TransformObject.position + fovLeft * TransformObject.forward * Distance);
		Gizmos.DrawLine(TransformObject.position, TransformObject.position + fovRight * TransformObject.forward * Distance);
	}
}
