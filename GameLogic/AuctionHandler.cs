using assemblyCsharp;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuctionHandler : MonoBehaviour {


    private MyEnum.auctionType auctionType;
    private int currentSphereIndex = 0;
    private int currentColonyIndex = 0;

    private int indexOfCurrentItem;
    private int currentBid = 0;
    private int indexOfHighestBidderSoFar = -1;
    private bool open = false;


    private Dictionary<int, bool> playerPasses = new Dictionary<int, bool>();
    private Dictionary<int, int> playerBids = new Dictionary<int, int>();


    public void incrementSphereIndex()
    {
        if(currentSphereIndex < State.getSphereOrder().Count)
        {
            currentSphereIndex += 1;
        }
        else
        {
            currentSphereIndex = 0;
        }
    }


    public void incrementColonyIndex()
    {
        if (currentSphereIndex < State.getColonyOrder().Count)
        {
            currentColonyIndex += 1;
        }
        else
        {
            currentColonyIndex = 0;
        }
    }



    public void NewAuction()
    {
        AuctionHandler auction = State.getAuctionHandler();
        indexOfHighestBidderSoFar = -1;
        currentBid = 0;
        open = true;
        playerPasses.Clear();
        playerBids.Clear();

        foreach (int index in State.getTurnOrder())
        {
            playerPasses[index] = false;
            playerBids[index] = 0;
        }

        MyEnum.auctionType auctionType = MyEnum.auctionType.colony;
        if (State.turn / 2 == 1)
        {
            auction.setAuctionType(MyEnum.auctionType.sphere);
            newSphereAuction();
        }
        else
        {
            auction.setAuctionType(MyEnum.auctionType.colony);
            newColonyAuction();
        }
    }

    
    private void newSphereAuction()
    {
        indexOfCurrentItem = State.getSphereOrder()[currentSphereIndex];
        incrementSphereIndex();
        Nation currentItem = State.getNations()[indexOfCurrentItem];


    }

    private void newColonyAuction()
    {
        indexOfCurrentItem = State.getColonyOrder()[currentSphereIndex];
        Nation currentItem = State.getNations()[indexOfCurrentItem];
        incrementColonyIndex();
    }

    public bool acutionControlFlow()
    {
        bool done = false;
        AuctionHandler auction = State.getAuctionHandler();
        App app = UnityEngine.Object.FindObjectOfType<App>();
        Nation player = State.getNations()[app.GetHumanIndex()];
        while (done == false)
        {
            Nation currentPlayer = State.getNextPlayer();
            if (currentPlayer.getIndex() == player.getIndex())
            {
                return false;
            }
            else
            {
                done = auction.newBid(currentPlayer);
            }
        }
        if (done == true)
        {
            State.advanceGamePhase();
            return true;
        }
        return false;
    }


    public bool newBid(Nation currentPlayer)
    {
        Nation currentItem = State.getNations()[indexOfCurrentItem];
        App app = UnityEngine.Object.FindObjectOfType<App>();
        int newBid = -1;
        if (auctionType == MyEnum.auctionType.sphere)
        {    
            newBid = currentPlayer.getAI().makeBidSphere(currentItem);
        }
        if (auctionType == MyEnum.auctionType.colony)
        {
            newBid = currentPlayer.getAI().makeBidColony(currentItem);
        }
        if(newBid > currentBid)
        {
            currentBid = newBid;
            indexOfHighestBidderSoFar = currentPlayer.getIndex();
            playerBids[currentPlayer.getIndex()] = newBid;

        }
        else
        {
            playerPasses[currentPlayer.getIndex()] = true;
        }
        ////Check if any more bids will be made
        bool isOver = checkIfAuctionIsOver();
        if(isOver == true && currentBid > 0)
        {
            Nation winner = State.getNations()[indexOfHighestBidderSoFar];
            if(auctionType == MyEnum.auctionType.sphere)
            {
                if(currentItem.IsSphereOf() > -1)
                {
                    Nation oldOverlord = State.getNations()[currentItem.IsSphereOf()];
                    oldOverlord.removeSphere(currentItem.getIndex());
                }
                winner.addSphere(currentItem.getIndex());
                currentItem.MakeSphereOf(winner.getIndex());
                winner.useDiplomacyPoints(currentBid);
            }
            else
            {
                  if(currentItem.IsColonyOf() > -1)
                {
                    Nation oldOverlord = State.getNations()[currentItem.IsColonyOf()];
                    oldOverlord.removeColony(currentItem.getIndex());
                }
                winner.addColony(currentItem.getIndex());
                currentItem.MakeColonyOf(winner.getIndex());
                winner.SpendColonialPoints(currentBid);

            }
            ///give item to the highest bidder  and collect his bid, unless the highest bid is 0
            ///in which case noone get the item (but it is still passed over until next cycle
            open = false;
        }
        return isOver;
    }

    public bool isOpen()
    {
        return this.open;
    }


    public bool checkIfAuctionIsOver()
    {
        int playersRemaining = 0;
        foreach(KeyValuePair<int, bool> entry in this.playerPasses)
        {
            if(entry.Value == true)
            {
                playersRemaining += 1;
            }
        }
        if(playersRemaining < 2)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void setAuctionType(MyEnum.auctionType aType)
    {
        this.auctionType = aType;
    }

    public MyEnum.auctionType getAuctionType()
    {
        return this.auctionType;
    }

    public int getIndexCurrentCol()
    {
        return currentColonyIndex;
    }

    public int getIndexCurrentSphere()
    {
        return currentSphereIndex;
    }

    public int getPlayerBid(int id)
    {
        return this.playerBids[id];
    }


    public void incrementPlayerBid(int id)
    {
        this.playerBids[id] += 1;
    }

    public void decreasePlayerBid(int id)
    {
        this.playerBids[id] -= 1;
    }

    public bool getIfPlayerPass(int id)
    {
        return playerPasses[id];
    }

    public void passOnBid(int id)
    {
        playerPasses[id] = true;
    }

    public int getCurrentBid()
    {
        return this.currentBid;
    }

    public void increaseCurrentBid(int amount)
    {
        this.currentBid += amount;
    }

    public int getHighestBidderSoFar()
    {
        return this.indexOfHighestBidderSoFar;
    }

    public void setIndexOfHighestBidderSoFar(int id)
    {
        this.indexOfHighestBidderSoFar = id;
    }

    public void setPlayerBid(int id, int amount)
    {
        this.playerBids[id] = amount;
    }

    public void setCurrentBid(int amount)
    {
        this.currentBid = amount;
    }

}
