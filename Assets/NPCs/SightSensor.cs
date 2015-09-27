using System;
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
		Debug.Log("LOOK");
		var detectedObjects = Physics.OverlapSphere(transform.position, Distance, LayerMask.GetMask("Detectable"));
	    foreach (var detected in detectedObjects)
	    {
	        if (SeeTarget != null)
	        {
	            var toDetected = detected.transform.position - transform.position;
                var angleTo = Vector3.Angle(transform.forward, toDetected);
	            if (angleTo < FieldOfView)
	            {
                    var sightRay = new Ray(transform.position, toDetected);
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

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.white;
		Gizmos.DrawWireSphere(transform.position, Distance);
	}
}
