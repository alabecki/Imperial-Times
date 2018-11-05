using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandUnit
{
    private float strength;
    private float attack;
    private float defense;
    private float ammoUse;
    private float oilUse;
    private int weight;
    private float shock;

    public LandUnit()
    {

    }


    public LandUnit(float strength, float attackStrength, float defenseStrength, 
        float ammoUse, float oilUse, int weight, float shock)
    {
        this.strength = strength;
        this.attack = attackStrength;
        this.defense = defenseStrength;
        this.ammoUse = ammoUse;
        this.oilUse = oilUse;
        this.weight = weight;
        this.shock = shock;
    
    }

    public float AttackStrength
    {
        get
        {
            return attack;
        }

        set
        {
            attack = value;
        }
    }

    public float DefenseStrength
    {
        get
        {
            return defense;
        }

        set
        {
            defense = value;
        }
    }


    public float Shock
    {
        get
        {
            return shock;
        }

        set
        {
            shock = value;
        }
    }



    public float AmmoUse
    {
        get
        {
            return ammoUse;
        }

        set
        {
            ammoUse = value;
        }
    }

    public float OilUse
    {
        get
        {
            return oilUse;
        }

        set
        {
            oilUse = value;
        }
    }

    public int Weight
    {
        get
        {
            return weight;
        }

        set
        {
            weight = value;
        }
    }

    public void ImproveAttackStrength(float addition)
    {
        this.attack += addition;
    }

    public void ImproveDefenseStrength(float addition)
    {
        this.defense += addition;
    }

   

    public void IncreaseAmmoConsumption(float amount)
    {
        this.ammoUse += amount;
    }

 
    public void DecreaseStrength(float loss)
    {
        this.strength -= loss;
    }


    public void IncreaseShock(float increase)
    {
        this.shock += increase;
    }

    public float GetStrength()
    {
        return strength;
    }

    public void AddStrength(float amount)
    {
        this.strength += amount;
    }

}
