using assemblyCsharp;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deal 
{
    int human;
    int ai;
    int playerOffersMoney; 
    List<int> playerRegSpheres = new List<int>();
    List<int> playerRegColonies = new List<int>();
    List<int> playerRegClaims = new List<int>();
    List<int> playerWillBoycott = new List<int>();

    int AI_OffersMoney;
    List<int> AI_RegSpheres = new List<int>();
    List<int> AI_RegColonies = new List<int>();
    List<int> AI_RegClaims = new List<int>();
    List<int> AI_WillBoycott = new List<int>();

    public void setPlayerOffersMoney(int value)
    {
        playerOffersMoney = value;
    }

    public void setAI_OffersMoney(int value)
    {
        AI_OffersMoney = value;
    }

    public void addPlayerRegSphere(int item)
    {
        playerRegSpheres.Add(item);
    }

    public void removePlayerRegSphere(int item)
    {
        playerRegSpheres.Remove(item);
    }

    public void addPlayerRegColony(int item)
    {
        playerRegColonies.Add(item);
    }

    public void removePlayerRegColony(int item)
    {
        playerRegColonies.Remove(item);
    }

    public void addPlayerRegClaim(int item)
    {
        playerRegClaims.Add(item);
    }

    public void removePlayerRegClaim(int item)
    {
        playerRegClaims.Remove(item);
    }

    public void addPlayerBoycott(int item)
    {
        playerWillBoycott.Add(item);
    }

    public void removePlayerBoycott(int item)
    {
        playerWillBoycott.Remove(item);
    }

    public void addAI_RegSphere(int item)
    {
        AI_RegSpheres.Add(item);
    }

    public void removeAI_RegSphere(int item)
    {
        AI_RegSpheres.Remove(item);
    }

    public void addAI_RegColony(int item)
    {
        AI_RegColonies.Add(item);
    }

    public void removeAI_RecColony(int item)
    {
        AI_RegColonies.Remove(item);
    }
    

    public void addAI_RegClaim(int item)
    {
        AI_RegClaims.Add(item);
    }

    public void removeAI_RegClaim(int item)
    {
        AI_RegClaims.Remove(item);
    }

    public void addAI_PlayerBoycott(int item)
    {
        AI_WillBoycott.Add(item);
    }

    public void removeAI_PlayerBoycott(int item)
    {
        AI_WillBoycott.Remove(item);
    }


        public void processDeal()
    {
        Nation aiNation = State.getNations()[ai];
        Nation humanNation = State.getNations()[human];
        int relation = humanNation.Relations[ai];
  
        //check if deal is null
        if (playerOffersMoney == 0 && playerRegSpheres.Count == 0 && playerRegColonies.Count == 0 && playerWillBoycott.Count == 0)
        {
            if(AI_OffersMoney == 0 && AI_RegSpheres.Count == 0 && AI_RegColonies.Count == 0 && AI_WillBoycott.Count == 0)
            {
                return;
            }
        }
        else if (playerOffersMoney == 0 && playerRegSpheres.Count == 0 && playerRegColonies.Count == 0 && playerWillBoycott.Count == 0)
        {
            //check if Player was making a demand then relations worsen but cannot get into war with AI for next 4 turns
            humanNation.adjustRelation(aiNation, -15);
        }
        else if (AI_OffersMoney == 0 && AI_RegColonies.Count == 0 && AI_WillBoycott.Count == 0 && AI_RegSpheres.Count == 0)
        {
            //check if Player was making a demand then relations worsen but cannot get into war with AI for next 4 turns
            humanNation.adjustRelation(aiNation, -15);
        }
        humanNation.payGold(playerOffersMoney);
        for(int i = 0; i < playerRegSpheres.Count; i++)
        {
            humanNation.RecognizingTheseClaims.Add(playerRegSpheres[i]);
        }
        for(int i = 0; i < playerRegColonies.Count; i++)
        {
            humanNation.RecognizingTheseClaims.Add(playerRegColonies[i]);
        }

    }

    public void clearDeal()
    {
      
        playerOffersMoney = 0;
        playerRegSpheres.Clear();
        playerRegColonies.Clear();
        playerRegClaims.Clear();
        playerWillBoycott.Clear();

        AI_OffersMoney = 0;
        AI_RegSpheres.Clear();
        AI_RegColonies.Clear() ;
        AI_RegClaims.Clear();
        AI_WillBoycott.Clear();

    }


    public int getAI_Nation()
    {
        return ai;
    }

    public int getPlayerMoneyOffer()
    {
        return playerOffersMoney;
    }

    public List<int> getPlayerSphereOffers()
    {
        return playerRegSpheres;
    }

    public List<int> getPlayerColonyOffers()
    {
        return playerRegColonies;
    }

    public List<int> getPlayerClaimOffers()
    {
        return playerRegClaims;
    }

    public List<int> getPlayerBoycottOffers()
    {
        return playerWillBoycott;
    }

    public int getAI_MoneyOffer()
    {
        return AI_OffersMoney;
    }

    public List<int> getAI_SphereOffers()
    {
        return AI_RegSpheres;
    }

    public List<int> getAI_ColonyOffers()
    {
        return AI_RegColonies;
    }

    public List<int> getAI_ClaimOffers()
    {
        return AI_RegClaims;
    }

    public List<int> getAI_BoycottOffers()
    {
        return AI_WillBoycott;
    }

    public void performDeal()
    {
        App app = UnityEngine.Object.FindObjectOfType<App>();
        int playerIndex = app.GetHumanIndex();
        Nation humanPlayer = State.getNations()[playerIndex];
        int aiIndex = State.getCurrentSlectedNationDiplomacy();
        Nation aiPlayer = State.getNations()[aiIndex];

        Deal deal = State.deal;

        int playerMoneyOffer = deal.getPlayerMoneyOffer();
        List<int> playerClaimOffers = deal.getPlayerClaimOffers();
        List<int> playerColonyOffers = deal.getPlayerColonyOffers();
        List<int> playerSphereOffers = deal.getPlayerSphereOffers();
        List<int> playerBoycottOffers = deal.getPlayerBoycottOffers();

        int aiMoneyOffer = deal.getAI_MoneyOffer();
        List<int> aiClaimOffers = deal.getAI_ClaimOffers();
        List<int> aiColonyOffers = deal.getAI_ColonyOffers();
        List<int> aiSphereOffers = deal.getAI_SphereOffers();
        List<int> aiBoycottOffers = deal.getAI_BoycottOffers();

        aiPlayer.receiveGold(playerMoneyOffer);
        humanPlayer.receiveGold(aiMoneyOffer);

        for (int i = 0; i < playerClaimOffers.Count; i++)
        {
           // Relation relationToReceiver = humanPlayer.getRelationToThisPlayer(aiIndex);
          //  relationToReceiver.addProvinceRecognizing(playerClaimOffers[i]);
            // aiPlayer.addRecognizedClaim(playerClaimOffers[i], humanPlayer.getIndex());
           // aiPlayer.RecognizingTheseClaims.Add(playerClaimOffers[i]);
        }

        for (int i = 0; i < playerColonyOffers.Count; i++)
        {
            // aiPlayer.addRecognizedClaim(playerColonyOffers[i], humanPlayer.getIndex());
            humanPlayer.RecognizingTheseClaims.Add(playerColonyOffers[i]);
        }

        for (int i = 0; i < playerSphereOffers.Count; i++)
        {
            Nation nation = State.getNations()[playerSphereOffers[i]];
            Debug.Log("Now Recognizing: " + nation.getName() + " " + nation.getIndex());
            humanPlayer.RecognizingTheseClaims.Add(playerSphereOffers[i]);
        }

        for(int i = 0; i < playerBoycottOffers.Count; i++)
        {
            humanPlayer.addBoycott(playerBoycottOffers[i]);
            Nation otherNation = State.getNations()[playerBoycottOffers[i]];
            humanPlayer.adjustRelation(otherNation, -15);
        }


        for (int i = 0; i < aiClaimOffers.Count; i++)
        {
           // Relation relationToReceiver = aiPlayer.getRelationToThisPlayer(playerIndex);
          //  relationToReceiver.addProvinceRecognizing(aiClaimOffers[i]);
         //   aiPlayer.RecognizingTheseClaims.Add(playerClaimOffers[i]);
        }

        for (int i = 0; i < aiColonyOffers.Count; i++)
        {
            Nation nation = State.getNations()[aiColonyOffers[i]];

            Debug.Log("Now Recognizing Colony: " + nation.getName() + " " + nation.getIndex());

            aiPlayer.RecognizingTheseClaims.Add(aiColonyOffers[i]);
        }

        for (int i = 0; i < aiSphereOffers.Count; i++)
        {
            Nation nation = State.getNations()[aiSphereOffers[i]];
            Debug.Log("Now Recognizing: " + nation.getName() + " " + nation.getIndex());

            aiPlayer.RecognizingTheseClaims.Add(aiSphereOffers[i]);    
        }


        for (int i = 0; i < aiBoycottOffers.Count; i++)
        {
            aiPlayer.addBoycott(aiBoycottOffers[i]);
            Nation otherNation = State.getNations()[aiBoycottOffers[i]];
            aiPlayer.adjustRelation(otherNation, -15);
        }



        Debug.Log("Performing Deal");


    }


}
