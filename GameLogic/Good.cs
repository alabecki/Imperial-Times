using assemblyCsharp;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Good {

    private MyEnum.Goods goodType;
    private float currentPrice = 5;
    private float minimumPrice = 2;
    private float maximumPrice = 10;
    private float initialPrice = 5;
    private List<float> priceHistory = new List<float>();

    private List<Signal> currentOffers = new List<Signal>();
    private List<Signal> currentBids = new List<Signal>();

    private int numberUnitsSold = 0;
    private int numberUnitsOffered = 0;
    private int numberUnitsBid = 0;

    private float averagePrice = 0.0f;

    private List<float> buysThisTurn = new List<float>();


    private int indexOfTopProducer;

    private bool easyToBuy = false;
    private bool beingOffered = false;

    public float InitialPrice { get => initialPrice; set => initialPrice = value; }
    public bool BeingOffered { get => beingOffered; set => beingOffered = value; }

    public Good()
    {

    }

    public Good(MyEnum.Goods resType, float minPrice, float maxPrice, float initialPrice)
    {
        this.goodType = resType;
        this.minimumPrice = minPrice;
        this.maximumPrice = maxPrice;
        this.initialPrice = initialPrice;
    }

    public float getMinPrice()
    {
        return this.minimumPrice;
    }

    public float getMaxPrice()
    {
        return this.maximumPrice;
    }


    public void setPrice(float price)
    {
        currentPrice = price;
    }

    public void adjustPrice(float change)
    {
        currentPrice += change;
    }

    public float getPrice()
    {
        return currentPrice;
    }

    public void augmentHistory(float newPrice)
    {
        int turn = State.turn;
        //  this.priceHistory[turn + 1] = newPrice;
        this.priceHistory.Add(newPrice);
    }

    public List<float> getHistory()
    {
      
        return priceHistory;
    }

    public void addOffer(Signal offer)
    {
        this.currentOffers.Add(offer);
    }


    public void addBid(Signal bid)
    {
        this.currentBids.Add(bid);
    }

    public void removeOffer(Signal offer)
    {
        this.currentOffers.Remove(offer);
    }

    public void removeBid(Signal bid)
    {
        this.currentBids.Remove(bid);
    }

    public List<Signal> getOffers()
    {
        return this.currentOffers;
    }

    public List<Signal> getBids()
    {
        return this.currentBids;
    }

    public void addToNumberOfUnitsSold()
    {
        this.numberUnitsSold += 1;
    }

    public int getNumberOfUnitsSold()
    {
        return this.numberUnitsSold;
    }

    public void clearDataForNewTurn()
    {
        this.numberUnitsSold = 0;
        this.numberUnitsOffered = 0;
        this.buysThisTurn.Clear();
     

    }

    public void returnUnsoldGoods()
    {
        foreach(Signal signal in currentOffers)
        {
            if (signal.getType() == MyEnum.marketChoice.offer  && !signal.isProcessed())
            {
                int ownerIndex = signal.getOwnerIndex();
                Nation owner = State.getNations()[ownerIndex];
                MyEnum.Goods goodType = signal.getGoodType();
                owner.collectGoods(goodType, 1);
            }
        }
    }

    public void clearDataForNextTurn()
    {
        this.currentBids.Clear();
        this.currentOffers.Clear();
       // Debug.Log("Current bids: " + currentBids.Count + "  Current offers: " + currentOffers.Count);
    }

    public void setNumberOfUnitsOffered(int num)
    {
        this.numberUnitsOffered = num;
    }

    public int getNumberOfUnitsOffered()
    {
        return this.numberUnitsOffered;
    }


    public void setNumberOfUnitsBid(int num)
    {
        this.numberUnitsBid = num;
    }

    public int getNumberOfUnitsBid()
    {
        return this.numberUnitsBid;
    }

    public void randomizeSignalOrder()
    {
        foreach (MyEnum.Goods res in Enum.GetValues(typeof(MyEnum.Goods)))
        {
            State.ListShuffle<Signal>(this.currentOffers);
            State.ListShuffle<Signal>(this.currentBids);
        }
    }

    public void addBuyThisTurn(float price)
    {
        buysThisTurn.Add(price);
    }

    public void updatePrice()
    {
        int numberOfOffers = getNumberOfUnitsOffered();
        int numberSold = getNumberOfUnitsSold();
        int numberOfBids = getNumberOfUnitsBid();
        if (numberOfOffers >= 2 && numberSold / numberOfOffers < 0.666)
        {
            easyToBuy = true;
        }
        else
        {
            easyToBuy = false;
        }

        if (numberOfOffers >= 2)
        {
            beingOffered = true;
        }
        else
        {
            beingOffered = false;
        }

        float oldPrice = getPrice();
        Debug.Log("Number of Offers: " + numberOfOffers + " ==============================================");
        Debug.Log("Number Bids: " + numberOfBids);
        Debug.Log("Old Price: " + oldPrice);
        float change = ((numberOfBids + 0.01f) - (numberOfOffers + 0.01f)) /
            (numberOfBids + numberOfOffers + 0.01f);
        Debug.Log("Price Change " + change);
        if (change > 0.25f)
        {
            change = 0.25f;
        }
        if (change < -0.25f)
        {
            change = -0.25f;
        }

        float newPrice = oldPrice + change;
        newPrice = (float)Math.Round(newPrice, 2);

        if (newPrice < minimumPrice)
        {
            setPrice(minimumPrice);
        }
        else if (newPrice > maximumPrice)
        {
            setPrice(maximumPrice);
        }
        else
        {
            setPrice(newPrice);
            Debug.Log("New price: " + newPrice);
        }

        augmentHistory(getPrice());
      //  Debug.Log("History length: " + getHistory().Count);

    }

    public void clearOffers()
    {
        currentOffers.Clear();
    }

    public void clearBids()
    {
        currentBids.Clear();
    }

    public void setEasyToBuy(bool value)
    {
        this.easyToBuy = value;
    }

    public bool getEasyToBuy()
    {
        return easyToBuy;
    }

  

}
