using EasyUIAnimator;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using assemblyCsharp;
using UI.Tables;
using WorldMapStrategyKit;
using UI.ThreeDimensional;

public class TradeResultButton : MonoBehaviour {

    WMSK map;

    public GameObject tradeResultsPanel;
    public Button turnButton;

    public GameObject AuctionPanel;
    public GameObject AuctionWinnerPanel;
    public UIObject3D itemFlag1;
    public Toggle isRecognizing;

    public UIObject3D itemFlag2;
    public UIObject3D winnerFlag;


    public Text auctionPrizeText;
    public Text auctionWinnerText;
    public Image backGroudImage;

    public GameObject ministerPanel;
    public GameObject diplomatPanel;

    public GameObject AuctionPanelTitleText;
    public GameObject ItemNameText;
    public TableLayout itemProvinces;
    public TableLayout biddingTable;
     public TableRow biddingRow;
    public RectTransform biddingConnector;

    public Button bid;
    public Button pass;
    public Button increaseBid;
    public Button decreaseBid;

    public Text currentBidAmount;

    public Text playersCurrentBid;
    public Text playerRemainingPoints;

   // public Image PointTypeImage;
    //public Image itemFlag;
    //public UIObject3D itemFlag;

    public Button tradeResultButton;
    public Button closeTradeResultPanel;
    public Text currentNumberOfBiddingPoints;

    public Text ministerText;
    public Text diplomatText;

    public ShowTerrain terrainUpdater;

    bool playerTurn;

    public UI_Updater ui_updater;

    private UIAnimation tradeResultsExit;
    private UIAnimation auctionEnter;
    private UIAnimation  auctionFadeIn;

    private Graphic[] auctionRect;

    public CanvasGroup UniversalGUI;
    public CanvasGroup MapViewGUI;
    public CanvasGroup CityCanvas;
    // public Terrain CityTerrain;

    public Text currentOwnerName;
    public Text relationWithCurrentOwner;
    public Text pointTypeText;

    //public EventRegister eventObject;
    public EventRegisterObject eventRegisterObject;

    // Use this for initialization
    void Start()
    {
        AuctionWinnerPanel.SetActive(false);
        map = WMSK.instance;

        AuctionPanel.SetActive(false);

        tradeResultButton.onClick.AddListener(delegate { contiueToAuction(); });
        closeTradeResultPanel.onClick.AddListener(delegate { contiueToAuction(); });

        RectTransform tradeResultsRect = tradeResultsPanel.GetComponent<RectTransform>();
        tradeResultsExit = UIAnimator.Move(tradeResultsRect, new Vector2(0.5f, 0.48f), new Vector2(0.5f, -0.2f), 1f).SetModifier(Modifier.Linear);

        //RectTransform auctionPanelRect = AuctionPanel.GetComponent<RectTransform>();
        // auctionEnter = UIAnimator.

        auctionRect = AuctionPanel.GetComponentsInChildren<Graphic>();
        pass.onClick.AddListener(delegate { playerPass(); });
        bid.onClick.AddListener(delegate { playerBid(); });
        increaseBid.onClick.AddListener(delegate { IncreaseBid(); });
        decreaseBid.onClick.AddListener(delegate { DecreaseBid(); });
    }
 
    public void nextAuctionPhase()
    {
        Debug.Log("Begin new auction phase");
     //   waitASec(0.8f);

        AuctionHandler auction = State.getAuctionHandler();
        App app = UnityEngine.Object.FindObjectOfType<App>();
        int humanIndex = app.GetHumanIndex();
        Nation humanPlayer = State.getNations()[humanIndex];
        updateAuctionPanel();
        if (auction.checkIfAuctionIsOver())
        {
            Debug.Log("Auction Should be Over now");
            continueToNextTurn();
            AuctionPanel.SetActive(false);
            auction.concludeAuction();
            prepareWinnerPanel();
            AuctionWinnerPanel.SetActive(true);
            terrainUpdater.ColourContries();
        }
        else
        {
            Debug.Log("Auction Will Continue");
            //If all players have bid and auction is not over - go back to the first bidder
          //  Debug.Log(auction.CurrentBidPosition);
          //  if(auction.CurrentBidPosition == State.getMajorNations().Count)
          //  {
          //      Debug.Log("Return to first remaining bidder");
          //      auction.CurrentBidPosition = 0;
          //  }
            // Go to auction control flow - it will have each AI player bid or pass until reaching the Human controlled player
            auctionControlFlow();
            updateAuctionPanel();
            // If human has passed  
            Debug.Log("Human has passed?: " + humanPlayer.getIndex());
            if (auction.getIfPlayerPass(humanPlayer.getIndex()))
            {   // Check if all players have now passed
                Debug.Log("Human Player has passed");

              /*  if (auction.checkIfAuctionIsOver())
                {

                    Debug.Log("Auction Should be Over now");
                    continueToNextTurn();
                    AuctionPanel.SetActive(false);
                    auction.concludeAuction();
                    prepareWinnerPanel();
                    AuctionWinnerPanel.SetActive(true);
                    terrainUpdater.ColourContries();
                } */
                // NextPhaseAfterPlayerPasses(1.5f);
               // else
               // {
                    Debug.Log("Bidding continues without human");
                   auctionControlFlow();
               // }
            }
            Debug.Log(auction.CurrentBidPosition);
            int indexOfCurrentBidder = auction.getBiddingOrder()[auction.CurrentBidPosition];
            if(indexOfCurrentBidder != humanIndex)
            {

                auctionControlFlow();
            }
            // nextAuctionPhase();
            Debug.Log("Should be player's turn now?");
            //If player has not passed, control will now be handed to the player
        }       
    }

    public void auctionControlFlow()
    {
        AuctionHandler auction = State.getAuctionHandler();

        Debug.Log("Begin Auction Control Flow");
        App app = UnityEngine.Object.FindObjectOfType<App>();
        int playerIndex = app.GetHumanIndex();
        Nation humanPlayer = State.getNations()[playerIndex];

        // int start = currentBidPosition;
        //for (int i = start; i < biddingOrder.Count; i++)
        //{
        while (true)
        {
            Debug.Log("Top of while loop");
            Nation currentPlayer = State.getNations()[auction.getBiddingOrder()[auction.CurrentBidPosition]];
            State.setCurrentPlayer(auction.getBiddingOrder()[auction.CurrentBidPosition]);

            Debug.Log("Current Position is: " + auction.CurrentBidPosition);
            Debug.Log("Current Bidder is: " + currentPlayer.getName());
            Debug.Log("Curret Bidder Index is: " + currentPlayer.getIndex());
            //Debug.Log("Current player - " + currentPlayer.nationName);
            Debug.Log("Current player passed already? :" + auction.getIfPlayerPass(currentPlayer.getIndex()));
            if (humanPlayer.getIndex() == currentPlayer.getIndex() && !auction.getIfPlayerPass(humanPlayer.getIndex()))
            //  if (humanPlayer.getIndex() == currentPlayer.getIndex())
            {
                Debug.Log("Current is human");
                return;
            }
            if (humanPlayer.getIndex() == currentPlayer.getIndex() && auction.getIfPlayerPass(humanPlayer.getIndex()))
            {
                auction.incrementBiddingPosition();
                return;
            }

            else if (currentPlayer.getIndex() == auction.getHighestBidderSoFar())
            {
                Debug.Log("Returned to highest bidder (auction presumably over) " + auction.CurrentBidPosition);
                return;
            }
            else
            {
                Debug.Log("AI bid");
                auction.newBid(currentPlayer);
            }
            auction.incrementBiddingPosition();
        }
                
    }



    public void prepareWinnerPanel()
    {
        Debug.Log("Prepare winner Panel");
        AuctionHandler auction = State.getAuctionHandler();
        int itemIndex = auction.getIndexOfCurrentItem();
        Nation itemNation = State.getNations()[itemIndex];
        Debug.Log(itemNation.getNationName());
        GameObject itemFlag = Instantiate(Resources.Load<GameObject>("Flags/Prefabs/" + itemNation.getNationName()));
        itemFlag2.ObjectPrefab = itemFlag.transform;

        Nation winner = State.getNations()[auction.getHighestBidderSoFar()];
        GameObject _winnerFlag = Instantiate(Resources.Load<GameObject>("Flags/Prefabs/" + winner.getNationName()));
        winnerFlag.ObjectPrefab = _winnerFlag.transform;
        
        auctionPrizeText.text = "The colony of " + itemNation.getName();
        auctionWinnerText.text = "Goes to " + winner.getName() + "!";
    }

    public void contiueToAuction()
    {
        increaseBid.gameObject.SetActive(true);
        decreaseBid.gameObject.SetActive(true);
        App app = UnityEngine.Object.FindObjectOfType<App>();
        int playerIndex = app.GetHumanIndex();
        Nation player = State.getNations()[playerIndex];
        Debug.Log("Coutinune to Auction @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@" + State.GetPhase().ToString());

        tradeResultsExit.Play();
        tradeResultsPanel.SetActive(false);

        //State.advanceGamePhase();
        Debug.Log("GamePhase is: " + State.GetPhase().ToString());
        // Check if should to go auction (if enough turns have gone by)
        int turn = State.turn;
 
        if (turn > 5 && turn % 3 == 0)
        {
             
            // Debug.Log("Auction Time? It is Turn: " + State.turn);
            AuctionHandler auction = State.getAuctionHandler();

            auction.NewAuction();
            prepareAuctionPanel(auction);
            openAuctionPanel();

            nextAuctionPhase();
            turnButton.interactable = true;
        }
        else
        {
            // turnButton.interactable = true;
            continueToEvents();
            // Have these guys only get called after all events are resolved
            // Probably want to have at least one Decision Event come up and have the final one call continueToNextTurn when
            // the event queue is empty
            turnButton.interactable = true;
        
            continueToNextTurn();
        }
    }


    private void continueToEvents()
    {
        // remember to make turn button interactable afterwards
        EventRegister eventLogic = State.eventRegister;
        eventRegisterObject.processAndGenerateEvents();
     
    }


    private void continueToNextTurn()
    {
        Debug.Log("Countinue to Next Turn ___________________________________________________________________________________________________________________");
        ColourContries();
        Debug.Log("Turn: " + State.turn);
        //  State.TurnIncrement();
        ui_updater.updateUI();
        turnButton.interactable = true;
        while (State.GetPhase() != MyEnum.GamePhase.adminstration)
        {
            State.advanceGamePhase();
        }
       
    }

    private void ColourContries()
    {
        Debug.Log("Color Countries ___________________________________________________________________________________________________________________");

        foreach (assemblyCsharp.Province prov in State.getProvinces().Values)
        {
            int provIndex = prov.getIndex();
            WorldMapStrategyKit.Province mapProv = map.provinces[provIndex];
            int countryIndex = mapProv.countryIndex;
            int nationIndex = prov.getOwnerIndex();
            if(countryIndex != nationIndex)
            {
                Debug.Log("Reassign Nation");
                mapProv.countryIndex = nationIndex;
                Region provRegion = mapProv.mainRegion;
                Country newOwnerCountry = map.countries[nationIndex];
                map.CountryTransferProvinceRegion(countryIndex, provRegion, true);

            } 
        }
        for (int k = 0; k < map.countries.Length; k++)
        {
            Color color = new Color(UnityEngine.Random.Range(0.0f, 0.65f),
            UnityEngine.Random.Range(0.0f, 0.65f), UnityEngine.Random.Range(0.0f, 0.65f));
            Nation nation = State.getNations()[k];
           // Debug.Log(nation.getName());
            nation.setColor(color);
            map.ToggleCountrySurface(k, true, color);
        }
    }

    private void openAuctionPanel()
    {
        AuctionPanel.SetActive(true);
    }

    private void updateAuctionPanel()
    {
        Debug.Log("Update Auction Panel ___________________________");
        App app = UnityEngine.Object.FindObjectOfType<App>();
        int playerIndex = app.GetHumanIndex();
        Nation player = State.getNations()[playerIndex];
        AuctionHandler auction = State.getAuctionHandler();
        Nation item = State.getNations()[auction.getIndexOfCurrentItem()];
       // Debug.Log(playerIndex);
        //TextMeshProUGUI currentPlayerBid = currentBidAmount.GetComponent<TextMeshProUGUI>();
        currentBidAmount.text = auction.getPlayerBid(playerIndex).ToString();
        if (auction.getPlayerBid(playerIndex) > 0 && !auction.getIfPlayerPass(playerIndex))
        {
            decreaseBid.interactable = true;
        }
        else
        {
            decreaseBid.interactable = false;
        }

        if (player.ColonialPoints <= State.CurrentColonyAuctionBid || player.RecognizingTheseClaims.Contains(item.getIndex()) || 
            auction.getIfPlayerPass(playerIndex) || player.landForces.Strength < 2)
        {
            increaseBid.interactable = false;
        }
        else
        {
            increaseBid.interactable = true;
        }
        
        biddingTable.ClearRows();
        for (int i = 0; i < auction.getBiddingOrder().Count; i++)
        {
            int currNationIndex = auction.getBiddingOrder()[i];
            Nation currNat = State.getNations()[currNationIndex];
         
            TableRow newRow = Instantiate<TableRow>(biddingRow);
            newRow.gameObject.SetActive(true);
            newRow.preferredHeight = 30;
            newRow.name = currNat.getIndex().ToString();
            biddingTable.AddRow(newRow);
            biddingConnector.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (biddingTable.transform as RectTransform).rect.height);

            int currNatIndex = currNat.getIndex();
         //   Debug.Log("Current Nation: " + currNatIndex);
            int currentNatBid = auction.getPlayerBid(currNatIndex);

            newRow.Cells[0].GetComponentInChildren<Text>().text = currentNatBid.ToString();
            newRow.Cells[1].GetComponentInChildren<Text>().text = currNat.getNationName().ToString();
            // Transform res = newRow.Cells[2].transform.GetChild(0);

            UIObject3D flagImage = newRow.Cells[2].GetComponentInChildren<UIObject3D>();
            // flagImage.sprite = Resources.Load("Flags/" + currNat.getNationName().ToString(), typeof(Sprite)) as Sprite;
            GameObject flagPrefab = Instantiate(Resources.Load<GameObject>("Flags/Prefabs/" + currNat.getNationName()));
            flagImage.ObjectPrefab = flagPrefab.transform;
            flagImage.RenderScale = 0;
            flagImage.LightIntensity = 1;
            Transform statusTransform = newRow.Cells[3].transform.GetChild(0);
            Image statusImage = statusTransform.GetComponent<Image>();
            if (auction.getIfPlayerPass(currNatIndex) == true)
            {
                statusImage.sprite = Resources.Load("Sprites/GUI/Dark_Red_x", typeof(Sprite)) as Sprite;
            }
            else
            {
                statusImage.sprite = Resources.Load("Sprites/GUI/AuctionHammer", typeof(Sprite)) as Sprite;
            }
            
        }
        if(auction.getPlayerBid(playerIndex) > State.CurrentColonyAuctionBid)
        {
            bid.interactable = true;
        }
        else
        {
            bid.interactable = false;
            
        }
        TextMeshProUGUI passButtonText = pass.GetComponentInChildren<TextMeshProUGUI>();

        if (auction.getIfPlayerPass(playerIndex) || auction.getHighestBidderSoFar() == playerIndex)
        {
            passButtonText.SetText("Continue");
        }
        else
        {
            passButtonText.SetText("Pass");
        }
    }

    private void prepareAuctionPanel(AuctionHandler auction)
    {
        increaseBid.enabled = true;
        decreaseBid.enabled = true;
        App app = UnityEngine.Object.FindObjectOfType<App>();
        int playerIndex = app.GetHumanIndex();
        Nation player = State.getNations()[playerIndex];
        TextMeshProUGUI title = AuctionPanelTitleText.GetComponent<TextMeshProUGUI>();
        Nation item = State.getNations()[0];
     
        // Colony Auction
        backGroudImage.sprite = Resources.Load("Sprites/BackGrounds/imperialism_cartoon", typeof(Sprite)) as Sprite;
        title.text = "Colony Bids";
        pointTypeText.text = "Colonial Points";
        // PointTypeImage.sprite = Resources.Load("Sprites/GUI/eagle", typeof(Sprite)) as Sprite;

        item = State.getNations()[auction.getIndexOfCurrentItem()];

        // GameObject mapPrefab = Instantiate(Resources.Load<GameObject>("RPG100set/512/Prop/06_letter" + item.getNationName()));
        currentNumberOfBiddingPoints.text = player.GetColonialPoints().ToString();
        if (player.GetColonialPoints() < State.CurrentColonyAuctionBid || player.RecognizingTheseClaims.Contains(item.getIndex() )|| player.landForces.Strength < 2)
        {
            increaseBid.interactable = false;
        }
        else
        {
            increaseBid.interactable = true;
        }
        
        TextMeshProUGUI itemName = ItemNameText.GetComponent<TextMeshProUGUI>();
        itemName.text = item.getNationName();

        GameObject itemFlag = Instantiate(Resources.Load<GameObject>("Flags/Prefabs/" + item.getNationName()));
        itemFlag1.ObjectPrefab = itemFlag.transform;

        // TextMeshProUGUI currentPlayerBid = currentBidAmount.GetComponent<TextMeshProUGUI>();
        currentBidAmount.text = "00";
        int firstProvIndex = -1;
        int secondProvIndex = -1;
        foreach(int provIndex in item.getProvinces())
        {
            if(firstProvIndex == -1)
            {
                firstProvIndex = provIndex;
            }
            else
            {
                secondProvIndex = provIndex;
            }
        }

        assemblyCsharp.Province firstProvince = State.getProvince(firstProvIndex);
        itemProvinces.Rows[1].Cells[0].GetComponentInChildren<Text>().text = firstProvince.getProvName();
        //  itemProvinces.Rows[1].Cells[1].GetComponentInChildren<Image>().sprite = 
        Transform P1Res = itemProvinces.Rows[1].Cells[1].transform.GetChild(0);
        Image P1ResImage = P1Res.GetComponent<Image>();
       // Image provOneRes = itemProvinces.Rows[1].Cells[1].GetComponentInChildren<Image>() as Image;
        //resImg.sprite = Resources.Load("Resource/" + imp.getResourceType().ToString(), typeof(Sprite)) as Sprite;
      //  Debug.Log("First Resource is: " + firstProvince.getResource());
        P1ResImage.sprite = Resources.Load("Resource/" + firstProvince.getResource().ToString(), typeof(Sprite)) as Sprite;
        itemProvinces.Rows[1].Cells[2].GetComponentInChildren<Text>().text = firstProvince.getDevelopmentLevel().ToString();
        Toggle provOneRail = itemProvinces.Rows[1].Cells[3].GetComponentInChildren<Toggle>();
        if (firstProvince.getRail())
        {
            provOneRail.isOn = true;
        }
        else
        {
            provOneRail.isOn = false;
        }

        assemblyCsharp.Province secondProvince = State.getProvince(secondProvIndex);
        itemProvinces.Rows[2].Cells[0].GetComponentInChildren<Text>().text = secondProvince.getProvName();
        //  itemProvinces.Rows[1].Cells[1].GetComponentInChildren<Image>().sprite = 
       // Image provTwoRes = itemProvinces.Rows[2].Cells[1].GetComponentInChildren<Image>() as Image;
       // Debug.Log("Second Resource is: " + secondProvince.getResource());
        Transform P2Res = itemProvinces.Rows[2].Cells[1].transform.GetChild(0);
        Image P2ResImage = P2Res.GetComponent<Image>();
        P2ResImage.sprite = Resources.Load("Resource/" + secondProvince.getResource().ToString(), typeof(Sprite)) as Sprite;
        itemProvinces.Rows[2].Cells[2].GetComponentInChildren<Text>().text = secondProvince.getDevelopmentLevel().ToString();
        Toggle provTwoRail = itemProvinces.Rows[2].Cells[3].GetComponentInChildren<Toggle>();
        if (secondProvince.getRail())
        {
            provTwoRail.isOn = true;
        }
        else
        {
            provTwoRail.isOn = false;
        }
        if(item.IsColonyOf() != -1)
        {
            Nation currentOwner = State.getNations()[item.IsColonyOf()];
            currentOwnerName.text = currentOwner.getNationName();
            relationWithCurrentOwner.text = player.Relations[currentOwner.getIndex()].ToString();
        }
        else
        {
            currentOwnerName.text = "None";
        }

        if (player.RecognizingTheseClaims.Contains(item.getIndex()))
        {
            isRecognizing.isOn = true;
        }
        else
        {
            isRecognizing.isOn = false;
        }
    }

    private void declareAuctionResults()
    {
        App app = UnityEngine.Object.FindObjectOfType<App>();
        int playerIndex = app.GetHumanIndex();
        Nation player = State.getNations()[playerIndex];
        AuctionHandler auction = State.getAuctionHandler();
        Nation winner = State.getNations()[auction.getHighestBidderSoFar()];
        Nation item = State.getNations()[0];
       
        item = State.getNations()[auction.getIndexCurrentCol()];
        if (winner.getIndex() == player.getIndex())
        {
            ministerText.text = "Congratulations! We may now bring the light of civilization to the people of " + item.getNationName() +
                " asking for nothing in return other than unrestricted access to their resources and labour!";
            //  ministerPanel.SetActive(true);
        }
        else
        {
            ministerText.text = "It looks like " + winner.getNationName() + " has managed to seal control of " + item.getNationName() +
                " for now.... Not that we really wanted anything to do with that insignifigant nation anyway.";
        }
        ministerPanel.SetActive(true);
        
    }


    private void playerPass()
    {

        Debug.Log("Player Passes  >>>>>>>>>>>>>>>>>>>>>>>>>>");
        AuctionHandler auction = State.getAuctionHandler();
        App app = UnityEngine.Object.FindObjectOfType<App>();
        int playerIndex = app.GetHumanIndex();
        Text bidText = bid.GetComponentInChildren<Text>();
        auction.tempHumanBidLevel = auction.getPlayerBid(playerIndex);
        auction.passOnBid(playerIndex);
        auction.incrementBiddingPosition();
        nextAuctionPhase();
        // Nation player = State.getNations()[playerIndex];
    }

    private void playerBid()
    {
        Debug.Log("Player bids >>>>>>>>>>>>>>>>>>>>>>>>>>>>");
        AuctionHandler auction = State.getAuctionHandler();
        App app = UnityEngine.Object.FindObjectOfType<App>();
        int playerIndex = app.GetHumanIndex();
        int playerBid = auction.getTempHumanBidLevel();
        auction.setPlayerBid(playerIndex, playerBid);
        auction.setIndexOfHighestBidderSoFar(playerIndex);
        //State.CurrentColonyAuctionBid = auction.getPlayerBid(playerIndex);
        State.UpdateColonyAuctionBid(auction.getPlayerBid(playerIndex));
        auction.incrementBiddingPosition();

        nextAuctionPhase();
    }

    public void IncreaseBid()
    {
        Debug.Log("Increase bid");
        AuctionHandler auction = State.getAuctionHandler();
        App app = UnityEngine.Object.FindObjectOfType<App>();
        int playerIndex = app.GetHumanIndex();
        Nation player = State.getNations()[playerIndex];
        auction.incrementPlayerTempBid(playerIndex);
        decreaseBid.interactable = true;

        //   TextMeshProUGUI playerBid = playerBidText.GetComponent<TextMeshProUGUI>();
        playersCurrentBid.text = auction.getTempHumanBidLevel().ToString();
     
        playerRemainingPoints.text = (player.GetColonialPoints() - auction.getTempHumanBidLevel()).ToString();
        Debug.Log("Colonial Points: " + player.GetColonialPoints());
        Debug.Log("Player Bid: " + auction.getPlayerBid(playerIndex));

        if (auction.tempHumanBidLevel >= player.GetColonialPoints() || player.RecognizingTheseClaims.Contains(auction.currentColonyIndex))
        {
            increaseBid.interactable = false;
        }
        else
        {
            increaseBid.interactable = true;
        }
        Debug.Log("Current high bid: " + State.CurrentColonyAuctionBid);
        if (auction.getTempHumanBidLevel() > State.CurrentColonyAuctionBid && !player.RecognizingTheseClaims.Contains(auction.currentColonyIndex))
        {
            bid.interactable = true;
        }
        else
        {
            bid.interactable = false;
        }
    }

    public void DecreaseBid()
    {
        Debug.Log("Decrease bid");
        AuctionHandler auction = State.getAuctionHandler();
        App app = UnityEngine.Object.FindObjectOfType<App>();
        int playerIndex = app.GetHumanIndex();
        Nation player = State.getNations()[playerIndex];
        auction.decreasePlayerTempBid(playerIndex);
        // TextMeshProUGUI playerBid = playerBidText.GetComponent<TextMeshProUGUI>();
        playersCurrentBid.text = auction.getTempHumanBidLevel().ToString();
        increaseBid.interactable = true;
        if (auction.getPlayerBid(playerIndex) == 0)
        {
            decreaseBid.interactable = false;
        }
        Debug.Log("Current high bid: " + State.CurrentColonyAuctionBid);
        if (auction.getTempHumanBidLevel() > State.CurrentColonyAuctionBid)
        {
            bid.interactable = true;
        }
        else
        {
            bid.interactable = false;
        }
        playerRemainingPoints.text = (player.GetColonialPoints() - auction.getTempHumanBidLevel()).ToString();
    
        Debug.Log("Colonial Points: " + player.GetColonialPoints());
        Debug.Log("Player Bid: " + auction.getPlayerBid(playerIndex));
        
    }



}

