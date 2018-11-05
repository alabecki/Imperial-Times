using assemblyCsharp;
using System;
using System.Collections;
using System.Collections.Generic;
using UI.Tables;
using UnityEngine;
using UnityEngine.UI;
using EasyUIAnimator;

public class TabHandler : MonoBehaviour {

    public Camera cam1;
    public Camera cam2;

    public GameObject EmpireTab;
    public GameObject MarketTab;
    public GameObject DiplomacyTab;
    public GameObject DataTab;
    public GameObject ReportTab;

    public Button empireButton;
    public Button captialButton;
    public Button marketButton;
    public Button diplomacyButton;
    public Button diplomacyProvinceButton;
    public Text selectedNationName;
    private bool diplomacyFromProv = false; 

    public Button dataButton;
    public Button reportButton;

    public GameObject mapOptions;

    //Market Objects
    public TableLayout marketTable;
    public TableRow marketRow;
    public RectTransform marketScrollviewContent;
    DD_DataDiagram priceGraph;
    public List<float> priceHistory = new List<float>();
    public Image itemImage;
    public Text offeredLastTurn;
    public Text soldLastTurn;
    public Button plus;
    public Button minus;
    public Toggle High;
    public Toggle Medium;
    public Toggle Low;

    public Image typeSort;
    public Image inventorySort;
    public Image priceSort;
    public Image changeSort;

    //DiplomacyObjects
    public TableLayout nationList;
    public Image nationFlag;
    public Text nationName;
    public Text prestigePoints;
    public Text prestigeRank;
    public Text industrialPoints;
    public Text industrialRank;
    public Text militaryPoints;
    public Text militaryRank;
    public Text nationPopValue;
    public Text nationGoldValue;
    public Image nationStability;

    public GameObject currentWars;
    public GameObject currentDefensiveAlliances;
    public GameObject currentFullAlliances;
    public GameObject currentCollonies;
    public GameObject currentSpheres;
    public GameObject currentEmbargos;

    public Text ourRelationsText;
    public Button improveRelations;
    public Button worsenRelations;
    public Button declareWar;
    public Button gainCB;
    public Button offerAliance;
    public Button embargo;
    public Button negotiate;
    public Button openBorders;
    public Button leaveAlliance;

    public Button nationSelectorA;
    public Button nationSelectorB;


    private bool marketFlag = false;
    private bool diploFlag = false;


    private UIAnimation marketEnter;
    private UIAnimation marketExit;

    private UIAnimation diplomacyFade;
    private UIAnimation diplomacyResize;

    Graphic[] diploReact;

    public GameObject warClaimPanel;
    public TableLayout warClaimListTable;
    public TableRow claimRow;
    public Button selectClaim;
    public Button confirmWarClaim;
    private UIAnimation claimOpen;
    private UIAnimation claimClose;
    public TableLayout warClaimDetails;
    private string currentClaimID;
    private bool warClaimFlag = false;
    private bool nationHasBeenSelected = false;

    public Text responseText;
    public GameObject diplomaticResponse;
    public Image diplomaticResponseFlag;
    public Text nameOfResponder;
    public Image diploResponsePic;

    public GameObject ministerReport;
    public Text ministerReportText;
    public Image ministerImage;

    //Empire Panel Items

    public Image flagImage;
    public Text capitalCity;
    public Text nationality;
    public Text corruption;
    public Text morale;

    public Text urbanPOPs;
    public Text militaryPOPs;
    public Text ruralPOPs;
    public Text unemployedPOPs;
    public Text totalPOPS;

    public Text Prestige;
    public Text militaryScore;
    public Text industrialScore;
    public Text totalScore;

    public Text industrialPOINTS;
    public Text industrialLevel;
    public Text researchPoints;
    public Text numPatents;
    public Text researchLevel;
    public Text colonialPoints;
    public Text numberColonies;
    public Text colonialLevel;
    public Text infulencePoints;
    public Text numberSpheres;
    public Text reputations;
    public Text cultLevel;
    public Text militaryLevel;

    public Text wheatInventory;
    public Text wheatProducing;
    public Text wheatConsuming;
    public Text wheatForecast;

    public Text meatInventory;
    public Text meatProducing;
    public Text meatConsuming;
    public Text meatForecast;

    public Text fruitInventory;
    public Text fruitProducing;
    public Text fruitConsuming;
    public Text fruitForecast;

    public Text coalInventory;
    public Text coalProducing;
    public Text coalConsuming;
    public Text coalForecast;

    public Text oilInventory;
    public Text oilProducing;
    public Text oilConsuming;
    public Text oilForecast;



    // Use this for initialization
    void Start()
    {
        RectTransform marketRect = MarketTab.GetComponent<RectTransform>();
        diploReact = DiplomacyTab.GetComponentsInChildren<Graphic>();

        marketEnter = UIAnimator.Move(marketRect, new Vector2(0.5f, 1f), new Vector2(0.5f, 0.48f), 1f).SetModifier(Modifier.Linear); ;
        marketExit = UIAnimator.Move(marketRect, new Vector2(0.5f, 0.48f), new Vector2(0.5f, 1f), 1f).SetModifier(Modifier.Linear); ;

    
        //diplomacyEnter =UIAnimator.ChangeColor(diploReact, new Color(1,1,1,0), new Color(1,1,1,1), 1.2f);
        //diplomacyExit = UIAnimator.ChangeColor(diploReact, new Color(1, 1, 1, 1), new Color(1, 1, 1, 0), 1.2f);

        empireButton.onClick.AddListener(delegate { showEmpirePanel(); });
        captialButton.onClick.AddListener(delegate { showCapitalPanel(); });
        marketButton.onClick.AddListener(delegate { showMarketPanel(); });
        diplomacyButton.onClick.AddListener(delegate { showDiplomacyPanel(); });
        diplomacyProvinceButton.onClick.AddListener(delegate { showDiplomacyPanelProv(); });
       // nationSelectorA.onClick.AddListener(delegate { nationFromDiploPanelA(); });
       // nationSelectorB.onClick.AddListener(delegate { nationFromDiploPanelB(); });

        dataButton.onClick.AddListener(delegate { showDataPanel(); });
        reportButton.onClick.AddListener(delegate { showReportPanel(); });

        improveRelations.onClick.AddListener(delegate { improveNationRelations(); });
        worsenRelations.onClick.AddListener(delegate { worsenNationRelations(); });
        declareWar.onClick.AddListener(delegate { declareNationWar(); });
        gainCB.onClick.AddListener(delegate { gainNationCB(); });
        offerAliance.onClick.AddListener(delegate { alterAlliance(); });
        embargo.onClick.AddListener(delegate { alterEmbargo(); });
        negotiate.onClick.AddListener(delegate { negotiateNation(); });
        openBorders.onClick.AddListener(delegate { openBordersNation(); });
        leaveAlliance.onClick.AddListener(delegate { leaveAllianceNation(); });

        warClaimPanel.SetActive(false);
        diplomaticResponse.SetActive(false);
        ministerReport.SetActive(false);




        selectClaim.onClick.AddListener(delegate { getClaimDetails(); });
        confirmWarClaim.onClick.AddListener(delegate { ConfirmWarClaim(); });

        claimOpen = UIAnimator.Scale
           (warClaimPanel.GetComponent<RectTransform>(), new Vector3(0, 0, 0), new Vector3(1, 1, 1), 1f);
        claimClose = UIAnimator.Scale
          (warClaimPanel.GetComponent<RectTransform>(), new Vector3(1, 1, 1), new Vector3(0, 0, 0), 1f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void showEmpirePanel()
    {
        if (EmpireTab.activeSelf == false)
        {
            if(marketFlag == true)
            {
                marketExit.Play();
                marketFlag = false;
            }
            if(diploFlag == true)
            {
                diploExitTop();
                diploFlag = false;
            }
            DataTab.SetActive(false);
            ReportTab.SetActive(false);
            updateEmpireTab();
            EmpireTab.SetActive(true);
        }
        else
        {
            EmpireTab.SetActive(false);
        }

    }

    private void showCapitalPanel()
    {
        
            if (cam1.enabled)
            {
                EmpireTab.SetActive(false);
            if (marketFlag == true)
            {
                marketExit.Play();
                marketFlag = false;
            }
            if (diploFlag == true)
            {
                diploExitTop();
                //diplomacyExit.Play();
                diploFlag = false;
            }
                DataTab.SetActive(false);
                ReportTab.SetActive(false);
                 mapOptions.SetActive(false);

                updateCapitalTab();
                cam1.enabled = !cam1.enabled;
                cam2.enabled = !cam2.enabled;
            }
            else
            {
            mapOptions.SetActive(true);
            cam1.enabled = !cam1.enabled;
            cam2.enabled = !cam2.enabled;
            mapOptions.SetActive(true);
        }
    }

    private void showMarketPanel()
    {


        if (MarketTab.activeSelf == false || marketFlag == false)
        {
            EmpireTab.SetActive(false);
            DataTab.SetActive(false);
            ReportTab.SetActive(false);
            if (diploFlag == true)
            {
                diploExitTop();
                //diplomacyExit.Play();
                diploFlag = false;
            }
            updateMarketTab();
            MarketTab.SetActive(true);
            marketFlag = true;
            marketEnter.Play();
        }
        else
        {
            marketExit.Play();
            marketFlag = false;
            //Task.Delay(1000).ContinueWith(t => MarketTab.SetActive(false));
            // MarketTab.SetActive(false);
        }

    }

    private void showDiplomacyPanel()
    {
        Debug.Log("Clicked on Diplo button");
        diplomacyFromProv = false;
        if (DiplomacyTab.activeSelf == false || diploFlag == false)
        {
            EmpireTab.SetActive(false);
            if (marketFlag == true)
            {
                marketExit.Play();
                marketFlag = false;
            }
            DataTab.SetActive(false);
            ReportTab.SetActive(false);

            UpdateDiplomacy();
            diploFlag = true;
            DiplomacyTab.SetActive(true);
            // diplomacyEnter.Play();
            diploEnterTop();
        }
        else
        {
            diploFlag = false;
            diploExitTop();
           // diplomacyExit.Play();
           //   DiplomacyTab.SetActive(false);
        }
    }

    private void showDiplomacyPanelProv()
    {
        {
            Debug.Log("Clicked on Diplo button");
            diplomacyFromProv = true;
            nationHasBeenSelected = false;
            if (DiplomacyTab.activeSelf == false || diploFlag == false)
            {
                EmpireTab.SetActive(false);
                if (marketFlag == true)
                {
                    marketExit.Play();
                    marketFlag = false;
                }
                DataTab.SetActive(false);
                ReportTab.SetActive(false);

                UpdateDiplomacy();
                DiplomacyTab.SetActive(true);
                diploEnterTop();
            }
            else
            {
                diploFlag = false;
                diploExitTop();
              //  diplomacyExit.Play();
            }
        }
    }


    private void showDataPanel()
    {
        if (DataTab.activeSelf == false)
        {
            EmpireTab.SetActive(false);
            if (marketFlag == true)
            {
                marketExit.Play();
                marketFlag = false;
            }
            if (diploFlag == true)
            {
                diploExitTop();
                //diplomacyExit.Play();
                diploFlag = false;
            }
            ReportTab.SetActive(false);

            UpdateDataTab();
            DataTab.SetActive(true);
        }
        else
        {
            DataTab.SetActive(false);
        }
    }

    private void showReportPanel()
    {
        if (ReportTab.activeSelf == false)
        {
            EmpireTab.SetActive(false);
            if (marketFlag == true)
            {
                marketExit.Play();
                marketFlag = false;
            }
            if (diploFlag == true)
            {
                diploExitTop();
                //diplomacyExit.Play();
                diploFlag = false;
            }
            DataTab.SetActive(false);

            updateReportTab();
            ReportTab.SetActive(true);
        }
        else
        {
            ReportTab.SetActive(false);
        }
    }


    private void diploEnterTop()
    {
        foreach (Graphic graphic in diploReact)
        {
            diplomacyResize = UIAnimator.Scale
              (DiplomacyTab.GetComponent<RectTransform>(), new Vector3(0, 0, 0), new Vector3(1, 1, 1), 0.1f);
            diplomacyFade = UIAnimator.ChangeColor(graphic, new Color(1, 1, 1, 0), new Color(1, 1, 1, 1), 1.2f);
            diplomacyResize.SetDelay(0f);
            diplomacyResize.Play();
            diplomacyFade.Play();


        }
    }

    private void diploExitTop()
    {
        foreach (Graphic graphic in diploReact)
        {
            diplomacyFade = UIAnimator.ChangeColor(graphic, new Color(1, 1, 1, 1), new Color(1, 1, 1, 0), 1.2f);
            diplomacyResize = UIAnimator.Scale
                (DiplomacyTab.GetComponent<RectTransform>(), new Vector3(1, 1, 1), new Vector3(0, 0, 0), 0.1f);
            diplomacyResize.SetDelay(1.25f);
            diplomacyFade.Play();
            diplomacyResize.Play();
        }
    }


    private void updateEmpireTab()
    {
        App app = UnityEngine.Object.FindObjectOfType<App>();
        int playerIndex = app.GetHumanIndex();
        Nation player = State.getNations()[playerIndex];
        flagImage.sprite = Resources.Load("Flags/" + player.getNationName().ToString(), typeof(Sprite)) as Sprite;
        capitalCity.text = player.capital;
        nationality.text = player.culture;
        morale.text = player.GetMilitaryForm().maxMorale.ToString();
        corruption.text = player.GetCorruption().ToString();
        urbanPOPs.text = player.getUrbanPOP().ToString();
        militaryPOPs.text = player.getMilitaryPOP().ToString();
        ruralPOPs.text = player.getRuralPOP().ToString();
        float POPgrowthRate = PlayerCalculator.calculatePOPGrowthRate(player);
        float POPgrowth = (POPgrowthRate * player.getTotalPOP())/100;
        unemployedPOPs.text = player.getUnemployed().ToString() + " (" + POPgrowth + ")";   
        totalPOPS.text = player.getTotalPOP().ToString();

        Prestige.text = player.getPrestige().ToString();
        militaryScore.text = player.getMilitaryScore().ToString();
        industrialScore.text = player.getIndustrialScore().ToString();
        totalScore.text = player.getTotalScore().ToString();

        industrialPOINTS.text = player.getIP().ToString();
        industrialLevel.text = player.getInvestmentLevel().ToString();
        researchPoints.text = player.Research.ToString();
        numPatents.text = player.getNumberPattents().ToString();
        researchLevel.text = player.getResearchLevel().ToString();
        colonialPoints.text = player.GetColonialPoints().ToString();
        numberColonies.text = player.getColonies().Count.ToString();
        colonialLevel.text = player.getColonialLevel().ToString();
        infulencePoints.text = player.InfulencePoints.ToString();
        numberSpheres.text = player.getSpheres().Count.ToString();
        reputations.text = player.Reputation.ToString();
        cultLevel.text = player.getCulureLevel().ToString();
        militaryLevel.text = player.getArmyLevel().ToString();

        float currentWheat = player.getNumberResource(MyEnum.Resources.wheat);
        float wheat_Producing = PlayerCalculator.getResourceProducing(player, MyEnum.Resources.wheat);
        float wheat_comsuming = (player.getTotalPOP() / 10);
        wheatInventory.text =  Math.Floor(currentWheat).ToString("0.0");
        wheatProducing.text = wheat_Producing.ToString("0.0");
        wheatConsuming.text = wheat_comsuming.ToString("0.0");
        wheatForecast.text = ((currentWheat + wheat_Producing) - wheat_comsuming).ToString("0.0");

        float currentMeat = player.getNumberResource(MyEnum.Resources.meat);
        float meat_Producing = PlayerCalculator.getResourceProducing(player, MyEnum.Resources.meat);
        float meat_comsuming = (player.getTotalPOP() / 20);
        meatInventory.text = Math.Floor(currentMeat).ToString("0.0");
        meatProducing.text = meat_Producing.ToString("0.0");
        meatConsuming.text = meat_comsuming.ToString("0.0");
        meatForecast.text = ((currentMeat + meat_Producing) - meat_comsuming).ToString("0.0");

        float currentFruit = player.getNumberResource(MyEnum.Resources.fruit);
        float fruit_Producing = PlayerCalculator.getResourceProducing(player, MyEnum.Resources.fruit);
        float fruit_comsuming = (player.getTotalPOP() / 20);
        fruitInventory.text = Math.Floor(currentFruit).ToString("0.0");
        fruitProducing.text = fruit_Producing.ToString("0.0");
        fruitConsuming.text = fruit_comsuming.ToString("0.0");
        fruitForecast.text = ((currentFruit + fruit_Producing) - fruit_comsuming).ToString("0.0");


        float currentCoal = player.getNumberResource(MyEnum.Resources.coal);
        float coal_Producing = PlayerCalculator.getResourceProducing(player, MyEnum.Resources.coal);
        float coal_consuming = PlayerCalculator.coalNeededForRailRoads(player);
        coalInventory.text = Math.Floor(currentCoal).ToString("0.0");
        coalProducing.text = coal_Producing.ToString("0.0");
        coalConsuming.text = coal_consuming.ToString("0.0");
        coalForecast.text = ((currentCoal + coal_Producing) - coal_consuming).ToString("0.0");


        float currentOil = player.getNumberResource(MyEnum.Resources.oil);
        float oil_Producing = PlayerCalculator.getResourceProducing(player, MyEnum.Resources.oil);
        // float oil_consuming = PlayerCalculator.coalNeededForRailRoads(player);
        float oilCon = player.getOilNeeded();
        oilInventory.text = Math.Floor(currentOil).ToString("0.0");
        oilProducing.text = oil_Producing.ToString("0.0");
        oilConsuming.text = oilCon.ToString("0.0");
         oilForecast.text = ((currentOil + oil_Producing) - oilCon).ToString("0.0");
    }




    private void updateCapitalTab()
    {

    }

    private void updateMarketTab()
    {
            App app = UnityEngine.Object.FindObjectOfType<App>();
            int playerIndex = app.GetHumanIndex();
            Nation player = State.getNations()[playerIndex];
            Market market = State.market;
            marketTable.ClearRows();
            int turn = State.turn;
            High.interactable = false;
            Medium.interactable = false;
            Low.interactable = false;

            plus.interactable = false;
            minus.interactable = false;
        itemImage.sprite = Resources.Load("Resource/wheat", typeof(Sprite)) as Sprite;


        foreach (MyEnum.Resources resource in Enum.GetValues(typeof(MyEnum.Resources)))
            {
                if (resource == MyEnum.Resources.gold)
                {
                    continue;
                }
                if (resource == MyEnum.Resources.rubber && !player.GetTechnologies().Contains("electricity"))
                {
                    continue;
                }
                if (resource == MyEnum.Resources.oil && !player.GetTechnologies().Contains("oil_drilling"))
                {
                    continue;
                }
                TableRow newRow = Instantiate<TableRow>(marketRow);
                var fieldGameObject = new GameObject("Field", typeof(RectTransform));
                newRow.gameObject.SetActive(true);
                newRow.preferredHeight = 30;
                newRow.name = resource.ToString();
            marketTable.AddRow(newRow);
                Debug.Log("   Row Name " + newRow.name);

            marketScrollviewContent.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (marketTable.transform as RectTransform).rect.height);
                // var textGameObject = new GameObject("Text", typeof(RectTransform));
                List<TableCell> cells = newRow.Cells;
                Debug.Log(resource.ToString());

                Transform res = newRow.Cells[0].transform.GetChild(0);
                Image resImg = res.GetComponent<Image>();
                resImg.preserveAspect = true;
                resImg.sprite = Resources.Load("Resource/" + resource.ToString(), typeof(Sprite)) as Sprite;
                //newRow.Cells[0].GetComponentInChildren<Image>().sprite = Resources.Load("Resource/" + resource.ToString(), typeof(Sprite)) as Sprite;
                newRow.Cells[1].GetComponentInChildren<Text>().text = player.getNumberResource(resource).ToString();
                if (turn == 1)
                {
                    newRow.Cells[2].GetComponentInChildren<Text>().text = "3";
                }
                else
                {
                    newRow.Cells[2].GetComponentInChildren<Text>().text =
                        market.getPriceOfResource(resource).ToString();

                }
                Transform chg = newRow.Cells[3].transform.GetChild(0);
                Image chgImg = chg.GetComponent<Image>();
                chgImg.preserveAspect = true;
                if (turn < 2)
                {
                    chgImg.sprite = Resources.Load("Sprites/flat", typeof(Sprite)) as Sprite;
                }
                else
                {
                    float currentTurnPrice = market.getPriceOfResource(resource);
                    float lastTurnPrice =  State.market.getResourcePriceHistory(resource)[turn - 1];
                    if (currentTurnPrice > lastTurnPrice)
                    {
                        chgImg.sprite = Resources.Load("Sprites/greenUp", typeof(Sprite)) as Sprite;
                    }
                    else if (currentTurnPrice < lastTurnPrice)
                    {
                        chgImg.sprite = Resources.Load("Sprites/redDown", typeof(Sprite)) as Sprite;

                    }
                }
            }
            foreach (MyEnum.Goods good in Enum.GetValues(typeof(MyEnum.Goods)))
            {
                if (good == MyEnum.Goods.chemicals && !player.GetTechnologies().Contains("chemistry"))
                {
                    continue;
                }
                if (good == MyEnum.Goods.gear && !player.GetTechnologies().Contains("electricity"))
                {
                    continue;
                }
                if (good == MyEnum.Goods.telephone && !player.GetTechnologies().Contains("telephone"))
                {
                    continue;
                }
                if (good == MyEnum.Goods.auto && !player.GetTechnologies().Contains("automobile"))
                {
                    continue;
                }
                if (good == MyEnum.Goods.fighter && !player.GetTechnologies().Contains("flight"))
                {
                    continue;
                }
                if (good == MyEnum.Goods.tank && !player.GetTechnologies().Contains("mobile_warfare"))
                {
                    continue;
                }
                // Debug.Log("Instnatiate " + good.ToString() + " row");
                TableRow newRow = Instantiate<TableRow>(marketRow);
                var fieldGameObject = new GameObject("Field", typeof(RectTransform));
                newRow.gameObject.SetActive(true);
                newRow.preferredHeight = 30;
                newRow.name = good.ToString();
                marketTable.AddRow(newRow);
                marketScrollviewContent.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,
                    (marketTable.transform as RectTransform).rect.height);
                // var textGameObject = new GameObject("Text", typeof(RectTransform));
                List<TableCell> cells = newRow.Cells;
                // Debug.Log(good.ToString());

                Transform res = newRow.Cells[0].transform.GetChild(0);
                Image resImg = res.GetComponent<Image>();
                resImg.preserveAspect = true;
                resImg.sprite = Resources.Load("FinishedGoods/" + good.ToString(), typeof(Sprite)) as Sprite;
                newRow.Cells[1].GetComponentInChildren<Text>().text = player.getNumberGood(good).ToString();
                if (turn == 1)
                {
                    newRow.Cells[2].GetComponentInChildren<Text>().text = "5";
                }
                else
                {
                    newRow.Cells[2].GetComponentInChildren<Text>().text =
                        market.getPriceOfGood(good).ToString();

                }
                Transform chg = newRow.Cells[3].transform.GetChild(0);
                Image chgImg = chg.GetComponent<Image>();
                chgImg.preserveAspect = true;
                if (turn < 2)
                {
                    chgImg.sprite = Resources.Load("Sprites/flat", typeof(Sprite)) as Sprite;
                }
                else
                {
                    float currentTurnPrice = market.getPriceOfGood(good);
                    float lastTurnPrice = market.getPriceOfGood(good);
                    if (currentTurnPrice > lastTurnPrice)
                    {
                        chgImg.sprite = Resources.Load("Sprites/greenUp", typeof(Sprite)) as Sprite;
                    }
                    else if (currentTurnPrice < lastTurnPrice)
                    {
                        chgImg.sprite = Resources.Load("Sprites/redDown", typeof(Sprite)) as Sprite;

                    }
                }

        }
        // marketScrollviewContent.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,
        //       (marketTable.transform as RectTransform).rect.height);
        MarketHelper.currentItem = "wheat";

        //   itemImage.sprite = Resources.Load("Resources/wheat", typeof(Sprite)) as Sprite;
        itemImage.sprite = Resources.Load("Resource/wheat", typeof(Sprite)) as Sprite;

            if (State.turn > 1)
            {
                offeredLastTurn.text = "Offered Last Turn: " +
                    market.getNumberOfResourcesOffered(MyEnum.Resources.wheat).ToString();
                soldLastTurn.text = "Sold Last Turn: " + market.getNumberResourcesSold(MyEnum.Resources.wheat).ToString();
                priceHistory = market.getResourcePriceHistory(MyEnum.Resources.wheat);
            }
            else
            {
                offeredLastTurn.text = "Offered Last Turn: 0";
                soldLastTurn.text = "Sold Last Turn: 0";
            }

     //   MarketHelper.currentItem = "wheat";
        typeSort.sprite = Resources.Load("Sprites/sort", typeof(Sprite)) as Sprite;
        inventorySort.sprite = Resources.Load("Sprites/sort", typeof(Sprite)) as Sprite;
        priceSort.sprite = Resources.Load("Sprites/sort", typeof(Sprite)) as Sprite;
        changeSort.sprite = Resources.Load("Sprites/sort", typeof(Sprite)) as Sprite;
    }

    private void nationFromDiploPanelA()
    {
        int otherIndex = Int32.Parse(nationSelectorA.transform.parent.parent.name);
        State.setCurrentSelectedNationDiplomacy(otherIndex);
        Debug.Log("clicked");
        diplomacyFromProv = false;
        nationHasBeenSelected = true;

         UpdateDiplomacy();
    }

    private void nationFromDiploPanelB()
    {
        int otherIndex = Int32.Parse(nationSelectorB.transform.parent.parent.name);
        State.setCurrentSelectedNationDiplomacy(otherIndex);
        Debug.Log("clicked");
        diplomacyFromProv = false;
        nationHasBeenSelected = true;

        UpdateDiplomacy();
    }

    private void UpdateDiplomacy()
    {
        App app = UnityEngine.Object.FindObjectOfType<App>();
        int playerIndex = app.GetHumanIndex();
        Nation player = State.getNations()[playerIndex];
        Nation chosenNation = State.getNations()[0];
        //int otherIndex = 0;
        if (diplomacyFromProv == true)
        {
            chosenNation = State.GetNationByName(selectedNationName.text);
            Debug.Log("Chosen nation: " + chosenNation.getNationName() + " index: " + chosenNation.getIndex());
            State.setCurrentSelectedNationDiplomacy(chosenNation.getIndex());
        }
        else if (nationHasBeenSelected == true)
        {
            chosenNation = State.getNations()[State.getCurrentSlectedNationDiplomacy()];
            Debug.Log("Chosen nation: " + chosenNation.getNationName() + " index: " + chosenNation.getIndex());

        }
        else if (nationHasBeenSelected == false)
        {
            for (int i = 0; i < State.getNations().Count; i++)
            {
                Nation thisNation = State.getNations()[i];
                Debug.Log("Chosen nation: " + chosenNation.getNationName() + " index: " + chosenNation.getIndex());

                if (thisNation.getType() == MyEnum.NationType.major && thisNation.getIndex() != player.getIndex())
                {
                    chosenNation = State.getNations()[i];
                    State.setCurrentSelectedNationDiplomacy(chosenNation.getIndex());
                }
            }
        }
        Relation relationToChosenPlayer = player.getRelationToThisPlayer(chosenNation.getIndex());
        Relation relationFromChosenPlayer = player.getRelationFromThisPlayer(chosenNation.getIndex());

        ourRelationsText.text = relationFromChosenPlayer.getAttitude().ToString();

        if (relationFromChosenPlayer.getAttitude() < 20)
        {
            ourRelationsText.color = Color.red;
        }
        else if (relationFromChosenPlayer.getAttitude() < 40)
        {
            ourRelationsText.color = new Color(255, 165, 0);
        }
        else if (relationFromChosenPlayer.getAttitude() < 60)
        {
            ourRelationsText.color = Color.yellow;
        }
        else if (relationFromChosenPlayer.getAttitude() < 80)
        {
            ourRelationsText.color = Color.green;
        }
        else
        {
            ourRelationsText.color = Color.blue;
        }

        nationFlag.sprite = Resources.Load("Flags/" + chosenNation.getNationName().ToString(), typeof(Sprite)) as Sprite;
        nationName.text = chosenNation.getNationName().ToString();
        prestigePoints.text = chosenNation.getPrestige().ToString();
        prestigeRank.text = State.history.getPrestigeRanking(chosenNation.getIndex()).ToString();
        industrialPoints.text = chosenNation.getIndustrialScore().ToString();
        industrialRank.text = State.history.getMilitaryRanking(chosenNation.getIndex()).ToString();
        militaryPoints.text = chosenNation.getMilitaryScore().ToString();
        militaryRank.text = State.history.getMilitaryRanking(chosenNation.getIndex()).ToString();
        nationPopValue.text = chosenNation.getTotalPOP().ToString();
        nationGoldValue.text = chosenNation.getGold().ToString();
        float nationStab = chosenNation.Stability;
        if (nationStab < -2.6)
        {
            nationStability.sprite = Resources.Load("Sprites/Stability/-3", typeof(Sprite)) as Sprite;
        }
        else if (nationStab < -1.6)
        {
            nationStability.sprite = Resources.Load("Sprites/Stability/-2", typeof(Sprite)) as Sprite;
        }
        else if (nationStab < -0.6)
        {
            nationStability.sprite = Resources.Load("Sprites/Stability/-1", typeof(Sprite)) as Sprite;
        }
        else if (nationStab < 0.45)
        {
            nationStability.sprite = Resources.Load("Sprites/Stability/0", typeof(Sprite)) as Sprite;
        }
        else if (nationStab < 1.45)
        {
            nationStability.sprite = Resources.Load("Sprites/Stability/1", typeof(Sprite)) as Sprite;
        }
        else if (nationStab < 2.45)
        {
            nationStability.sprite = Resources.Load("Sprites/Stability/2", typeof(Sprite)) as Sprite;
        }
        else
        {
            nationStability.sprite = Resources.Load("Sprites/Stability/3", typeof(Sprite)) as Sprite;
        }

        int warCount = 1;
        int defAllianceCount = 1;
        int fullAllianceCount = 1;
        int embargoCount = 1;
        foreach (Nation nation in State.getNations().Values)
        {
            if (nation.getType() == MyEnum.NationType.oldMinor || nation.getIndex() == chosenNation.getIndex())
            {
                continue;
            }
            // Debug.Log("Initial Nation is: " + chosenNation.getNationName().ToString());
            //  Debug.Log("Other Nation is: " + nation.getNationName().ToString() + " index: " + nation.getIndex());

            Relation relationToThisNation = chosenNation.getRelationFromThisPlayer(nation.getIndex());
            if (relationToThisNation.isAtWar())
            {
                if (warCount < 5)
                {
                    currentWars.transform.GetChild(warCount).GetChild(0).GetComponent<Image>().sprite =
                         Resources.Load("Flags/" + nation.getNationName(), typeof(Sprite)) as Sprite;
                    warCount += 1;
                }
            }
            if (relationToThisNation.isDefensiveAlliance())
            {
                if (defAllianceCount < 5)
                {

                    currentDefensiveAlliances.transform.GetChild(defAllianceCount).GetChild(0).GetComponent<Image>().sprite =
                    Resources.Load("Flags/" + nation.getNationName(), typeof(Sprite)) as Sprite;
                    defAllianceCount += 1;
                }
            }
            if (relationToThisNation.isFullAlliance())
            {
                if (fullAllianceCount < 5)
                {

                    currentFullAlliances.transform.GetChild(fullAllianceCount).GetChild(0).GetComponent<Image>().sprite =
                   Resources.Load("Flags/" + nation.getNationName(), typeof(Sprite)) as Sprite;
                    fullAllianceCount += 1;
                }
            }
            if (relationToThisNation.isEmbargoing())
            {
                if (embargoCount < 5)
                {
                    currentEmbargos.transform.GetChild(embargoCount).GetChild(0).GetComponent<Image>().sprite =
                   Resources.Load("Flags/" + nation.getNationName(), typeof(Sprite)) as Sprite;
                    embargoCount += 1;
                }
            }
        }

        for (int col = 1; col <= chosenNation.getColonies().Count; col++)
        {
            Nation thisColony = State.getNations()[chosenNation.getColonies()[col]];
            currentCollonies.transform.GetChild(col).GetChild(0).GetComponent<Image>().sprite =
                Resources.Load("Flags/" + thisColony.getNationName(), typeof(Sprite)) as Sprite;
            if (col > 4)
            {
                break;
            }
        }

        for (int sp = 1; sp <= chosenNation.getSpheres().Count; sp++)
        {
            Nation thisSphere = State.getNations()[chosenNation.getColonies()[sp]];
            currentSpheres.transform.GetChild(sp).GetChild(0).GetComponent<Image>().sprite =
                 Resources.Load("Flags/" + thisSphere.getNationName(), typeof(Sprite)) as Sprite;
            if (sp > 4)
            {
                break;
            }
        }
        updateDiplomacyOptionButtons(player, chosenNation);

    }

    private void updateDiplomacyOptionButtons(Nation player, Nation chosenNation)
    {
        Relation relationToChosenPlayer = player.getRelationToThisPlayer(chosenNation.getIndex());
        Relation relationFromChosenPlayer = player.getRelationFromThisPlayer(chosenNation.getIndex());

        improveRelations.interactable = true;
        worsenRelations.interactable = true;
        gainCB.interactable = true;
        declareWar.interactable = true;
        offerAliance.interactable = true;
        embargo.interactable = true;
        negotiate.interactable = true;
        openBorders.interactable = true;
        leaveAlliance.interactable = true;

        if (player.DiplomacyPoints < 1)
        {
            improveRelations.interactable = false;
            worsenRelations.interactable = false;
            gainCB.interactable = false;
            offerAliance.interactable = false;
        }

        if (relationFromChosenPlayer.getAttitude() == 100)
        {
            improveRelations.interactable = false;

        }

        if (relationFromChosenPlayer.getAttitude() == 0)
        {
            worsenRelations.interactable = false;

        }
        bool possCB = false;
        foreach (WarClaim claim in player.getWarClaims())
        {
            if (claim.getOtherNation() == chosenNation.getIndex())
            {
                possCB = true;
                break;
            }
        }
        if (possCB == false || relationToChosenPlayer.isAtWar()
            || relationToChosenPlayer.isDefensiveAlliance() || relationToChosenPlayer.isFullAlliance() ||
            relationFromChosenPlayer.givesMilitaryAccess() || relationToChosenPlayer.isRecentPeace() ||
            relationToChosenPlayer.getAttitude() >= 25)
        {
            gainCB.interactable = false;
            declareWar.interactable = false;

        }
        if (relationToChosenPlayer.getCasusBelli().Count < 1)
        {
            declareWar.interactable = false;

        }

        if (relationFromChosenPlayer.getAttitude() < 75)
        {
            offerAliance.interactable = false;
        }

        if (relationToChosenPlayer.getAttitude() > 35)
        {
            embargo.interactable = false;
        }

        if (!relationToChosenPlayer.isDefensiveAlliance() && !relationToChosenPlayer.isFullAlliance())

        {
            leaveAlliance.interactable = false;
        }

        if (relationFromChosenPlayer.getAttitude() < 80)
        {
            openBorders.interactable = false;
        }

        if (relationToChosenPlayer.isAtWar())
        {
            declareWar.GetComponentInChildren<Text>().text = "Offer Peace";
            declareWar.interactable = true;

        }

        if (relationToChosenPlayer.isDefensiveAlliance())
        {
            offerAliance.GetComponentInChildren<Text>().text = "Offer Full Alliance";
            if (relationFromChosenPlayer.getAttitude() > 88)
            {
                offerAliance.interactable = true;
            }
            else
            {
                offerAliance.interactable = false;
            }
        }

        if (relationToChosenPlayer.isEmbargoing())
        {
            embargo.GetComponentInChildren<Text>().text = "End Embargo";
            embargo.interactable = true;
        }

        if (relationToChosenPlayer.givesMilitaryAccess())
        {
            openBorders.GetComponentInChildren<Text>().text = "Cancel Open Borders";
        }

       
    }

    private void improveNationRelations()
    {
        int  otherIndex = State.getCurrentSlectedNationDiplomacy();
        App app = UnityEngine.Object.FindObjectOfType<App>();
        int playerIndex = app.GetHumanIndex();
        Nation player = State.getNations()[playerIndex];
        Debug.Log("Other index: " + otherIndex);
        Nation otherNation = State.getNations()[otherIndex];
        PlayerReceiver.improveRelations(player, otherIndex);
        ourRelationsText.text = player.getRelationFromThisPlayer(otherIndex).getAttitude().ToString();
        Relation relationFromChosenPlayer = player.getRelationFromThisPlayer(otherNation.getIndex());
        updateDiplomacyOptionButtons(player, otherNation);
        responseText.text = "It was lovely having a spot of tea with you this afternoon. I feel like we really bonded, especialy upon learning about our shared " +
            "love of bird watching.";
        nameOfResponder.text = "Response from " + otherNation.getNationName().ToString();
        diplomaticResponseFlag.sprite = Resources.Load("Flags/" + otherNation.getNationName().ToString(), typeof(Sprite)) as Sprite;
        updateDiplomacyOptionButtons(player, otherNation);
        diploResponsePic.sprite = Resources.Load("Sprites/Diplomat1", typeof(Sprite)) as Sprite;
        diplomaticResponse.SetActive(true);


    }

    private void worsenNationRelations()
    {
       int  otherIndex = State.getCurrentSlectedNationDiplomacy();

        App app = UnityEngine.Object.FindObjectOfType<App>();
        int playerIndex = app.GetHumanIndex();
        Nation player = State.getNations()[playerIndex];
        Nation otherNation = State.getNations()[otherIndex];
        PlayerReceiver.worsenRelations(player, otherIndex);
        ourRelationsText.text = player.getRelationFromThisPlayer(otherIndex).getAttitude().ToString();
        updateDiplomacyOptionButtons(player, otherNation);
        responseText.text = "The most insulting thing about your insults is that you did not even take the time to come up with a truly witty way " +
            "to belittle our nation's great leaders. So many missed oppertunities. Do try harder next time!";
        nameOfResponder.text = "Response from " + otherNation.getNationName().ToString();
        diplomaticResponseFlag.sprite = Resources.Load("Flags/" + otherNation.getNationName().ToString(), typeof(Sprite)) as Sprite;
        updateDiplomacyOptionButtons(player, otherNation);
        diploResponsePic.sprite = Resources.Load("Sprites/Diplomat1", typeof(Sprite)) as Sprite;
        diplomaticResponse.SetActive(true);

    }

    private void declareNationWar()
    {
        App app = UnityEngine.Object.FindObjectOfType<App>();
        int playerIndex = app.GetHumanIndex();
       Nation player = State.getNations()[playerIndex];
      int   otherIndex = State.getCurrentSlectedNationDiplomacy();
        Relation relationTo = player.getRelationToThisPlayer(otherIndex);
        Nation otherNation = State.getNations()[otherIndex];
        if (relationTo.isAtWar() == false)
        {
            PlayerReceiver.declareWar(player, otherIndex);
            declareWar.GetComponent<Text>().text = "Offer Peace";
            declareWar.interactable = false;
            responseText.text = "Oh, I see how it is. Yes, yes. Take advantage of us during out time of weakness. And to think, just two generations" +
                "ago our repsective royal families were impreeding together. Know that we will make this as costly for you as possible.";
            nameOfResponder.text = "Response from " + otherNation.getNationName().ToString();
            diplomaticResponseFlag.sprite = Resources.Load("Flags/" + otherNation.getNationName().ToString(), typeof(Sprite)) as Sprite;
            updateDiplomacyOptionButtons(player, otherNation);
            diploResponsePic.sprite = Resources.Load("Sprites/Diplomat1", typeof(Sprite)) as Sprite;
            diplomaticResponse.SetActive(true);

        }
        else
        {
            //Must check if other player is willing to make peace
        }

        updateDiplomacyOptionButtons(player, otherNation);

    }

    private void gainNationCB()
    {
       int  otherIndex = State.getCurrentSlectedNationDiplomacy();
        App app = UnityEngine.Object.FindObjectOfType<App>();
        int playerIndex = app.GetHumanIndex();
       Nation player = State.getNations()[playerIndex];
        createWarClaimList();
        warClaimFlag = true;
        warClaimPanel.SetActive(true);
        claimOpen.Play();
        Nation otherNation = State.getNations()[otherIndex];
        updateDiplomacyOptionButtons(player, otherNation);
        if (player.getDiplomacyPoints() >= 1)
        {
            declareWar.interactable = true;
        }

    }

    private void createWarClaimList()
    {

        App app = UnityEngine.Object.FindObjectOfType<App>();
        int playerIndex = app.GetHumanIndex();
        Nation player = State.getNations()[playerIndex];
        warClaimListTable.ClearRows();
        foreach (WarClaim claim in player.getWarClaims())
        {
            TableRow newRow = Instantiate<TableRow>(claimRow);
            newRow.gameObject.SetActive(true);
            newRow.preferredHeight = 20;
            newRow.name = claim.getID();
            warClaimListTable.AddRow(newRow);
            newRow.Cells[0].GetComponentInChildren<Text>().text =
                claim.getOtherNation().ToString() + " for " + claim.GetClaimType();
            newRow.Cells[0].GetComponentInChildren<Button>().name = claim.getID();

        }
    }

    private void getClaimDetails()
    {
       int  otherIndex = State.getCurrentSlectedNationDiplomacy();
        App app = UnityEngine.Object.FindObjectOfType<App>();
        int playerIndex = app.GetHumanIndex();
        Nation player = State.getNations()[playerIndex];
        WarClaim claim = player.getWarClaims()[0];

        string claimID = selectClaim.name;
        currentClaimID = claimID;
        foreach (WarClaim cl in player.getWarClaims())
        {
            if (cl.getID() == claimID)
            {
                claim = cl;
            }
        }
        warClaimDetails.Rows[0].Cells[1].GetComponentInChildren<Text>().text = claim.getOtherNation().ToString();
        if (claim.getProvinceClaimed() == -1)
        {
            warClaimDetails.Rows[1].Cells[1].GetComponentInChildren<Text>().text = "None";
        }
        else
        {
            Province prov = State.getProvinces()[claim.getProvinceClaimed()];
            warClaimDetails.Rows[1].Cells[1].GetComponentInChildren<Text>().text = prov.getProvName().ToString();
        }
        if (claim.getColonyClaimed() == -1)
        {
            warClaimDetails.Rows[2].Cells[1].GetComponentInChildren<Text>().text = "None";
        }
        else
        {
            Nation nat = State.getNations()[claim.getColonyClaimed()];
            warClaimDetails.Rows[2].Cells[1].GetComponentInChildren<Text>().text = nat.getNationName().ToString();
        }
        if (claim.getSphereClaimed() == -1)
        {
            warClaimDetails.Rows[3].Cells[1].GetComponentInChildren<Text>().text = "None";
        }
        else
        {
            Nation nat = State.getNations()[claim.getColonyClaimed()];
            warClaimDetails.Rows[3].Cells[1].GetComponentInChildren<Text>().text = nat.getNationName().ToString();
        }

        if (claim.checkTradeClaim() == false)
        {
            warClaimDetails.Rows[4].Cells[1].GetComponentInChildren<Text>().text = "No";
        }
        else
        {
            warClaimDetails.Rows[4].Cells[1].GetComponentInChildren<Text>().text = "Yes";
        }

        if (claim.checkPaymentClaim() == false)
        {
            warClaimDetails.Rows[5].Cells[1].GetComponentInChildren<Text>().text = "No";
        }
        else
        {
            warClaimDetails.Rows[5].Cells[1].GetComponentInChildren<Text>().text = "Yes";
        }

    }

    private void ConfirmWarClaim()
    {
        int otherIndex = State.getCurrentSlectedNationDiplomacy();
        App app = UnityEngine.Object.FindObjectOfType<App>();
        int playerIndex = app.GetHumanIndex();
        Nation player = State.getNations()[playerIndex];
        WarClaim claim = player.getWarClaims()[0];
        foreach (WarClaim cl in player.getWarClaims())
        {
            if (cl.getID() == currentClaimID)
            {
                PlayerReceiver.gainCB(player, otherIndex, cl);
            }
        }
        gainCB.interactable = false;
        Nation otherNation = State.getNations()[otherIndex];
        updateDiplomacyOptionButtons(player, otherNation);
        claimClose.Play();
        responseText.text = " ";
        ministerReportText.text = "Your excellency, after our patient attempts to reason with "  + otherNation.getNationName() + " " +
            "fell upon deaf ears, we were given no choice but abandon the the path of persuasion. We have established our political and moral right to" +
            " enforce justice by the sword";
        updateDiplomacyOptionButtons(player, otherNation);
        ministerImage.sprite = Resources.Load("Sprites/bismark", typeof(Sprite)) as Sprite;
        ministerReport.SetActive(true);

    }

    private void alterAlliance()
    {
        App app = UnityEngine.Object.FindObjectOfType<App>();
        int playerIndex = app.GetHumanIndex();
        Nation player = State.getNations()[playerIndex];
        int otherIndex = State.getCurrentSlectedNationDiplomacy();
        Nation otherNation = State.getNations()[otherIndex];
        responseText.text = otherNation.getAI().GetDiplomatic().responceToAllianceRequest(player, otherIndex);
        nameOfResponder.text = "Response from " + otherNation.getNationName().ToString();
        diplomaticResponseFlag.sprite = Resources.Load("Flags/" + otherNation.getNationName().ToString(), typeof(Sprite)) as Sprite;
        updateDiplomacyOptionButtons(player, otherNation);
        diploResponsePic.sprite = Resources.Load("Sprites/Diplomat1", typeof(Sprite)) as Sprite;
        diplomaticResponse.SetActive(true);


    }

    private void alterEmbargo()
    {
        App app = UnityEngine.Object.FindObjectOfType<App>();
        int playerIndex = app.GetHumanIndex();
        Nation player = State.getNations()[playerIndex];
       int  otherIndex = State.getCurrentSlectedNationDiplomacy();

        Relation relationTo = player.getRelationToThisPlayer(otherIndex);
        Relation relationFrom = player.getRelationFromThisPlayer(otherIndex);
        Nation otherNation = State.getNations()[otherIndex];
        diplomaticResponseFlag.sprite = Resources.Load("Flags/" + otherNation.getNationName().ToString(), typeof(Sprite)) as Sprite;
        diploResponsePic.sprite = Resources.Load("Sprites/Diplomat1", typeof(Sprite)) as Sprite;

        if (!relationTo.isEmbargoing())
        {
            relationTo.startEmbargo();
            relationTo.adjustAttude(-10);
            responseText.text = " We find your decision to forego trade relations between our mighty nations is disturbing. It is all too easy" +
                "for way by way of coin to escalate to war by way of arms.";
        }
        else
        {
            relationTo.endEmbargo();
            relationTo.adjustAttude(5);
            responseText.text = " We see you have finally come to your senses and recognized that freedom of trade benefits us all. Let us pray " +
                "that the damage caused to our friendship is soon undone through mutual cooperation and prosperity.";
        }
        updateDiplomacyOptionButtons(player, otherNation);
        diplomaticResponse.SetActive(true);

    }

    private void negotiateNation()
    {

    }

    private void leaveAllianceNation()
    {
        App app = UnityEngine.Object.FindObjectOfType<App>();
        int playerIndex = app.GetHumanIndex();
        Nation player = State.getNations()[playerIndex];
        int otherIndex = State.getCurrentSlectedNationDiplomacy();

        Relation relationTo = player.getRelationToThisPlayer(otherIndex);
        Relation relationFrom = player.getRelationFromThisPlayer(otherIndex);
        if (relationTo.isDefensiveAlliance())
        {
            relationTo.endDefensiveAlliance();
            relationFrom.endDefensiveAlliance();
            relationTo.adjustAttude(-10);
        }
        if (relationTo.isFullAlliance())
        {
            relationTo.endFullAlliance();
            relationFrom.endFullAlliance();
            relationTo.adjustAttude(-10);
        }

    }

    private void openBordersNation()
    {
        App app = UnityEngine.Object.FindObjectOfType<App>();
        int playerIndex = app.GetHumanIndex();
       Nation player = State.getNations()[playerIndex];
      int   otherIndex = State.getCurrentSlectedNationDiplomacy();

        Relation relationTo = player.getRelationToThisPlayer(otherIndex);
        Relation relationFrom = player.getRelationFromThisPlayer(otherIndex);
        if (!relationTo.givesMilitaryAccess())
        {
            //must check if AI agreess
        }
        else
        {
            relationTo.EndMilitaryAccess();
            relationFrom.EndMilitaryAccess();
        }

    }



    private void UpdateDataTab()
    {

    }
    
    private void updateReportTab()
    {

    }



    /*void updateResourceTable()
    {
        App app = UnityEngine.Object.FindObjectOfType<App>();
        Nation player = State.getNations()[app.GetHumanIndex()];
        int k = 0;
        int turn = State.turn;
        
        foreach (MyEnum.Resources resource in Enum.GetValues(typeof(MyEnum.Resources)))
        {
            if(resource == MyEnum.Resources.gold)
            {
                continue;
            }

            Text inventory = resourceTable.Rows[k].Cells[1].GetComponentInChildren<Text>();
            inventory.text = player.getNumberResource(resource).ToString();

            Text lastPrice = resourceTable.Rows[k].Cells[2].GetComponentInChildren<Text>();
            lastPrice.text = State.market.getPriceOfResource(resource, turn).ToString();
            k += 1;
        }
        
    } */

    /* void updateGoodsTable()
     {
         App app = UnityEngine.Object.FindObjectOfType<App>();
         int k = 0;
         Nation player = State.getNations()[app.GetHumanIndex()];
         int turn = State.turn;

         foreach (MyEnum.Goods good in Enum.GetValues(typeof(MyEnum.Goods)))
         {

             Text inventory = goodsTable.Rows[k].Cells[1].GetComponentInChildren<Text>();
             inventory.text = player.getNumberGood(good).ToString();
             Text lastPrice = goodsTable.Rows[k].Cells[2].GetComponentInChildren<Text>();
             lastPrice.text = State.market.getPriceOfGood(good, turn).ToString();
             k += 1;

         }

     } */

    /* void updateProductionTab()
     {

         App app = UnityEngine.Object.FindObjectOfType<App>();
         Nation player = State.getNations()[app.GetHumanIndex()];
         confirmBuild.interactable = false;

         if (player.FreePop < 1)
         {
             increaseUrbanPOP.interactable = false;
         }
         else
         {
             increaseUrbanPOP.interactable = true;
         }
         if (player.getUrbanPOP() < 1)
         {
             decreaseUrbanPOP.interactable = false;
         }
         else
         {
             decreaseUrbanPOP.interactable = true;
         }
         numberUrbanWorkers.text = "Number of Urban" + Environment.NewLine + "Workers: " +
            player.getUrbanPOP().ToString();

         if (player.getNumberGood(MyEnum.Goods.chemicals) >= 1)
         {
             toDyes.interactable = true;
         }
         else
         {
             toDyes.interactable = false;
         }
         if (player.getNumberGood(MyEnum.Goods.chemicals) >= 3)
         {
             toOil.interactable = true;
         }
         else
         {
             toOil.interactable = false;
         }
         if (player.getNumberGood(MyEnum.Goods.chemicals) >= 1 &&
              player.getNumberResource(MyEnum.Resources.oil) >= 1)
         {
             toRubber.interactable = true;
         }
         else
         {
             toRubber.interactable = false;
         }
     } */

    /*   public void upgradeTechnologyTree(){
            App app = UnityEngine.Object.FindObjectOfType<App>();
            Nation player = State.getNations()[app.GetHumanIndex()];
            float researchPoints = player.Research;
            Image[] allChildren = techTreeConnector.GetComponentsInChildren<Image>();
            string realName = transform.Find("TechName").GetComponent<Text>().text;
            foreach (Image tech in allChildren)
            {

            // string name = tech.transform.Find("TechName").GetComponent<Text>().text;
                string name = tech.name;
            //Technology currentTech = State.technologies[name];
                if (player.GetTechnologies().Contains(name))
             {
                 tech.sprite = Resources.Load("Textures/WoodTexture", typeof(Sprite)) as Sprite;
             }
                //string cost = tech.transform.Find("TechCost").GetComponent<Text>().text;

            }
        }  */




    private void UpdateCityView()
    {
      //  var myNewSmoke = Instantiate(poisonSmoke, Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
       // myNewSmoke.transform.parent = gameObject.transform;
        MyEnum.Era era = State.era;
        if(era == MyEnum.Era.Early)
        {

        }

    }





 
}
