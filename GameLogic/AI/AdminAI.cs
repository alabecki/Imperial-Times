using assemblyCsharp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using WorldMapStrategyKit;

public class AdminAI
{
    private int apAdds;
    private int ppAdds;
    private int ipAdds;
    private int rpAdds;

    public int ApAdds { get => apAdds; set => apAdds = value; }
    public int PpAdds { get => ppAdds; set => ppAdds = value; }
    public int IpAdds { get => ipAdds; set => ipAdds = value; }
    public int RpAdds { get => rpAdds; set => rpAdds = value; }


    public void handleAP(Nation player)
    {

        tryToIncreaseAP(player);


        if (State.turn > 12 && player.getAP() <= 8)
        {
            tryToIncreaseAP(player);
        }

        if(State.turn > 25 && player.getAP() <= 14)
        {
            tryToIncreaseAP(player);

        }

        if (State.turn > 36)
        {
            tryToIncreaseAP(player);
        }    
    }
   
    public void handleDP(Nation player)
    {
       
        tryToIncreaseDP(player);

        if (State.turn >= 16 && State.turn < 32 && player.getDP() < 4)
        {
            tryToIncreaseDP(player);
            if (player.getDP() < 4)
            {
                tryToIncreaseDP(player);
            }
        }

        else if (State.turn >= 32 && State.turn < 64 && player.getDP() < 8)
        {
            tryToIncreaseDP(player);
            if (player.getDP() < 6)
            {
                tryToIncreaseDP(player);
            }
            if (player.getDP() < 6)
            {
                tryToIncreaseDP(player);
            }
        }
        else
        {
            tryToIncreaseDP(player);
            if (player.getDP() < 8)
            {
                tryToIncreaseDP(player);
            }
            if (player.getDP() < 7)
            {
                tryToIncreaseDP(player);
            }
        }
        
    }

    public void considerShipConstruction(Nation player)
    {
        int totalNumShips = PlayerCalculator.getTotalNumberShips(player);
        int desiredNumberOfShips = State.turn / 14 + 2;
        if (totalNumShips < desiredNumberOfShips)
        {
            tryBuildNavy(player);
        }
    }

    public void considerUnitDrafting(Nation player)
    {
        if(player.landForces.Strength < 3)
        {
            tryToBuildUnit(player);

        }
        int desiredNumbeOfArmies = (State.turn / 12) + 3;
        if (player.landForces.Strength < desiredNumbeOfArmies)
        {
            tryToBuildUnit(player);
        }
        if (considerDefensiveNeed(player))
        {
            tryToBuildUnit(player);
        }
    }

    public void considerDoctrines(Nation player)
    {
        int numberOfDoctrines = player.landForces.numberOfDoctrines();
        int desiredNumberOfDoctrines = 0;
        if(State.turn < 13)
        {
            return;
        }
        if (State.turn < 32)
        {
            desiredNumberOfDoctrines = State.turn / 12;
        }
        else if (State.turn < 48)
        {
            desiredNumberOfDoctrines = State.turn / 10;
        }
        else

        {
            desiredNumberOfDoctrines = State.turn / 8;
        }
        if (numberOfDoctrines < desiredNumberOfDoctrines)
        {
            tryToAddDoctrine(player);
        }
    }

    public void considerFactory(Nation player)
    {
        List<MyEnum.Goods> desiredFactoryOptions = getDesiredFactoryList(player);
        Debug.Log("Number of Desired Factories" + desiredFactoryOptions.Count + " ######################################################");
        if(desiredFactoryOptions.Count > 0)
            if (player.getIP() < 1)
            {
                Debug.Log("Need IP");
                if (PlayerCalculator.canMakeDevelopmentAction(player))
                {
                    PlayerPayer.payForDevelopmentAction(player, 1);
                    PlayerReceiver.receiveIP(player);
                    ipAdds++;
                }
            }
            else
            {
                TopLevel topLevel = player.getAI().getTopLevel();
                bool flag = false;
                int count = 0;
                MyEnum.Goods wish = MyEnum.Goods.arms;
                while(flag == false && count < 11)
                {
                    wish = topLevel.getFactoryPriority(count);
                    Debug.Log(wish);
                    if (desiredFactoryOptions.Contains(wish))
                    {
                        flag = true;
                    }
                    else
                    {
                        count++;
                        if(count == 11)
                        {
                            return;
                        }
                    }
                }
                Debug.Log("Wish setteled on: " + wish);
                if (player.industry.CheckIfCanUpgradeFactory(player, wish))
                {
                    Debug.Log("Should build factory");
                    PlayerPayer.payForFactory(player, wish);
                    PlayerReceiver.increaseFactoryLevel(player, wish);
                    topLevel.alterResourcesAndGoodsToKeepAfterBuildFactory(player, wish);

                }
                else
                {
                    Debug.Log("Cannot build a " + wish + " factory at this time");
                    if(player.getNumberGood(MyEnum.Goods.parts) < 2)
                    {
                        tryToObtainParts(player);
                    }
                    if(player.getNumberGood(MyEnum.Goods.lumber) < 2)
                    {
                        tryToObtainLumber(player);
                    }
                    if (player.getIP() < 2)
                    {
                        if (PlayerCalculator.canMakeDevelopmentAction(player))
                        {
                            PlayerPayer.payForDevelopmentAction(player, 1);
                            PlayerReceiver.receiveIP(player);
                            ipAdds++;
                        }
                    }
                }
            }
    }

    public void considerProvinceDevelopment(Nation player)
    {
        Debug.Log("Considering Province Development!   dddddd");
        List<int> devOptions = getProviceDevelopmentOptions(player);
        Debug.Log("Number of Province Development Options: " + devOptions.Count);
        if(devOptions.Count > 0)
        {
            if (player.getIP() < 1)
            {
                if (PlayerCalculator.canMakeDevelopmentAction(player))
                {
                    PlayerPayer.payForDevelopmentAction(player, 1);
                    PlayerReceiver.receiveIP(player);
                    ipAdds++;
                }
            }
            else
            {
                tryProvinceDevelopment(player, devOptions);
            }
        }
    }

    public void considerDevelopment(Nation player)
    {
        int numberOfTrains = player.industry.getNumberOfTrains();
        int numberOfDevelopments = PlayerCalculator.getNumberProvDevelopments(player);
        int numberOfTrainsNeeded = numberOfDevelopments / 2;
        if(numberOfTrainsNeeded > numberOfTrains)
        {
            return;
        }
        if (checkForPossibleInvesmentOptions(player))
        {
            Debug.Log("Has possible investment options");
            if (player.getIP() < 1)
            {
                if (PlayerCalculator.canMakeDevelopmentAction(player))
                {
                    PlayerPayer.payForDevelopmentAction(player, 1);
                    PlayerReceiver.receiveIP(player);
                    ipAdds++;
                }
            }
            else
            {
                performDevelopmentAction(player);
            }
        }
    }

    public void considerRailroads(Nation player)
    {
        int numberOfRailroads = PlayerCalculator.getNumberProvRailRoads(player);
        int numberOfProvDevelopments = PlayerCalculator.getTotalDevelopment(player);
        if(numberOfRailroads - numberOfProvDevelopments >= 2)
        {
            return;
        }
        List<int> options = player.industry.getProvincesWhereRailroadsCanBeBuilt(player);
        if (PlayerCalculator.canBuildRaiload(player))
        {
            if(options.Count > 0)
            {
                Debug.Log("Number of Railroad Options: " + options.Count);
                assemblyCsharp.Province prov = State.getProvinces()[options[0]];
                PlayerPayer.payForRailroad(player);
                prov.railroad = true;
                AI ai = player.getAI();
                ai.NewRailRoads.Enqueue(prov.getIndex());
               // ai.NewRailRoads.Enqueue(prov.getIndex());           ?????????????
            }    
        }
        else
        {
            if (options.Count > 0 && player.GetTechnologies().Contains("steam_locomotive"))
            {
                if (PlayerCalculator.canMakeDevelopmentAction(player))
                {
                    PlayerPayer.payForDevelopmentAction(player, 1);
                    PlayerReceiver.receiveIP(player);
                    ipAdds++;
                }
            }
            if(player.getNumberGood(MyEnum.Goods.steel) < 1)
            {
                tryToObtainSteel(player);
            }
            if(player.getNumberGood(MyEnum.Goods.lumber) < 1)
            {
                tryToObtainLumber(player);
            }
        }
    }

    public void considerTrains(Nation player)
    {
        int numDevelopments = PlayerCalculator.getNumberProvDevelopments(player);
        int numTrains = player.industry.getNumberOfTrains();
        int totalProv = PlayerCalculator.getTotalNumberOfProvinces(player);
        if(numDevelopments >= (numTrains/2))
        {
            if(player.getIP() < 1)
            {
                if (PlayerCalculator.canMakeDevelopmentAction(player))
                {
                    PlayerPayer.payForDevelopmentAction(player, 1);
                    PlayerReceiver.receiveIP(player);
                    ipAdds++;
                }
            }
            if (PlayerCalculator.canBuildTrain(player))
            {
                PlayerPayer.payForTrain(player);
                PlayerReceiver.buildTrain(player);
            }
            else
            {
                tryToObtainParts(player);
            }
        }
    }

    public void considerCulture(Nation player)
    {
        int numberCulture = player.getCultureCards().Count;
        int desiredNumberOfCultureCards = 0;
        if (State.turn < 40)
        {
            desiredNumberOfCultureCards = State.turn / 8;
        }
        else if (State.turn < 75)
        {
            desiredNumberOfCultureCards = State.turn / 6;
        }
        else
        {
            desiredNumberOfCultureCards = State.turn / 5;
        }
        if (PlayerCalculator.canMakeDevelopmentAction(player) && numberCulture < desiredNumberOfCultureCards)
        {
            PlayerPayer.payForDevelopmentAction(player, 1);
            PlayerReceiver.collectCultureCard(player);
        }
    }


    public void performDevelopmentAction(Nation player)
    {
        TopLevel topLevel = player.getAI().getTopLevel();

        MyEnum.developmentPriorities priority = topLevel.getTopDevelopmentPriority();
        if (priority == MyEnum.developmentPriorities.buildFactory)
        {
            Debug.Log("Build Factory ----------------------");
            List<MyEnum.Goods> factoryOptions = getPotentialFactoryUpgradeOptions(player);
            if (factoryOptions.Count == 0)
            {
                topLevel.developmentPriorities[MyEnum.developmentPriorities.buildFactory]--;
                Debug.Log("No factory options :(");
                return;
            }
            for (int i = 0; i < 4; i++)
            {
                bool result = tryFactory(player, i);
                if (result == true)
                {
                    topLevel.developmentPriorities[MyEnum.developmentPriorities.buildFactory]--;
                    topLevel.metaPriorities[MyEnum.metaPriorities.development]--;
                    break;
                }
            }
        }
        else if (priority == MyEnum.developmentPriorities.developProvince)
        {
            Debug.Log("Develop Province ------------------------");
            if (tryProvinceDevelopment(player))
            {
                topLevel.developmentPriorities[MyEnum.developmentPriorities.developProvince]--;
                topLevel.metaPriorities[MyEnum.metaPriorities.development]--;
            }
        }
        else if (priority == MyEnum.developmentPriorities.railroad)
        {
            Debug.Log("Build Railroad -------------------------------");
            if (tryToBuildRailRoad(player))
            {
                player.getAI().GetTopLevel().developmentPriorities[MyEnum.developmentPriorities.railroad]--;
                topLevel.metaPriorities[MyEnum.metaPriorities.development]--;

            }
        }
        else if (priority == MyEnum.developmentPriorities.fortification)
        {
            Debug.Log("Fortification --------------------------");
            if (PlayerCalculator.canUpgradeFort(player))
            {
                PlayerPayer.payFortUpgrade(player);
                player.getAI().GetTopLevel().developmentPriorities[MyEnum.developmentPriorities.fortification]--;
                topLevel.metaPriorities[MyEnum.metaPriorities.development]--;

            }
            else
            {
                tryToObtainArms(player);
                tryToObtainArms(player);
            }
        }
        else if (priority == MyEnum.developmentPriorities.trains)
        {
            Debug.Log("Trains --------------------------------------");
            if (PlayerCalculator.canBuildTrain(player))
            {
                PlayerPayer.payForTrain(player);
                PlayerReceiver.buildTrain(player);
                player.getAI().GetTopLevel().developmentPriorities[MyEnum.developmentPriorities.trains]--;
                topLevel.metaPriorities[MyEnum.metaPriorities.development]--;

            }
            else
            {
                tryToObtainGear(player);
            }
        }
        else if (priority == MyEnum.developmentPriorities.warehouse)
        {
            Debug.Log("Warehouse ------------------------------------");
            if (PlayerCalculator.canUpgradeWarehouse(player))
            {
                PlayerPayer.payWarehouseExpansion(player);
                player.getAI().GetTopLevel().developmentPriorities[MyEnum.developmentPriorities.warehouse] -= 2;
                topLevel.metaPriorities[MyEnum.metaPriorities.development]--;
            }
            else
            {
                tryToObtainLumber(player);
            }
        }
    }

    public MyEnum.OffBidLevels checkResourceSupplyVsDemand(MyEnum.Resources res)
    {
        int offeredLastTurn = State.market.getNumberOfResourcesOffered(res);
        int soldLastTurn = State.market.getNumberResourcesSold(res);
        if (offeredLastTurn - soldLastTurn < 0)
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
            price = price * 1.2f;
        }
        if (level == MyEnum.OffBidLevels.low)
        {
            price = price + 0.8f;
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

    public void tryToIncreaseAP(Nation player)
    {
        if (PlayerCalculator.canAddAP(player))
        {
            PlayerPayer.payForAP(player);
            PlayerReceiver.collectAP(player, 5);
            apAdds++;
            Debug.Log("Increasing AP            !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
        }
        else
        {
            tryToBeAbleToGetAP(player);
        }
    }

    public void tryToBeAbleToGetAP(Nation player)
    {
        if(player.getNumberResource(MyEnum.Resources.wheat) < 2)
        {
            tryBuyResource(player, MyEnum.Resources.wheat);
        }
        if (player.getNumberResource(MyEnum.Resources.wheat) < 1)
        {
            tryBuyResource(player, MyEnum.Resources.wheat);
        }
        if (player.getNumberResource(MyEnum.Resources.meat) < 1)
        {
            tryBuyResource(player, MyEnum.Resources.meat);
        }
        if (player.getNumberResource(MyEnum.Resources.fruit) < 1)
        {
            tryBuyResource(player, MyEnum.Resources.fruit);
        }
        if(player.getNumberGood(MyEnum.Goods.clothing) < 1)
        {
            tryToObtainClothing(player);
        }

        if(State.turn > 20)
        {
            if (player.getNumberResource(MyEnum.Resources.wheat) < 3)
            {
                tryBuyResource(player, MyEnum.Resources.wheat);
            }
        
            if (player.getNumberResource(MyEnum.Resources.meat) < 2)
            {
                tryBuyResource(player, MyEnum.Resources.meat);
            }
            if (player.getNumberResource(MyEnum.Resources.fruit) < 2)
            {
                tryBuyResource(player, MyEnum.Resources.fruit);
            }
            if (player.getNumberGood(MyEnum.Goods.clothing) < 2)
            {
                tryToObtainClothing(player);
            }
        }
       
       // if(player.GetAPIncreasedThisTurn())
      //  {
       //     tryToObtainChemicals(player);
       // }
    }

    public void tryToIncreaseDP(Nation player)
    {
        if (PlayerCalculator.canAddDP(player))
        {

            PlayerPayer.payForDP(player);
            //  PlayerReceiver.collectDP(player);
            ppAdds++;
            Debug.Log("Increasing DP !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
        }
        else
        {
            if(player.getNumberResource(MyEnum.Resources.spice) < 1)
            {
                tryBuyResource(player, MyEnum.Resources.spice);
            }
            if(player.getNumberGood(MyEnum.Goods.furniture) < 1)
            {
                tryToObtainFurniture(player);
            }
            if(player.getNumberGood(MyEnum.Goods.paper) < 1)
            {
                tryToObtainPaper(player);
            }


            if(State.turn > 22)
            {
                if (player.getNumberResource(MyEnum.Resources.spice) < 3)
                {
                    tryBuyResource(player, MyEnum.Resources.spice);
                }
                if (player.getNumberGood(MyEnum.Goods.furniture) < 2)
                {
                    tryToObtainFurniture(player);
                }
                if (player.getNumberGood(MyEnum.Goods.paper) < 2)
                {
                    tryToObtainPaper(player);
                }

            }
           // if(player.GetDPIncreasedThisTurn())
          //  {
           //     tryToObtainArms(player);
           // }
        }
    }

    public bool tryFactory(Nation player, int priorityLevel)
    {
      
        //MyEnum.Goods type = player.getAI().GetTopLevel().getTopFactoryPriority();
        MyEnum.Goods factoryType = player.getAI().GetTopLevel().getFactoryPriority(priorityLevel);
        Debug.Log("Build a " + factoryType + " factory?");
        TopLevel topLevel = player.getAI().getTopLevel();
        if (player.industry.CheckIfCanUpgradeFactory(player, factoryType))
        {
            PlayerPayer.payForFactory(player, factoryType);
            PlayerReceiver.increaseFactoryLevel(player, factoryType);
            //  player.getAI().GetTopLevel().alterMacroPriority(player, MyEnum.macroPriorities.buildFactory, -0.25f);
            // player.getAI().getTopLevel().alterMacroPriority(player, MyEnum.macroPriorities.manufactureGoods, 0.01f);
            topLevel.metaPriorities[MyEnum.metaPriorities.development]--;
            topLevel.developmentPriorities[MyEnum.developmentPriorities.buildFactory]--;
            topLevel.alterResourcesAndGoodsToKeepAfterBuildFactory(player, factoryType);
            return true;
        }
        else
        {
            if (PlayerCalculator.canMakeDevelopmentAction(player))
            {
                PlayerPayer.payForDevelopmentAction(player, 1);
                PlayerReceiver.receiveIP(player);
                ipAdds++;
            }

            int factLevel = player.getFactoryLevel(factoryType);
            if (factLevel == 0)
            {
                if (player.getNumberGood(MyEnum.Goods.parts) < 1)
                {
                    tryToObtainParts(player);
                }
                if (player.getNumberGood(MyEnum.Goods.lumber) < 1)
                {
                    tryToObtainLumber(player);
                }
            }
            if (factLevel > 0)
            {
                if (player.getNumberGood(MyEnum.Goods.parts) < 2)
                {
                    tryToObtainParts(player);
                }
                if (player.getNumberGood(MyEnum.Goods.lumber) < 2)
                {
                    tryToObtainLumber(player);
                }
            }
            if (factLevel == 2)
            {
                if (player.getNumberGood(MyEnum.Goods.gear) < 2)
                {
                    tryToObtainGear(player);
                }
            }
        }
        return false;
    }

    public List<MyEnum.Goods> getFactoryOptions(Nation player)
    {
        List<MyEnum.Goods> options = new List<MyEnum.Goods>();

        foreach (MyEnum.Goods good in Enum.GetValues(typeof(MyEnum.Goods)))
        {
            if (good == MyEnum.Goods.steel && player.industry.CheckIfCanUpgradeFactory(player, good))
            {
                if (player.getNumberResource(MyEnum.Resources.iron) > 4)
                {
                    options.Add(good);
                }
            }
            else if ((good == MyEnum.Goods.lumber || good == MyEnum.Goods.paper) &&
                player.industry.CheckIfCanUpgradeFactory(player, good))
            {
                if (player.getNumberResource(MyEnum.Resources.wood) > 4)
                {
                    options.Add(good);
                }
            }
            else if (good == MyEnum.Goods.fabric && player.industry.
                CheckIfCanUpgradeFactory(player, good))
            {
                if (player.getNumberResource(MyEnum.Resources.cotton) > 4)
                {
                    options.Add(good);
                }
            }
        }
        return options;

    }

    public bool tryProvinceDevelopment(Nation player)
    {
        List<int> devOptions = getProviceDevelopmentOptions(player);
       // List<int> infOptions = getProviceInfrastructureOptions(player);
        if (devOptions.Count == 0)
        {
            Debug.Log("No provices can be developed at this time");
            return false;
        }
        else
        {
            if(tryProvinceDevelopment(player, devOptions))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }


    public void tryToBuildUnit(Nation player)
    {
        bool ableToBuildUnit = PlayerCalculator.canExpandArmy(player);
        if (ableToBuildUnit)
        {
            //want unit
            PlayerPayer.PayExpandArmy(player);
            PlayerReceiver.receiveArmyUnit(player);
        }
        else
        {
            if (player.getNumberGood(MyEnum.Goods.arms) < 3)
            {
                tryToObtainArms(player);
            }
            if (player.getNumberGood(MyEnum.Goods.arms) < 2)
            {
                tryToObtainArms(player);
            }
        }
    }

    public void tryToAddDoctrine(Nation player)
    {
        bool ableToAddDoctrine = PlayerCalculator.canMakeDevelopmentAction(player);
        if (ableToAddDoctrine)
        {
            MyEnum.ArmyDoctrines doctrineChoice = AI_Helper.chooseDoctrine(player);
            Debug.Log("Randomly Chosen Doctrine: " + doctrineChoice);
            PlayerPayer.payForDevelopmentAction(player, 1);
            PlayerReceiver.receiveDoctrine(player, doctrineChoice);
        }
    }


    public void tryToBeAbleToMakeProgressActions(Nation player)
    {
        
        if(player.getNumberResource(MyEnum.Resources.spice) < 1)
        {
            tryBuyResource(player, MyEnum.Resources.spice);
        }
        if(player.getNumberGood(MyEnum.Goods.furniture) < 1)
        {
            tryToObtainFurniture(player);
        }
        if(player.getNumberGood(MyEnum.Goods.paper) < 1)
        {
            tryToObtainPaper(player);
        }
    }

    public bool tryBuildNavy(Nation player)
    {
        
        MyEnum.Era era = State.era;
        bool able = PlayerCalculator.canBuildNavy(player);
        TopLevel topLevel = player.getAI().getTopLevel();
        if (able == true)
        {
            if (player.GetTechnologies().Contains("oil_powered_ships"))
            {
                PlayerPayer.PayDreadnought(player);
                return able;
            }

            else if (player.GetTechnologies().Contains("ironclad"))
            {
                PlayerPayer.PayIronClad(player); 
                return able;
            }
            else
            {
                PlayerPayer.PayFrigate(player);
                Debug.Log("Building Frigate!!!");
                return able;
            }
        }
       
        else if (!player.GetTechnologies().Contains("ironclad"))
            {
                if (player.getNumberGood(MyEnum.Goods.lumber) < 1)
                {
                    tryToObtainLumber(player);
                }
                if(player.getNumberGood(MyEnum.Goods.fabric) < 1)
                {
                    tryToObtainFabric(player);
                }
                if(player.getNumberGood(MyEnum.Goods.arms) < 1)
                {
                    tryToObtainArms(player);
                }
            }
        else if (!player.GetTechnologies().Contains("oil_powered_ships"))
        {
            if(player.getNumberGood(MyEnum.Goods.steel) < 1)
            {
                tryToObtainSteel(player);
            }
            if(player.getNumberGood(MyEnum.Goods.arms) < 1)
            {
                tryToObtainArms(player);
            }
            if(player.getNumberGood(MyEnum.Goods.parts) < 1)
            {
                tryToObtainParts(player);
            }
        }

        else
        {
            if(player.getNumberGood(MyEnum.Goods.gear) < 1)
            {
                tryToObtainGear(player);
            }
            if(player.getNumberGood(MyEnum.Goods.parts) < 1)
            {
                tryToObtainParts(player);
            }
            if(player.getNumberGood(MyEnum.Goods.steel) < 2)
            {
                tryToObtainSteel(player);
            }
            if (player.getNumberGood(MyEnum.Goods.steel) < 1)
            {
                tryToObtainSteel(player);
            }
            if (player.getNumberGood(MyEnum.Goods.arms) < 2)
            {
                tryToObtainArms(player);
            }
            if (player.getNumberGood(MyEnum.Goods.arms) < 1)
            {
                tryToObtainArms(player);
            }
        }
        
        return able;
    }

    public void tryToBuildShipNextTurn(Nation player)
    {
        if (player.GetTechnologies().Contains("oil_powered_ships"))
        {
            if (player.GetShipyardLevel() < 3)
            {
                tryBuildShipyard(player);
            }
            float numberOfArms = player.getNumberGood(MyEnum.Goods.arms);
            // while (player.getNumberGood(MyEnum.Goods.arms) < 6)
            if (numberOfArms < 6)
            {
                for (int i = 0; i < (6 - numberOfArms); i++)
                {
                    tryToObtainArms(player);
                }
            }
            if (player.getNumberGood(MyEnum.Goods.steel) < 6)
            {
                for (int i = 0; i < (6 - player.getNumberGood(MyEnum.Goods.steel)); i++)
                {
                    tryToObtainSteel(player);
                }
            }
            if (player.getNumberGood(MyEnum.Goods.parts) < 1)
            {
                tryToObtainParts(player);
            }
            if (player.getNumberGood(MyEnum.Goods.gear) < 1)
            {
                tryToObtainGear(player);
            }
        }

        else if (player.GetTechnologies().Contains("ironclad"))
        {
            if (player.GetShipyardLevel() < 2)
            {
                tryBuildShipyard(player);
            }
            else
            {
                if (player.getNumberGood(MyEnum.Goods.arms) < 2)
                {
                    tryToObtainArms(player);
                }
                if (player.getNumberGood(MyEnum.Goods.steel) < 1)
                {
                    tryToObtainSteel(player);
                }
                if (player.getNumberGood(MyEnum.Goods.parts) < 1)
                {
                    tryToObtainParts(player);
                }
            }
        }
        else
        {
            if (player.GetShipyardLevel() < 1)
            {
                tryBuildShipyard(player);
            }
            else
            {
                if (player.getNumberGood(MyEnum.Goods.arms) < 2)
                {
                    tryToObtainArms(player);
                }
                if (player.getNumberGood(MyEnum.Goods.fabric) < 1)
                {
                    tryToObtainFabric(player);
                }
                if (player.getNumberGood(MyEnum.Goods.lumber) < 1)
                {
                    tryToObtainLumber(player);
                }
            }
        }
    }

    
        /*else
        {
            if (player.getNumberGood(MyEnum.Goods.clothing) < 1)
            {
                tryToObtainClothing(player);
            }
        }
        MyEnum.Era era = State.era;
        if (era != MyEnum.Era.Late && player.getNumberResource(MyEnum.Resources.spice) < 1)
        {
            tryBuyResource(player, MyEnum.Resources.spice);
        }
        if (era != MyEnum.Era.Early)
        {
            tryToObtainClothing(player);
        }
        if (era == MyEnum.Era.Late)
        {
            if (player.getNumberGood(MyEnum.Goods.telephone) < 1)
            {
                tryToObtainTelephones(player);

            }
            if (player.getNumberGood(MyEnum.Goods.telephone) < 1)
            {
                tryToObtainAuto(player);
            }
        } */

    public List<MyEnum.Goods> getDesiredFactoryList(Nation player)
    {
        List<MyEnum.Goods> factoryOptions = getPotentialFactoryUpgradeOptions(player);
        List<MyEnum.Goods> desiredOptions = new List<MyEnum.Goods>();
        for (int i = 0; i < factoryOptions.Count; i++)
        {
            if (checkIfWantFactory(player, factoryOptions[i]))
            {


                desiredOptions.Add(factoryOptions[i]);
            }
        }
        return desiredOptions;
    }

    public bool checkForPossibleInvesmentOptions(Nation player)
    {
        List<MyEnum.Goods> factoryOptions = getPotentialFactoryUpgradeOptions(player);
        for (int i = 0; i < factoryOptions.Count; i++)
        {
            if (checkIfWantFactory(player, factoryOptions[i]))
            {
                return true;
            }
        }
        List<int> devOptions = getProviceDevelopmentOptions(player);
        if(devOptions.Count > 0)
        {
            return true;
        }
        List<int> railOptions = getProviceRailRoadOptions(player);
        if(railOptions.Count > 0)
        {
            return true;
        }
        return false;

    }

    public bool checkIfWantFactory(Nation player, MyEnum.Goods good)
    {
        if(good == MyEnum.Goods.steel)
        {
            if (PlayerCalculator.getResourceProducing(player, MyEnum.Resources.iron) < 1 || PlayerCalculator.getResourceProducing(player, MyEnum.Resources.coal) < 1)
            {
                return false;
            }
            if(player.getFactoryLevel(MyEnum.Goods.steel) == 1)
            {
                if (PlayerCalculator.getResourceProducing(player, MyEnum.Resources.iron) < 3 || PlayerCalculator.getResourceProducing(player, MyEnum.Resources.coal) < 2)
                {
                    return false;
                }
            }
            return true;
        }
        if(good == MyEnum.Goods.lumber)
        {
            if(PlayerCalculator.getResourceProducing(player, MyEnum.Resources.wood) < 1)
            {
                return false;
            }
            if(player.getFactoryLevel(MyEnum.Goods.lumber) == 1)
            {
                if(PlayerCalculator.getResourceProducing(player, MyEnum.Resources.wood) < 3)
                {
                    return false;
                }
            }
            return true;
        }
        if(good == MyEnum.Goods.fabric)
        {
            if(player.getFactoryLevel(MyEnum.Goods.fabric) == 1)
            {
                return true;
            }
            if(player.getFactoryLevel(MyEnum.Goods.fabric) == 1)
            {
                if(PlayerCalculator.getResourceProducing(player, MyEnum.Resources.cotton) < 3 || PlayerCalculator.getResourceProducing(player, MyEnum.Resources.dyes) < 1)
                {
                    return false;
                }
            }
        }
        if(good == MyEnum.Goods.arms)
        {
            if(player.getFactoryLevel(MyEnum.Goods.arms) == 0)
            {
                return true;
            }
            if (player.getFactoryLevel(MyEnum.Goods.arms) == 1)
            {
                if (PlayerCalculator.getResourceProducing(player, MyEnum.Resources.iron) < 3 || PlayerCalculator.getResourceProducing(player, MyEnum.Resources.coal) < 3)
                {
                    return false;
                }
            }
        }
        if(good == MyEnum.Goods.chemicals)
        {
            if(PlayerCalculator.getResourceProducing(player, MyEnum.Resources.coal) < 2)
            {
                return false;
            }
            if(player.getFactoryLevel(MyEnum.Goods.chemicals) == 1)
            {
                if(PlayerCalculator.getResourceProducing(player, MyEnum.Resources.coal) < 4)
                {
                    return false;
                }
            }
        }
        if(good == MyEnum.Goods.clothing)
        {
            if (player.getFactoryLevel(MyEnum.Goods.clothing) == 0)
            {
                return true;
            }
            if (player.getFactoryLevel(MyEnum.Goods.clothing) == 1)
            {
                if (PlayerCalculator.getResourceProducing(player, MyEnum.Resources.cotton) < 3 || PlayerCalculator.getResourceProducing(player, MyEnum.Resources.dyes) < 1)
                {
                    return false;
                }
            }
        }
        if(good == MyEnum.Goods.furniture)
        {
            if(player.getFactoryLevel(MyEnum.Goods.furniture) == 0)
            {
                return true;
            }
            if (player.getFactoryLevel(MyEnum.Goods.furniture) == 1)
            {
                if (PlayerCalculator.getResourceProducing(player, MyEnum.Resources.wood) < 3 || PlayerCalculator.getResourceProducing(player, MyEnum.Resources.cotton) < 1)
                {
                    return false;
                }
            }
        }
        if(good == MyEnum.Goods.gear)
        {
            if(player.getFactoryLevel(MyEnum.Goods.gear) == 0)
            {
                return true;
            }
            if(player.getFactoryLevel(MyEnum.Goods.gear) == 1)
            {
                if(PlayerCalculator.getResourceProducing(player, MyEnum.Resources.rubber) < 2)
                {
                    return false;
                }
            }
        }
        if(good == MyEnum.Goods.paper)
        {
            if(player.getFactoryLevel(MyEnum.Goods.paper) == 0)
            {
                return true;
            }
            if (player.getFactoryLevel(MyEnum.Goods.paper) == 1)
            {
                if (PlayerCalculator.getResourceProducing(player, MyEnum.Resources.wood) < 2)
                {
                    return false;
                }
            }
        }
        if (good == MyEnum.Goods.parts)
        {
            if (player.getFactoryLevel(MyEnum.Goods.parts) == 0)
            {
                return true;
            }
            if (player.getFactoryLevel(MyEnum.Goods.parts) == 1)
            {
                if (PlayerCalculator.getResourceProducing(player, MyEnum.Resources.iron) < 3 || PlayerCalculator.getResourceProducing(player, MyEnum.Resources.coal) < 3)
                {
                    return false;
                }
            }
        }
        if(good == MyEnum.Goods.telephone)
        {
            if(player.getFactoryLevel(MyEnum.Goods.telephone) == 0)
            {
                return true;
            }
            if(player.getFactoryLevel(MyEnum.Goods.telephone) == 1)
            {
                if (PlayerCalculator.getResourceProducing(player, MyEnum.Resources.rubber) < 2 || PlayerCalculator.getResourceProducing(player, MyEnum.Resources.wood) < 1)
                {
                    return false;
                }
            }
        }
   
        return true;
    }

    public void productionForExport(Nation player)
    {
        Debug.Log("Producing for Exports ;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;");
        TopLevel topLevel = player.getAI().getTopLevel();
        List<MyEnum.Goods> goods = topLevel.getProductionPriorities(player);
        //int count = 0;
        // while(player.getAP() >= 2 && count < 10)
        Market market = State.market;


        for (int i = 0; i < goods.Count; i++)
        {
            if (player.getAP() < 2)
            {
                break;
            }
            MyEnum.Goods good = goods[i];

            int overstock = market.getDifferenceBetweenSupplyAndDemandGood(good);
            if(overstock > 8)
            {
                continue;
            }


            if (good == MyEnum.Goods.paper || good == MyEnum.Goods.furniture)
            {
                if (!market.goodBeingOffered(MyEnum.Goods.lumber))
                {
                    continue;
                }
            }

            if(good == MyEnum.Goods.parts || good == MyEnum.Goods.arms)
            {
                if (!market.goodBeingOffered(MyEnum.Goods.steel))
                {
                    continue;
                }
            }

            if(good == MyEnum.Goods.clothing)
            {
                if (!market.goodBeingOffered(MyEnum.Goods.fabric))
                {
                    int randInt = UnityEngine.Random.Range(1, 10);
                    if (randInt <= 6)
                    {
                        continue;
                    }
                }
            }

            int level = player.getFactoryLevel(good);
            if (level < 1)
            {
                continue;
            }
            if (player.industry.getGoodProducing(good) > 0)
            {
                continue;
            }
            float currentSupply = player.getNumberGood(good);
            Debug.Log("Current Supply of " + good + " " + currentSupply + "//////////////////////");
            if (currentSupply > 7 && State.turn < 20)
            {
                continue;
            }
            if (currentSupply > 9 && State.turn < 30)
            {
                continue;
            }
            if(currentSupply > 14 && State.turn < 45)
            {
                continue;
            }
            if(currentSupply > 20 && State.turn < 60)
            {
                continue;
            }
            if(currentSupply > 25 && State.turn < 75)
            {
                continue;
            }
            if(currentSupply > 30)
            {
                continue;
            }
            float canProduce = player.industry.determineCanProduce(good, player);
            if(canProduce < 2 + level)
            {
                tryToBuyMaterials(player, good);
                continue;
            }
            else
            {
                PlayerPayer.payForFactoryProduction(player, good, (int)canProduce);
            }  
        }
    }


    public void tryToBuyMaterials(Nation player, MyEnum.Goods good)
    {
        if (player.getGold() <= player.getTotalCurrentBiddingCost())
        {
            return;
        }
        Dictionary<string, float> costs = ProductionCosts.GetCosts(good);
        foreach (string entry in costs.Keys)
        {
            if (Enum.IsDefined(typeof(MyEnum.Goods), entry))
            {
                MyEnum.Goods itemType = (MyEnum.Goods)System.
                    Enum.Parse(typeof(MyEnum.Goods), entry);
                int factLevel = player.getFactoryLevel(good);
                for(int i = 0; i < factLevel * 2 - player.getNumberGood(itemType); i++)
                {
                    if (itemType == MyEnum.Goods.arms)
                    {
                        tryToObtainArms(player);
                    }
                    else if (itemType == MyEnum.Goods.auto)
                    {
                        tryToObtainAuto(player);
                    }
                    else if (itemType == MyEnum.Goods.chemicals)
                    {
                        tryToObtainChemicals(player);
                    }
                    else if (itemType == MyEnum.Goods.clothing)
                    {
                        tryToObtainClothing(player);
                    }
                    else if (itemType == MyEnum.Goods.fabric)
                    {
                        tryToObtainFabric(player);
                    }
                
                    else if (itemType == MyEnum.Goods.furniture)
                    {
                        tryToObtainFurniture(player);
                    }
                    else if (itemType == MyEnum.Goods.arms)
                    {
                        tryToObtainArms(player);
                    }
                    else if (itemType == MyEnum.Goods.gear)
                    {
                        tryToObtainGear(player);
                    }
                    else if (itemType == MyEnum.Goods.lumber)
                    {
                        tryToObtainLumber(player);
                    }
                    else if (itemType == MyEnum.Goods.paper)
                    {
                        tryToObtainPaper(player);
                    }
                    else if (itemType == MyEnum.Goods.parts)
                    {
                        tryToObtainParts(player);
                    }
                    else if (itemType == MyEnum.Goods.steel)
                    {
                        tryToObtainSteel(player);
                    }
                    else if (itemType == MyEnum.Goods.telephone)
                    {
                        tryToObtainTelephones(player);
                    }
                    //  tryToObtainGood(player, itemType);
                    // tryBuyGood(player, itemType);
                    if (State.currentPlayerIsMajor)
                    {
                        Debug.Log("Buying " + itemType + " to build " + good);
                    }
                }
            }
            else
            {
                MyEnum.Resources itemType = (MyEnum.Resources)System.
                   Enum.Parse(typeof(MyEnum.Resources), entry);
                int factLevel = player.getFactoryLevel(good);
                for (int i = 0; i < factLevel * 2 - player.getNumberResource(itemType); i++)
                {
                    tryBuyResource(player, itemType);
                    if (State.currentPlayerIsMajor)
                    {
                        Debug.Log("Buying " + itemType + " to build " + good);
                    }
                }

            }
        }
    }

    public void tryToGainTechnology(Nation player)
    {
        List<Technology> options = getTechOptions(player);
        Debug.Log("Number of tech options:  " + options.Count);
        if (options.Count > 0)
        {
            Technology choice = chooseMostPrefferedTech(player, options);
            PlayerPayer.PayForTechnology(player, choice.GetTechName());
            PlayerReceiver.addNewTech(player, choice);
            PlayerReceiver.registerTechChanges(choice, player);
            //adjust priorities _____________________________________________________//

            Debug.Log("Has researched " + choice.GetTechName() + " !!!");
        }
    }

    public List<Technology> getTechOptions(Nation player)
    {
        List<Technology> options = new List<Technology>();
        foreach (KeyValuePair<string, Technology> item in State.GetTechnologies())
        {
            if (PlayerCalculator.hasTechPreRequisites(player, item.Key) && PlayerCalculator.canAffordTech(player, item.Key))
            {
                options.Add(item.Value);
            }
        }
        return options;
    }


    public Technology chooseMostPrefferedTech(Nation player, List<Technology> options)
    {
        float desireability = -100;
        Technology choice = null;
        foreach (Technology tech in options)
        {
            float temp = player.getAI().GetTopLevel().getPriorityLevelOfTech(player, tech.GetTechName());
            if (temp > desireability)
            {
                if (PlayerCalculator.hasTechPreRequisites(player, tech.GetTechName()) && PlayerCalculator.canAffordTech(player, tech.GetTechName()))
                {

                    desireability = temp;
                    choice = tech;
                }
            }
        }
        return choice;
    }

    //Shopping
    public void bidOnGood(Nation player, MyEnum.Goods good)
    {
        Debug.Log("Makes a bid for: " + good + "___________________");
        MyEnum.OffBidLevels level = checkGoodSupplyVsDemand(good);
        Signal newSignal =
            new Signal(MyEnum.marketChoice.bid, player.getIndex(), MyEnum.Resources.wheat, good, 1, false, false);
        State.market.addGoodBid(good, newSignal);
        updateTotalBidCostGood(good, player, level);
    }

    public void bidOnResource(Nation player, MyEnum.Resources res)
    {

        Debug.Log("Makes a bid for: " + res + "_____________________");

        MyEnum.OffBidLevels level = checkResourceSupplyVsDemand(res);
        Signal newSignal =
            new Signal(MyEnum.marketChoice.bid, player.getIndex(), res, MyEnum.Goods.parts, 1, true, false);
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
            if (player.industry.CheckIfCanUpgradeFactory(player, good))
            {
                options.Add(good);
            }
        }
        return options;
    }

    public List<MyEnum.Goods> getPotentialFactoryUpgradeOptions(Nation player)
    {
        List<MyEnum.Goods> options = new List<MyEnum.Goods>();

        foreach (MyEnum.Goods good in Enum.GetValues(typeof(MyEnum.Goods)))
        {
            if (player.industry.CheckIfFactoryCanBeUpgraded(player, good))
            {
                options.Add(good);
            }
        }
        return options;
    }

    public List<int> getProviceDevelopmentOptions(Nation player)
    {
        List<int> options = new List<int>();
        HashSet<int> allProvinces = PlayerCalculator.getAllProvinces(player);
        foreach (int provIndex in allProvinces)
        {
            assemblyCsharp.Province prov = State.getProvinces()[provIndex];
            if (PlayerCalculator.checkPotentialUpgradeDevelopment(prov, player))
            {
                options.Add(provIndex);
            }
        }
        return options;
    }

    public bool tryToBuildRailRoad(Nation player)
    {
        List<int> options = getProviceRailRoadOptions(player);
        if(options.Count == 0)
        {
            return false;
        }
        else
        {
            int intResOrderCounter = 0;
            while (intResOrderCounter < 5)
            {
                MyEnum.Resources desiredRes = player.getAI().getTopLevel().getTopResourcePriorityN(intResOrderCounter);

                for (int i = 0; i < options.Count; i++)
                {
                    assemblyCsharp.Province prov = State.getProvinces()[options[i]];
                    MyEnum.Resources res = prov.getResource();
                    if (res == desiredRes)
                    {
                        PlayerPayer.payForRailroad(player);
                        PlayerReceiver.addRailroad(player, prov);
                        TopLevel topLevel = player.getAI().getTopLevel();
                        topLevel.metaPriorities[MyEnum.metaPriorities.development]--;
                        topLevel.developmentPriorities[MyEnum.developmentPriorities.railroad]--;

                        //Player takes care of increase;
                        return true;
                    }
                }
                intResOrderCounter++;

            }
            return false;
        }

    }

    public List<int> getProviceRailRoadOptions(Nation player)
    {
        List<int> options = new List<int>();
        if (!PlayerCalculator.canBuildRaiload(player))
        {
            return options;
        }
        foreach(int provIndex in player.getProvinces())
        {
            assemblyCsharp.Province prov = State.getProvinces()[provIndex];
            if (!prov.getRail())
            {
                options.Add(prov.getIndex());
            }
        }
        foreach(int colIndex in player.getColonies())
        {
            assemblyCsharp.Province prov = State.getProvinces()[colIndex];
            if (!prov.getRail())
            {
                options.Add(prov.getIndex());
            }

        }
        return options;
    }

    public bool canAffordRes(Nation player, MyEnum.Resources res)
    {
        bool afford = PlayerCalculator.checkIfCanAffordResource(player, res);
        return afford;
    }

    public void tryBuyResource(Nation player, MyEnum.Resources res)
    {
        bool afford = PlayerCalculator.checkIfCanAffordResource(player, res);
        TopLevel topLevel = player.getAI().GetTopLevel();
        topLevel.alterResPriority(player, res, 0.066f);
        if (afford == true)
        {
            bidOnResource(player, res);
        }
    }

    public bool canAffordGood(Nation player, MyEnum.Goods good)
    {
        bool afford = PlayerCalculator.checkIfCanAffordGood(player, good);
        return afford;
    }

    public void tryBuyGood(Nation player, MyEnum.Goods good)
    {
        bool afford = PlayerCalculator.checkIfCanAffordGood(player, good);
        if (afford == true)
        {
            bidOnGood(player, good);
        }
    }


    public float amountNeeded(int target, float current)
    {
        return target - (int)current;
    }

    public void tryToObtainFabric(Nation player)
    {
        if(player.getNumberResource(MyEnum.Resources.cotton) < 2)
        {
            tryBuyResource(player, MyEnum.Resources.cotton);
        }
        Market market = State.market;
        if (State.currentPlayerIsMajor)
        {
            Debug.Log("Try to obtain Fabric");
        }
        int factLevel = player.getFactoryLevel(MyEnum.Goods.fabric);
        if (factLevel == 0 && market.goodBeingOffered(MyEnum.Goods.fabric))
        {
            if (PlayerCalculator.checkIfCanAffordGood(player, MyEnum.Goods.fabric))
            {
                tryBuyGood(player, MyEnum.Goods.fabric);
               // return;
            }
           
        }
  
        float canProduceFab = player.industry.determineCanProduce(MyEnum.Goods.fabric, player);
        bool decision = decideIfProduceGood(player, MyEnum.Goods.fabric, canProduceFab);
        if (decision == true)
        {
            return;
        }
        else
        {
            int target = 1 + (player.getFactoryLevel(MyEnum.Goods.fabric));
            int counter = 0;
           // tryBuyResource(player, MyEnum.Resources.cotton);

            while (counter < amountNeeded(target, player.getNumberResource(MyEnum.Resources.cotton)))
            {

                if (State.currentPlayerIsMajor)
                {
                    Debug.Log("Should try to buy cotton");
                }
                tryBuyResource(player, MyEnum.Resources.cotton);
                counter++;

            }
  

            if (market.getEasyToBuyGood(MyEnum.Goods.fabric))
            {
                tryBuyGood(player, MyEnum.Goods.fabric);
            }
        }
    }

    public void tryToObtainSteel(Nation player)
    {
        Market market = State.market;
        if (State.currentPlayerIsMajor)
        {
            Debug.Log("Try to obtain Steel");
        }
        int factLevel = player.getFactoryLevel(MyEnum.Goods.steel);

        if (factLevel == 0 && market.goodBeingOffered(MyEnum.Goods.steel))
        {
            if (PlayerCalculator.checkIfCanAffordGood(player, MyEnum.Goods.steel))
            {
                tryBuyGood(player, MyEnum.Goods.steel);
                return;
            }
        }
    
        int target = 1 + (factLevel * 2);
        float canProduce = player.industry.determineCanProduce(MyEnum.Goods.steel, player);
        bool decision = decideIfProduceGood(player, MyEnum.Goods.steel, canProduce);
        if (decision)
        {
            return;
        }
        else
        {
            int counter = 0;
     

            while (counter < amountNeeded(target, player.getNumberResource(MyEnum.Resources.iron)) * 0.7f)
            {
                if (State.currentPlayerIsMajor)
                {
                    Debug.Log("Should try to buy iron");
                }
                tryBuyResource(player, MyEnum.Resources.iron);
                counter++;
            }
            counter = 0;
            while (counter < amountNeeded(target, player.getNumberResource(MyEnum.Resources.coal)) * 0.35f)
            {
                if (State.currentPlayerIsMajor)
                {
                    Debug.Log("Should try to buy coal");
                }
                tryBuyResource(player, MyEnum.Resources.coal);
                counter++;
            }
        }
        if (market.getEasyToBuyGood(MyEnum.Goods.steel))
        {
            tryBuyGood(player, MyEnum.Goods.steel);
        }
    }

    public void tryToObtainLumber(Nation player)
    {
        Market market = State.market;
        int factLevel = player.getFactoryLevel(MyEnum.Goods.lumber);

        if (State.currentPlayerIsMajor)
        {
            Debug.Log("Try to obtain Lumber");
        }
        if (factLevel == 0  && market.goodBeingOffered(MyEnum.Goods.lumber))
        {
            if (PlayerCalculator.checkIfCanAffordGood(player, MyEnum.Goods.lumber))
            {
                tryBuyGood(player, MyEnum.Goods.lumber);
                return;
            }
        }
        int target = 1 + (factLevel * 2);
        float canProduce = player.industry.determineCanProduce(MyEnum.Goods.lumber, player);
        bool decision = decideIfProduceGood(player, MyEnum.Goods.lumber, canProduce);
        if (decision)
        {
            return;
        }
        else
        {
            int counter = 0;
            tryBuyResource(player, MyEnum.Resources.wood);
            while (counter < amountNeeded(target, player.getNumberResource(MyEnum.Resources.wood)))
            {
                if (State.currentPlayerIsMajor)
                {
                    Debug.Log("Should try to buy wood");
                }
                tryBuyResource(player, MyEnum.Resources.wood);
                counter++;
            }
        }
        if (market.getEasyToBuyGood(MyEnum.Goods.lumber))
        {
            tryBuyGood(player, MyEnum.Goods.lumber);
        }
    }

    public void tryToObtainClothing(Nation player)
    {
        Market market = State.market;
        if (player.getType() == MyEnum.NationType.major)
        {
            Debug.Log("Try to obtain Clothing");
        }
        int factLevel = player.getFactoryLevel(MyEnum.Goods.clothing);

        if (factLevel == 0 && market.goodBeingOffered(MyEnum.Goods.clothing))
        {
            if (PlayerCalculator.checkIfCanAffordGood(player, MyEnum.Goods.clothing))
            {
                Debug.Log("Try Buy");
                tryBuyGood(player, MyEnum.Goods.clothing);
               // return;
            }
        }
        int target = 1 + (factLevel * 2);
        // Market market = State.market;

        float canProduce = player.industry.determineCanProduce(MyEnum.Goods.clothing, player);
        bool decision = decideIfProduceGood(player, MyEnum.Goods.clothing, canProduce);
        if (decision)
        {
            return;
        }
        else
        {
            int counter = 0;
            while (counter < amountNeeded(target, player.getNumberGood(MyEnum.Goods.fabric)))
            {
                if (State.currentPlayerIsMajor)
                {
                    Debug.Log("Should try to obtain fabric");
                }
                tryToObtainFabric(player);
                counter++;
            }
            if (player.getNumberResource(MyEnum.Resources.dyes) < target * 0.5f)
            {
                counter = 0;
                //     tryBuyResource(player, MyEnum.Resources.dyes);
                float dyesPlusChem = player.getNumberResource(MyEnum.Resources.dyes) + player.getNumberGood(MyEnum.Goods.chemicals);
                while (counter < amountNeeded(target, dyesPlusChem) * 0.33f)
                {
                    if (State.currentPlayerIsMajor)
                    {
                        Debug.Log("Should try to buy Dyes");
                    }
                    if (market.goodBeingOffered(MyEnum.Goods.chemicals))
                    {
                        tryBuyGood(player, MyEnum.Goods.chemicals);
                    }
                    tryBuyResource(player, MyEnum.Resources.dyes);
                    counter++;
                }
            }
        }
        if (market.goodBeingOffered(MyEnum.Goods.clothing))
        {
            tryBuyGood(player, MyEnum.Goods.clothing);
        }
    }

    public void tryToObtainParts(Nation player)
    {
        Market market = State.market;
        if (State.currentPlayerIsMajor)
        {
            Debug.Log("Try to obtain Parts");
        }
        int factLevel = player.getFactoryLevel(MyEnum.Goods.parts);

        if (factLevel == 0 && market.goodBeingOffered(MyEnum.Goods.parts))
        {
            if (PlayerCalculator.checkIfCanAffordGood(player, MyEnum.Goods.parts))
            {
                tryBuyGood(player, MyEnum.Goods.parts);
                return;
            }
        }
        int target = 1 + (factLevel * 2);
        float canProduce = player.industry.determineCanProduce
            (MyEnum.Goods.parts, player);
        bool decision = decideIfProduceGood(player, MyEnum.Goods.parts, canProduce);
        if (decision)
        {
            return;
        }
        else
        {
            int counter = 0;
            while (counter < amountNeeded(target, player.getNumberGood(MyEnum.Goods.steel)))
            {
                if (State.currentPlayerIsMajor)
                {
                    Debug.Log("Should try to buy steel");
                }
                tryToObtainSteel(player);
                counter++;
            }
        }
        if (market.goodBeingOffered(MyEnum.Goods.parts))
        {
            tryBuyGood(player, MyEnum.Goods.parts);
        }
    }

    public void tryToObtainArms(Nation player)
    {
        Market market = State.market;
        if (State.currentPlayerIsMajor)
        {
            Debug.Log("Try to obtain Arms");
        }
        int factLevel = player.getFactoryLevel(MyEnum.Goods.arms);

        if (factLevel == 0 && market.goodBeingOffered(MyEnum.Goods.arms))
        {
            if (PlayerCalculator.checkIfCanAffordGood(player, MyEnum.Goods.arms))
            {
                tryBuyGood(player, MyEnum.Goods.arms);
                return;
            }
        }
 
        int target = 1 + (factLevel * 2);
        float canProduce = player.industry.determineCanProduce(MyEnum.Goods.arms, player);
        bool decision = decideIfProduceGood(player, MyEnum.Goods.arms, canProduce);
        if (decision)
        {
            return;
        }
        else
        {
            int counter = 0;
            while (counter < amountNeeded(target, player.getNumberGood(MyEnum.Goods.steel)))
            {
                if (State.currentPlayerIsMajor)
                {
                    Debug.Log("Should try to buy steel");
                }
                tryToObtainSteel(player);
                counter++;
            }
        }
        if (market.goodBeingOffered(MyEnum.Goods.arms))
        {
            tryBuyGood(player, MyEnum.Goods.arms);
        }
    }

    public void tryToObtainPaper(Nation player)
    {
        Market market = State.market;
        if (State.currentPlayerIsMajor)
        {
            Debug.Log("Try to obtain Paper");
        }
        int factLevel = player.getFactoryLevel(MyEnum.Goods.paper);

        if (factLevel == 0 && market.goodBeingOffered(MyEnum.Goods.paper))
        {
            if (PlayerCalculator.checkIfCanAffordGood(player, MyEnum.Goods.paper))
            {
                tryBuyGood(player, MyEnum.Goods.paper);
                return;
            }
        }
        int target = 1 + (factLevel * 2);
        float canProduceFab = player.industry.determineCanProduce(MyEnum.Goods.paper, player);
        bool decision = decideIfProduceGood(player, MyEnum.Goods.paper, canProduceFab);
        if (decision)
        {
            return;
        }
        else
        {
            int counter = 0;
            while (counter < amountNeeded(target, player.getNumberResource(MyEnum.Resources.wood)))
            {
                if (State.currentPlayerIsMajor)
                {
                    Debug.Log("Should try to buy wood");
                }
                tryToObtainLumber(player);
                counter++;
            }
        }
        if (market.goodBeingOffered(MyEnum.Goods.paper))
        {
            tryBuyGood(player, MyEnum.Goods.paper);
        }
    }

    public void tryToObtainFurniture(Nation player)
    {
        Market market = State.market;
        if (State.currentPlayerIsMajor)
        {
            Debug.Log("Try to obtain Furniture");
        }
        int factLevel = player.getFactoryLevel(MyEnum.Goods.furniture);

        if (factLevel == 0 && market.goodBeingOffered(MyEnum.Goods.furniture))
        {
            if (PlayerCalculator.checkIfCanAffordGood(player, MyEnum.Goods.furniture))
            {
                tryBuyGood(player, MyEnum.Goods.furniture);
                return;
            }
        }

        int target = 1;
        if(factLevel == 1)
        {
            target = 2;
        }
        if(factLevel == 2)
        {
            target = 5;
        }
        float canProduce = player.industry.determineCanProduce(MyEnum.Goods.furniture, player);
        bool decision = decideIfProduceGood(player, MyEnum.Goods.furniture, canProduce);
        if (decision)
        {
            return;
        }
        else
        {
            int counter = 0;
            while (counter < amountNeeded(target, player.getNumberGood(MyEnum.Goods.lumber)) * 0.75)
            {
                if (State.currentPlayerIsMajor)
                {
                    Debug.Log("Should try to obtain lumber");
                }
                tryToObtainLumber(player);
                counter++;
            }

            counter = 0;
            while (counter < amountNeeded(target, player.getNumberGood(MyEnum.Goods.fabric)) * 0.33)
            {
                if (State.currentPlayerIsMajor)
                {
                    Debug.Log("Should try to obtain fabric");
                }
                tryToObtainFabric(player);
                counter++;
            }

            if (market.goodBeingOffered(MyEnum.Goods.furniture))
            {
                tryBuyGood(player, MyEnum.Goods.furniture);
            }
        }
    }


    // void tryToObtainGood(Nation player, MyEnum.Goods good)
    // {

    //   Market market = State.market;
    //  bool afford = AdminConstraintChecker.checkIfCanAffordGood(player, good);
    // if (afford == true)
    //  {
    //     bidOnGood(player, good);
    // }
    // float canProduce = player.industry.determineCanProduce(good, player);
    //  if (canProduce >= 1)
    //  {
    //     decideIfProduceGood(player, good, canProduce);
    // }
    //  else
    // {
    //    tryToBuyMaterials(player, good);
    // }
    //}


    public void tryToObtainChemicals(Nation player)
    {
        int factLevel = player.getFactoryLevel(MyEnum.Goods.chemicals);
        Market market = State.market;


        if (factLevel == 0 && market.goodBeingOffered(MyEnum.Goods.chemicals))
        {
            tryBuyGood(player, MyEnum.Goods.chemicals);
            return;
        }
        int target = 1 + (factLevel * 2);
        float canProduceFab = player.industry.determineCanProduce(MyEnum.Goods.chemicals, player);
        if (canProduceFab >= 1)
        {
            decideIfProduceGood(player, MyEnum.Goods.chemicals, canProduceFab);
        }
        else
        {

            int counter = 0;
            while (counter < amountNeeded(target, player.getNumberResource(MyEnum.Resources.coal)))
            {
                if (State.currentPlayerIsMajor)
                {
                    Debug.Log("Should try to buy coal");
                }
                tryBuyResource(player, MyEnum.Resources.coal);
                counter++;
            }

            bool afford = PlayerCalculator.checkIfCanAffordGood(player, MyEnum.Goods.chemicals);
            if (afford == true && State.turn > 20)
            {
                bidOnGood(player, MyEnum.Goods.chemicals);
            }
        }
        if (market.getEasyToBuyGood(MyEnum.Goods.chemicals))
        {
            tryBuyGood(player, MyEnum.Goods.chemicals);
        }
    }

    public void tryToObtainGear(Nation player)
    {
        if (player.getType() == MyEnum.NationType.major)
        {
            Debug.Log("Try to obtain gear");
        }
        else
        {
            tryBuyGood(player, MyEnum.Goods.gear);
            return;
        }

        int factLevel = player.getFactoryLevel(MyEnum.Goods.gear);
        int target = 1 + (factLevel * 2);
        float canProduceFab = player.industry.determineCanProduce(MyEnum.Goods.gear, player);
        if (canProduceFab >= 1)
        {
            decideIfProduceGood(player, MyEnum.Goods.gear, canProduceFab);
        }
        else
        {
            tryBuyGood(player, MyEnum.Goods.gear);

            while (player.getNumberGood(MyEnum.Goods.gear) < target * 0.5f && canAffordGood(player, MyEnum.Goods.gear))
            {
                tryToObtainGear(player);  //
            }
            int counter = 0;
            while (counter + player.getNumberResource(MyEnum.Resources.wood) < target * 0.25f
                && canAffordRes(player, MyEnum.Resources.wood)) ;
            tryBuyResource(player, MyEnum.Resources.wood);
            counter++;

        }
    }

    public void tryToObtainAuto(Nation player)
    {
        if (State.currentPlayerIsMajor)
        {
            Debug.Log("Try to obtain Auto");
        }
        int factLevel = player.getFactoryLevel(MyEnum.Goods.auto);
        int target = 1 + (factLevel * 2);
        float canProduceFab = player.industry.determineCanProduce(MyEnum.Goods.auto, player);
        if (canProduceFab >= 1)
        {
            decideIfProduceGood(player, MyEnum.Goods.auto, canProduceFab);
        }
        else
        {
            tryBuyGood(player, MyEnum.Goods.auto);
            int counter = 0;
            while (counter + player.getNumberGood(MyEnum.Goods.gear) < target * 0.5f && canAffordGood(player, MyEnum.Goods.gear))
            {
                tryToObtainGear(player);  //
                counter++;
            }
            counter = 0;
            while (counter + player.getNumberGood(MyEnum.Goods.parts) < target * 0.5f && canAffordGood(player, MyEnum.Goods.parts))
            {
                tryToObtainGear(player);  //
                counter++;
            }
            counter = 0;
            while (counter + player.getNumberGood(MyEnum.Goods.steel) < target * 0.25f
                && canAffordGood(player, MyEnum.Goods.steel))
            {
                tryToObtainSteel(player);  //
                counter++;
            }
            counter = 0;
            while (counter + player.getNumberResource(MyEnum.Resources.rubber) < target * 0.25f
               && canAffordRes(player, MyEnum.Resources.rubber)) ;
            {
                tryToObtainSteel(player);  //
                counter++;
            }

        }
    }

    public void tryToObtainTelephones(Nation player)
    {
        Debug.Log("Try to obtain Telephone");

        int factLevel = player.getFactoryLevel(MyEnum.Goods.telephone);
        int target = 1 + (factLevel * 2);
        float canProduceFab = player.industry.determineCanProduce(MyEnum.Goods.telephone, player);

        if (canProduceFab >= 1)
        {
            decideIfProduceGood(player, MyEnum.Goods.telephone, canProduceFab);
        }
        else
        {
            tryBuyGood(player, MyEnum.Goods.telephone);
            int counter = 0;
            while (counter + player.getNumberGood(MyEnum.Goods.gear) < target * 0.75f && canAffordGood(player, MyEnum.Goods.steel))
            {
                tryToObtainSteel(player);  //
                counter++;
            }
            counter = 0;
            while (player.getNumberResource(MyEnum.Resources.rubber) < target * 0.5f
                && canAffordRes(player, MyEnum.Resources.rubber)) ;
            tryBuyResource(player, MyEnum.Resources.rubber);
            counter++;
        }
    }

    public bool decideIfProduceGood(Nation player, MyEnum.Goods good, float canProduce)
    {
        //Later add more considerations - in particular consider the value of the material relative to the value of the product.
        float currentNumberOfGood = player.getNumberGood(good);
        if(currentNumberOfGood > 14)
        {
            return false;
        }
        if (player.getAP() < 1 || canProduce < 1)
        {
            return false;
        }
        int factoryLevel = player.getFactoryLevel(good);
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
                float amount = Math.Min(canProduce, 9.0f);
                PlayerPayer.payForFactoryProduction(player, good, (int)amount);

            }
        }
        return true;
    }

    public void tryToObtainMaterialForFactory(Nation player, MyEnum.Goods good)
    {
        //  AdminAI admin = player.getAI().GetAdmin();
        float potential = getProcessingCapacity(player, good);
        if (good == MyEnum.Goods.steel)
        {
            int ironWanted = (int)(potential * 0.66f);
            int coalWanted = (int)(potential * 0.33f);
            if (player.getNumberResource(MyEnum.Resources.iron) < ironWanted)
            {
                int ironToBuy = (int)(ironWanted - player.getNumberResource(MyEnum.Resources.iron)) + 1;
                for (int i = 0; i < ironToBuy; i++)
                {
                    tryBuyResource(player, MyEnum.Resources.iron);
                }
            }
            if (player.getNumberResource(MyEnum.Resources.coal) < coalWanted)
            {
                int coalToBuy = (int)(coalWanted - player.getNumberResource(MyEnum.Resources.coal)) + 1;
                for (int i = 0; i < coalWanted; i++)
                {
                    tryBuyResource(player, MyEnum.Resources.coal);
                }
            }
        }

        if(good == MyEnum.Goods.lumber || good == MyEnum.Goods.paper)
        {
            int woodWanted = (int)(potential);
            if (player.getNumberResource(MyEnum.Resources.wood) < woodWanted)
            {
                int woodToBuy = (int)(woodWanted - player.getNumberResource(MyEnum.Resources.wood)) + 1;
                for (int i = 0; i < woodToBuy; i++)
                {
                    tryBuyResource(player, MyEnum.Resources.wood);
                }
            }
        }
        if (good == MyEnum.Goods.fabric)
        {
            int cottonWanted = (int)(potential * 0.83f);
            int dyesWanted = (int)(potential * 0.23f);
            if (player.getNumberResource(MyEnum.Resources.cotton) < cottonWanted)
            {
                int cottonToBuy = (int)(cottonWanted - player.getNumberResource(MyEnum.Resources.iron)) + 1;
                for (int i = 0; i < cottonToBuy; i++)
                {
                    tryBuyResource(player, MyEnum.Resources.cotton);
                }
            }
            if (player.getNumberResource(MyEnum.Resources.dyes) < dyesWanted)
            {
                int coalToBuy = (int)(dyesWanted - player.getNumberResource(MyEnum.Resources.dyes)) + 1;
                for (int i = 0; i < dyesWanted; i++)
                {
                    tryBuyResource(player, MyEnum.Resources.dyes);
                }
            }
        
    }

        if(good == MyEnum.Goods.parts || good == MyEnum.Goods.arms)
        {
            int steelWanted = (int)(potential);
            if (player.getNumberGood(MyEnum.Goods.steel) < steelWanted)
            {
                int steelToBuy = (int)(steelWanted - player.getNumberGood(MyEnum.Goods.steel)) + 1;
                for (int i = 0; i < steelToBuy; i++)
                {
                    tryBuyGood(player, MyEnum.Goods.steel);
                }
            }
        }

        if(good == MyEnum.Goods.clothing)
        {
            int fabricWanted = (int)(potential);
            if (player.getNumberGood(MyEnum.Goods.fabric) < fabricWanted)
            {
                int fabricToBuy = (int)(fabricWanted - player.getNumberGood(MyEnum.Goods.fabric)) + 1;
                for (int i = 0; i < fabricToBuy; i++)
                {
                    tryBuyGood(player, MyEnum.Goods.fabric);
                }
            }
        }

        if(good == MyEnum.Goods.furniture)
        {
            int lumberWanted = (int)(potential * 0.75f);
            int fabricWanted = (int)(potential * 0.25f);
            if (player.getNumberGood(MyEnum.Goods.fabric) < fabricWanted)
            {
                int fabricToBuy = (int)(fabricWanted - player.getNumberGood(MyEnum.Goods.fabric)) + 1;
                for (int i = 0; i < fabricToBuy; i++)
                {
                    tryBuyGood(player, MyEnum.Goods.fabric);
                }
            }
            if (player.getNumberGood(MyEnum.Goods.lumber) < lumberWanted)
            {
                int lumberToBuy = (int)(lumberWanted - player.getNumberGood(MyEnum.Goods.lumber)) + 1;
                for (int i = 0; i < lumberToBuy; i++)
                {
                    tryBuyGood(player, MyEnum.Goods.lumber);
                }
            }
        }

        if (good == MyEnum.Goods.chemicals)
        {
            int coalWanted = (int)(potential);
            if (player.getNumberResource(MyEnum.Resources.coal) < coalWanted)
            {
                int woodToBuy = (int)(coalWanted - player.getNumberResource(MyEnum.Resources.coal)) + 1;
                for (int i = 0; i < coalWanted; i++)
                {
                    tryBuyResource(player, MyEnum.Resources.coal);
                }
            }
        }
    }
   

    public int getProcessingCapacity(Nation player, MyEnum.Goods good)
    {
        int factoryLevel = player.getFactoryLevel(good);

        if(factoryLevel == 0)
        {
            return 1;
        }
        if(factoryLevel == 1)
        {
            return 3;
        }
        if(factoryLevel == 2)
        {
            return 6;
        }
        if(factoryLevel == 3)
        {
            return 9;
        }
        else
        {
            return 0;
        }

    }

    public bool tryProvinceDevelopment(Nation player, List<int> devOptions)
    {
        int intResOrderCounter = 0;
        while (intResOrderCounter < 12)
        {
            MyEnum.Resources desiredRes = player.getAI().getTopLevel().getTopResourcePriorityN(intResOrderCounter);
            for (int i = 0; i < devOptions.Count; i++)
            {
                assemblyCsharp.Province prov = State.getProvinces()[devOptions[i]];
                MyEnum.Resources res = prov.getResource();
                if (res == desiredRes)
                {
                    if (PlayerCalculator.checkUpgradeDevelopment(prov, player))
                    {
                        PlayerPayer.payDevelopment(player, prov);
                        TopLevel topLevel = player.getAI().getTopLevel();
                        topLevel.metaPriorities[MyEnum.metaPriorities.development]--;
                        topLevel.developmentPriorities[MyEnum.developmentPriorities.developProvince]--;
                        //Player takes care of increase;
                        return true;
                    }
                    else
                    {
                        if(player.getIP() < 1)
                        {
                            if (PlayerCalculator.canMakeDevelopmentAction(player))
                            {
                                PlayerPayer.payForDevelopmentAction(player, 1);
                                PlayerReceiver.receiveIP(player);
                                ipAdds++;
                            }
                        }
                        if(prov.resource == MyEnum.Resources.wheat)
                        {
                            if (player.getNumberGood(MyEnum.Goods.parts) < 1)
                            {
                                Debug.Log("Try to get parts for wheat development");
                                tryToObtainParts(player);
                                if(prov.getDevelopmentLevel() == 1)
                                {
                                    tryToObtainParts(player);
                                }
                            }                 
                        }
                        if(prov.resource == MyEnum.Resources.meat)
                        {
                            if(prov.getDevelopmentLevel() == 0 && player.getNumberGood(MyEnum.Goods.lumber) < 1)
                            {
                                Debug.Log("Try to get lumber for meat development");

                                tryToObtainLumber(player);
                            }
                            if(prov.getDevelopmentLevel() == 1)
                            {
                                if(player.getNumberGood(MyEnum.Goods.parts) < 2)
                                {
                                    Debug.Log("Try to get parts for meat development");

                                    tryToObtainParts(player);

                                }
                                if(player.getNumberGood(MyEnum.Goods.parts) < 1)
                                {
                                    tryToObtainParts(player);

                                }
                            }
                        }
                        if (prov.getResource() == MyEnum.Resources.fruit || prov.getResource() == MyEnum.Resources.dyes ||
                                prov.getResource() == MyEnum.Resources.rubber)
                        {
                            if(prov.getDevelopmentLevel() == 0 && player.getNumberGood(MyEnum.Goods.parts) < 1)
                            {
                                Debug.Log("Try to get parts for fruit/dyes/rubber development");

                                tryToObtainParts(player);
                            }
                            if(prov.getDevelopmentLevel() == 1)
                            {
                                if (player.getNumberGood(MyEnum.Goods.chemicals) < 2)
                                {
                                    Debug.Log("Try to get chemicals for fruit/dyes/rubber development");

                                    tryToObtainChemicals(player);

                                }
                                if (player.getNumberGood(MyEnum.Goods.chemicals) < 1)
                                {
                                    tryToObtainChemicals(player);                          
                                }
                            }
                        }
                        if (prov.getResource() == MyEnum.Resources.iron || prov.getResource() == MyEnum.Resources.coal ||
                                 prov.getResource() == MyEnum.Resources.gold)
                        {
                            if (prov.getDevelopmentLevel() == 0 && player.getNumberGood(MyEnum.Goods.lumber) < 1)
                            {
                                Debug.Log("Try to get lumber for iron/coal/gold development");

                                tryToObtainLumber(player);
                            }
                            if(prov.getDevelopmentLevel() == 1)
                            {
                                if (player.getNumberGood(MyEnum.Goods.parts) < 1)
                                {
                                    Debug.Log("Try to get parts for iron/coal/gold development");

                                    tryToObtainParts(player);
                                }
                                if (player.getNumberGood(MyEnum.Goods.chemicals) < 1)
                                {
                                    tryToObtainChemicals(player);
                                }
                            }
                        }
                        if (prov.getResource() == MyEnum.Resources.cotton)
                        {
                            if(player.getNumberGood(MyEnum.Goods.parts) < 1)
                            {
                                Debug.Log("Try to get parts for cotton development");

                                tryToObtainParts(player);
                            }
                            if(prov.getDevelopmentLevel() == 1 && player.getNumberGood(MyEnum.Goods.lumber) < 1)
                            {
                                tryToObtainLumber(player);
                            }
                        }
                        if (prov.getResource() == MyEnum.Resources.wood)
                        {
                            if(player.getNumberGood(MyEnum.Goods.parts) < 1)
                            {
                                Debug.Log("Try to get parts for wood development");

                                tryToObtainParts(player);
                            }
                            if(prov.getDevelopmentLevel() == 1 && player.getNumberGood(MyEnum.Goods.parts) < 2)
                            {
                                tryToObtainParts(player);
                            }
                        }
                        if (prov.getResource() == MyEnum.Resources.oil)
                        {
                            if (player.getNumberGood(MyEnum.Goods.parts) < 2)
                            {
                                tryToObtainParts(player);
                                tryToObtainParts(player);
                            }
                        }

                    }

                }
            }
            intResOrderCounter++;       
        }
        return false;
    }

    public void tryManufactureGoods(Nation player)
    {
        //stub
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
            if (player.getNumberGood(MyEnum.Goods.lumber) < 1)
            {
                tryToObtainLumber(player);
            }
            if (player.getNumberGood(MyEnum.Goods.steel) < 1)
            {
                tryToObtainSteel(player);
            }
            if (player.GetShipyardLevel() == 2)
            {
                if (player.getNumberGood(MyEnum.Goods.lumber) < 1)
                {
                    tryToObtainLumber(player);
                }
                if (player.getNumberGood(MyEnum.Goods.steel) < 1)
                {
                    tryToObtainSteel(player);
                }
                if (player.getNumberGood(MyEnum.Goods.parts) < 1)
                {
                    tryToObtainParts(player);
                }

            }
        }
    }

    public bool considerDefensiveNeed(Nation player)
    {
        int strength = PlayerCalculator.CalculateArmyScore(player);
        TopLevel topLevel = player.getAI().getTopLevel();
       // foreach (KeyValuePair<int, Relation> entry in relations)
       foreach(int majorIndex in State.getMajorNations())
        {
            Nation other = State.getNations()[majorIndex];
            if (player.Relations[majorIndex] < 30)
            {
                if (State.mapUtilities.shareLandBorder(player, other))
                {
                    if (PlayerCalculator.CalculateArmyScore(other) / strength + 0.01f > 1.25f)
                    {
                        
                        return true;
                    }
                }
                else
                {
                    if (PlayerCalculator.CalculateNavalProjection(other) / strength + 0.01f > 1.22f)
                    {
                        int otherNavalStrength = other.seaForces.getNavalStrength(other);
                        int selfNavalStrength = player.seaForces.getNavalStrength(player);
                        if (otherNavalStrength / (selfNavalStrength + 0.01f) > 1.25f)
                        {
                            return true;
                        }
                    }
                }
            }
        }
        return false;
    }

    //modify preferences
    public void considerMilitaryNeed(Nation player)
    {
        int strength = PlayerCalculator.CalculateArmyScore(player);
        TopLevel topLevel = player.getAI().getTopLevel();
        foreach (int majorIndex in State.getMajorNations())
        {
            Nation other = State.getNations()[majorIndex];
            if (player.Relations[majorIndex] < 30)
            {
                if (State.mapUtilities.shareLandBorder(player, other))
                {
                    if (PlayerCalculator.CalculateArmyScore(other) / strength > 1.25)
                    {
                        topLevel.productTypePriorities[MyEnum.productionPriorities.buildUnit]++;
                        topLevel.metaPriorities[MyEnum.metaPriorities.production]++;
                    }
                }
                else
                {
                    if (PlayerCalculator.CalculateNavalProjection(other) / strength > 1.22)
                    {
                        topLevel.productTypePriorities[MyEnum.productionPriorities.buildUnit]++;
                        topLevel.metaPriorities[MyEnum.metaPriorities.production]++;

                    }
                }
            }
        }

    }

    public void exportResourcesAndGoods(Nation player)
    {
        TopLevel aiTopLevel = player.getAI().GetTopLevel();
        Market market = State.market;
        foreach (MyEnum.Resources res in Enum.GetValues(typeof(MyEnum.Resources)))
        {
            if (res == MyEnum.Resources.gold)
            {
                continue;
            }
            float amount = player.getNumberResource(res);
            if (amount > aiTopLevel.getResToKeep(res))
            {
                int willingToSell = (int)(amount - aiTopLevel.getResToKeep(res));
                if (willingToSell > 1)
                {
                    aiTopLevel.alterResPriority(player, res, -0.05f);
                }

                if (willingToSell > 6)
                {
                    aiTopLevel.alterResPriority(player, res, -0.05f);
                    willingToSell = 6;
                }
                if (State.currentPlayerIsMajor) {
                    Debug.Log("Difference between supply and demand: " + market.getDifferenceBetweenSupplyAndDemandResource(res));
                        }
                if (willingToSell >= 1 //&& market.getDifferenceBetweenSupplyAndDemandResource(res) < 1)
                    )
                   {
                    Debug.Log("Willing to sell: " + willingToSell + " " + res);
                    for (int i = 0; i < willingToSell; i++)
                    {
                      
                        Signal offer = new Signal
                            (MyEnum.marketChoice.offer, player.getIndex(), res, MyEnum.Goods.arms, 1, true, false);
                        market.addResourceOffer(res, offer);
                        player.consumeResource(res, 1);
                        Debug.Log(player.getName() + " has placed a " + res + " on the market _________________");

                    }
                }
                else if (willingToSell >= 1 && market.getDifferenceBetweenSupplyAndDemandResource(res) < 3 && player.getGold() < 30)
                {
                    for (int i = 0; i < willingToSell; i++)
                    {
                        Signal offer = new Signal
                            (MyEnum.marketChoice.offer, player.getIndex(), res, MyEnum.Goods.arms, 1, true, false);
                        market.addResourceOffer(res, offer);
                        player.consumeResource(res, 1);

                        Debug.Log(player.getName() + " has placed a " + res + " on the market _______________");

                    }
                }
                else if (willingToSell >= 1 && market.getDifferenceBetweenSupplyAndDemandResource(res) < 8 && player.getGold() < 12)
                {
                    for (int i = 0; i < willingToSell; i++)
                    {
                        Signal offer = new Signal
                            (MyEnum.marketChoice.offer, player.getIndex(), res, MyEnum.Goods.arms, 1, true, false);
                        market.addResourceOffer(res, offer);
                        player.consumeResource(res, 1);

                        Debug.Log(player.getName() + " has placed a " + res + " on the market _______________________");
                    }
                }

            }
        }
        foreach (MyEnum.Goods good in Enum.GetValues(typeof(MyEnum.Goods)))
        {
            float amount = player.getNumberGood(good);
            if (amount > aiTopLevel.getGoodToKeep(good))
            {
                int willingToSell = (int)(amount - aiTopLevel.getGoodToKeep(good));

                if (willingToSell > 6)
                {
                    aiTopLevel.alterProductionPriorities(player, good, -0.1f);
                    willingToSell = 6;
                }

                for (int i = 0; i < willingToSell; i++)
                {
                    Signal offer = new Signal
                        (MyEnum.marketChoice.offer, player.getIndex(), MyEnum.Resources.coal, good, 1, false, false);
                    market.addGoodOffer(good, offer);
                    player.consumeGoods(good, 1);
                    Debug.Log(player.getName() + " has placed a " + good + " on the market");

                }
            }

        }
    }


    public void AI_TransportChoices(Nation player)
    {
        foreach (MyEnum.Resources res in Enum.GetValues(typeof(MyEnum.Resources)))
        {
            player.setResTransportFlow(res, 0);
        }

        int totalProduction = PlayerCalculator.calculateTotalResourceProduction(player);
        int flowCapacity = PlayerCalculator.calculateMaxTransportFlow(player);
        int totalNumProvinces = PlayerCalculator.getTotalNumberOfProvinces(player);
        if (flowCapacity >= totalProduction)
        {
            foreach (MyEnum.Resources res in Enum.GetValues(typeof(MyEnum.Resources)))
            {
                int production = PlayerCalculator.getResourceProducing(player, res);
                player.setResTransportFlow(res, production);

            }
            float coalNeeded = (totalProduction - totalNumProvinces) * 0.2f;
            player.consumeResource(MyEnum.Resources.coal, coalNeeded);
        }
        else
        {
            int counter = 0;
            while (counter < 12 && PlayerCalculator.calculateTransportFlow(player) < flowCapacity)
            {
                MyEnum.Resources res = player.getAI().GetTopLevel().getTopResourcePriorityN(counter);
               
                int prod = PlayerCalculator.getResourceProducing(player, res);
                player.setResTransportFlow(res, prod); 
                counter++;
            }
            // Increased coal use to text if it is working 
            float coalNeeded = (PlayerCalculator.calculateTransportFlow(player) - totalNumProvinces) * 0.25f;
            player.consumeResource(MyEnum.Resources.coal, coalNeeded);
        }
    }

    public int infulenceToSaveForEvents(Nation player)
    {
        int turn = State.turn;
        if(turn < 4)
        {
            return 1;
        }
        if(turn < 10)
        {
            return 2;
        }
        if(turn < 20)
        {
            return 3;
        }
        if(turn < 32)
        {
            return 4;
        }
        else
        {
            return 5;
        }
    }
  
    public int decideWhoToInfulence(Nation player)
    {
        List<int> minors = State.getMinorNations();

        List<int> options = new List<int>();
        HashSet<int> recog = player.RecognizingTheseClaims;
        foreach(int item in minors)
        {
            if (!recog.Contains(item))
            {
                options.Add(item);
            }
        }

        int numOpts = options.Count;

        if (player.getType() != MyEnum.NationType.major)
        {
            int choice = UnityEngine.Random.Range(0, numOpts);
            return options[choice];
        }
        else
        {
            HashSet<int> preferedSpheres = player.getAI().SpherePreferences;
            foreach (int item in preferedSpheres)
            {
                if (recog.Contains(item))
                {
                    preferedSpheres.Remove(item);
                }
            }
            //First check if any relation is obviously too low
            foreach (int minorIndex in preferedSpheres)
            {
                int relation = player.Relations[minorIndex];
                if(relation < 55)
                {
                    return minorIndex;
                }
            }
            foreach (int minorIndex in preferedSpheres)
            {
                Debug.Log(minorIndex);
                int relation = player.Relations[minorIndex];
                if (relation < 65)
                {
                    return minorIndex;
                }
            }

            int roll = UnityEngine.Random.Range(0, 100);
            if(roll < 5)
            {
                Debug.Log(preferedSpheres.Count);
                int choice = UnityEngine.Random.Range(0, preferedSpheres.Count -1);
                int[] preferecesArray = preferedSpheres.ToArray();
                return preferecesArray[choice];
            }
            else
            {
                int choice = UnityEngine.Random.Range(0, numOpts);
                return options[choice];
            }
        }
    }

    public void finalShopping(Nation player)
    {
        Market market = State.market;
        AI ai = player.getAI();
        TopLevel topLevel = ai.getTopLevel();
        float moneyToSpend = player.getGold() - player.getTotalCurrentBiddingCost();
        foreach (MyEnum.Resources res in Enum.GetValues(typeof(MyEnum.Resources)))
        {
            if(player.getGold() < 4)
            {
                return;
            }
            if (res == MyEnum.Resources.rubber && !player.GetTechnologies().Contains("electricity"))
            {
                continue;
            }

            if(res == MyEnum.Resources.oil && !player.GetTechnologies().Contains("oil_drilling"))
            {
                continue;
            }

            if(res == MyEnum.Resources.gold)
            {
                continue;
            }
            float numberToKeep = topLevel.getResToKeep(res);
            float numRes = player.getNumberResource(res);
            float desireToAdd = numberToKeep - numRes;

            for(int i = 0; i < (int)desireToAdd; i++)
            {
                if (canAffordRes(player, res))
                {
                    bidOnResource(player, res);
                }
                moneyToSpend = player.getGold() - player.getTotalCurrentBiddingCost();
                if(moneyToSpend < 6)
                {
                    break;
                }  
            }     
        }

        foreach (MyEnum.Goods good in Enum.GetValues(typeof(MyEnum.Goods)))
        {
            if (player.getGold() < 5)
            {
                return;
            }

            float numberToKeep = topLevel.getGoodToKeep(good);
            float numGood = player.getNumberGood(good);
            float desireToAdd = numberToKeep - numGood;
            for (int i = 0; i < (int)desireToAdd; i++)
            {
                if (market.goodBeingOffered(good) && canAffordGood(player, good))
                {
                    bidOnGood(player, good);
                    moneyToSpend = player.getGold() - player.getTotalCurrentBiddingCost();
                    if (moneyToSpend < 8)
                    {
                        break;
                    }
                }
            }


        }

       /*     if (player.getGold() - player.getTotalCurrentBiddingCost() > 10)
        {
            if(canAffordRes(player, MyEnum.Resources.wheat) && player.getNumberResource(MyEnum.Resources.wheat) < 3)
            {
                bidOnResource(player, MyEnum.Resources.wheat);
            }
            if (canAffordRes(player, MyEnum.Resources.meat) && player.getNumberResource(MyEnum.Resources.meat) < 2)
            {
                bidOnResource(player, MyEnum.Resources.meat);
            }
            if (canAffordRes(player, MyEnum.Resources.fruit) && player.getNumberResource(MyEnum.Resources.fruit) < 2)
            {
                bidOnResource(player, MyEnum.Resources.fruit);
            }
        }

        if (player.getGold() - player.getTotalCurrentBiddingCost() > 12)
        {
            if (canAffordRes(player, MyEnum.Resources.wheat) && player.getNumberResource(MyEnum.Resources.wheat) < 3)
            {
                bidOnResource(player, MyEnum.Resources.wheat);
            }
   
        }



        if (player.getGold() - player.getTotalCurrentBiddingCost() > 10)
        {
            if(market.goodBeingOffered(MyEnum.Goods.clothing) && player.getNumberGood(MyEnum.Goods.clothing) < 2)
            {
                bidOnGood(player, MyEnum.Goods.clothing);
            }
        }
        if (player.getGold() - player.getTotalCurrentBiddingCost() > 10)
        {
            if (market.goodBeingOffered(MyEnum.Goods.parts) && player.getNumberGood(MyEnum.Goods.parts) < 2)
            {
                bidOnGood(player, MyEnum.Goods.parts);
            }
        }

        if (player.getGold() - player.getTotalCurrentBiddingCost() > 10)
        {
            if (market.goodBeingOffered(MyEnum.Goods.paper) && player.getNumberGood(MyEnum.Goods.paper) < 2)
            {
                bidOnGood(player, MyEnum.Goods.paper);
            }
        }

        if (player.getGold() - player.getTotalCurrentBiddingCost() > 10)
        {
            if (market.goodBeingOffered(MyEnum.Goods.arms) && player.getNumberGood(MyEnum.Goods.arms) < 3)
            {
                bidOnGood(player, MyEnum.Goods.arms);
            }
        }

        if (player.getGold() - player.getTotalCurrentBiddingCost() > 16)
        {
            if (market.goodBeingOffered(MyEnum.Goods.auto) && player.getNumberGood(MyEnum.Goods.auto) < 2)
            {
                bidOnGood(player, MyEnum.Goods.auto);
            }
        }

        if (player.getGold() - player.getTotalCurrentBiddingCost() > 10)
        {
            if (market.goodBeingOffered(MyEnum.Goods.furniture) && player.getNumberGood(MyEnum.Goods.furniture) < 2)
            {
                bidOnGood(player, MyEnum.Goods.furniture);
            }
        }
     

        if (player.getGold() - player.getTotalCurrentBiddingCost() > 10)
        {
            if (market.goodBeingOffered(MyEnum.Goods.telephone) && player.getNumberGood(MyEnum.Goods.telephone) < 2)
            {
                bidOnGood(player, MyEnum.Goods.telephone);
            }
        }

       

        if (player.getGold() - player.getTotalCurrentBiddingCost() > 10)
        {
            if (market.goodBeingOffered(MyEnum.Goods.chemicals) && player.getNumberGood(MyEnum.Goods.chemicals) < 2)
            {
                bidOnGood(player, MyEnum.Goods.chemicals);
            }
        }

        if (player.getGold() - player.getTotalCurrentBiddingCost() > 10)
        {
            if (market.goodBeingOffered(MyEnum.Goods.gear) && player.getNumberGood(MyEnum.Goods.gear) < 2)
            {
                bidOnGood(player, MyEnum.Goods.gear);
            }
        }

        if (player.getGold() - player.getTotalCurrentBiddingCost() > 10)
        {
            if (market.goodBeingOffered(MyEnum.Goods.clothing) && player.getNumberGood(MyEnum.Goods.clothing) < 3)
            {
                bidOnGood(player, MyEnum.Goods.clothing);
            }
        }

        if (player.getGold() - player.getTotalCurrentBiddingCost() > 10)
        {
            if (market.goodBeingOffered(MyEnum.Goods.arms) && player.getNumberGood(MyEnum.Goods.arms) < 3)
            {
                bidOnGood(player, MyEnum.Goods.arms);
            }
        }

        if (player.getGold() - player.getTotalCurrentBiddingCost() > 10)
        {
            if (market.goodBeingOffered(MyEnum.Goods.arms) && player.getNumberGood(MyEnum.Goods.arms) < 4)
            {
                bidOnGood(player, MyEnum.Goods.arms);
            }
        }  */



    }

    public int getMinimalCashFlow(Nation player)
    {
        int minimalCashFlow = 0;
        MyEnum.NationType type = player.getType();
        int turn = State.turn;
        if (type == MyEnum.NationType.major)
        {
            minimalCashFlow = 8 + (turn / 5);
        }
        else if (type == MyEnum.NationType.minor)
        {
            minimalCashFlow = 6 + (turn / 10);
        }
        return minimalCashFlow;
    }

    public int getCashToKeepOnHand(Nation player)
    {
        int cashToKeep = 1000;
        MyEnum.NationType type = player.getType();
        int turn = State.turn;
        if (type == MyEnum.NationType.major)
        {
            cashToKeep = 10 + turn;
        }
        else if (type == MyEnum.NationType.minor)
        {
            cashToKeep = 8 + (turn / 6);
        }
        return cashToKeep;
    }



    public void decideToRaiseMoney(Nation player)
    {
        if(player.getType() == MyEnum.NationType.oldMinor)
        {
            return;
        }
        WorldBank bank = State.bank;
        int minimalCashFlow = getMinimalCashFlow(player);
        int playerIndex = player.getIndex();
        // Check if need to raise money
        int loopBreaker = 0;
        while(player.getGold() < minimalCashFlow && loopBreaker < 5)
        {
            loopBreaker++;
            //If player has assets - withdrawl from bank
            int assets = bank.getDeposits(player);
            if(assets > 0)
            {
                bank.withdrawDeposit(player);
            }
            // Else take out a loan
            else if (PlayerCalculator.canTakeOutLoan(player))
            {
                bank.takeLoan(player);
            }
        }
    }

    public void decideToPayLoan(Nation player)
    {
        WorldBank bank = State.bank;
        int debt = bank.getDebt(player);
        int cashToKeep = getCashToKeepOnHand(player);
        int cash = (int)player.getGold();
        int bondSize = bank.BondSize;
        int loopBreaker = 0;
        while (player.getGold() >= (cashToKeep + bondSize) && bank.getDebt(player) > 0 && loopBreaker < 5)
        {
            loopBreaker++;        
            bank.payOffLoan(player);         
        }
    }


    public void decideToDepositMoney(Nation player)
    {
        WorldBank bank = State.bank;
        int debt = bank.getDebt(player);
        int cashToKeep = getCashToKeepOnHand(player);
        int cash = (int)player.getGold();
        int bondSize = bank.BondSize;
        int loopBreaker = 0;
        while (player.getGold() > cashToKeep + bondSize && loopBreaker < 5)
        {
            loopBreaker++;
            bank.makeDeposit(player);
        }
    }
}


    











