using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using assemblyCsharp;
using EasyUIAnimator;
using WorldMapStrategyKit;


public class TurnHandler : MonoBehaviour
{
   public RailPlacer railPlacer;

   public void processTurnAdmin()
    {
        Debug.Log("Begin Process Turn Admin");
        // State.setCurrentTurnOrderIndex(0);
        State.setCurrentPlayer(0);
        // MyEnum.GamePhase currentPhase = State.GetPhase();
        App app = UnityEngine.Object.FindObjectOfType<App>();
        int playerIndex = app.GetHumanIndex();
        Nation humanPlayer = State.getNations()[playerIndex];
        State.currentPlayer = 0;
        Nation currentPlayer = State.getNations()[0];
        // PlayerCollector.collectForPlayer(player);

        // Debug.Log("Count of getTurnOrder: " + State.getTurnOrder().Count);
        bool first = true;
        while (currentPlayer.getIndex() != 0 || first == true )
        {
            first = false;
           // Debug.Log("Current Turn Order Index: " + State.currentTurnOrderIndex);
         //   Debug.Log("Current player is index: " + currentPlayer.getIndex());
            if (humanPlayer.getIndex() == currentPlayer.getIndex())
            {
                currentPlayer = State.getNextPlayer();
            }
           // EarlyGameAdminstration.mainEarlyAdminLoop(currentPlayer);
           //------------------------------------------------------------------
           currentPlayer.getAI().processAdminstrationPhase(currentPlayer);
            // PlayerProcessor.processSignals(currentPlayer);
          //  maintenancePayer.payMaintenance(currentPlayer);
            PlayerCollector.collectForPlayer(currentPlayer);
            currentPlayer = State.getNextPlayer();
           Debug.Log("New current player is: " + currentPlayer.getName() +
                " " + currentPlayer.getIndex());
           
        Queue<int> newRailroads = currentPlayer.getAI().NewRailRoads;
        while(newRailroads.Count > 0)
            {
                int nextProvIndex = newRailroads.Dequeue();
                assemblyCsharp.Province prov = State.getProvinces()[nextProvIndex];
                railPlacer.drawRailroadOnMap(prov, currentPlayer);
            }
            
            
        }

     

      //  State.advanceGamePhase();
        Debug.Log(State.GetPhase());
     
        return;

    }


    public  MyEnum.GamePhase ProcessTurn()
    {
        MyEnum.GamePhase phase = State.GetPhase();
        Debug.Log("Current Phase: " + State.GetPhase().ToString());
        

        if (State.currentPlayer < State.getNations().Count)
        {
            App app = UnityEngine.Object.FindObjectOfType<App>();
            int humanIndex = app.GetHumanIndex();
            Nation human = State.getNations()[humanIndex];
            Nation currentPlayer = State.getNextPlayer();
            Debug.Log("Current Player is " + currentPlayer.getNationName());
            if (currentPlayer.getIndex() == humanIndex && phase != MyEnum.GamePhase.trade)
            {
                Debug.Log("Player is Human");
                return phase;
            }
            else
            {
                Debug.Log("Player is AI");
                if (phase == MyEnum.GamePhase.adminstration)
                {
                      currentPlayer.getAI().processAdminstrationPhase(currentPlayer);
                    //EarlyGameAdminstration.mainEarlyAdminLoop(currentPlayer);
                    currentPlayer = State.getNextPlayer();
                }
                else if (phase == MyEnum.GamePhase.trade)
                {
                    //  State.getTradeHandler().handleTrades();
                    currentPlayer = State.getNextPlayer();
                    return phase;
                }
                else if (phase == MyEnum.GamePhase.auction)
                {
                    // acutionControlFlow(currentPlayer, phase);
                    //currentPlayer = State.getNextPlayer();
                    

                }
                else if (phase == MyEnum.GamePhase.events)
                {
                    updateGameMap();
                    if (currentPlayer.getIndex() == humanIndex)
                    {
                        return phase;
                    }
                    else
                    {
                        currentPlayer.getAI().processMovements();
                        currentPlayer = State.getNextPlayer();
                    }
                }
                else if (phase == MyEnum.GamePhase.end)
                {
                    State.advanceGamePhase();
                    return phase;
                }
            }
        }
        else
        {
            State.advanceGamePhase();
            if (State.GetPhase() == MyEnum.GamePhase.adminstration)
            {
                //  State.TurnIncrement();
                // State.refillTurnOrder();
                State.setCurrentPlayer(0);
            }
            //State.setCurrentTurnOrderIndex(0);
            return phase;
        }
        return phase;
    }


    private void updateGameMap()
    {
        WMSK map = WMSK.instance;
        foreach(Nation nation in State.getNations().Values)
        {
            if(nation.IsColonyOf() >= 0)
            {
                Nation major = State.getNations()[nation.IsColonyOf()];
                Color color = major.getColor();
                nation.setColor(color);
                //Colonies taken from Old Empires will be handled differently.
            }
            Queue<int> newRailroads = nation.getAI().NewRailRoads;
            while(newRailroads.Count > 0)
            {
                int provIndex = newRailroads.Dequeue();
                assemblyCsharp.Province prov = State.getProvinces()[provIndex];
                railPlacer.drawRailroadOnMap(prov, nation);
            }
        }
    }


   










}
