using UnityEngine;

[RequireComponent(typeof(Camera))]
public class TargetController : MonoBehaviour
{
    private Camera attachedCamera;
    public Transform TargetObject;

    private void Awake()
    {
        attachedCamera = GetComponent<Camera>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            var mouseRay = attachedCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit mouseHit;
            if (Physics.Raycast(mouseRay, out mouseHit, 100f, LayerMask.GetMask("Ground")))
            {
                TargetObject.position = mouseHit.point;
            }
        }
    }
}
