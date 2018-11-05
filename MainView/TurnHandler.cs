using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using assemblyCsharp;
using EasyUIAnimator;
using WorldMapStrategyKit;

public static class TurnHandler
{


   public static void processTurnAdmin()
    {
        MyEnum.GamePhase currentPhase = State.GetPhase();
        App app = UnityEngine.Object.FindObjectOfType<App>();
        int playerIndex = app.GetHumanIndex();
        Nation player = State.getNations()[playerIndex];
        State.currentPlayer = State.getTurnOrder()[State.currentTurnOrderIndex];
        Nation currentPlayer = State.getNextPlayer();
        Debug.Log("Current player is: " + currentPlayer.getIndex());
        // PlayerCollector.collectForPlayer(player);
        PlayerProcessor.processSignals(player);
        maintenancePayer.payMaintenance(player);
        PlayerCollector.collectForPlayer(player);

        while (State.getCurrentTurnOrderIndex() < State.getTurnOrder().Count)
        {
            if (player.getIndex() == currentPlayer.getIndex())
            {
                currentPlayer = State.getNextPlayer();

                continue;
            }
            else
            {
                currentPlayer.getAI().processAdminstrationPhase(currentPlayer);
                PlayerProcessor.processSignals(currentPlayer);
                maintenancePayer.payMaintenance(currentPlayer);
                PlayerCollector.collectForPlayer(currentPlayer);
                currentPlayer = State.getNextPlayer();
            }
        }
        State.advanceGamePhase();
        return;

    }


    public static MyEnum.GamePhase ProcessTurn()
    {
        MyEnum.GamePhase phase = State.GetPhase();
        Debug.Log("Current Phase: " + State.GetPhase().ToString());


        if (State.getCurrentTurnOrderIndex() < State.getTurnOrder().Count)
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
                else if (phase == MyEnum.GamePhase.movement)
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
                State.turn += 1;
                State.refillTurnOrder();
            }
            State.setCurrentTurnOrderIndex(0);
            return phase;
        }
        return phase;

 


    }


    private static void updateGameMap()
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
        }

    }





}
