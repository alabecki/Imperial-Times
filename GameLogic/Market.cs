using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Market {


    private Dictionary<MyEnum.Resources, Resource> Resources =
        new Dictionary<MyEnum.Resources,  Resource>();

    private Dictionary<MyEnum.Goods, Good> Goods =
       new Dictionary<MyEnum.Goods, Good>();



    public float getPriceOfResource(MyEnum.Resources resource)
    {
        return this.Resources[resource].getPrice();
    }

    public float getPriceOfGood(MyEnum.Goods good)
    {
        
        return this.Goods[good].getPrice();

    }


    public void recordResourcePrice(MyEnum.Resources resource, float price, int turn)
    {

        this.Resources[resource].augmentHistory(price);
        this.Resources[resource].setPrice(price);

    }

    public void recordGoodsPrice(MyEnum.Goods good, float price, int turn)
    {
        this.Goods[good].augmentHistory(price);
        this.Goods[good].setPrice(price);
    }


    public void InitializeMarket()
    {
        foreach (MyEnum.Resources resource in Enum.GetValues(typeof(MyEnum.Resources)))
        {
            this.Resources[resource] = new Resource(resource);
            this.Resources[resource].augmentHistory(3);
          
        }
        foreach (MyEnum.Goods good in Enum.GetValues(typeof(MyEnum.Goods)))
        {
            this.Goods[good] = new Good(good);
            if(good == MyEnum.Goods.arms || good == MyEnum.Goods.clothing || good == MyEnum.Goods.furniture || good == MyEnum.Goods.parts)
            {
                Goods[good].setPrice(7);
                Goods[good].augmentHistory(7);
            }
            if(good == MyEnum.Goods.chemicals || good == MyEnum.Goods.fabric || good == MyEnum.Goods.lumber || 
                good == MyEnum.Goods.steel || good == MyEnum.Goods.paper)
            {
                Goods[good].setPrice(5);
                Goods[good].augmentHistory(5);
            }
        }
        
    }

   public void setResourceOffered(MyEnum.Resources res, int amount)
    {
        this.Resources[res].setNumberOfUnitsOffered(amount);
    }

    public void setNumberOfGoodOffered(MyEnum.Goods good, int amount)
    {
        this.Goods[good].setNumberOfUnitsOffered(amount);
    }


    public int getResourceOffered(MyEnum.Resources res)
    {
        return this.Resources[res].getNumberOfUnitsOffered();
    }

    public int getNumberOfGoodOffered(MyEnum.Goods good)
    {
        return this.Goods[good].getNumberOfUnitsOffered();

    }

    public void clearLastTurnActivityInfo()
    {
        foreach (MyEnum.Resources res in Enum.GetValues(typeof(MyEnum.Resources)))
        {
            Resources[res].clearDataForNewTurn();
        }
        foreach (MyEnum.Goods good in Enum.GetValues(typeof(MyEnum.Goods)))
        {
            Goods[good].clearDataForNewTurn();
        }
    }

    public void addResourceSold(MyEnum.Resources res)
    {
        Resources[res].addToNumberOfUnitsSold();
    }

    public void addGoodSold(MyEnum.Goods good)
    {
        Goods[good].addToNumberOfUnitsSold();
    }

    public int getResourceSold(MyEnum.Resources res)
    {
        return Resources[res].getNumberOfUnitsSold();
    }

    public int getGoodsSold(MyEnum.Goods good)
    {
        return Goods[good].getNumberOfUnitsSold();
    }

    public int getNumberOfResourcesOffered(MyEnum.Resources res)
    {
        return Resources[res].getNumberOfUnitsOffered();
    }

    public int getNumberResourcesSold(MyEnum.Resources res)
    {
        return Resources[res].getNumberOfUnitsSold();
    }

    public int getNumberGoodsOffered(MyEnum.Goods good)
    {
        return Goods[good].getNumberOfUnitsOffered();
    }

    public int getNumberOfGoodsSoldLastTurn(MyEnum.Goods good)
    {
        return Goods[good].getNumberOfUnitsSold();
    }

    public List<float> getResourcePriceHistory(MyEnum.Resources res)
    {
        return Resources[res].getHistory();
    }

    public List<float> getGoodPriceHistory(MyEnum.Goods good)
    {
        return Goods[good].getHistory();
    }


    public void addResourceOffer(MyEnum.Resources res, Signal offer)
    {
        Resources[res].addOffer(offer);
    }

    public void addResourceBid(MyEnum.Resources res, Signal bid)
    {
        Resources[res].addBid(bid);
    }

    public void addGoodOffer(MyEnum.Goods good, Signal offer)
    {
        Goods[good].addOffer(offer);
    }

    public void addGoodBid(MyEnum.Goods good, Signal bid)
    {
        Goods[good].addBid(bid);
    }


    public void removeResourceOffer(MyEnum.Resources res, Signal offer)
    {
        Resources[res].removeOffer(offer);
    }

    public void removeResourceBid(MyEnum.Resources res, Signal bid)
    {
        Resources[res].removeBid(bid);
    }

    public void removeGoodOffer(MyEnum.Goods good, Signal offer)
    {
        Goods[good].removeOffer(offer);
    }

    public void removeGoodBid(MyEnum.Goods good, Signal bid)
    {
        Goods[good].removeBid(bid);
    }

    public List<Signal> getResourceOffers(MyEnum.Resources res)
    {
        return Resources[res].getOffers();
    }

    public List<Signal> getResourceBids(MyEnum.Resources res)
    {
        return Resources[res].getBids();
    }

    public List<Signal> getGoodOffers(MyEnum.Goods good)
    {
        return Goods[good].getOffers();
    }

    public List<Signal> getGoodBids(MyEnum.Goods good)
    {
        return Goods[good].getBids();
    }


    public void randomizeSignalOrder()
    {
        foreach (MyEnum.Resources res in Enum.GetValues(typeof(MyEnum.Resources)))
        {
            Resources[res].randomizeSignalOrder();
       }

        foreach (MyEnum.Goods good in Enum.GetValues(typeof(MyEnum.Goods)))
        {
            Goods[good].randomizeSignalOrder();
        }
    }


}
