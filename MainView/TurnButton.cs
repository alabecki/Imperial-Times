using assemblyCsharp;
using EasyUIAnimator;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UI.Tables;
using UI.ThreeDimensional;
using UnityEngine;
using UnityEngine.UI;
using WorldMapStrategyKit;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;



public class TurnButton : MonoBehaviour {

    private int demoTurnLimit = 40;
    public Button turnButton;

    public TableLayout transportTableA;
    public TableLayout transportTableB;

    public CanvasGroup UniversalGUI;
    public CanvasGroup MapViewGUI;
    public CanvasGroup CityCanvas;

    public GameObject TradeResults;
    public GameObject importExportDif;
    public TableLayout ImportsResultTable;
    public TableLayout ExportsResultTable;
    public RectTransform importsConnector;
    public RectTransform exportConnector;
    public TableRow resSoldRow;

    public EraCostUpdater eraUpdater;

    public TurnHandler turnHandler;

    public GameObject endGameMessage;
    public Button endGameContinueButton;
    //public TableRow resBoughtRow;
    //public TableRow goodSoldRow;
    //public TableRow goodBoughtRow;

    WMSK map;

    //update main header values
    public Text turn;


    private UIAnimation tradeResultsEnter;

  

    // Use this for initialization
    void Start()
    {
        endGameMessage.SetActive(false);
        TradeResults.SetActive(false);
        map = WMSK.instance;

        turnButton.onClick.AddListener(delegate { Turn_Button(); });
        endGameContinueButton.onClick.AddListener(delegate { endDemo(); });


        RectTransform tradeResultsRect = TradeResults.GetComponent<RectTransform>();
        tradeResultsEnter = UIAnimator.Move(tradeResultsRect, new Vector2(0.5f, 0f), new Vector2(0.5f, 0.48f), 1f).SetModifier(Modifier.Linear);

        MarketHelper.resetBidsAndOffers();


    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.KeypadEnter))
        {
            Debug.Log("Turn");
            Turn_Button();
        }
    }


    private void endDemo()
    {
        SceneManager.LoadScene(1);
    }

    private void Turn_Button()
    {

        if(State.turn >= demoTurnLimit)
        {
            endGameMessage.SetActive(true);
            return;
        }
        turnButton.interactable = false;
        Debug.Log("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
        Debug.Log("Begin turn processing...");
        Debug.Log("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
        Debug.Log("Current phase: " + State.GetPhase());
        //  blockPlayerFromActing();
        //turnPopUpEnter.Play();
        App app = UnityEngine.Object.FindObjectOfType<App>();
        int playerIndex = app.GetHumanIndex();
        Nation player = State.getNations()[playerIndex];
        //State.turn++;
        PlayerProcessor.processSignals(player);
       // MarketHelper.resetBidsAndOffers();
      //  maintenancePayer.payMaintenance(player);
        PlayerCollector.collectForPlayer(player);

        // Texting ======================================================================================
        turnHandler.processTurnAdmin();
        State.advanceGamePhase();
        State.tradeHandler.handleTrades();


        WorldBank bank = State.bank;
        Debug.Log("Banking           Banking                     Banking");
        bank.collectInterest();
        bank.distributeCollectedInterest();
        Debug.Log("Finished Banking - should not proceed to Trade Results");
        showTradeResults(player);
        TradeResults.SetActive(true);
         tradeResultsEnter.Play();
        //turnPopUpExit.Play();
       // payCoalMaintaince(player);
        updateHeaderValues();
        if (State.turn == 40)
        {
            eraUpdater.swichEra();
        }
        if (State.turn == 80)
        {
            eraUpdater.swichEra();

        }
    }

    // Only for human player
    private int payCoalMaintaince(Nation player)
    {
        int totalFlow = 0;
        foreach (MyEnum.Resources res in System.Enum.GetValues(typeof(MyEnum.Resources)))
        {
            int currentFlow = player.getResTransportFlow(res);
            totalFlow += currentFlow;
        }
        int nonCoalFlow = player.getProvinces().Count + player.getColonies().Count;
        int coalFlow = totalFlow - nonCoalFlow;
        if(coalFlow < 0)
        {
            coalFlow = 0;
        }
        return coalFlow;
    }

    /*    MyEnum.GamePhase currentPhase;
        App app = UnityEngine.Object.FindObjectOfType<App>();
        int playerIndex = app.GetHumanIndex();
        Nation player = State.getNations()[playerIndex];
        State.currentTurnOrderIndex += 1;
        State.currentPlayer = State.getTurnOrder()[State.currentTurnOrderIndex];
        Debug.Log("Current player is: " + State.getNations()[State.currentTurnOrderIndex].getIndex());
        // PlayerCollector.collectForPlayer(player);
        PlayerProcessor.processSignals(player);        
        bool cont = true;
        while(cont == true)
        {
            currentPhase = TurnHandler.ProcessTurn();
            if(currentPhase == MyEnum.GamePhase.adminstration)
            {
                Debug.Log("Admin Phase");
                if (playerIndex == State.currentPlayer)
                {
                    cont = false;
                    handAdminPhaseToPlayer();
                }
                else
                {
                    currentPhase = TurnHandler.ProcessTurn();
                }
            }
            if(currentPhase == MyEnum.GamePhase.trade)
            {
                Debug.Log("Trade Phase");
                State.tradeHandler.handleTrades();
                cont = false;
                //make sure that the continue button on the trade result panel is essentially a copy of the Turn button
                showTradeResults(player);
            }
            if(currentPhase == MyEnum.GamePhase.auction)
            {
                if (playerIndex == State.currentPlayer)
                {
                    cont = false;
                    openAuctionPanel();
                }
             //   else
             //   {
              //      currentPhase = TurnHandler.ProcessTurn();

             //   }

            }
            if(currentPhase == MyEnum.GamePhase.movement)
            {
                if (playerIndex == State.currentPlayer)
                {
                    cont = false;
                    allowUnitMovement();
                }
                else
                {
                    currentPhase = TurnHandler.ProcessTurn();
                }
            }
        }
        restorePlayerControl();

    }  */


    private void restorePlayerControl()
    {
        UniversalGUI.blocksRaycasts = false;
        MapViewGUI.blocksRaycasts = false;
        CityCanvas.blocksRaycasts = false;
        map.showCursor = true;
    }

    private void blockPlayerFromActing()
    {
       // UniversalGUI.blocksRaycasts = true;
       // MapViewGUI.blocksRaycasts = true;
        //CityCanvas.blocksRaycasts = true;
        map.showCursor = false;
      //  map.HideProvinceSurfaces();
        //GameObject[] buildings = CityTerrain.GetComponentsInChildren<GameObject>();
       // foreach (GameObject item in buildings)
        //{
         //   item.GetComponent<Collider>().isTrigger = false;
       // }
    }

    private void handAdminPhaseToPlayer()
    {
        map.showCursor = true;

        UniversalGUI.blocksRaycasts = false;
        MapViewGUI.blocksRaycasts = false;
        CityCanvas.blocksRaycasts = false;
    }

    private void showTradeResults(Nation player)
    {
        Debug.Log("Preparing Trade Results");
        Market market = State.market;
        TradeResults.SetActive(true);
        tradeResultsEnter.Play();
        float imports = player.getImportValues();
        float exports = player.getExportValue();
        imports = (float)Math.Round(imports, 2);
        exports = (float)Math.Round(exports, 2);
        float difference = Mathf.Abs(imports - exports);
        difference = (float)Math.Round(difference, 2);

        TextMeshProUGUI _importExportDif = importExportDif.GetComponent<TextMeshProUGUI>();
        if (imports > exports)
        {
            _importExportDif.SetText("Imports: " + imports + " Exports: " + exports + " Deficit: " + difference);
            _importExportDif.color = new Color32(255, 0, 0, 255);
        }
        else
        {
            _importExportDif.SetText("Imports: " + imports + " Exports: " + exports + " Surplus: " + difference);
            _importExportDif.color = new Color32(0, 204, 0, 255);
        }

        ImportsResultTable.ClearRows();
        foreach(Signal imp in player.getImports())
        {
            TableRow newRow = Instantiate<TableRow>(resSoldRow);
          
            newRow.gameObject.SetActive(true);
            newRow.preferredHeight = 25;
            ImportsResultTable.AddRow(newRow);
            importsConnector.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (ImportsResultTable.transform as RectTransform).rect.height);

            newRow.Cells[0].GetComponentInChildren<Text>().text = "Bought";
            //Image
            Transform res = newRow.Cells[1].transform.GetChild(0);
            Image resImg = res.GetComponent<Image>();
            resImg.preserveAspect = true;
            if (imp.getRes())
            {
                resImg.sprite = Resources.Load("Resource/" + imp.getResourceType().ToString(), typeof(Sprite)) as Sprite;
            }
            else
            {
                resImg.sprite = Resources.Load("FinishedGoods/" + imp.getGoodType().ToString(), typeof(Sprite)) as Sprite;

            }
            newRow.Cells[2].GetComponentInChildren<Text>().text = "from";
            Transform flag = newRow.Cells[3].transform.GetChild(0);
            //Image flagImg = flag.GetComponent<Image>();
            UIObject3D flagObject = flag.GetComponent<UIObject3D>();
           // flagImg.preserveAspect = true;
            Nation owner = State.getNations()[imp.getOwnerIndex()];
            GameObject flagPrefab = Instantiate(Resources.Load<GameObject>("Flags/Prefabs/" + owner.getNationName()));
            flagObject.ObjectPrefab = flagPrefab.transform;
            // flagImg.sprite = Resources.Load("Flags/" + owner.getName().ToString(), typeof(Sprite)) as Sprite;
            newRow.Cells[4].GetComponentInChildren<Text>().text = "for " + imp.getPriceSold().ToString("0.0");

        }

        ExportsResultTable.ClearRows();
        foreach (Signal exp in player.getExports())
        {
            TableRow newRow = Instantiate<TableRow>(resSoldRow);
            newRow.gameObject.SetActive(true);
            newRow.preferredHeight = 25;
            ExportsResultTable.AddRow(newRow);
            exportConnector.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (ExportsResultTable.transform as RectTransform).rect.height);

            newRow.Cells[0].GetComponentInChildren<Text>().text = "Sold";
            Transform res = newRow.Cells[1].transform.GetChild(0);
            Image resImg = res.GetComponent<Image>();
            resImg.preserveAspect = true;
            if (exp.getRes())
            {
                resImg.sprite = Resources.Load("Resource/" + exp.getResourceType().ToString(), typeof(Sprite)) as Sprite;
            }
            else
            {
                resImg.sprite = Resources.Load("FinishedGoods/" + exp.getGoodType().ToString(), typeof(Sprite)) as Sprite;

            }
            newRow.Cells[2].GetComponentInChildren<Text>().text = "to";
            Transform flag = newRow.Cells[3].transform.GetChild(0);
            //Image flagImg = flag.GetComponent<Image>();
            //flagImg.preserveAspect = true;
            Nation owner = State.getNations()[exp.getOwnerIndex()];
            UIObject3D flagObject = flag.GetComponent<UIObject3D>();
            Debug.Log(owner.getNationName());
            GameObject flagPrefab = Instantiate(Resources.Load<GameObject>("Flags/Prefabs/" + owner.getNationName()));
            flagObject.ObjectPrefab = flagPrefab.transform;
            //flagImg.sprite = Resources.Load("Flags/" + owner.getName().ToString(), typeof(Sprite)) as Sprite;
            newRow.Cells[4].GetComponentInChildren<Text>().text = "for " + exp.getPriceSold().ToString("0.0");
        }

        Debug.Log("Finished Preparing Trade Results");


        //  State.advanceGamePhase();

    }

    private void openAuctionPanel()
    {


        //close auction button should also be a copty of Turn
    }


    private void allowUnitMovement()
    {

    }

    public void updateHeaderValues()
    {
        turn.text = State.turn.ToString();
    }


    /*private void displayStabilityFace()
    {

        App app = UnityEngine.Object.FindObjectOfType<App>();
        int playerIndex = app.GetHumanIndex();
        Nation player = State.getNations()[playerIndex];

        int nationStab = player.Stability;

        if (nationStab == -3)
        {
            stability.sprite = Resources.Load("Sprites/Stability/-3", typeof(Sprite)) as Sprite;
        }
        else if (nationStab == -2)
        {
            stability.sprite = Resources.Load("Sprites/Stability/-2", typeof(Sprite)) as Sprite;
        }
        else if (nationStab == -1)
        {
            stability.sprite = Resources.Load("Sprites/Stability/-1", typeof(Sprite)) as Sprite;
        }
        else if (nationStab == 0)
        {
          stability.sprite = Resources.Load("Sprites/Stability/0", typeof(Sprite)) as Sprite;
       }
        else if (nationStab == 1)
        {
            stability.sprite = Resources.Load("Sprites/Stability/1", typeof(Sprite)) as Sprite;
        }
        else if (nationStab == 2)
        {
            stability.sprite = Resources.Load("Sprites/Stability/2", typeof(Sprite)) as Sprite;
        }
        else
        {
            stability.sprite = Resources.Load("Sprites/Stability/3", typeof(Sprite)) as Sprite;
        }
    } */
}


