using assemblyCsharp;
using System;
using System.Collections;
using System.Collections.Generic;
using UI.Tables;
using UnityEngine;
using UnityEngine.UI;
using EasyUIAnimator;
using ChartAndGraph;
using UnityEngine.EventSystems;
using TMPro;
using SuperTreeView;
using UI.ThreeDimensional;
using WorldMapStrategyKit;

public class TabHandler : MonoBehaviour {

    public Camera mainCamera;
    public Camera diplomacyCamera;
    public UI_Updater uiUpdater;

    public Button turnButton;

    public GameObject MarketTab;
    public GameObject DiplomacyTab;
    // public GameObject DataTab;
    //  public GameObject ReportTab;

    public GameObject thisProvince;
    public GameObject otherPrivince;

   // Top Header _____________________________________________________________________________________
    public GameObject MenuPanel;
    public Button marketButton;
    public Button diplomacyButton;
    public Button productionButton;
    public Button diplomacyProvinceButton;
   // public Button transportButton;
    public Button menuButton;

    public Text selectedNationName;
    private bool diplomacyFromProv = false;

    private bool inventoryFlag;

    //public Button dataButton;
    public Button reportButton;
    public GameObject mapOptions;

    //Progress Panel____________________________________________________________________________________________
    public GameObject developmentPanel;
    public Button developmentPanelButton;
    public Button addAPButton;
    public Button addDPBUtton;
    public Text currentAP;
    public Text currentPP;

    public GameObject apCost;
    public GameObject apCostTwo;

    public GameObject earlyDPCost;
    public GameObject lateDPCost;

    public Text devRP;
    public Text devIP;
    public Text devStability;
    public Text corruption;

   // public Button increasePOPButton;
    public Button fundResearchButton;
    public Button fundCultureButton;
    public Button capitalInvestmentButton;
    public Button fightCorruption;

    public Button viewAchievements;

    // Naval Panel__________________________________________________________________________
    public Button navalPanelButton;
    public GameObject NavyPanel;
    public TableLayout navyTable;

    public Text recruitingFrigatesText;
    public Text recruitingIroncladsText;
    public Text recruitingBattleshipsText;

    public Button recruitFrigateButton;
    public Button recruitIroncladButton;
    public Button recruitBattleshipButton;

    public Button upgradeShipyardButton;
    public TextMeshProUGUI shipyardLevelValue;

    public GameObject shipyardUpgradeCostOne;
    public GameObject shipyardUpgradeCostTwo;

    // public Button upgradeShipyardButton;

    // Market Panel _________________________________________________________________________________________________________________________________
    public TableLayout marketTableList;
    public RectTransform marketScrollContent;
    private MyEnum.Resources mostRecentResource;
    private MyEnum.Goods mostRecentGood;
    private int mostRecentColIndex = 0;

    public Image selectedItemImage;
    public Text selectedItemPrice;
    public Image priceChange;

    public Text selectedIntemInventory;
    public Text selectdItemProducing;
    public Text offeredLastTurn;
    public Text soldLastTurn;
    public Text bidLastTurn;

    public GraphChartBase marketGraph;
    List<float> priceHistory = new List<float>();

    // Production Panel ___________________________________________________________________________________________________________________________________
    public GameObject productionPanel;
    public TableLayout productionTable;

    // Diplomacy Objects ___________________________________________________________________________________________________________________________________
    public TableRow nationRow;

    public TableLayout nationList;
    private Animator leaderAnimator;

    public UIObject3D otherNationFlag;
   // public UIObject3D otherNationLeader;

    public GameObject sitariLeader;
    public GameObject bambakiLeader;
    public GameObject sideroLeader;
    public GameObject chaldeaLeader;
    public GameObject wyvermountLeader;
    public GameObject boreoisLeader;

    private Vector3 leaderPosition;
    private Quaternion leaderRotation;

    public Text otherNationName; 

    public Button boycottButton;
    public Button negotiationButton;

  //  public Text currentlySelectedItem;
    Graphic[] diploReact;
    bool nationHasBeenSelected = false;
    public Text ourRelations;
   // public Text otherNationDialogue;

    public TableLayout generalInformation;

    public TextMeshProUGUI otherNationDialogue;
   // public GameObject otherNationLeaderModel;

    public Button generalInfoButton;
    public Button otherColoniesButton;
    public Button otherSpheresButton;

    public GameObject otherNationGeneralInfo;
    public GameObject otherNationColonies;
    public GameObject otherNationSpheres;
    public TreeView otherNationTheyRecognize;
    public TreeView otherNationWeRecognize;

    public Transform sitariFlag;
    public Transform bambakiFlag;
    public Transform sideroFlag;
    public Transform chaldeaFlag;
    public Transform wyvermountFlag;
    public Transform boreoisFlag;

    public TableLayout PlayerSelectedItemTable;
    public TableLayout AIselectedItemTable;

    public TableRow aiHolding;

    TreeViewItem theyRecognizeTheseSpheres;
    TreeViewItem theyRecognizeTheseColonies;
    TreeViewItem theyRecognizeTheseProvinces;

    TreeViewItem weRecognizeTheseSpheres;
    TreeViewItem weRecognizeTheseColonies;
    TreeViewItem weRecognizeTheseProvinces;

   public GameObject offerMoney;
   public GameObject requestMoney;

    public TableRow SelectedItem;

    // public TreeView theyRecognize;
    //  public TreeView weRecognize;

    bool theyRecognizeSpheres = false;
    bool theyRecognizeColonies = false;
    bool theyRecognizeProvinces = false;

    bool weRecognizeSpheres = false;
    bool weRecognizeColonies = false;
    bool weRecognizeProvinces = false;

    // public TableLayout nationList;
    public Button nationSelectorA;
    // public Button nationSelectorB;

    public GUI_Coordinator coordinator;

   /* private bool progressFlag = false;
    private bool marketFlag = false;
    private bool diploFlag = false;
    private bool prodFlag = false;
    private bool transportFlag = false;
    private bool relationsFlag = false; */


    public GameObject negotiateHumanPanel;
    public GameObject negotiateAIPanel;
    public bool negotiationsOpen;

    public Button offerButton;
    public GameObject otherNationAdditionalInformation;

    // Animations_______________________________________________________________________________________________________________________

    private UIAnimation progressEnter;
    private UIAnimation progressExit;

    private UIAnimation marketEnter;
    private UIAnimation marketExit;

    private UIAnimation diplomacyFade;
    private UIAnimation diplomacyResize;

    private UIAnimation productionEnter;
    private UIAnimation productionExit;

    private UIAnimation transportEnter;
    private UIAnimation transportExit;

    public GameObject ministerReport;
    public Text ministerReportText;
    public Image ministerImage;

    // Transport Panel________________________________________________________________________________________________
    public Button transportButton;
    public GameObject transportPanel;
    public Text transportCapacity;
    public Text coalCapacity;
    public Button addTransportCapacityButton;
    public TableLayout resourceTableA;
    public TableLayout resourceTableB;

    public AudioClip wagon;
    public AudioClip train;

    // Help Panels _____________________________________________________________________________________________________________
    public GameObject navalHelp;
    public GameObject generalHelp;
    public GameObject techTreeHelp;
    public GameObject cultureHelp;
    public GameObject majorRelationsHelp;
    public GameObject mainDiplomacyHelp;
    public GameObject provinceHelpPanel;
    public GameObject cheatSheet;

    // Major Power Relations _____________________________________________________________________________________________________

    public GameObject majorPowerRelations;


    public GameObject flags;

    public Image BambakiSidero;
    public Image BoreoisSitari;
    public Image ChaldeaWyvermount;
    public Image BambakiChaldea;
    public Image BambakiSitari;
    public Image BambakiBoreois;
    public Image BambakiWyvermount;
    public Image BoreoisChaldea;
    public Image SitariWyvermount;
    public Image BoreoisWyvermount;
    public Image ChaldeaSitari;
    public Image ChaldeaSidero;
    public Image SideroSitari;
    public Image SideroWyvermount;
    public Image BoreoisSidero;

    private UIAnimation relationsEnter;
    private UIAnimation relationsExit;

    // Minor Diplomacy
    public GameObject minorDiplomacyPanel;
    public Text minorNationName;
    public Text relationsWithMinor;
    public Text minorCoin;
    public Text minorStability;
    public Text minorCorruption;
    public Text minorTechnologies;
    public Button infulenceMinorButton;
    public UIObject3D minorFlag;
    public UIObject3D mostFavouredFlag;
    public Text mostFavouredInfulence;


    void Start()
    {
        State.currentlySelectedNationDiplomacy = findFirstNonHumanPlayer();
        developmentPanel.SetActive(false);
        MarketTab.SetActive(false);
        productionPanel.SetActive(false);
        DiplomacyTab.SetActive(false);
        transportPanel.SetActive(false);
        majorPowerRelations.SetActive(false);


        negotiateHumanPanel.SetActive(false);
        negotiateAIPanel.SetActive(false);

        offerMoney.SetActive(false);
        requestMoney.SetActive(false);
        MenuPanel.SetActive(false);
        NavyPanel.SetActive(false);
        minorDiplomacyPanel.SetActive(false);
      //  majorPowerRelations.SetActive(false);


        navalHelp.SetActive(false);
        generalHelp.SetActive(false);
        techTreeHelp.SetActive(false);
        cultureHelp.SetActive(false);
        majorRelationsHelp.SetActive(false);
        mainDiplomacyHelp.SetActive(false);
        provinceHelpPanel.SetActive(false);
        cheatSheet.SetActive(false);

        hideAllLeaders();

        RectTransform progressRect = developmentPanel.GetComponent<RectTransform>();
        RectTransform marketRect = MarketTab.GetComponent<RectTransform>();
        RectTransform productionRect = productionPanel.GetComponent<RectTransform>();
        RectTransform transportRect = transportPanel.GetComponent<RectTransform>();
        RectTransform majorPowerRelationsRect = majorPowerRelations.GetComponent<RectTransform>();


        diploReact = DiplomacyTab.GetComponentsInChildren<Graphic>();

        progressEnter = UIAnimator.Move(progressRect, new Vector2(0.5f, 1.2f), new Vector2(0.5f, 0.49f), 1f).SetModifier(Modifier.Linear);
        progressExit = UIAnimator.Move(progressRect, new Vector2(0.5f, 0.49f), new Vector2(0.5f, 1.2f), 1f).SetModifier(Modifier.Linear);

        marketEnter = UIAnimator.Move(marketRect, new Vector2(0.5f, 1.2f), new Vector2(0.5f, 0.49f), 1f).SetModifier(Modifier.Linear); 
        marketExit = UIAnimator.Move(marketRect, new Vector2(0.5f, 0.49f), new Vector2(0.5f, 1.2f), 1f).SetModifier(Modifier.Linear); 

        productionEnter = UIAnimator.Move(productionRect, new Vector2(0.5f, 1.2f), new Vector2(0.5f, 0.49f), 1f).SetModifier(Modifier.Linear); 
        productionExit = UIAnimator.Move(productionRect, new Vector2(0.5f, 0.49f), new Vector2(0.5f, 1.2f), 1f).SetModifier(Modifier.Linear);

        transportEnter = UIAnimator.Move(transportRect, new Vector2(0.5f, 1.2f), new Vector2(0.5f, 0.49f), 1f).SetModifier(Modifier.Linear);
        transportExit = UIAnimator.Move(transportRect, new Vector2(0.5f, 0.49f), new Vector2(0.5f, 1.2f), 1f).SetModifier(Modifier.Linear);

        relationsEnter = UIAnimator.Move(majorPowerRelationsRect, new Vector2(0.5f, 1.2f), new Vector2(0.5f, 0.49f), 1f).SetModifier(Modifier.Linear);
        relationsExit = UIAnimator.Move(majorPowerRelationsRect, new Vector2(0.5f, 0.49f), new Vector2(0.5f, 1.2f), 1f).SetModifier(Modifier.Linear);

        // diplomacyEnter =UIAnimator.ChangeColor(diploReact, new Color(1,1,1,0), new Color(1,1,1,1), 1.2f);
        //diplomacyExit = UIAnimator.ChangeColor(diploReact, new Color(1, 1, 1, 1), new Color(1, 1, 1, 0), 1.2f);
        //empireButton.onClick.AddListener(delegate { showEmpirePanel(); });

        turnButton.onClick.AddListener(delegate { turnButtonCloseStuff(); });

        developmentPanelButton.onClick.AddListener(delegate { showDevelopmentPanel(); });
      //  captialButton.onClick.AddListener(delegate { showCapitalPanel(); });
        marketButton.onClick.AddListener(delegate { showMarketPanel(); });
        diplomacyButton.onClick.AddListener(delegate { openMajorRelationsPanel(); });
       // diplomacyProvinceButton.onClick.AddListener(delegate { showDiplomacyPanelProv(); });
     
        productionButton.onClick.AddListener(delegate { showProductionPanel(); });
        transportButton.onClick.AddListener(delegate { showTransportPanel(); });

        menuButton.onClick.AddListener(delegate { showMenuPanel(); });

        addTransportCapacityButton.onClick.AddListener(delegate { addTransportCapacity(); });

        navalPanelButton.onClick.AddListener(delegate { openNavalPanel(); });

        nationSelectorA.onClick.AddListener(delegate { nationFromDiploPanelA(); });
        diplomacyProvinceButton.onClick.AddListener(delegate { openDiplomacyFromProvince(); });
        // dataButton.onClick.AddListener(delegate { showDataPanel(); });
       // reportButton.onClick.AddListener(delegate { showReportPanel(); });

        boycottButton.onClick.AddListener(delegate { boycottNation(); });
        negotiationButton.onClick.AddListener(delegate { openNegotiations(); });
        offerButton.onClick.AddListener(delegate { makeOffer(); });

        generalInfoButton.onClick.AddListener(delegate { showGeneralInfoPanel(); });
        otherColoniesButton.onClick.AddListener(delegate { showColoniesPanel(); });
        otherSpheresButton.onClick.AddListener(delegate { showSpheresPanel(); });

        infulenceMinorButton.onClick.AddListener(delegate { infulenceMinor(); });

        TableRow bidButtons = marketTableList.Rows[1];
        TableRow iconButtons = marketTableList.Rows[2];
        TableRow offerButtons = marketTableList.Rows[3];

        Debug.Log("Begin start for transport sliders. #########################################################################");
        for (int i = 0; i < 6; i++)
        {
            Slider resSliderA = resourceTableA.Rows[i].Cells[2].GetComponentInChildren<Slider>();
            resSliderA.onValueChanged.AddListener(delegate { changeTransportSlider(); });
            Slider resSliderB = resourceTableB.Rows[i].Cells[2].GetComponentInChildren<Slider>();
            resSliderB.onValueChanged.AddListener(delegate { changeTransportSlider(); });
        }

        int colIndex = 0;
        Debug.Log("Runtime assignment of Market Buttons");
        foreach (MyEnum.Resources res in Enum.GetValues(typeof(MyEnum.Resources)))
        {
           // Debug.Log(res + " " + colIndex);
           if(res == MyEnum.Resources.gold)
            {
                continue;
            }
            Button bidButton = bidButtons.Cells[colIndex].GetComponentInChildren<Button>();
           // Debug.Log(bidButton);
            bidButton.onClick.AddListener(delegate { OnBidButtonClick(); });

            Button iconButton = iconButtons.Cells[colIndex].GetComponentInChildren<Button>();
            iconButton.onClick.AddListener(delegate { OnIconButtonClick(); });

            Button offerButton = offerButtons.Cells[colIndex].GetComponentInChildren<Button>();
            offerButton.onClick.AddListener(delegate { OnOfferButtonClick(); });
            colIndex++;
        }

        foreach (MyEnum.Goods good in Enum.GetValues(typeof(MyEnum.Goods)))
        {
            // Debug.Log(good + " " + colIndex);

            Button bidButton = bidButtons.Cells[colIndex].GetComponentInChildren<Button>();
            bidButton.onClick.AddListener(delegate { OnBidButtonClick(); });

            Button iconButton = iconButtons.Cells[colIndex].GetComponentInChildren<Button>();
            iconButton.onClick.AddListener(delegate { OnIconButtonClick(); });

            Button offerButton = offerButtons.Cells[colIndex].GetComponentInChildren<Button>();
            offerButton.onClick.AddListener(delegate { OnOfferButtonClick(); });
            colIndex++;
        }

        Button[] flagButtons = flags.GetComponentsInChildren<Button>();
        for (int i = 0; i < flagButtons.Length; i++)
        {
           // Debug.Log("Number of Flags :  " + flagButtons.Length + "              ooooooooooooooooooooo");
            Button flagButton = flagButtons[i];
            flagButton.onClick.AddListener(delegate { goToNation(); });
        }


        CreateNationList();
        leaderPosition = chaldeaLeader.transform.position;
        leaderRotation = chaldeaLeader.transform.rotation;

        otherNationColonies.SetActive(false);
        otherNationSpheres.SetActive(false);
        otherNationGeneralInfo.SetActive(true);
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.P))
        {
            showDevelopmentPanel();
        }
        if (Input.GetKey(KeyCode.Y))
        {
            showMarketPanel();
        }

        if (Input.GetKey(KeyCode.G))
        {
            openMajorRelationsPanel();
        }

        if (Input.GetKey(KeyCode.M))
        {
            showProductionPanel();
        }

        if (Input.GetKey(KeyCode.T))
        {
            showTransportPanel();
        }

        if (Input.GetKey(KeyCode.Escape))
        {
            showMenuPanel();
        }
    }


    private void turnButtonCloseStuff()
    {
        if (coordinator.marketFlag == true)
        {
            coordinator.marketFlag = false;
            marketExit.Play();
        }
        if (coordinator.prodFlag == true)
        {
            coordinator.prodFlag = false;
            productionExit.Play();
        }
        if (coordinator.transportFlag == true)
        {
            coordinator.transportFlag = false;
            transportExit.Play();
        }

    }

    private void openDiplomacyFromProvince()
    {
        int selectedNationindex = State.getCurrentSlectedNationDiplomacy();
        Nation selectedNation = State.getNations()[selectedNationindex];
        if(selectedNation.getType() == MyEnum.NationType.major)
        {
            showDiplomacyPanel();
        }
        else
        {
            showMinorDiplomacy();
        }
    }

    private void showMinorDiplomacy()
    {
        if(minorDiplomacyPanel.activeSelf == false)
        {
            updateMinorDiplomactPanel();
            minorDiplomacyPanel.SetActive(true);
        }
        else
        {
            minorDiplomacyPanel.SetActive(false);
        }
    }

    private void updateMinorDiplomactPanel()
    {
        App app = UnityEngine.Object.FindObjectOfType<App>();
        int humanIndex = app.GetHumanIndex();
        Nation humanPlayer = State.getNations()[humanIndex];

        int selectedNationindex = State.getCurrentSlectedNationDiplomacy();
        Nation selectedNation = State.getNations()[selectedNationindex];

        if (humanPlayer.RecognizingTheseClaims.Contains(selectedNationindex))
        {
            Text infulenceButtonText = infulenceMinorButton.GetComponentInChildren<Text>();
            infulenceButtonText.text = "Cannot Infulence";
        }

        minorNationName.text = selectedNation.getName();
        int relationsValue = humanPlayer.Relations[selectedNationindex];
        relationsWithMinor.text = relationsValue.ToString();
        relationsWithMinor.color = getRelationColor(humanIndex, selectedNationindex);
         minorCoin.text = selectedNation.getGold().ToString();
         minorStability.text = selectedNation.Stability.ToString();
         minorCorruption.text  = selectedNation.Corruption.ToString();
         minorTechnologies.text = selectedNation.GetTechnologies().Count.ToString();

        if(humanPlayer.InfulencePoints > 0 && !humanPlayer.RecognizingTheseClaims.Contains(selectedNationindex))
        {
            infulenceMinorButton.interactable = true;
        }
        else
        {
            infulenceMinorButton.interactable = false;
        }
        GameObject flagPrefab = Instantiate(Resources.Load<GameObject>("Flags/Prefabs/" + selectedNation.getName()));
        minorFlag.ObjectPrefab = flagPrefab.transform;
        int mostFavouredNationIndex = PlayerCalculator.getMostFavouredMajorNation(selectedNation);
        Nation mostFavouedNation = State.getNations()[mostFavouredNationIndex];
        string mostFavouredNation = mostFavouedNation.getName();
        GameObject flagPrefab2 = Instantiate(Resources.Load<GameObject>("Flags/Prefabs/" + mostFavouredNation));
        mostFavouredFlag.ObjectPrefab = flagPrefab2.transform;
        mostFavouredInfulence.text = mostFavouedNation.Relations[selectedNationindex].ToString();
    }



    private void infulenceMinor()
    {
        Debug.Log("Infulence Minor");
        App app = UnityEngine.Object.FindObjectOfType<App>();
        int humanIndex = app.GetHumanIndex();
        Nation humanPlayer = State.getNations()[humanIndex];

        int selectedNationIndex = State.getCurrentSlectedNationDiplomacy();
        Nation selectedNation = State.getNations()[selectedNationIndex];
        PlayerPayer.payToInfulenceMinor(humanPlayer);
        PlayerReceiver.receiveInfulenceMinor(humanPlayer, selectedNation);
        uiUpdater.updateUI();
        updateMinorDiplomactPanel();
    }


    private void expandTheirSphereRecognitions()
    {
        if (theyRecognizeSpheres)
        {
            Debug.Log("Check");
            theyRecognizeTheseSpheres.Collapse();
            //sphereList.GetComponent<TreeList>().CollapseAllItem();
            theyRecognizeSpheres = false;
            return;

        }
        else
        {
            theyRecognizeSpheres = true;
            UpdateTheirSphereRecognitions();
            // sphereList.GetComponent<TreeList>().ExpandAllItem();
            theyRecognizeTheseSpheres.Expand();
        }
    }

    private void expandTheirProvinceRecognitions()
    {
        if (theyRecognizeProvinces)
        {
            //  boycottList.GetComponent<TreeList>().CollapseAllItem();
            theyRecognizeTheseProvinces.Collapse();
            theyRecognizeProvinces = false;
            return;
        }
        else
        {
            theyRecognizeProvinces = true;
            //UpdateTheirProvinceRecognitions();
            theyRecognizeTheseProvinces.GetComponent<TreeList>().ExpandAllItem();

        }
    }

    private void expandTheirColonyRecognitions()
    {
        if (theyRecognizeColonies)
        {
            // colonyList.GetComponent<TreeList>().CollapseAllItem();
            theyRecognizeTheseColonies.Collapse();
            theyRecognizeColonies = false;
            return;

        }
        else
        {
            theyRecognizeColonies = true;
            UpdateTheirColonyRecognitions();
            theyRecognizeTheseColonies.GetComponent<TreeList>().ExpandAllItem();
        }
    }

    private void expandOurSphereRecognitions()
    {
        if (theyRecognizeSpheres)
        {
            Debug.Log("Check");
            weRecognizeTheseSpheres.Collapse();
            //sphereList.GetComponent<TreeList>().CollapseAllItem();
            weRecognizeSpheres = false;
            return;

        }
        else
        {
            weRecognizeSpheres = true;
            UpdateOurSphereRecognitions();
            // sphereList.GetComponent<TreeList>().ExpandAllItem();
            weRecognizeTheseSpheres.Expand();
        }
    }

    private void expandOurProvinceRecognitions()
    {
        if (weRecognizeProvinces)
        {
            //  boycottList.GetComponent<TreeList>().CollapseAllItem();
            weRecognizeTheseProvinces.Collapse();
            weRecognizeProvinces = false;
            return;
        }
        else
        {
            weRecognizeProvinces = true;
         //   UpdateOurProvinceRecognitions();
            weRecognizeTheseProvinces.GetComponent<TreeList>().ExpandAllItem();

        }
    }

    private void expandOurColonyRecognitions()
    {
        if (weRecognizeColonies)
        {
            // colonyList.GetComponent<TreeList>().CollapseAllItem();
            weRecognizeTheseColonies.Collapse();
            weRecognizeColonies = false;
            return;

        }
        else
        {
            weRecognizeColonies = true;
            UpdateOurColonyRecognitions();
            weRecognizeTheseColonies.GetComponent<TreeList>().ExpandAllItem();
        }
    }

    private void hideAllLeaders()
    {
        bambakiLeader.SetActive(false);
        sitariLeader.SetActive(false);
        chaldeaLeader.SetActive(false);
        wyvermountLeader.SetActive(false);
        sideroLeader.SetActive(false);
        boreoisLeader.SetActive(false);
    }

    private void OnBidButtonClick()
    {
        Debug.Log("Was clicked");
        App app = UnityEngine.Object.FindObjectOfType<App>();
        int playerIndex = app.GetHumanIndex();
        Nation player = State.getNations()[playerIndex];
        Market market = State.market;

        int turn = State.turn;
        float cost = 0;
        int colIndex = 0;
        string currentButtonName = EventSystem.current.currentSelectedGameObject.name;
        MyEnum.Resources item = determineResButton(currentButtonName);
        TableRow bidButtons = marketTableList.Rows[1];
        TableRow bidAmounts = marketTableList.Rows[0];
            TableRow offerButtons = marketTableList.Rows[3];
        //  Text bidAmount = bidAmounts.Cells[colIndex].GetComponent<Text>();
        TableRow offerAmounts = marketTableList.Rows[4];
        //if gold, then we know that the item is in fact a good rather than a resoruce
        if (item != MyEnum.Resources.gold)
        {
            colIndex = getIndexOfResource(item);
            Button bidButton = bidButtons.Cells[colIndex].GetComponent<Button>();
            Button offerButton = offerButtons.Cells[colIndex].GetComponentInChildren<Button>();
            Debug.Log("Item is: " + item + " Index is: " + colIndex);
            Debug.Log("offer Button name: " + offerButton.name);
            //first check if the bid is really just reducing the number of offers
            int numOffers = player.getNumberResourceOffers(item);
            if (numOffers > 0)
            {
                player.decrementResOffers(item);
                uiUpdater.updateUI();

                TextMeshProUGUI offerAmount = offerAmounts.Cells[colIndex].
                    GetComponentInChildren<TextMeshProUGUI>();
                offerAmount.text = player.getNumberResourceOffers(item).ToString();
              
                if (player.getNumberResource(item) - player.getNumberResourceOffers(item) < 1)
                {
                    offerButton.interactable = false;
                }
                else
                {
                    offerButton.interactable = true;
                }

            }
            else if (numOffers < 1)
            {
               // Debug.Log("Resource_Bid");
                cost = market.getPriceOfResource(item);
                player.increaseTotalCurrentBiddingCost(cost);
               Debug.Log("Current Bid Cost " + player.getTotalCurrentBiddingCost());
                //Update Bid Button Status
                updateBidButtonStatus();
                player.incrementResBids(item);
                TextMeshProUGUI bidAmount = bidAmounts.Cells[colIndex].
                    GetComponentInChildren<TextMeshProUGUI>();
               // Debug.Log(bidAmount);
                bidAmount.text = player.getNumberResourceBid(item).ToString();
               // Debug.Log("WTF?");
                offerButton.interactable = true;
            }
        }
        else
        {
            MyEnum.Goods good = determineGoodButton(currentButtonName);
            colIndex = getIndexOfGood(good);
            Debug.Log(good + " " + colIndex);
            //first check if the bid is really just reducing the number of offers
            int numOffers = player.getNumberGoodsOffers(good);
            if (numOffers > 0)
            {
                Debug.Log("Num offers: " + numOffers);
                player.decrementGoodOffers(good);
                uiUpdater.updateUI();

                TextMeshProUGUI offerAmount = offerAmounts.Cells[colIndex].
                   GetComponentInChildren<TextMeshProUGUI>();
                offerAmount.text = player.getNumberGoodsOffers(good).ToString();
                Button offerButton = offerButtons.Cells[colIndex].GetComponentInChildren<Button>();
                if (player.getNumberGood(good) - player.getNumberGoodsOffers(good) < 1)
                {
                    offerButton.interactable = false;
                }
                else
                {
                    offerButton.interactable = true;
                }
            }
            else if (numOffers < 1)
            {
                Debug.Log("Good_Bid");

                cost = market.getPriceOfGood(good);
                player.increaseTotalCurrentBiddingCost(cost);
                Debug.Log("Total bidding cost is now " + player.getTotalCurrentBiddingCost());
                //update all bidding buttons based on cost of total bids
                updateBidButtonStatus();
                player.incrementGoodBids(good);
                TextMeshProUGUI bidAmount = bidAmounts.Cells[colIndex].
                  GetComponentInChildren<TextMeshProUGUI>();
                Debug.Log("num bids " + player.getNumberGoodBid(good));
                bidAmount.text = player.getNumberGoodBid(good).ToString();
                Button offerButton = offerButtons.Cells[colIndex].GetComponentInChildren<Button>();
                offerButton.interactable = true;
            }
        }
        uiUpdater.updateUI();
    }

    private void updateBidButtonStatus()
    {
        App app = UnityEngine.Object.FindObjectOfType<App>();
        int playerIndex = app.GetHumanIndex();
        Nation player = State.getNations()[playerIndex];
        Market market = State.market;
        TableRow bidButtons = marketTableList.Rows[1];

        foreach (MyEnum.Resources res in Enum.GetValues(typeof(MyEnum.Resources)))
        {
            if (res == MyEnum.Resources.gold)
            {
                continue;
            }
            float cost = market.getPriceOfResource(res);
            int index = getIndexOfResource(res);
           // Debug.Log(res + " " + index);
            Button button = bidButtons.Cells[index].GetComponentInChildren<Button>();
            if (player.getTotalCurrentBiddingCost() + cost > player.getGold())
            {
                button.interactable = false;
            }
            else
            {
                button.interactable = true;
            }
        }

        foreach(MyEnum.Goods good in Enum.GetValues(typeof(MyEnum.Goods)))
        {
            float cost = market.getPriceOfGood(good);
            int index = getIndexOfGood(good);
            Button button = bidButtons.Cells[index].GetComponentInChildren<Button>();
            if (player.getTotalCurrentBiddingCost() + cost > player.getGold())
            {
                
                button.interactable = false;
            }
            else
            {
                button.interactable = true;
            }
        }
    }

    private void OnIconButtonClick()
    {
        Debug.Log("Was clicked");

        App app = UnityEngine.Object.FindObjectOfType<App>();
        int playerIndex = app.GetHumanIndex();
        Nation player = State.getNations()[playerIndex];
        Market market = State.market;
        int turn = State.turn;

        string currentButtonName = EventSystem.current.currentSelectedGameObject.name;
        MyEnum.Resources item = determineResButton(currentButtonName);
        Debug.Log("Items is: " + item);
        if (item != MyEnum.Resources.gold)
        {
            selectedItemImage.sprite = Resources.Load("Resource/" + item, typeof(Sprite)) as Sprite;
            Text selectedItemName = selectedItemImage.GetComponentInChildren<Text>();
            selectedItemName.text = item.ToString();
            selectedIntemInventory.text = "Inventory: " + player.getNumberResource(item);
            selectdItemProducing.text = 
                "Producing: " + player.getResTransportFlow(item);

            if (State.turn > 1)
            {
                offeredLastTurn.text = market.getNumberOfResourcesOffered(item).ToString();
                soldLastTurn.text = market.getNumberResourcesSold(item).ToString();
                bidLastTurn.text = market.getNumberResourcesBid(item).ToString();

                selectedItemPrice.text =
                "Price: " + market.getPriceOfResource(item);
            }
            else
            {
                selectedItemPrice.text = "Price: 3";
                offeredLastTurn.text = "0";
                bidLastTurn.text = "0";
                soldLastTurn.text = "0";
            }

            if (State.turn > 2)
            {
                float currentTurnPrice = market.getPriceOfResource(item);
                float lastTurnPrice = State.market.getResourcePriceHistory(item)[turn - 2];
                Debug.Log("Price Last Turn: " + lastTurnPrice);

                if (currentTurnPrice > lastTurnPrice)
                {
                    priceChange.sprite = Resources.Load("Sprites/GUI/greenUp", typeof(Sprite)) as Sprite;
                }
                else if (currentTurnPrice < lastTurnPrice)
                {
                    priceChange.sprite = Resources.Load("Sprites/GUI/redDown", typeof(Sprite)) as Sprite;

                }
                else
                {
                    priceChange.sprite = Resources.Load("Sprites/GUI/flat", typeof(Sprite)) as Sprite;

                }
            }
            else
            {
               priceChange.sprite = Resources.Load("Sprites/GUI/flat", typeof(Sprite)) as Sprite;
            }

            priceHistory = market.getResourcePriceHistory(item);

        }
        else
        // If the item is a good
        {
            MyEnum.Goods good = determineGoodButton(currentButtonName);
            selectedItemImage.sprite = Resources.Load("FinishedGoods/" + good, typeof(Sprite)) as Sprite;
            selectedIntemInventory.text = "Inventory: " + player.getNumberGood(good);
            selectdItemProducing.text =
                "Producing: " + player.industry.getGoodProducing(good);
            selectedItemPrice.text = "Price: " + market.getPriceOfGood(good).ToString();
            Text selectedItemName = selectedItemImage.GetComponentInChildren<Text>();
            selectedItemName.text = item.ToString();

            if (State.turn > 1)
            {
                offeredLastTurn.text = market.getNumberGoodsOffered(good).ToString();
                soldLastTurn.text = market.getNumberOfGoodsSoldLastTurn(good).ToString();
                priceHistory = market.getGoodPriceHistory(good);
                bidLastTurn.text = market.getNumberOfGoodsBidLastTurn(good).ToString();

                float currentTurnPrice = market.getPriceOfGood(good);
                float lastTurnPrice = State.market.getGoodPriceHistory(good)[turn - 2];
                Debug.Log("Price Last Turn: " + lastTurnPrice);

                if (currentTurnPrice > lastTurnPrice)
                {
                    priceChange.sprite = Resources.Load("Sprites/GUI/greenUp", typeof(Sprite)) as Sprite;
                }
                else if (currentTurnPrice < lastTurnPrice)
                {
                    priceChange.sprite = Resources.Load("Sprites/GUI/redDown", typeof(Sprite)) as Sprite;

                }
                else
                {
                    priceChange.sprite = Resources.Load("Sprites/GUI/flat", typeof(Sprite)) as Sprite;
                }
            }
            else
            {
                offeredLastTurn.text = "0";
                bidLastTurn.text = "0";
                soldLastTurn.text = "0";
                priceChange.sprite = Resources.Load("Sprites/GUI/flat", typeof(Sprite)) as Sprite;

            }
            priceHistory = market.getGoodPriceHistory(good);
        }
        updateMarketGraph(priceHistory);
}

    private void updateMarketGraph(List<float> priceHistory)
    {
        //  GraphChartBase graph = marketGraph.GetComponent<GraphChartBase>();
        marketGraph.DataSource.StartBatch();
        marketGraph.DataSource.ClearCategory("price");
        // graph.DataSource.ClearAndMakeBezierCurve("Player 2");
        for (int i = 1; i < priceHistory.Count; i++)
        {
            marketGraph.DataSource.AddPointToCategory("price", i, priceHistory[i]);
        }
        marketGraph.DataSource.EndBatch();
    }

    private void OnOfferButtonClick()
    {
        Debug.Log("Was clicked");
        App app = UnityEngine.Object.FindObjectOfType<App>();
        int playerIndex = app.GetHumanIndex();
        Nation player = State.getNations()[playerIndex];
        Market market = State.market;

        int turn = State.turn;
        int colIndex = 0;

        string currentButtonName = EventSystem.current.currentSelectedGameObject.name;
        Debug.Log(currentButtonName);
        MyEnum.Resources item = determineResButton(currentButtonName);
        TableRow bidButtons = marketTableList.Rows[1];
        TableRow bidAmounts = marketTableList.Rows[0];
        //  Text bidAmount = bidAmounts.Cells[colIndex].GetComponent<Text>();
        TableRow offerButtons = marketTableList.Rows[3];
        TableRow offerAmounts = marketTableList.Rows[4];

        //if gold, then we know that the item is in fact a good rather than a resoruce
        if (item != MyEnum.Resources.gold)
        {
            colIndex = getIndexOfResource(item);
            Debug.Log(item + " " + colIndex);
            int numBids = player.getNumberResourceBid(item);
            // In this case, we  are not really offering so much as decreasing the bid
            if (numBids > 0)
            {
                player.decrementResBid(item);
                float cost = 0;
                TextMeshProUGUI bidAmount =bidAmounts.Cells[colIndex].
                   GetComponentInChildren<TextMeshProUGUI>();
                bidAmount.text = player.getNumberResourceBid(item).ToString();
  
                cost = market.getPriceOfResource(item);
                player.increaseTotalCurrentBiddingCost(cost * -1);
                Debug.Log("Total bidding cost is now " + player.getTotalCurrentBiddingCost());
                // Debug.Log("Current Bid Cost " + player.getTotalCurrentBiddingCost());
                //update all bidding buttons based on cost of total bids
                updateBidButtonStatus();
                Button offerButton = offerButtons.Cells[colIndex].
                      GetComponentInChildren<Button>();
                if (player.getNumberResource(item) < 1 && 
                    player.getNumberResourceBid(item) == 8)
                {
                    offerButton.interactable = false;
                    
                }
                else
                {
                    offerButton.interactable = true;
                }
            }
            else if (numBids < 1)
            {
                player.incrementResOffers(item);
                TextMeshProUGUI offerAmount = offerAmounts.Cells[colIndex].
                   GetComponentInChildren<TextMeshProUGUI>();
                offerAmount.text = player.getNumberResourceOffers(item).ToString();

                //int itemIndex = getIndexOfResource(item);
                Button currentButton = offerButtons.Cells[colIndex].GetComponentInChildren<Button>();
                Debug.Log("Player has " + player.getNumberResource(item).ToString());
                // Debug.Log("Player has offered " + MarketHelper.ResourceOfferBidAmount[item]);
                if (player.getNumberResource(item) < 1)
                {
                    currentButton.interactable = false;
                }
                else
                {
                    currentButton.interactable = true;
                }
            }
        }
        else
        {
            MyEnum.Goods good = determineGoodButton(currentButtonName);
            int numBids = player.getNumberGoodBid(good);
            colIndex = getIndexOfGood(good);
          //  Debug.Log(good + " " + colIndex);
            if (numBids > 0)
            {
                player.decrementGoodBids(good);
                float cost = 0;
                TextMeshProUGUI bidAmount = bidAmounts.Cells[colIndex].
                GetComponentInChildren<TextMeshProUGUI>();
                bidAmount.text = player.getNumberGoodBid(good).ToString();

                cost = market.getPriceOfGood(good);
                player.increaseTotalCurrentBiddingCost(cost * -1);
                Debug.Log("Current Bid Cost " + player.getTotalCurrentBiddingCost());
                //update all bidding buttons based on cost of total bids
                updateBidButtonStatus();
                Button offerButton = offerButtons.Cells[colIndex].
                      GetComponentInChildren<Button>();
                if (player.getNumberGood(good) < 1 && player.getNumberGoodBid(good) < 1)
                {
                    offerButton.interactable = false;

                }
                else
                {
                    offerButton.interactable = true;
                }
            }
            if (numBids < 1)
            {

                player.incrementGoodOffers(good);
                TextMeshProUGUI offerAmount = offerAmounts.Cells[colIndex].
                   GetComponentInChildren<TextMeshProUGUI>();
                offerAmount.text = player.getNumberGoodsOffers(good).ToString();

                int itemIndex = getIndexOfGood(good);
                Button currentButton = offerButtons.Cells[itemIndex].GetComponentInChildren<Button>();
                Debug.Log("Player has offered " + player.getNumberGoodsOffers(good) + " " + good);
                if (player.getNumberGood(good) < 1)
                {
                    currentButton.interactable = false;
                }
                else
                {
                    currentButton.interactable = true;
                }
            }

        }
        uiUpdater.updateUI();

    }

    private bool isIndexResource()
    {
        int index = mostRecentColIndex;
        int numResources = Enum.GetValues(typeof(MyEnum.Resources)).Length;
        if(index <= numResources)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    
    private MyEnum.Resources determineResButton(string str)
    {
        if (str.ToUpper().Contains("WHEAT"))
        {
            return MyEnum.Resources.wheat;
        }
        else if (str.ToUpper().Contains("IRON"))
        {
            return MyEnum.Resources.iron;
        }
        else if (str.ToUpper().Contains("MEAT"))
        {
            return MyEnum.Resources.meat;
        }
        else if (str.ToUpper().Contains("FRUIT"))
        {
            return MyEnum.Resources.fruit;
        }
        else if (str.ToUpper().Contains("WOOD"))
        {
            return MyEnum.Resources.wood;
        }
        else if (str.ToUpper().Contains("COTTON"))
        {
            return MyEnum.Resources.cotton;
        }
        else if (str.ToUpper().Contains("COAL"))
        {
            return MyEnum.Resources.coal;
        }
        else if (str.ToUpper().Contains("SPICE"))
        {
            return MyEnum.Resources.spice;
        }
        else if (str.ToUpper().Contains("DYES"))
        {
            return MyEnum.Resources.dyes;
        }
        else if (str.ToUpper().Contains("RUBBER"))
        {
            return MyEnum.Resources.rubber;
        }
        else if (str.ToUpper().Contains("OIL"))
        {
            return MyEnum.Resources.oil;
        }
        else
        {
            return MyEnum.Resources.gold;
        }
    }

    private MyEnum.Goods determineGoodButton(string str)
    {
        if (str.ToUpper().Contains("STEEL"))
        {
            return MyEnum.Goods.steel;
        }
        else if (str.ToUpper().Contains("LUMBER"))
        {
            return MyEnum.Goods.lumber;
        }
        else if (str.ToUpper().Contains("FABRIC"))
        {
            return MyEnum.Goods.fabric;
        }
        else if (str.ToUpper().Contains("ARMS"))
        {
            return MyEnum.Goods.arms;
        }
        else if (str.ToUpper().Contains("PARTS"))
        {
            return MyEnum.Goods.parts;
        }
        else if (str.ToUpper().Contains("CLOTHING"))
        {
            return MyEnum.Goods.clothing;
        }
        else if (str.ToUpper().Contains("PAPER"))
        {
            return MyEnum.Goods.paper;
        }
        else if (str.ToUpper().Contains("FURNITURE"))
        {
            return MyEnum.Goods.furniture;
        }
        else if (str.ToUpper().Contains("CHEMICAL"))
        {
            return MyEnum.Goods.chemicals;
        }
        else if (str.ToUpper().Contains("GEAR"))
        {
            return MyEnum.Goods.gear;
        }
        else if (str.ToUpper().Contains("TELEPHONE"))
        {
            return MyEnum.Goods.telephone;
        }
        else if (str.ToUpper().Contains("AUTO"))
        {
            return MyEnum.Goods.auto;
        }
        else
        {
            return MyEnum.Goods.steel;
        }

    }

    private int getIndexOfResource(MyEnum.Resources res)
    {
        if (res == MyEnum.Resources.wheat)
        {
            return 0;
        }
        else if (res == MyEnum.Resources.meat)
        {
            return 1;
        }
        else if (res == MyEnum.Resources.fruit)
        {
            return 2;
        }
        else if (res == MyEnum.Resources.cotton)
        {
            return 3;
        }
        else if (res == MyEnum.Resources.iron)
        {
            return 4;
        }
        else if (res == MyEnum.Resources.wood)
        {
            return 5;
        }
        else if (res == MyEnum.Resources.coal)
        {
            return 6;
        }
        else if (res == MyEnum.Resources.spice)
        {
            return 7;
        }
        else if (res == MyEnum.Resources.dyes)
        {
            return 8;
        }
        else if (res == MyEnum.Resources.rubber)
        {
            return 9;
        }
        else if (res == MyEnum.Resources.oil)
        {
            return 10;
        }
        else
        {
            return -1;
        }

    }

    private int getIndexOfGood(MyEnum.Goods good)
    {
    
        if (good == MyEnum.Goods.lumber)
        {
            return 11;
        }
        else if(good == MyEnum.Goods.fabric)
        {
            return 12;
        }
        else if(good == MyEnum.Goods.steel)
        {
            return 13;
        }
        else if(good == MyEnum.Goods.clothing)
        {
            return 14;
        }
       else if(good == MyEnum.Goods.paper)
        {
            return 15;
        }
        else if(good == MyEnum.Goods.furniture)
        {
            return 16;
        }
       else  if(good == MyEnum.Goods.parts)
        {
            return 17;
        }
        if (good == MyEnum.Goods.arms)
        {
            return 18;
        }
       else if(good == MyEnum.Goods.chemicals)
        {
            return 19;
        }
        else if(good == MyEnum.Goods.gear)
        {
            return 20;
        }
        else if(good == MyEnum.Goods.telephone)
        {
            return 21;
        }
        else if (good == MyEnum.Goods.auto)
        {
            return 22;
        }
        else
        {
            return -1;
        }
    }

    private void revertToZeroResource(MyEnum.Resources resource)
    {
        App app = UnityEngine.Object.FindObjectOfType<App>();
        int playerIndex = app.GetHumanIndex();
        Nation player = State.getNations()[playerIndex];
        Market market = State.market;
        Debug.Log("Current State: " + MarketHelper.getResoueceOfferBid(resource));
        if (MarketHelper.getResoueceOfferBid(resource) == MyEnum.marketChoice.bid)
        {
            player.increaseTotalCurrentBiddingCost(-1 * MarketHelper.ResourceOfferBidAmount[resource] *
                market.getPriceOfResource(resource));
        }
        MarketHelper.ResourceOfferBidAmount[resource] = 0;
        MarketHelper.ResourceOfferBidLevel[resource] = MyEnum.OffBidLevels.medium;
        Debug.Log("Resouece Offer Bid Amount: " + MarketHelper.ResourceOfferBidAmount[resource].ToString());
       // numOffBid.text = MarketHelper.ResourceOfferBidAmount[resource].ToString();
    }

    private void revertToZeroGoods(MyEnum.Goods good)
    {
        App app = UnityEngine.Object.FindObjectOfType<App>();
        int playerIndex = app.GetHumanIndex();
        Nation player = State.getNations()[playerIndex];
        Market market = State.market;
        Debug.Log("Current State: " + MarketHelper.getGoodOfferBid(good));
        if (MarketHelper.getGoodOfferBid(good) == MyEnum.marketChoice.bid)
        {
            player.increaseTotalCurrentBiddingCost(-1 * MarketHelper.GoodsOfferBidAmount[good] *
                market.getPriceOfGood(good));
        }
        MarketHelper.GoodsOfferBidAmount[good] = 0;
        MarketHelper.GoodsOfferBidLevel[good] = MyEnum.OffBidLevels.medium;
        Debug.Log("Good Offer Bid Amount: " + MarketHelper.GoodsOfferBidAmount[good].ToString());
    }


    public void showDevelopmentPanel()
    {
        Debug.Log("Show development panel");
        if (diplomacyCamera.enabled)
        {
            otherNationAdditionalInformation.SetActive(false);

            DiplomacyTab.SetActive(false);
            diplomacyCamera.enabled = false;
            mainCamera.enabled = true;
        }
        if (coordinator.progressFlag == true)
        {
            coordinator.progressFlag = false;
            Debug.Log("progress flag false");
            progressExit.Play();
        }
        else
        {
            coordinator.progressFlag = true;
            Debug.Log("progress flag true");

            if (coordinator.relationsFlag == true)
            {
                coordinator.relationsFlag = false;
                otherNationAdditionalInformation.SetActive(false);
                relationsExit.Play();
            }
            if (coordinator.marketFlag == true)
            {
                coordinator.marketFlag = false;
                marketExit.Play();
            }
            if(coordinator.prodFlag == true)
            {
                coordinator.prodFlag = false;
                productionExit.Play();
            }
            if(coordinator.transportFlag == true)
            {
                coordinator.transportFlag = false;
                transportExit.Play();
            }
            
            developmentPanel.SetActive(true);
            updateDevelopmentPanel();
            progressEnter.Play();
        }
    }

    private void showMarketPanel()
    {
        if (diplomacyCamera.enabled)
        {
            otherNationAdditionalInformation.SetActive(false);

            DiplomacyTab.SetActive(false);
            diplomacyCamera.enabled = false;
            mainCamera.enabled = true;
        }
        if (coordinator.marketFlag == true)
        {
            coordinator.marketFlag = false;
            marketExit.Play();
        }
        else
        {
            coordinator.marketFlag = true;
            if(coordinator.relationsFlag == true)
            {
                coordinator.relationsFlag = false;
                otherNationAdditionalInformation.SetActive(false);

                relationsExit.Play();
            }
            if(coordinator.progressFlag == true)
            {
                coordinator.progressFlag = false;
                progressExit.Play();
            }
            if(coordinator.prodFlag == true)
            {
                coordinator.prodFlag = false;
                productionExit.Play();
            }
            if(coordinator.transportFlag == true)
            {
                coordinator.transportFlag = false;
                transportExit.Play();
            }
            updateMarketTab();
            MarketTab.SetActive(true);
            marketEnter.Play();
        }
    }

    public void showDiplomacyPanel()
    {
        Debug.Log("Clicked on Diplo button");
        diplomacyFromProv = false;
        if (mainCamera.enabled)
        {
            //    EmpireTab.SetActive(false);
            if (coordinator.marketFlag == true)
            {
                marketExit.Play();
                coordinator.marketFlag = false;
            }
            if(coordinator.progressFlag == true)
            {
                coordinator.progressFlag = false;
                progressExit.Play();
            }
            if(coordinator.prodFlag == true)
            {
                coordinator.prodFlag = false;
                productionExit.Play();
            }
            if(coordinator.transportFlag == true)
            {
                coordinator.transportFlag = false;
                transportExit.Play();
            }
            if (coordinator.relationsFlag == true)
            {
                otherNationAdditionalInformation.SetActive(false);

                coordinator.relationsFlag = false;
                relationsExit.Play();
            }

            UpdateDiplomacy();
            //  DataTab.SetActive(false);
            // ReportTab.SetActive(false);
            //  mapOptions.SetActive(false);
            Debug.Log("What is happening?");
            DiplomacyTab.SetActive(true);
            mainCamera.enabled = !mainCamera.enabled;
            diplomacyCamera.enabled = !diplomacyCamera.enabled;
            return;
        }
        else
        {
            Debug.Log("What is happening?");

            DiplomacyTab.SetActive(false);
            TreeViewItem[] humanOfferLists = negotiateHumanPanel.GetComponentsInChildren<TreeViewItem>();
            TreeViewItem[] aiOffersLists = negotiateAIPanel.GetComponentsInChildren<TreeViewItem>();
            for (int i = 0; i < humanOfferLists.Length; i++)
            {
                TreeViewItem item = humanOfferLists[i];
                item.Collapse();
            }
            for (int i = 0; i < aiOffersLists.Length; i++)
            {
                TreeViewItem item = aiOffersLists[i];
                item.Collapse();
            }
            mapOptions.SetActive(true);
            mainCamera.enabled = !mainCamera.enabled;
            diplomacyCamera.enabled = !diplomacyCamera.enabled;
            mapOptions.SetActive(true);
        }
    }

    private void showDiplomacyPanelProv()
    {
        {
            thisProvince.SetActive(false);
            otherPrivince.SetActive(false);
            Debug.Log("Clicked on Diplo button");
            diplomacyFromProv = true;
            if (DiplomacyTab.activeSelf == false || coordinator.diploFlag == false)
            {
            //    EmpireTab.SetActive(false);
                if (coordinator.marketFlag == true)
                {
                    marketExit.Play();
                    coordinator.marketFlag = false;
                }
            //    DataTab.SetActive(false);
                CreateNationList();
                UpdateDiplomacy();
                DiplomacyTab.SetActive(true);
               // diploEnterTop();
            }
            else
            {
                coordinator.diploFlag = false;
                diploExitTop();
              //  diplomacyExit.Play();
            }
        }
    }

    private void showProductionPanel()
    {
        if (diplomacyCamera.enabled)
        {
            otherNationAdditionalInformation.SetActive(false);

            DiplomacyTab.SetActive(false);
            diplomacyCamera.enabled = false;
            mainCamera.enabled = true;
        }
        if (coordinator.prodFlag == true)
        {
            coordinator.prodFlag = false;
            productionExit.Play();
        }
        else
        {
            coordinator.prodFlag = true;
            if (coordinator.relationsFlag == true)
            {
                otherNationAdditionalInformation.SetActive(false);

                coordinator.relationsFlag = false;
                relationsExit.Play();
            }

            if (coordinator.progressFlag == true)
            {
                coordinator.progressFlag = false;
                progressExit.Play();
            }
            if (coordinator.marketFlag == true)
            {
                coordinator.marketFlag = false;
                marketExit.Play();
            }
            if (coordinator.transportFlag == true)
            {
                coordinator.transportFlag = false;
                transportExit.Play();
            }
            thisProvince.SetActive(false);
            otherPrivince.SetActive(false);
            updateProductionPanel();
            productionPanel.SetActive(true);
            productionEnter.Play();
        }
    }

    private void showTransportPanel()
    {
        if (diplomacyCamera.enabled)
        {
            otherNationAdditionalInformation.SetActive(false);

            DiplomacyTab.SetActive(false);
            diplomacyCamera.enabled = false;
            mainCamera.enabled = true;
        }
        if (coordinator.transportFlag == true)
        {
            coordinator.transportFlag = false;
            transportExit.Play();
        }
        else
        {
            coordinator.transportFlag = true;

            if (coordinator.relationsFlag == true)
            {
                otherNationAdditionalInformation.SetActive(false);

                coordinator.relationsFlag = false;
                relationsExit.Play();
            }

            if (coordinator.progressFlag == true)
            {
                coordinator.progressFlag = false;
                progressExit.Play();
            }
            if (coordinator.marketFlag == true)
            {
                coordinator.marketFlag = false;
                marketExit.Play();
            }
            if (coordinator.prodFlag == true)
            {
                coordinator.prodFlag = false;
                productionExit.Play();
            }

            thisProvince.SetActive(false);
            otherPrivince.SetActive(false);

            Debug.Log("Show transport Panel");
            updateTransportPanel();
            transportPanel.SetActive(true);
            transportEnter.Play();
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

    private void updateMarketTab()
    {
        App app = UnityEngine.Object.FindObjectOfType<App>();
        int playerIndex = app.GetHumanIndex();
        Nation player = State.getNations()[playerIndex];
        Market market = State.market;

        selectedItemImage.sprite = 
            Resources.Load("Resource/wheat", typeof(Sprite)) as Sprite;
        Text imageName = selectedItemImage.GetComponentInChildren<Text>();
        imageName.text = "Wheat";

        selectedIntemInventory.text = "Inventory: " + player.getNumberResource(MyEnum.Resources.wheat).ToString();
        float prod = player.getResTransportFlow(MyEnum.Resources.wheat);
        selectdItemProducing.text = "Producing: " + prod;
        int turn = State.turn;
        if (turn == 1)
        {
            selectedItemPrice.text = "Price: 3";
            priceChange.sprite = 
                Resources.Load("Sprites/GUI/flat", typeof(Sprite)) as Sprite;
            bidLastTurn.text = "0";
            offeredLastTurn.text = "0";
            soldLastTurn.text = "0";
        }
        else
        {
            selectedItemPrice.text = 
                "Price: " + market.getPriceOfResource(MyEnum.Resources.wheat);
            float currentTurnPrice = market.getPriceOfResource(MyEnum.Resources.wheat);
            float lastTurnPrice = State.market.getResourcePriceHistory(MyEnum.Resources.wheat)[turn - 1];
            if (currentTurnPrice > lastTurnPrice)
            {
                priceChange.sprite = Resources.Load("Sprites/GUI/greenUp", typeof(Sprite)) as Sprite;
            }
            else if (currentTurnPrice < lastTurnPrice)
            {
                priceChange.sprite = Resources.Load("Sprites/GUI/redDown", typeof(Sprite)) as Sprite;

            }
            else
            {
                priceChange.sprite = Resources.Load("Sprites/GUI/flat", typeof(Sprite)) as Sprite;

            }
            bidLastTurn.text = 
                market.getNumberResourcesBid(MyEnum.Resources.wheat).ToString();
            offeredLastTurn.text =
                market.getNumberOfResourcesOffered(MyEnum.Resources.wheat).ToString();
            soldLastTurn.text =
                market.getNumberResourcesSold(MyEnum.Resources.wheat).ToString();
        }
        TableRow bidValueRow = marketTableList.Rows[0];
        TableRow bidButtons = marketTableList.Rows[1];
        TableRow iconButtons = marketTableList.Rows[2];
        TableRow offerButtons = marketTableList.Rows[3];
        TableRow offerValues = marketTableList.Rows[4];

        int colIndex = 0;
        foreach(MyEnum.Resources res in Enum.GetValues(typeof(MyEnum.Resources)))
        {
            if(res == MyEnum.Resources.gold)
            {
                continue;
            }
            Debug.Log(res + " " + colIndex);
            TextMeshProUGUI bidText = bidValueRow.Cells[colIndex].GetComponentInChildren<TextMeshProUGUI>();
            bidText.text = player.getNumberResourceBid(res).ToString();
            TextMeshProUGUI offerText = offerValues.Cells[colIndex].GetComponentInChildren<TextMeshProUGUI>();
            offerText.text = player.getNumberResourceOffers(res).ToString();
            Button offerButton = offerButtons.Cells[colIndex].GetComponentInChildren<Button>();
            Debug.Log("Res: " + res + " " +  player.getNumberResource(res) + " " + 
                player.getNumberResourceOffers(res));
            if ((player.getNumberResource(res) - player.getNumberResourceOffers(res) < 1) &&
                player.getNumberResourceBid(res) < 1)
            {
                offerButton.interactable = false;
            }
            else
            {
                offerButton.interactable = true;
            }
            colIndex++;
        }
        foreach (MyEnum.Goods good in Enum.GetValues(typeof(MyEnum.Goods)))
        {
            TextMeshProUGUI bidText = bidValueRow.Cells[colIndex].GetComponentInChildren<TextMeshProUGUI>();
            bidText.text = player.getNumberGoodBid(good).ToString();
            TextMeshProUGUI offerText = offerValues.Cells[colIndex].GetComponentInChildren<TextMeshProUGUI>();
            offerText.text = player.getNumberGoodsOffers(good).ToString();
            Button offerButton = offerButtons.Cells[colIndex].GetComponentInChildren<Button>();
            Debug.Log("Good: " + good + " " + player.getNumberGood(good) + " " +
               player.getNumberGoodsOffers(good));
            if ((player.getNumberGood(good) - player.getNumberGoodsOffers(good) < 1) &&
                player.getNumberGoodBid(good) < 1)
            {
                offerButton.interactable = false;
            }
            else
            {
              //  Debug.Log("Has some");
                offerButton.interactable = true;
            }
            colIndex++;
        }
        updateBidButtonStatus();
    }

    private void updateDevelopmentPanel()
    {
        viewAchievements.interactable = true;
        App app = UnityEngine.Object.FindObjectOfType<App>();
        int playerIndex = app.GetHumanIndex();
        Nation player = State.getNations()[playerIndex];

        currentAP.text = player.getAP().ToString();
        currentPP.text = player.getDP().ToString();
        corruption.text = player.Corruption.ToString();

        if (player.GetAPIncreasedThisTurn())
        {
            apCost.SetActive(false);
            apCostTwo.SetActive(true);
        }
        else
        {
            apCost.SetActive(true);
            apCostTwo.SetActive(false);
        }

        MyEnum.Era era = State.GerEra();
        if(era!= MyEnum.Era.Late)
        {
            earlyDPCost.SetActive(true);
            lateDPCost.SetActive(false);
        }
        else
        {
            earlyDPCost.SetActive(false);
            lateDPCost.SetActive(true);
        }

        devRP.text = player.Research.ToString();
        devIP.text = player.getIP().ToString();
        devStability.text = player.Stability.ToString("G");

        if (PlayerCalculator.canAddAP(player))
        {
            addAPButton.interactable = true;
        }
        else
        {
            addAPButton.interactable = false;
        }
        if (PlayerCalculator.canAddDP(player))
        {
            Debug.Log("Can add PP");
            addDPBUtton.interactable = true;
        }
        else
        {
            Debug.Log("Cannot add PP");
            addDPBUtton.interactable = false;
        }
        if (PlayerCalculator.canMakeDevelopmentAction(player) == true)
        {
            Debug.Log("Can make Development Actions");
            fundResearchButton.interactable = true;
            fundCultureButton.interactable = true;
            capitalInvestmentButton.interactable = true;
            fightCorruption.interactable = true;
        }
        else
        {
            Debug.Log("Cannot make Development Actions");
            fundResearchButton.interactable = false;
            fundCultureButton.interactable = false;
            capitalInvestmentButton.interactable = false;
            fightCorruption.interactable = false;
        }
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
        //int otherIndex = Int32.Parse(nationSelectorB.transform.parent.parent.name);
        //State.setCurrentSelectedNationDiplomacy(otherIndex);
        Debug.Log("clicked");
        diplomacyFromProv = false;
        nationHasBeenSelected = true;

        UpdateDiplomacy();
    }

   private void boycottNation()
    {
        App app = UnityEngine.Object.FindObjectOfType<App>();
        int playerIndex = app.GetHumanIndex();
        Nation player = State.getNations()[playerIndex];

        int aiIndex = State.getCurrentSlectedNationDiplomacy();
        Nation aiPlayer = State.getNations()[aiIndex];

        int relation = player.Relations[aiIndex];
        // otherNationLeader.ObjectPrefab = leaderPrefab.transform;
        //leaderAnimation = leaderPrefab.GetComponent<Animator>();

        if (player.Boycots.Contains(aiIndex))
        {
            player.Boycots.Remove(aiIndex);

            player.adjustRelation(aiPlayer, 5);
       
            otherNationDialogue.text = "We are happy that you have finally come around to see reason!";
            leaderAnimator.SetTrigger("clapping");

        }
        else
        {
            player.Boycots.Add(aiIndex);
            player.adjustRelation(aiPlayer, -15);
            // Maybe add minor relation damage to nations that are close to that nation AND ITS SPHERES!!
            otherNationDialogue.text = "Your arrogance will your downfall";
            leaderAnimator.SetTrigger("fustrated");
            aiPlayer.Boycots.Add(playerIndex);
        }
    }

    private void openNegotiations()
    {
        if (negotiateHumanPanel.activeSelf)
        {
            TextMeshProUGUI negButTxt = negotiationButton.GetComponentInChildren<TextMeshProUGUI>();
            negButTxt.text = "Propose Deal";
            negotiateHumanPanel.SetActive(false);
            negotiateAIPanel.SetActive(false);
        }
        else
        {
            TextMeshProUGUI negButTxt = negotiationButton.GetComponentInChildren<TextMeshProUGUI>();
            negButTxt.text = "Nevermind";
            negotiateHumanPanel.SetActive(true);
            negotiateAIPanel.SetActive(true);         
        }
    }

    private void makeOffer()
    {
        App app = UnityEngine.Object.FindObjectOfType<App>();
        int playerIndex = app.GetHumanIndex();
        Nation player = State.getNations()[playerIndex];

        Debug.Log("Make Offer");
        int aiIndex = State.getCurrentSlectedNationDiplomacy();
        Nation aiPlayer = State.getNations()[aiIndex];
        MyEnum.diploIntrepretation decision = aiPlayer.getAI().GetDiplomatic().respondToDealOffer(aiPlayer, State.deal);
        Debug.Log("Decision: " + decision);

        int relation = player.Relations[aiIndex];

        Deal deal = State.deal;
        //GameObject leaderPrefab = Instantiate(Resources.Load<GameObject>("Characters/Prefabs/" + aiPlayer.getNationName()));

       // otherNationLeader.ObjectPrefab = leaderPrefab.transform;
       // leaderAnimation = leaderPrefab.GetComponent<Animator>();

        if (decision == MyEnum.diploIntrepretation.demand)
        {
            otherNationDialogue.text = "What?!, your tardy demands are an insult to our empire!";
            ourRelations.text = relation.ToString();
            leaderAnimator.SetTrigger("yellingOut");
        }
        if(decision == MyEnum.diploIntrepretation.gift)
        {
            otherNationDialogue.text = "Oh, that is just what I needed! We are most flattered by your kind generousity";
            ourRelations.text = relation.ToString();
            deal.performDeal();
            //  leaderAnimation.Play("Clapping");
            leaderAnimator.SetTrigger("clapping");
        }
        if(decision == MyEnum.diploIntrepretation.badDeal)
        {
            otherNationDialogue.text = "I am afraid that that deal is not in our best interest at this time.";
            // leaderAnimation.Play("ThoughtfulHeadShake");
            leaderAnimator.SetTrigger("thoughtfulHeadShake");
        }
        if(decision == MyEnum.diploIntrepretation.goodDeal && relation > 75)
        {
            otherNationDialogue.text = "Splendid! It is always a pleasure doing business with you!";
            // leaderAnimation.Play("HeadNod");
            deal.performDeal();
            leaderAnimator.SetTrigger("headNod");


        }
        if (decision == MyEnum.diploIntrepretation.goodDeal && relation <= 75)
        {
            otherNationDialogue.text = "Agreed! This deal seems to be in the best interest of both your nation and ours.";
            deal.performDeal();
            // leaderAnimation.Play("ShakingHands");
            leaderAnimator.SetTrigger("shakingHands");
        }
        deal.clearDeal();
        PlayerSelectedItemTable.ClearRows();
        AIselectedItemTable.ClearRows();
        TextMeshProUGUI negButTxt = negotiationButton.GetComponentInChildren<TextMeshProUGUI>();
        negButTxt.text = "Propose Deal";
        TreeViewItem[] humanOfferLists = negotiateHumanPanel.GetComponentsInChildren<TreeViewItem>();
        TreeViewItem[] aiOffersLists = negotiateAIPanel.GetComponentsInChildren<TreeViewItem>();
        for(int i = 0; i < humanOfferLists.Length; i++)
        {
            TreeViewItem item = humanOfferLists[i];
            item.Collapse();
        }
        for (int i = 0; i < aiOffersLists.Length; i++)
        {
            TreeViewItem item = aiOffersLists[i];
            item.Collapse();
        }
        negotiateHumanPanel.SetActive(false);
        negotiateAIPanel.SetActive(false);
    }

    private void showGeneralInfoPanel()
    {
        Nation chosenNation = State.getNations()[State.getCurrentSlectedNationDiplomacy()];
        Debug.Log(chosenNation.getName());
        Debug.Log(chosenNation.culture);
        TextMeshProUGUI culture = generalInformation.Rows[0].Cells[1].GetComponentInChildren<TextMeshProUGUI>();
        culture.text = chosenNation.culture;
        TextMeshProUGUI prestige = generalInformation.Rows[1].Cells[1].GetComponentInChildren<TextMeshProUGUI>();
        prestige.text = chosenNation.getPrestige().ToString();
        TextMeshProUGUI gold = generalInformation.Rows[2].Cells[1].GetComponentInChildren<TextMeshProUGUI>();
        gold.text = Math.Round(chosenNation.getGold()).ToString();
        TextMeshProUGUI stability = generalInformation.Rows[3].Cells[1].GetComponentInChildren<TextMeshProUGUI>();
        stability.text = chosenNation.Stability.ToString();
        TextMeshProUGUI corruption = generalInformation.Rows[4].Cells[1].GetComponentInChildren<TextMeshProUGUI>();
        corruption.text = chosenNation.GetCorruption().ToString();
        TextMeshProUGUI technologies = generalInformation.Rows[5].Cells[1].GetComponentInChildren<TextMeshProUGUI>();
        technologies.text = chosenNation.GetTechnologies().Count.ToString();
        TextMeshProUGUI factories = generalInformation.Rows[6].Cells[1].GetComponentInChildren<TextMeshProUGUI>();
        factories.text = PlayerCalculator.getNumberOfFactories(chosenNation).ToString();
        TextMeshProUGUI railways = generalInformation.Rows[7].Cells[1].GetComponentInChildren<TextMeshProUGUI>();
        railways.text = chosenNation.industry.getNumberOfTrains().ToString();
          //  (chosenNation.industry.getTransportCapacity() - chosenNation.getProvinces().Count).ToString();
        TextMeshProUGUI provDev = generalInformation.Rows[8].Cells[1].GetComponentInChildren<TextMeshProUGUI>();
        provDev.text = PlayerCalculator.getNumberProvDevelopments(chosenNation).ToString();
        TextMeshProUGUI cultureCards = generalInformation.Rows[9].Cells[1].GetComponentInChildren<TextMeshProUGUI>();
        cultureCards.text = chosenNation.getCultureCards().Count.ToString();
        TextMeshProUGUI doctrines = generalInformation.Rows[10].Cells[1].GetComponentInChildren<TextMeshProUGUI>();
       doctrines.text = chosenNation.landForces.numberOfDoctrines().ToString();
        TextMeshProUGUI fleetSize = generalInformation.Rows[11].Cells[1].GetComponentInChildren<TextMeshProUGUI>();
        fleetSize.text = PlayerCalculator.getTotalNumberShips(chosenNation).ToString();
        Debug.Log("Number of Ships: " + PlayerCalculator.getTotalNumberShips(chosenNation));


        otherNationSpheres.SetActive(false);
        otherNationColonies.SetActive(false);
        otherNationGeneralInfo.SetActive(true);

    }

    private void showColoniesPanel()
    {
        Debug.Log("Show colonies panel");

        //  TreeView holdingsTree = otherNationHoldings.GetComponentInChildren<TreeView>();
        //  Debug.Log(holdingsTree.name);
        //  holdingsTree.GetComponent<HoldingsTree>().updateBoycottList();
        //  holdingsTree.GetComponent<HoldingsTree>().updateColonyList();
        //  holdingsTree.GetComponent<HoldingsTree>().updateSphereList();

        TableLayout colonyTable = otherNationColonies.GetComponentInChildren<TableLayout>();
        colonyTable.ClearRows();
        int aiIndex = State.getCurrentSlectedNationDiplomacy();
        Nation otherNation = State.getNations()[aiIndex];

        foreach (int colIndex in otherNation.getColonies())
        {
            Nation colony = State.getNations()[colIndex];
            Debug.Log("Has " + colony.getName());

            TableRow newRow = Instantiate<TableRow>(aiHolding);
            newRow.preferredHeight = 40;
            newRow.gameObject.SetActive(true);
            newRow.name = colony.getIndex().ToString();
            Text colName = newRow.Cells[0].GetComponentInChildren<Text>();
            colName.text = colony.getNationName().ToString();
            UIObject3D flag = newRow.Cells[1].GetComponentInChildren<UIObject3D>();
            GameObject flagPrefab = Instantiate(Resources.Load<GameObject>("Flags/Prefabs/" + colony.getNationName()));
            flag.ObjectPrefab = flagPrefab.transform;
            colonyTable.AddRow(newRow);
        }

        otherNationColonies.SetActive(true);
        otherNationSpheres.SetActive(false);
        otherNationGeneralInfo.SetActive(false);
    }

    private void showSpheresPanel()
    {
        Debug.Log("Show spheres panel");
        TableLayout sphereTable = otherNationSpheres.GetComponentInChildren<TableLayout>();
        sphereTable.ClearRows();
        int aiIndex = State.getCurrentSlectedNationDiplomacy();
        Nation otherNation = State.getNations()[aiIndex];

        foreach (int sphereIndex in otherNation.getSpheres())
        {
            Nation sphere = State.getNations()[sphereIndex];
            Debug.Log("Has " + sphere.getName());
            TableRow newRow = Instantiate<TableRow>(aiHolding);
            newRow.preferredHeight = 40;
            newRow.gameObject.SetActive(true);
            newRow.name = sphere.getIndex().ToString();
            Text colName = newRow.Cells[0].GetComponentInChildren<Text>();
            colName.text = sphere.getNationName().ToString();
            UIObject3D flag = newRow.Cells[1].GetComponentInChildren<UIObject3D>();
            GameObject flagPrefab = Instantiate(Resources.Load<GameObject>("Flags/Prefabs/" + sphere.getNationName()));
            flag.ObjectPrefab = flagPrefab.transform;
            sphereTable.AddRow(newRow);
        }
        otherNationColonies.SetActive(false);
        otherNationSpheres.SetActive(true);
        otherNationGeneralInfo.SetActive(false);
    }

    private void showAgreementsPanel()
    {

        otherNationTheyRecognize.InitView();
        theyRecognizeTheseSpheres = otherNationTheyRecognize.AppendItem("itemType");
        Text theyRecoconizeTheseSpheresText = theyRecognizeTheseSpheres.GetComponentInChildren<Text>();
        theyRecoconizeTheseSpheresText.text = "Spheres";
        Button theyRecognizeTheseSpheresButton = theyRecognizeTheseSpheres.GetComponentInChildren<Button>();
        theyRecognizeTheseSpheresButton.onClick.AddListener(delegate { expandTheirSphereRecognitions(); });

        theyRecognizeTheseColonies = otherNationTheyRecognize.AppendItem("itemType");
        Text theyRecognizeTheseColoniesText = theyRecognizeTheseColonies.GetComponentInChildren<Text>();
        theyRecognizeTheseColoniesText.text = "Colonies";
        Button theyRecognizeTheseColoniesButton = theyRecognizeTheseColonies.GetComponentInChildren<Button>();
        theyRecognizeTheseColoniesButton.onClick.AddListener(delegate { expandTheirColonyRecognitions(); });

        theyRecognizeTheseProvinces = otherNationTheyRecognize.AppendItem("itemType");
        Text theyRecognizeTheseProvincesText = theyRecognizeTheseProvinces.GetComponentInChildren<Text>();
        theyRecognizeTheseProvincesText.text = "Provinces";
        Button theyRecognizeTheseProvincesButton = theyRecognizeTheseProvinces.GetComponentInChildren<Button>();
        theyRecognizeTheseProvincesButton.onClick.AddListener(delegate { expandTheirProvinceRecognitions(); });

        otherNationWeRecognize.InitView();
        weRecognizeTheseSpheres = otherNationWeRecognize.AppendItem("itemType");
        Text weRecoconizeTheseSpheresText = weRecognizeTheseSpheres.GetComponentInChildren<Text>();
        weRecoconizeTheseSpheresText.text = "Spheres";
        Button weRecognizeTheseSpheresButton = weRecognizeTheseSpheres.GetComponentInChildren<Button>();
        weRecognizeTheseSpheresButton.onClick.AddListener(delegate { expandOurSphereRecognitions(); });

        weRecognizeTheseColonies = otherNationWeRecognize.AppendItem("itemType");
        Text weRecognizeTheseColoniesText = weRecognizeTheseColonies.GetComponentInChildren<Text>();
        weRecognizeTheseColoniesText.text = "Colonies";
        Button weRecognizeTheseColoniesButton = weRecognizeTheseColonies.GetComponentInChildren<Button>();
        weRecognizeTheseColoniesButton.onClick.AddListener(delegate { expandOurColonyRecognitions(); });

        weRecognizeTheseProvinces = otherNationWeRecognize.AppendItem("itemType");
        Text weRecognizeTheseProvincesText = weRecognizeTheseProvinces.GetComponentInChildren<Text>();
        weRecognizeTheseProvincesText.text = "Provinces";
        Button weRecognizeTheseProvincesButton = weRecognizeTheseProvinces.GetComponentInChildren<Button>();
        weRecognizeTheseProvincesButton.onClick.AddListener(delegate { expandOurProvinceRecognitions(); });

        UpdateOurColonyRecognitions();
        UpdateOurSphereRecognitions();
        UpdateTheirColonyRecognitions();
        UpdateTheirSphereRecognitions();

        otherNationColonies.SetActive(false);
        otherNationGeneralInfo.SetActive(false);
        otherNationSpheres.SetActive(true);

    }

    private void UpdateTheirSphereRecognitions()
    {
        int aiIndex = State.getCurrentSlectedNationDiplomacy();
        theyRecognizeTheseSpheres.Init();
        int j = 0;
        App app = UnityEngine.Object.FindObjectOfType<App>();
        int humanIndex = app.GetHumanIndex();
        Nation ai = State.getNations()[aiIndex];
        int relation = ai.Relations[humanIndex];

        HashSet<int> recognizing = ai.RecognizingTheseClaims;
        foreach (int item in recognizing)
        {
            Nation nation = State.getNations()[item];
            if(nation.getType() != MyEnum.NationType.minor)
            {
                continue;
            }
            TreeViewItem newItem = theyRecognizeTheseSpheres.ChildTree.AppendItem("holdingItem");
            Text newItemText = newItem.GetComponentInChildren<Text>();
            newItemText.text = nation.getNationName();
            newItem.ItemIndex = j;
            j++;
        }
    }

    private void UpdateTheirColonyRecognitions()
    {
        theyRecognizeTheseColonies.Init();
        int j = 0;
        App app = UnityEngine.Object.FindObjectOfType<App>();
        int humanIndex = app.GetHumanIndex();
        int aiIndex = State.getCurrentSlectedNationDiplomacy();
        Nation ai = State.getNations()[aiIndex];
        int relation = ai.Relations[humanIndex];

        HashSet<int> recognizing = ai.RecognizingTheseClaims;

        foreach (int item in recognizing)
        {
            Nation nation = State.getNations()[item];
            if (nation.getType() != MyEnum.NationType.oldMinor)
            {
                continue;
            }
            TreeViewItem newItem = theyRecognizeTheseColonies.ChildTree.AppendItem("holdingItem");
            Text newItemText = newItem.GetComponentInChildren<Text>();
            newItemText.text = nation.getNationName();
            newItem.ItemIndex = j;
            j++;
        }
    }

    private void UpdateOurSphereRecognitions()
    {
        weRecognizeTheseSpheres.Init();
        int j = 0;
        App app = UnityEngine.Object.FindObjectOfType<App>();
        int humanIndex = app.GetHumanIndex();
        int aiIndex = State.getCurrentSlectedNationDiplomacy();
        //Nation ai = State.getNations()[selectedAI_ID];
        Nation human = State.getNations()[humanIndex];
        int relation = human.Relations[humanIndex];

        HashSet<int> recognizing = human.RecognizingTheseClaims;

        foreach (int item in recognizing)
        {
            Nation nation = State.getNations()[item];
            if (nation.getType() != MyEnum.NationType.minor)
            {
                continue;
            }
            TreeViewItem newItem = weRecognizeTheseSpheres.ChildTree.AppendItem("holdingItem");
            Text newItemText = newItem.GetComponentInChildren<Text>();
            newItemText.text = nation.getNationName();
            newItem.ItemIndex = j;
            j++;
        }
    }

    private void UpdateOurColonyRecognitions()
    {
        TreeViewItem weRecognizeTheseColonies = otherNationWeRecognize.AppendItem("itemType");
        Text weRecognizeTheseColoniesText = weRecognizeTheseColonies.GetComponentInChildren<Text>();
        weRecognizeTheseColoniesText.text = "Colonies";
        Button weRecognizeTheseColoniesButton = weRecognizeTheseColonies.GetComponentInChildren<Button>();
        weRecognizeTheseColoniesButton.onClick.AddListener(delegate { expandOurColonyRecognitions(); });
        weRecognizeTheseColonies.Init();
        int j = 0;
        App app = UnityEngine.Object.FindObjectOfType<App>();
        int humanIndex = app.GetHumanIndex();
        int aiIndex = State.getCurrentSlectedNationDiplomacy();

        Nation human = State.getNations()[humanIndex];
        int relation = human.Relations[humanIndex];

        HashSet<int> recognizing = human.RecognizingTheseClaims;

        foreach (int item in recognizing)
        {
            Nation nation = State.getNations()[item];
            if (nation.getType() != MyEnum.NationType.oldMinor)
            {
                continue;
            }
            TreeViewItem newItem = weRecognizeTheseColonies.ChildTree.AppendItem("holdingItem");
            Text newItemText = newItem.GetComponentInChildren<Text>();
            newItemText.text = nation.getNationName();
            newItem.ItemIndex = j;
            j++;
        }
    }

    private int findFirstNonHumanPlayer()
    {
        App app = UnityEngine.Object.FindObjectOfType<App>();
        int playerIndex = app.GetHumanIndex();
        for(int i = 0; i < State.getNations().Count; i++)
        {
            if (i == playerIndex)
            {
                continue;
            }
            else
            {
                Nation nation = State.getNations()[i];
                if(nation.getType() == MyEnum.NationType.major)
                {
                    return i;
                }
            }
        }
        return -1;
    }

    private void CreateNationList()
    {
        App app = UnityEngine.Object.FindObjectOfType<App>();
        int playerIndex = app.GetHumanIndex();
        // Nation player = State.getNations()[playerIndex];
        nationList.ClearRows();
        Debug.Log("Human Player Index: " + playerIndex + "       BBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBB");
        foreach (int nationIndex in State.getMajorNations())
        {
            Nation nation = State.getNations()[nationIndex];
           // Debug.Log("Nation Index " +  nation.getIndex()   + " Nation name " + nation.getName());
           
            if(nation.getIndex() == playerIndex)
            {
               // Debug.Log("If 19 should skip!!");
                continue;
            }
         //   currentNationSelected.text = nation.getName();
            TableRow newRow = Instantiate<TableRow>(nationRow);
            newRow.gameObject.SetActive(true);
            //    newRow.preferredHeight = 30;
            newRow.name = nation.getIndex().ToString();
         //   Debug.Log("Row Name:" + newRow.name);
            UIObject3D flag = newRow.Cells[0].GetComponentInChildren<UIObject3D>();
            string nationName = nation.getName();
            // GameObject threeDObject = Instantiate(Resources.Load<GameObject>("Flags/Prefabs/" + nationName));
            Debug.Log("Nation Index = " + nation.getIndex() + " --------------");
            flag.LightIntensity = 0.7f;

            if (nation.getIndex() == 2)
            {
                flag.ObjectPrefab = bambakiFlag;
                //otherNationLeader.ObjectPrefab = aerakaraLeader;
            }
            else if (nation.getIndex() == 7)
            {
                flag.ObjectPrefab = sitariFlag;
                //  otherNationLeader.ObjectPrefab = an_rioLeader;

            }
            else if (nation.getIndex() == 9)
            {
                flag.ObjectPrefab = chaldeaFlag;
                //  otherNationLeader.ObjectPrefab = ditealerLeader;

            }
            else if (nation.getIndex() == 18)
            {
                flag.ObjectPrefab = wyvermountFlag;
                // otherNationLeader.ObjectPrefab = crystaliceLeader;

            }
            else if (nation.getIndex() == 25)
            {
                flag.ObjectPrefab = sideroFlag;
                // otherNationLeader.ObjectPrefab = dessakLeader;
            }
            else if (nation.getIndex() == 27)
            {
                flag.ObjectPrefab = boreoisFlag;
                // otherNationLeader.ObjectPrefab = feandraLeader;
            }
            //flag.ObjectPrefab = threeDObject.transform;
            Button flagButton = newRow.Cells[0].GetComponentInChildren<Button>();
            flagButton.name = nation.getName();
            flagButton.onClick.AddListener(delegate { SelectNationButton(); });
            nationList.AddRow(newRow);

        }
        //  scrollviewContent.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (nationList.transform as RectTransform).rect.height);
    }

    private void SelectNationButton()
    {
        Debug.Log("I am clicked");
        String nationName = EventSystem.current.currentSelectedGameObject.name;
        Debug.Log("Nation Name is: " + nationName);
        Nation clickedNation = State.GetNationByName(nationName);
        State.setCurrentSelectedNationDiplomacy(clickedNation.getIndex());
        UpdateDiplomacy();
        nationHasBeenSelected = true;
        diplomacyFromProv = false;
        TreeViewItem[] humanOfferLists = negotiateHumanPanel.GetComponentsInChildren<TreeViewItem>();
        TreeViewItem[] aiOffersLists = negotiateAIPanel.GetComponentsInChildren<TreeViewItem>();
        for (int i = 0; i < humanOfferLists.Length; i++)
        {
            TreeViewItem item = humanOfferLists[i];
            item.Collapse();
        }
        for (int i = 0; i < aiOffersLists.Length; i++)
        {
            TreeViewItem item = aiOffersLists[i];
            item.Collapse();
        }
        //  String nationName1 = EventSystem.current.currentSelectedGameObject.name;
        //  Nation clickedNation1 = State.GetNationByName(nationName1);
        // UpdateDiplomacy(); 
    }

    private void UpdateDiplomacy()
    {
        Deal deal = State.deal;
        deal.clearDeal();
        TextMeshProUGUI negButTxt = negotiationButton.GetComponentInChildren<TextMeshProUGUI>();
        negButTxt.text = "Propose Deal";
        negotiateHumanPanel.SetActive(false);
        negotiateAIPanel.SetActive(false);
        PlayerSelectedItemTable.ClearRows();
        AIselectedItemTable.ClearRows();

        App app = UnityEngine.Object.FindObjectOfType<App>();
        int playerIndex = app.GetHumanIndex();
        Nation player = State.getNations()[playerIndex];
        int firstNonHumanMajor = findFirstNonHumanPlayer();
        Nation chosenNation = State.getNations()[firstNonHumanMajor];

        if (State.getMajorNations().Contains(State.getCurrentSlectedNationDiplomacy()))
        {
            chosenNation = State.getNations()[State.getCurrentSlectedNationDiplomacy()];
        }
        Debug.Log("Chosen nation: " + chosenNation.getNationName() + " index: " + chosenNation.getIndex());
        
        //Otherwise, the diplomacy panel has just been opened, so no specific nation has been selected, show default

        hideAllLeaders();
        int chosenNationIndex = chosenNation.getIndex();

        if (chosenNationIndex == 2)
        {
            bambakiLeader.SetActive(true);
            leaderAnimator = bambakiLeader.GetComponent<Animator>();
            bambakiLeader.transform.position = leaderPosition;
            bambakiLeader.transform.rotation = leaderRotation;

        }
        else if (chosenNationIndex == 7)
        {
            sitariLeader.SetActive(true);
            leaderAnimator = sitariLeader.GetComponent<Animator>();
            sitariLeader.transform.position = leaderPosition;
            sitariLeader.transform.rotation = leaderRotation;
        }
        else if (chosenNationIndex == 9)
        {
            chaldeaLeader.SetActive(true);
            leaderAnimator = chaldeaLeader.GetComponent<Animator>();
            chaldeaLeader.transform.position = leaderPosition;
            chaldeaLeader.transform.rotation = leaderRotation;
        }
        else if (chosenNationIndex == 18)
        {
            wyvermountLeader.SetActive(true);
            leaderAnimator = wyvermountLeader.GetComponent<Animator>();
            wyvermountLeader.transform.position = leaderPosition;
            wyvermountLeader.transform.rotation = leaderRotation;
        }
        else if (chosenNationIndex == 25)
        {
            sideroLeader.SetActive(true);
            leaderAnimator = sideroLeader.GetComponent<Animator>();
            sideroLeader.transform.position = leaderPosition;
            sideroLeader.transform.rotation = leaderRotation;

        }
        else if (chosenNationIndex == 27)
        {
            boreoisLeader.SetActive(true);
            leaderAnimator = boreoisLeader.GetComponent<Animator>();
            boreoisLeader.transform.position = leaderPosition;
            boreoisLeader.transform.rotation = leaderRotation;
        }
        Debug.Log(leaderAnimator.name);

        TextMeshProUGUI boycttButtonText = boycottButton.GetComponentInChildren<TextMeshProUGUI>();

       
        int relation = player.Relations[chosenNationIndex];

        if (chosenNation.Boycots.Contains(playerIndex))
        {
            boycttButtonText.text = "Under Boycott";
            boycottButton.interactable = false;
        }

        if (player.Boycots.Contains(chosenNationIndex))
        {
            boycttButtonText.text = "End Boycott";
            boycottButton.interactable = true;
        }
        else
        {
            boycttButtonText.text = "Boycott";
            boycottButton.interactable = true;
        }

        otherNationName.text = chosenNation.getNationName().ToString();
        Debug.Log("Nation Index = " + chosenNation.getIndex() + " " + chosenNation.getNationName());

        GameObject flagPrefab = Instantiate(Resources.Load<GameObject>("Flags/Prefabs/" + chosenNation.getNationName()));
        otherNationFlag.ObjectPrefab = flagPrefab.transform;
        // otherNationFlag.TargetOffset = new Vector2(0, 0);
       // leaderPrefab = Instantiate(Resources.Load<GameObject>("Characters/Prefabs/" + chosenNation.getNationName()));
       // Debug.Log(leaderPrefab.name);
       // otherNationLeader.ObjectPrefab = leaderPrefab.transform;
       // leaderAnimation = leaderPrefab.GetComponent<Animator>();
  
        ourRelations.text = relation.ToString();
        if (relation < 20)
        {
            ourRelations.color = Color.red;
            otherNationDialogue.text = "I was hoping I would not have to ever see you again.";
            //  leaderAnimation.Play("Taunt");
            leaderAnimator.SetTrigger("taunt");

        }
        else if (relation < 35)
        {
            ourRelations.color = new Color(1.0f, 0.64f, 0.0f);
            otherNationDialogue.text = "What do you want?";
            //  leaderAnimation.Play("Angry Gesture");
            leaderAnimator.SetTrigger("angryGesture");

        }
        else if (relation < 60)
        {
            ourRelations.color = Color.yellow;
            otherNationDialogue.text = "Greetings, what matter of business would you like to discuss on this fine day?";
            // leaderAnimation.Play("breathingIdle");
            leaderAnimator.SetTrigger("breathingIdle");
        }
        else if (relation < 80)
        {
            ourRelations.color = Color.green;
            otherNationDialogue.text = "Greetings and salutations, my good friend, I anticipate you have some mutually benificial proposal in mind?";
            // leaderAnimation.Play("formalBow");
            leaderAnimator.SetTrigger("formalBow");
        }
        else
        {
            ourRelations.color = Color.blue;
            otherNationDialogue.text = "It is always a joy to see you! Please, let us sip on this fine congac while we discuss matters of state.";
            //  leaderAnimation.Play("Clapping");
            leaderAnimator.SetTrigger("clapping");
        }
        //lnationFlag.sprite = Resources.Load("Flags/" + chosenNation.getNationName().ToString(), typeof(Sprite)) as Sprite;

        showGeneralInfoPanel();

        /*   float nationStab = chosenNation.Stability;
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
           } */
    }

    private void updateProductionPanel()
    {
        App app = UnityEngine.Object.FindObjectOfType<App>();
        Nation player = State.getNations()[app.GetHumanIndex()];
        int size = productionTable.Rows.Count;
        int index = 0;
        foreach(MyEnum.Goods good in Enum.GetValues(typeof(MyEnum.Goods)))
        {
            Debug.Log("Production panel " + good);
            TableRow row = productionTable.Rows[index];
            Slider productionSlider = row.Cells[1].GetComponentInChildren<Slider>();
            float ableToProduce = player.industry.determineCanProduce(good, player);
            //canProduce.text = ableToProduce.ToString();
            Debug.Log("Able to produce " + ableToProduce + " " + good);
            Debug.Log("Currently Producing ------------------- " + player.industry.getGoodProducing(good));
            productionSlider.maxValue = (float)Math.Floor(ableToProduce + player.industry.getGoodProducing(good));
            int sliderValue = player.industry.getGoodProducing(good);
            productionSlider.value = sliderValue;

            Text currentValMax = row.Cells[2].GetComponentInChildren<Text>();
            currentValMax.text = productionSlider.value + "/" + (productionSlider.maxValue);

            Button produceButton = row.Cells[4].GetComponentInChildren<Button>();

            Image factoryLevelImage = row.Cells[5].GetComponentInChildren<Image>();
            Text factorylevelText = factoryLevelImage.GetComponentInChildren<Text>();

            if (player.getFactoryLevel(good) == 0)
            {
                factoryLevelImage.sprite = Resources.Load("Sprites/GUI/workshop", typeof(Sprite)) as Sprite;
                factorylevelText.text = "0";
            }

            else if (player.getFactoryLevel(good) == 1)
            {
                Debug.Log("Level One");
                factoryLevelImage.sprite = Resources.Load("Sprites/GUI/factorySmall", typeof(Sprite)) as Sprite;
                factorylevelText.text = "1";
            }

            else if (player.getFactoryLevel(good) == 2)
            {
                factoryLevelImage.sprite = Resources.Load("Assets/Resources/Sprites/GUI/factoryMedium", typeof(Sprite)) as Sprite;
                factorylevelText.text = "2";
            }

            if (productionSlider.maxValue >= 1 && player.industry.getGoodProducing(good) == 0)
            {
                productionSlider.interactable = true;
                produceButton.interactable = true;
                Text produceButtonText = produceButton.GetComponentInChildren<Text>();
                GameObject parent = produceButton.transform.parent.gameObject;
                Text buttonText = parent.GetComponentInChildren<Text>();
                buttonText.text = "Produce";
            }
            else
            {       
                productionSlider.interactable = false;
                produceButton.interactable = true;
                Text produceButtonText = produceButton.GetComponentInChildren<Text>();
                GameObject parent = produceButton.transform.parent.gameObject;
                Text buttonText = parent.GetComponentInChildren<Text>();
                buttonText.text = "Cancel";
                if (player.industry.getGoodProducing(good) > 0)
                {             
                    buttonText.text = "Cancel";
                    produceButton.interactable = true;
                }
                else
                {
                    buttonText.text = "Produce";
                    produceButton.interactable = false;
                }
            }

            int factoryLevel = player.getFactoryLevel(good);

           // factoryLevelImage = row.Cells[4].GetComponentInChildren<Image>();
            if (factoryLevel == 0)
            {
                //canProduce.text = ableToProduce.ToString();
                factoryLevelImage.sprite = Resources.Load("Sprites/GUI/workshop",
                    typeof(Sprite)) as Sprite;
            }
            if (factoryLevel == 1)
            {
                factoryLevelImage.sprite = Resources.Load("Sprites/GUI/factorySmall", typeof(Sprite)) as Sprite;
            }
            if (factoryLevel == 2)
            {
                factoryLevelImage.sprite = Resources.Load("Sprites/GUI/factoryMedium", typeof(Sprite)) as Sprite;
            }
            if (factoryLevel == 3)
            {
                factoryLevelImage.sprite = Resources.Load("Sprites/GUI/FactoryBig", typeof(Sprite)) as Sprite;
            }

            bool upgradeCheck = player.industry.CheckIfCanUpgradeFactory(player, good);

            Button upgrade = row.Cells[6].GetComponentInChildren<Button>();
            if (upgradeCheck == true)
            {
                Debug.Log("Can upgrade");
                upgrade.interactable = true;
            }
            else
            {
                Debug.Log("Cannot upgrade");
                upgrade.interactable = false;
            }
          //  TableLayout finalCell = row.Cells[6].GetComponentInChildren<TableLayout>();
          //  Text marketPrice = finalCell.Rows[0].GetComponentInChildren<Text>();
          //  Text productionCost = finalCell.Rows[1].GetComponentInChildren<Text>();

         //   marketPrice.text = "Market Price: " + State.market.getPriceOfGood(good).ToString("0.0");
          //  float costToProduce = PlayerCalculator.getProductionCost(player, good);
         //   productionCost.text = "Production Cost" + costToProduce.ToString("0.0");
            index++;
        }
    }

    private void alterEmbargo()
    {
        App app = UnityEngine.Object.FindObjectOfType<App>();
        int playerIndex = app.GetHumanIndex();
        Nation player = State.getNations()[playerIndex];
       int  otherIndex = State.getCurrentSlectedNationDiplomacy();

        int relation = player.Relations[otherIndex];
        Nation otherNation = State.getNations()[otherIndex];
       // diplomaticResponseFlag.sprite = Resources.Load("Flags/" + otherNation.getNationName().ToString(), typeof(Sprite)) as Sprite;
      //  diploResponsePic.sprite = Resources.Load("Sprites/Diplomat1", typeof(Sprite)) as Sprite;

        if (!player.Boycots.Contains(otherIndex))
        {
            player.Boycots.Add(otherIndex);
            player.adjustRelation(otherNation, -15);
         //   responseText.text = " We find your decision to forego trade relations between our mighty nations is disturbing. It is all too easy" +
           //     "for way by way of coin to escalate to war by way of arms.";
        }
        else
        {
            player.Boycots.Remove(otherIndex);
            player.adjustRelation(otherNation, 5);
        //    responseText.text = " We see you have finally come to your senses and recognized that freedom of trade benefits us all. Let us pray " +
           //     "that the damage caused to our friendship is soon undone through mutual cooperation and prosperity.";
        }
     //   updateDiplomacyOptionButtons(player, otherNation);
     //   diplomaticResponse.SetActive(true);
    }

    private void negotiateNation()
    {

    }

    private void UpdateDataTab()
    {

    }
    
    private void updateReportTab()
    {

    }

    private void UpdateCityView()
    {
      //  var myNewSmoke = Instantiate(poisonSmoke, Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
       // myNewSmoke.transform.parent = gameObject.transform;
        MyEnum.Era era = State.era;
        if(era == MyEnum.Era.Early)
        {

        }
    }

    private void openNavalPanel()
    {
        developmentPanel.SetActive(false);
        coordinator.progressFlag = false;
        Debug.Log("Should open Naval Panel -------------------------------------");
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
        SeaForces seaForces = player.seaForces;

        Text frigateAttack = navyTable.Rows[1].Cells[1].GetComponentInChildren<Text>();
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

        Debug.Log("Number Frigates: " + player.seaForces.frigate.getNumber());
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
        Debug.Log("Current Era is " + State.GerEra());
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

    private void showMenuPanel()
    {
        if (MenuPanel.activeSelf)
        {
            MenuPanel.SetActive(false);
        }
        else
        {
            MenuPanel.SetActive(true);
        }
    }

    private void updateTransportPanel()
    {
        Debug.Log("Begin Updating Transport Panel");
        App app = UnityEngine.Object.FindObjectOfType<App>();
        Nation player = State.getNations()[app.GetHumanIndex()];
        int transportFlow = PlayerCalculator.calculateTransportFlow(player);
        int totalNumberProv = PlayerCalculator.getTotalNumberOfProvinces(player);
        int realTransportCapacity = PlayerCalculator.calculateMaxTransportFlow(player);
        Debug.Log("Transport Flow: " + transportFlow);
        Debug.Log("Current Transport Capacity: " + realTransportCapacity);
        // player.industry.setTransportFlow(PlayerCalculator.calculateTransportFlow(player));
        float coalUsedForTransport = (transportFlow - totalNumberProv) * 0.2f;
        transportCapacity.text = transportFlow.ToString() + "/" + realTransportCapacity.ToString();
        coalCapacity.text = player.getNumberResource(MyEnum.Resources.coal).ToString();
        int remainingFlow = realTransportCapacity - transportFlow;
        int numberOfTrains = player.industry.getNumberOfTrains();
        int numberOfTrainsAva = player.industry.getUnusedTrainCapacity(player);
      //  Debug.Log("Remaining Flow Capacity: " + remainingFlow);

        if (PlayerCalculator.canBuildTrain(player))
        {
            addTransportCapacityButton.interactable = true;
        }
        else
        {
            addTransportCapacityButton.interactable = false;
        }

        // Counter used for switching from first to second table in Production GUI
        int counter = 0;
        foreach (MyEnum.Resources res in Enum.GetValues(typeof(MyEnum.Resources)))
        {
            int producing = PlayerCalculator.getResourceProducing(player, res);
            int numberProvProducing = PlayerCalculator.getNumberProvicesProducingRes(player, res);
            int currentFlow = player.getResTransportFlow(res);
            Debug.Log("Resource is: " + res + "---------------------------------------------");
            Debug.Log("Producing: " + producing);
            Debug.Log("Current Flow: " + currentFlow);
            //int systemFlow = player.industry.getTransportFlow();
            // Capacity first calculated as numberOfProv producing that good and current flow - this is a garenteed capacity
            int capacity1 = 0;
            if(currentFlow < numberProvProducing)
            {
                capacity1 = numberProvProducing - currentFlow;
            }
            // Flow might be greater, however, if
            int  capacity2 = Math.Min(producing - currentFlow, numberOfTrainsAva);

            int capacity = Math.Max(capacity1, capacity2);

            // capacity = Math.Min(producing - currentFlow, numberOfTrainsAva);
            // capacity = Math.Max(capacity, numberProvProducing);
            Debug.Log("Number of Trains:" + player.industry.getNumberOfTrains());
            Debug.Log("Number of Trains Aval: " + numberOfTrainsAva);
            Debug.Log("Remaining " + res + " capacity: " + capacity);
            int max = currentFlow + capacity;
            Debug.Log("Max: " + max);
            if (counter <= 5)
            {
                Text resFlow = resourceTableA.Rows[counter].Cells[1].GetComponentInChildren<Text>();
                Slider resSlider = resourceTableA.Rows[counter].Cells[2].GetComponentInChildren<Slider>();
                resFlow.text = currentFlow + "/" + producing;
                if (max == currentFlow && max < producing)
                {
                    resFlow.color = new Color32(170, 12, 12, 255);
                }
                else
                {
                    resFlow.color = new Color32(0, 0, 0, 255);
                }
                resSlider.maxValue = max;
                resSlider.minValue = 0;
                resSlider.value = currentFlow;
            }
            else
            {
                Text resFlow = resourceTableB.Rows[counter % 6].Cells[1].GetComponentInChildren<Text>();
                Slider resSlider = resourceTableB.Rows[counter % 6].Cells[2].GetComponentInChildren<Slider>();
                resFlow.text = currentFlow + "/" + producing;
                if (max == currentFlow && max < producing)
                {
                    Debug.Log("red");
                    resFlow.color = new Color32(170, 12, 12, 255);
                }
                else
                {
                  
                    Debug.Log("Text Should Be White");
                    resFlow.color = Color.white;

                  
                }
                resSlider.maxValue = max;
                resSlider.minValue = 0;
                resSlider.value = currentFlow;
            }
            counter++;
            Debug.Log("Counter: " + counter);
        }


    }

    private void changeTransportSlider()
    {
        App app = UnityEngine.Object.FindObjectOfType<App>();
        Nation player = State.getNations()[app.GetHumanIndex()];
        int oldFlow = PlayerCalculator.calculateTransportFlow(player);

        // int oldFlow = player.industry.getTransportFlow();
        updateFlow(player);
        int newFlow = PlayerCalculator.calculateTransportFlow(player);
        //If slider was reduced new flow will be less than old flow - return coal to player
        int difference = Math.Abs(oldFlow - newFlow);
        int totalNumberOfProvinces = PlayerCalculator.getTotalNumberOfProvinces(player);

        // In case we reduced the flow
        if (newFlow < oldFlow)
        {
            if (oldFlow <= totalNumberOfProvinces)
            {
                // do nothing
            }
            else if (oldFlow > totalNumberOfProvinces)
            {
                if (newFlow >= totalNumberOfProvinces)
                {
                    player.collectResource(MyEnum.Resources.coal, difference * 0.2f);

                }
                else
                {
                    player.collectResource(MyEnum.Resources.coal, (oldFlow - totalNumberOfProvinces) * 0.2f);

                }
            }
        }
        else
        // The case where we have increased the amount of flow
        {
            if (newFlow <= totalNumberOfProvinces)
            {
                // do nothing
            }
            else if (newFlow > totalNumberOfProvinces)
            {
                // The case where we were already making use of some coal for transport
                if (oldFlow >= totalNumberOfProvinces)
                {
                    player.consumeResource(MyEnum.Resources.coal, difference * 0.2f);
                }
                else
                {
                    player.consumeResource(MyEnum.Resources.coal, (newFlow - player.getProvinces().Count) * 0.2f);

                }
            }
        }
        updateTransportPanel();
        var thisSlider = EventSystem.current.currentSelectedGameObject;

        AudioSource audioSource = thisSlider.GetComponent<AudioSource>();
        if(newFlow > totalNumberOfProvinces)
        {
            audioSource.clip = train;
        }
        else
        {
            audioSource.clip = wagon;
        }
        audioSource.Play();
    }

    private void updateFlow(Nation player)
    {
        //foreach (MyEnum.Resources res in Enum.GetValues(typeof(MyEnum.Resources)))
        //  {
        //      player.setResTransportFlow(res, 0);
        //  }

        int counter = 0;
        foreach (MyEnum.Resources res in Enum.GetValues(typeof(MyEnum.Resources)))
        {
            if (counter <= 5)
            {
                Slider resSlider = resourceTableA.Rows[counter].Cells[2].GetComponentInChildren<Slider>();
                player.setResTransportFlow(res, (int)resSlider.value);
            }
            else
            {
                Slider resSlider = resourceTableB.Rows[counter % 6].Cells[2].GetComponentInChildren<Slider>();
                player.setResTransportFlow(res, (int)resSlider.value);
            }
            counter++;
            //   Debug.Log(counter);
        }
    }

    private void addTransportCapacity()
    {
        App app = UnityEngine.Object.FindObjectOfType<App>();
        Nation player = State.getNations()[app.GetHumanIndex()];
        PlayerPayer.payForTrain(player);
        PlayerReceiver.buildTrain(player);
        uiUpdater.updateUI();
        updateTransportPanel();
    }

    public void closeAllPanels()
    {
        coordinator.transportFlag = false;
        coordinator.marketFlag = false;
        coordinator.progressFlag = false;
        coordinator.prodFlag = false;

        progressExit.Play();
        productionExit.Play();
        marketExit.Play();
        transportExit.Play();
    }

    private void goToNation()
    {
        if(majorPowerRelations.activeSelf == false)
        {
            return;
        }
        Debug.Log("Go to nation");
        var _button = EventSystem.current.currentSelectedGameObject;
        string nationName = _button.name;
        Nation nation = State.GetNationByName(nationName);
        State.currentlySelectedNationDiplomacy = nation.getIndex();

        App app = UnityEngine.Object.FindObjectOfType<App>();
        int playerIndex = app.GetHumanIndex();
        if (nation.getIndex() == playerIndex)
        {
            return;
        }
        Debug.Log("Go to nation");
        // relationsExit.Play();
       // majorPowerRelations.SetActive(false);
       // relationsFlag = false;
        showDiplomacyPanel();
        majorPowerRelations.SetActive(false);
        coordinator.relationsFlag = false;
    }

    private void openMajorRelationsPanel()
    {
        if (diplomacyCamera.enabled)
        {
            otherNationAdditionalInformation.SetActive(false);

            otherNationAdditionalInformation.SetActive(false);
            DiplomacyTab.SetActive(false);
            diplomacyCamera.enabled = false;
            mainCamera.enabled = true;
            return;
        }
        if (coordinator.relationsFlag == false)
        {
            Debug.Log("WTF?");
            coordinator.relationsFlag = true;
            if (coordinator.progressFlag == true)
            {
                coordinator.progressFlag = false;
                progressExit.Play();
            }
            if (coordinator.marketFlag == true)
            {
                coordinator.marketFlag = false;
                marketExit.Play();
            }
            if (coordinator.transportFlag == true)
            {
                coordinator.transportFlag = false;
                transportExit.Play();
            }
            if (coordinator.prodFlag == true)
            {
                coordinator.prodFlag = false;
                productionExit.Play();
            }
            UpdateMajorRelationsPanel();
            majorPowerRelations.SetActive(true);
            relationsEnter.Play();
        }
        else
        {
            otherNationAdditionalInformation.SetActive(false);

            coordinator.relationsFlag = false;
            relationsExit.Play();
        }
     


    }

    private void UpdateMajorRelationsPanel()
    {
        // Bambaki = 2;  Boreois = 27;  Chaldea = 9; Sidero = 25;  Sitari = 7;  Wyvermount = 18

        Nation bambaki = State.getNations()[2];
        Nation boreios = State.getNations()[27];
        Nation Chaldea = State.getNations()[9];
        Nation Sidero = State.getNations()[25];
        Nation Sitari = State.getNations()[7];
        Nation Wyvermount = State.getNations()[18];

        BambakiSidero.color = getRelationColor(2, 25);
        BoreoisSitari.color = getRelationColor(27, 7);
        ChaldeaWyvermount.color = getRelationColor(9, 18);
        BambakiChaldea.color = getRelationColor(2, 9);
        BambakiSitari.color = getRelationColor(2, 7);
        BambakiBoreois.color = getRelationColor(2, 27);
        BambakiWyvermount.color = getRelationColor(2, 18);
        BoreoisChaldea.color = getRelationColor(27, 9);
        SitariWyvermount.color = getRelationColor(7, 18);
        BoreoisWyvermount.color = getRelationColor(27, 18);
        ChaldeaSitari.color = getRelationColor(9, 7);
        ChaldeaSidero.color = getRelationColor(9, 25);
        SideroSitari.color = getRelationColor(25, 7);
        SideroWyvermount.color = getRelationColor(25, 18);
        BoreoisSidero.color = getRelationColor(27, 25);

    }

    private Color getRelationColor(int first, int second)
    {
        Nation firstNation = State.getNations()[first];
        int value = firstNation.Relations[second];

        if (value < 20)
        {
            Debug.Log(first + " " + second);
            return Color.red;
        }
        if (value < 40)
        {
            Debug.Log(first + " " + second);
            return new Color(1.0f, 0.64f, 0.0f, 1.0f);

        }
        if (value < 60)
        {
            Debug.Log(first + " " + second);

            return Color.yellow;
        }
        if (value < 80)
        {
            Debug.Log(first + " " + second);

            return Color.green;
        }
        return Color.blue;
    }



}
