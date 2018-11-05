using assemblyCsharp;
using System.Collections;
using System.Collections.Generic;
using UI.Tables;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ArmyPanel : MonoBehaviour {


    public Button drawTacticCards;
    public Button recruitUnits;
    public Button viewTacticsHand;
    public Text numberTacticCards;
    public Text armyLevel;
    public GameObject ArmyRecuitPanel;
    public GameObject tacticCardPanel;
    public GameObject tacticCardBook;

    public TableLayout armyTable;

    public GameObject midCost;
    public GameObject lateCost;

    public Text AP;

    // Use this for initialization
    void Start () {

        midCost.SetActive(false);
        lateCost.SetActive(false);

        drawTacticCards.onClick.AddListener(delegate { getTacticCards(); });
        recruitUnits.onClick.AddListener(delegate { showRecruitUnitsPanel(); });
        viewTacticsHand.onClick.AddListener(delegate { showTacticsHandPanel(); });


    }
    // Update is called once per frame
    void Update () {
		
	}

    private void getTacticCards() {
        Debug.Log("press tactic cards");
        App app = UnityEngine.Object.FindObjectOfType<App>();
        Nation player = State.getNations()[app.GetHumanIndex()];
        //....................

        PlayerPayer.payTacticCards(player);
        PlayerReceiver.collectTacticCards(player);

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
        showTacticsHandPanel();
        AP.text = player.getAP().ToString();
    }

    private void showRecruitUnitsPanel()
    {

        App app = UnityEngine.Object.FindObjectOfType<App>();
        Nation player = State.getNations()[app.GetHumanIndex()];
        UpdateArmyTable();
        numberTacticCards.text = player.getTacticCards().Count.ToString()
            + "/" + player.getMaximumTacticHandSize().ToString();
        armyLevel.text = player.getArmyLevel().ToString();
        if (ArmyRecuitPanel.activeSelf == false)
        {
          //  Debug.Log("ON");
            ArmyRecuitPanel.SetActive(true);
            //Debug.Log("OFF_");

            return;
        }
        else if (ArmyRecuitPanel.activeSelf == true)
        {
            //Debug.Log("OFF");
            ArmyRecuitPanel.SetActive(false);
            //Debug.Log("OFF_");
            return;
        }


    }

    private void showTacticsHandPanel()
    {
        if(tacticCardPanel.activeSelf == false)
        {
            updateTacticCardBook();
            tacticCardPanel.SetActive(true);
        }
        else
        {
            tacticCardPanel.SetActive(false);
        }
    }


    public void UpdateArmyTable()
    {
        App app = UnityEngine.Object.FindObjectOfType<App>();
        Nation player = State.getNations()[app.GetHumanIndex()];
        MilitaryForm militaryForm = player.GetMilitaryForm();

        Text infantryAttack = armyTable.Rows[1].Cells[1].GetComponentInChildren<Text>();
        infantryAttack.text = militaryForm.infantry.AttackStrength.ToString();

        Text cavAttack = armyTable.Rows[1].Cells[2].GetComponentInChildren<Text>();
        cavAttack.text = militaryForm.cavalry.AttackStrength.ToString();

        Text artAttack = armyTable.Rows[1].Cells[3].GetComponentInChildren<Text>();
        artAttack.text = militaryForm.artillery.AttackStrength.ToString();

        Text fighterAttack = armyTable.Rows[1].Cells[4].GetComponentInChildren<Text>();
        fighterAttack.text = militaryForm.fighter.AttackStrength.ToString();

        Text tankAttack = armyTable.Rows[1].Cells[5].GetComponentInChildren<Text>();
        tankAttack.text = militaryForm.tank.AttackStrength.ToString();

        Text infantryDefend = armyTable.Rows[2].Cells[1].GetComponentInChildren<Text>();
        infantryDefend.text = militaryForm.infantry.DefenseStrength.ToString();

        Text cavDefend = armyTable.Rows[2].Cells[2].GetComponentInChildren<Text>();
        cavDefend.text = militaryForm.cavalry.DefenseStrength.ToString();

        Text artDefend = armyTable.Rows[2].Cells[3].GetComponentInChildren<Text>();
        artDefend.text = militaryForm.artillery.DefenseStrength.ToString();

        Text fighterDefend = armyTable.Rows[2].Cells[4].GetComponentInChildren<Text>();
        fighterDefend.text = militaryForm.fighter.DefenseStrength.ToString();

        Text tankDefend = armyTable.Rows[2].Cells[5].GetComponentInChildren<Text>();
        tankDefend.text = militaryForm.tank.DefenseStrength.ToString();



        Text InfantryShock = armyTable.Rows[3].Cells[1].GetComponentInChildren<Text>();
        InfantryShock.text = militaryForm.infantry.Shock.ToString();

        Text CavShock = armyTable.Rows[3].Cells[2].GetComponentInChildren<Text>();
        CavShock.text = militaryForm.cavalry.Shock.ToString();

        Text ArtShock = armyTable.Rows[3].Cells[3].GetComponentInChildren<Text>();
        ArtShock.text = militaryForm.artillery.Shock.ToString();

        Text FighterShock = armyTable.Rows[3].Cells[4].GetComponentInChildren<Text>();
        FighterShock.text = militaryForm.fighter.Shock.ToString();

        Text TankShock = armyTable.Rows[3].Cells[5].GetComponentInChildren<Text>();
        TankShock.text = militaryForm.tank.Shock.ToString();

        Text InfantryAmmo = armyTable.Rows[4].Cells[1].GetComponentInChildren<Text>();
        InfantryAmmo.text = militaryForm.infantry.AmmoUse.ToString();

        Text CavAmmo = armyTable.Rows[4].Cells[2].GetComponentInChildren<Text>();
        CavAmmo.text = militaryForm.cavalry.AmmoUse.ToString();

        Text ArtAmmo = armyTable.Rows[4].Cells[3].GetComponentInChildren<Text>();
        ArtAmmo.text = militaryForm.artillery.AmmoUse.ToString();

        Text FighterAmmo = armyTable.Rows[4].Cells[4].GetComponentInChildren<Text>();
        FighterAmmo.text = militaryForm.fighter.AmmoUse.ToString();

        Text TankAmmo = armyTable.Rows[4].Cells[5].GetComponentInChildren<Text>();
        TankAmmo.text = militaryForm.tank.AmmoUse.ToString();

        Text InfantryOil = armyTable.Rows[6].Cells[1].GetComponentInChildren<Text>();
        InfantryOil.text = militaryForm.infantry.OilUse.ToString();
      //  Debug.Log("Infantry Oiluse " + militaryForm.infantry.OilUse.ToString());

        Text CavOil = armyTable.Rows[5].Cells[2].GetComponentInChildren<Text>();
        CavOil.text = militaryForm.cavalry.OilUse.ToString();

        Text ArtOil = armyTable.Rows[5].Cells[3].GetComponentInChildren<Text>();
        CavOil.text = militaryForm.artillery.OilUse.ToString();

        Text FighterOil = armyTable.Rows[5].Cells[4].GetComponentInChildren<Text>();
        FighterOil.text = militaryForm.fighter.OilUse.ToString();

        Text TankOil = armyTable.Rows[5].Cells[5].GetComponentInChildren<Text>();
        TankOil.text = militaryForm.tank.OilUse.ToString();

        Text InfWeight = armyTable.Rows[5].Cells[1].GetComponentInChildren<Text>();
        InfantryOil.text = militaryForm.infantry.Weight.ToString();
       // Debug.Log("Infantry Weight: " + militaryForm.infantry.Weight.ToString());

        Text CavWeight = armyTable.Rows[6].Cells[2].GetComponentInChildren<Text>();
        CavWeight.text = militaryForm.cavalry.Weight.ToString();

        Text ArtWeight = armyTable.Rows[6].Cells[3].GetComponentInChildren<Text>();
        ArtWeight.text = militaryForm.artillery.Weight.ToString();

        Text FighterWeight = armyTable.Rows[6].Cells[4].GetComponentInChildren<Text>();
        FighterWeight.text = militaryForm.fighter.Weight.ToString();

        Text TankWeight = armyTable.Rows[6].Cells[5].GetComponentInChildren<Text>();
        TankWeight.text = militaryForm.tank.Weight.ToString();

        Text NumInf = armyTable.Rows[7].Cells[1].GetComponentInChildren<Text>();
        Text NumArt = armyTable.Rows[7].Cells[2].GetComponentInChildren<Text>();
        Text NumCav = armyTable.Rows[7].Cells[3].GetComponentInChildren<Text>();
        Text NumFight = armyTable.Rows[7].Cells[4].GetComponentInChildren<Text>();
        Text NumTank = armyTable.Rows[7].Cells[5].GetComponentInChildren<Text>();

        float infantryCount = 0;
        float artilleryCount = 0;
        float cavalryCount = 0;
        float fighterCount = 0;
        float tankCount = 0;
        for (int i = 0; i < player.GetArmies().Count; i++)
        {
            


            infantryCount += player.GetArmy(i).GetInfantry();
            artilleryCount += player.GetArmy(i).GetArtillery();
            cavalryCount += player.GetArmy(i).GetCavalry();
            fighterCount += player.GetArmy(i).GetFighter();
            tankCount += player.GetArmy(i).GetTank();
        }
        NumInf.text = infantryCount.ToString();
        NumArt.text = artilleryCount.ToString();
        NumCav.text = cavalryCount.ToString();
        NumFight.text = fighterCount.ToString();
        NumTank.text = tankCount.ToString();

        Text recInf = armyTable.Rows[8].Cells[1].GetComponentInChildren<Text>();
        Text recArt = armyTable.Rows[8].Cells[2].GetComponentInChildren<Text>();
        Text recCav = armyTable.Rows[8].Cells[3].GetComponentInChildren<Text>();
        Text recFight = armyTable.Rows[8].Cells[4].GetComponentInChildren<Text>();
        Text recTank = armyTable.Rows[8].Cells[5].GetComponentInChildren<Text>();

        recInf.text = player.getArmyProducing(MyEnum.ArmyUnits.infantry).ToString();
        recArt.text = player.getArmyProducing(MyEnum.ArmyUnits.artillery).ToString();
        recCav.text = player.getArmyProducing(MyEnum.ArmyUnits.cavalry).ToString();
        recFight.text = player.getArmyProducing(MyEnum.ArmyUnits.fighter).ToString();
        recTank.text = player.getArmyProducing(MyEnum.ArmyUnits.tank).ToString();

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

    private void updateTacticCardBook()
    {
        Transform hand = tacticCardBook.transform;
        App app = UnityEngine.Object.FindObjectOfType<App>();
        Nation player = State.getNations()[app.GetHumanIndex()];
        int numberCards = player.getTacticCards().Count;
        int loopLimit = 5 + numberCards;
        int j = 0;
        for (int i = 5; i < loopLimit; ++i)
        {
            Debug.Log(j.ToString());
            Image image = hand.GetChild(i).GetComponent<Image>();
            string name = player.getTacticCards()[j].type.ToString();
            Debug.Log("Name of next card " + name);
            placeCardOnTable(name, i);
            j += 1;

        }

    }

    private void placeCardOnTable(string cardName, int cardSlot)
    {
        Debug.Log(cardName);
        GameObject card = Resources.Load("TacticCards/" + cardName) as GameObject;
        GameObject myNewInstance = (GameObject)Instantiate(card);
        Transform hand = tacticCardBook.transform;
        myNewInstance.transform.SetParent(hand.transform.GetChild(cardSlot), false);
        Debug.Log("The Parent is: " + hand.transform.GetChild(cardSlot).name);
        myNewInstance.transform.localPosition = new Vector3(0, 30, 0);
        myNewInstance.transform.localScale = new Vector3(0.66f, 0.66f, 0.66f);
    }

}
