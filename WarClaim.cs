using assemblyCsharp;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WarClaim {


    private MyEnum.claimType claimType;

    private string ID;
    private int otherNation;  //index of nation on which the claim is made
    private bool activeCB;    //whether or not the claim is currently the basis of a CB

    private int claimedProvince;   //index of province  - set to -1 if n/a
    private int colony;   //index of colony  - set to -1 if n/a
    private int sphere; //index of sphered nation  - set to -1 if n/a
    private bool openTrade;   
    private bool moneyPayment;

    private int value;  //used by AI to determine how much the AI cares about this claim

    public WarClaim(MyEnum.claimType type, int otherNation, bool activeCB, int province, 
        int colony, int sphere, bool openTrade, bool moneyPayment)
    {
        this.claimType = type; 
        this.otherNation = otherNation;
        this.activeCB = activeCB;
        this.claimedProvince = province;
        this.colony = colony;
        this.sphere = sphere;
        this.openTrade = openTrade;
        this.moneyPayment = moneyPayment;
        this.ID = otherNation.ToString() + province.ToString() + colony.ToString() + sphere.ToString() +
            openTrade.ToString() + moneyPayment.ToString();
    }


    public int getOtherNation()
    {
        return this.otherNation;
    }

    public bool checkIfActive()
    {
        return this.activeCB;
    }

    public void toggleActive()
    {
        if(this.activeCB == true)
        {
            this.activeCB = false;
        }
        else
        {
            this.activeCB = true;
        }
    }

    public int getProvinceClaimed()
    {
        return this.claimedProvince;
    }

    public int getSphereClaimed()
    {
        return this.sphere;
    }

    public int getColonyClaimed()
    {
        return this.colony;
    }
    
    public bool checkTradeClaim()
    {
        return this.openTrade;
    }

    public bool checkPaymentClaim()
    {
        return this.moneyPayment;
    }

    /*public void orderClaims(Nation player)
    {
        List<WarClaim> claims = player.getWarClaims().OrderBy(o => o.value).ToList();
    } */

    public void setValue(int value)
    {
        this.value = value;
    }

    public int getValue()
    {
        return this.value;
    }

    public MyEnum.claimType GetClaimType()
    {
        return this.claimType;
    }

    public string getID()
    {
        return this.ID;
    }

}
