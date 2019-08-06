using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WorldMapStrategyKit;
using assemblyCsharp;
using AssemblyCsharp;
using System.Globalization;

public class ZoomRotation : MonoBehaviour
{

    WMSK map;
    public Camera cam;

    public GameObject viewPort;
    bool zoomedIn;

    //public float zoomThreshold;    //0,35f for main camera
    public float speed = 2f;
    private float X;
    private float Y;
    public float[] RotateXBound = new float[] { 10, 40 };
   // public float[] RotateYBound = new float[] { -40, 30 };
    public float[] PositionYBound = new float[] { 60, 90 };
    public float[] PositionZBound = new float[] { -120, -100 };
    public float ZoomSpeedMouse = 8f;
    public float baseXrotation = 89.98f;


    // Use this for initialization
    void Start()
    {
        map = WMSK.instance;
        zoomedIn = false;
    }

    // Update is called once per frame
    void Update()
    {

        if (cam.enabled)
        {

            if (Input.GetMouseButton(1))
            {
                cam.transform.Rotate(new Vector3(Input.GetAxis("Mouse Y") * speed, Input.GetAxis("Mouse X") * speed, 0));
                X = transform.rotation.eulerAngles.x;
                Quaternion target = cam.transform.rotation;
                target = Quaternion.Euler(X, 0, 0);
                cam.transform.rotation = Quaternion.Slerp(cam.transform.rotation, target, speed * Time.deltaTime);

                cam.transform.position =  new Vector3
                    (cam.transform.position.x, cam.transform.position.y + speed * Input.GetAxis("Mouse Y"), cam.transform.position.z + speed * Input.GetAxis("Mouse Y") * 1.333f);




                //   Y = transform.rotation.eulerAngles.y;
                //cam.transform.rotation = Quaternion.Euler(X, 0, 0);
                // -----------------------------------------------------------------------------------------
                Vector2 rpos = cam.transform.rotation.eulerAngles;
                //  rpos.x = Mathf.Clamp(cam.transform.rotation.eulerAngles.x, RotateXBound[0], RotateXBound[1]);
                // rpos.y = Mathf.Clamp(transform.rotation.eulerAngles.y, RotateXBound[0], RotateXBound[1]);
                if (cam.transform.rotation.eulerAngles.x < baseXrotation - RotateXBound[0])
                {
                    rpos.x = baseXrotation - RotateXBound[0];
                }
                if (cam.transform.rotation.eulerAngles.x > baseXrotation + RotateXBound[1])
                {
                    rpos.x = baseXrotation + RotateXBound[1];
                }
                cam.transform.rotation = Quaternion.Euler(rpos.x, 0, 0);

                if(cam.transform.position.y > PositionYBound[1])
                {
                    cam.transform.position = new Vector3(cam.transform.position.x, PositionYBound[1], cam.transform.position.z);
                }
                if(cam.transform.position.y < PositionYBound[0])
                {
                    cam.transform.position = new Vector3(cam.transform.position.x, PositionYBound[0], cam.transform.position.z);
                }
                if (cam.transform.position.z > PositionZBound[1])
                {
                    cam.transform.position = new Vector3(cam.transform.position.x, cam.transform.position.y, PositionZBound[1]);
                }
                if (cam.transform.position.z < PositionZBound[0])
                {
                    cam.transform.position = new Vector3(cam.transform.position.x, cam.transform.position.y, PositionZBound[0]);
                }

            }
            float scroll = Input.GetAxis("Mouse ScrollWheel");



            /*    float zoomLevel = map.GetZoomLevel();
            if(zoomLevel < zoomThreshold && zoomedIn == false)    
            {
                viewPort.transform.rotation = Quaternion.Euler(120f, 0f, 0f);
               // GetComponent<Transform>().position = new Vector3(0, 40, -25);
                zoomedIn = true;
                Debug.Log("Current Zoom: " + zoomLevel);

            }
            if (zoomLevel >= zoomThreshold && zoomedIn == true)
            {
                viewPort.transform.rotation = Quaternion.Euler(89.98f, 0f, 0f);
               // GetComponent<Transform>().position = new Vector3(0, 40, -25);
                zoomedIn = false;

                Debug.Log("Current Zoom: " + zoomLevel);

            } */



        }
    }
}
