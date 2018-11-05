using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MilitaryForm {

    public float maxMorale;
    public int armyNumberCounter;
    public int navyNumberCounter;


    public InfantryForm infantry = new InfantryForm(1.0f, 1.0f, 1.2f, 0.05f, 0f, 1, 0.1f);
    public ArtilleryForm artillery = new ArtilleryForm(1.0f, 1.1f, 1.8f, 0.1f, 0.0f, 2, 0.2f);
    public CavalryForm cavalry = new CavalryForm(1.0f, 1.5f, 1.0f, 0.05f, 0.0f, 2, 0.3f);
    public TankForm tank = new TankForm(1.0f, 3.0f, 2.0f, 0.075f, 0.1f, 3, 1.0f);
    public FighterForm fighter = new FighterForm(1.0f, 1.0f, 1.2f, 0.075f, 0.1f, 3, 3, 0.1f);

    public FrigateForm frigate = new FrigateForm(1.0f, 1.0f, 0.6f, 0.1f, 0.0f, 2,  2, 0);
    public IroncladForm ironclad = new IroncladForm(2.0f, 1.4f, 0.8f, 0.1f, 0.0f, 2, 3, 0);
    public DreadnoughtForm dreadnought = new DreadnoughtForm(4.0f, 4.0f, 0.4f, 0.25f,0.2f, 6, 4, 0);



    public class InfantryForm : LandUnit {
        public InfantryForm(float strength, float attack, float defense,
        float ammoUse, float oilUse, int weight, float shock)
            : base(strength, attack, defense, ammoUse, oilUse, weight, shock)
        {

        }
    }

    public class ArtilleryForm : LandUnit
    {
        public ArtilleryForm(float strength, float attack, float defense,
        float ammoUse, float oilUse, int weight, float shock)
            : base(strength, attack, defense, ammoUse, oilUse, weight, shock)

        {

        }
    }

    public class CavalryForm : LandUnit
    {
        public CavalryForm(float strength, float attack, float defense, float ammoUse, float oilUse, int weight, float shock)
            : base(strength, attack, defense, ammoUse, oilUse, weight, shock)

        {

        }
    }

    public class TankForm : LandUnit
    {
        public TankForm(float strength, float attack, float defense, 
        float ammoUse, float oilUse, int weight, float shock)
            : base(strength, attack, defense, ammoUse, oilUse, weight, shock)

        {
           
        }
    }
        
    public class FrigateForm : SeaUnit
    {
        public FrigateForm(float strength, float attack, float maneuver, float ammoUse, float oilUse, int capacity,
            int movement, float experience)
              :base(strength, attack, maneuver, ammoUse, oilUse, capacity, movement, experience)
                {
            attack = 1.0f;
                }
    }

    public class IroncladForm : SeaUnit
    {
        public IroncladForm(float strength, float attack, float maneuver, float ammoUse, float oilUse, int capacity,
            int movement, float experience)
              : base(strength, attack, maneuver, ammoUse, oilUse, capacity, movement, experience)
        {
            attack = 1.4f;
        }
    }

    public class DreadnoughtForm : SeaUnit
    {
        public DreadnoughtForm(float strength, float attack, float maneuver, float ammoUse, float oilUse, int capacity,
             int movement, float experience)
              : base(strength, attack, maneuver, ammoUse, oilUse, capacity, movement, experience)
        {
            attack = 4.0f;
        }
    }

    public class FighterForm : AirUnit
    {
        public FighterForm(float strength, float attack, float defense,
       float ammoUse, float oilUse, int weight, int movement,  float shock)
           : base(strength, attack, defense, ammoUse, oilUse, weight, movement, shock)
        {

        }

    }


    public void adjustMaxMorale(float amount)
    {

        this.maxMorale += amount ;
    }


    public float getMaxMorale()
    {
        return maxMorale;
    }

    public void setMaxMorale(float amount)
    {
        maxMorale = amount;
    }


    public int getArmyCounter()
    {
        return this.armyNumberCounter;
    }

    public void incrementArmyCounter()
    {
        this.armyNumberCounter += 1;
    }

    public int getNavyCounter()
    {
        return this.navyNumberCounter;
    }

    public void incrementNavyCounter()
    {
        this.navyNumberCounter += 1;
    }






}
