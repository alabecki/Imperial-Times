using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using assemblyCsharp;

public class BidPass : MonoBehaviour {

    public Button pass;
    public Button bid;

    bool playerTurn;

	// Use this for initialization
	void Start () {
        pass.onClick.AddListener(delegate { playerPass(); });
        bid.onClick.AddListener(delegate { playerBid(); });

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void playerPass()
    {
        AuctionHandler auction = State.getAuctionHandler();
       App app = UnityEngine.Object.FindObjectOfType<App>();
        int playerIndex = app.GetHumanIndex();
        auction.passOnBid(playerIndex);

       // Nation player = State.getNations()[playerIndex];
        playerTurn = false;
    }

    private void playerBid()
    {
        AuctionHandler auction = State.getAuctionHandler();
        App app = UnityEngine.Object.FindObjectOfType<App>();
        int playerIndex = app.GetHumanIndex();

        auction.setIndexOfHighestBidderSoFar(playerIndex);
        auction.setCurrentBid(auction.getPlayerBid(playerIndex));
        playerTurn = false;
    }


}
