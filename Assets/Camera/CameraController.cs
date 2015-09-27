using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Vector3 delta;
    private float closeness;

    private void Update()
    {
        delta = 10f*new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        closeness = 100f*Input.GetAxis("Mouse ScrollWheel");
    }

    private void LateUpdate()
    {
        transform.position += (delta + closeness*transform.forward)*Time.deltaTime;
    }
}
