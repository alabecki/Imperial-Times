using assemblyCsharp;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GovernmentBuilding : MonoBehaviour
{

    //public GameObject building;
    Color color;
    Color Ccolor;
    public GameObject buildingInterface;
    public Text currentCorruption;
    public Text unemployed;
    public Button increasePOP;
    public Button decreaseCorruption;

    // Use this for initialization
    void Start()
    {

    }

    private void OnMouseEnter()
    {
        string buildingName = name;

      
    }


    private void OnMouseExit()
    {

        string buildingName = name;
        GetComponent<Renderer>().material.color = Color.yellow;


    }

    private void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;

        App app = UnityEngine.Object.FindObjectOfType<App>();
        Nation player = State.getNations()[app.GetHumanIndex()];
        string buildingName = name;
        if (buildingInterface.activeSelf)
        {
            buildingInterface.SetActive(false);
        }
        else
        {
            updatePanel(player);
            buildingInterface.SetActive(true);
        }
    }

    private void updatePanel(Nation player)
    {
        currentCorruption.text = player.GetCorruption().ToString();
        unemployed.text = player.getUnemployed().ToString();
        MyEnum.Era era = State.era;
        increasePOP.interactable = true;
        if (player.getNumberResource(MyEnum.Resources.wheat) < 1 ||
            player.getNumberGood(MyEnum.Goods.clothing) < 1 || player.getAP() < 1)
        {
            increasePOP.interactable = false;
        }
        else
        {
            increasePOP.interactable = true;
        }

        decreaseCorruption.interactable = false;
        if (player.getNumberResource(MyEnum.Resources.spice) < 1 || player.getAP() < 1 ||
            player.getNumberGood(MyEnum.Goods.paper) < 1)
        {
            decreaseCorruption.interactable = false;
        }
        else
        {
            decreaseCorruption.interactable = true;
        }

    }
}