using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using assemblyCsharp;

public class Technology  {

    private string techName;
    private int index;
    private List<string> preRequisites = new List<string>();
    private int cost;
    private int prestige;
    private int payment;
    private bool discovered;
    private int discoveredBy = -1;
    private string[] description;

    public Technology(string techName, int index, List<string> preRequisites, int cost, 
        int prestige, int payment, string[] description)
    {
        this.techName = techName;
        this.index = index;
        this.preRequisites = preRequisites;
        this.cost = cost;
        this.prestige = prestige;
        this.payment = payment;
        this.description = description;
    }

    public string GetTechName()
    {
        return this.techName;
    }

    public int GetIndex()
    {
        return this.index;
    }

    public List<string> GetPreRequisites()
    {
        return this.preRequisites;
    }

    public int GetCost()
    {
        Debug.Log("Base cost of tech is: " + this.cost);
        if (discovered)
        {
            Debug.Log("Discovered");

            int newCost = this.cost / 2;
            if(this.cost % 2 == 1)
            {
                newCost++;
            }
            return newCost;
        }
        else
        {
            return this.cost;
        }
    }

    public int GetPrestige()
    {
        return this.prestige;
    }

    public int GetPayment()
    {
        return this.payment;
    }

    public bool GetDiscovered()
    {
        return this.discovered;
    }

    public void SetDiscovered(bool discovered)
    {
        this.discovered = discovered;
    }

    public void SetDiscoveredBy(int discoveredBy)
    {
        this.discoveredBy = discoveredBy;
    }

    public int GetDiscoveredBy()
    {
        return this.discoveredBy;
    }

    public string[] GetDescription()
    {
        return this.description;
    }


    public bool isDiscovered()
    {
        if (discovered)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
