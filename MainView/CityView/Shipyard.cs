using assemblyCsharp;
using System.Collections;
using System.Collections.Generic;
using UI.Tables;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class Shipyard : MonoBehaviour
{

    public GameObject building;
    public GameObject NavyPanel;
    public TableLayout navyTable;

    public GameObject shipyardLevel;
    public Button upgradeShipyard;
  
    //Remember to have a UpdateEra script that will update all of the needs when new era begins


    Color color;
    Color Ccolor;


    // Use this for initialization
    void Start()
    {


    }

    private void OnMouseEnter()
    {


    }


    private void OnMouseExit()
    {


    }

    private void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;

        string buildingName = name;
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




    private void UpdateNavyTable()
    {
        App app = UnityEngine.Object.FindObjectOfType<App>();
        Nation player = State.getNations()[app.GetHumanIndex()];
        MilitaryForm militaryForm = player.GetMilitaryForm();

        Text frigateAttack = navyTable.Rows[1].Cells[1].GetComponentInChildren<Text>();
        Debug.Log("frigate attack " + militaryForm.frigate.Attack.ToString());
        frigateAttack.text = militaryForm.frigate.Attack.ToString();
        Text ironcladAttack = navyTable.Rows[1].Cells[2].GetComponentInChildren<Text>();
        ironcladAttack.text = militaryForm.ironclad.Attack.ToString();
        Text dreadnoughtAttack = navyTable.Rows[1].Cells[3].GetComponentInChildren<Text>();
        dreadnoughtAttack.text = militaryForm.dreadnought.Attack.ToString();

        Text frigateMaxStrength = navyTable.Rows[2].Cells[1].GetComponentInChildren<Text>();
        frigateMaxStrength.text = militaryForm.frigate.GetStrength().ToString();
        Text ironcladMaxStrength = navyTable.Rows[2].Cells[2].GetComponentInChildren<Text>();
        ironcladMaxStrength.text = militaryForm.ironclad.GetStrength().ToString();
        Text dreadnoughtMaxStrength = navyTable.Rows[2].Cells[3].GetComponentInChildren<Text>();
        dreadnoughtMaxStrength.text = militaryForm.dreadnought.GetStrength().ToString();

        Text frigateMan = navyTable.Rows[3].Cells[1].GetComponentInChildren<Text>();
        frigateMan.text = militaryForm.frigate.Maneuver.ToString();
        Text ironcladMan = navyTable.Rows[3].Cells[2].GetComponentInChildren<Text>();
        ironcladMan.text = militaryForm.ironclad.Maneuver.ToString();
        Text dreadnoughtMan = navyTable.Rows[3].Cells[3].GetComponentInChildren<Text>();
        dreadnoughtMan.text = militaryForm.dreadnought.Maneuver.ToString();

        Text frigateAmmo = navyTable.Rows[4].Cells[1].GetComponentInChildren<Text>();
        frigateAmmo.text = militaryForm.frigate.AmmoUse.ToString();
        Text ironcladAmmo = navyTable.Rows[4].Cells[2].GetComponentInChildren<Text>();
        ironcladAmmo.text = militaryForm.ironclad.AmmoUse.ToString();
        Text dreadnoughtAmmo = navyTable.Rows[4].Cells[3].GetComponentInChildren<Text>();
        dreadnoughtAmmo.text = militaryForm.dreadnought.AmmoUse.ToString();

        Text frigateOil = navyTable.Rows[5].Cells[1].GetComponentInChildren<Text>();
        frigateOil.text = militaryForm.frigate.OilUse.ToString();
        Text ironcladOil = navyTable.Rows[5].Cells[2].GetComponentInChildren<Text>();
        ironcladOil.text = militaryForm.ironclad.OilUse.ToString();
        Text dreadnoughtOil = navyTable.Rows[5].Cells[3].GetComponentInChildren<Text>();
        dreadnoughtOil.text = militaryForm.dreadnought.OilUse.ToString();

        Text frigateCap = navyTable.Rows[6].Cells[1].GetComponentInChildren<Text>();
        frigateCap.text = militaryForm.frigate.Capacity.ToString();
        Text ironcladCap = navyTable.Rows[6].Cells[2].GetComponentInChildren<Text>();
        ironcladCap.text = militaryForm.ironclad.Capacity.ToString();
        Text dreadnoughtCap = navyTable.Rows[6].Cells[3].GetComponentInChildren<Text>();
        dreadnoughtCap.text = militaryForm.dreadnought.Capacity.ToString();

        Text frigateMov = navyTable.Rows[7].Cells[1].GetComponentInChildren<Text>();
        frigateMov.text = militaryForm.frigate.Movement.ToString();
        Text ironcladMov = navyTable.Rows[7].Cells[2].GetComponentInChildren<Text>();
        ironcladMov.text = militaryForm.ironclad.Movement.ToString();
        Text dreadnoughtMov = navyTable.Rows[7].Cells[3].GetComponentInChildren<Text>();
        dreadnoughtMov.text = militaryForm.dreadnought.Movement.ToString();

        Text NumFrig = navyTable.Rows[9].Cells[1].GetComponentInChildren<Text>();
        Text NumIC = navyTable.Rows[9].Cells[2].GetComponentInChildren<Text>();
        Text NumDN = navyTable.Rows[9].Cells[3].GetComponentInChildren<Text>();

        float frigCount = 0;
        float ICCount = 0;
        float DNCount = 0;

        for (int i = 0; i < player.GetFleets().Count; i++)
        {
            frigCount += player.GetFleet(i).GetFrigate();
            ICCount += player.GetFleet(i).GetIronClad();
            DNCount += player.GetFleet(i).GetDreadnought();

        }
        NumFrig.text = frigCount.ToString();
        NumIC.text = ICCount.ToString();
        NumDN.text = DNCount.ToString();

        Text recFrig = navyTable.Rows[10].Cells[1].GetComponentInChildren<Text>();
        Text recIC = navyTable.Rows[10].Cells[2].GetComponentInChildren<Text>();
        Text recDN = navyTable.Rows[10].Cells[3].GetComponentInChildren<Text>();

        recFrig.text = player.getNavyProducing(MyEnum.NavyUnits.frigates).ToString();
        recIC.text = player.getNavyProducing(MyEnum.NavyUnits.ironclad).ToString();
        recDN.text = player.getNavyProducing(MyEnum.NavyUnits.dreadnought).ToString();

        Button recruitFrigateButton = navyTable.Rows[11].Cells[1].GetComponentInChildren<Button>();
        if (PlayerCalculator.canBuildFrigate(player))
        {
            recruitFrigateButton.interactable = true;
        }
        else
        {
            recruitFrigateButton.interactable = false;
        }

            Button recruitIroncladButton = navyTable.Rows[11].Cells[2].GetComponentInChildren<Button>();
            if (PlayerCalculator.canBuildIronclad(player))
            {
                recruitIroncladButton.interactable = true;
            }
            else
            {
                recruitIroncladButton.interactable = false;
            }

            Button recruitDreadnoughtButton = navyTable.Rows[11].Cells[3].GetComponentInChildren<Button>();
            if (PlayerCalculator.canBuildDreadnought(player))
            {
                recruitDreadnoughtButton.interactable = true;
            }
            else
            {
                upgradeShipyard.interactable = false;
            }

            TextMeshProUGUI _shipyardLevel = shipyardLevel.GetComponent<TextMeshProUGUI>();
            _shipyardLevel.SetText("Shipyard Level: " + player.GetShipyardLevel().ToString());
            if (PlayerCalculator.canUpgradeShipyard(player) == true)
            {
                upgradeShipyard.interactable = true;
            }
            else
            {
                upgradeShipyard.interactable = false;
            }
        }
            


    }

 
