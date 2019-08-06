using assemblyCsharp;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Diplomatic  {



        //-1 refuse and Insulted,  0 = refuse, 1 = accept, 2 = accepted and flattered
        // Needs to be fixed!!!!!!!!!!!!!!
    public MyEnum.diploIntrepretation respondToDealOffer(Nation player, Deal deal)
    {
        App app = UnityEngine.Object.FindObjectOfType<App>();
        int humanIndex = app.GetHumanIndex();
        Nation human = State.getNations()[humanIndex];

        int relation = player.Relations[humanIndex];

        int playerMoneyOffer = deal.getPlayerMoneyOffer();
        Debug.Log("Player Money Offer: " + playerMoneyOffer);
        List<int> playerClaimOffers =  deal.getPlayerClaimOffers();
        List<int> playerColonyOffers = deal.getPlayerColonyOffers();
        List<int> playerSphereOffers = deal.getPlayerSphereOffers();
        List<int> playerBoycottOffers = deal.getPlayerBoycottOffers();

        float valueReceived = getSumOfDealValues(player, playerMoneyOffer, playerClaimOffers, playerColonyOffers, playerSphereOffers);

        float boycottPart = evaulateBoycottItems(player, playerBoycottOffers, false);

        valueReceived += boycottPart;


        int aiMoneyOffer = deal.getAI_MoneyOffer();
        List<int> aiClaimOffers = deal.getAI_ClaimOffers();
        List<int> aiColonyOffers = deal.getAI_ColonyOffers();
        List<int> aiSphereOffers = deal.getAI_SphereOffers();
        List<int> aiBoycottOffers = deal.getAI_BoycottOffers();

        float valueGiven = getSumOfDealValues(player, aiMoneyOffer, aiClaimOffers, aiColonyOffers, aiSphereOffers);

        float boycottPartGiven = evaulateBoycottItems(player, aiBoycottOffers, true);

        valueGiven += boycottPartGiven;

        Debug.Log("Value Given: " + valueGiven);
        Debug.Log("Value Reveived" + valueReceived);


        //check if it is a gift
        if (valueGiven == 0 && valueReceived > 2)
        {
            player.adjustRelation(human, (int)(valueGiven * 2));

            Debug.Log("Here");
            return MyEnum.diploIntrepretation.gift;
        }

        // check if it is a demand
        if (valueReceived == 0 && valueGiven > 0)
        {
            player.adjustRelation(human, -10);
            Debug.Log("Here");
            return MyEnum.diploIntrepretation.demand;
        }
        
        // if relations are poor and we are offering anything at all 
        if(relation < 28 && valueGiven > 0)
        {
            Debug.Log("Here");
            return MyEnum.diploIntrepretation.badDeal;
        }

        // relations are normal and value given is not much better than value received
        if (valueGiven >= valueReceived * 1.1 && relation < 72)
        {
            Debug.Log("Here");
            return MyEnum.diploIntrepretation.badDeal;
        }

        // if AI is giving more than it receives
        if(valueGiven > valueReceived)
        {
            Debug.Log("Here");
            return MyEnum.diploIntrepretation.badDeal;
        }
        Debug.Log("Here");

        return MyEnum.diploIntrepretation.goodDeal;
    }


    private float getColonySphereValue(Nation player, int minorIndex)
    {
        AI ai = player.getAI();

        float value = 0;
        Nation item = State.getNations()[minorIndex];
        Debug.Log("Item: " + item.getName());
        foreach(int index in player.getProvinces())
        {
            Province prov = State.getProvince(index);
            MyEnum.Resources res = prov.getResource();
            value +=  player.getAI().GetTopLevel().getResPriority(player, res);
        }
        Debug.Log("Sphere/Colony Value: " + value);
        HashSet<int> preferredSpheres = ai.SpherePreferences;
        if (preferredSpheres.Contains(minorIndex))
        {
            value += 1;
        }
        return value;
    }

    private float getSumOfDealValues(Nation player, int gold,  List<int> claims, List<int> colonies, List<int> spheres)
    {
        float value = 0;
        for(int i = 0; i < colonies.Count; i++)
        {
            float add = getColonySphereValue(player, colonies[i]) * 1.5f;
            value += add;
        }

        for (int i = 0; i < spheres.Count; i++)
        {
            float add = getColonySphereValue(player, spheres[i]);
            value += add;
        }

        for(int i = 0; i < claims.Count; i++)
        {
            int provIndex = claims[i];
            Province prov = State.getProvinces()[provIndex];
            MyEnum.Resources res = prov.getResource();
            value += player.getAI().GetTopLevel().getResPriority(player, res);
        }

  
        int turn = State.turn;
        value += gold * 0.05f;
        //Debug.Log("Total Value of Offer: " + value);
        return value;
    }


    private float evaulateBoycottItems(Nation player, List<int> boycotts, bool offer)
    {
        float value = 0;
        // the AI would be making the Boyoctt
        if(offer == true)
        {
            foreach(int item in boycotts)
            {
                //Nation nation = State.getNations()[item];
                int relation = player.Relations[item];
                if(relation < 20)
                {
                    value += 0.1f;
                }
                else if(relation < 30)
                {
                    value += 0.5f;
                }
                else if(relation < 40)
                {
                    value += 1;
                }
                else if(relation < 50)
                {
                    value += 2;
                }
                else if(relation < 60)
                {
                    value += 3;
                }
                else if(relation < 70)
                {
                    value += 4;
                }
                else
                {
                    value += 8;
                }
            }
        }
        if (offer == false)
        {
            foreach (int item in boycotts)
            {
                int relation = player.Relations[item];
                if (relation < 20)
                {
                    value += 2;
                }
                else if (relation < 30)
                {
                    value += 1f;
                }
                else if (relation < 40)
                {
                    value += 0.5f;
                }
                else if (relation < 50)
                {
                    value += 0.25f;
                }

            }
        }
        return value;
    }

    




}
