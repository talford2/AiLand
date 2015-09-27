using UnityEngine;

[RequireComponent(typeof(Camera))]
public class TargetController : MonoBehaviour
{
    private Camera attachedCamera;
    public Transform TargetObject;

    private SphereCollider targetCollider;
    private bool isActive;
    private string label;
    private GUIStyle style;
    private Vector2 screenPosition;

    private void Awake()
    {
        attachedCamera = GetComponent<Camera>();
        targetCollider = TargetObject.GetComponentInChildren<SphereCollider>();
        isActive = targetCollider.enabled;
        label = isActive ? "ON" : "OFF";
        style = new GUIStyle
        {
            normal = {textColor = Color.black},
            alignment = TextAnchor.MiddleCenter
        };
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
        if (Input.GetButtonUp("Jump"))
        {
            isActive = !isActive;
            targetCollider.enabled = isActive;
            label = isActive ? "ON" : "OFF";
        }
    }

    private void LateUpdate()
    {
        screenPosition = attachedCamera.WorldToScreenPoint(TargetObject.position);
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(Screen.width/2f - 200f, 10f, 400f, 30), "Hit Spacebar to toggle target on and off.", style);
        GUI.Label(new Rect(screenPosition.x - 30f, Screen.height - screenPosition.y - 50f, 60f, 30f), label, style);
    }
}
