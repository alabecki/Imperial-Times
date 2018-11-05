using assemblyCsharp;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Good {

    private MyEnum.Goods goodType;
    private float currentPrice;
    private Dictionary<int, float> priceHistory = new Dictionary<int, float>();

    private List<Signal> currentOffers = new List<Signal>();
    private List<Signal> currentBids = new List<Signal>();

    private int numberUnitsSold;
    private int numberUnitsOffered;
    private float averagePrice;
    


    private int indexOfTopProducer;


    public Good(MyEnum.Goods resType)
    {
        this.goodType = resType;
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
        this.priceHistory[turn] = newPrice;
    }

    public List<float> getHistory()
    {
        List<float> price_history = new List<float>();
        for (int i = 1; i <= price_history.Count; i++)
        {
            price_history.Add(priceHistory[i]);
        }
        return price_history;
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
        return this.currentBids;
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
    }

    public void setNumberOfUnitsOffered(int num)
    {
        this.numberUnitsOffered = num;
    }

    public int getNumberOfUnitsOffered()
    {
        return this.numberUnitsOffered;
    }

    public void randomizeSignalOrder()
    {
        foreach (MyEnum.Goods res in Enum.GetValues(typeof(MyEnum.Goods)))
        {
            State.ListShuffle<Signal>(this.currentOffers);
            State.ListShuffle<Signal>(this.currentBids);
        }
    }

}
