using EasyUIAnimator;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using assemblyCsharp;
using UI.Tables;

public class TradeResultButton : MonoBehaviour {


    public GameObject tradeResultsPanel;

    public GameObject AuctionPanel;

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

    public GameObject currentBidAmount;

    public Image PointTypeImage;
    public Image itemFlag;

    public Button tradeResultButton;
    public Text currentNumberOfBiddingPoints;

    public Text ministerText;
    public Text diplomatText;



    bool playerTurn;


    private UIAnimation tradeResultsExit;
    private UIAnimation auctionEnter;
    private UIAnimation  auctionFadeIn;

    private Graphic[] auctionRect;


    // Use this for initialization
    void Start()
    {

        AuctionPanel.SetActive(false);

        tradeResultButton.onClick.AddListener(delegate { contiueToAuction(); });

        RectTransform tradeResultsRect = tradeResultsPanel.GetComponent<RectTransform>();
        tradeResultsExit = UIAnimator.Move(tradeResultsRect, new Vector2(0.5f, 0.48f), new Vector2(0.5f, 0.0f), 1f).SetModifier(Modifier.Linear);

        //RectTransform auctionPanelRect = AuctionPanel.GetComponent<RectTransform>();
        // auctionEnter = UIAnimator.

        auctionRect = AuctionPanel.GetComponentsInChildren<Graphic>();


    }
        // Update is called once per frame
        void Update () {
		
	}


    public void contiueToAuction()
    {
        App app = UnityEngine.Object.FindObjectOfType<App>();
        int playerIndex = app.GetHumanIndex();
        Nation player = State.getNations()[playerIndex];

        tradeResultsExit.Play();
        State.advanceGamePhase();
        Debug.Log("GamePhase is: " + State.GetPhase().ToString());
        if (State.turn > 2)
        {
            AuctionHandler auction = State.getAuctionHandler();
            auction.NewAuction();
            prepareAuctionPanel(auction);
            openAuctionPanel();
            bool done = false;
            playerTurn = false;
            while(done == false)
            {
                done = auction.acutionControlFlow();
                updateAuctionPanel();

                if (auction.getIfPlayerPass(playerIndex))
                {
                    playerTurn = false;
                }
                else
                {
                    playerTurn = true;
                }
     
                while(playerTurn == true)
                {
                    
                    //Loop will be borken by pressing the Bid or Pass Button
                }

                //show result
                updateAuctionPanel();

            }
        }


        foreach (Graphic graphic in auctionRect)
        {
            auctionFadeIn = UIAnimator.FadeIn(graphic, 1.4f);
            auctionFadeIn.Play();
        }
    }


    private void openAuctionPanel()
    {
        foreach (Graphic graphic in auctionRect)
        {
            auctionFadeIn = UIAnimator.FadeIn(graphic, 1.4f);
            auctionFadeIn.Play();
        }
    }


    private void updateAuctionPanel()
    {
        App app = UnityEngine.Object.FindObjectOfType<App>();
        int playerIndex = app.GetHumanIndex();
        Nation player = State.getNations()[playerIndex];
        AuctionHandler auction = State.getAuctionHandler();

        TextMeshProUGUI currentPlayerBid = currentBidAmount.GetComponent<TextMeshProUGUI>();
        currentPlayerBid.text = auction.getPlayerBid(playerIndex).ToString();

        if (auction.getPlayerBid(playerIndex) > 0)
        {
            decreaseBid.gameObject.SetActive(true);
        }
        else
        {
            decreaseBid.gameObject.SetActive(false);

        }

        if (auction.getAuctionType() == MyEnum.auctionType.colony)
        {
            if (player.getDiplomacyPoints() <= auction.getPlayerBid(playerIndex))
            {
                increaseBid.gameObject.SetActive(false);
            }

        }

        for (int i = 0; i < State.getNations().Count; i++)
        {
            Nation currNat = State.getNations()[State.getTurnOrder()[i]];
            if (currNat.getType() == MyEnum.NationType.major)
            {
                TableRow currentRow = biddingTable.Rows[i];
                int currNatIndex = currNat.getIndex();
                int currentNatBid = auction.getPlayerBid(currNatIndex);
                currentRow.Cells[0].GetComponentInChildren<Text>().text = currentPlayerBid.ToString();
                Transform res2 = currentRow.Cells[3].transform.GetChild(0);
                Image resImg2 = res2.GetComponent<Image>();
                if (auction.getIfPlayerPass(currNatIndex) == true)
                {
                    resImg2.sprite = Resources.Load("Sprites/GUI/Dark_Red_x" + currNat.getNationName().ToString(), typeof(Sprite)) as Sprite;

                }
                else
                {
                    resImg2.sprite = Resources.Load("Sprites/GUI/AuctionHammer" + currNat.getNationName().ToString(), typeof(Sprite)) as Sprite;

                }
                resImg2.sprite = Resources.Load("Sprites/GUI/AuctionHammer" + currNat.getNationName().ToString(), typeof(Sprite)) as Sprite;

            }
        }
        if(auction.getPlayerBid(playerIndex) > auction.getCurrentBid())
        {
            bid.interactable = true;
        }
        else
        {
            bid.interactable = false;
        }
    }

    private void prepareAuctionPanel(AuctionHandler auction)
    {
        App app = UnityEngine.Object.FindObjectOfType<App>();
        int playerIndex = app.GetHumanIndex();
        Nation player = State.getNations()[playerIndex];
        TextMeshProUGUI title = AuctionPanelTitleText.GetComponent<TextMeshProUGUI>();
        Sprite background = AuctionPanel.GetComponent<Image>().sprite;
        Nation item = State.getNations()[0];
        if (auction.getAuctionType() == MyEnum.auctionType.sphere)
        {
            background = Resources.Load("Sprites/Backgrounds/imperalism_cartoon", typeof(Sprite)) as Sprite;
            title.text = "Sphere Bids";
            PointTypeImage.sprite = Resources.Load("Sprites/GUI/crown", typeof(Sprite)) as Sprite;
            item = State.getNations()[auction.getIndexCurrentSphere()];
            itemFlag.sprite = Resources.Load("Flags/" + item.getNationName(), typeof(Sprite)) as Sprite;
            currentNumberOfBiddingPoints.text = player.DiplomacyPoints.ToString();
            if(player.getDiplomacyPoints() < 1)
            {
                increaseBid.interactable = false;
            }
            else
            {
                increaseBid.interactable = true;
            }
        }
        else
        {
            background = Resources.Load("Sprites/Backgrounds/Berlin_und_Rom", typeof(Sprite)) as Sprite;
            title.text = "Colony Bids";
            PointTypeImage.sprite = Resources.Load("Sprites/GUI/eagle", typeof(Sprite)) as Sprite;
            item = State.getNations()[auction.getIndexCurrentCol()];
            itemFlag.sprite = Resources.Load("Flags/" + item.getNationName(), typeof(Sprite)) as Sprite;
            currentNumberOfBiddingPoints.text = player.GetColonialPoints().ToString();
            if (player.GetColonialPoints() < 1)
            {
                increaseBid.interactable = false;
            }
            else
            {
                increaseBid.interactable = true;
            }
        }

        TextMeshProUGUI itemName = ItemNameText.GetComponent<TextMeshProUGUI>();
        itemName.text = item.getNationName();
        TextMeshProUGUI currentPlayerBid = currentBidAmount.GetComponent<TextMeshProUGUI>();
        currentPlayerBid.text = "00";

        Province firstProvince = State.getProvinces()[item.getProvinces()[0]];
        itemProvinces.Rows[1].Cells[0].GetComponentInChildren<Text>().text = firstProvince.getProvName();
      //  itemProvinces.Rows[1].Cells[1].GetComponentInChildren<Image>().sprite = 
        Sprite provOneRes = itemProvinces.Rows[1].Cells[1].GetComponentInChildren<Image>().sprite;
         provOneRes = Resources.Load("Resource/" + firstProvince.getResource().ToString(), typeof(Sprite)) as Sprite;
        itemProvinces.Rows[1].Cells[2].GetComponentInChildren<Text>().text = firstProvince.getDevelopmentLevel().ToString();
        itemProvinces.Rows[1].Cells[3].GetComponentInChildren<Text>().text = firstProvince.getInfrastructure().ToString();

        Province secondProvince = State.getProvinces()[item.getProvinces()[1]];
        itemProvinces.Rows[2].Cells[0].GetComponentInChildren<Text>().text = secondProvince.getProvName();
        //  itemProvinces.Rows[1].Cells[1].GetComponentInChildren<Image>().sprite = 
        Sprite provTwoRes = itemProvinces.Rows[2].Cells[1].GetComponentInChildren<Image>().sprite;
        provTwoRes = Resources.Load("Resource/" + secondProvince.getResource().ToString(), typeof(Sprite)) as Sprite;
        itemProvinces.Rows[2].Cells[2].GetComponentInChildren<Text>().text = secondProvince.getDevelopmentLevel().ToString();
        itemProvinces.Rows[2].Cells[3].GetComponentInChildren<Text>().text = secondProvince.getInfrastructure().ToString();

        //Create Table of Major Nations set to Bid
        biddingTable.ClearRows();
        for(int i = 0; i < State.getNations().Count; i++)
        {
            Nation currNat = State.getNations()[State.getTurnOrder()[i]];
            if(currNat.getType() == MyEnum.NationType.major)
            {
                TableRow newRow = Instantiate<TableRow>(biddingRow);
                newRow.gameObject.SetActive(true);
                newRow.preferredHeight = 30;
                newRow.name = currNat.getIndex().ToString();
                biddingTable.AddRow(newRow);
                biddingConnector.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (biddingTable.transform as RectTransform).rect.height);

                newRow.Cells[0].GetComponentInChildren<Text>().text = "00";
                newRow.Cells[1].GetComponentInChildren<Text>().text = currNat.getNationName().ToString();
                Transform res = newRow.Cells[2].transform.GetChild(0);
                Image resImg = res.GetComponent<Image>();
                resImg.sprite = Resources.Load("Flags/" + currNat.getNationName().ToString(), typeof(Sprite)) as Sprite;
                Transform res2 = newRow.Cells[3].transform.GetChild(0);
                Image resImg2 = res2.GetComponent<Image>();
                resImg2.sprite = Resources.Load("Sprites/GUI/AuctionHammer" + currNat.getNationName().ToString(), typeof(Sprite)) as Sprite;

            }
        }

        decreaseBid.gameObject.SetActive(false);

    }

    private void declareAuctionResults()
    {
        App app = UnityEngine.Object.FindObjectOfType<App>();
        int playerIndex = app.GetHumanIndex();
        Nation player = State.getNations()[playerIndex];
        AuctionHandler auction = State.getAuctionHandler();
        Nation winner = State.getNations()[auction.getHighestBidderSoFar()];
        Nation item = State.getNations()[0];
        if (auction.getAuctionType() == MyEnum.auctionType.colony)
        {
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

        if (auction.getAuctionType() == MyEnum.auctionType.sphere)
        {
            item = State.getNations()[auction.getIndexCurrentSphere()];
            item = State.getNations()[auction.getIndexCurrentCol()];
            if (winner.getIndex() == player.getIndex())
            {
                diplomatText.text = "Congratulations! The wise people " + item.getNationName() + " now look toward us for direction " +
                    "in matters of statecraft, culture, industrial development, and commerce";
                //  ministerPanel.SetActive(true);
            }
            else
            {
                diplomatText.text = "It looks like the ingrates of " + item.getNationName() + " have fallen under the infulence of " +
                    "the hornswogglers of " + winner.getNationName();

            }
            diplomatPanel.SetActive(true);
        }
    }

}

