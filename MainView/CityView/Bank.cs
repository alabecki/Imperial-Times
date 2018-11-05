using assemblyCsharp;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Bank : MonoBehaviour
{

    public GameObject building;
    public GameObject InvestmentlPanel;
    public Text currentIP;
    public Text IPLevel;
    public Button increaseIP;
    //Remember to have a UpdateEra script that will update all of the needs when new era begins





    // Use this for initialization
    void Start()
    {
     


    }

    private void OnMouseEnter()
    {
        string buildingName = name;

        Debug.Log("Moving Over: " + buildingName);
    
    }


    private void OnMouseExit()
    {
        string buildingName = name;

     

        Debug.Log("Name of building: " + buildingName);
   
    }

    private void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;

        string buildingName = name;
        Debug.Log("Clicked on: " + buildingName);
        if (InvestmentlPanel.activeSelf)
        {
            InvestmentlPanel.SetActive(false);
          //  Debug.Log("OFF");
        }
        else
        {
            updatePanel();
            InvestmentlPanel.SetActive(true);
            //Debug.Log("ON");
        }
    }

    private void updatePanel()
    {
        App app = UnityEngine.Object.FindObjectOfType<App>();
        Nation player = State.getNations()[app.GetHumanIndex()];
        currentIP.text = player.getIP().ToString();
        IPLevel.text = player.getInvestmentLevel().ToString();

        increaseIP.interactable = true;
        if(player.getNumberResource(MyEnum.Resources.spice) < 1  || player.getAP() < 1 ||
            player.getNumberGood(MyEnum.Goods.furniture) < 1) {
            increaseIP.interactable = false;
        }
        MyEnum.Era era = State.era;
        if(era == MyEnum.Era.Middle && player.getNumberGood(MyEnum.Goods.paper) < 1)
        {
            increaseIP.interactable = false;
        }
        if(era == MyEnum.Era.Late && (player.getNumberGood(MyEnum.Goods.telephone) < 1 ||
            player.getNumberGood(MyEnum.Goods.auto) < 1)){
            increaseIP.interactable = false;
        }


    }
}