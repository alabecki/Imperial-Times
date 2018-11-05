using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using assemblyCsharp;
using System.Linq;
using System;

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



    public  int CalculateIndustrialScore(Nation player)
    {
        float industrialScore =0;
        foreach(int provIndex in player.getAllProvinceIndexes())
        {
            Province prov = State.getProvinces()[provIndex];
            industrialScore += prov.getInfrastructure();
        }
        foreach (MyEnum.Goods good in System.Enum.GetValues(typeof(MyEnum.Goods)))
        {
            industrialScore += player.industry.getFactoryLevel(good) * 1.25f;
        }
        industrialScore += player.GetShipyardLevel();
        return (int)Math.Floor(industrialScore);
    }

    public int CaululateArmyScore(Nation player)
    {
        float militaryScore = 0;
        float infantryMod = 0.6f;
        float cavalryMod = 0.75f;
        float fighterMod = 1.0f;
        if (player.GetTechnologies().Contains("flintlock"))
        {
            infantryMod = 1f;
            cavalryMod = 1.1f;
        }
        if (player.GetTechnologies().Contains("breech_loaded_arms"))
        {
            infantryMod = 1.2f;
            cavalryMod = 1.2f;

        }
        if (player.GetTechnologies().Contains("machine_guns"))
        {
            infantryMod = 1.5f;
            cavalryMod = 1.3f;
        }
        if (player.GetTechnologies().Contains("bolt_action_rifles"))
        {
            infantryMod = 1.8f;
            cavalryMod = 1.4f;

        }

        float artMod = 1.0f;
        float tankMod = 2.0f;
        if (player.GetTechnologies().Contains("breech_loaded_arms"))
        {
            artMod = 1.15f;
        }
        if (player.GetTechnologies().Contains("indirect_fire"))
        {
            artMod = 1.45f;
        }
        if (player.GetTechnologies().Contains("heavy_armament"))
        {
            artMod = 1.8f;
            tankMod = 1.22f;
        }

        if (player.GetTechnologies().Contains("bombers"))
        {
            fighterMod = 1.25f;
        }

        if (player.GetTechnologies().Contains("radar"))
        {
            fighterMod = 1.5f;

        }

        Debug.Log("Num of Armies: " + player.GetArmies().Count.ToString());

        foreach (Army army in player.GetArmies())
        {
            Debug.Log("Num Inf: " + army.GetInfantry());
            Debug.Log("Num Cav: " + army.GetCavalry());

            militaryScore += army.GetInfantry() * infantryMod;
            militaryScore += army.GetArtillery() * artMod * 1.33f;
            militaryScore += army.GetCavalry() * cavalryMod * 1.22f;
            militaryScore += army.GetTank() * tankMod * 3.5f;
            militaryScore += army.GetFighter() * fighterMod * 2.8f;
        }
        militaryScore += player.getArmyLevel() * 2;
        Debug.Log("Score " + militaryScore.ToString());
        Debug.Log("Divided Score " + (militaryScore/2.5).ToString());


        return (int)Math.Floor(militaryScore / 2.5f);
    }

     
    public int CalculateNavyScore(Nation player) {

            float navyScore = 0;
            foreach (Fleet fleet in player.GetFleets())
            {
            navyScore += fleet.GetFrigate() * 2;
            navyScore += fleet.GetIronClad() * 3.5f;
            navyScore += fleet.GetDreadnought() * 8f;
            } 
        return  (int)Math.Floor(navyScore/ 2.5f);
    }


    public int CalculateMilitaryScore(Nation player)
    {
        int score = 0;
        score += CalculateNavyScore(player);
        score += CaululateArmyScore(player);
        return score;
    }


    public int getNavalUnitCapacity(Nation player)
    {
        int capacity = 0;
        foreach (Fleet fleet in player.GetFleets())
        {
            capacity += fleet.GetFrigate() * 2;
            capacity += fleet.GetIronClad() * 2;
            capacity += fleet.GetDreadnought() * 6;
        }
        return capacity;

    }

    public int getNavelProjection(Nation player)
    {
        int capacity = getNavalUnitCapacity(player);
        float strength = 0;
        float infantryMod = 0.6f;
        float cavalryMod = 0.75f;
        float fighterMod = 1.0f;
        if (player.GetTechnologies().Contains("flintlock"))
        {
            infantryMod = 1f;
            cavalryMod = 1.1f;
        }
        if (player.GetTechnologies().Contains("breech_loaded_arms"))
        {
            infantryMod = 1.2f;
            cavalryMod = 1.2f;

        }
        if (player.GetTechnologies().Contains("machine_guns"))
        {
            infantryMod = 1.5f;
            cavalryMod = 1.3f;
        }
        if (player.GetTechnologies().Contains("bolt_action_rifles"))
        {
            infantryMod = 1.8f;
            cavalryMod = 1.4f;

        }
        float artMod = 1.0f;
        float tankMod = 2.0f;
        if (player.GetTechnologies().Contains("breech_loaded_arms"))
        {
            artMod = 1.15f;
        }
        if (player.GetTechnologies().Contains("indirect_fire"))
        {
            artMod = 1.45f;
        }
        if (player.GetTechnologies().Contains("heavy_armament"))
        {
            artMod = 1.8f;
            tankMod = 1.22f;
        }

        if (player.GetTechnologies().Contains("bombers"))
        {
            fighterMod = 1.25f;
        }

        if (player.GetTechnologies().Contains("radar"))
        {
            fighterMod = 1.5f;
        }

        int numInf = PlayerCalculator.getTotalNumberInfantry(player);
        int numCav = PlayerCalculator.getTotalNumberCavalry(player);
        int numArt = PlayerCalculator.getTotalNumberArtillery(player);
        int numTank = PlayerCalculator.getTotalNumberTanks(player);
        int numFighter = PlayerCalculator.getTotalNumberFighters(player);
 
        strength += fighterMod * 2.8f * numFighter;

        for (int i = 0; i < numInf; i++)
        {
            strength += infantryMod;
            capacity -= 1;
            if (capacity < 1)
            {
                return (int)strength;
            }
        }

        for (int i = 0; i < numTank; i++)
        {
            strength += tankMod * 3.5f;
            capacity -= 3;
            if (capacity < 1)
            {
                return (int)strength;
            }
        }

        for (int i = 0; i < numArt; i++)
        {
            strength += artMod * 1.33f;
            capacity -= 2;
            if (capacity < 1)
            {
                return (int)strength;
            }
        }

        for (int i = 0; i < numArt; i++)
        {
            strength += cavalryMod * 1.22f;
            capacity -= 2;
            if (capacity < 1)
            {
                return (int)strength;
            }
        }
        return (int)strength;   

    }



    



}
