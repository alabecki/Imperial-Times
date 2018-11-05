using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using assemblyCsharp;
using UnityEngine.UI;
using UI.Tables;
using System;

public class ArmyUnitRecruiter : MonoBehaviour {

    public TableLayout armyTable;
    public Button recruitButton;
    public Text recruitingText;
    public Text freePOP;
    public Text AP;
    public int type;


    // Use this for initialization
    void Start()
    {
        recruitButton.onClick.AddListener(delegate { Recruit(); });
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void Recruit()
    {
        Debug.Log("Is Pressed");
        App app = UnityEngine.Object.FindObjectOfType<App>();
        Nation player = State.getNations()[app.GetHumanIndex()];
        if (type == 1)
        {
            PlayerPayer.PayInfantry(player);
            recruitingText.text = player.getArmyProducing(MyEnum.ArmyUnits.infantry).ToString();
         
        }
        if (type == 2)
        {
            PlayerPayer.PayCavalry(player);
            recruitingText.text = player.getArmyProducing(MyEnum.ArmyUnits.cavalry).ToString();
          
        
        }
        if (type == 3)
        {
            PlayerPayer.PayArtillery(player);
            recruitingText.text = player.getArmyProducing(MyEnum.ArmyUnits.artillery).ToString();
       

        }
        if (type == 4)
        {
            PlayerPayer.PayFighter(player);
            recruitingText.text = player.getArmyProducing(MyEnum.ArmyUnits.fighter).ToString();
              
            }
        if (type == 5)
        {
            PlayerPayer.PayTank(player);
            recruitingText.text = player.getArmyProducing(MyEnum.ArmyUnits.tank).ToString();
              
            }

        freePOP.text = player.getRuralPOP().ToString();
        AP.text = player.getAP().ToString();
        UpdateRectuitButtons(player);
       

    }

    private void UpdateRectuitButtons(Nation player)
    {
     
        Button recruitInfButton = armyTable.Rows[10].Cells[1].GetComponentInChildren<Button>();

        if (player.getUrbanPOP() < 1 || player.getNumberGood(MyEnum.Goods.arms) < 1)
        {
            recruitInfButton.interactable = false;
        }
        else
        {
            recruitInfButton.interactable = true;
        }

        Button recruitArtButton = armyTable.Rows[10].Cells[3].GetComponentInChildren<Button>();
        if (player.getUrbanPOP() < 1 || player.getNumberGood(MyEnum.Goods.arms) < 2)
        {
            recruitArtButton.interactable = false;
        }
        else
        {
            recruitArtButton.interactable = true;
        }

        Button recruitCavButton = armyTable.Rows[10].Cells[2].GetComponentInChildren<Button>();
        if (player.getUrbanPOP() < 1 || player.getNumberGood(MyEnum.Goods.arms) < 1 ||
            player.getNumberResource(MyEnum.Resources.wheat) < 1)
        {
            recruitCavButton.interactable = false;
        }
        else
        {
            recruitCavButton.interactable = true;
        }

        Button recruitFighterButton = armyTable.Rows[10].Cells[4].GetComponentInChildren<Button>();
        if (player.getUrbanPOP() < 1 || player.getNumberGood(MyEnum.Goods.fighter) < 1)
        {
            recruitFighterButton.interactable = false;
        }
        else
        {
            recruitFighterButton.interactable = true;
        }

        Button recruitTankButton = armyTable.Rows[10].Cells[5].GetComponentInChildren<Button>();
        if (player.getUrbanPOP() < 1 || player.getNumberGood(MyEnum.Goods.tank) < 1)
        {
            recruitTankButton.interactable = false;
        }
        else
        {
            recruitTankButton.interactable = true;
        }
    }


}
