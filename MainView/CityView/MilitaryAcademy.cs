using assemblyCsharp;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MilitaryAcademy : MonoBehaviour
{

    public GameObject building;
    public GameObject ArmyPanel;
    public Text numberTacticCards;
    public Text armyLevel;
    public Button drawTacticCards;
    //Remember to have a UpdateEra script that will update all of the needs when new era begins

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
     


    }

    private void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;

        string buildingName = name;
        if (ArmyPanel.activeSelf)
        {
            ArmyPanel.SetActive(false);

        }
        else
        {
            updatePanel();
            ArmyPanel.SetActive(true);
        }
    }

    private void updatePanel()
    {
        App app = UnityEngine.Object.FindObjectOfType<App>();
        Nation player = State.getNations()[app.GetHumanIndex()];
        string numCards = player.getTacticCards().Count.ToString();
        numberTacticCards.text = numCards;
        armyLevel.text = player.getArmyLevel().ToString();

        drawTacticCards.interactable = true;
        MyEnum.Era era = State.era;
        if (player.getNumberGood(MyEnum.Goods.arms) < 1 || player.getAP() < 1 ||
            player.getNumberResource(MyEnum.Resources.spice) < 1)
        {
            drawTacticCards.interactable = false;
        }
        if (era != MyEnum.Era.Early)
        {
            if (player.getNumberGood(MyEnum.Goods.furniture) < 1)
            {
                drawTacticCards.interactable = false;
            }
        }
        if (era == MyEnum.Era.Late && player.getNumberGood(MyEnum.Goods.auto) < 1)
        {
            drawTacticCards.interactable = false;
        }

    }
}