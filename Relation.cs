using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using assemblyCsharp;

public class Relation  {
    //each Nation has a dictionary of relations with other nations
    //The values on 'your' end of of the relation concern information from 'your' side. 
    //Will have to consult relation with other player for information from 'his' side
    //Some symmetrical data will be kept on both sides, such as being at war.
    private int relationTo;

    private int attitude;   //out of 100
    private bool war;
    private List<WarClaim> CasusBelli = new List<WarClaim>();
    private bool defensiveAlliance;
    private bool fullAlliance;
    private bool grantMilitaryAccess;
    private bool embargoing;

   // private List<int> colonyClaimsRecognizing = new List<int>();
   // private List<int> sphereClaimsRecognizing = new List<int>();

    private int turnsSinceLastPeace = 1000;

    public Relation(int relationTo, int attitude, bool war, bool defensiveAlliance, bool fullAlliance, bool grantMilitaryAccess, bool embargoing)
    {
        this.relationTo = relationTo;
        this.attitude = attitude;
        this.war = war;
        this.defensiveAlliance = defensiveAlliance;
        this.fullAlliance = fullAlliance;
        this.grantMilitaryAccess = grantMilitaryAccess;
        this.embargoing = embargoing;
    }

    public void adjustAttude(int amount)
    {
        this.attitude += amount;
        if(this.attitude > 100)
        {
            this.attitude = 100;
        }
        if(this.attitude < 0)
        {
            this.attitude = 0;
        }
    }

    public int getAttitude()
    {
        return this.attitude;
    }

    public void makePeace()
    {
        this.war = false;
    }

    public void makeWar()
    {
        this.war = true;
    }

    public bool isAtWar()
    {
        return war;
    }

    public List<WarClaim> getCasusBelli()
    {
        return this.CasusBelli;
    }

    public void addCasusBelli(WarClaim claim)
    {
        claim.toggleActive();
        this.CasusBelli.Add(claim);
    }

    public void formDefensiveAlliance()
    {
        this.defensiveAlliance = true;
    }

    public void  endDefensiveAlliance()
    {
        this.defensiveAlliance = false;
    }

    public bool isDefensiveAlliance()
    {
        return this.defensiveAlliance;
    }


    public void formFullAlliance()
    {
        this.fullAlliance = true;
    }

    public void endFullAlliance()
    {
        this.fullAlliance = false;
    }

    public bool isFullAlliance()
    {
        return this.fullAlliance;
    }

    public void GrantMilitaryAccess()
    {
        this.grantMilitaryAccess = true;
    }

    public void EndMilitaryAccess()
    {
        this.grantMilitaryAccess = false;
    }

    public bool givesMilitaryAccess()
    {
        return this.grantMilitaryAccess;
    }

    public void startEmbargo()
    {
        this.embargoing = true;
    }

    public void endEmbargo()
    {
        this.embargoing = false;
    }

    public bool isEmbargoing()
    {
        return this.embargoing;
    }

    public void resetTurnsSinceLastPeace()
    {
        this.turnsSinceLastPeace = 0;
    }

    public void incrementTurnsSinceLastPeace()
    {
        this.turnsSinceLastPeace += 1;
    }

    public int getTurnsSinceLastPeace()
    {
        return this.turnsSinceLastPeace;
    }

    public bool isRecentPeace()
    {
        if (turnsSinceLastPeace < 5)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public int getRelationTo()
    {
        return relationTo;
    }

}
