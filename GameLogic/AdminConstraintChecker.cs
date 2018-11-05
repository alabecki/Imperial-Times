using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using assemblyCsharp;

public static class AdminConstraintChecker 
{

    public static bool checkIfCanAffordGood(Nation player, MyEnum.Goods good)
    {
        if (player.getGold() - player.getTotalCurrentBiddingCost() < State.market.getPriceOfGood(good))
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public static bool checkIfCanAffordResource(Nation player, MyEnum.Resources res)
    {
        if (player.getGold() - player.getTotalCurrentBiddingCost() < State.market.getPriceOfResource(res))
        {
            return false;
        }
        else
        {
            return true;
        }

    }


    public static bool checkIfAbleToAddColonists(Nation player)
    {
        bool able = true;
        if (player.getNumberGood(MyEnum.Goods.clothing) < 1 || player.getAP() < 1 ||
             player.getNumberGood(MyEnum.Goods.furniture) < 1)
        {
            able = false;
        }
        MyEnum.Era era = State.era;
        if (era != MyEnum.Era.Early && player.getNumberGood(MyEnum.Goods.paper) < 1)
        {
            able = false;
        }
        if (era == MyEnum.Era.Late && player.getNumberResource(MyEnum.Resources.spice) < 1)
        {
            able = false;
        }
        return able;
    }


    



}
