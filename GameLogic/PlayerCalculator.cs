using assemblyCsharp;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerCalculator
{


    public static float getResourceProducing(Nation player, MyEnum.Resources res)
    {

        float producing = 0;
        foreach (int provIndex in player.getProvinces())
        {
            Province prov = State.getProvinces()[provIndex];
            if (prov.getResource() == res)
            {
                producing += prov.getProduction();
            }
        }
        return producing;
    }

    public static float coalNeededForRailRoads(Nation player)
    {
        int numberRailRoadLevels = 0;
        for (int i = 0; i < player.getProvinces().Count; i++)
        {
            Province prov = State.getProvinces()[i];
            numberRailRoadLevels += prov.getInfrastructure();
        }
        return (numberRailRoadLevels / 5);
    }

    //Another version will be used for actual turns - as it will draw upon 
    public static float calculatePOPGrowthRate(Nation player)
    {
        float rate = 0.8f + (player.Stability / 3);
        if (player.GetTechnologies().Contains("Medicine"))
        {
            rate += 0.1f;
        }
        if (player.GetTechnologies().Contains("Electricity"))
        {
            rate += 0.1f;
        }
        if (player.getFoodShortage() > 0)
        {
            rate -= player.getFoodShortage();
        }
        return rate;
    }


    public static bool canUpgradeShipyard(Nation player)
    {
        bool answer = true;
        if (player.GetShipyardLevel() == 0 && (player.getAP() < 1 || player.getNumberGood(MyEnum.Goods.lumber) < 1 ||
            player.getNumberGood(MyEnum.Goods.steel) < 1))
        {
            answer = false;
        }
        else if (player.GetShipyardLevel() == 1)
        {
            if (!player.GetTechnologies().Contains("advanced_iron_working"))
            {
                answer = false;
            }
            else if (player.getAP() < 1 || player.getNumberGood(MyEnum.Goods.lumber) < 1 || player.getNumberGood(MyEnum.Goods.steel) < 1 ||
             player.getNumberGood(MyEnum.Goods.parts) < 1)
            {
                answer = false;
            }
        }
        else if (player.GetShipyardLevel() == 2)
        {
            if (!player.GetTechnologies().Contains("OilPoweredShips"))
            {
                answer = false;
            }
            else if (player.getAP() < 2 || player.getNumberGood(MyEnum.Goods.lumber) < 2 || player.getNumberGood(MyEnum.Goods.steel) < 2 ||
          player.getNumberGood(MyEnum.Goods.parts) < 1)
            {
                answer = false;
            }

        }
        return answer;

    }


    public static bool checkUpgradeDevelopment(Province prov, Nation player)
    {

        if (player.getAP() < 1 || prov.getDevelopmentLevel() == 2)
        {
            return false;
        }
        if (prov.getResource() == MyEnum.Resources.wheat)
        {
            if (player.getNumberGood(MyEnum.Goods.parts) < 1)
            {
                return false;
            }
            if (!player.GetTechnologies().Contains("steel_plows"))
            {
                return false;
            }
            if (prov.getDevelopmentLevel() == 1)
            {
                if (!player.GetTechnologies().Contains("mechanical_reaper"))
                {
                    return false;
                }
                else if (player.getNumberGood(MyEnum.Goods.parts) < 2)
                {
                    return false;
                }
            }
        }
        if (prov.getResource() == MyEnum.Resources.meat)
        {
            if (!player.GetTechnologies().Contains("biology"))
            {
                return false;
            }
            else if (prov.getDevelopmentLevel() == 0 && player.getNumberGood(MyEnum.Goods.lumber) < 1)
            {
                return false;
            }
            if (prov.getDevelopmentLevel() == 1)
            {
                if (!player.GetTechnologies().Contains("barbed_wire"))
                {
                    return false;
                }
                else if (player.getNumberGood(MyEnum.Goods.parts) < 2)
                {
                    return false;
                }
            }
        }
        if (prov.getResource() == MyEnum.Resources.fruit || prov.getResource() == MyEnum.Resources.dyes ||
            prov.getResource() == MyEnum.Resources.rubber)
        {
            if (!player.GetTechnologies().Contains("steel_plows"))
            {
                return false;
            }
            else if (prov.getDevelopmentLevel() == 0 && player.getNumberGood(MyEnum.Goods.parts) < 1)
            {
                return false;
            }
            if (prov.getDevelopmentLevel() == 1)
            {
                if (!player.GetTechnologies().Contains("fertlizer"))
                {
                    return false;
                }
                else if (player.getNumberGood(MyEnum.Goods.chemicals) < 2)
                {
                    return false;
                }
            }
        }
        if (prov.getResource() == MyEnum.Resources.iron || prov.getResource() == MyEnum.Resources.coal ||
            prov.getResource() == MyEnum.Resources.gold)
        {
            if (!player.GetTechnologies().Contains("square_timbering"))
            {
                return false;
            }
            else if (prov.getDevelopmentLevel() == 0 && player.getNumberGood(MyEnum.Goods.lumber) < 1)
            {
                return false;
            }
            if (prov.getDevelopmentLevel() == 1)
            {
                if (!player.GetTechnologies().Contains("dynamite"))
                {
                    return false;
                }
                else if (player.getNumberGood(MyEnum.Goods.parts) < 1 || player.getNumberGood(MyEnum.Goods.chemicals) < 1)
                {
                    return false;
                }
            }
        }
        if (prov.getResource() == MyEnum.Resources.cotton)
        {
            if (!player.GetTechnologies().Contains("cotton_gin"))
            {
                return false;
            }
            else if (prov.getDevelopmentLevel() == 0 && player.getNumberGood(MyEnum.Goods.parts) < 1)
            {
                return false;
            }
            if (prov.getDevelopmentLevel() == 1)
            {
                if (!player.GetTechnologies().Contains("cotton_jenny"))
                {
                    return false;
                }
                else if (player.getNumberGood(MyEnum.Goods.parts) < 1 || player.getNumberGood(MyEnum.Goods.lumber) < 1)
                {
                    return false;
                }
            }
        }

        if (prov.getResource() == MyEnum.Resources.wood)
        {
            if (!player.GetTechnologies().Contains("high_power_steam_engine"))
            {
                return false;
            }
            else if (prov.getDevelopmentLevel() == 0 && player.getNumberGood(MyEnum.Goods.parts) < 1)
            {
                return false;
            }
            if (prov.getDevelopmentLevel() == 1)
            {
                if (!player.GetTechnologies().Contains("compound_steam_engine"))
                {
                    return false;
                }
                else if (player.getNumberGood(MyEnum.Goods.parts) < 2)
                {
                    return false;
                }
            }
        }

        if (prov.getResource() == MyEnum.Resources.oil)
        {
            if (!player.GetTechnologies().Contains("oil_rilling"))
            {
                return false;
            }
            else if (prov.getDevelopmentLevel() == 0 && player.getNumberGood(MyEnum.Goods.parts) < 1)
            {
                return false;
            }
            if (prov.getDevelopmentLevel() == 1)
            {
                if (!player.GetTechnologies().Contains("rotary_drilling"))
                {
                    return false;
                }
                else if (player.getNumberGood(MyEnum.Goods.parts) < 2)
                {
                    return false;
                }
            }
        }


        return true;
    }

    public static bool canUpgradeRailRoad(Province prov, Nation player)
    {
        if (player.getAP() < 1 || prov.getInfrastructure() == 2 || player.getNumberGood(MyEnum.Goods.parts) < 1 ||
            player.getNumberGood(MyEnum.Goods.lumber) < 1 || !player.GetTechnologies().Contains("high_power_steam_engine"))
        {
            return false;
        }
        if (prov.getInfrastructure() == 1)
        {
            if (!player.GetTechnologies().Contains("compound_steam_engine"))
            {
                return false;
            }
            else if (player.getAP() < 2 || player.getNumberGood(MyEnum.Goods.steel) < 1)
            {
                return false;
            }
        }
        return true;
    }


    public static bool canUpgradeFort(Province prov, Nation player)
    {
        if (prov.getFortLevel() == 3 || player.getAP() == 0 || player.getNumberGood(MyEnum.Goods.arms) < 2)
        {
            return false;
        }
        if (prov.getFortLevel() >= 1 && (!player.GetTechnologies().Contains("cement") || player.getAP() < 2))
        {
            return false;
        }
        if (prov.getFortLevel() == 2 && (!player.GetTechnologies().Contains("dynamite") || player.getNumberGood(MyEnum.Goods.arms) < 3))
        {
            return false;
        }
        return true;
    }


    public static int getNumberProvDevelopments(Nation player)
    {
        int amount = 0;
        for (int i = 0; i < player.getProvinces().Count; i++)
        {
            int pIndex = player.getProvinces()[i];
            assemblyCsharp.Province prov = State.getProvinces()[pIndex];
            amount += prov.getDevelopmentLevel();
        }
        return amount;
    }


    public static int getNumberProvRailRoads(Nation player)
    {
        int amount = 0;
        for (int i = 0; i < player.getProvinces().Count; i++)
        {
            int pIndex = player.getProvinces()[i];
            assemblyCsharp.Province prov = State.getProvinces()[pIndex];
            amount += prov.getInfrastructure();
        }
        return amount;
    }


    public static float getCorruptionFactor(Nation player)
    {
        float corruption = player.GetCorruption();
        float corruptionFactor = 1 - (corruption * 0.1f);
        return corruptionFactor;
    }

    public static float getIndustrialExperienceFactor(Nation player)
    {
        int level = player.getInvestmentLevel();
        float indExpFac = 1.0f;
        indExpFac += level * 0.01f;
        return indExpFac;
    }


    public static bool canInvest(Nation player)
    {
        bool able = true;
        if (player.getNumberResource(MyEnum.Resources.spice) < 1 || player.getAP() < 1 ||
            player.getNumberGood(MyEnum.Goods.furniture) < 1)
        {
            able = false;
        }
        MyEnum.Era era = State.era;
        if (era == MyEnum.Era.Middle && player.getNumberGood(MyEnum.Goods.paper) < 1)
        {
            able = false;
        }
        if (era == MyEnum.Era.Late && (player.getNumberGood(MyEnum.Goods.telephone) < 1 ||
            player.getNumberGood(MyEnum.Goods.auto) < 1))
        {
            able = false;
        }
        return able;
    }

    public static int getTotalNumberInfantry(Nation player)
    {
        int count = 0;
        for (int i = 0; i < player.GetArmies().Count; i++)
        {
            count += player.GetArmy(i).GetInfantry();
        }
        return count;
    }

    public static int getTotalNumberCavalry(Nation player)
    {
        int count = 0;
        for (int i = 0; i < player.GetArmies().Count; i++)
        {
            count += player.GetArmy(i).GetCavalry();
        }
        return count;
    }

    public static int getTotalNumberArtillery(Nation player)
    {
        int count = 0;
        for (int i = 0; i < player.GetArmies().Count; i++)
        {
            count += player.GetArmy(i).GetArtillery();
        }
        return count;
    }

    public static int getTotalNumberTanks(Nation player)
    {
        int count = 0;
        for (int i = 0; i < player.GetArmies().Count; i++)
        {
            count += player.GetArmy(i).GetTank();
        }
        return count;
    }


    public static int getTotalNumberFighters(Nation player)
    {
        int count = 0;
        for (int i = 0; i < player.GetArmies().Count; i++)
        {
            count += player.GetArmy(i).GetFighter();
        }
        return count;
    }

    public static int getTotalNumberUnits(Nation player)
    {
        int count = 0;
        for (int i = 0; i < player.GetArmies().Count; i++)
        {
            count += player.GetArmy(i).GetInfantry();
            count += player.GetArmy(i).GetCavalry();
            count += player.GetArmy(i).GetArtillery();
            count += player.GetArmy(i).GetTank();
            count += player.GetArmy(i).GetFighter();

        }
        return count;

    }

    public static bool canBuildInfantry(Nation player)
    {
        if (player.getAP() < 1 || player.getNumberGood(MyEnum.Goods.arms) < 2)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public static bool canBuildCavalry(Nation player)
    {
        if (player.getAP() < 1 || player.getNumberGood(MyEnum.Goods.arms) < 2 ||
            player.getNumberResource(MyEnum.Resources.meat) < player.getTotalPOP() / 20 + 1)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public static bool canBuildArt(Nation player)
    {
        if (player.getAP() < 1 || player.getNumberGood(MyEnum.Goods.arms) < 3)
        {
            return false;
        }
        else
        {
            return true;
        }
    }


    public static bool canBuildTank(Nation player)
    {
        if (player.getAP() < 1 || player.getNumberGood(MyEnum.Goods.tank) < 1)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public static bool canBuildFighter(Nation player)
    {
        if (player.getAP() < 1 || player.getNumberGood(MyEnum.Goods.fighter) < 1)
        {
            return false;
        }
        else
        {
            return true;
        }
    }


    public static bool canGetCulture(Nation player)
    {
        bool able = true;
        MyEnum.Era era = State.era;
        if (player.getNumberGood(MyEnum.Goods.paper) < 1 || player.getAP() < 1)
        {
            able = false;
        }
        if (era != MyEnum.Era.Late && player.getNumberResource(MyEnum.Resources.spice) < 1)
        {
            able = false;
        }
        if (era != MyEnum.Era.Early)
        {
            if (player.getNumberGood(MyEnum.Goods.clothing) < 1)
            {
                able = false;
            }
        }
        if (era == MyEnum.Era.Late && player.getNumberGood(MyEnum.Goods.telephone) < 1)
        {
            able = false;
        }
        return able;
    }


    public static bool canDoResearch(Nation player)
    {
        bool able = true;
        if (player.getAP() < 1 || player.getNumberGood(MyEnum.Goods.paper) < 1 ||
            player.getNumberGood(MyEnum.Goods.parts) < 1)
        {
            able = false;
        }
        MyEnum.Era era = State.era;
        if (era != MyEnum.Era.Early && player.getNumberGood(MyEnum.Goods.chemicals) < 1)
        {
            able = false;
        }
        if (era == MyEnum.Era.Late && player.getNumberGood(MyEnum.Goods.gear) < 1)
        {
            able = false;
        }
        return able;
    }

    public static bool canBuildFrigate(Nation player)
    {
        if (player.getUrbanPOP() < 1 || player.getNumberGood(MyEnum.Goods.arms) < 1 ||
            player.getNumberGood(MyEnum.Goods.lumber) < 1 || player.getNumberGood(MyEnum.Goods.fabric) < 1 ||
            player.GetShipyardLevel() < 1)
        {
            return false;
        }
        else
        {
            return true;
        }

    }


    public static bool canBuildIronclad(Nation player)
    {
        if (player.getUrbanPOP() < 1 || player.getNumberGood(MyEnum.Goods.arms) < 3 ||
          player.getNumberGood(MyEnum.Goods.steel) < 3 || player.getNumberGood(MyEnum.Goods.parts) < 1 ||
          player.getNumberGood(MyEnum.Goods.gear) < 1 || player.GetShipyardLevel() < 3)
        {
            return false;
        }
        else
        {
            return true;
        }
    }


    public static bool canBuildDreadnought(Nation player)
    {
        if (player.getUrbanPOP() < 1 || player.getNumberGood(MyEnum.Goods.arms) < 3 ||
        player.getNumberGood(MyEnum.Goods.steel) < 3 || player.getNumberGood(MyEnum.Goods.parts) < 1 ||
        player.getNumberGood(MyEnum.Goods.gear) < 1 || player.GetShipyardLevel() < 3)
        {
            return false;
        }
        else
        {
            return true;
        }

    }


    public static bool canBuildNavy(Nation player)
    {
        if (player.GetTechnologies().Contains("oil_powered_ships"))
        {
            if (canBuildDreadnought(player))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        if (player.GetTechnologies().Contains("ironclad"))
        {
            if (canBuildIronclad(player))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        if (canBuildFrigate(player))
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    public static bool canIncreasePop(Nation player)
    {
        int alreadyIncreased = player.getPOPIncreasedThisTurn();
        if(alreadyIncreased == 2)
        {
            return false;
        }
        if(alreadyIncreased == 1 && player.getNumberGood(MyEnum.Goods.chemicals) < 1)
        {
            return false;
        }
        
        if (player.getNumberResource(MyEnum.Resources.wheat) < player.getTotalPOP()/20 ||
        player.getNumberGood(MyEnum.Goods.clothing) < 1 || player.getAP() < 1)
        {
            return false;
        }
        return true;
        
    }


    public static bool canReduceCorruption(Nation player)
    {
        if (player.getNumberResource(MyEnum.Resources.spice) < 1 || player.getAP() < 1 ||
            player.getNumberGood(MyEnum.Goods.paper) < 1)
        {
            return false;
        }
        else
        {
            return true;
        }
    }





}

      
        




      





