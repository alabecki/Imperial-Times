using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WorldMapStrategyKit;

public class CityRotationZoom : MonoBehaviour
{


    public GameObject viewPort;
    public Camera camera;
    bool zoomedIn;

    public float zoomThreshold;    //0,35f for main camera


    // Use this for initialization
    void Start()
    {
        zoomedIn = false;

    }

    // Update is called once per frame
    void Update()
    {
        //float zoomLevel = map.GetZoomLevel();
        float zoomLevel = camera.fieldOfView;
        Debug.Log("field of view: " + zoomLevel);
        if (zoomLevel < zoomThreshold && zoomedIn == false)
        {
            viewPort.transform.rotation = Quaternion.Euler(30f, 0f, 0f);
            GetComponent<Transform>().position = new Vector3(525, 2000, -300);
            zoomedIn = true;

        }
        if (zoomLevel >= zoomThreshold && zoomedIn == true)
        {
          viewPort.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            GetComponent<Transform>().position = new Vector3(525, 1810, -225);
            zoomedIn = false;

            Debug.Log("Current Zoom: " + zoomLevel);

        }



    }
}