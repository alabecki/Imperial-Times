using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using assemblyCsharp;
using System.Linq;
using System;


[System.Serializable]
public class StatTracker
{

    private Dictionary<int, List<int>> prestigeScore = new Dictionary<int, List<int>>();
    private Dictionary<int, List<int>> industrialScore = new Dictionary<int, List<int>>();
    private Dictionary<int, List<int>> militaryScore = new Dictionary<int, List<int>>();


    private List<int> prestigeRanking = new List<int>();
    private List<int> industryRanking = new List<int>();
    public List<int> militaryRanking = new List<int>();




    public void addPresitgeScore(int nation, int score, int turn)
    {
        this.prestigeScore[nation][turn] = score;
    }

    public  void addIndustrialScore(int nation, int score, int turn)
    {
        industrialScore[nation][turn] = score;
    }

    public  void addMilitaryScore(int nation, int score, int turn)
    {
        militaryScore[nation][turn] = score;
    }

    public  List<int> getPrestigeHistory(int nation)
    {
        return prestigeScore[nation];
    }

    public  List<int> getIndustrialHistory(int nation)
    {
        return industrialScore[nation];
    }

    public  List<int> getMiliaryScore(int nation)
    {
        return militaryScore[nation];
    }

    public  int getPrestigeRanking(int thisNation)
    {
        for (int i = 0; i < prestigeRanking.Count; i++)
        {
            if (prestigeRanking[i] == thisNation)
            {
                return i + 1;
            }
        }
        return -1;
    }

    public  int getIndustrialRanking(int thisNation)
    {
        for (int i = 0; i < industryRanking.Count; i++)
        {
            if (industryRanking[i] == thisNation)
            {
                return i + 1;
            }
        }
        return -1;
    }

    public  int getMilitaryRanking(int thisNation)
    {
        for (int i = 0; i < militaryRanking.Count; i++)
        {
            if (militaryRanking[i] == thisNation)
            {
                return i + 1;
            }
        }
        return -1;
    }

    public  List<int> getMilitaryRankings()
    {
        return militaryRanking;
    }

    public  List<int> getIndustryRankings()
    {
        return industryRanking;
    }

    public  List<int> getPrestigeRankings()
    {
        return prestigeRanking;
    }

    public  void determinePrestigeRanking(int turn)
    {
        Dictionary<int, int> prestigeRankings = new Dictionary<int, int>();
        foreach (Nation nation in State.getNations().Values)
        {
            prestigeRankings[nation.getIndex()] = getPrestigeHistory(nation.getIndex())[turn];
        }
        List<int> ranking = new List<int>();
        foreach (KeyValuePair<int, int> index in prestigeRankings.OrderBy(key => key.Value))
        {
            ranking.Add(index.Key);
        }
        prestigeRanking = ranking;
    }


    public  void determineIndustryRanking(int turn)
    {
        Dictionary<int, int> industrialRankings = new Dictionary<int, int>();
        foreach (Nation nation in State.getNations().Values)
        {
            industrialRankings[nation.getIndex()] = getPrestigeHistory(nation.getIndex())[turn];
        }
        List<int> ranking = new List<int>();
        foreach (KeyValuePair<int, int> index in industrialRankings.OrderBy(key => key.Value))
        {
            ranking.Add(index.Key);
        }
        industryRanking = ranking;
    }


    public  void determineMilitaryRanking(int turn)
    {
        Dictionary<int, int> militaryRankings = new Dictionary<int, int>();
        foreach (Nation nation in State.getNations().Values)
        {
            militaryRankings[nation.getIndex()] = getPrestigeHistory(nation.getIndex())[turn];
        }
        List<int> ranking = new List<int>();
        foreach (KeyValuePair<int, int> index in militaryRankings.OrderBy(key => key.Value))
        {
            ranking.Add(index.Key);
        }
        militaryRanking = ranking;
    }







     




    



}
