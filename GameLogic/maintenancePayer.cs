using assemblyCsharp;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class maintenancePayer {


    public static void payMaintenance(Nation player)
    {
        if(player.IsColonyOf() > -1)
        {
            return;
        }

        int population = (int)player.getTotalPOP();
        float wheatNeeded = population * 0.1f;
        float meatNeeded = population * 0.05f;
        float fruitNeeded = population * 0.05f;
        player.consumeResource(MyEnum.Resources.wheat, wheatNeeded);
        player.consumeResource(MyEnum.Resources.meat, meatNeeded);
        player.consumeResource(MyEnum.Resources.fruit, fruitNeeded);

        float coalNeeded = PlayerCalculator.coalNeededForRailRoads(player);
        player.consumeResource(MyEnum.Resources.coal, coalNeeded);
        float oilNeeded = player.getOilNeeded();
        player.consumeResource(MyEnum.Resources.oil, oilNeeded);

        if (player.getNumberResource(MyEnum.Resources.wheat) < 0)
        {
            manageFoodShortage(MyEnum.Resources.wheat, MyEnum.Resources.meat, MyEnum.Resources.fruit, player);
        }
        if (player.getNumberResource(MyEnum.Resources.meat) < 0)
        {
            manageFoodShortage(MyEnum.Resources.meat, MyEnum.Resources.wheat, MyEnum.Resources.fruit, player);
        }
        if (player.getNumberResource(MyEnum.Resources.fruit) < 0)
        {
            manageFoodShortage(MyEnum.Resources.fruit, MyEnum.Resources.meat, MyEnum.Resources.wheat, player);
        }

        if(player.getNumberResource(MyEnum.Resources.coal) < 0)
        {
            manageCoalShortage(player);
        }

        if(player.getNumberResource(MyEnum.Resources.oil) < 0)
        {
            manageOilShortage(player);
        }

        player.ChangeCorruption(0.1f);
    }




    private static void manageFoodShortage(MyEnum.Resources shortage, MyEnum.Resources firstOther, MyEnum.Resources secondOther, Nation player)
    {
        float deficit = Math.Abs(player.getNumberResource(shortage));
        player.collectResource(shortage, deficit);
        float one = player.getNumberResource(firstOther);
        float two = player.getNumberResource(secondOther);
        if (one + two > deficit)
        {
            if ((one >= System.Math.Abs(deficit / 2)) && (two >= System.Math.Abs(deficit / 2)))
            {
                player.consumeResource(firstOther, deficit / 2);
                player.consumeResource(secondOther, deficit / 2);
                player.increaseFoodImbalance(deficit);

            }
            else
            {
                player.increaseFoodShortage(deficit);
                player.decreasePrestige(1);

            }
        }
    }


    private static void manageCoalShortage(Nation player)
    {
        float deficit = Math.Abs(player.getNumberResource(MyEnum.Resources.coal));
        player.decreasePrestige(1);
        player.setNumberResource(MyEnum.Resources.coal, 0);
        float totalCoalNeed = PlayerCalculator.coalNeededForRailRoads(player);
        int numRailRoads =  (int)totalCoalNeed * 5;
        int numRailsNotWorking = (int)deficit*5;
        int count = Math.Min(numRailsNotWorking, numRailRoads);
        List<int> provs = player.getProvinces();
        System.Random rnd = new System.Random();
        for (int i = 0; i < count; i++)
        {
            bool flag = false;
            while(flag == false)
            {
                int r = rnd.Next(provs.Count);
                Province prov = State.getProvinces()[r];
                if(prov.getDevelopmentLevel() >= 1)
                {
                    flag = true;
                    prov.addRailNotWorking();
                }
            }
            
            
        }

    }

    private static void  manageOilShortage(Nation player)
    {
        player.decreasePrestige(1);
        float deficit = Math.Abs(player.getNumberResource(MyEnum.Resources.oil));
        player.setNumberResource(MyEnum.Resources.oil, 0);
        player.Stability -= deficit / 2;
    }



    


}
