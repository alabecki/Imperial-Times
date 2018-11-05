using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour
{

    private float speed = 2.0f;
    private float zoomSpeed = 2.0f;

    public static float[] BoundsX = new float[] { 300f, 800f };
    public static readonly float[] BoundsY = new float[] { 1600f, 1925f };
    public static readonly float[] BoundsZ = new float[] { -250, 100f };
    public static readonly float[] ZoomBounds = new float[] { -15f, 70f };

    public float minX = 300;
    public float maxX = 800;

    public float minY = 1600.0f;
    public float maxY = 1925.0f;

    public float sensX = 100.0f;
    public float sensY = 100.0f;

    float rotationY = 0.0f;
    float rotationX = 0.0f;

    void Update()
    {



        float scroll = Input.GetAxis("Mouse ScrollWheel");
        transform.Translate(0, scroll * zoomSpeed, scroll * zoomSpeed, Space.World);

        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.position += Vector3.right * speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.position += Vector3.left * speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.position += Vector3.forward * speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.position += Vector3.back * speed * Time.deltaTime;
        }

        if (Input.GetMouseButton(0))
        {
            rotationX += Input.GetAxis("Mouse X") * sensX * Time.deltaTime;
            rotationY += Input.GetAxis("Mouse Y") * sensY * Time.deltaTime;
            rotationY = Mathf.Clamp(rotationY, minY, maxY);
            transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
        }
    }
}
