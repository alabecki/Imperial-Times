using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using assemblyCsharp;

public static class ProductionCosts {



    public static Dictionary<string, float> GetCosts(MyEnum.Goods good)
    {
        Dictionary<string, float> cost = new Dictionary<string, float>();
        if (good.Equals(MyEnum.Goods.lumber))
        {
            cost["wood"] = 1.0f;
        }
        if (good.Equals(MyEnum.Goods.fabric))
        {
            cost["cotton"] = 1.0f;

        }
        if (good.Equals(MyEnum.Goods.steel))
        {
            cost["iron"] = 0.67f;
            cost["coal"] = 0.33f;
        }
       
        if (good.Equals(MyEnum.Goods.parts) || good.Equals(MyEnum.Goods.arms))
        {
            cost["steel"] = 1.0f;
        }
        if (good.Equals(MyEnum.Goods.paper))
        {
            cost["wood"] = 1.0f;
        }
        if (good.Equals(MyEnum.Goods.clothing))
        {
            cost["fabric"] = 0.8f;
            cost["dyes"] = 0.2f;
        }
        if (good.Equals(MyEnum.Goods.furniture))
        {
            cost["lumber"] = 0.75f;
            cost["fabric"] = 0.25f;
        }
        if (good.Equals(MyEnum.Goods.chemicals))
        {
            cost["coal"] = 1.0f;
        }
        if (good.Equals(MyEnum.Goods.gear))
        {
            cost["rubber"] = 0.75f;
            cost["steel"] = 0.25f;
        }
        if (good.Equals(MyEnum.Goods.telephone))
        {
            cost["gear"] = 0.8f;
            cost["lumber"] = 0.2f;
        }
        if (good.Equals(MyEnum.Goods.auto))
        {
            cost["rubber"] = 0.25f;
            cost["gear"] = 0.5f;
            cost["parts"] = 0.5f;
            cost["steel"] = 0.25f;
        }
        if (good.Equals(MyEnum.Goods.fighter))
        {
            cost["lumber"] = 0.5f;
            cost["gear"] = 0.5f;
            cost["parts"] = 0.5f;
            cost["arms"] = 1.0f;
        }
        if (good.Equals(MyEnum.Goods.tank))
        {
            cost["steel"] = 1.0f;
            cost["arms"] = 1.5f;
            cost["gear"] = 0.5f;
            cost["parts"] = 0.5f;
        }

        return cost;
    }

}
