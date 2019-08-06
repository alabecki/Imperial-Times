using assemblyCsharp;
using System.Collections;
using System.Collections.Generic;
using UI.Tables;
using UnityEngine;
using UnityEngine.UI;

public class DevelopmentPanel : MonoBehaviour
{
    public GameObject progressPanel;
    public UI_Updater uiUpdater;

    public Button addAPButton;
    public Button addDPButton;

    public Button encourageCapitalists;
    public Button fundResearch;
    public Button openResearchTree;

    public Button fundCulture;
    public Button viewCultureHand;
    public Button fightCorruptionButton;

    public GameObject selfProvince;
    public GameObject otherProvince;

   // public Button openMilitaryTabButton;

    //public Button drawTacticCards;
    //public Button viewTacticsHand;

   // public Button viewCurrentNavy;

    public GameObject TechnologyTree;
    public GameObject techTreeConnector;

    public GameObject chooseDoctrineTable;
    public GameObject armyTab;
    //public GameObject tacticCardPanel;
    //public GameObject tacticCardBook;

    public Text numberPattents;

    public GameObject cultureCardPanel;
    public GameObject cardArea;

    public Text numberOfMovements;
    public Text numberOfGenera;
    public Text numberOfEras;
    public Text techPoints;

    public GameObject DPEarlyCost;
    public GameObject DPLateCost;

    public GUI_Coordinator coordinator;

    /*public GameObject ResearchEarlyCost;
    public GameObject ResearchMiddleCost;
    public GameObject ResearchLateCost;

    public GameObject ArmyEarlyCost;
    public GameObject ArmyMiddelCost;
    public GameObject ArmyLateCost;

    public GameObject frigateCost;
    public GameObject ironcladCost;
    public GameObject dreadnoughtCost;

    public GameObject CultureEarlyCost;
    public GameObject CultureMiddleCost;
    public GameObject CultureLateCost; */

    // Start is called before the first frame update
    void Awake()
    {
        /* IPMiddleCost.SetActive(false);
         IPLateCost.SetActive(false);
         ResearchMiddleCost.SetActive(false);
         ResearchLateCost.SetActive(false);
         ArmyMiddelCost.SetActive(false);
         ArmyLateCost.SetActive(false);
         ironcladCost.SetActive(false);
         dreadnoughtCost.SetActive(false);
         CultureMiddleCost.SetActive(false);
         CultureLateCost.SetActive(false); */

        cultureCardPanel.SetActive(false);

        addAPButton.onClick.AddListener(delegate { addAP(); });
        addDPButton.onClick.AddListener(delegate { addDP(); });

        encourageCapitalists.onClick.AddListener(delegate { IncreaseIP(); });

        fundResearch.onClick.AddListener(delegate { addResearchPoints(); });
        openResearchTree.onClick.AddListener(delegate { OpenResearchTree(); });

        //improveArmyButton.onClick.AddListener(delegate { improveArmy(); });
       // openMilitaryTabButton.onClick.AddListener(delegate { openMilitaryTab(); });

       // buildNavyButton.onClick.AddListener(delegate { buildNavy(); });
       //viewCurrentNavy.onClick.AddListener(delegate { viewNavy(); });

        fundCulture.onClick.AddListener(delegate { getCultureCards(); });
        viewCultureHand.onClick.AddListener(delegate { showCultureHandPanel(); });

        fightCorruptionButton.onClick.AddListener(delegate { fightCorruption(); });

    }

    public void addAP()
    {
        App app = UnityEngine.Object.FindObjectOfType<App>();
        Nation player = State.getNations()[app.GetHumanIndex()];
        PlayerPayer.payForAP(player);
        PlayerReceiver.collectAP(player, 5);

        uiUpdater.updateUI();
    }

    public void addDP()
    {
        App app = UnityEngine.Object.FindObjectOfType<App>();
        Nation player = State.getNations()[app.GetHumanIndex()];
        PlayerPayer.payForDP(player);
        uiUpdater.updateUI();
    }

   /* private void openMilitaryTab()
    {
        App app = UnityEngine.Object.FindObjectOfType<App>();
        Nation player = State.getNations()[app.GetHumanIndex()];
        armyTab.SetActive(true);
        if(PlayerCalculator.canExpandArmy(player) == false)
        {
            expandArmyButton.interactable = false;
        }
        else
        {
            expandArmyButton.interactable = true;
        }

        Text size = armyTable.Rows[0].Cells[1].GetComponentInChildren<Text>();
        size.text = player.landForces.Strength.ToString();
        Text attack = armyTable.Rows[1].Cells[1].GetComponentInChildren<Text>();
        attack.text = player.landForces.Attack.ToString();
        Text defense = armyTable.Rows[2].Cells[1].GetComponentInChildren<Text>();
        defense.text = player.landForces.Defense.ToString();
        Text morale = armyTable.Rows[3].Cells[1].GetComponentInChildren<Text>();
        morale.text = player.landForces.Morale.ToString();
        Text shock = armyTable.Rows[4].Cells[1].GetComponentInChildren<Text>();
        shock.text = player.landForces.Shock.ToString();
        Text maneuver = armyTable.Rows[5].Cells[1].GetComponentInChildren<Text>();
        maneuver.text = player.landForces.Maneuver.ToString();
        Text recon = armyTable.Rows[6].Cells[1].GetComponentInChildren<Text>();
        recon.text = player.landForces.Recon.ToString();
        Text prediction = armyTable.Rows[7].Cells[1].GetComponentInChildren<Text>();
        prediction.text = player.landForces.Prediction.ToString();
    } */

    private void fightCorruption()
    {
        App app = UnityEngine.Object.FindObjectOfType<App>();
        Nation player = State.getNations()[app.GetHumanIndex()];
        PlayerPayer.payForDevelopmentAction(player, 1);
        // PlayerReceiver.reduceCorruption(player);
        player.decreaseCorruption();
        uiUpdater.updateUI();
    }


   /* private void IncreasePOP()
    {
        App app = UnityEngine.Object.FindObjectOfType<App>();
        Nation player = State.getNations()[app.GetHumanIndex()];
        PlayerPayer.payForMorePOP(player);
        uiUpdater.updateUI();
    } */

    private void IncreaseIP()
    {
        App app = UnityEngine.Object.FindObjectOfType<App>();
        Nation player = State.getNations()[app.GetHumanIndex()];
        PlayerPayer.payForDevelopmentAction(player, 1);
        Debug.Log("Increasing AP");
        PlayerReceiver.receiveIP(player);
        uiUpdater.updateUI();
    }

    private void addResearchPoints()
    {
        Debug.Log("Doing Research");

        App app = UnityEngine.Object.FindObjectOfType<App>();
        Nation player = State.getNations()[app.GetHumanIndex()];
        PlayerPayer.payForDevelopmentAction(player, 1);
        PlayerReceiver.CollectResearchPoints(player);
        uiUpdater.updateUI();
    }

    private void OpenResearchTree()
    {
        selfProvince.SetActive(false);
        otherProvince.SetActive(false);
        progressPanel.SetActive(false);
        coordinator.progressFlag = false;
        App app = UnityEngine.Object.FindObjectOfType<App>();
        Nation player = State.getNations()[app.GetHumanIndex()];
        updateTechnologyTree();
        numberPattents.text = player.getNumberPattents().ToString();
        

        if (TechnologyTree.activeSelf == false)
        {
            TechnologyTree.SetActive(true);
            return;
        }
        else if (TechnologyTree.activeSelf == true)
        {
            TechnologyTree.SetActive(false);
            return;
        }
    }

    public void updateTechnologyTree()
    {
        App app = UnityEngine.Object.FindObjectOfType<App>();
        Nation player = State.getNations()[app.GetHumanIndex()];
        float researchPoints = player.Research;
        Debug.Log(techTreeConnector.name);
        // string realName = transform.Find("TechName").GetComponent<Text>().text;
        foreach (Transform tech in techTreeConnector.transform)
        {
            // string name = tech.transform.Find("TechName").GetComponent<Text>().text;
            string name = tech.name;
            Debug.Log(tech.name);
            if(name == "LineContainer")
            {
                continue;
            }
            Text tname = tech.transform.GetChild(0).GetComponentInChildren<Text>();
            Text tcost = tech.transform.GetChild(1).GetComponentInChildren<Text>();
            Debug.Log(name);
            Technology thisTech = State.GetTechnologies()[name];
            Debug.Log(thisTech.GetTechName());
            string techName = thisTech.GetTechName();
            techName = techName.Replace('_', ' ');
           // tname.text = techName;
            tcost.text = thisTech.GetCost().ToString();
        
            //Technology currentTech = State.technologies[name];
            if (player.GetTechnologies().Contains(name))
            {
                Image techImage = tech.GetComponent<Image>();
                techImage.sprite = Resources.Load("AlchemistUITools/SteamContent/Sprites/Buttons/QuadroBtnBg", typeof(Sprite)) as Sprite;      
            }
            //string cost = tech.transform.Find("TechCost").GetComponent<Text>().text;
        }
    }


    private void placeCardOnTable(string cardName, int cardSlot)
    {
        Debug.Log(cardName);
        GameObject card = Resources.Load("TacticCards/" + cardName) as GameObject;
        GameObject myNewInstance = (GameObject)Instantiate(card);
       // Transform hand = tacticCardBook.transform;
       // myNewInstance.transform.SetParent(hand.transform.GetChild(cardSlot), false);
      //  Debug.Log("The Parent is: " + hand.transform.GetChild(cardSlot).name);
        myNewInstance.transform.localPosition = new Vector3(0, 30, 0);
        myNewInstance.transform.localScale = new Vector3(0.66f, 0.66f, 0.66f);
    }

   /* private void buildNavy()
    {
        App app = UnityEngine.Object.FindObjectOfType<App>();
        int playerIndex = app.GetHumanIndex();
        Nation player = State.getNations()[playerIndex];
        if (player.GetTechnologies().Contains("oil_powered_ships"))
        {
            PlayerPayer.PayDreadnought(player);
            player.seaForces.dreadnought.addUnit();
        }

        else if (player.GetTechnologies().Contains("ironclad"))
        {
            PlayerPayer.PayIronClad(player);
            player.seaForces.ironclad.addUnit();
        }
        else
        {
            PlayerPayer.PayFrigate(player);
            player.seaForces.frigate.addUnit();
        }
        uiUpdater.updateUI();

    } */

    private void getCultureCards()
    {
        selfProvince.SetActive(false);
        otherProvince.SetActive(false);
        progressPanel.SetActive(false);
        coordinator.progressFlag = false;
        
        App app = UnityEngine.Object.FindObjectOfType<App>();
        Nation player = State.getNations()[app.GetHumanIndex()];
        PlayerPayer.payForDevelopmentAction(player, 1);

        MyEnum.cultCard newCard = PlayerReceiver.collectCultureCard(player);

        uiUpdater.updateUI();

        Stack<CultureCard> remainingCultCards = State.getCultureDeck();
        HashSet<CultureCard> cultDeckSet = new HashSet<CultureCard>(remainingCultCards);

        List<MyEnum.cultCard> playerCultHand = new List<MyEnum.cultCard>(player.getCultureCards());
        HashSet<CultureCard> playerCultSet = new HashSet<CultureCard>(remainingCultCards);
        foreach (MyEnum.cultCard cardName in playerCultHand)
        {
            CultureCard card = State.getCultureCardByName(cardName);
            playerCultSet.Add(card);
        }
     
        updateCultureCardBook(newCard);
        showCultureHandPanel();
    }

    private void showCultureHandPanel()
    {
        selfProvince.SetActive(false);
        otherProvince.SetActive(false);
        progressPanel.SetActive(false);
        coordinator.progressFlag = false;
        App app = UnityEngine.Object.FindObjectOfType<App>();
        Nation player = State.getNations()[app.GetHumanIndex()];
        Debug.Log("Show culture cards");
        if (cultureCardPanel.activeSelf == false)
        {
            // updateCultureCardBook();
            numberOfMovements.text = player.NumberOfMovementSets.ToString();
            numberOfGenera.text = player.NumberOfGeneraSets.ToString();
            numberOfEras.text = player.NumberOfEraSets.ToString();
            cultureCardPanel.SetActive(true);
        }
        else
        {
            cultureCardPanel.SetActive(false);
        }

    }

    private void updateCultureCardBook(MyEnum.cultCard newCard)
    {
        Transform hand = cultureCardPanel.transform;
        Debug.Log("Child count: " + hand.childCount);
        App app = UnityEngine.Object.FindObjectOfType<App>();
        Nation player = State.getNations()[app.GetHumanIndex()];
        int cardSlot = player.getCultureCards().Count -1;
        placeCultureCardOnTable(newCard, cardSlot);
    }

    private void placeCultureCardOnTable(MyEnum.cultCard cardName, int cardSlot)
    {
        Debug.Log(cardSlot);
        App app = UnityEngine.Object.FindObjectOfType<App>();
        Nation player = State.getNations()[app.GetHumanIndex()];
        Debug.Log(cardName.ToString());
        CultureCard newCard = State.getCultureCardByName(cardName);

        Transform cardTransform = cardArea.transform.GetChild(cardSlot); 
        GameObject card = cardTransform.gameObject;
        card.SetActive(true);
        Image cardImage = card.GetComponent<Image>();
        cardImage.sprite = Resources.Load("CultureCards/CultureCardSprites/" + newCard.getCardName(), typeof(Sprite)) as Sprite;
        cardImage.enabled = true;
        Debug.Log(card.name);
        //  Image border = card.GetComponentInChildren<Image>();
        Image border = card.transform.Find("border").GetComponentInChildren<Image>();
        border.name = cardName.ToString();
        Debug.Log(border.name);
        Text[] texts = border.GetComponentsInChildren<Text>();
        Text name = texts[0];
        Text era = texts[1];
        Text effect = texts[2];
        //Image CardType = border.GetComponentInParent<Image>();

        Image CardType = border.transform.Find("Type").GetComponentInChildren<Image>();
        card.name = newCard.getCardName().ToString();
        Debug.Log("Type: " + newCard.getCardType());
        border.sprite = Resources.Load("CultureCards/CultureCardSprites/" + newCard.getCardType(), typeof(Sprite)) as Sprite;
        name.text = newCard.getCardName().ToString();
        era.text = newCard.getEra().ToString();
        effect.text = newCard.getDescription();
        CardType.sprite = Resources.Load("CultureCards/CultureCardSprites/TypeSymbols/" + newCard.getCardType(), typeof(Sprite)) as Sprite;

    
        /*myNewInstance.transform.SetParent(hand.transform.GetChild(cardSlot), false);
        //  MyEnum.cultCard cardType = (MyEnum.cultCard)Enum.Parse(typeof(MyEnum.cultCard), cardName, true);
        CultureCard thisCard = State.getCultureCard(cardName);
        if (thisCard.getOriginator() == player.getIndex())
        {

            Transform tempItem = hand.transform.GetChild(cardSlot);
            Transform grandChild = tempItem.GetChild(0);
            Image img = grandChild.GetComponent<Image>();
            Debug.Log(img.name);
            img.transform.SetParent(hand.transform.GetChild(cardSlot), false);
            img.enabled = true;
            img.transform.SetAsLastSibling();
        }
        //Debug.Log("The Parent is: " + hand.transform.GetChild(cardSlot).name);
        myNewInstance.transform.localPosition = new Vector3(0, 20, 0);
        myNewInstance.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f); */
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
