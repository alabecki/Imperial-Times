using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MoveCamera : MonoBehaviour {
    public  Camera cam;
    private Vector3 mouseOrigin;    // Position of cursor when mouse dragging starts
  
    public  float[] BoundsX = new float[] { 250, 650 };
    public  float[] BoundsY = new float[] { 35, 150 };
    public float[] BoundsZ = new float[] { 290, 325 };
    public float[] RotateXBound = new float[] { 3.5f, 60 };
    public float[] RotateYBound = new float[] { -30, 30 };


    public float ZoomSpeedMouse = 8.5f;
    public   float[] ZoomBounds = new float[] { -10f, 70f };


    float xAxisValue;
    float yAxisValue;
    float zAxisValue;

    public float speed = 3f;
    private float X;
    private float Y;
    // Use this for initialization
    void Start () {
 

    }
	
	// Update is called once per frame
	void Update()
    {

     
    
        if (cam.enabled)
        {
            xAxisValue = Input.GetAxis("Horizontal");
            yAxisValue = Input.GetAxis("Vertical");
            zAxisValue = Input.GetAxis("Vertical");
            if (Input.GetMouseButton(1))
            {
                cam.transform.Rotate(new Vector3(Input.GetAxis("Mouse Y") * speed, Input.GetAxis("Mouse X") * speed, 0));
                X = transform.rotation.eulerAngles.x;
             //   Y = transform.rotation.eulerAngles.y;
                cam.transform.rotation = Quaternion.Euler(X, 0, 0);
                Vector2 rpos = cam.transform.rotation.eulerAngles;
              //  rpos.x = Mathf.Clamp(cam.transform.rotation.eulerAngles.x, RotateXBound[0], RotateXBound[1]);
                // rpos.y = Mathf.Clamp(transform.rotation.eulerAngles.y, RotateXBound[0], RotateXBound[1]);
                if (cam.transform.rotation.eulerAngles.x < RotateXBound[0])
                {
                    rpos.x = RotateXBound[0];
                }
                if(cam.transform.rotation.eulerAngles.x > RotateXBound[1])
                {
                    rpos.x = RotateXBound[1];
                }
                cam.transform.rotation = Quaternion.Euler(rpos.x, 0, 0);
            }
            float scroll = Input.GetAxis("Mouse ScrollWheel");

            if (scroll != 0)
            {
                cam.fieldOfView = Mathf.Clamp(cam.fieldOfView -
                    (scroll * ZoomSpeedMouse), ZoomBounds[0], ZoomBounds[1]);
            }



            cam.transform.Translate(new Vector3(xAxisValue*10f, yAxisValue * 10f, zAxisValue*10.0f));
            Vector3 pos = cam.transform.position;

     


         //   Vector3 pos = cam.transform.position;
            pos.x = Mathf.Clamp(cam.transform.position.x, BoundsX[0], BoundsX[1]);
            pos.y = Mathf.Clamp(cam.transform.position.y, BoundsY[0], BoundsY[1]);
           pos.z = Mathf.Clamp(cam.transform.position.z, BoundsZ[0], BoundsZ[1]);
            cam.transform.position = pos;


        }

    }
}
