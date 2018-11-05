using assemblyCsharp;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WorldMapStrategyKit;

public class InitializeNation : MonoBehaviour
{
    PlaceArmy armyPlacer = new PlaceArmy();
   

    public void InitializeThisNation(Nation player)
    {
        player.setIndustrialScore();

        Dictionary<int, Relation> relations = player.getRelations();
        foreach(Nation nation in State.getNations().Values)
        {
        //    Debug.Log("Adding " + nation.getName() + "index " + nation.getIndex() + " to relations of " + player.getName());
            if(nation.getType() == MyEnum.NationType.oldMinor || nation.getIndex() == player.getIndex())
            {
                continue;
            }
            Relation newRelation = new Relation(nation.getIndex(), 50, false, false, false, false, false);
            relations[nation.getIndex()] = newRelation;
        }

        foreach(Nation nation in State.getNations().Values)
        {
            if(nation.getType() == MyEnum.NationType.major)
            {
                continue;
            }
            player.addRecognizedClaim(nation.getIndex(), -1);
        }

        player.Reputation = 65;
        player.setReliability(65);
        player.ChangeCorruption(1);

        distributeStartingResourcesAndGoods(player);
        initalizeArmies(player);
        initalizePOP(player);


        // pickNewFleetPosition(player);

    }


    private void pickNewFleetPosition(Nation player)
    {
        WMSK map = WMSK.instance;
        Debug.Log(player.getNationName());
        string capitalString = player.capital;
        assemblyCsharp.Province capital = State.GetProvinceByName(capitalString);
        int capitalIndex = capital.getIndex();
        List<Vector2> capitalCoast = map.GetProvinceCoastalPoints(capitalIndex);
        int coastSize = capitalCoast.Count;
        int middle = (int)coastSize / 2;
     //   Debug.Log("Middle: " + middle);
        Vector2 navalChoice = capitalCoast[middle];
        player.setNewNavyPosition(navalChoice);
    }

    private void initalizeArmies(Nation player)
    {
        player.GetMilitaryForm().setMaxMorale(1);
        Army firstArmy = new Army(player.getIndex());

       // Debug.Log("Nation Type: " + player.getType() + "________________________");
        if (player.getType() == MyEnum.NationType.major)
        {
        //    Debug.Log("Any Major Nations Here?");
            firstArmy.addUnit(MyEnum.ArmyUnits.infantry);
            firstArmy.addUnit(MyEnum.ArmyUnits.infantry);
            firstArmy.addUnit(MyEnum.ArmyUnits.cavalry);

            Fleet firstFleet = new Fleet(player.getIndex());
            firstFleet.addUnit(MyEnum.NavyUnits.frigates, 1);
            player.addFleet(firstFleet);
            //Still need to place fleet
        }
        else
        {
            firstArmy.addUnit(MyEnum.ArmyUnits.infantry);
        }
        player.addArmy(firstArmy);
        PlaceArmy armyPlacer = new PlaceArmy();
        string capName = player.capital;
        Debug.Log(player.getNationName());
        Debug.Log(capName);
        assemblyCsharp.Province prov = State.GetProvinceByName(capName);
        int capIndex = prov.getIndex();
        armyPlacer.placeArmyOnMap(capIndex, player, firstArmy.GetIndex());
        player.setMilitaryScore();
        Debug.Log("Military Score: " + player.getMilitaryScore().ToString());
    }


    private void distributeStartingResourcesAndGoods(Nation player)
    {
        for (int i = 0; i < player.getProvinces().Count; i++)
        {
            int index = player.getProvinceIndex(i);
            assemblyCsharp.Province prov = State.getProvinces()[index];
            MyEnum.Resources res = prov.getResource();
            float amount = prov.getProduction();
            if (res == MyEnum.Resources.gold)
            {
                player.receiveGold(amount * 5);
            }
            else
            {
                player.collectResource(res, amount);
            }
            player.addRuralPOP();
            player.addRuralPOP();
        }

        player.receiveGold(5);

        if (player.getType() == MyEnum.NationType.major)
        {
            player.collectGoods(MyEnum.Goods.steel, 1);
            player.collectGoods(MyEnum.Goods.fabric, 1);
            player.collectGoods(MyEnum.Goods.lumber, 1);
            player.collectGoods(MyEnum.Goods.arms, 2);

            player.collectResource(MyEnum.Resources.wheat, 1);
            player.collectResource(MyEnum.Resources.meat, 0.5f);
            player.collectResource(MyEnum.Resources.fruit, 0.5f);
            player.receiveGold(5);

        }

    }

    public void initalizePOP(Nation player) { 

        if(player.getType() == MyEnum.NationType.minor)
        {
            player.setUrbanPOP(1);
            player.setTotalPOP(5);
            player.setAP(1);

        }
        if (player.getType() == MyEnum.NationType.major)
        {
            player.setUrbanPOP(3);
            player.setMilitaryPOP(1);
            player.setTotalPOP(16);
            player.setAP(3);
        }
    }

    //just for AI
    public void initalizeFactoryPriorities(Nation player)
    {
        TopLevel aiTopLevel = player.getAI().GetTopLevel();
        foreach (MyEnum.Goods good in Enum.GetValues(typeof(MyEnum.Goods)))
        {

            aiTopLevel.setFactPriority(player, good, 1);
        }

        for (int i = 0; i < player.getProvinces().Count; i++)
        {
            int pIndex = player.getProvinces()[i];
            assemblyCsharp.Province prov = State.getProvinces()[pIndex];

            if (prov.getResource() == MyEnum.Resources.coal)
            {
                aiTopLevel.alterFactPriority(player, MyEnum.Goods.steel, 0.25f);
                aiTopLevel.alterFactPriority(player, MyEnum.Goods.parts, 0.15f);
                aiTopLevel.alterFactPriority(player, MyEnum.Goods.arms, 0.15f);
                aiTopLevel.alterFactPriority(player, MyEnum.Goods.chemicals, 0.2f);


            }
            if (prov.getResource() == MyEnum.Resources.cotton)
            {
                aiTopLevel.alterFactPriority(player, MyEnum.Goods.fabric, 0.25f);
                aiTopLevel.alterFactPriority(player, MyEnum.Goods.clothing, 0.2f);
                aiTopLevel.alterFactPriority(player, MyEnum.Goods.furniture, 0.1f);
            }

            if (prov.getResource() == MyEnum.Resources.dyes)
            {
                aiTopLevel.alterFactPriority(player, MyEnum.Goods.fabric, 0.25f);
                aiTopLevel.alterFactPriority(player, MyEnum.Goods.clothing, 0.2f);
            }

            if (prov.getResource() == MyEnum.Resources.iron)
            {
                aiTopLevel.alterFactPriority(player, MyEnum.Goods.steel, 0.25f);
                aiTopLevel.alterFactPriority(player, MyEnum.Goods.parts, 0.2f);
                aiTopLevel.alterFactPriority(player, MyEnum.Goods.arms, 0.2f);
                aiTopLevel.alterFactPriority(player, MyEnum.Goods.gear, 0.075f);
                aiTopLevel.alterFactPriority(player, MyEnum.Goods.auto, 0.075f);
                aiTopLevel.alterFactPriority(player, MyEnum.Goods.tank, 0.12f);

            }

            if (prov.getResource() == MyEnum.Resources.oil)
            {
                aiTopLevel.alterFactPriority(player, MyEnum.Goods.auto, 0.15f);
                aiTopLevel.alterFactPriority(player, MyEnum.Goods.fighter, 0.1f);
                aiTopLevel.alterFactPriority(player, MyEnum.Goods.tank, 0.1f);

            }
            if (prov.getResource() == MyEnum.Resources.rubber)
            {
                aiTopLevel.alterFactPriority(player, MyEnum.Goods.gear, 0.25f);
                aiTopLevel.alterFactPriority(player, MyEnum.Goods.telephone, 0.2f);
                aiTopLevel.alterFactPriority(player, MyEnum.Goods.auto, 0.12f);
                aiTopLevel.alterFactPriority(player, MyEnum.Goods.fighter, 0.075f);
                aiTopLevel.alterFactPriority(player, MyEnum.Goods.tank, 0.075f);

            }
         
            if (prov.getResource() == MyEnum.Resources.wood)
            {
                aiTopLevel.alterFactPriority(player, MyEnum.Goods.lumber, 0.25f);
                aiTopLevel.alterFactPriority(player, MyEnum.Goods.paper, 0.25f);
                aiTopLevel.alterFactPriority(player, MyEnum.Goods.furniture, 0.15f);
                aiTopLevel.alterFactPriority(player, MyEnum.Goods.telephone, 0.075f);
                aiTopLevel.alterFactPriority(player, MyEnum.Goods.fighter, 0.075f);

            }
        }
    }

    public void initalizeResourcePriorities(Nation player)
    {
        TopLevel aiTopLevel = player.getAI().GetTopLevel();
        aiTopLevel.setResPriority(player, MyEnum.Resources.wheat, 3);
        aiTopLevel.setResPriority(player, MyEnum.Resources.meat, 2.5f);
        aiTopLevel.setResPriority(player, MyEnum.Resources.fruit, 2.5f);
        aiTopLevel.setResPriority(player, MyEnum.Resources.coal, 1);
        aiTopLevel.setResPriority(player, MyEnum.Resources.cotton, 1.5f);
        aiTopLevel.setResPriority(player, MyEnum.Resources.dyes, 2.5f);
        aiTopLevel.setResPriority(player, MyEnum.Resources.gold, 3f);
        aiTopLevel.setResPriority(player, MyEnum.Resources.iron, 2.25f);
        aiTopLevel.setResPriority(player, MyEnum.Resources.oil, 0.25f);
        aiTopLevel.setResPriority(player, MyEnum.Resources.rubber, 0.25f);
        aiTopLevel.setResPriority(player, MyEnum.Resources.spice, 2.75f);
        aiTopLevel.setResPriority(player, MyEnum.Resources.wood, 1.5f);

        for (int i = 0; i < player.getProvinces().Count; i++)
        {
            int pIndex = player.getProvinces()[i];
            assemblyCsharp.Province prov = State.getProvinces()[pIndex];
            if(prov.getResource() == MyEnum.Resources.wheat)
            {
                aiTopLevel.alterResPriority(player, MyEnum.Resources.wheat, -0.2f);
            }
            if (prov.getResource() == MyEnum.Resources.meat)
            {
                aiTopLevel.alterResPriority(player, MyEnum.Resources.meat, -0.15f);
            }
            if (prov.getResource() == MyEnum.Resources.fruit)
            {
                aiTopLevel.alterResPriority(player, MyEnum.Resources.fruit, -0.15f);
            }
            if (prov.getResource() == MyEnum.Resources.coal)
            {
                aiTopLevel.alterResPriority(player, MyEnum.Resources.iron, 0.15f);
                aiTopLevel.alterResPriority(player, MyEnum.Resources.coal, -0.05f);
            }
            if (prov.getResource() == MyEnum.Resources.cotton)
            {
                aiTopLevel.alterResPriority(player, MyEnum.Resources.dyes, 0.16f);
                aiTopLevel.alterResPriority(player, MyEnum.Resources.cotton, -0.075f);
                aiTopLevel.alterResPriority(player, MyEnum.Resources.wood, 0.075f);
            }

            if (prov.getResource() == MyEnum.Resources.dyes)
            {
                aiTopLevel.alterResPriority(player, MyEnum.Resources.cotton, 0.2f);

            }
            if (prov.getResource() == MyEnum.Resources.gold)
            {
                aiTopLevel.alterResPriority(player, MyEnum.Resources.gold, -0.15f);

            }
            if (prov.getResource() == MyEnum.Resources.iron)
            {
                aiTopLevel.alterResPriority(player, MyEnum.Resources.coal, 0.1f);
                aiTopLevel.alterResPriority(player, MyEnum.Resources.iron, -0.075f);
            }
            if (prov.getResource() == MyEnum.Resources.oil)
            {
                aiTopLevel.alterResPriority(player, MyEnum.Resources.oil, -0.075f);
            }
            if (prov.getResource() == MyEnum.Resources.rubber)
            {
                aiTopLevel.alterResPriority(player, MyEnum.Resources.rubber, -0.075f);
            }
            if (prov.getResource() == MyEnum.Resources.spice)
            {
                aiTopLevel.alterResPriority(player, MyEnum.Resources.gold, -0.075f);
            }
            if (prov.getResource() == MyEnum.Resources.wood)
            {
                aiTopLevel.alterResPriority(player, MyEnum.Resources.wood, -0.1f);
                aiTopLevel.alterResPriority(player, MyEnum.Resources.cotton, 0.075f);

            }

        }
    }

    

    public void initalizeAIPriorities(Nation player)
    {
        TopLevel aiTopLevel = player.getAI().GetTopLevel();
        foreach (MyEnum.macroPriorities macro in Enum.GetValues(typeof(MyEnum.macroPriorities)))
        {

            aiTopLevel.setMacroPriority(player, macro, 1);
        }
        for (int i = 0; i < player.getProvinces().Count; i++)
        {
            int pIndex = player.getProvinces()[i];
            assemblyCsharp.Province prov = State.getProvinces()[pIndex];
            if (prov.getResource() == MyEnum.Resources.coal)
            {
                aiTopLevel.alterMacroPriority(player, MyEnum.macroPriorities.buildFactory, 0.15f);
                aiTopLevel.alterMacroPriority(player, MyEnum.macroPriorities.research, 0.15f);

            }
            if (prov.getResource() == MyEnum.Resources.cotton)
            {
                aiTopLevel.alterMacroPriority(player, MyEnum.macroPriorities.colonies, 0.1f);
                aiTopLevel.alterMacroPriority(player, MyEnum.macroPriorities.developProvince, 0.1f);

            }
            if (prov.getResource() == MyEnum.Resources.dyes)
            {
                aiTopLevel.alterMacroPriority(player, MyEnum.macroPriorities.developProvince, 0.1f);
            }
            if (prov.getResource() == MyEnum.Resources.gold)
            {
                aiTopLevel.alterMacroPriority(player, MyEnum.macroPriorities.culture, 0.15f);
                aiTopLevel.alterMacroPriority(player, MyEnum.macroPriorities.spheres, 0.15f);

            }
            if (prov.getResource() == MyEnum.Resources.iron)
            {
                aiTopLevel.alterMacroPriority(player, MyEnum.macroPriorities.buildFactory, 0.15f);
                aiTopLevel.alterMacroPriority(player, MyEnum.macroPriorities.conquest, 0.15f);

            }
            if (prov.getResource() == MyEnum.Resources.oil)
            {
                aiTopLevel.alterMacroPriority(player, MyEnum.macroPriorities.developProvince, 0.1f);
                aiTopLevel.alterMacroPriority(player, MyEnum.macroPriorities.conquest, 0.05f);

            }
            if (prov.getResource() == MyEnum.Resources.rubber)
            {
                aiTopLevel.alterMacroPriority(player, MyEnum.macroPriorities.research, 0.1f);
            }
            if (prov.getResource() == MyEnum.Resources.spice)
            {
                aiTopLevel.alterMacroPriority(player, MyEnum.macroPriorities.culture, 0.1f);
            }
            if (prov.getResource() == MyEnum.Resources.wood)
            {
                aiTopLevel.alterMacroPriority(player, MyEnum.macroPriorities.colonies, 0.1f);
                aiTopLevel.alterMacroPriority(player, MyEnum.macroPriorities.navy, 0.1f);
                aiTopLevel.alterMacroPriority(player, MyEnum.macroPriorities.buildFactory, 0.05f);
            }
        }
        
    }

    public void initializeUnitPriority(Nation player)
    {
        TopLevel aiTopLevel = player.getAI().GetTopLevel();

        foreach (MyEnum.ArmyUnits unit in Enum.GetValues(typeof(MyEnum.ArmyUnits)))
        {
            if (MyEnum.ArmyUnits.infantry == unit)
            {
                aiTopLevel.setArmyUnitPriority(player, unit, 1.2f);

            }
            else
            {
                aiTopLevel.setArmyUnitPriority(player, unit, 1);
            }
        }

    }






}
