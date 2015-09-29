using System;
using UnityEngine;

public class HearingSensor : MonoBehaviour
{
	public float Distance;

    public void Detect(Action<Transform> action)
    {
        var detectedObjects = Physics.OverlapSphere(transform.position, Distance, LayerMask.GetMask("Detectable"));
        foreach (var detected in detectedObjects)
        {
            Debug.Log("HEAR: " + detected.GetComponent<Detectable>().name);
            action(detected.GetComponent<Detectable>().Target);
        }
    }

	private void OnDrawGizmos()
	{
	    Gizmos.color = Color.green;
		Gizmos.DrawWireSphere(transform.position, Distance);
	}
}
