using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaUnit {

    private int number;
    private float attack;
    private float maneuver;
    private int capacity;
    private float hitPoints;
    private int strength;
    private int colonial;

    public SeaUnit()
    {

    }

    public SeaUnit(int number, int hitPoints, float attack, float maneuver, int capacity,
        int colonial)
    {
        this.number = number;
        this.hitPoints = hitPoints;
        this.maneuver = maneuver;
        this.capacity = capacity;
      //  this.hitPoints = hitPoints;
        this.colonial = colonial;
    }


    public void addUnit(int number)
    {
        this.number += number;
    }

    public void deleteUnit()
    {
        this.number--;
    }

    public int getNumber()
    {
        return this.number;
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


    public int Strength { get => strength; set => strength = value; }
    public int Number { get => number; set => number = value; }
    public int Colonial { get => colonial; set => colonial = value; }
    public float HitPoints { get => hitPoints; set => hitPoints = value; }
    public int Capacity { get => capacity; set => capacity = value; }

    public void ImproveAttack(float addition)
    {
        this.attack += addition;
    }

    public void ImproveManouver(float addition)
    {
        this.maneuver += addition;
    }

 

  





 

   


  


}
