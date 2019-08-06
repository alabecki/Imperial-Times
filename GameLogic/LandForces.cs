using assemblyCsharp;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class LandForces 
{
    private List<MyEnum.ArmyDoctrines> doctrines = new List<MyEnum.ArmyDoctrines>();


    private int  size;            
    //the following are current base values and are typically affected by the size of your force

    // Attack and defense pertain to how much damage your army does when attacking or defending. This is modified with size
    // but can have diminishing returns when your army is not well positioned or does not maneuver well (most units not in position to
    // fire most of the time
    private int  attack;          
    private int  defense;

    // If an army runs out of morale, it will retreat even it most forces remain intact. Also has some effect on manouvering. 
    private int morale;

    // Determines how much moral damage your army does (same comments as with attack and defense)
    private int shock;

    // Determines how well your army executes plans
    private int maneuver;


    private int espionage;
    private int counterEspionage = 0;

    // Determines how well you can scout out or otherwise detect the position of opposing armies
    private int recon;

    // If true, it will be more difficult for opposing armies to detect yours

     private int evasion;

    private int concealment;

    private int judgment;

    private int mobilizationSpeed;

    private float ammoUse;
    private float oilUse;

    public int Strength { get => size; set => size = value; }
    public int Attack { get => attack; set => attack = value; }
    public int Defense { get => defense; set => defense = value; }
    public int Morale { get => morale; set => morale = value; }
    public int Shock { get => shock; set => shock = value; }
    public int Maneuver { get => maneuver; set => maneuver = value; }
    public int Espionage { get => espionage; set => espionage = value; }
    public int Recon { get => recon; set => recon = value; }
    public float AmmoUse { get => ammoUse; set => ammoUse = value; }
    public float OilUse { get => oilUse; set => oilUse = value; }
    public int MobilizationSpeed { get => mobilizationSpeed; set => mobilizationSpeed = value; }
    public int CounterEspionage { get => counterEspionage; set => counterEspionage = value; }
    public int Evasion { get => evasion; set => evasion = value; }
    public int Concealment { get => concealment; set => concealment = value; }
    public int Judgment { get => judgment; set => judgment = value; }

    // Maybe replace whole thing with Radial Menu - up to THREE levels in each of
    // attack, defend, espionage, counterEspionage, Maneuver, Judgement, Recon, Concealment, Evasion, Morale, Shock, Mobilization
    public void addDoctrine(MyEnum.ArmyDoctrines doctrine)
    {
        if (doctrine == MyEnum.ArmyDoctrines.ConcentratedArtillery)
        {
            attack += 5;
            defense += 5;
        }
        else if (doctrine == MyEnum.ArmyDoctrines.CounterIntelligence)
        {
            counterEspionage += 10;
        }
        else if (doctrine == MyEnum.ArmyDoctrines.DragoonCommanders)
        {
            maneuver += 20;
        }
        else if (doctrine == MyEnum.ArmyDoctrines.EarlyDetection)
        {
            recon += 20;

        }
        else if (doctrine == MyEnum.ArmyDoctrines.Evasion)
        {
            evasion += 15;
        }
        else if (doctrine == MyEnum.ArmyDoctrines.ExpertRaiders)
        {
            shock += 10;
        }
        else if (doctrine == MyEnum.ArmyDoctrines.ExpertSpies)
        {
            espionage += 20;
        }
        else if (doctrine == MyEnum.ArmyDoctrines.InsipiringLeaders)
        {
            morale += 15;
        }
        else if (doctrine == MyEnum.ArmyDoctrines.InterceptionPatrols)
        {
            concealment += 10;
        }
        else if (doctrine == MyEnum.ArmyDoctrines.LockStep)
        {
            maneuver += 20;
        }
        else if (doctrine == MyEnum.ArmyDoctrines.MilitaryScience)
        {
            judgment += 15;
        }
        else if (doctrine == MyEnum.ArmyDoctrines.MilitaryTradition)
        {
            judgment += 15;
        }
        else if (doctrine == MyEnum.ArmyDoctrines.Professionalism)
        {
            morale += 15;
        }
        else if (doctrine == MyEnum.ArmyDoctrines.ShockAndAwe)
        {
            shock += 15;
        }
        else if (doctrine == MyEnum.ArmyDoctrines.SkilledHussars)
        {
            recon += 20;
        }
        else if (doctrine == MyEnum.ArmyDoctrines.SpyNetwork)
        {
            espionage += 20;
        }
        else if(doctrine == MyEnum.ArmyDoctrines.MassConscription)
        {
            mobilizationSpeed += 2;
        }
        else if(doctrine == MyEnum.ArmyDoctrines.ExpertLogistics)
        {
            mobilizationSpeed += 2;
        }

        this.doctrines.Add(doctrine);
    }


    public bool hasDoctrine(MyEnum.ArmyDoctrines doctrine)
    {
        if (this.doctrines.Contains(doctrine)){
            return true;
        }
        else
        {
            return false;
        }
    }

    public int numberOfDoctrines()
    {
        return this.doctrines.Count;
    }

}
