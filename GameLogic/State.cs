using System.Collections;
using System.Collections.Generic;
using assemblyCsharp;
using WorldMapStrategyKit;
using System;
using System.Linq;
using System.IO;
using UnityEngine;


[System.Serializable]

public static class State {


    public static bool currentPlayerIsMajor = false;
    public static int selectedOpponentPlayer = 0;

     public static MapUtilities mapUtilities = new MapUtilities();
    public static TurnLoader turnLoader = new TurnLoader();
    public static int turn = 1;
    public static int currentPlayer;
    public static int currentTurnOrderIndex = 0;
    //public static List<int> turnOrder = new List<int>();
    public static PlaceArmy armyPlacer = new PlaceArmy();
    public static Deal deal = new Deal();

    public static AuctionHandler auctionHandler = new AuctionHandler();
   public static TradeHandler tradeHandler = new TradeHandler();

    public static MyEnum.Era era;

    public static MyEnum.GamePhase gamePhase;

    private static Dictionary<int, Nation> nations = new Dictionary<int, Nation>();
    private static Dictionary<int, assemblyCsharp.Province> provinces =
        new Dictionary<int, assemblyCsharp.Province>();

    private static List<GameObjectAnimator> ResourceIcons = new List<GameObjectAnimator>();

    public static Market market = new Market();
    public static WorldBank bank = new WorldBank();
    public static EventRegister eventRegister = new EventRegister();

    public static Dictionary<string, Technology> technologies = new Dictionary<string, Technology>();
   // public static List<Relation> relations;
    //public static  List<War> wars;
    public static StatTracker history = new StatTracker();	
    public static List<TacticCard> tacticCardTypes = new List<TacticCard>();
    public static Stack<TacticCard> tacticCardDeck = new Stack<TacticCard>();
    public static List<CultureCard> cultureCardTypes = new List<CultureCard>();
    public static Stack<CultureCard> earlyCultureCardDeck = new Stack<CultureCard>();
    public static Stack<CultureCard> midCultureCardDeck = new Stack<CultureCard>();
    public static Stack<CultureCard> lateCultureCardDeck = new Stack<CultureCard>();

    private static int currentColonyAuctionBid;

    private static List<DecisionEvent> decisionEvents = new List<DecisionEvent>();

    private static List<MapChange> mapChanges = new List<MapChange>();

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

    public static int currentlySelectedNationDiplomacy = -1;
    public static int currentlySelectedProvince = -1;
    public static WorldAffairs worldAffairs = new WorldAffairs();

    public static int CurrentColonyAuctionBid { get => currentColonyAuctionBid; set => currentColonyAuctionBid = value; }
    public static List<MapChange> MapChanges { get => mapChanges; set => mapChanges = value; }

    public static void UpdateColonyAuctionBid(int value)
    {
        int current = currentColonyAuctionBid;
        if (value > currentColonyAuctionBid)
        {
            currentColonyAuctionBid = value;
        }
        else
        {
            Debug.Log("What the fuck just happened you fucker!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
        }
    }

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
            //    Debug.Log("Returning early deck");
            Debug.Log("Current size of " + era + "culture deck: " + earlyCultureCardDeck.Count);

            return earlyCultureCardDeck;

        }
        else if(era == MyEnum.Era.Middle)
        {
            //  Debug.Log("Returning middle deck");
            Debug.Log("Current size of " + era + "culture deck: " + midCultureCardDeck.Count);

            return midCultureCardDeck;


        }
        else
        {
            //  Debug.Log("Returning late deck");
            Debug.Log("Current size of " + era + "culture deck: " + lateCultureCardDeck.Count);

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
      //  Debug.Log(scenario);
        string culturePath = Application.streamingAssetsPath + "/Scenarios/" + scenario + "/CultureCards/";
        //string culturePath = Application.dataPath + 
        //   "/StreamingAssets/Scenarios/" + scenario + "/CultureCards";
        string[] cultFiles = Directory.GetFiles(culturePath, "*.json");
        foreach (string file in cultFiles)
        {
          //  Debug.Log(file);
            string dataAsJson = File.ReadAllText(file);
            var newCult = Newtonsoft.Json.JsonConvert.DeserializeObject<CultureCard>(dataAsJson);
           // Debug.Log(newCult.getCardName() + " " + newCult.getEra());
            cultureCardTypes.Add(newCult);
        }
       // Debug.Log("Number of culture cards: " + cultureCardTypes.Count);
        foreach (CultureCard cardType in cultureCardTypes)
        {
            
            if (cardType.getEra() == MyEnum.Era.Early)
            {
                for (int i = 0; i < 8; i++)
                {
                 //   Debug.Log("Adding: " + cardType.getCardName() + " to Early");
                    earlyCultureCardDeck.Push(cardType);
                }
            }
            else if(cardType.getEra() == MyEnum.Era.Middle)
            {
                for(int i = 0; i < 8; i++)
                {
               //     Debug.Log("Adding: " + cardType.getCardName() + " to Middle");
                    midCultureCardDeck.Push(cardType);
                }
                  
            }
            else if(cardType.getEra() == MyEnum.Era.Late)
            {
                for (int i = 0; i < 8; i++)
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


  /*  public static void refillTurnOrder()
    {
        foreach(Nation nation in nations.Values)
        {
                turnOrder.Add(nation.getIndex());
        }
        turnOrder = shufflePlayerOrder(turnOrder);
        currentTurnOrderIndex = 0;
        currentPlayer = turnOrder[0];
        Debug.Log("Current Player Index is " + currentPlayer);
    }  */

    private static List<int> shufflePlayerOrder(List<int> list)
    {
        ListShuffle<int>(list);
        return list;

    }
 

    public static Nation getNextPlayer()
    {
        Debug.Log("Current Player: " + currentPlayer);
        //Rewritten to simply go through player by index (for now) 
        if (currentPlayer < nations.Count-1)
        {
            currentPlayer ++ ;
        }
        else
        {
            currentPlayer = 0;
        }
        // int nextIndex = turnOrder[currentTurnOrderIndex];
       // Debug.Log("Current Player: " + currentPlayer);
       // Debug.Log("Number of players: " + nations.Count());
        Nation nextNation = nations[currentPlayer];
       // Debug.Log("Current player Index: " + nextIndex);
       // currentTurnOrderIndex ++;
        return nextNation;
    }

    /*public static List<int> getTurnOrder()
    {
        return turnOrder;
    } */

    public static void advanceGamePhase()
    {
        Debug.Log("^^^^^^^^^^^^^^ Current Game Phase: " + gamePhase + " ^^^^^^^^^^^^^^^^^^^^");

       // refillTurnOrder();
        if (gamePhase == MyEnum.GamePhase.adminstration)
        {
            gamePhase = MyEnum.GamePhase.trade;
        }
        else if(gamePhase == MyEnum.GamePhase.trade)
        {
            gamePhase = MyEnum.GamePhase.auction;
        }
        else if(gamePhase == MyEnum.GamePhase.auction)
        {
            gamePhase = MyEnum.GamePhase.events;
        }
       else  if(gamePhase == MyEnum.GamePhase.events)
        {
           // turn += 1;
            gamePhase = MyEnum.GamePhase.end;
        }
       else  if (gamePhase == MyEnum.GamePhase.end)
        {
            turn += 1;
            Debug.Log("^^^^^^^^^^^^^^ New Game Turn: " + turn + " ^^^^^^^^^^^^^^^^^^^^");
           
            gamePhase = MyEnum.GamePhase.adminstration;
        }
        Debug.Log("^^^^^^^^^^^^^^ Advancing Game Phase: " + gamePhase + " ^^^^^^^^^^^^^^^^^^^^");

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


 
    public static void setCurrentPlayer(int currentIndex)
    {
        currentPlayer = currentIndex;
    }

    /*public static void setCurrentTurnOrderIndex(int index)
    {
        currentTurnOrderIndex = index;
    } */

    public static TurnLoader getTurnLoader()
    {
        return turnLoader;
    }


    public static int getNumberOfMajorNations()
    {
        int count = 0;
        for(int i = 0; i < nations.Count; i++)
        {
            Nation nation = nations[i];
            if(nation.getType() == MyEnum.NationType.major)
            {
                count++;
            }
        }
        return count;
    }

    public static List<int> getMajorNations()
    {
        List<int> majors = new List<int>();
        for (int i = 0; i < nations.Count; i++)
        {
            Nation nation = nations[i];
            if (nation.getType() == MyEnum.NationType.major)
            {
                majors.Add(nation.getIndex());
            }
        }
        return majors;
    }

    public static List<int> getMinorNations()
    {
        List<int> minors = new List<int>();
        for (int i = 0; i < nations.Count; i++)
        {
            Nation nation = nations[i];
            if (nation.getType() == MyEnum.NationType.minor)
            {
                minors.Add(nation.getIndex());
            }
        }
        return minors;
    }

    public static void clearState()
    {
        nations.Clear();
        provinces.Clear();
        ResourceIcons.Clear();
        technologies.Clear();
        earlyCultureCardDeck.Clear();
        midCultureCardDeck.Clear();
        lateCultureCardDeck.Clear();
    }

    public static assemblyCsharp.Province getProvince(int index)
    {
        return provinces[index];
    }

    public static Nation getNation(int index)
    {
      
        return nations[index];
    }



    public static void addDecisionEvent(DecisionEvent decision)
    {
        decisionEvents.Add(decision);
    }

    public static DecisionEvent getDecisionEvent(int index)
    {
        return decisionEvents[index];
    }

    public static int numberOfDecisionevents()
    {
        return decisionEvents.Count;
    }


}

