using assemblyCsharp;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SeaForces 
{
    public FrigateForm frigate = new FrigateForm                (0, 1, 25, 0.6f, 2, 1);
    public IroncladForm ironclad = new IroncladForm             (0, 2, 35, 0.8f, 3, 2);
    public DreadnoughtForm dreadnought = new DreadnoughtForm    (0, 4, 80, 0.8f, 6, 5);

    public float[] frigateSim = new float[8];
    public float[] ironSim = new float[8];
    public float[] dreadSim = new float[8];



    public void dataTransferToSim()
    {
     //   Debug.Log("Fleet size save: " + frigate.Number);
        frigateSim[0] = frigate.Number;
        frigateSim[1] = frigate.HitPoints;
        frigateSim[2] = frigate.Attack;
        frigateSim[3] = frigate.Maneuver;
        frigateSim[6] = frigate.Capacity;
        frigateSim[7] = frigate.Colonial;

        ironSim[0] = ironclad.getNumber();
        ironSim[1] = ironclad.HitPoints;
        ironSim[2] = ironclad.Attack;
        ironSim[3] = ironclad.Maneuver;
        ironSim[6] = ironclad.Capacity;

        dreadSim[0] = dreadnought.getNumber();
        dreadSim[1] = dreadnought.HitPoints;
        dreadSim[2] = dreadnought.Attack;
        dreadSim[3] = dreadnought.Maneuver;
        dreadSim[6] = dreadnought.Capacity;
    }

    public void dataTransferFromSim()
    {
   //     Debug.Log("Fleet size load: " + (int)frigateSim[0]);
        frigate.Number = (int)frigateSim[0];
        frigate.HitPoints = frigateSim[1];
        frigate.Attack = frigateSim[2];
        frigate.Maneuver = frigateSim[3];
        frigate.Capacity = (int)frigateSim[4];
        frigate.Colonial = (int)frigateSim[5];

        ironclad.Number = (int)ironSim[0];
        ironclad.HitPoints = ironSim[1];
        ironclad.Attack = ironSim[2];
        ironclad.Maneuver = ironSim[3];
        ironclad.Capacity = (int)ironSim[4];
        ironclad.Colonial = (int)ironSim[5];


        dreadnought.Number = (int)dreadSim[0];
        dreadnought.HitPoints = dreadSim[1];
        dreadnought.Attack = dreadSim[2];
        dreadnought.Maneuver = dreadSim[3];
        dreadnought.Capacity = (int)dreadSim[4];
        dreadnought.Colonial = (int)dreadSim[5];

    }

    public class FrigateForm : SeaUnit
    {
        public FrigateForm(int number, int HP, float attack, float maneuver,
            int capacity, int colonial)
              : base(number, HP, attack, maneuver, capacity, colonial)
        {
            this.Number = number;
            this.HitPoints = HP;
            this.Attack = attack;
            this.Maneuver = maneuver;
            this.Capacity = capacity;
            this.Colonial = colonial;
        }
    }

    public class IroncladForm : SeaUnit
    {
        public IroncladForm(int number, int HP, float attack, float maneuver, int capacity,
            int colonial)
              : base(number, HP, attack, maneuver, capacity, colonial)
        {
            this.Number = number;
            this.HitPoints = HP;
            this.Attack = attack;
            this.Maneuver = maneuver;
            this.Capacity = capacity;
            this.Colonial = colonial;
        }
    }

    public class DreadnoughtForm : SeaUnit
    {
        public DreadnoughtForm(int number, int HP, float attack, float maneuver,  int capacity,
             int colonial)
              : base(number, HP, attack, maneuver, capacity, colonial)
        {
            this.Number = number;
            this.HitPoints = HP;
            this.Attack = attack;
            this.Maneuver = maneuver;
            this.Capacity = capacity;
            this.Colonial = colonial;
        }
    }

    public int getNavalStrength(Nation player)
    {
        int strength = 0;
        strength += frigate.Number;
        strength += ironclad.Number * 2;
        strength += dreadnought.Number * 5;
        return strength;
    }

}
