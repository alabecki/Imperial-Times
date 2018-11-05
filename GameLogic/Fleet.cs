using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using assemblyCsharp;

public class Fleet
{

    public Fleet(int owner)
    {
        this.owner = owner;
        Nation ownerPlayer = State.getNations()[owner];
        int navyNum = ownerPlayer.GetMilitaryForm().getNavyCounter();
        ownerPlayer.GetMilitaryForm().incrementNavyCounter();
        if (navyNum == 1)
        {
            this.navyName = navyNum + "st Fleet";
        }
        if (navyNum == 2)
        {
            this.navyName = navyNum + "ed Fleet";
        }
        if (navyNum == 3)
        {
            this.navyName = navyNum + "rd Fleet";
        }
        else
        {
            this.navyName = navyNum + "th Fleet";
        }
        this.index = staticIndex;
        staticIndex += 1;

    }

    private static int staticIndex;
    private int index;

    private string navyName;
    private float ammo;
    private float oil;
    private float food;

    private int frigate;
    private int ironclad;
    private int dreadnought;

    private int owner;
    private int location;

    public int GetFrigate()
    {
        return this.frigate;
    }

    public int GetIronClad()
    {
        return this.ironclad;
    }

    public int GetDreadnought()
    {
        return this.dreadnought;
    }

    public void AddFrigate(int amount)
    {
        this.frigate += amount;
    }

    public void AddIronclad(int amount)
    {
        this.ironclad += amount;
    }

    public void AddDreadnought(int amount)
    {
        this.dreadnought += amount;
    }

    public void addUnit(MyEnum.NavyUnits unit, int amount)
    {
        if (unit == MyEnum.NavyUnits.dreadnought)
        {
            AddDreadnought(amount);
        }
        if (unit == MyEnum.NavyUnits.frigates)
        {
        AddFrigate(amount);
        }
        if (unit == MyEnum.NavyUnits.ironclad)
        {
        AddIronclad(amount);
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

    public void NameNavy(string name)
    {
        this.navyName = name;
    }

    public string GetName()
    {
        return this.navyName;
    }

    public int getIndex()
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
