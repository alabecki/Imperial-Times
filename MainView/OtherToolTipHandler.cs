using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherToolTipHandler : MonoBehaviour
{

    public GameObject resourceTool;
    public GameObject railTool;
    public GameObject shipyardTool;
    public GameObject fortTool;
    public GameObject flagTool;



    private void Start()
    {
        GameObject[] toolTypes = {
        resourceTool, railTool, shipyardTool, fortTool, flagTool,};
        for (int i = 0; i < toolTypes.Length; i++)
        {
            toolTypes[i].SetActive(false);
        }
    }







            public void ShowToolTip(int type)
    {

        GameObject[] toolTypes = {
        resourceTool, railTool, shipyardTool, fortTool, flagTool,
       };



        toolTypes[type].SetActive(true);
    }


    public void HideToolTip(int type)
    {
        GameObject[] toolTypes = {
        resourceTool, railTool, shipyardTool, fortTool, flagTool,
    };
        toolTypes[type].SetActive(false);
    }

    public void HideAllToolTips()
    {
        GameObject[] toolTypes = {
        resourceTool, railTool, shipyardTool, fortTool, flagTool,
    };
        for (int i = 0; i < toolTypes.Length; i++)
        {
            toolTypes[i].SetActive(false);

        }
    }
}
