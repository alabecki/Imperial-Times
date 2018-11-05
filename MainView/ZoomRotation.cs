using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WorldMapStrategyKit;

public class ZoomRotation : MonoBehaviour {

    WMSK map;

    public GameObject viewPort;
    bool zoomedIn;

    public float zoomThreshold;    //0,35f for main camera


    // Use this for initialization
    void Start () {
        map = WMSK.instance;
        zoomedIn = false;

    }

    // Update is called once per frame
    void Update () {
        float zoomLevel = map.GetZoomLevel();
        if(zoomLevel < zoomThreshold && zoomedIn == false)    
        {
            viewPort.transform.rotation = Quaternion.Euler(120f, 0f, 0f);
            GetComponent<Transform>().position = new Vector3(0, 40, -25);
            zoomedIn = true;
            Debug.Log("Current Zoom: " + zoomLevel);

        }
        if (zoomLevel >= zoomThreshold && zoomedIn == true)
        {
            viewPort.transform.rotation = Quaternion.Euler(89.98f, 0f, 0f);
            GetComponent<Transform>().position = new Vector3(0, 40, -25);
            zoomedIn = false;

            Debug.Log("Current Zoom: " + zoomLevel);

        }



    }
}
