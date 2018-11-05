using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MarketHelper {

    public static string currentItem;

    public static Dictionary<MyEnum.Resources, MyEnum.marketChoice> ResourceOfferBid =
      new Dictionary<MyEnum.Resources, MyEnum.marketChoice>();

    public static Dictionary<MyEnum.Goods, MyEnum.marketChoice> GoodsOfferBid =
     new Dictionary<MyEnum.Goods, MyEnum.marketChoice>();

    public static Dictionary<MyEnum.Resources, int> ResourceOfferBidAmount =
        new Dictionary<MyEnum.Resources, int>();

    public static Dictionary<MyEnum.Goods, int> GoodsOfferBidAmount =
     new Dictionary<MyEnum.Goods, int>();

    public static Dictionary<MyEnum.Resources, MyEnum.OffBidLevels> ResourceOfferBidLevel =
       new Dictionary<MyEnum.Resources, MyEnum.OffBidLevels>();

    public static Dictionary<MyEnum.Goods, MyEnum.OffBidLevels> GoodsOfferBidLevel =
     new Dictionary<MyEnum.Goods, MyEnum.OffBidLevels>();


    
    
}
