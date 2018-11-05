using UnityEngine;
using System.Collections;

public class zoomCamera : MonoBehaviour
{



    private static readonly float PanSpeed = 70f;
    private static readonly float ZoomSpeedTouch = 1.5f;
    private static readonly float ZoomSpeedMouse = 8.5f;

    public static float[] BoundsX = new float[] { 250, 650 };
    public static readonly float[] BoundsY = new float[] { 1600f, 1925f };
    public static readonly float[] BoundsZ = new float[] { -250, 100f };
    public static readonly float[] ZoomBounds = new float[] { -15f, 70f };

    public float speedH = 2.0f;
    public float speedV = 2.0f;

 

    public Camera cam;

    private Vector3 lastPanPosition;
    private int panFingerId; // Touch mode only

    private bool wasZoomingLastFrame; // Touch mode only
    private Vector2[] lastZoomPositions; // Touch mode only

    void Awake()
    {
        cam = GetComponent<Camera>();
    }

    void Update()
    {
        
            HandleMouse();
    }

    

    void HandleMouse()
    {
        if (cam.enabled)
        {
        
            // On mouse down, capture it's position.
            // Otherwise, if the mouse is still down, pan the camera.
          //  if (Input.GetMouseButtonDown(0))
        //    {
             //   lastPanPosition = Input.mousePosition;
        //    }
        //    else if (Input.GetMouseButton(0))
        //    {
              //  PanCamera(Input.mousePosition);
       //     }
            // Check for scrolling to zoom the camera
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            ZoomCamera(scroll, ZoomSpeedMouse);
        }
    }

    void PanCamera(Vector3 newPanPosition)
    {
        // Determine how much to move the camera
        Vector3 offset = cam.ScreenToViewportPoint(lastPanPosition - newPanPosition);
        Vector3 move = new Vector3(offset.x * PanSpeed, 0, offset.y * PanSpeed);

        // Perform the movement
        cam.transform.Translate(move, Space.World);

        // Ensure the camera remains within bounds.
        Vector3 pos = cam.transform.position;
        pos.x = Mathf.Clamp(cam.transform.position.x, BoundsX[0], BoundsX[1]);
        pos.z = Mathf.Clamp(cam.transform.position.z, BoundsZ[0], BoundsZ[1]);
        cam.transform.position = pos;

        // Cache the position
        lastPanPosition = newPanPosition;
    }

    void ZoomCamera(float offset, float speed)
    {
        if (offset == 0)
        {
            return;
        }

        cam.fieldOfView = Mathf.Clamp(cam.fieldOfView - (offset * speed), ZoomBounds[0], ZoomBounds[1]);
    }
}
