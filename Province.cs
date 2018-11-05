using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace assemblyCsharp
{

    [System.Serializable]
    public class Province
    {
        public string provinceName;
        public string owner;
        public int index;
        public int ownerIndex;
        // private MyEnum.Resources resource;
        //public string resource;
        [JsonConverter(typeof(StringEnumConverter))]
        public MyEnum.Resources resource;
        public int infrastructure;
        private int numRailsWorking;
        public int development;
        private bool functioning;

        public float[] quality = new float[5];
        public string culture;
        public int POP;
        public int fortification;
        //public int shipyard;
        public bool coastal;

        private bool rayBlocked = false;

        List<int> friendlyArmies = new List<int>();
        List<int> hostileArmies = new List<int>();


        public Province()
        {

        }
        public static Province CreateFromJSON(string jsonString)
        {
            return JsonUtility.FromJson<Province>(jsonString);
        }

        public Province(string provinceName, string owner, int index, int ownerIndex, MyEnum.Resources resource, int infrastructure,
        float[] quality, string culture)
        {
            this.provinceName = provinceName;
            this.owner = owner;
            this.index = index;
            this.ownerIndex = ownerIndex;
            this.resource = resource;
            this.infrastructure = infrastructure;
            this.quality = quality;
            this.culture = culture;
            this.functioning = true;
        }
        public string getProvName()
        {
            return this.provinceName;
        }
        public int getIndex()
        {
            return this.index;
        }

        public MyEnum.Resources getResource()
        {
            return this.resource;
        }

   

        public int getFortLevel()
        {
            return this.fortification;
        }


        public string getCulture()
        {
            return this.culture;
        }

      


        public string getOwner()
        {
            return this.owner;
        }

        public int getOwnerIndex()
        {
            return this.ownerIndex;
        }


        public void SetRayBlocked(bool value)
        {
            this.rayBlocked = value;
        }

        public bool GetRayBlocked()
        {
            return this.rayBlocked;
        }


        public void setDevelopmentLevel(int level)
        {
            this.development = level;
        }

        public void changeDevelopmentLevel(int amount)
        {
            this.development += amount;
        }

        public int getDevelopmentLevel()
        {
            return this.development;
        }


        public float getProduction()
        {
            float output = 0f;
            int totalImprovement = this.numRailsWorking + this.development;
            Nation owner = State.GetNationByName(this.getOwner());
            float corruption = owner.GetCorruption() / 2;
            int corruptionMod = (int)Math.Floor(corruption);
            totalImprovement = Math.Max(0, totalImprovement - corruptionMod);
            output = this.quality[totalImprovement];
            if(owner.culture != this.getCulture())
            {
                if (State.worldAffairs.IsNationalismDiscovered())
                {
                    output = output * 0.8f;
                }
                else
                {
                    output = output * 0.9f;
                }
            }
            return output;

        }

        public int getPOP()
        {
            return POP;
        }

        public void setPOP(int amount)
        {
            this.POP = amount;
        }

        public void changePOP(int amount)
        {
            this.POP += amount;
        }

        public int getInfrastructure()
        {
            return this.infrastructure;
        }

        public void upgradeInfrastructure()
        {
            this.infrastructure += 1;
            this.numRailsWorking += 1;
        }

        public void addFriendlyArmy(int id)
        {
            this.friendlyArmies.Add(id);
        }

        public void removeFriendlyArmy(int id)
        {
            this.friendlyArmies.Remove(id);
        }

        public void addHostileArmy(int id)
        {
            this.hostileArmies.Add(id);
        }

        public void removeHostileArmy(int id)
        {
            this.hostileArmies.Remove(id);
        }

        public List<int> getFriendlyArmies()
        {
            return this.friendlyArmies;
        }

        public List<int> getHostileArmies()
        {
            return this.hostileArmies;
        }

        public void setFunctioning(bool val)
        {
            functioning = val;
        }

        public bool getFunctioning(bool val)
        {
            return functioning;
        }

        public int getNumberRailsWorking()
        {
            return this.numRailsWorking;
        }

        public void setNumberRailsWorking(int amount)
        {
            this.numRailsWorking = amount;
        }

        public void addRailNotWorking()
        {
            this.numRailsWorking -= 1;
        }


    }

 
}
