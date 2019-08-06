using assemblyCsharp;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AI {

    private TopLevel topLevel = new TopLevel();
    private Diplomatic diploMod = new Diplomatic();
    private EventResponder eventResponder = new EventResponder();

    private Tactical tacticsMod = new Tactical();
    private AdminAI adminMod = new AdminAI();
    private DevelopmentAI developmentAI = new DevelopmentAI();
    public int numberOfTurns = 0;
    private HashSet<int> spherePreferences = new HashSet<int>();

    public bool usedAllAPLastTurn = false;
    private bool didResearch = false;
    private Queue<int> newRailRoads = new Queue<int>();

    public Queue<int> NewRailRoads { get => newRailRoads; set => newRailRoads = value; }
    public HashSet<int> SpherePreferences { get => spherePreferences; set => spherePreferences = value; }

    public void setDidResearch(bool value)
    {
        didResearch = value;
    }

    public bool getDidResearch()
    {
        return didResearch;
    }

    public TopLevel GetTopLevel()
    {
        return this.topLevel;
    }

    public Diplomatic GetDiplomatic()
    {
        return diploMod;
    }

    public Tactical GetTactical()
    {
        return tacticsMod;
    }

    public AdminAI GetAdmin()
    {
        return adminMod;
    }

    public DevelopmentAI getDevelopmentAI()
    {
        return this.developmentAI;
    }


    public void processAdminstrationPhase(Nation player)
    {
        this.numberOfTurns++;
        newRailRoads.Clear(); 
        ///for now
        //  player.setCorruption(MyEnum.fiveLevelLow_High.normal);
        UnityEngine.Random.InitState(System.Environment.TickCount);
        if(player.IsColonyOf() != -1)
        {
            return;
        }
        Debug.Log("It is now " + player.getName() + "'s turn____________________________________________________________________");
        Debug.Log("Gold: " + player.getGold());
        Debug.Log("AP:  " + player.getAP());
        Debug.Log("Type is: " + player.getType());
        player.resetTotalCurrentBiddingCost();

       // topLevel.UpdateDevelopmentPriorities(player);
      //  topLevel.UpdateProductTypePriorities(player);
      //  topLevel.UpdateProgressPriorities();
       // topLevel.UpdateMetaPriorities(player);
       // Debug.Log("Current player is major?: " + State.currentPlayerIsMajor);
        topLevel.updateProductionPriorities(player);



        AdminAI admin = GetAdmin();
        admin.AI_TransportChoices(player);


        /*   PRINCIPLE ACTIONS:
         *       (1) Try to get AP
         *       (2) Try to get DP
         *       (3) Spend DP
         *           (a) Research
         *               (i) Get Technology
         *           (b) Investment
         *           (c) Culture
         *           (d) Doctrines
         *       (4) Development
         *           (a) Build/Upgrade Factory
         *           (b) Develop Province
         *           (c) Build Railway
         *           (d) Build Train
         *           (e) Upgrade Port
         *       (5) Export Manifacture
         *           
         */
        //admin.handleEachResourceNeed(player);

        if (State.turn > 6)
        {
            admin.decideToPayLoan(player);
            admin.decideToDepositMoney(player);
            admin.decideToRaiseMoney(player);

        }
        mainAI_Sequence(player);

        if(player.getType() == MyEnum.NationType.major && State.turn > 18)
        {
            mainAI_Sequence(player);

        }
        admin.productionForExport(player);
        admin.exportResourcesAndGoods(player);
        admin.finalShopping(player);
        /* int tries = ((int)player.getAP() * 2);

         int minAP = 3;
        if(State.era != MyEnum.Era.Early)
         {
             minAP = 2;
         } */

        /* for(int i = 0; i < tries; i++){
             if(player.getAP() <= minAP)
             {
                 return;
             }
             MyEnum.metaPriorities pick = topLevel.getTopMetaPriority();
             topLevel.metaPriorities[pick]--;

             if (pick == MyEnum.metaPriorities.development)
             {
                 Debug.Log("Development -------------");
                 admin.performDevelopmentAction(player, admin);
             }
             if(pick == MyEnum.metaPriorities.production)
             {
                 Debug.Log("Production --------------");
                 performProductionActions(player, admin);
             }
             if(pick == MyEnum.metaPriorities.progress)
             {
                 Debug.Log("Progress-------------------");
                 performProgressAction(player, admin);
             }
         } */

        // if(player.getAP() == 0)
        //  {
        //      player.getAI().usedAllAPLastTurn = true;
        //  }

    }

    public void mainAI_Sequence(Nation player)
    {
        AdminAI admin = GetAdmin();
        MyEnum.NationType type = player.getType();

        if (type == MyEnum.NationType.major || type == MyEnum.NationType.oldEmpire)
        {
            int factLevels = player.industry.getTotalNumberFactoryLevels(player);
            if (factLevels < 3 || factLevels <= PlayerCalculator.getNumberProvDevelopments(player))
            {
                admin.considerFactory(player);
            }
        }

        // Gaining AP
        admin.handleAP(player);

        admin.considerRailroads(player);
        admin.considerTrains(player);
        admin.considerProvinceDevelopment(player);


        // Gaining DP
        admin.handleDP(player);
        admin.considerUnitDrafting(player);
        int researchRoll = Random.Range(0, 100);
        if (researchRoll < 80)
        {
            if (PlayerCalculator.canMakeDevelopmentAction(player))
            {
                PlayerPayer.payForDevelopmentAction(player, 1);
                PlayerReceiver.CollectResearchPoints(player);
                admin.RpAdds++;
                admin.tryToGainTechnology(player);
                admin.tryToGainTechnology(player);
            }
        }
        else
        {
            if(State.turn > 25)
            {
                if (PlayerCalculator.canMakeDevelopmentAction(player))
                {
                    PlayerPayer.payForDevelopmentAction(player, 1);
                    PlayerReceiver.CollectResearchPoints(player);
                    admin.RpAdds++;
                    admin.tryToGainTechnology(player);
                    admin.tryToGainTechnology(player);
                }
            }
        }

        admin.considerCulture(player);

        admin.considerDoctrines(player);
       
        admin.considerShipConstruction(player);

        if (player.GetCorruption() > 4)
        {
            if (PlayerCalculator.canMakeDevelopmentAction(player))
            {
                PlayerPayer.payForDevelopmentAction(player, 1);
                player.decreaseCorruption();
                //PlayerReceiver.reduceCorruption(player);
            }
        }

        int infulenceToSave = admin.infulenceToSaveForEvents(player);

        while(player.InfulencePoints > infulenceToSave)
        {
            int minorIndex = admin.decideWhoToInfulence(player);
            PlayerPayer.payToInfulenceMinor(player);
            Nation minor = State.getNations()[minorIndex];
            PlayerReceiver.receiveInfulenceMinor(player, minor);
        }
    }

    public void performProgressAction(Nation player, AdminAI admin)
    {
        if(player.getAP() < 1 || player.getDP() < 1)
        {
            return;
        }
        TopLevel topLevel = player.getAI().topLevel;
       
        PlayerPayer.payForDevelopmentAction(player, 1);
        topLevel.metaPriorities[MyEnum.metaPriorities.progress]--;

        MyEnum.progressPriorities priority = topLevel.getTopProgressPriority();
        if(priority == MyEnum.progressPriorities.corruption)
        {
            Debug.Log("Corruption -------------------------------");
            player.decreaseCorruption();
            //PlayerReceiver.reduceCorruption(player);
            topLevel.progressPriorities[MyEnum.progressPriorities.corruption] --;
        }
        if(priority == MyEnum.progressPriorities.culture)
        {
            Debug.Log("Culture ----------------------------------------");
            PlayerReceiver.collectCultureCard(player);
            topLevel.progressPriorities[MyEnum.progressPriorities.culture]--;
        }
        if (priority == MyEnum.progressPriorities.doctrines)
        {
            Debug.Log("Doctrines ----------------------------------------------");
            MyEnum.ArmyDoctrines doctrineChoice = AI_Helper.chooseDoctrine(player);
            player.landForces.addDoctrine(doctrineChoice);
            topLevel.progressPriorities[MyEnum.progressPriorities.doctrines]--;
        }
        if (priority == MyEnum.progressPriorities.investment)
        {
            Debug.Log("Investment ----------------------------------------------");
            PlayerReceiver.receiveIP(player);
            topLevel.progressPriorities[MyEnum.progressPriorities.investment]--;
        }
        if (priority == MyEnum.progressPriorities.research)
        {
            Debug.Log("Research ---------------------------------------------------");
            PlayerReceiver.CollectResearchPoints(player);
            admin.tryToGainTechnology(player);
            topLevel.progressPriorities[MyEnum.progressPriorities.research]--;
        }
    }


    public void performProductionActions(Nation player, AdminAI admin)
    {
        TopLevel topLevel = player.getAI().topLevel;
        MyEnum.productionPriorities priority = topLevel.getTopProductPriority();
        if(priority == MyEnum.productionPriorities.buildShip)
        {
            Debug.Log("Build Ship ------------------------------------");
            if (admin.tryBuildNavy(player))
            {
                player.getAI().GetTopLevel().productTypePriorities[MyEnum.productionPriorities.buildShip]--;
                player.getAI().GetTopLevel().metaPriorities[MyEnum.metaPriorities.production]--;
                //  player.getAI().GetTopLevel().productTypePriorities[MyEnum.productionPriorities.manifactureGoods]++;
            }
            else
            {
                admin.tryToBuildShipNextTurn(player);
            }
        }
        else if(priority == MyEnum.productionPriorities.buildUnit)
        {
            Debug.Log("Build Unit ---------------------------------------");
           // if (admin.tryToBuildUnit(player))
            {
            //    player.getAI().GetTopLevel().productTypePriorities[MyEnum.productionPriorities.buildUnit]--;
             //   player.getAI().GetTopLevel().metaPriorities[MyEnum.metaPriorities.production]--;
                //player.getAI().GetTopLevel().productTypePriorities[MyEnum.productionPriorities.manifactureGoods]++;
            }
        }
        else
        {
            Debug.Log("Manfacture Goods -----------------------------------");
            // Note that this function is currently a stub. 
            // The function will have to first determine what the player ought to produce. Since prodcution of needed goods will be called independently
            //when trying to accomplish various goals, the goods produced here will likely be primarily for export purposes, so the player will want to 
            //consider what has been in hight demand in the Market and what he himself is able to produce in decent numbers.
            admin.tryManufactureGoods(player);
            player.getAI().GetTopLevel().productTypePriorities[MyEnum.productionPriorities.manifactureGoods]--;
            player.getAI().GetTopLevel().metaPriorities[MyEnum.metaPriorities.production]--;
        }
    }


    public void processMovements()
    {
        return;
    }

    // Returns the most desired resource
    public Vector2 resDesirabilityOfItem(Nation player, Nation item)
    {
        //Check desirability of each province
        int currentBest = 1000;
        int secondBest = 1001;
        for(int i = 0; i < item.getProvinces().Count; i++)
        foreach(int provIndex in item.getProvinces())
        {
            Province prov = State.getProvince(provIndex);
            MyEnum.Resources res = prov.resource;
            for(int j = 0; j < Enum.GetNames(typeof(MyEnum.Resources)).Length; j++)
            {
                MyEnum.Resources thisRes = player.getAI().getTopLevel().getTopResourcePriorityN(j);
                if (thisRes == res) {
                    if (j < currentBest)
                    {
                        if (currentBest < 1000)
                        {
                            secondBest = currentBest;
                            currentBest = j;
                        }
                        else
                        {
                            currentBest = j;
                        }
                    }
                    else
                    {
                        secondBest = j ;
                    }
                }
            }
        }
        Vector2 des = new Vector2(currentBest, secondBest);
        Debug.Log(des.x + " " + des.y);
        return des;
    }

    public bool decideIfBidItem(Nation player, Nation item, int currentBid, Vector2 desirability)
    {
        if(player.landForces.Strength < 3)
        {
            return false;
        }
        HashSet<int> recognizing = player.RecognizingTheseClaims;
        if (recognizing.Contains(item.getIndex()))
        {
            return false;
        }
        int points = 0;
      
        points = player.GetColonialPoints();
        
        if(points == 0)
        {
            return false;
        }
        Debug.Log("Points: " + points);
        Debug.Log("Current High Bid " + currentBid);
        if (currentBid >= points)
        {
            Debug.Log("Current bid higher than points");
            return false;
        }
       // Vector2 desirability = resDesirabilityOfIten(player, item);
        if(desirability.x == 0)
        {
            return true;
        }
        if(desirability.x == 1 && desirability.y == 2)
        {
            return true;
        }
        float percentOfCP = (float)currentBid / (float)points;
        Debug.Log("Percent of CP: " + percentOfCP);
        if(desirability.x <= 2 && percentOfCP < 0.62f)
        {
            return true;
        }
        if(desirability.x <= 3 && percentOfCP < 0.5f)
        {
            return true;
        }
        if(desirability.x <= 4 && percentOfCP < .33f)
        {
            return true;
        }
        if(desirability.x <= 5 && percentOfCP < .25f)
        {
            return true;
        }
        if(desirability.x <= 6 && percentOfCP < 0.15f)
        {
            return true;
        }
        return false;
    }

    public int decideBidAmountItem(Nation player, Nation item, int currentBid, Vector2 desired)
    {
        //Debug.Log("Current Bid " + currentBid);
        int points = 0;
      
       points = player.GetColonialPoints();
        
        if(currentBid < 3)
        {
            return currentBid += 1;
        }
        float percentOfCP = currentBid / points;
        int difference = points - currentBid;
        if(percentOfCP >= 0.88f  || difference < 3)
        {
            return currentBid += 1;
        }
        if(percentOfCP >= 0.75f || difference < 4)
        {
            return currentBid += 2;
        }
        if(percentOfCP >= 0.6f || difference < 5)
        {
            return currentBid += 3;
        }
        if(percentOfCP >= 0.5 || difference < 6)
        {
            return currentBid += 4;
        }
        if(percentOfCP >= 0.35 || difference < 7)
        {
            return currentBid += 5;
        }
        else
        {
            return currentBid += 6;
        }

    }


    public int makeBidColony(Nation nation)
    {
        int bid = -1;


        return bid;
    }

    public TopLevel getTopLevel()
    {
        return topLevel;
    }


    public EventResponder getEventResponder()
    {
        return eventResponder;
    }

   


}
