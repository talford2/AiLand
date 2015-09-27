using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Vector3 delta;

    private void Update()
    {
        delta = 10f*new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
    }

    private void LateUpdate()
    {
        transform.position += delta*Time.deltaTime;
    }
}
