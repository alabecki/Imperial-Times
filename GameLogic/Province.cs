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
        WorldMapStrategyKit.WMSK map;

        public string provinceName;
        public string owner;
        public int index;
        public int ownerIndex;
        // private MyEnum.Resources resource;
        //public string resource;
        [JsonConverter(typeof(StringEnumConverter))]
        public MyEnum.Resources resource;
        public int development;
        public bool railroad;
        public bool newRailroad;
        //private bool functioning;
        public int discontentment = 0;
        public bool rioting = false;
        public bool disaster = false;
        public bool functioning = true;
        private bool bonus = false;

        // public MyEnum.provinceState state = MyEnum.provinceState.normal;

        public float[] quality = new float[5];
        public string culture;
        public int fortification;
        //public int shipyard;
        public bool coastal;
        public bool isColony;

        private bool rayBlocked = false;

        List<int> friendlyArmies = new List<int>();
        List<int> hostileArmies = new List<int>();
        List<int> neighbours = new List<int>();

        List<int> linked = new List<int>();




        public List<int> Neighbours { get => neighbours; set => neighbours = value; }
        public List<int> Linked { get => linked; set => linked = value; }
        public bool Bonus { get => bonus; set => bonus = value; }

        public Province()
        {

        }
        public static Province CreateFromJSON(string jsonString)
        {
            return JsonUtility.FromJson<Province>(jsonString);
        }

        public Province(string provinceName, string owner, int index, int ownerIndex, MyEnum.Resources resource, bool railroad,
        float[] quality, string culture)
        {
            this.provinceName = provinceName;
            this.owner = owner;
            this.index = index;
            this.ownerIndex = ownerIndex;
            this.resource = resource;
            this.railroad = railroad;
            this.quality = quality;
            this.culture = culture;
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

        public void setOwner(string newOwner)
        {
            owner = newOwner;
        }


        public string getOwner()
        {
            return this.owner;
        }

        public int getOwnerIndex()
        {
            return this.ownerIndex;
        }

        public void setOwnerIndex(int value)
        {
            this.ownerIndex = value;
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


        public int getProduction()
        {
            //keeping it simple for now - will later factor in corruption, happiness,
            // and nationalism
            if (this.functioning == false)
            {
                return 0;
            }
            if (this.railroad == false)
            {
                return 1;
            }
            int output = development + 1;
            if(bonus == true)
            {
                output *= 2;
            }
            return output;

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


        public bool getRail()
        {
            return railroad;
        }

        public void buildRailroad()
        {
            railroad = true;
        }

        public void destroyRailroad()
        {
            railroad = false;
        }

        public bool getRioting()
        {
            return this.rioting;
        }

        public bool getDisaster()
        {
            return disaster;
        }

        public void setDisaster(bool value)
        {
            disaster = value;
        }


        public void setFunctioning()
        {
            if (this.rioting || this.disaster)
            {
                this.functioning = false;
            }
            else
            {
                this.functioning = true;
            }
        }

        public bool getFunctioning()
        {
            return this.functioning;
        }

        public void setRioting(bool state)
        {
            if (state == true)
            {
                this.rioting = true;
            }
            else
            {
                this.rioting = false;
            }
            setFunctioning();
        }


        public void setDiscontentment(int value)
        {
            this.discontentment = value;
        }

        public void adjustDiscontentment(int value)
        {
            this.discontentment += value;
        }

        public int getDiscontentment()
        {
            return this.discontentment;
        }

     

        public void resetDiscontentment(Nation newOwner)
        {
            int stability = newOwner.Stability;
            int corruption = newOwner.GetCorruption();
            int newDiscontment = (3 - stability) + corruption;
            setDiscontentment(newDiscontment);
        }


        public void changeProvinceControl(Nation oldOwner, Nation newOwner)
        {
            Debug.Log("Change Province Ownership -----------------------------------------------------------------------------------------------------------------");
            map = WorldMapStrategyKit.WMSK.instance;

            oldOwner.removeProvince(this.getIndex());
            newOwner.addProvince(this.getIndex());
            this.resetDiscontentment(newOwner);
            newOwner.increasePrestige(2);
            oldOwner.decreasePrestige(2);

         
            WorldMapStrategyKit.Province mapProv = map.provinces[this.index];
            WorldMapStrategyKit.Region provRegion = mapProv.mainRegion;
            map.CountryTransferProvinceRegion(newOwner.getIndex(), provRegion, false);

            foreach (assemblyCsharp.Province prov in State.getProvinces().Values)
            {
                int provIndex = prov.getIndex();
                WorldMapStrategyKit.Province _mapProv = map.provinces[provIndex];
                int countryIndex = _mapProv.countryIndex;
                int nationIndex = prov.getOwnerIndex();
                if (countryIndex != nationIndex)
                {
                    Debug.Log("Reassign Nation");
                    mapProv.countryIndex = nationIndex;
                    WorldMapStrategyKit.Region _provRegion = _mapProv.mainRegion;
                    WorldMapStrategyKit.Country newOwnerCountry = map.countries[nationIndex];
                    map.CountryTransferProvinceRegion(countryIndex, _provRegion, true);

                }
            }

            for (int k = 0; k < map.countries.Length; k++)
            {
                Color color = new Color(UnityEngine.Random.Range(0.0f, 0.65f),
                UnityEngine.Random.Range(0.0f, 0.65f), UnityEngine.Random.Range(0.0f, 0.65f));
                Nation nation = State.getNations()[k];
                // Debug.Log(nation.getName());
                nation.setColor(color);
                map.ToggleCountrySurface(k, true, color);
            }

            Utilities.SaveMapData();

            if (this.getRail())
            {
                this.newRailroad = true;
            }
        }
    }
}
