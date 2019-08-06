using assemblyCsharp;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using WorldMapStrategyKit;

[System.Serializable]

public class AuctionHandler : MonoBehaviour {

    WMSK map;

    public int currentColonyIndex = 0;

    public int tempHumanBidLevel = 0;

    public List<int> colonySequence = new List<int>();

    public int indexOfCurrentItem = 0;
    //private int currentBid = 0;
    private int indexOfHighestBidderSoFar = -1;
    private bool open = false;

    private List<int> biddingOrder = new List<int>();
    private int currentBidPosition;

    private Dictionary<int, bool> playerPasses = new Dictionary<int, bool>();
    private Dictionary<int, int> playerBids = new Dictionary<int, int>();

    private void Start()
    {
        map = WMSK.instance;
    }

    public int CurrentBidPosition { get => currentBidPosition; set => currentBidPosition = value; }



    public void incrementColonyIndex()
    {
    //    Debug.Log(colonySequence.Count);
        Debug.Log(currentColonyIndex);
     //   indexOfCurrentItem = colonySequence[currentColonyIndex];
        if (currentColonyIndex < colonySequence.Count -1)
        {
            currentColonyIndex ++;
        }
        else
        {
            currentColonyIndex = 0;
        }
        indexOfCurrentItem = colonySequence[currentColonyIndex];
    }

    public void NewAuction()
    {
       // AuctionHandler auction = State.getAuctionHandler();
       if(State.turn < 7)
        {
            currentColonyIndex = 0;
        }
        indexOfHighestBidderSoFar = -1;
        State.CurrentColonyAuctionBid = 0;
        open = true;
        playerPasses.Clear();
        playerBids.Clear();
        tempHumanBidLevel = 0;
        List<int> majorNations = State.getMajorNations();
        biddingOrder = ShuffleIntList(majorNations);
        currentBidPosition = 0;
     //  Debug.Log("New Bidding order...");
        for (int i = 0; i < biddingOrder.Count; i++)
        {
          //  Debug.Log(biddingOrder[i]);
            playerPasses[biddingOrder[i]] = false;
            playerBids[biddingOrder[i]] = 0;
        }
        Nation currentItem = State.getNations()[indexOfCurrentItem];
        incrementColonyIndex();
        
    }


    private List<int> ShuffleIntList(List<int> inputList)
    {
        List<int> randomList = new List<int>();

        System.Random r = new System.Random();
        int randomIndex = 0;
        while (inputList.Count > 0)
        {
            randomIndex = r.Next(0, inputList.Count); //Choose a random object in the list
            randomList.Add(inputList[randomIndex]); //add it to the new, random list
            inputList.RemoveAt(randomIndex); //remove to avoid duplicates
        }

        return randomList; //return the new random list
    }


  

    public void newBid(Nation currentPlayer)
    {
        Nation currentItem = State.getNations()[indexOfCurrentItem];
        App app = UnityEngine.Object.FindObjectOfType<App>();
        Debug.Log("Time for " + currentPlayer.nationName + " to bid");
        Vector2 desireability = currentPlayer.getAI().resDesirabilityOfItem(currentPlayer, currentItem);
        int currentBid = State.CurrentColonyAuctionBid;
        bool willBid = currentPlayer.getAI().decideIfBidItem(currentPlayer, currentItem, currentBid, desireability);
        if (willBid)
        {
            int newBidAmount = currentPlayer.getAI().decideBidAmountItem(currentPlayer, currentItem, currentBid, desireability);
            State.CurrentColonyAuctionBid = newBidAmount;
            indexOfHighestBidderSoFar = currentPlayer.getIndex();
            playerBids[currentPlayer.getIndex()] = newBidAmount;
            Debug.Log("Decides to Bid :   " + newBidAmount);

        }
        else
        {
            Debug.Log("Decides to Pass");
            playerPasses[currentPlayer.getIndex()] = true;
        }
        ////Check if any more bids will be made
   
    }


  

    public void concludeAuction()
    {
        Debug.Log("Conclude Auction");
        Debug.Log(indexOfCurrentItem);
        Nation currentItem = State.getNations()[indexOfCurrentItem];
        Nation winner = State.getNations()[indexOfHighestBidderSoFar];
        int currentBid = State.CurrentColonyAuctionBid;
     
        if (currentItem.IsColonyOf() > -1)
        {
            Nation oldOverlord = State.getNations()[currentItem.IsColonyOf()];
            oldOverlord.removeColony(currentItem.getIndex());
            oldOverlord.adjustRelation(winner, -15);
            oldOverlord.decreasePrestige(2);
        }
        Debug.Log("Add " + currentItem.getName() + " as a colony of " + winner.getName());
        currentItem.MakeColonyOf(winner.getIndex());
        winner.addColony(currentItem.getIndex());
        currentItem.MakeColonyOf(winner.getIndex());
        winner.SpendColonialPoints(currentBid);
        winner.landForces.Strength--;
        winner.receiveGold(currentItem.getGold());
        currentItem.gold = 0;
        winner.increasePrestige(2);
        TopLevel topLevel = winner.getAI().getTopLevel();
    //    for (int i = 0; i < currentItem.getProvinces().Count; i++)
            foreach (int provIndex in currentItem.getProvinces())
        {
            assemblyCsharp.Province prov = State.getProvince(provIndex);
            prov.isColony = true;
            prov.coastal = true;
            winner.industry.increaseTransportCapacity(1);
            topLevel.alterPrioritiesBasedOnProvinceGain(winner, prov.getResource());

        }
        open = false;

        updateMiniMap();
        WMSK map = WMSK.instance;

        map.FlyToCountry(currentItem.getIndex());
        ///give item to the highest bidder  and collect his bid, unless the highest bid is 0
        ///in which case noone get the item (but it is still passed over until next cycle
        
    }

    public void updateMiniMap()
    {
        float left = 0.79f;
        float top = 0.02f;
        float width = 0.2f;
        float height = 0.2f;
        Vector4 normalizedScreenRect = new Vector4(left, top, width, height);
        WMSKMiniMap minimap = WMSKMiniMap.Show(normalizedScreenRect);
        minimap.map.earthStyle = EARTH_STYLE.SolidColor;
        //Eventually get from Scenario Folder
        //Texture2D miniMapTexture = Resources.Load("AlphaPrime", typeof(Texture2D)) as Texture2D;

        // minimap.map.earthTexture = miniMapTexture;
        minimap.map.fillColor = Color.blue;
        minimap.map.earthColor = Color.blue;
        minimap.duration = 1.5f;
        minimap.zoomLevel = 0.365f;

        WMSK map = WMSK.instance;
        for (int countryIndex = 0; countryIndex < map.countries.Length; countryIndex++)
        {
            Nation country = State.getNations()[countryIndex];
            if (country.IsColonyOf() > -1)
            {
                Nation owner = State.getNations()[country.IsColonyOf()];
                Color color = owner.getColor();
                minimap.map.ToggleCountrySurface(countryIndex, true, color);
            }
            else
            {
                Color color = country.getColor();
                minimap.map.ToggleCountrySurface(countryIndex, true, color);
            }
        }

    }

    public bool isOpen()
    {
        return this.open;
    }

    public bool checkIfAuctionIsOver()
    {
        int numberOfMajorsRemaining = State.getNumberOfMajorNations();
      //  int playersRemaining = 0;
        foreach(KeyValuePair<int, bool> entry in this.playerPasses)
        {
            if(entry.Value == true)
            {
                numberOfMajorsRemaining--;
            }
        }
        if(numberOfMajorsRemaining < 2)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

  

    public int getIndexCurrentCol()
    {
        return currentColonyIndex;
    }

 

    public int getPlayerBid(int id)
    {
        //  Debug.Log(this.playerBids[id]);
      //  Debug.Log(id);
        return this.playerBids[id];
    }


    public void incrementPlayerTempBid(int id)
    {
        this.tempHumanBidLevel++;
    }

    public void decreasePlayerTempBid(int id)
    {
        this.tempHumanBidLevel --;
    }

    public void setTempHumanBidLevel(int value)
    {
        this.tempHumanBidLevel = value;
    }

    public int getTempHumanBidLevel()
    {
        return this.tempHumanBidLevel;
    }

    public bool getIfPlayerPass(int id)
    {
        return playerPasses[id];
    }

    public void passOnBid(int id)
    {
        playerPasses[id] = true;
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


    public void determineColonySequence()
    {
        colonySequence.Clear();
        List<int> earlyList = new List<int>();
        List<int> midList = new List<int>();
        List<int> lateList = new List<int>();
        for (int i = 0; i < State.getNations().Count; i++)
        {
            Nation nation = State.getNations()[i];
            if (nation.getType() == MyEnum.NationType.oldMinor)
            {
               // Debug.Log(nation.getName());
                List<MyEnum.Resources> nationResources = new List<MyEnum.Resources>();
                foreach(int provIndex in nation.getProvinces())
                {
                    //Debug.Log(nation.getProvinces()[j]);
                    assemblyCsharp.Province prov = State.getProvince(provIndex);
                  //  Debug.Log(prov.getResource());
                    MyEnum.Resources resource = prov.getResource();
                    nationResources.Add(resource);
                }

                if (nationResources.Contains(MyEnum.Resources.oil))
                {
                //    Debug.Log("Add to late");
                    lateList.Add(nation.getIndex());
                    continue;
                }
                if (nationResources.Contains(MyEnum.Resources.rubber))
                {
              //      Debug.Log("Add to Middle");
                    midList.Add(nation.getIndex());
                    continue;
                }
                else
                {
                   // Debug.Log("Add to Early");
                    earlyList.Add(nation.getIndex());
                    continue;
                }

            }
        }
        Debug.Log("Early List Length: " + earlyList.Count);
        earlyList = RandomizeList(earlyList);
        midList = RandomizeList(midList);
        lateList = RandomizeList(lateList);
        Debug.Log("Early List Length: " + earlyList.Count);

        for (int i = 0; i < earlyList.Count; i++)
        {
            Nation nat = State.getNations()[earlyList[i]];
            colonySequence.Add(earlyList[i]);
           // Debug.Log("Adding: " + nat.getName());
        }
        for (int i = 0; i < midList.Count; i++)
        {
            Nation nat = State.getNations()[midList[i]];
            colonySequence.Add(midList[i]);
         //   Debug.Log("Adding: " + nat.getName());

        }
        for (int i = 0; i < lateList.Count; i++)
        {
            Nation nat = State.getNations()[lateList[i]];
            colonySequence.Add(lateList[i]);
         //   Debug.Log("Adding: " + nat.getName());

        }
        int first = colonySequence[0];
        Nation firstCol = State.getNations()[first];
        Debug.Log("First Col Seq: " + firstCol.getName());
        Debug.Log("Colony Sequence: " + colonySequence.Count + "++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");
    }

    public List<int> RandomizeList(List<int> Intlist)
    {
        List<int> copy = new List<int>(Intlist);
        int n = copy.Count;
        while (n > 1)
        {
            n--;
            // exclusive int range, add one since we remove one earlier
            int k = UnityEngine.Random.Range(0, n + 1);
            // store temporary value
            int value = copy[k];
            // swap without overwrite
            copy[k] = copy[n];
            copy[n] = value;
        }
        return copy;
    }


    public void setIndexOfCurrentItem(int index)
    {
        this.indexOfCurrentItem = index;
    }

    public int getIndexOfCurrentItem()
    {
        return this.indexOfCurrentItem;
    }

    public List<int> getBiddingOrder()
    {
        return this.biddingOrder;
    }

    public void incrementBiddingPosition()
    {
        currentBidPosition++;
        if (currentBidPosition == biddingOrder.Count)
        {
            currentBidPosition = 0;
        }
    }

 
}


