using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using assemblyCsharp;

public class Army
{
    public Army(int owner)
    {
        this.owner = owner;
        Nation ownerPlayer = State.getNations()[owner];
        int armyNum = ownerPlayer.GetMilitaryForm().getArmyCounter();
        ownerPlayer.GetMilitaryForm().incrementArmyCounter();
        if (armyNum == 1)
        {
            this.armyName = armyNum + "st Army";
        }
        if (armyNum == 2)
        {
            this.armyName = armyNum + "ed Army";
        }
        if (armyNum == 3)
        {
            this.armyName = armyNum + "rd Army";
        }
        else
        {
            this.armyName = armyNum + "th Army";
        }

        this.index = staticIndex;
        staticIndex += 1;

    }


    private static int staticIndex;
    private int index;

    private string armyName;

    private float ammo;
    private float oil;
    private float food;
    private float morale;

    private int infantry;
    private int cavalry;
    private int artillery;
    private int tank;
    private int fighter;

    private int owner;
    private int location;


    public int GetInfantry()
    {
        return this.infantry;
    }

   public int GetArtillery()
    {
        return this.artillery;
    }

    public int GetCavalry()
    {
        return this.cavalry;
    }

    public int GetFighter()
    {
        return this.fighter;
    }

    public int GetTank()
    {
        return this.tank;
    }

    public void RemoveInfantry(int amount)
    {
        this.infantry -= amount;
    }

    public void RemoveCavalry(int amount)
    {
        this.cavalry -= amount;
    }

    public void RemoveArtillery(int amount)
    {
        this.artillery -= amount;
    }

    public void RemoveTank(int amount)
    {
        this.tank -= amount;
    }

    public void RemoveFighter(int amount)
    {
        this.fighter -= amount;
    }

    public void AddInfantry(int amount)
    {
        this.infantry += amount;
    }

    public void AddCavalry(int amount)
    {
        this.cavalry += amount;
    }

    public void AddArtillery(int amount)
    {
        this.artillery += amount;
    }

    public void AddTank(int amount)
    {
        this.tank += amount;
    }

    public void AddFighter(int amount)
    {
        this.fighter += amount;
    }

    public void addUnit(MyEnum.ArmyUnits unit)
    {
        if(unit == MyEnum.ArmyUnits.artillery)
        {
            AddArtillery(1);
        }
        if(unit == MyEnum.ArmyUnits.cavalry)
        {
            AddCavalry(1);
        }
        if (unit == MyEnum.ArmyUnits.fighter)
        {
            AddFighter(1);
        }
        if(unit == MyEnum.ArmyUnits.infantry)
        {
            Debug.Log("Add");
            AddInfantry(1);
        }
        if(unit == MyEnum.ArmyUnits.tank)
        {
            AddTank(1);
        }
    }



    public float GetFood()
    {
        return this.food;
    }

    public float GetOil()
    {
        return this.oil;
    }

    public float GetAmmo()
    {
        return this.ammo;
    }

    public void AddFood(float amount)
    {
        this.food += amount;
    }

    public void AddOil(float amount)
    {
        this.oil += amount;
    }

    public void AddAmmo(float amount)
    {
        this.ammo += amount;
    }

    public void UseAmmo(float amount)
    {
        this.ammo -= amount;
    }

    public void UseOil(float amount)
    {
        this.oil -= amount;
    }

    public void UseFood(float amount)
    {
        this.food -= amount;
    }

    public void NameArmy(string name)
    {
        this.armyName = name;
    }

    public string GetName()
    {
        return this.armyName;
    }

    public int GetIndex()
    {
        return this.index;
    }

    public void setLocation(int loc)
    {
        this.location = loc;
    }

    public int getLocation()
    {
        return location;
    }

   
}
