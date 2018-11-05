using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using assemblyCsharp;
using WorldMapStrategyKit;
using System;
using System.Linq;
using System.IO;

public static class State {

    public static MapUtilities mapUtilities = new MapUtilities();
    public static int turn;
    public static int currentPlayer;
    public static int currentTurnOrderIndex = 0;
    public static List<int> turnOrder = new List<int>(); 

    private static AuctionHandler auctionHandler = new AuctionHandler();
   public static TradeHandler tradeHandler = new TradeHandler();

    public static MyEnum.Era era;

    private static MyEnum.GamePhase gamePhase;

    private static Dictionary<int, Nation> nations = new Dictionary<int, Nation>();
    private static Dictionary<int, assemblyCsharp.Province> provinces =
        new Dictionary<int, assemblyCsharp.Province>();
    private static List<int> colonyOrder = new List<int>();
    private static List<int> sphereOrder = new List<int>();




    private static List<GameObjectAnimator> ResourceIcons = new List<GameObjectAnimator>();
    public static Market market = new Market();
    public static Dictionary<string, Technology> technologies = new Dictionary<string, Technology>();
    public static List<Relation> relations;
    //public static  List<War> wars;
    public static StatTracker history = new StatTracker();	
    public static List<TacticCard> tacticCardTypes = new List<TacticCard>();
    public static Stack<TacticCard> tacticCardDeck = new Stack<TacticCard>();
    public static List<CultureCard> cultureCardTypes = new List<CultureCard>();
    public static Stack<CultureCard> earlyCultureCardDeck = new Stack<CultureCard>();
    public static Stack<CultureCard> midCultureCardDeck = new Stack<CultureCard>();
    public static Stack<CultureCard> lateCultureCardDeck = new Stack<CultureCard>();

    public static CultureCard getCultureCardByName(MyEnum.cultCard cardName)
    {
        foreach(CultureCard card in cultureCardTypes)
        {
            if(cardName == card.getCardName())
            {
                return card;
            }
        }
        Debug.Log("Something went wrong");
        return cultureCardTypes[0];
    }

    public static int currentlySelectedNationDiplomacy = 0;
    public static int currentlySelectedProvince = -1;
    public static WorldAffairs worldAffairs = new WorldAffairs();


    public static MyEnum.GamePhase GetPhase()
    {
        return gamePhase;
    }

    public static Dictionary<int, Nation> getNations()
    {
        return nations;
    }

    public static Dictionary<int, assemblyCsharp.Province> getProvinces()
    {
        return provinces;
    }


    public static void TurnIncrement()
    {
        turn = turn + 1;
    }


    public static void setGamePhase(MyEnum.GamePhase phase)
    {
        gamePhase = phase;
    }

    public static void addResourceIcon(GameObjectAnimator icon)
    {
        ResourceIcons.Add(icon);
    }

    public static List<GameObjectAnimator> GetResourceIcons()
    {
        return ResourceIcons;
    }

    public static Dictionary<string, Technology> GetTechnologies()
    {
        return technologies;
    }


    public static Nation GetNationByName(string name)
    {
       // Debug.Log(name);
        for (int i = 0; i < nations.Count; i++)
        {
           // Debug.Log(i);
            if (nations[i].getNationName() == name)
            {
                return nations[i];
            }
        }
        return null;
    }

    public static assemblyCsharp.Province GetProvinceByName(string name)
    {
        {
            for (int i = 0; i < provinces.Count; i++)
            {
                if (provinces[i].getProvName() == name)
                {
                    return provinces[i];
                }
            }
            return null;
        }

    }

    public static void NextEra()
    {
        if (era == MyEnum.Era.Early)
        {
            era = MyEnum.Era.Middle;
        }
        else if (era == MyEnum.Era.Middle)
        {
            era = MyEnum.Era.Late;
        }
        else if (era == MyEnum.Era.Late)
        {
            Debug.Log("Already LATE ERA!!");
        }
    }

    public static MyEnum.Era GerEra()
    {
        return era;
    }

    public static void SetEra(MyEnum.Era _era)
    {
        era = _era;
    }

    public static Stack<TacticCard> getTacticDeck()
    {
        return tacticCardDeck;
    }

    public static List<TacticCard> getTacticCardTypes()
    {
        return tacticCardTypes;
    }

    public static Stack<CultureCard> getCultureDeck()
    {
        if (era == MyEnum.Era.Early)
        {
            Debug.Log("Returning early deck");
            return earlyCultureCardDeck;
        }
        else if(era == MyEnum.Era.Middle)
        {
            Debug.Log("Returning middle deck");
            return midCultureCardDeck;
        }
        else
        {
            Debug.Log("Returning late deck");
            return lateCultureCardDeck;
        }
    }

    public static List<CultureCard> getCultureCardTypes()
    {
        return cultureCardTypes;
    }

    public static CultureCard getCultureCard(MyEnum.cultCard card)
    {
        for(int i = 0; i < cultureCardTypes.Count; i++)
        {
            if(cultureCardTypes[i].getCardName() == card)
            {
                return cultureCardTypes[i];
            }
        }
        return cultureCardTypes[0];
   }



    public static void initializeTacticCards()
    {
        TacticCard ambush = new TacticCard(MyEnum.TacticCards.Ambush, MyEnum.TacticCardPhase.Engagement_Defend, 3);
        tacticCardTypes.Add(ambush);
        //TacticCard breakout = new TacticCard(MyEnum.TacticCards.Breakout, MyEnum.TacticCardPhase.Escape, 3);
        //tacticCardTypes.Add(breakout);
        TacticCard counter = new TacticCard(MyEnum.TacticCards.CounterScheme, MyEnum.TacticCardPhase.Intelligence, 3);
        tacticCardTypes.Add(counter);
        TacticCard deception = new TacticCard(MyEnum.TacticCards.Deception, MyEnum.TacticCardPhase.Intelligence, 4);
        tacticCardTypes.Add(deception);
        TacticCard demoralRaid = new TacticCard(MyEnum.TacticCards.DemoralizingRaid, MyEnum.TacticCardPhase.Raid, 4);
        tacticCardTypes.Add(demoralRaid);
        TacticCard evasion = new TacticCard(MyEnum.TacticCards.Evasion, MyEnum.TacticCardPhase.Escape, 3);
        tacticCardTypes.Add(evasion);
        TacticCard feign = new TacticCard(MyEnum.TacticCards.FeignedRetreat, MyEnum.TacticCardPhase.Engagement_Defend, 3);
        tacticCardTypes.Add(feign);
        TacticCard flank = new TacticCard(MyEnum.TacticCards.Flank, MyEnum.TacticCardPhase.Engagement_Attack, 3);
        tacticCardTypes.Add(flank);
        TacticCard drandDec = new TacticCard(MyEnum.TacticCards.GrandDeception, MyEnum.TacticCardPhase.Intelligence, 3);
        tacticCardTypes.Add(drandDec);
        TacticCard indirect = new TacticCard(MyEnum.TacticCards.IndirectApproach, MyEnum.TacticCardPhase.Engagement_Attack, 3);
        tacticCardTypes.Add(indirect);
        TacticCard line = new TacticCard(MyEnum.TacticCards.LineDefense, MyEnum.TacticCardPhase.Engagement_Defend, 3);
        tacticCardTypes.Add(line);
        TacticCard mastermind = new TacticCard(MyEnum.TacticCards.Mastermind, MyEnum.TacticCardPhase.Intelligence, 3);
        tacticCardTypes.Add(mastermind);
        TacticCard spy = new TacticCard(MyEnum.TacticCards.MilitarySpy, MyEnum.TacticCardPhase.Intelligence, 3);
        tacticCardTypes.Add(spy);
        TacticCard pen = new TacticCard(MyEnum.TacticCards.Penetration, MyEnum.TacticCardPhase.Engagement_Attack, 3);
        tacticCardTypes.Add(pen);
        TacticCard recon = new TacticCard(MyEnum.TacticCards.Recon, MyEnum.TacticCardPhase.Intelligence, 5);
        tacticCardTypes.Add(recon);
        TacticCard repelRaid = new TacticCard(MyEnum.TacticCards.RepelRaid, MyEnum.TacticCardPhase.Raid, 3);
        tacticCardTypes.Add(repelRaid);
        TacticCard supRaid = new TacticCard(MyEnum.TacticCards.SupplyRaid, MyEnum.TacticCardPhase.Raid, 3);
        tacticCardTypes.Add(supRaid);
        TacticCard TargetedRaid = new TacticCard(MyEnum.TacticCards.TargetedRaid, MyEnum.TacticCardPhase.Raid, 3);
        tacticCardTypes.Add(TargetedRaid);
        TacticCard turtle = new TacticCard(MyEnum.TacticCards.TurtleDefense, MyEnum.TacticCardPhase.Engagement_Defend, 3);
        tacticCardTypes.Add(turtle);
    }

    public static void createTacticCardDeck()
    {
        HashSet<MyEnum.TacticCards> standardCards = new HashSet<MyEnum.TacticCards> {
        MyEnum.TacticCards.Flank, MyEnum.TacticCards.Penetration, MyEnum.TacticCards.TurtleDefense,
        MyEnum.TacticCards.LineDefense, MyEnum.TacticCards.Evasion};
        foreach (TacticCard cardType in tacticCardTypes)
        {
            if (standardCards.Contains(cardType.type))
            {
                continue;
            }
            int numberInDeck = cardType.numberInDeck;
            for (int i = 0; i < numberInDeck; i++)
            {
                tacticCardDeck.Push(cardType);
            }
        }

        Shuffle<TacticCard>(tacticCardDeck);
        Shuffle<TacticCard>(tacticCardDeck);
        //foreach(TacticCard card in tacticCardDeck)
        //   {
        //       Debug.Log(card.type.ToString());
        //   }

    } 


   public static void initalizeCultureCards()
    {
        App app = UnityEngine.Object.FindObjectOfType<App>();
        string scenario = app.GetScenario();
        string culturePath = Application.dataPath +
            "/StreamingAssets/Scenarios/" + scenario + "/CultureCards";
        string[] techFiles = Directory.GetFiles(culturePath, "*.json");
        foreach (string file in techFiles)
        {
            string dataAsJson = File.ReadAllText(file);
            var newCult = Newtonsoft.Json.JsonConvert.DeserializeObject<CultureCard>(dataAsJson);
            cultureCardTypes.Add(newCult);
        }
        foreach (CultureCard cardType in cultureCardTypes)
        {
            for (int i = 0; i < 4; i++)
            {
                if (cardType.getEra() == MyEnum.Era.Early)
                {
                 //   Debug.Log("Adding: " + cardType.getCardName() + " to Early" );
                    earlyCultureCardDeck.Push(cardType);
                }
                if(cardType.getEra() == MyEnum.Era.Middle)
                {
              //      Debug.Log("Adding: " + cardType.getCardName() + " to Middle");
                    midCultureCardDeck.Push(cardType);
                }
                if(cardType.getEra() == MyEnum.Era.Late)
                {
                  //  Debug.Log("Adding: " + cardType.getCardName() + " to Late");
                    lateCultureCardDeck.Push(cardType);
                }
            }
        }
        Shuffle<CultureCard>(earlyCultureCardDeck);
        Shuffle<CultureCard>(midCultureCardDeck);
        Shuffle<CultureCard>(lateCultureCardDeck);

    }

    public static void Shuffle<T>(this Stack<T> stack)
    {
        System.Random rnd = new System.Random();

        var values = stack.ToArray();
        stack.Clear();
        foreach (var value in values.OrderBy(x => rnd.Next()))
            stack.Push(value);
    }

    public static void ListShuffle<T>(this List<T> list)
    {
        System.Random rnd = new System.Random();

        var values = list.ToArray();
        list.Clear();
        foreach (var value in values.OrderBy(x => rnd.Next()))
            list.Add(value);
    }

    public static void setCurrentSelectedNationDiplomacy(int value)
    {
        currentlySelectedNationDiplomacy = value;
    }

    public static int getCurrentSlectedNationDiplomacy()
    {
        return currentlySelectedNationDiplomacy;
    }

    public static void setCurrentlySelectedProvince(int provIndex)
    {
        currentlySelectedProvince = provIndex;
    }

    public static int getCurrentlySelectedProvince()
    {
        return currentlySelectedProvince;
    }


    public static void refillTurnOrder()
    {
        foreach(Nation nation in nations.Values)
        {
                turnOrder.Add(nation.getIndex());
        }
        turnOrder = shufflePlayerOrder(turnOrder);
        currentTurnOrderIndex = 0;
        currentPlayer = turnOrder[0];
        Debug.Log("Current Player Index is " + currentPlayer);
    }

    private static List<int> shufflePlayerOrder(List<int> list)
    {
        ListShuffle<int>(list);
        return list;

    }

    public static Nation getNextPlayer()
    {
        int nextIndex = turnOrder[currentTurnOrderIndex];
        Nation nextNation = nations[nextIndex];
        currentPlayer = nextIndex;
        Debug.Log("Current player Index: " + nextIndex);
        currentTurnOrderIndex ++;
        return nextNation;
    }

    public static List<int> getTurnOrder()
    {
        return turnOrder;
    }

    public static void advanceGamePhase()
    {
        refillTurnOrder();
        if (gamePhase == MyEnum.GamePhase.adminstration)
        {
            gamePhase = MyEnum.GamePhase.trade;
        }
        if(gamePhase == MyEnum.GamePhase.trade)
        {
            gamePhase = MyEnum.GamePhase.auction;
        }
        if(gamePhase == MyEnum.GamePhase.auction)
        {
            gamePhase = MyEnum.GamePhase.movement;
        }
        if(gamePhase == MyEnum.GamePhase.movement)
        {
           // turn += 1;
            gamePhase = MyEnum.GamePhase.end;
        }
        if (gamePhase == MyEnum.GamePhase.end)
        {
            // turn += 1;
            gamePhase = MyEnum.GamePhase.adminstration;
        }
    }

    public static AuctionHandler getAuctionHandler()
    {
        return auctionHandler;
    }

    public static TradeHandler getTradeHandler()
    {
        return tradeHandler;
    }

    public static int getCurrentTurnOrderIndex()
    {
        return currentTurnOrderIndex;
    }


    public static void initializeColonyOrder()
    {
        foreach(Nation nation in nations.Values)
        {
            if(nation.getType() == MyEnum.NationType.oldMinor)
            {
                colonyOrder.Add(nation.getIndex());
            }
        }
        ListShuffle<int>(colonyOrder);
    }

    public static void initializeShereOrder()
    {
        foreach (Nation nation in nations.Values)
        {
            if (nation.getType() == MyEnum.NationType.minor)
            {
                sphereOrder.Add(nation.getIndex());
            }
        }
        ListShuffle<int>(sphereOrder);
    }

    public static  List<int> getColonyOrder()
    {
        return colonyOrder;
    }

    public static List<int> getSphereOrder()
    {
        return sphereOrder;
    }

    public static void setCurrentPlayer(int currentIndex)
    {
        currentPlayer = turnOrder[currentIndex];
    }

    public static void setCurrentTurnOrderIndex(int index)
    {
        currentTurnOrderIndex = index;
    }
}
