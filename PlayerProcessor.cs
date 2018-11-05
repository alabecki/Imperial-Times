using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using assemblyCsharp;
using System.Linq;
using System;

public static class PlayerProcessor{


    public static void processSignals(Nation player)
    {
        Debug.Log("Processing Signals");
        foreach(KeyValuePair<MyEnum.Resources, MyEnum.marketChoice> entry in MarketHelper.ResourceOfferBid)
        {
            if(entry.Value == MyEnum.marketChoice.offer)
            {
                for(int i = 0; i < MarketHelper.ResourceOfferBidAmount[entry.Key]; i++)
                {
                    
                    Signal newSignal = new Signal(MyEnum.marketChoice.offer, player.getIndex(), entry.Key, MyEnum.Goods.arms, 1, 
                        MarketHelper.ResourceOfferBidLevel[entry.Key]);
                    State.market.addResourceOffer(entry.Key, newSignal);
                }
            }
            if(entry.Value == MyEnum.marketChoice.bid)
            {
                for (int i = 0; i < MarketHelper.ResourceOfferBidAmount[entry.Key]; i++)
                {

                    Signal newSignal = new Signal(MyEnum.marketChoice.bid, player.getIndex(), entry.Key, MyEnum.Goods.arms, 1,
                        MarketHelper.ResourceOfferBidLevel[entry.Key]);
                    State.market.addResourceBid(entry.Key, newSignal);

                }
            }
        
        }

    foreach (KeyValuePair<MyEnum.Goods, MyEnum.marketChoice> entry in MarketHelper.GoodsOfferBid)
    {
        if (entry.Value == MyEnum.marketChoice.offer)
        {
            for (int i = 0; i < MarketHelper.GoodsOfferBidAmount[entry.Key]; i++)
            {

                Signal newSignal = new Signal(MyEnum.marketChoice.offer, player.getIndex(), MyEnum.Resources.coal, entry.Key, 1,
                    MarketHelper.GoodsOfferBidLevel[entry.Key]);
                    State.market.addGoodOffer(entry.Key, newSignal);
               
            }
        }
        if (entry.Value == MyEnum.marketChoice.bid)
        {
            for (int i = 0; i < MarketHelper.GoodsOfferBidAmount[entry.Key]; i++)
            {

                Signal newSignal = new Signal(MyEnum.marketChoice.bid, player.getIndex(), MyEnum.Resources.coal, entry.Key, 1,
                    MarketHelper.GoodsOfferBidLevel[entry.Key]);
                    State.market.addGoodBid(entry.Key, newSignal);

                }
            }

    }
        MarketHelper.ResourceOfferBid.Clear();
        MarketHelper.GoodsOfferBid.Clear();
}

    public static void determineLoveToHaveList(Nation player)
    {
        var sortedList = player.getRelations().OrderByDescending(r => r.Value.getAttitude()).ToList();
        // player.getRelations().Sort(new Comparison<StatInfo>((x, y) => getAttitude().Compare(x.attitude, y.attitude)));
        player.getLoveToHateList().Clear();
        int index = 0;
        foreach (var item in sortedList)
        {
            player.getLoveToHateList()[index] = item.Key;
            index++;
        }
    }




}
