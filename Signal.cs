using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Signal{

    private MyEnum.marketChoice type;
    private int ownerIndex;
    private MyEnum.Resources resource;
    private MyEnum.Goods good;
    private float quality;
    private MyEnum.OffBidLevels offerLevel;

    private float priceSold = 0;


    public Signal(MyEnum.marketChoice type, int playerIndex, MyEnum.Resources resource, MyEnum.Goods good, 
        float quality, MyEnum.OffBidLevels offerLevel)
    {
        this.type = type;
        this.ownerIndex = playerIndex;
        this.resource = resource;
        this.good = good;
        this.quality = quality;
        this.offerLevel = offerLevel;
    }

    public  MyEnum.marketChoice getType()
    {
        return type;
    }

    public int getOwnerIndex()
    {
        return ownerIndex;
    }


    public MyEnum.Resources getResourceType()
    {
        return resource;
    }

    public MyEnum.Goods getGoodType()
    {
        return good;
    }

    public float getQuality()
    {
        return this.quality;
    }

    public MyEnum.OffBidLevels getOffBidLevel()
    {
        return this.offerLevel;
    }

    public void setPriceSold(float amount)
    {
        this.priceSold = amount;
    }

    public float getPriceSold()
    {
        return this.priceSold;
    }

}
