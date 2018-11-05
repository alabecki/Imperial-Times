using assemblyCsharp;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WorldMapStrategyKit;

public class AdminAI
{
    
    //FUnctions for Resource Maintainece 
    public void handleEachResrouceNeed(Nation player)
    {
        handleResourceNeed(MyEnum.Resources.wheat, player);
        handleResourceNeed(MyEnum.Resources.meat, player);
        handleResourceNeed(MyEnum.Resources.fruit, player);
        handleResourceNeed(MyEnum.Resources.coal, player);
        handleResourceNeed(MyEnum.Resources.oil, player);

    }
    public void handleResourceNeed(MyEnum.Resources res, Nation player)
    {
        float forecast = checkMaintainceNeed(res, player);
        if (forecast < 1)
        {
            int needed = 1 + (int)Mathf.Abs(forecast);

            if (res == MyEnum.Resources.wheat)
            {
                needed += 1;
            }
            for (int i = 0; i < needed; i++)
            {
                if (player.getTotalCurrentBiddingCost() >= player.getGold())
                {
                    return;
                }
                MyEnum.OffBidLevels level = checkResourceSupplyVsDemand(res);
                Signal newSignal =
                    new Signal(MyEnum.marketChoice.bid, player.getIndex(), res, MyEnum.Goods.arms, 1, level);
                State.market.addResourceBid(res, newSignal);
                updateTotalBidCostRes(res, player, level);
            }
        }

    }
    public float checkMaintainceNeed(MyEnum.Resources res, Nation player)
    {
        float currentInventory = player.getNumberResource(res);
        float producing = PlayerCalculator.getResourceProducing(player, res);
        float consuming = getConsuming(res, player);
        float forecast = (currentInventory + producing) - consuming;
        return forecast;
    }
    public float getConsuming(MyEnum.Resources res, Nation player)
    {
        if (res == MyEnum.Resources.wheat)
        {
            return player.getTotalPOP() / 10;
        }
        if (res == MyEnum.Resources.meat || res == MyEnum.Resources.fruit)
        {
            return player.getTotalPOP() / 20;
        }
        if (res == MyEnum.Resources.coal)
        {
            return PlayerCalculator.coalNeededForRailRoads(player);
        }
        if (res == MyEnum.Resources.oil)
        {
            return player.getOilNeeded();
        }
        return 0f;
    }
    public MyEnum.OffBidLevels checkResourceSupplyVsDemand(MyEnum.Resources res)
    {
        int offeredLastTurn = State.market.getNumberOfResourcesOffered(res);
        int soldLastTurn = State.market.getNumberResourcesSold(res);
        if (offeredLastTurn - soldLastTurn < 2)
        {
            return MyEnum.OffBidLevels.high;
        }
        else if (offeredLastTurn - soldLastTurn > 6)
        {
            return MyEnum.OffBidLevels.low;
        }
        else
        {
            return MyEnum.OffBidLevels.medium;
        }
    }


    public void updateTotalBidCostRes(MyEnum.Resources res, Nation player, MyEnum.OffBidLevels level)
    {
        float price = State.market.getPriceOfResource(res);
        if (level == MyEnum.OffBidLevels.high)
        {
            price = price * 1.25f;
        }
        if (level == MyEnum.OffBidLevels.low)
        {
            price = price + 0.75f;
        }
        player.increaseTotalCurrentBiddingCost(price);
    }
    public void updateTotalBidCostGood(MyEnum.Goods good, Nation player, MyEnum.OffBidLevels level)
    {
        float price = State.market.getPriceOfGood(good);
        if (level == MyEnum.OffBidLevels.high)
        {
            price = price * 1.25f;
        }
        if (level == MyEnum.OffBidLevels.low)
        {
            price = price + 0.75f;
        }
        player.increaseTotalCurrentBiddingCost(price);
    }


    public void tryIncreasePOP(Nation player)
    {
        bool able = PlayerCalculator.canIncreasePop(player);
        if (able)
        {
            PlayerPayer.payForMorePOP(player);
            player.getAI().GetTopLevel().alterMacroPriority(player, MyEnum.macroPriorities.increasePOP, -0.25f);
        }
        else
        {
            if (player.getNumberResource(MyEnum.Resources.wheat) < player.getTotalPOP() / 20)
            {
                tryBuyResource(player, MyEnum.Resources.wheat);
            }
            if (player.getNumberGood(MyEnum.Goods.clothing) < 1)
            {
                tryToObtainGood(player, MyEnum.Goods.clothing);
            }
            if (player.getPOPIncreasedThisTurn() > 0)
            {
                tryToObtainGood(player, MyEnum.Goods.chemicals);
            }
        }
    }
    public void tryReduceCorruption(Nation player)
    {

        bool able = PlayerCalculator.canReduceCorruption(player);
        if (able)
        {
            PlayerPayer.payToReduceCorruption(player);
            player.getAI().GetTopLevel().alterMacroPriority(player, MyEnum.macroPriorities.corruption, -0.25f);

        }
        else
        {
            if (player.getNumberResource(MyEnum.Resources.spice) < 1)
            {
                tryBuyResource(player, MyEnum.Resources.spice);
            }
            if (player.getNumberGood(MyEnum.Goods.paper) < 1)
            {
                tryToObtainGood(player, MyEnum.Goods.paper);
            }

        }
    }

    public void tryInvestment(Nation player)
    {
        bool able = PlayerCalculator.canInvest(player);
        if (able == true)
        {
            PlayerPayer.payForIP(player);
            PlayerReceiver.receiveIP(player);

        }
        else
        {
            if (player.getNumberResource(MyEnum.Resources.spice) < 1)
            {
                tryBuyResource(player, MyEnum.Resources.spice);
            }
            if (player.getNumberGood(MyEnum.Goods.furniture) < 1)
            {
                tryToObtainGood(player, MyEnum.Goods.furniture);
            }
            MyEnum.Era era = State.era;
            if (era == MyEnum.Era.Middle)
            {
                if (player.getNumberGood(MyEnum.Goods.paper) < 1)
                {
                    tryToObtainGood(player, MyEnum.Goods.paper);
                }
            }
            if (era == MyEnum.Era.Late)
            {
                if (player.getNumberGood(MyEnum.Goods.telephone) < 1)
                {
                    tryToObtainGood(player, MyEnum.Goods.telephone);
                }

                if (player.getNumberGood(MyEnum.Goods.auto) < 1)
                {
                    tryToObtainGood(player, MyEnum.Goods.auto);
                }
            }
        }
    }
    public void tryFactory(Nation player)
    {
        decideTryInvestment(player);
        if (player.getAP() < 1)
        {
            return;
        }
        MyEnum.Goods type = player.getAI().GetTopLevel().getTopFactoryPriority();
        MyEnum.Goods factoryType = player.getAI().GetTopLevel().getTopFactoryPriority();
        if (player.industry.CheckIfCanUpgradeFactory(factoryType))
        {
            PlayerPayer.payForFactory(player, factoryType);
            PlayerReceiver.increaseFactoryLevel(player, factoryType);
            player.getAI().GetTopLevel().alterMacroPriority(player, MyEnum.macroPriorities.buildFactory, -0.25f);

        }
        else
        {
            if (player.getNumberGood(MyEnum.Goods.parts) < 0)
            {
                tryToObtainGood(player, MyEnum.Goods.parts);
            }
            if (player.getNumberGood(MyEnum.Goods.lumber) < 0)
            {
                tryToObtainGood(player, MyEnum.Goods.lumber);
            }
            if (State.era == MyEnum.Era.Late && player.getNumberGood(MyEnum.Goods.gear) < 1)
            {
                tryToObtainGood(player, MyEnum.Goods.gear);

            }
        }

    }
    public void tryProvinceDevelopment(Nation player)
    {
        decideTryInvestment(player);
        List<MyEnum.Goods> factoryOptions = getFactoryUpgradeOptions(player);
        if (factoryOptions.Count == 0)
        {
            return;
        }
        Debug.Log("Is able to build some factories");
        List<int> devOptions = getProviceDevelopmentOptions(player);
        List<int> infOptions = getProviceInfrastructureOptions(player);
        if (devOptions.Count == 0 && infOptions.Count == 0)
        {
            return;
        }
        if (devOptions.Count == 0)
        {
            tryProvinceUpgrade(player, infOptions);
        }
        else
        {
            tryProvinceDevelopment(player, devOptions);
        }
    }

    public void tryColonial(Nation player)
    {
        bool able = AdminConstraintChecker.checkIfAbleToAddColonists(player);
        if (able)
        {
            PlayerPayer.payForColonialists(player);
            player.getAI().getTopLevel().alterMacroPriority(player, MyEnum.macroPriorities.colonies, -0.2f);
        }
        else
        {
            if (player.getNumberGood(MyEnum.Goods.clothing) < 1)
            {
                tryToObtainGood(player, MyEnum.Goods.clothing);

            }
            if (player.getNumberGood(MyEnum.Goods.furniture) < 1)
            {
                tryToObtainGood(player, MyEnum.Goods.furniture);

            }
            if (State.era != MyEnum.Era.Early && player.getNumberGood(MyEnum.Goods.paper) < 1)
            {
                tryToObtainGood(player, MyEnum.Goods.paper);
            }
            if (State.era == MyEnum.Era.Late)
            {
                if (player.getNumberResource(MyEnum.Resources.spice) < 1)
                {
                    tryBuyResource(player, MyEnum.Resources.spice);
                }

            }

        }
    }
    public void tryGainInfulence(Nation player)
    {
        System.Random rn = new System.Random();
        float value = rn.Next(0, 1);
        if (value <= 0.5)
        {
            tryImproveMilitary(player);
            player.getAI().GetTopLevel().alterMacroPriority(player, MyEnum.macroPriorities.spheres, -0.25f);

        }
        else
        {
            tryCulture(player);
            player.getAI().GetTopLevel().alterMacroPriority(player, MyEnum.macroPriorities.spheres, -0.25f);

        }
    }
    public void tryResearch(Nation player)
    {
        MyEnum.Era era = State.era;
        bool able = PlayerCalculator.canDoResearch(player);
        if (able)
        {
            PlayerPayer.PayForResearch(player);
            PlayerReceiver.CollectResearchPoints(player);
            player.getAI().GetTopLevel().alterMacroPriority(player, MyEnum.macroPriorities.research, -0.25f);

        }
        else
        {
            if (player.getNumberGood(MyEnum.Goods.parts) < 1)
            {
                tryToObtainGood(player, MyEnum.Goods.parts);
            }
            if (player.getNumberGood(MyEnum.Goods.paper) < 1)
            {
                tryToObtainGood(player, MyEnum.Goods.paper);
            }
            if (era != MyEnum.Era.Early && player.getNumberGood(MyEnum.Goods.chemicals) < 1)
            {
                tryToObtainGood(player, MyEnum.Goods.chemicals);
            }
            if (era == MyEnum.Era.Late && player.getNumberGood(MyEnum.Goods.gear) < 1)
            {
                tryToObtainGood(player, MyEnum.Goods.gear);
            }
        }
    }

    public void tryImproveMilitary(Nation player)
    {
        int milLevel = player.getArmyLevel();
        int numInf = PlayerCalculator.getTotalNumberInfantry(player);
        int numArt = PlayerCalculator.getTotalNumberCavalry(player);
        int numCav = PlayerCalculator.getTotalNumberArtillery(player);
        int numTnk = PlayerCalculator.getTotalNumberTanks(player);
        int numFgt = PlayerCalculator.getTotalNumberFighters(player);
        numInf -= player.getColonies().Count;
        if (numInf < numArt * 2 || numInf < numArt * 2 || numInf < numCav * 2)
        {
            if (PlayerCalculator.canBuildInfantry(player))
            {
                PlayerPayer.PayInfantry(player);
                player.getAI().GetTopLevel().alterMacroPriority(player, MyEnum.macroPriorities.conquest, -0.25f);
                return;
            }
        }
        else
        {
            if (numTnk < numArt && PlayerCalculator.canBuildTank(player))
            {
                PlayerPayer.PayTank(player);
                player.getAI().GetTopLevel().alterMacroPriority(player, MyEnum.macroPriorities.conquest, -0.25f);
                return;
            }
            if (numFgt < numArt && PlayerCalculator.canBuildFighter(player))
            {
                PlayerPayer.PayFighter(player);
                player.getAI().GetTopLevel().alterMacroPriority(player, MyEnum.macroPriorities.conquest, -0.25f);
                return;
            }
            if (!player.GetTechnologies().Contains("mobile_warfare") &&
                numCav < numArt && PlayerCalculator.canBuildCavalry(player))
            {
                PlayerPayer.PayCavalry(player);
                player.getAI().GetTopLevel().alterMacroPriority(player, MyEnum.macroPriorities.conquest, -0.25f);
            }

        }

    }
    public void tryBuildNavy(Nation player)
    {
        MyEnum.Era era = State.era;
        bool able = PlayerCalculator.canBuildNavy(player);
        if (able == true)
        {
            if (player.GetTechnologies().Contains("oil_powered_ships"))
            {
                PlayerPayer.PayDreadnought(player);
                player.getAI().GetTopLevel().alterMacroPriority(player, MyEnum.macroPriorities.navy, -0.25f);
                return;
            }

            else if (player.GetTechnologies().Contains("ironclad"))
            {
                PlayerPayer.PayIronClad(player);
                player.getAI().GetTopLevel().alterMacroPriority(player, MyEnum.macroPriorities.navy, -0.25f);

                return;
            }
            else
            {
                PlayerPayer.PayFrigate(player);
                player.getAI().GetTopLevel().alterMacroPriority(player, MyEnum.macroPriorities.navy, -0.25f);
            }
        }
        else
        {
            if (player.GetTechnologies().Contains("oil_powered_ships"))
            {
                if(player.GetShipyardLevel() < 3)
                {
                    tryBuildShipyard(player);
                }
                while (player.getNumberGood(MyEnum.Goods.arms) < 6)
                {
                    tryToObtainGood(player, MyEnum.Goods.arms);
                }
                while (player.getNumberGood(MyEnum.Goods.steel) < 6)
                {
                    tryToObtainGood(player, MyEnum.Goods.arms);
                }
                if (player.getNumberGood(MyEnum.Goods.parts) < 1)
                {
                    tryToObtainGood(player, MyEnum.Goods.parts);
                }
                if (player.getNumberGood(MyEnum.Goods.gear) < 1)
                {
                    tryToObtainGood(player, MyEnum.Goods.gear);
                }
                return;
            }

            else if (player.GetTechnologies().Contains("ironclad"))
            {
                if (player.GetShipyardLevel() < 2)
                {
                    tryBuildShipyard(player);
                    return;
                }
                while (player.getNumberGood(MyEnum.Goods.arms) < 4)
                {
                    tryToObtainGood(player, MyEnum.Goods.arms);
                }
                if (player.getNumberGood(MyEnum.Goods.steel) < 1)
                {
                    tryToObtainGood(player, MyEnum.Goods.steel);
                }
                if (player.getNumberGood(MyEnum.Goods.parts) < 1)
                {
                    tryToObtainGood(player, MyEnum.Goods.parts);
                }
                return;
            }
            else
            {
                if (player.GetShipyardLevel() < 1)
                {
                    tryBuildShipyard(player);
                    return;
                }
                while (player.getNumberGood(MyEnum.Goods.arms) < 2)
                {
                    tryToObtainGood(player, MyEnum.Goods.arms);
                }
                if (player.getNumberGood(MyEnum.Goods.fabric) < 1)
                {
                    tryToObtainGood(player, MyEnum.Goods.fabric);
                }
                if (player.getNumberGood(MyEnum.Goods.lumber) < 1)
                {
                    tryToObtainGood(player, MyEnum.Goods.lumber);
                }
            }
        }

    }
    public void tryCulture(Nation player)
    {
        if (PlayerCalculator.canGetCulture(player))
        {
            PlayerPayer.payForCultureCard(player);
            player.getAI().GetTopLevel().alterMacroPriority(player, MyEnum.macroPriorities.culture, -0.25f);

        }
        else
        {
            if (player.getNumberGood(MyEnum.Goods.paper) < 1)
            {
                tryToObtainGood(player, MyEnum.Goods.paper);
            }
        }
        MyEnum.Era era = State.era;
        if (era != MyEnum.Era.Late && player.getNumberResource(MyEnum.Resources.spice) < 1)
        {
            tryBuyResource(player, MyEnum.Resources.spice);
        }
        if (era != MyEnum.Era.Early)
        {
            tryToObtainGood(player, MyEnum.Goods.clothing);
        }
        if (era == MyEnum.Era.Late && player.getNumberGood(MyEnum.Goods.telephone) < 1)
        {
            tryToObtainGood(player, MyEnum.Goods.telephone);
        }
    }


    //Shopping
    public void bidOnGood(Nation player, MyEnum.Goods good)
    {
        MyEnum.OffBidLevels level = checkGoodSupplyVsDemand(good);
        Signal newSignal =
            new Signal(MyEnum.marketChoice.bid, player.getIndex(), MyEnum.Resources.wheat, good, 1, level);
        State.market.addGoodBid(good, newSignal);
        updateTotalBidCostGood(good, player, level);
    }
    public void bidOnResource(Nation player, MyEnum.Resources res)
    {
        MyEnum.OffBidLevels level = checkResourceSupplyVsDemand(res);
        Signal newSignal =
            new Signal(MyEnum.marketChoice.bid, player.getIndex(), res, MyEnum.Goods.parts, 1, level);
        State.market.addResourceBid(res, newSignal);
        updateTotalBidCostRes(res, player, level);
    }
    public MyEnum.OffBidLevels checkGoodSupplyVsDemand(MyEnum.Goods good)
    {
        int offeredLastTurn = State.market.getNumberOfGoodOffered(good);
        int soldLastTurn = State.market.getNumberOfGoodsSoldLastTurn(good);
        if (offeredLastTurn - soldLastTurn < 2)
        {
            return MyEnum.OffBidLevels.high;
        }
        else if (offeredLastTurn - soldLastTurn > 6)
        {
            return MyEnum.OffBidLevels.low;
        }
        else
        {
            return MyEnum.OffBidLevels.medium;
        }
    }

    public List<MyEnum.Goods> getFactoryUpgradeOptions(Nation player)
    {
        List<MyEnum.Goods> options = new List<MyEnum.Goods>();

        foreach (MyEnum.Goods good in Enum.GetValues(typeof(MyEnum.Goods)))
        {
            if (player.industry.CheckIfCanUpgradeFactory(good))
            {
                options.Add(good);
            }
        }
        return options;
    }

    public List<int> getProviceDevelopmentOptions(Nation player)
    {
        List<int> options = new List<int>();
        for (int i = 0; i < player.getProvinces().Count; i++)
        {
            int pIndex = player.getProvinces()[i];
            assemblyCsharp.Province prov = State.getProvinces()[pIndex];
            if (PlayerCalculator.checkUpgradeDevelopment(prov, player))
            {
                options.Add(pIndex);
            }
        }
        return options;

    }

    public List<int> getProviceInfrastructureOptions(Nation player)
    {
        List<int> options = new List<int>();
        for (int i = 0; i < player.getProvinces().Count; i++)
        {
            int pIndex = player.getProvinces()[i];
            assemblyCsharp.Province prov = State.getProvinces()[pIndex];
            if (PlayerCalculator.canUpgradeRailRoad(prov, player))
            {
                options.Add(pIndex);
            }
        }
        return options;
    }

    public void tryBuyResource(Nation player, MyEnum.Resources res)
    {
        bool afford = AdminConstraintChecker.checkIfCanAffordResource(player, res);
        if (afford == true)
        {
            bidOnResource(player, res);
        }
    }

    public void tryBuyGood(Nation player, MyEnum.Goods good)
    {
        bool afford = AdminConstraintChecker.checkIfCanAffordGood(player, good);
        if (afford == true)
        {
            bidOnGood(player, good);
        }
    }

    public void tryToObtainGood(Nation player, MyEnum.Goods good)
    {
        float canProduce = player.industry.determineCanProduce(MyEnum.Goods.clothing, player);
        if (canProduce > 1)
        {
            decideIfProduceGood(player, good, canProduce);
        }
        else
        {
            tryBuyGood(player, good);
        }
    }

    public bool decideIfProduceGood(Nation player, MyEnum.Goods good, float canProduce)
    {
        if (player.getAP() < 1)
        {
            return false;
        }
        int factoryLevel = player.industry.getFactoryLevel(good);
        if (factoryLevel == 0)
        {
            PlayerPayer.payForFactoryProduction(player, good, 1);
            return true;
        }
        if (factoryLevel == 1)
        {
            if (canProduce < 2)
            {
                return false;
            }
            else
            {
                float amount = Math.Min(canProduce, 4.0f);
                PlayerPayer.payForFactoryProduction(player, good, (int)amount);
            }
        }
        if (factoryLevel == 2)
        {
            if (canProduce < 3)
            {
                return false;
            }
            else
            {
                float amount = Math.Min(canProduce, 7.0f);
            }
        }
        if (factoryLevel == 3)
        {
            if (canProduce < 3)
            {
                return false;
            }
            else
            {
                float amount = Math.Min(canProduce, 12.0f);
            }
        }
        return true;


    }

    public bool decideTryInvestment(Nation player)
    {
        MyEnum.Era era = State.era;

        if (era == MyEnum.Era.Early && player.getIP() < 1)
        {
            return true;
        }
        if (era == MyEnum.Era.Middle && player.getIP() < 2)
        {
            tryInvestment(player);
            return true;
        }
        if (era == MyEnum.Era.Middle && player.getIP() < 3)
        {
            tryInvestment(player);
            return true;
        }
        return false;

    }

    public void tryProvinceDevelopment(Nation player, List<int> devOptions)
    {
        int intResOrderCounter = 0;
        while (true)
        {
            MyEnum.Resources desiredRes = player.getAI().getTopLevel().getTopResourcePriorityN(intResOrderCounter);

            for (int i = 0; i < devOptions.Count; i++)
            {
                assemblyCsharp.Province prov = State.getProvinces()[devOptions[i]];
                MyEnum.Resources res = prov.getResource();
                if (res == desiredRes)
                {
                    PlayerPayer.payDevelopment(player, prov);
                    player.getAI().GetTopLevel().alterMacroPriority(player, MyEnum.macroPriorities.developProvince, -0.25f);

                    //Player takes care of increase;
                    return;
                }
            }
            intResOrderCounter++;
        }

    }

    public void tryProvinceUpgrade(Nation player, List<int> infOptions)
    {
        int intResOrderCounter = 0;
        while (true)
        {
            MyEnum.Resources desiredRes = player.getAI().getTopLevel().getTopResourcePriorityN(intResOrderCounter);

            for (int i = 0; i < infOptions.Count; i++)
            {
                assemblyCsharp.Province prov = State.getProvinces()[infOptions[i]];
                MyEnum.Resources res = prov.getResource();
                if (res == desiredRes)
                {
                    PlayerPayer.payDevelopment(player, prov);
                    player.getAI().GetTopLevel().alterMacroPriority(player, MyEnum.macroPriorities.developProvince, -0.25f);
                    //Player takes care of increase;
                    return;
                }
            }
            intResOrderCounter++;
        }

    }

    public void tryImproveDefense(Nation player)
    {
        //first consider building a fort
        List<int> borderProvs = State.mapUtilities.getProvincesOnBorders(player);
        if (borderProvs.Count > 0)
        {
            //consider best option
            int chosenProvIndex = 0;
            int relationLevel = 110;
            for (int i = 0; i < borderProvs.Count; i++)
            {
                WMSK map = WMSK.instance;
                List<WorldMapStrategyKit.Province> neighbours = map.ProvinceNeighbours(borderProvs[i]);
                foreach (WorldMapStrategyKit.Province mapProv in neighbours)
                {
                    int owner = mapProv.countryIndex;
                    if (owner != player.getIndex())
                    {
                        if (player.getRelationFromThisPlayer(owner).getAttitude() < relationLevel)
                        {
                            relationLevel = player.getRelationFromThisPlayer(owner).getAttitude();
                            chosenProvIndex = borderProvs[i];
                        }
                    }
                }
            }
            assemblyCsharp.Province chosenProv = State.getProvinces()[chosenProvIndex];
            bool able = PlayerCalculator.canUpgradeFort(chosenProv, player);
            if (able)
            {
                PlayerPayer.payFortUpgrade(player, chosenProv);
            }

        }else
        {
            tryImproveMilitary(player);

        }

        
    }

    public void tryBuildShipyard(Nation player)
    {
        bool able = PlayerCalculator.canUpgradeShipyard(player);
        if (able)
        {
            PlayerPayer.payShipyYardUpgrade(player);
        }
        else
        {
            if(player.getNumberGood(MyEnum.Goods.lumber) < 1)
            {
                tryToObtainGood(player, MyEnum.Goods.lumber);
            }
            if (player.getNumberGood(MyEnum.Goods.steel) < 1)
            {
                tryToObtainGood(player, MyEnum.Goods.steel);
            }
            if (player.GetShipyardLevel() == 2)
            {
                if (player.getNumberGood(MyEnum.Goods.lumber) < 1)
                {
                    tryToObtainGood(player, MyEnum.Goods.lumber);
                }
                if (player.getNumberGood(MyEnum.Goods.steel) < 1)
                {
                    tryToObtainGood(player, MyEnum.Goods.steel);
                }
                if (player.getNumberGood(MyEnum.Goods.parts) < 1)
                {
                    tryToObtainGood(player, MyEnum.Goods.parts);
                }

            }
        }
    }

    //modify preferences
    public void considerMilitaryNeed(Nation player)
    {
        Dictionary<int, Relation> relations = player.getRelations();
        int strenght = State.history.CalculateMilitaryScore(player);
       
        foreach (KeyValuePair<int, Relation> entry in relations)
        {
            int otherIndex = entry.Value.getRelationTo();
            Nation other = State.getNations()[otherIndex];
            if(entry.Value.getAttitude() < 30)
            {
                if (State.mapUtilities.shareLandBorder(player, other))
                {
                    if (State.history.CaululateArmyScore(other) / strenght > 1.25)
                    {
                        player.getAI().getTopLevel().alterMacroPriority(player, MyEnum.macroPriorities.defense, 0.35f);
                    }
                }
                else
                {
                    if(State.history.getNavelProjection(other)/ strenght > 1.22)
                    {
                        player.getAI().getTopLevel().alterMacroPriority(player, MyEnum.macroPriorities.defense, 0.35f);

                    }
                }

            }


            
        }

    }


    public void updateMacroPriorities(Nation player)
    {
        MyEnum.macroPriorities mainFocus = player.getAI().getPrimaryFocus();
        MyEnum.macroPriorities secondFocus = player.getAI().getSecondaryFocus();

        player.getAI().getTopLevel().alterMacroPriority(player, mainFocus, 0.16f);

        player.getAI().getTopLevel().alterMacroPriority(player, secondFocus, 0.08f);

        float corrupt = player.GetCorruption();
        player.getAI().getTopLevel().alterMacroPriority(player, MyEnum.macroPriorities.corruption, corrupt * 0.1f);
        MyEnum.Era era = State.GerEra();
        int factLevels = player.industry.getTotalNumberFactoryLevels();
        if(factLevels < 3 && era == MyEnum.Era.Early)
        {
            player.getAI().getTopLevel().alterMacroPriority(player, MyEnum.macroPriorities.buildFactory, 0.05f);
        }
        else if (factLevels < 7 && era == MyEnum.Era.Middle)
        {
            player.getAI().getTopLevel().alterMacroPriority(player, MyEnum.macroPriorities.buildFactory, 0.05f);
        }
        else if (factLevels < 12 && era == MyEnum.Era.Late)
        {
            player.getAI().getTopLevel().alterMacroPriority(player, MyEnum.macroPriorities.buildFactory, 0.05f);
        }

        if(player.Stability < 1)
        {
            player.getAI().getTopLevel().alterMacroPriority(player, MyEnum.macroPriorities.culture, 0.02f);
        }
        else if (player.Stability < 0)
        {
            player.getAI().getTopLevel().alterMacroPriority(player, MyEnum.macroPriorities.culture, 0.075f);
        }
        else if (player.Stability < -1)
        {
            player.getAI().getTopLevel().alterMacroPriority(player, MyEnum.macroPriorities.culture, 0.15f);
        }
        else if (player.Stability < -2)
        {
            player.getAI().getTopLevel().alterMacroPriority(player, MyEnum.macroPriorities.culture, 0.35f);
            player.getAI().getTopLevel().alterMacroPriority(player, MyEnum.macroPriorities.corruption, 0.1f);

        }


    }


    }











