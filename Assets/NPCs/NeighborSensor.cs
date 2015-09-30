using System;
using UnityEngine;

public class NeighborSensor : MonoBehaviour {
    public float Distance;

    public void Detect(Action<Transform> action)
    {
        var detectedObjects = Physics.OverlapSphere(transform.position, Distance, LayerMask.GetMask("Detectable"));
        foreach (var detected in detectedObjects)
        {
            //Debug.Log("NEIGHBOR: " + detected.GetComponent<Detectable>().name);
            action(detected.GetComponent<Detectable>().Target);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, Distance);
    }
}
