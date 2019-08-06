using assemblyCsharp;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UI.Tables;
using UnityEngine;
using UnityEngine.UI;

public class navyPanel : MonoBehaviour
{

   // public Button navalPanelButton;
    public GameObject NavyPanel;
    public TableLayout navyTable;
    public Button recruitFrigateButton;
    public Button recruitIroncladButton;
    public Button recruitBattleshipButton;

    public Text recruitingFrigatesText;
    public Text recruitingIroncladsText;
    public Text recruitingBattleshipsText;

    // public Text freePOP;

    public Button upgradeShipyardButton;
    public TextMeshProUGUI shipyardLevelValue;

    public GameObject shipyardUpgradeCostOne;
    public GameObject shipyardUpgradeCostTwo;

    public UI_Updater ui_updater;

    // Use this for initialization
    void Start()
    {
       // NavyPanel.SetActive(true);
       // navalPanelButton.onClick.AddListener(delegate { openNavalPanel(); });
        recruitFrigateButton.onClick.AddListener(delegate { RecruitFrigate(); });
        recruitIroncladButton.onClick.AddListener(delegate { RecruitIronclad(); });
        recruitBattleshipButton.onClick.AddListener(delegate { RecruitBattleship(); });
        upgradeShipyardButton.onClick.AddListener(delegate { upgradeShipyard(); });
    }

    // Update is called once per frame
    void Update()
    {

    }




    private void openNavalPanel()
    {
        Debug.Log("Should open Naval Panel");
        UpdateNavyTable();
        if (NavyPanel.activeSelf)
        {
            NavyPanel.SetActive(false);

        }
        else
        {
            UpdateNavyTable();
            NavyPanel.SetActive(true);
        }
    }

    private void upgradeShipyard()
    {
        App app = UnityEngine.Object.FindObjectOfType<App>();
        Nation player = State.getNations()[app.GetHumanIndex()];
        PlayerPayer.payShipyYardUpgrade(player);

        TextMeshProUGUI _shipyardLevel = shipyardLevelValue.GetComponent<TextMeshProUGUI>();
        _shipyardLevel.SetText("Shipyard Level: " + player.GetShipyardLevel().ToString());
        UpdateNavyTable();
        upgradeShipyardButton.interactable = false;

    }


    public void RecruitFrigate()
    {
        Debug.Log("Build Frigate!");
        App app = UnityEngine.Object.FindObjectOfType<App>();
        Nation player = State.getNations()[app.GetHumanIndex()];
        PlayerPayer.PayFrigate(player);
        recruitingFrigatesText.text = player.getNavyProducing(MyEnum.NavyUnits.frigates).ToString();
        UpdateNavyTable();
        upgradeShipyardButton.interactable = false;
        ui_updater.updateUI();
    }



    public void RecruitIronclad()
    {
        Debug.Log("Build Ironclad!");

        App app = UnityEngine.Object.FindObjectOfType<App>();
        Nation player = State.getNations()[app.GetHumanIndex()];
        PlayerPayer.PayIronClad(player);
        recruitingIroncladsText.text = player.getNavyProducing(MyEnum.NavyUnits.ironclad).ToString();
        UpdateNavyTable();
        upgradeShipyardButton.interactable = false;
        ui_updater.updateUI();

    }



    public void RecruitBattleship()
    {
        App app = UnityEngine.Object.FindObjectOfType<App>();
        Nation player = State.getNations()[app.GetHumanIndex()];
        PlayerPayer.PayDreadnought(player);
        recruitingBattleshipsText.text = player.getNavyProducing(MyEnum.NavyUnits.dreadnought).ToString();
        UpdateNavyTable();
        upgradeShipyardButton.interactable = false;
        ui_updater.updateUI();

    }




    private void UpdateNavyTable()
    {
        App app = UnityEngine.Object.FindObjectOfType<App>();
        Nation player = State.getNations()[app.GetHumanIndex()];
        SeaForces seaForces = player.seaForces;

        Text frigateAttack = navyTable.Rows[1].Cells[1].GetComponentInChildren<Text>();
        Debug.Log("frigate attack " + seaForces.frigate.Attack.ToString());
        frigateAttack.text = seaForces.frigate.Attack.ToString();
        Text ironcladAttack = navyTable.Rows[1].Cells[2].GetComponentInChildren<Text>();
        ironcladAttack.text = seaForces.ironclad.Attack.ToString();
        Text dreadnoughtAttack = navyTable.Rows[1].Cells[3].GetComponentInChildren<Text>();
        dreadnoughtAttack.text = seaForces.dreadnought.Attack.ToString();

        Text frigateMaxStrength = navyTable.Rows[2].Cells[1].GetComponentInChildren<Text>();
        frigateMaxStrength.text = seaForces.frigate.HitPoints.ToString();
        Text ironcladMaxStrength = navyTable.Rows[2].Cells[2].GetComponentInChildren<Text>();
        ironcladMaxStrength.text = seaForces.ironclad.HitPoints.ToString();
        Text dreadnoughtMaxStrength = navyTable.Rows[2].Cells[3].GetComponentInChildren<Text>();
        dreadnoughtMaxStrength.text = seaForces.dreadnought.HitPoints.ToString();

        Text frigateMan = navyTable.Rows[3].Cells[1].GetComponentInChildren<Text>();
        frigateMan.text = seaForces.frigate.Capacity.ToString();
        Text ironcladMan = navyTable.Rows[3].Cells[2].GetComponentInChildren<Text>();
        ironcladMan.text = seaForces.ironclad.Capacity.ToString();
        Text dreadnoughtMan = navyTable.Rows[3].Cells[3].GetComponentInChildren<Text>();
        dreadnoughtMan.text = seaForces.dreadnought.Capacity.ToString();

        Text frigateAmmo = navyTable.Rows[4].Cells[1].GetComponentInChildren<Text>();
        frigateAmmo.text = seaForces.frigate.Colonial.ToString();
        Text ironcladAmmo = navyTable.Rows[4].Cells[2].GetComponentInChildren<Text>();
        ironcladAmmo.text = seaForces.ironclad.Colonial.ToString();
        Text dreadnoughtAmmo = navyTable.Rows[4].Cells[3].GetComponentInChildren<Text>();
        dreadnoughtAmmo.text = seaForces.dreadnought.Colonial.ToString();

        Text NumFrig = navyTable.Rows[6].Cells[1].GetComponentInChildren<Text>();
        Text NumIC = navyTable.Rows[6].Cells[2].GetComponentInChildren<Text>();
        Text NumDN = navyTable.Rows[6].Cells[3].GetComponentInChildren<Text>();


        NumFrig.text = player.seaForces.frigate.getNumber().ToString();
        NumIC.text = player.seaForces.ironclad.getNumber().ToString();
        NumDN.text = player.seaForces.dreadnought.getNumber().ToString();

        Text recFrig = navyTable.Rows[7].Cells[1].GetComponentInChildren<Text>();
        Text recIC = navyTable.Rows[7].Cells[2].GetComponentInChildren<Text>();
        Text recDN = navyTable.Rows[7].Cells[3].GetComponentInChildren<Text>();

        recFrig.text = player.getNavyProducing(MyEnum.NavyUnits.frigates).ToString();
        recIC.text = player.getNavyProducing(MyEnum.NavyUnits.ironclad).ToString();
        recDN.text = player.getNavyProducing(MyEnum.NavyUnits.dreadnought).ToString();

        shipyardLevelValue.SetText(player.GetShipyardLevel().ToString());
        if (PlayerCalculator.canUpgradeShipyard(player) == true)
        {
            upgradeShipyardButton.interactable = true;
        }
        else
        {
            upgradeShipyardButton.interactable = false;
        }

        if (PlayerCalculator.canBuildFrigate(player) && State.GerEra() != MyEnum.Era.Late)
        {
            recruitFrigateButton.interactable = true;
        }
        else
        {
            recruitFrigateButton.interactable = false;
        }
        if (PlayerCalculator.canBuildIronclad(player))
        {
            recruitIroncladButton.interactable = true;
        }
        else
        {
            recruitIroncladButton.interactable = false;
        }

        if (PlayerCalculator.canBuildDreadnought(player))
        {
            recruitBattleshipButton.interactable = true;
        }
        else
        {
            recruitBattleshipButton.interactable = false;
        }

        if (player.GetShipyardLevel() == 1)
        {
            shipyardUpgradeCostOne.SetActive(true);
            shipyardUpgradeCostTwo.SetActive(false);
        }
        else if (player.GetShipyardLevel() == 2)
        {
            shipyardUpgradeCostOne.SetActive(false);
            shipyardUpgradeCostTwo.SetActive(true);
        }

        else
        {
            shipyardUpgradeCostOne.SetActive(false);
            shipyardUpgradeCostTwo.SetActive(false);
        }

    }

}
