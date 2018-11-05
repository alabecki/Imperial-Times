using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirUnit  {
    private float strength;
    private float attack;
    private float defense;
    private float ammoUse;
    private float oilUse;
    private int weight;
    private int movement;
    private float morale;
    private float shock;

    public AirUnit()
    {

    }

    public AirUnit(float strength, float attack, float defense,
        float ammoUse, float oilUse, int weight, int movement, float shock)
    {
        this.strength = strength;
        this.attack = attack;
        this.defense = defense;
        this.ammoUse = ammoUse;
        this.oilUse = oilUse;
        this.weight = weight;
        this.movement = movement;
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

    public int Movement
    {
        get
        {
            return movement;
        }

        set
        {
            movement = value;
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

    public float Morale
    {
        get
        {
            return morale;
        }

        set
        {
            morale = value;
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

    public void ImproveMorale(float addition)
    {
        this.morale += addition;
    }



    public void DecreaseStrength(float loss)
    {
        this.strength -= loss;
    }

    public void DecreaseMorale(float loss)
    {
        this.morale -= loss;
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

