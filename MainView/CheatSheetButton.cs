using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using assemblyCsharp;
using System;

public class CheatSheetButton : MonoBehaviour
{
    int nationIndex;

    public Text nationName;

    public Text AP;
    public Text PP;
    public Text IP;
    public Text railroads;
    public Text trains;
    public Text units;
    public Text numberProvDevelopments;
    public Text shipyard;
    public Text fort;
    public Text warehouse;
    public Text colonialPoints;
    public Text infulencePoints;
    
    public Text wheat;
    public Text meat;
    public Text fruit;
    public Text iron;
    public Text cotton;
    public Text wood;
    public Text coal;
    public Text spice;
    public Text dyes;
    public Text rubber;
    public Text oil;

    public Text steel;
    public Text lumber;
    public Text fabric;
    public Text parts;
    public Text arms;
    public Text clothing;
    public Text furniture;
    public Text paper;
    public Text chemicals;
    public Text gear;
    public Text telephone;
    public Text auto;

    public Text ap_adds;
    public Text pp_adds;
    public Text ip_adds;
    public Text rp_adds;
    public Text turns;

    public Text savings;
    public Text debt;

    public Button cheatSheetButton;

    //public Button SelectionButton;

    public GameObject cheatSheetPanel;

    // Start is called before the first frame update
    void Start()
    {
        cheatSheetButton.onClick.AddListener(delegate {showCheatSheet();});
        cheatSheetPanel.SetActive(false);
    }

    private void showCheatSheet()
    {
        cheatSheetPanel.SetActive(true);
       // int otherIndex = Int32.Parse(SelectionButton.transform.parent.parent.name);
       //State.setCurrentSelectedNationDiplomacy(otherIndex);
        Nation chosenNation = State.getNations()[State.getCurrentSlectedNationDiplomacy()];
        nationName.text = chosenNation.nationName.ToString();

        AP.text = chosenNation.getAP().ToString();
        PP.text = chosenNation.getDP().ToString();
        IP.text = chosenNation.getIP().ToString();
        railroads.text = PlayerCalculator.getNumberProvRailRoads(chosenNation).ToString();
        trains.text = chosenNation.industry.getNumberOfTrains().ToString();
        units.text = chosenNation.landForces.Strength.ToString();
        numberProvDevelopments.text = PlayerCalculator.getNumberProvDevelopments(chosenNation).ToString();
        shipyard.text = chosenNation.GetShipyardLevel().ToString();
        fort.text = chosenNation.getFortLevel().ToString();
        warehouse.text = chosenNation.GetCurrentWarehouseCapacity().ToString(); 
        colonialPoints.text = chosenNation.ColonialPoints.ToString();
        infulencePoints.text = chosenNation.InfulencePoints.ToString();

        wheat.text = chosenNation.getNumberResource(MyEnum.Resources.wheat).ToString("0,0");
        meat.text = chosenNation.getNumberResource(MyEnum.Resources.meat).ToString("0,0");
        fruit.text = chosenNation.getNumberResource(MyEnum.Resources.fruit).ToString("0,0");
        iron.text = chosenNation.getNumberResource(MyEnum.Resources.iron).ToString("0,0");
        cotton.text = chosenNation.getNumberResource(MyEnum.Resources.cotton).ToString("0,0"); ;
        wood.text = chosenNation.getNumberResource(MyEnum.Resources.wood).ToString("0,0");
        coal.text = chosenNation.getNumberResource(MyEnum.Resources.coal).ToString("0,0");
        spice.text = chosenNation.getNumberResource(MyEnum.Resources.spice).ToString("0,0");
        dyes.text = chosenNation.getNumberResource(MyEnum.Resources.dyes).ToString("0,0");
        rubber.text = chosenNation.getNumberResource(MyEnum.Resources.rubber).ToString("0,0"); 
        oil.text = chosenNation.getNumberResource(MyEnum.Resources.oil).ToString("0,0"); 

        steel.text = chosenNation.getNumberGood(MyEnum.Goods.steel).ToString("0,0");
        lumber.text = chosenNation.getNumberGood(MyEnum.Goods.lumber).ToString("0,0");
        fabric.text = chosenNation.getNumberGood(MyEnum.Goods.fabric).ToString("0,0");
        parts.text = chosenNation.getNumberGood(MyEnum.Goods.parts).ToString("0,0");
        arms.text = chosenNation.getNumberGood(MyEnum.Goods.arms).ToString("0,0");
        clothing.text = chosenNation.getNumberGood(MyEnum.Goods.clothing).ToString("0,0");
        furniture.text = chosenNation.getNumberGood(MyEnum.Goods.furniture).ToString("0,0");
        paper.text = chosenNation.getNumberGood(MyEnum.Goods.paper).ToString("0,0");
        chemicals.text = chosenNation.getNumberGood(MyEnum.Goods.chemicals).ToString("0,0");
        telephone.text = chosenNation.getNumberGood(MyEnum.Goods.telephone).ToString("0,0");
        auto.text = chosenNation.getNumberGood(MyEnum.Goods.auto).ToString("0,0");
        gear.text = chosenNation.getNumberGood(MyEnum.Goods.gear).ToString("0.0");

        AdminAI admin = chosenNation.getAI().GetAdmin();
        ap_adds.text = admin.ApAdds.ToString();
        pp_adds.text = admin.PpAdds.ToString();
        ip_adds.text = admin.IpAdds.ToString();
        rp_adds.text = admin.IpAdds.ToString();
        turns.text = chosenNation.getAI().numberOfTurns.ToString();

        WorldBank bank = State.bank;
        int chosenNationIndex = chosenNation.getIndex();
        int bondSize = bank.BondSize;
        savings.text = (bank.getDeposits(chosenNation) * bondSize).ToString();
        debt.text = (bank.getDebt(chosenNation) * bondSize).ToString();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
