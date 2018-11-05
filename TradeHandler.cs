using assemblyCsharp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TradeHandler
{

    private enum marketSit { buyers, sellers};

    private Dictionary<MyEnum.Resources, marketSit> marketSituationForResources =
        new Dictionary<MyEnum.Resources, marketSit>();

    private Dictionary<MyEnum.Goods, marketSit> marketSituationForGoods =
       new Dictionary<MyEnum.Goods, marketSit>();



    public void handleTrades()
    {
        Market market = State.market;
        market.clearLastTurnActivityInfo();
        market.randomizeSignalOrder();

        foreach(Nation nat in State.getNations().Values)
        {
            nat.clearImportAndExportValues();
        }
        //For each resource/good, first determine whether it is a sellers market or buyers market.
        setMarketStateForBuyersSellers();
        //For each resource/good, 
        //If it is sellers market, go through players who have made offers by turn order, each selling a single item, and 
        //rounding back again until all offered items are sold. 
        //see first to the nation it likes the most, then second most, and so on, until its offers are all purchased
        //If it is buyers market, go through each players who has made bids by turn order, each buying a single item, and
        //rounding back again until all bids have been fulfilled.
        recordNumOffers();
        performResourceTrades();
        performGoodsTrades();
        clearOffersAndBids();

        
    }

    //For each resource/good, 
    //If it is sellers market, go through players who have made bids by turn order, each selling a single item, and 
    //rounding back again until all offered items are sold. 

    private void performResourceTrades()
    {
        Market market = State.market;
        foreach (MyEnum.Resources resource in Enum.GetValues(typeof(MyEnum.Resources)))
        {
            Debug.Log("Resource is " + resource);
            if (marketSituationForResources[resource] == marketSit.sellers)
            {
                for (int i = 0; i < market.getResourceOffers(resource).Count; i++)
                {
                    Signal offer = market.getResourceOffers(resource)[i];
                    Nation seller = State.getNations()[offer.getOwnerIndex()];
                    Dictionary<int, int> possibleBuyers = new Dictionary<int, int>();
                    for (int j = 0; j < market.getResourceBids(resource).Count; j++)
                    {
                        Nation bidder = State.getNations()[market.getResourceBids(resource)[j].getOwnerIndex()];
                        if (State.market.getResourceBids(resource)[j].getOffBidLevel() == MyEnum.OffBidLevels.high)
                        {
                            possibleBuyers.Add(j, seller.getRelations()[bidder.getIndex()].getAttitude() + 25);
                        }
                        else if (market.getResourceBids(resource)[j].getOffBidLevel() == MyEnum.OffBidLevels.low)
                        {
                            possibleBuyers.Add(j, seller.getRelations()[bidder.getIndex()].getAttitude() - 25);
                        }
                        else
                        {
                            possibleBuyers.Add(j, seller.getRelations()[bidder.getIndex()].getAttitude());
                        }
                    }
                    possibleBuyers = possibleBuyers.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
                    int chosenBuyerIndex = possibleBuyers.Keys.First();
                    Nation chosenBuyer = State.getNations()[chosenBuyerIndex];
                    Signal bid = market.getResourceBids(resource)[0];
                    foreach (Signal signal in market.getResourceBids(resource))
                    {
                        if (signal.getOwnerIndex() == chosenBuyer.getIndex())
                        {
                            bid = signal;
                            break;
                        }
                    }
                    performResourceTransaction(seller, chosenBuyer, offer, bid);
                }
            }
            if (marketSituationForResources[resource] == marketSit.buyers)
            {
                for (int i = 0; i < market.getResourceBids(resource).Count; i++)
                {
                    Signal bid = market.getResourceBids(resource)[i];
                    Nation bidder = State.getNations()[bid.getOwnerIndex()];
                    Dictionary<int, int> possibleSellers = new Dictionary<int, int>();
                    for (int j = 0; j < market.getResourceOffers(resource).Count; j++)
                    {
                        Nation seller = State.getNations()[market.getResourceOffers(resource)[j].getOwnerIndex()];
                        possibleSellers.Add(j, bidder.getRelations()[seller.getIndex()].getAttitude());
                    }
                    possibleSellers = possibleSellers.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
                    int chosenSellerIndex = possibleSellers.Keys.First();
                    Nation chosenSeller = State.getNations()[chosenSellerIndex];
                    Signal offer = market.getResourceBids(resource)[0];
                    foreach (Signal signal in market.getResourceOffers(resource))
                    {
                        if (signal.getOwnerIndex() == chosenSeller.getIndex())
                        {
                            offer = signal;
                            break;
                        }
                    }
                    performResourceTransaction(chosenSeller, bidder, offer, bid);

                }
            }
        }

    }


    private void performGoodsTrades()
    {
        Market market = State.market;
        foreach (MyEnum.Goods good in Enum.GetValues(typeof(MyEnum.Goods)))
        {
            Debug.Log("Good is " + good);
            if (marketSituationForGoods[good] == marketSit.sellers)
            {
                for (int i = 0; i < market.getGoodOffers(good).Count; i++)
                {
                    Signal offer = market.getGoodOffers(good)[i];
                    Nation seller = State.getNations()[offer.getOwnerIndex()];
                    Dictionary<int, int> possibleBuyers = new Dictionary<int, int>();
                    for (int j = 0; j < market.getGoodBids(good).Count; j++)
                    {
                        Nation bidder = State.getNations()[market.getGoodBids(good)[j].getOwnerIndex()];
                        if (State.market.getGoodBids(good)[j].getOffBidLevel() == MyEnum.OffBidLevels.high)
                        {
                            possibleBuyers.Add(j, seller.getRelations()[bidder.getIndex()].getAttitude() + 25);
                        }
                        else if (market.getGoodBids(good)[j].getOffBidLevel() == MyEnum.OffBidLevels.low)
                        {
                            possibleBuyers.Add(j, seller.getRelations()[bidder.getIndex()].getAttitude() - 25);
                        }
                        else
                        {
                            possibleBuyers.Add(j, seller.getRelations()[bidder.getIndex()].getAttitude());
                        }
                    }
                    possibleBuyers = possibleBuyers.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
                    int chosenBuyerIndex = possibleBuyers.Keys.First();
                    Nation chosenBuyer = State.getNations()[chosenBuyerIndex];
                    Signal bid = market.getGoodBids(good)[0];
                    foreach (Signal signal in market.getGoodBids(good))
                    {
                        if (signal.getOwnerIndex() == chosenBuyer.getIndex())
                        {
                            bid = signal;
                            break;
                        }
                    }
                    performGoodTransaction(seller, chosenBuyer, offer, bid);
                }
            }
            if (marketSituationForGoods[good] == marketSit.buyers)
            {
                for (int i = 0; i < market.getGoodBids(good).Count; i++)
                {
                    Signal bid = market.getGoodBids(good)[i];
                    Nation bidder = State.getNations()[bid.getOwnerIndex()];
                    Dictionary<int, int> possibleSellers = new Dictionary<int, int>();
                    for (int j = 0; j < market.getGoodOffers(good).Count; j++)
                    {
                        Nation seller = State.getNations()[market.getGoodOffers(good)[j].getOwnerIndex()];
                        possibleSellers.Add(j, bidder.getRelations()[seller.getIndex()].getAttitude());
                    }
                    possibleSellers = possibleSellers.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
                    int chosenSellerIndex = possibleSellers.Keys.First();
                    Nation chosenSeller = State.getNations()[chosenSellerIndex];
                    Signal offer = market.getGoodBids(good)[0];
                    foreach (Signal signal in market.getGoodOffers(good))
                    {
                        if (signal.getOwnerIndex() == chosenSeller.getIndex())
                        {
                            offer = signal;
                            break;
                        }
                    }
                    performGoodTransaction(chosenSeller, bidder, offer, bid);

                }
            }
        }
    }



    private void setMarketStateForBuyersSellers()
    {
        foreach (MyEnum.Resources resource in Enum.GetValues(typeof(MyEnum.Resources)))
        {
            if (State.market.getResourceBids(resource).Count > State.market.getResourceOffers(resource).Count)
            {
                marketSituationForResources[resource] = marketSit.sellers;
            }
            else
            {
                marketSituationForResources[resource] = marketSit.buyers;
            }
        }

        foreach (MyEnum.Goods good in Enum.GetValues(typeof(MyEnum.Goods)))
        {
            if (State.market.getGoodBids(good).Count > State.market.getGoodOffers(good).Count)
            {
                marketSituationForGoods[good] = marketSit.sellers;
            }
            else
            {
                marketSituationForGoods[good] = marketSit.buyers;
            }
        }
    }

    private void performResourceTransaction(Nation seller, Nation buyer, Signal offer, Signal bid)
    {
        Market market = State.market;
        MyEnum.Resources res = offer.getResourceType();
        float price = market.getPriceOfResource(res);
        if(bid.getOffBidLevel() == MyEnum.OffBidLevels.high)
        {
            price = price * 1.25f;
        }
        if (bid.getOffBidLevel() == MyEnum.OffBidLevels.low)
        {
            price = price * 75f;
        }
        seller.receiveGold(price);
        buyer.payGold(price);
        seller.consumeResource(res, 1);
        buyer.collectResource(res, 1);
        market.removeResourceBid(res, bid);
        market.removeResourceOffer(res, offer);
        market.addResourceSold(res);
        seller.addToExportValue(price);
        buyer.addToImportValue(price);
        bid.setPriceSold(price);
        offer.setPriceSold(price);
        seller.addExport(bid);
        buyer.addImport(offer);

    }


    private void performGoodTransaction(Nation seller, Nation buyer, Signal offer, Signal bid)
    {
        Market market = State.market;
        MyEnum.Goods good = offer.getGoodType();
        float price = market.getPriceOfGood(good);
        if (bid.getOffBidLevel() == MyEnum.OffBidLevels.high)
        {
            price = price * 1.25f;
        }
        if (bid.getOffBidLevel() == MyEnum.OffBidLevels.low)
        {
            price = price * 75f;
        }
        seller.receiveGold(price);
        buyer.payGold(price);
        seller.consumeGoods(good, 1);
        buyer.collectGoods(good, 1);
        market.removeGoodBid(good, bid);
        market.removeGoodOffer(good, offer);
        market.addGoodSold(good);
        seller.addToExportValue(price);
        buyer.addToImportValue(price);
        bid.setPriceSold(price);
        offer.setPriceSold(price);
        seller.addExport(bid);
        buyer.addImport(offer);

    }


    private void recordNumOffers()
    {
        Market market = State.market;
        foreach (MyEnum.Resources resource in Enum.GetValues(typeof(MyEnum.Resources)))
        {
            int numOffered = market.getResourceOffers(resource).Count;
            market.setResourceOffered(resource, numOffered);
        }
        foreach(MyEnum.Goods good in Enum.GetValues(typeof(MyEnum.Goods)))
        {
            int numOffered = market.getGoodOffers(good).Count;
            market.setNumberOfGoodOffered(good, numOffered);
        }
    }


    private void clearOffersAndBids()
    {
        Market market = State.market;

        foreach (MyEnum.Resources resource in Enum.GetValues(typeof(MyEnum.Resources)))
        {
            market.getResourceBids(resource).Clear();
            market.getResourceOffers(resource).Clear();
        }

        foreach (MyEnum.Goods good in Enum.GetValues(typeof(MyEnum.Goods)))
        {
            market.getGoodBids(good).Clear();
            market.getGoodOffers(good).Clear();
        }

    }







}
