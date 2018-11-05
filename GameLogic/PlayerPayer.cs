using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using assemblyCsharp;

public static class PlayerPayer
{

    public static void PayInfantry(Nation player)
    {
        //  player.removeUrbanPOP();
        player.AddMilitaryPOP();
        player.removeUrbanPOP(0.2f);
        player.UseAP(1);
        player.consumeGoods(MyEnum.Goods.arms, 1.0f);
        player.addArmyProducing(MyEnum.ArmyUnits.infantry);

    }

    public static void PayCavalry(Nation player)
    {
        player.AddMilitaryPOP();
        player.removeUrbanPOP(0.2f); player.AddMilitaryPOP();
        player.UseAP(1);
        player.consumeGoods(MyEnum.Goods.arms, 1.0f);
        player.consumeResource(MyEnum.Resources.meat, 1.0f);
        player.addArmyProducing(MyEnum.ArmyUnits.cavalry);
    }

    public static void PayArtillery(Nation player)
    {
        player.AddMilitaryPOP();
        player.removeUrbanPOP(0.2f);
        player.AddMilitaryPOP();
        player.UseAP(1);

        player.consumeGoods(MyEnum.Goods.arms, 2);
        player.addArmyProducing(MyEnum.ArmyUnits.artillery);

    }

    public static void PayFighter(Nation player)
    {
        player.AddMilitaryPOP();
        player.removeUrbanPOP(0.2f);
        player.UseAP(1);
        player.consumeGoods(MyEnum.Goods.fighter, 1);
        player.addArmyProducing(MyEnum.ArmyUnits.fighter);

    }

    public static void PayTank(Nation player)
    {
        player.AddMilitaryPOP();
        player.removeUrbanPOP(0.2f);
        player.UseAP(1);
        player.consumeGoods(MyEnum.Goods.tank, 1);
        player.addArmyProducing(MyEnum.ArmyUnits.tank);

    }

    public static void PayFrigate(Nation player)
    {
        player.AddMilitaryPOP();
        player.removeUrbanPOP(0.1f);
        player.UseAP(1);
        player.consumeGoods(MyEnum.Goods.arms, 1);
        player.consumeGoods(MyEnum.Goods.lumber, 1);
        player.consumeGoods(MyEnum.Goods.fabric, 1);
        player.UseAP(1);
        player.addNavyProducing(MyEnum.NavyUnits.frigates);
    }

    public static void PayIronClad(Nation player)
    {
        player.AddMilitaryPOP();
        player.removeUrbanPOP(0.1f);
        player.UseAP(1);
        player.consumeGoods(MyEnum.Goods.arms, 1);
        player.consumeGoods(MyEnum.Goods.steel, 1);
        player.consumeGoods(MyEnum.Goods.parts, 1);
        player.UseAP(1);
        player.addNavyProducing(MyEnum.NavyUnits.ironclad);

    }

    public static void PayDreadnought(Nation player)
    {
        player.AddMilitaryPOP();
        player.removeUrbanPOP(0.2f);
        player.UseAP(1);
        player.consumeGoods(MyEnum.Goods.arms, 3);
        player.consumeGoods(MyEnum.Goods.steel, 3);
        player.consumeGoods(MyEnum.Goods.parts, 1);
        player.consumeGoods(MyEnum.Goods.gear, 1);
        player.UseAP(2);
        player.addNavyProducing(MyEnum.NavyUnits.dreadnought);


    }

    public static void PayForResearch(Nation player)
    {
        MyEnum.Era era = State.era;
        player.consumeGoods(MyEnum.Goods.paper, 1);
        player.consumeGoods(MyEnum.Goods.parts, 1);
        player.UseAP(1);
        if (era != MyEnum.Era.Early)
        {
            player.consumeGoods(MyEnum.Goods.chemicals, 1);
        }
        if (era == MyEnum.Era.Late)
        {
            player.consumeGoods(MyEnum.Goods.telephone, 1);

        }

    }

    public static void PayForTechnology(Nation player, string techName)
    {
        Technology tech = State.GetTechnologies()[techName];
        player.Research = player.Research - tech.GetCost();

    }

    public static void payForMorePOP(Nation player)
    {
        player.consumeResource(MyEnum.Resources.wheat, 1);
        player.consumeGoods(MyEnum.Goods.clothing, 1);
        player.AddUrbanPOP();
        player.UseAP(1);
        if(player.getPOPIncreasedThisTurn() > 1)
        {
            player.consumeGoods(MyEnum.Goods.chemicals, 1);
        }
        player.AddUrbanPOP();

    }

    public static void payToReduceCorruption(Nation player)
    {
        player.consumeResource(MyEnum.Resources.spice, 1);
        player.consumeGoods(MyEnum.Goods.paper, 1);
        player.ChangeCorruption(-1);
        player.UseAP(1);


    }

    public static void payForColonialists(Nation player)
    {
        MyEnum.Era era = State.GerEra();
        player.consumeGoods(MyEnum.Goods.clothing, 1);
        player.consumeGoods(MyEnum.Goods.furniture, 1);
        player.AddColonialPoints(2);
        player.increaseColonialLevel();
        player.UseAP(1);
        if (era != MyEnum.Era.Early)
        {
            player.consumeGoods(MyEnum.Goods.paper, 1);
            player.AddColonialPoints(2);

        }
        if (era == MyEnum.Era.Late)
        {
            player.consumeResource(MyEnum.Resources.spice, 1);
            player.AddColonialPoints(2);
        }

    }

    public static void payForIP(Nation player)
    {
        player.UseAP(1);
        player.consumeResource(MyEnum.Resources.spice, 1);
        player.consumeGoods(MyEnum.Goods.furniture, 1);
        MyEnum.Era era = State.GerEra();
        if (era == MyEnum.Era.Middle)
        {
            player.consumeGoods(MyEnum.Goods.paper, 1);

        }
        if (era == MyEnum.Era.Late)
        {
            player.consumeGoods(MyEnum.Goods.telephone, 1);
            player.consumeGoods(MyEnum.Goods.auto, 1);
            player.increaseOilNeeded();
        }
    }

    public static void payTacticCards(Nation player)
    {
        player.UseAP(1);
        player.consumeResource(MyEnum.Resources.spice, 1);
        player.consumeGoods(MyEnum.Goods.arms, 1);
        MyEnum.Era era = State.GerEra();
        if (era != MyEnum.Era.Early)
        {
            player.consumeGoods(MyEnum.Goods.furniture, 1);
        }
        if (era == MyEnum.Era.Late)
        {
            player.consumeGoods(MyEnum.Goods.auto, 1);
            player.increaseOilNeeded();
        }

    }

    public static void payWarehouseExpansion(Nation player)
    {
        player.UseAP(1);
        player.consumeGoods(MyEnum.Goods.lumber, 1);
        player.IncreaseWarehouseCapacity(8);
    }


    public static void payForCultureCard(Nation player)
    {
        player.UseAP(1);
        MyEnum.Era era = State.GerEra();
        player.consumeGoods(MyEnum.Goods.paper, 1);
        if (era != MyEnum.Era.Early)
        {
            player.consumeGoods(MyEnum.Goods.clothing, 1);
        }


        if (era != MyEnum.Era.Late)
        {
            player.consumeResource(MyEnum.Resources.spice, 1);

        }
        if (era == MyEnum.Era.Late)
        {
            player.consumeGoods(MyEnum.Goods.furniture, 1);
            player.consumeGoods(MyEnum.Goods.telephone, 1);

        }
    }

    public static void payForFactory(Nation player, MyEnum.Goods factoryType)
    {
        int factoryLevel = player.industry.getFactoryLevel(factoryType);
        if (factoryLevel == 0)
        {
            player.consumeGoods(MyEnum.Goods.parts, 1);
            player.consumeGoods(MyEnum.Goods.lumber, 1);
            player.UseAP(1);
            player.useIP(1);

        }
        if (factoryLevel == 1)
        {
            player.consumeGoods(MyEnum.Goods.parts, 2);
            player.consumeGoods(MyEnum.Goods.lumber, 2);
            player.UseAP(1);
            player.useIP(2);

        }
        if (factoryLevel == 2)
        {
            player.consumeGoods(MyEnum.Goods.parts, 2);
            player.consumeGoods(MyEnum.Goods.lumber, 2);
            player.consumeGoods(MyEnum.Goods.gear, 2);
            player.UseAP(2);
            player.useIP(3);

        }
    }

    public static void payForFactoryProduction(Nation player, MyEnum.Goods good, int amount)
    {
        if (amount == 1)
        {
            player.UseAP(1);
        }
        if (amount > 1 && amount <= 4)
        {
            player.UseAP(2);
        }
        if (amount > 4 && amount <= 8)
        {
            player.UseAP(3);
        }
        if (amount > 8)
        {
            player.UseAP(4);
        }
        player.industry.setGoodProducing(good, amount);
        for (int k = 0; k < amount; k++)
        {
            player.industry.consumeGoodsMaterial(good, player);
        }

    }



    public static void payShipyYardUpgrade(Nation player)
    {
        player.UseAP(1);
        player.useIP(1);
        player.consumeGoods(MyEnum.Goods.lumber, 1);
        player.consumeGoods(MyEnum.Goods.steel, 1);
        if (player.GetShipyardLevel() == 1)
        {
            player.consumeGoods(MyEnum.Goods.parts, 1);
            {
                if (player.GetShipyardLevel() == 2)
                {
                    player.UseAP(1);
                    player.useIP(1);
                    player.consumeGoods(MyEnum.Goods.lumber, 1);
                    player.consumeGoods(MyEnum.Goods.steel, 1);
                }
                player.UpgradeShipyard();
            }
        }
    }

    public static void payRailRoad(Nation player, Province prov)
    {
        player.UseAP(1);
        player.useIP(1);
        player.consumeGoods(MyEnum.Goods.lumber, 1);
        player.consumeGoods(MyEnum.Goods.parts, 1);
        if(prov.getInfrastructure() == 1)
        {
            player.consumeGoods(MyEnum.Goods.steel, 1);
            player.UseAP(1);
        }
        prov.upgradeInfrastructure();
    }


    public static void payDevelopment(Nation player, Province prov)
    {
        player.UseAP(1);
        if (prov.getDevelopmentLevel() == 0)
        {
            if (prov.getResource() == MyEnum.Resources.wheat || prov.getResource() == MyEnum.Resources.fruit ||
                prov.getResource() == MyEnum.Resources.dyes || prov.getResource() == MyEnum.Resources.cotton ||
                prov.getResource() == MyEnum.Resources.wood)
            {
                player.consumeGoods(MyEnum.Goods.parts, 1);
            }
            if (prov.getResource() == MyEnum.Resources.meat || prov.getResource() == MyEnum.Resources.iron ||
                prov.getResource() == MyEnum.Resources.coal || prov.getResource() == MyEnum.Resources.gold)
            {
                player.consumeGoods(MyEnum.Goods.lumber, 1);
            }
            if (prov.getResource() == MyEnum.Resources.wheat)
            {
                player.consumeGoods(MyEnum.Goods.parts, 1);
            }
        }

        else if (prov.getDevelopmentLevel() == 1)
        {
            if (prov.getResource() == MyEnum.Resources.wheat || prov.getResource() == MyEnum.Resources.meat ||
               prov.getResource() == MyEnum.Resources.wood || prov.getResource() == MyEnum.Resources.oil)
            {
                player.consumeGoods(MyEnum.Goods.parts, 2);

            }
            if (prov.getResource() == MyEnum.Resources.fruit || prov.getResource() == MyEnum.Resources.dyes)
            {
                player.consumeGoods(MyEnum.Goods.chemicals, 2);
            }
            if (prov.getResource() == MyEnum.Resources.iron || prov.getResource() == MyEnum.Resources.coal ||
            prov.getResource() == MyEnum.Resources.gold)
            {
                player.consumeGoods(MyEnum.Goods.parts, 1);
                player.consumeGoods(MyEnum.Goods.chemicals, 1);
            }
            if (prov.getResource() == MyEnum.Resources.cotton)
            {
                player.consumeGoods(MyEnum.Goods.parts, 1);
                player.consumeGoods(MyEnum.Goods.lumber, 1);
            }
        }
        prov.changeDevelopmentLevel(1);
    }

    public static void payFortUpgrade(Nation player, Province prov)
    {
        player.UseAP(1);
        player.consumeGoods(MyEnum.Goods.arms, 1);
        if(prov.getFortLevel() == 1)
        {
            player.UseAP(1);
        }
        if(prov.getFortLevel() == 2)
        {
            player.consumeGoods(MyEnum.Goods.arms, 1);
        }
        prov.fortification += 1;
    }

}












