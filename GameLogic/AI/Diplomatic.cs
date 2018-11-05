using assemblyCsharp;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Diplomatic  {


    private int aggressiveLevel;
    private int realLoyality;

    private int claimFactor = 3;
    private int attitudeFactor = 3;


    private List<int> HitList = new List<int>();
    private List<int> BefriendList = new List<int>();


    private void updateHitList(Nation player)
    {
        List<WarClaim> claims = player.getWarClaims();
        Dictionary<int, float> claimStrength = new Dictionary<int, float>();
        foreach (WarClaim claim in claims)
        {
            if (!claimStrength.Keys.Contains(claim.getOtherNation())) {
                claimStrength[claim.getOtherNation()] = 0;
            }
        }
        foreach (WarClaim claim in claims)
        {
            claimStrength[claim.getOtherNation()] += claim.getValue();
        }

        float maxClaim = claimStrength.Values.ToList().Max();
        float ratio = 100f / maxClaim;

        Dictionary<int, float> relationFactor = new Dictionary<int, float>();
        Dictionary<int, float> newHitList = new Dictionary<int, float>();

        foreach (int key in claimStrength.Keys.ToList())
        {
            Nation otherPlayer = State.getNations()[key];
            //First - assign to each player the sum of the strength of all your war claims
            //(normalized and weighted)- this 
            //represents how much you stand to gain from conflict with this player
            newHitList[key] = claimStrength[key] * ratio * claimFactor;
            //now - subtract your current relations (divided by x, depending on Loyality)

            if (player.getAI().GetTopLevel().GetLoyality() == MyEnum.Loyality.low)
            {
                newHitList[key] = newHitList[key] - (player.getRelationFromThisPlayer(key).getAttitude() / 160);

            }
            if (player.getAI().GetTopLevel().GetLoyality() == MyEnum.Loyality.medium)
            {
                newHitList[key] = newHitList[key] - (player.getRelationFromThisPlayer(key).getAttitude() / 80);

            }
            if (player.getAI().GetTopLevel().GetLoyality() == MyEnum.Loyality.medium)
            {
                newHitList[key] = newHitList[key] - (player.getRelationFromThisPlayer(key).getAttitude() / 40);

            }
            //Modify by difference in military rank
            newHitList[key] = newHitList[key] +
                (State.history.getMilitaryRanking(player.getIndex() - State.history.getMilitaryRanking(key))) / 7;
            newHitList[key] = newHitList[key] - otherPlayer.Reputation / 100;
        }
        var ordered = newHitList.OrderByDescending(x => x.Value);
        int index = 0;
        foreach (var item in ordered)
        {
            this.HitList[index] = item.Key;
            index++;
        }
    }



    public void updateBefriendList(Nation player)
    {
        Dictionary<int, float> newBefriendList = new Dictionary<int, float>();

        foreach (KeyValuePair<int, Nation> nation in State.getNations())
        {
            if (player.getAI().GetTopLevel().GetLoyality() == MyEnum.Loyality.low)
            {
                newBefriendList[nation.Key] =
                    player.getRelationToThisPlayer(nation.Value.getIndex()).getAttitude() * 1.5f;
            }
            if (player.getAI().GetTopLevel().GetLoyality() == MyEnum.Loyality.medium)
            {
                newBefriendList[nation.Key] =
                    player.getRelationToThisPlayer(nation.Value.getIndex()).getAttitude() * 2.5f;
            }
            if (player.getAI().GetTopLevel().GetLoyality() == MyEnum.Loyality.medium)
            {
                newBefriendList[nation.Key] =
                    player.getRelationToThisPlayer(nation.Value.getIndex()).getAttitude() * 5f;
            }
        }
        for (int i = 0; i < this.HitList.Count; i++)
        {
            if(i > 4)
            {
                break;
            }
            newBefriendList[HitList[i]] -= ((5 - i) / 2.5f);
        }
        foreach (KeyValuePair<int, float> item in newBefriendList)
        {
            Nation otherPlayer = State.getNations()[item.Key];

            newBefriendList[item.Key] -= ((100 - otherPlayer.getReliability()) * 0.02f);
            newBefriendList[item.Key] += ((3.5f - State.history.getMilitaryRanking(item.Key)) / 2);
        }
        var ordered = newBefriendList.OrderByDescending(x => x.Value);
        int index = 0;
        foreach (var item in ordered)
        {
            this.BefriendList[index] = item.Key;
            index++;
        }



    }

    public List<int> getHitlist()
    {
        return this.HitList;
    }

    public List<int> getBefrientList()
    {
        return this.BefriendList;
    }

    public string responceToAllianceRequest(Nation Humanplayer, int AIIndex)
    {
        Debug.Log("Checking...");
        string response = " ";
        Relation relationTo = Humanplayer.getRelationToThisPlayer(AIIndex);
        Relation relationFrom = Humanplayer.getRelationFromThisPlayer(AIIndex);
        Nation otherNation = State.getNations()[AIIndex];
        int numMajors = getNumberOfMajorsNations();

        if (relationTo.isDefensiveAlliance() == false && relationTo.isFullAlliance() == false)
        {
            Debug.Log("Checking...");

            response = "Splendid! We've been contemplating just such an alliance ourselves, but we were too shy to ask" +
                "We pledge to come to the aid of our brothers in sisters in " + Humanplayer.getNationName() +
            "against aggressive powers and trust that they will" +
            "do the same for us!";
            //Must check if other player can have another defensive allisnce
            if (currentNumberOfDefenseiveAlliances(otherNation) > (numMajors - 1) / 2)
            {
                Debug.Log("Checking...");

                response = "We are afraid that we are already " +
                    "engaged in enough alliances at this time.  ";
            }
            Debug.Log("Checking...");
            // int othersTarget = -1;
            if (otherNation.getAI().GetDiplomatic().getHitlist().Count > 0)
            {
                int othersTarget = otherNation.getAI().GetDiplomatic().getHitlist()[0];

                if (othersTarget == Humanplayer.getIndex())
                {
                    Debug.Log("Checking...");

                    response = "We regret to inform you that it is not " +
                    "in our best interest to form such an alliance at this time.";
                }


                if (checkIfAlliedWith(Humanplayer, othersTarget))
                {
                    Debug.Log("Checking...");

                    string targetName = State.getNations()[othersTarget].getNationName();
                    response = "Unfortunately, your alliance with " + targetName +
                       " makes it impossible to entertain an alliance with you " +
                        "at this time.";
                }
            }
            if (Humanplayer.getReliability() < 35)
            {
                Debug.Log("Checking...");

                response = "We would be foolish to join an alliance with " +
                    "a petulant coward such as yourself yourself!";
            }
            if (checkIfReallyWantToBefriend(otherNation, Humanplayer.getIndex()) == false)
            {
                Debug.Log("Checking...");

                response = "Don't take this personally " +
                    "but we just don't really like you enough " +
                    "to make this sort of commitment. Let's just stay friends. Yeah... friends.";
            }
            Debug.Log("Checking...");

            return response;
        }
        if (relationTo.isDefensiveAlliance() == true)
        {
            //Must check if other player wants to upgrade to full alliance
            //Cannot have full alliance with more than 50% of other players
            return response;
        }
        return "Something went wrong";
    }

    private bool checkIfReallyWantToBefriend(Nation player, int self)
    {
        int numMajors = getNumberOfMajorsNations();
        bool result = false;
        List<int> BefriendList = player.getAI().GetDiplomatic().getBefrientList();
        for (int i = 0; i < BefriendList.Count; i++)
        {
            if (i > (numMajors / 2) + 1)
            {
                return result;
            }
            if (BefriendList[i] == self)
            {
                result = true;
            }
        }
        return result;
    }

    private bool checkIfAlliedWith(Nation player, int other)
    {
        bool result = false;
        if (player.getRelations()[other].isDefensiveAlliance() ||
            player.getRelations()[other].isFullAlliance())
        {
            result = true;
        }
        return result;
    }

    private int getNumberOfMajorsNations()
    {
        int majorCount = 0;
        foreach (Nation nation in State.getNations().Values)
        {
            if (nation.getType() == MyEnum.NationType.major)
            {
                majorCount += 1;
            }
        }
        return majorCount;
    }

    private int currentNumberOfDefenseiveAlliances(Nation nation)
    {
        int allianceCount = 0;
        foreach (Relation relation in nation.getRelations().Values)
        {
            if (relation.isDefensiveAlliance())
            {
                allianceCount += 1;
            }
        }
        return allianceCount;
    }





}
