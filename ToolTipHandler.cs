using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolTipHandler : MonoBehaviour {

    public GameObject resourceTool;
    public GameObject railTool;
    public GameObject shipyardTool;
    public GameObject fortTool;

    public GameObject OresourceTool;
    public GameObject OrailTool;
    public GameObject OshipyardTool;
    public GameObject OfortTool;
    public GameObject OflagTool;
    public Text OName;

    public GameObject APToolTip;
    public GameObject colonialToolTip;
    public GameObject diplomacyToolTip;
    public GameObject infulenceToolTip;
    public GameObject reputationToolTip;
    public GameObject researchToopTip;
    public GameObject populationToolTip;
    public GameObject freePOPToolTip;
    public GameObject VPToolTip;
    public GameObject culturePointsToolTip;
    public GameObject stabilityToolTip;


    /*
     0 resourceTool, 1 railTool, 2 shipyardTool, 3 fortTool,
        4 OresourceTool, 5 OrailTool, 6 OshipyardTool, 7 OfortTool, 8 OflagTool, 
        9 APToolTip, 10 colonialToolTip, 11 infulenceToolTip, 12 researchToopTip,
        13 populationToolTip, 14 freePOPToolTip, 15 culturePointsToolTip, 16 stabilityToolTip,
         17 diplomacyToolTip, 18 reputationToolTip, 19 VPToolTip
         */


    private void Start()
    {
       /* GameObject[] toolTypes = {
        resourceTool, railTool, shipyardTool, fortTool,
         OresourceTool, OrailTool, OshipyardTool, OfortTool, OflagTool, 
        APToolTip, colonialToolTip, infulenceToolTip, researchToopTip,
        populationToolTip, freePOPToolTip, culturePointsToolTip, stabilityToolTip,
         diplomacyToolTip, reputationToolTip, VPToolTip  };
        for (int i = 0; i < toolTypes.Length; i++)
        {
            toolTypes[i].SetActive(false);

        } */
    }





    public void ShowToolTip(int type)
    {
        
         GameObject[] toolTypes = {
        resourceTool, railTool, shipyardTool, fortTool,
                 OresourceTool, OrailTool, OshipyardTool, OfortTool, OflagTool,

        APToolTip, colonialToolTip, infulenceToolTip, researchToopTip,
        populationToolTip, freePOPToolTip, culturePointsToolTip, stabilityToolTip,
         diplomacyToolTip, reputationToolTip, VPToolTip  };


        if(toolTypes[type] == OflagTool)
        {
            OflagTool.GetComponent<Text>().text = OName.text;
        }
        toolTypes[type].SetActive(true);
    }


    public void HideToolTip(int type)
    {
        GameObject[] toolTypes = {
        resourceTool, railTool, shipyardTool, fortTool,
                 OresourceTool, OrailTool, OshipyardTool, OfortTool, OflagTool,

        APToolTip, colonialToolTip, infulenceToolTip, researchToopTip,
        populationToolTip, freePOPToolTip, culturePointsToolTip, stabilityToolTip,
         diplomacyToolTip, reputationToolTip, VPToolTip };
        toolTypes[type].SetActive(false);
    }

    public void HideAllToolTips()
    {
        GameObject[] toolTypes = {
        resourceTool, railTool, shipyardTool, fortTool,
        APToolTip, colonialToolTip, infulenceToolTip, researchToopTip,
        populationToolTip, freePOPToolTip, culturePointsToolTip, stabilityToolTip,
         diplomacyToolTip, reputationToolTip, VPToolTip,  };
        for (int i = 0; i < toolTypes.Length; i++)
        {
            toolTypes[i].SetActive(false);

        }
    }
}


