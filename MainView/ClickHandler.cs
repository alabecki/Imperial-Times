using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WorldMapStrategyKit;

public class ClickHandler : MonoBehaviour
{


    WMSK map;
    public GameObjectAnimator army { get; set; }


    // Use this for initialization
    void Start()
    {

        map = WMSK.instance;


    }

    void MapClickHandler(float x, float y, int buttonIndex)
    {


    }

    // Update is called once per frame
    void Update()
    {
    }

}
