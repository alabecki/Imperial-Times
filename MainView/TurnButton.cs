using assemblyCsharp;
using EasyUIAnimator;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UI.Tables;
using UnityEngine;
using UnityEngine.UI;
using WorldMapStrategyKit;

public class TurnButton : MonoBehaviour {

    public Button turnButton;
    public Button TradeResultButton;
    public GameObject turnPopUp;

    public CanvasGroup UniversalGUI;
    public CanvasGroup MapViewGUI;
    public CanvasGroup CityCanvas;
    public Terrain CityTerrain;

    public GameObject TradeResults;
    public GameObject importExportDif;
    public TableLayout ImportsResultTable;
    public TableLayout ExportsResultTable;
    public RectTransform importsConnector;
    public RectTransform exportConnector;
    public TableRow resSaleRow;

    WMSK map;

    private UIAnimation turnPopUpEnter;
    private UIAnimation turnPopUpExit;

    private UIAnimation tradeResultsEnter;

  

    // Use this for initialization
    void Start()
    {


        turnPopUp.SetActive(false);
        TradeResults.SetActive(false);
        map = WMSK.instance;

        turnButton.onClick.AddListener(delegate { Turn_Button(); });

        RectTransform popUpRect = turnPopUp.GetComponent<RectTransform>();

        turnPopUpEnter = UIAnimator.Scale(popUpRect, new Vector3(0, 0, 0), new Vector3(1, 1, 1), 2f).SetModifier(Modifier.PolyIn);
        turnPopUpExit = UIAnimator.Scale(popUpRect, new Vector3(1, 1, 1), new Vector3(0, 0, 0), 2f).SetModifier(Modifier.PolyOut);

        RectTransform tradeResultsRect = TradeResults.GetComponent<RectTransform>();
        tradeResultsEnter = UIAnimator.Move(tradeResultsRect, new Vector2(0.5f, 0f), new Vector2(0.5f, 0.48f), 1f).SetModifier(Modifier.Linear);



    }

    private void Turn_Button()
    {

        Debug.Log("Begin turn processing...");
        Debug.Log("Current phase: " + State.GetPhase());
        // blockPlayerFromActing();
        turnPopUp.SetActive(true);
        turnPopUpEnter.Play();
        State.turn++;
       TurnHandler.processTurnAdmin();
       State.tradeHandler.handleTrades();
        App app = UnityEngine.Object.FindObjectOfType<App>();
        int playerIndex = app.GetHumanIndex();
        Nation player = State.getNations()[playerIndex];
        showTradeResults(player);
        TradeResults.SetActive(true);
         tradeResultsEnter.Play();
        turnPopUpExit.Play();

    }





    private void continueFromBidOrPass()
    {

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
        UniversalGUI.blocksRaycasts = true;
        MapViewGUI.blocksRaycasts = true;
        CityCanvas.blocksRaycasts = true;
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
        GameObject[] buildings = CityTerrain.GetComponentsInChildren<GameObject>();
        foreach (GameObject item in buildings)
        {
            item.GetComponent<Collider>().isTrigger = true;
        }
    }

    private void showTradeResults(Nation player)
    {
        Market market = State.market;
        TradeResults.SetActive(true);
        tradeResultsEnter.Play();
        float imports = player.getImportValues();
        float exports = player.getExportValue();
        float difference = Mathf.Abs(imports - exports);

        TextMeshProUGUI _importExportDif = importExportDif.GetComponent<TextMeshProUGUI>();
        if (imports > exports)
        {
            _importExportDif.SetText("Imports: " + imports + " Exports: " + exports + " Deficit: " + difference);
        }
        else
        {
            _importExportDif.SetText("Imports: " + imports + " Exports: " + exports + " Surplus: " + difference);
        }

        ImportsResultTable.ClearRows();
        foreach(Signal imp in player.getImports())
        {
            TableRow newRow = Instantiate<TableRow>(resSaleRow);
            newRow.gameObject.SetActive(true);
            newRow.preferredHeight = 25;
            ImportsResultTable.AddRow(newRow);
            importsConnector.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (ImportsResultTable.transform as RectTransform).rect.height);

            newRow.Cells[0].GetComponentInChildren<Text>().text = "Bought";
            Transform res = newRow.Cells[1].transform.GetChild(0);
            Image resImg = res.GetComponent<Image>();
            resImg.preserveAspect = true;
            resImg.sprite = Resources.Load("Resource/" + imp.getResourceType().ToString(), typeof(Sprite)) as Sprite;
            newRow.Cells[2].GetComponentInChildren<Text>().text = "from";
            Transform flag = newRow.Cells[3].transform.GetChild(0);
            Image flagImg = res.GetComponent<Image>();
            flagImg.preserveAspect = true;
            Nation owner = State.getNations()[imp.getOwnerIndex()];
            flagImg.sprite = Resources.Load("Flags/" + owner.getName().ToString(), typeof(Sprite)) as Sprite;
            newRow.Cells[4].GetComponentInChildren<Text>().text = "for " + imp.getPriceSold().ToString("0.0");
        }

        ExportsResultTable.ClearRows();
        foreach (Signal exp in player.getImports())
        {
            TableRow newRow = Instantiate<TableRow>(resSaleRow);
            newRow.gameObject.SetActive(true);
            newRow.preferredHeight = 25;
            ExportsResultTable.AddRow(newRow);
            exportConnector.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (ExportsResultTable.transform as RectTransform).rect.height);

            newRow.Cells[0].GetComponentInChildren<Text>().text = "Sold";
            Transform res = newRow.Cells[1].transform.GetChild(0);
            Image resImg = res.GetComponent<Image>();
            resImg.preserveAspect = true;
            resImg.sprite = Resources.Load("FinishedGoods/" + exp.getGoodType().ToString(), typeof(Sprite)) as Sprite;
            newRow.Cells[2].GetComponentInChildren<Text>().text = "to";
            Transform flag = newRow.Cells[3].transform.GetChild(0);
            Image flagImg = res.GetComponent<Image>();
            flagImg.preserveAspect = true;
            Nation owner = State.getNations()[exp.getOwnerIndex()];
            flagImg.sprite = Resources.Load("Flags/" + owner.getName().ToString(), typeof(Sprite)) as Sprite;
            newRow.Cells[4].GetComponentInChildren<Text>().text = "for " + exp.getPriceSold().ToString("0.0");
        }




        //  State.advanceGamePhase();

    }

    private void openAuctionPanel()
    {


        //close auction button should also be a copty of Turn
    }


    private void allowUnitMovement()
    {

    }

}
