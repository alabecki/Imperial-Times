using assemblyCsharp;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BidIncrement : MonoBehaviour {

    public Button increceBid;
    public Button decreaseBid;

    public Button bid;

   public  GameObject playerBidText;


	// Use this for initialization
	void Start () {
        increceBid.onClick.AddListener(delegate { IncreaseBid(); });
        decreaseBid.onClick.AddListener(delegate { DecreaseBid(); });


    }

    // Update is called once per frame
    void Update () {
		
	}


    public void IncreaseBid()
    {
        AuctionHandler auction = State.getAuctionHandler();
        App app = UnityEngine.Object.FindObjectOfType<App>();
        int playerIndex = app.GetHumanIndex();
        Nation player = State.getNations()[playerIndex];

        auction.incrementPlayerBid(playerIndex);

        TextMeshProUGUI playerBid = playerBidText.GetComponent<TextMeshProUGUI>();
        playerBid.text = auction.getPlayerBid(playerIndex).ToString();
        if (auction.getAuctionType() == MyEnum.auctionType.sphere) {
            if (auction.getPlayerBid(playerIndex) >= player.getDiplomacyPoints())
            {
                increceBid.gameObject.SetActive(false);
                decreaseBid.gameObject.SetActive(true);
            }
        }
        else
        {
            if (auction.getPlayerBid(playerIndex) >= player.GetColonialPoints())
            {
                increceBid.gameObject.SetActive(false);
                decreaseBid.gameObject.SetActive(true);
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


    public void DecreaseBid()
    {
        AuctionHandler auction = State.getAuctionHandler();
        App app = UnityEngine.Object.FindObjectOfType<App>();
        int playerIndex = app.GetHumanIndex();
        Nation player = State.getNations()[playerIndex];
        auction.decreasePlayerBid(playerIndex);
        TextMeshProUGUI playerBid = playerBidText.GetComponent<TextMeshProUGUI>();
        playerBid.text = auction.getPlayerBid(playerIndex).ToString();
        if (auction.getPlayerBid(playerIndex) == 0)
        {
            increceBid.gameObject.SetActive(true);
            decreaseBid.gameObject.SetActive(false);
        }
        if (auction.getPlayerBid(playerIndex) > auction.getCurrentBid())
        {
            bid.interactable = true;
        }
        else
        {
            bid.interactable = false;
        }
    }
}
