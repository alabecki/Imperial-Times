using assemblyCsharp;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WorldMapStrategyKit;

public static class PlayerCollector {

    public static PlaceArmy armyPlacer;

    public static void collectForPlayer(Nation player)
    {
        if (player.IsColonyOf() > -1)
        {
            return;
        }
        player.setMilitaryScore();
        player.setIndustrialScore();
        player.setTotalScore();

        collectResources(player);

        collectInfulencePoints(player);
        collectInfulencePoints(player);
        resetAP(player);

        foreach (MyEnum.Goods good in Enum.GetValues(typeof(MyEnum.Goods)))
        {
            float amount = player.industry.getGoodProducing(good);
            player.collectGoods(good, amount);
            player.industry.setGoodProducing(good, 0);
        }

        placeNewLandUnits(player);
    }
    

    private static void placeNewLandUnits(Nation player)
    {
        foreach (MyEnum.ArmyUnits unitType in Enum.GetValues(typeof(MyEnum.ArmyUnits)))
        {
            string capitalString = player.capital;
            assemblyCsharp.Province capital = State.GetProvinceByName(capitalString);
            for (int i = 0; i < player.getArmyProducing(unitType); i++)
            {

                Army newArmy = new Army(player.getIndex());
                newArmy.addUnit(unitType);
                newArmy.setLocation(capital.getIndex());
                capital.addFriendlyArmy(newArmy.GetIndex());
                player.addArmy(newArmy);
                armyPlacer.placeArmyOnMap(capital.getIndex(), player, newArmy.GetIndex());
            }
        }
    }

    private static void placeNewSeaUnits(Nation player)
    {
        WMSK map = WMSK.instance;
        foreach (MyEnum.NavyUnits unitType in Enum.GetValues(typeof(MyEnum.ArmyUnits)))
        {
            string capitalString = player.capital;
            assemblyCsharp.Province capital = State.GetProvinceByName(capitalString);
            for (int i = 0; i < player.getNavyProducing(unitType); i++)
            {

                Fleet newFleet = new Fleet(player.getIndex());
                newFleet.addUnit(unitType, 1);
               // map.GetProvinceCoastalPoints
                newFleet.setLocation(capital.getIndex());
                player.addFleet(newFleet);

                //maybe add mount object for port and have new fleets appear there by default

            }
        }
    }




    private static void collectResources(Nation player)
    {
        for (int i = 0; i < player.getProvinces().Count; i++)
        {
            int index = player.getProvinces()[i];
            assemblyCsharp.Province prov = State.getProvinces()[index];
            float resYield = prov.getProduction();
            MyEnum.Resources res = prov.getResource();
            Debug.Log("prov res: " + res);
            if (res != MyEnum.Resources.gold)
            {
                player.collectResource(res, resYield);
            }
            else
            {
                player.receiveGold(resYield * 5);
            }
        }

        for (int i = 0; i < player.getColonies().Count; i++)
        {
            int index = player.getColonies()[i];
            assemblyCsharp.Province prov = State.getProvinces()[index];
            float resYield = prov.getProduction();
            MyEnum.Resources res = prov.getResource();
            player.collectResource(res, resYield);
        }
    }



    private static void collectInfulencePoints(Nation player)
    {
        int armyScore = State.history.CaululateArmyScore(player);
        int cultureLevel = player.getCulureLevel();
        int temp = armyScore + (cultureLevel * 3);
        int points = temp / 10;
        player.addDiplomacyPoints(points);

    }
   

    private static void collectColonialPoints(Nation player)
    {
        int navyScore = State.history.CalculateNavyScore(player);
        int colonoalLevel = player.getColonialLevel();
        int temp = navyScore + (colonoalLevel * 2);
        int points = temp / 7;
        player.AddColonialPoints(points);
   
    }

    private static void resetAP(Nation player)
    {
        float corruption = player.GetCorruption();
        float stability = player.Stability;
        float modifier = 1;
        if (player.GetTechnologies().Contains("electricity"))
        {
            modifier += 0.1f;
        }
        if (player.GetTechnologies().Contains("telegraph"))
        {
            modifier += 0.1f;
        }
        if (player.GetTechnologies().Contains("telephone"))
        {
            modifier += 0.1f;
        }
        modifier += (stability * 0.1f);
        modifier -= (corruption * 0.1f);
        int urbanPops = player.getUrbanPOP();
        int AP = (int)(urbanPops * modifier);
        player.setAP(AP);
    }
       



}
