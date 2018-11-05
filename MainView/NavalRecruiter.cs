using assemblyCsharp;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UI.Tables;
using UnityEngine;
using UnityEngine.UI;

public class NavalRecruiter : MonoBehaviour {

    public TableLayout navyTable;
    public Button recruitFrigateButton;
    public Button recruitIroncladButton;
    public Button recruitBattleshipButton;

    public Text recruitingFrigatesText;
    public Text recruitingIroncladsText;
    public Text recruitingBattleshipsText;

    // public Text freePOP;
    public Text AP;

    public Button upgradeShipyardButton;
    public GameObject shipyardLevelText;

    // Use this for initialization
    void Start()
    {
        recruitFrigateButton.onClick.AddListener(delegate { RecruitFrigate(); });
        recruitIroncladButton.onClick.AddListener(delegate { RecruitIronclad(); });
        recruitBattleshipButton.onClick.AddListener(delegate { RecruitBattleship(); });
        upgradeShipyardButton.onClick.AddListener(delegate { upgradeShipyard(); });

    }

    // Update is called once per frame
    void Update()
    {

    }


    private void upgradeShipyard()
    {
        App app = UnityEngine.Object.FindObjectOfType<App>();
        Nation player = State.getNations()[app.GetHumanIndex()];
        PlayerPayer.payShipyYardUpgrade(player);

        AP.text = player.getAP().ToString();
        TextMeshProUGUI _shipyardLevel = shipyardLevelText.GetComponent<TextMeshProUGUI>();
        _shipyardLevel.SetText("Shipyard Level: " + player.GetShipyardLevel().ToString());
        upgradeShipyardButton.interactable = false;
        UpdateNavalRectuitButtons(player);
    }


    public void RecruitFrigate()
    {
        App app = UnityEngine.Object.FindObjectOfType<App>();
        Nation player = State.getNations()[app.GetHumanIndex()];
        PlayerPayer.PayFrigate(player);
        recruitingFrigatesText.text = player.getNavyProducing(MyEnum.NavyUnits.frigates).ToString();
        AP.text = player.getAP().ToString();
        UpdateNavalRectuitButtons(player);
        upgradeShipyardButton.interactable = false;
    }



    public void RecruitIronclad()
    {
        App app = UnityEngine.Object.FindObjectOfType<App>();
        Nation player = State.getNations()[app.GetHumanIndex()];
        PlayerPayer.PayIronClad(player);
        recruitingIroncladsText.text = player.getNavyProducing(MyEnum.NavyUnits.ironclad).ToString();
        AP.text = player.getAP().ToString();
        UpdateNavalRectuitButtons(player);
        upgradeShipyardButton.interactable = false;
    }



    public void RecruitBattleship()
    {
        App app = UnityEngine.Object.FindObjectOfType<App>();
        Nation player = State.getNations()[app.GetHumanIndex()];
        PlayerPayer.PayDreadnought(player);
        recruitingBattleshipsText.text = player.getNavyProducing(MyEnum.NavyUnits.dreadnought).ToString();
        AP.text = player.getAP().ToString();
        UpdateNavalRectuitButtons(player);
        upgradeShipyardButton.interactable = false;
    }



    private void UpdateNavalRectuitButtons(Nation player)
    {
        Button recruitFrigateButton = navyTable.Rows[10].Cells[1].GetComponentInChildren<Button>();
        if (player.getUrbanPOP() < 1 || player.getNumberGood(MyEnum.Goods.arms) < 1 ||
            player.getNumberGood(MyEnum.Goods.lumber) < 1 || player.getNumberGood(MyEnum.Goods.fabric) < 1 ||
            player.GetShipyardLevel() < 1)

        {
            recruitFrigateButton.interactable = false;
        }
        else
        {
            recruitFrigateButton.interactable = true;
        }

        Button recruitIroncladButton = navyTable.Rows[10].Cells[2].GetComponentInChildren<Button>();
        if (player.getUrbanPOP() < 1 || player.getNumberGood(MyEnum.Goods.arms) < 1 ||
            player.getNumberGood(MyEnum.Goods.steel) < 1 || player.getNumberGood(MyEnum.Goods.parts) < 1 ||
            player.GetShipyardLevel() < 2)
        {
            recruitIroncladButton.interactable = false;
        }
        else
        {
            recruitIroncladButton.interactable = true;
        }

        Button recruitDreadnoughtButton = navyTable.Rows[10].Cells[3].GetComponentInChildren<Button>();
        if (player.getUrbanPOP() < 1 || player.getNumberGood(MyEnum.Goods.arms) < 3 ||
            player.getNumberGood(MyEnum.Goods.steel) < 3 || player.getNumberGood(MyEnum.Goods.parts) < 1 ||
            player.getNumberGood(MyEnum.Goods.gear) < 1 || player.GetShipyardLevel() < 3)
        {
            recruitDreadnoughtButton.interactable = false;
        }
        else
        {
            recruitDreadnoughtButton.interactable = true;
        }


    }


}
