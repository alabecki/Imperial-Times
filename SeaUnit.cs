using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaUnit {

    private float strength;
    private float attack;
    private float maneuver;
    private float ammoUse;
    private float oilUse;
    private int capacity;
    private float hitPoints;
    private int movement;    
    private float experience;


    public SeaUnit()
    {

    }

    public SeaUnit(float strength, float attack, float maneuver, float ammoUse, float oilUse, int capacity,
        int movement, float experience)
    {
        this.strength = strength;
        this.maneuver = maneuver;
        this.ammoUse = ammoUse;
        this.oilUse = oilUse;
        this.capacity = capacity;
      //  this.hitPoints = hitPoints;
        this.movement = movement;
        this.experience = experience;
    }

    public float Attack
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

    public float Maneuver
    {
        get
        {
            return maneuver;
        }

        set
        {
            maneuver = value;
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

    public int Capacity
    {
        get
        {
            return capacity;
        }

        set
        {
            capacity = value;
        }
    }

    public float HitPoints
    {
        get
        {
            return hitPoints;
        }

        set
        {
            hitPoints = value;
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

    public void ImproveAttack(float addition)
    {
        this.attack += addition;
    }

    public void ImproveManouver(float addition)
    {
        this.maneuver += addition;
    }

    public void IncreaseAmmoConsumption(float amount)
    {
        this.ammoUse += amount;
    }

  

    public void GainExperience(float addition)
    {
        this.experience += addition;
    }

    public void DecreaseStrength(float loss)
    {
        this.strength -= loss;
    }

    public float GetStrength()
    {
        return this.strength;
    }


  


}
