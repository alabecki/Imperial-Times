using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Linq;

namespace assemblyCsharp
{
    [System.Serializable]

    public class Nation
    {

        public PlayerIndustry industry = new PlayerIndustry();

        private AI ai = new AI();

        private bool human;
        private int prestige = 0;
        private int militaryScore;
        private int industrialScore;
        private int totalScore;

        public string nationName;
        public int index;
        public string capital;
        public Vector2 newNavyPosition = new Vector2();

        [JsonConverter(typeof(StringEnumConverter))]
        public MyEnum.NationType type;
        private int isSphereOf = -1;
        private int isColonyOf = -1;

        public Color Color;
        // public int type;

        public string culture;
        public List<string> acceptedCultures = new List<string>();

        [JsonProperty("provinceIndexes")]
        List<int> provinceIndexes = new List<int>();

        private float IP;

        private float AP;
        private float colonialPoints;
        private List<int> colonies = new List<int>();
        private float infulencePoints;
        private List<int> spheres = new List<int>();
        private float gold;

        private float importValue;
        private float exportValue;
        private List<Signal> imports = new List<Signal>();
        private List<Signal> exports = new List<Signal>();


        private float research;
        private int numberPattents;

        private int diplomacyPoints;
        private int reputation;
        private int reliability;

        private List<WarClaim> warClaims = new List<WarClaim>();
        private Dictionary<int, Relation> relations = new Dictionary<int, Relation>();
        private List<int> orderedRelationLoveToHate = new List<int>();

        //Types are: major, minor, oldEmpire, and oldMinor		
        //private MyEnum.NationType type;	
        //stabulity, happiness, and govenrment in [0, 100]
        //Corruption  and prestige are is a factors of stability
        private float stability;
        private float corruption;
        // int morale;     
        //prestige is a factor of morale

        //maybe make a Government data-structure
        private MyEnum.Government government;

        //Culture enumerator will be populated by scenario
        //private MyEnum.Culture culture;
        //private List<MyEnum.Culture> acceptedCulture = new List<MyEnum.Culture>();

        //Not sure yet if religion will be included - if so, will mostly affect diolomacy, might also affect happiness 
        //and research
        private MyEnum.Religion religion;

        //each string is name of a bordering nation 
        private HashSet<string> borders;

        private int urbanPOP;
        private float militaryPOP;
        private int ruralPOP;
        private float unemployed;
        private int totalPOP;

        private float foodShortage = 0;
        private float foodImbalance = 0;
        private float oilNeeded = 0;

        private int popIncreasedThisTurn;
        //private MyEnum.Specialist specialist;
        private HashSet<string> technologies = new HashSet<string>();
        private HashSet<string> patents = new HashSet<string>();


        private Dictionary<MyEnum.Resources, float> resources = new Dictionary<MyEnum.Resources, float>()
        {{MyEnum.Resources.wheat, 0}, {MyEnum.Resources.meat, 0},
        {MyEnum.Resources.fruit, 0}, {MyEnum.Resources.iron, 0}, {MyEnum.Resources.wood, 0},
        {MyEnum.Resources.cotton, 0}, {MyEnum.Resources.coal, 0}, {MyEnum.Resources.spice, 0},
        {MyEnum.Resources.dyes, 0}, {MyEnum.Resources.rubber, 0}, {MyEnum.Resources.oil, 0}
    };

        private Dictionary<MyEnum.Resources, int> resourceBids = new Dictionary<MyEnum.Resources, int>()
        {{MyEnum.Resources.wheat, 0}, {MyEnum.Resources.meat, 0},
        {MyEnum.Resources.fruit, 0}, {MyEnum.Resources.iron, 0}, {MyEnum.Resources.wood, 0},
        {MyEnum.Resources.cotton, 0}, {MyEnum.Resources.coal, 0}, {MyEnum.Resources.spice, 0},
        {MyEnum.Resources.dyes, 0}, {MyEnum.Resources.rubber, 0}, {MyEnum.Resources.oil, 0}
    };

        private Dictionary<MyEnum.Resources, int> resourceOffers = new Dictionary<MyEnum.Resources, int>()
        {{MyEnum.Resources.wheat, 0}, {MyEnum.Resources.meat, 0},
        {MyEnum.Resources.fruit, 0}, {MyEnum.Resources.iron, 0}, {MyEnum.Resources.wood, 0},
        {MyEnum.Resources.cotton, 0}, {MyEnum.Resources.coal, 0}, {MyEnum.Resources.spice, 0},
        {MyEnum.Resources.dyes, 0}, {MyEnum.Resources.rubber, 0}, {MyEnum.Resources.oil, 0}
    };

        private Dictionary<MyEnum.Goods, float> goods = new Dictionary<MyEnum.Goods, float>()
    {
        {MyEnum.Goods.arms, 0}, {MyEnum.Goods.auto, 0},
        {MyEnum.Goods.chemicals, 0}, {MyEnum.Goods.clothing, 0}, {MyEnum.Goods.fabric, 0},
        {MyEnum.Goods.fighter, 0}, {MyEnum.Goods.furniture, 0},
            { MyEnum.Goods.lumber, 0},{ MyEnum.Goods.gear, 0},
        {MyEnum.Goods.paper, 0}, {MyEnum.Goods.parts, 0},
        {MyEnum.Goods.steel, 0}, {MyEnum.Goods.tank, 0}, {MyEnum.Goods.telephone, 0}
    };

        private Dictionary<MyEnum.Goods, int> goodOffers = new Dictionary<MyEnum.Goods, int>()
    {
        {MyEnum.Goods.arms, 0}, {MyEnum.Goods.auto, 0},
        {MyEnum.Goods.chemicals, 0}, {MyEnum.Goods.clothing, 0}, {MyEnum.Goods.fabric, 0},
        {MyEnum.Goods.fighter, 0}, {MyEnum.Goods.furniture, 0},
            { MyEnum.Goods.lumber, 0}, { MyEnum.Goods.gear, 0},
        {MyEnum.Goods.paper, 0}, {MyEnum.Goods.parts, 0},
        {MyEnum.Goods.steel, 0}, {MyEnum.Goods.tank, 0}, {MyEnum.Goods.telephone, 0}
    };

        private Dictionary<MyEnum.Goods, int> goodBids = new Dictionary<MyEnum.Goods, int>()
    {
        {MyEnum.Goods.arms, 0}, {MyEnum.Goods.auto, 0},
        {MyEnum.Goods.chemicals, 0}, {MyEnum.Goods.clothing, 0}, {MyEnum.Goods.fabric, 0},
        {MyEnum.Goods.fighter, 0}, {MyEnum.Goods.furniture, 0},
            { MyEnum.Goods.lumber, 0}, { MyEnum.Goods.gear, 0},
        {MyEnum.Goods.paper, 0}, {MyEnum.Goods.parts, 0},
        {MyEnum.Goods.steel, 0}, {MyEnum.Goods.tank, 0}, {MyEnum.Goods.telephone, 0}
    };


        private MilitaryForm militaryForm = new MilitaryForm();
        private List<Army> armies = new List<Army>();
        private List<Fleet> fleets = new List<Fleet>();
        private int shipYardLevel = 0;

        private Dictionary<MyEnum.ArmyUnits, int> armyProducing = new Dictionary<MyEnum.ArmyUnits, int>()
        {
            {MyEnum.ArmyUnits.infantry, 0}, {MyEnum.ArmyUnits.artillery, 0}, {MyEnum.ArmyUnits.cavalry, 0},
            {MyEnum.ArmyUnits.fighter, 0}, {MyEnum.ArmyUnits.tank, 0}
        };

        private Dictionary<MyEnum.NavyUnits, int> navyProducing = new Dictionary<MyEnum.NavyUnits, int>()
        {
            {MyEnum.NavyUnits.frigates, 0}, {MyEnum.NavyUnits.ironclad, 0}, {MyEnum.NavyUnits.dreadnought, 0 }
        };

        private Dictionary<int, int> claimsRecognized = new Dictionary<int, int>();


        private int maxFort = 1;
        private float orgFactor;
        private int factoryThroughPut;
        private float productionModifier;
        //
        private int maxWarehouseCapacity;
        private int warehouseCapacity;
        private float warehouseUsed;

        private List<TacticCard> tacticCards = new List<TacticCard>();
        private int maxTacticHandSize;

        private int armyLevel;
        private int colonialLevel;
        private int researchLevel;
        private int investmentLevel;
        private int cultureLevel;

        private List<MyEnum.cultCard> cultureCards = new List<MyEnum.cultCard>();
        private int maxCultHandSize;

        private float currentTotalBiddingCost;


        private float clothingQuality;
        private float furnatureQuality;

        public Nation()
        {

        }
        public static Nation CreateFromJSON(string jsonString)
        {
            return JsonUtility.FromJson<Nation>(jsonString);
        }


        public Nation(string nationName, int index, float[] color, MyEnum.NationType type, List<int> provinceIndexes, string culture)
        {
            this.nationName = nationName;
            this.index = index;
            // Color _color = new Color(color[0], color[1], color[2], color[3]);
            this.Color = new Color(color[0], color[1], color[2], color[3]);
            this.type = type;
            this.provinceIndexes = provinceIndexes;
            this.culture = culture;
            this.industry = new PlayerIndustry();
        }

       

        public float ColonialPoints
        {
            get
            {
                return colonialPoints;
            }
            set
            {
                colonialPoints = value;
            }
        }

        public float InfulencePoints
        {
            get
            {
                return infulencePoints;
            }
            set
            {
                infulencePoints = value;
            }
        }

    public void receiveGold(float amount)
        {
            this.gold += amount;
        }

    public void payGold(float amount)
        {
            this.gold += amount;
        }
    
    public float getGold()
        {
            return this.gold;
        }

        public float Research
        {
            get
            {
                return research;
            }
            set
            {
                research = value;
            }
        }

        public void AddResearchPoints(float amount)
        {
            this.research += amount;
        }

        public void SpendResearchPoints(float amount)
        {
            this.research -= amount;
        }


        public float Stability
        {
            get
            {
                return stability;
            }

            set
            {
                stability = value;
            }
        }

        public int DiplomacyPoints
        {
            get
            {
                return diplomacyPoints;
            }

            set
            {
                diplomacyPoints = value;
            }
        }

        public int Reputation
        {
            get
            {
                return reputation;
            }

            set
            {
                reputation = value;
            }
        }







        public int getIndex()
        {
            return this.index;
        }

        public string getName()
        {
            return this.nationName;
        }



        public void addProvinceIndex(int index)
        {
            this.provinceIndexes.Add(index);
        }

        public int getProvinceIndex(int index)
        {
            return this.provinceIndexes[index];
        }

        public List<int> getAllProvinceIndexes()
        {
            return this.provinceIndexes;
        }

        public void setColor(Color color)
        {
            this.Color = color;
        }

        public Color getColor()
        {
            return this.Color;
        }

        public string getNationName()
        {
            return this.nationName;
        }

        public bool IsHuman()
        {
            return this.human;
        }

        public void SetHuman(bool value)
        {
            this.human = value;
        }

        public List<int> getProvinces()
        {
            return this.provinceIndexes;
        }


        public void setResourceOfferItem(MyEnum.Resources resource, int value)
        {
            this.resourceOffers[resource] = value;
        }

        public void setGoodOfferItem(MyEnum.Goods good, int value)
        {
            this.goodOffers[good] = value;
        }

        public void setResourceBidItem(MyEnum.Resources resource, int value)
        {
            this.resourceOffers[resource] = value;
        }

        public void setGoodBidItem(MyEnum.Goods good, int value)
        {
            this.goodOffers[good] = value;
        }

        public float getNumberResource(MyEnum.Resources resource)
        {
            return this.resources[resource];
        }

        public float getNumberGood(MyEnum.Goods good)
        {
            return this.goods[good];
        }

        public void setNumberResource(MyEnum.Resources resource, int value)
        {
            this.resources[resource] = value;
        }

        public void setNumberGood(MyEnum.Goods good, int value)
        {
            this.goods[good] = value;
        }

        public float getTotalPOP()
        {
            return this.totalPOP;
        }

        public void setTotalPOP(int population)
        {
            this.totalPOP = population;

        }

        public float getAP()
        {
            return this.AP;
        }

        public void setAP(float AP)
        {
            this.AP = AP;
        }

        public void addAP(int amount)
        {
            this.AP += amount;
        }

        public void consumeResource(MyEnum.Resources resource, float amount)
        {
            resources[resource] -= amount;
        }

        public void consumeGoods(MyEnum.Goods good, float amount)
        {
            goods[good] -= amount;
        }

        public void collectResource(MyEnum.Resources resource, float amount)
        {
            resources[resource] += amount;
        }

        public void collectGoods(MyEnum.Goods good, float amount)
        {
            goods[good] += amount;
        }

        public HashSet<string> GetTechnologies()
        {
            return this.technologies;
        }

        public void AddTechnology(string newTech)
        {
            this.technologies.Add(newTech);

        }

        public MilitaryForm GetMilitaryForm()
        {
            return this.militaryForm;
        }

        public void IncreaseMaxFort()
        {
            this.maxFort += 1;
        }

        public int GetMaxFort()
        {
            return this.maxFort;
        }

        public int GetFactoryThroughput()
        {
            return this.factoryThroughPut;
        }

        public void IncreaseFactoryThroughput(int increase)
        {
            this.factoryThroughPut += increase;
        }

        public float GetProductionModifier()
        {
            return this.productionModifier;
        }

        public void InceaseProductionModifier(float increase)
        {
            this.productionModifier += increase;
        }

        public float OrgFactory()
        {
            return this.orgFactor;
        }

        public void IncreaseOrgFactor(float increase)
        {
            this.orgFactor += increase;
        }



        public float GetCorruption()
        {
            return this.corruption;
        }

        public void ChangeCorruption(float change)
        {
            this.corruption += change;
        }

        public List<Army> GetArmies()
        {
            return this.armies;
        }

        public Army GetArmy(int index)
        {
            foreach(Army army in armies)
            {
                if(army.GetIndex() == index)
                {
                    return army;
                }
            }
            return armies[0];
        }

        public List<Fleet> GetFleets()
        {
            return this.fleets;
        }

        public Fleet GetFleet(int index)
        {
            foreach(Fleet fleet in fleets)
            {
                if (fleet.getIndex() == index)
                {
                    return fleet;
                }
            }
            return fleets[0];
        }

        public void addArmy(Army army)
        {
            this.armies.Add(army);
        }

        public void addFleet(Fleet fleet)
        {
            this.fleets.Add(fleet);
        }

        public void addArmyProducing(MyEnum.ArmyUnits unit)
        {
            this.armyProducing[unit] += 1;
        }

        public void addNavyProducing(MyEnum.NavyUnits unit)
        {
            this.navyProducing[unit] += 1;
        }

        public int getArmyProducing(MyEnum.ArmyUnits unit)
        {
            return this.armyProducing[unit];
        }

        public void setArmyProducing(MyEnum.ArmyUnits unit, int amount)
        {
            this.armyProducing[unit] = amount;
        }

        public void setNavyProducing(MyEnum.NavyUnits unit, int amount)
        {
            this.navyProducing[unit] = amount;
        }

        public int getNavyProducing(MyEnum.NavyUnits unit)
        {
            return this.navyProducing[unit];
        }

        public int GetShipyardLevel()
        {
            return this.shipYardLevel;
        }

        public void UpgradeShipyard()
        {
            this.consumeGoods(MyEnum.Goods.lumber, 1);
            this.consumeGoods(MyEnum.Goods.parts, 1);
            this.UseAP(1);
            int currentLevel = GetShipyardLevel();
            if (currentLevel == 2)
            {
                this.consumeGoods(MyEnum.Goods.steel, 1);
                this.consumeGoods(MyEnum.Goods.parts, 1);
                this.UseAP(1);

            }
            this.shipYardLevel += 1;
        }


        public void UseAP(int amount)
        {
            this.AP -= amount;
        }

        public void SetShipyardLevel(int level)
        {
            this.shipYardLevel = level;
        }

        public MyEnum.NationType getType()
        {
            return this.type;
        }

        public int getNumberPattents()
        {
            return this.numberPattents;
        }

        public void addPattent()
        {
            this.numberPattents++;
        }


        public void IncreaseMaxWareHouseCapacity(int amount)
        {
            this.maxWarehouseCapacity += amount;
        }

        public int GetMaxWarehouseCapacity()
        {
            return this.maxWarehouseCapacity;
        }

        public void IncreaseWarehouseCapacity(int change)
        {
            this.warehouseCapacity += change;
        }

        public int GetCurrentWarehouseCapacity()
        {
            return this.warehouseCapacity;
        }

        public void ChangeWarehouseUsed(float change)
        {
            this.warehouseUsed += change;
        }

        public float GetWarehouseUsed()
        {
            return this.warehouseUsed;
        }

        public void addTacticCard(TacticCard card)
        {
            this.tacticCards.Add(card);
        }

        public List<TacticCard> getTacticCards()
        {
            return this.tacticCards;
        }

        public void removeTacticCard(TacticCard card)
        {
            this.tacticCards.Remove(card);
        }

        public void increaseResearchLevel()
        {
            this.researchLevel++;
        }

        public void increaseCultureLevel()
        {
            this.cultureLevel++;
        }

        public void increaseArmyLevel()
        {
            this.armyLevel++;
        }

        public void increaseInvestmentLevel()
        {
            this.investmentLevel++;
        }

        public void increaseColonialLevel()
        {
            this.colonialLevel++;
        }

        public int getResearchLevel()
        {
            return this.researchLevel;
        }

        public int getCulureLevel()
        {
            return this.cultureLevel;
        }

        public int getArmyLevel()
        {
            return this.armyLevel;
        }

        public int getInvestmentLevel()
        {
            return this.investmentLevel;
        }

        public int getColonialLevel()
        {
            return this.colonialLevel;
        }


        public void setMaximumTacticHandSize(int size)
        {
            this.maxTacticHandSize = size;
        }

        public int getMaximumTacticHandSize()
        {
            return this.maxTacticHandSize;
        }

        public void AddColonialPoints(float amount)
        {
            this.colonialPoints += amount;
        }

        public void SpendColonialPoints(float amount)
        {
            this.colonialPoints -= amount;
        }

        public float GetColonialPoints()
        {
            return this.colonialPoints;
        }


        public void addColony(int colony)
        {
            this.colonies.Add(colony);
        }

        public List<int> getColonies()
        {
            return this.colonies;
        }

        public void removeColony(int colony)
        {
            this.colonies.Remove(colony);
        }

      

        public void addSphere(int sphere)
        {
            this.spheres.Add(sphere);
        }

        public void removeSphere(int sphere)
        {
            this.spheres.Remove(sphere);
        }

        public List<int> getSpheres()
        {
            return this.spheres;
        }

        public void addIP(float amount)
        {
            this.IP += amount;
        }

        public void useIP(float amount)
        {
            this.IP -= amount;
        }

        public float getIP()
        {
            return this.IP;
        }

        public void addPatent(string tech)
        {
            this.patents.Add(tech);
        }

        public HashSet<string> getPatents()
        {
            return this.patents;
        }

        public void addPOPIncreasedThisTurn()
        {
            this.popIncreasedThisTurn++;

        }

        public void clearPOPIncreasedThisTurn()
        {
            this.popIncreasedThisTurn = 0;
        }

        public int getPOPIncreasedThisTurn()
        {
            return this.popIncreasedThisTurn;
        }

        public void increasePOP(int amount)
        {
            this.totalPOP += amount;
            this.urbanPOP += amount;
        }

        public void AddMilitaryPOP()
        {
            this.militaryPOP += 0.2f;
        }

        public void removeMilitaryPOP()
        {
            this.militaryPOP -= 1;
        }

        public float getMilitaryPOP()
        {
            return this.militaryPOP;
        }

        public void AddUrbanPOP()
        {
            this.urbanPOP += 1;
        }

        public void setUrbanPOP(int amount)
        {
            this.urbanPOP = amount;
        }

        public void removeUrbanPOP(float amount)
        {
            this.urbanPOP -= 1;
        }

        public int getUrbanPOP()
        {
            return this.urbanPOP;
        }

        public void addRuralPOP()
        {
            this.ruralPOP += 1;
        }

        public void removeRuralPOP()
        {
            this.ruralPOP -= 1;
        }

        public int getRuralPOP()
        {
            return this.ruralPOP;
        }

        public void addCultureCard(MyEnum.cultCard cardName)
        {
            this.cultureCards.Add(cardName);
        }

        public List<MyEnum.cultCard> getCultureCards()
        {
            return this.cultureCards;
        }

        public void removeCultureCard(MyEnum.cultCard card)
        {
            this.cultureCards.Remove(card);
        }

        public int numberOfResourcesAndGoods()
        {
            float number = 0f;
            foreach (MyEnum.Resources item in Enum.GetValues(typeof(MyEnum.Resources)))
            {
                if (item == MyEnum.Resources.gold)
                {
                    continue;
                }
                number += this.getNumberResource(item);
            }
            foreach (MyEnum.Goods item in Enum.GetValues(typeof(MyEnum.Goods)))
            {
                number += this.getNumberGood(item);
            }
            int stuff = (int)Math.Floor(number);
            return stuff;
        }

        public void increaseTotalCurrentBiddingCost(float amount)
        {
            currentTotalBiddingCost += amount;
        }

        public float getTotalCurrentBiddingCost()
        {
            return currentTotalBiddingCost;
        }


        public void resetTotalCurrentBiddingCost()
        {
            currentTotalBiddingCost = 0;
        }

        public void increasePrestige(int amount)
        {
            prestige += amount;
        }

        public void decreasePrestige(int amount)
        {
            prestige -= amount;
        }

        public int getPrestige()
        {
            return prestige;
        }


        public void adjustFurnitureQuality(float amount)
        {
            float newFurnQuality = this.furnatureQuality * (1 + amount);
            this.furnatureQuality = newFurnQuality;
        }

        public float getFurnitureQuality()
        {
            return this.furnatureQuality;
        }

        public void adjustClothingQuality(float amount)
        {
            float newClothQual = this.clothingQuality * (1 + amount);
            this.clothingQuality = newClothQual;
        }

        public float getClothingQuality()
        {
            return this.clothingQuality;
        }

        public int getMilitaryScore()
        {
            return this.militaryScore;
        }

        public int getIndustrialScore()
        {
            return this.industrialScore;
        }

        public void setMilitaryScore()
        {
            this.militaryScore = State.history.CalculateMilitaryScore(this);
        }

        public void setIndustrialScore()
        {
            this.industrialScore = State.history.CalculateIndustrialScore(this);
        }

        public List<WarClaim> getWarClaims()
        {
            return this.warClaims;
        }

        public void orderWarClaims()
        {
            this.warClaims = this.warClaims.OrderBy(o => o.getValue()).ToList();

        }

        public List<WarClaim> getWarClaimsOnNation(Nation otherNation)
        {
            int otherIndex = otherNation.getIndex();
            List<WarClaim> warClaimsOnThisNation = new List<WarClaim>();
            foreach (WarClaim claim in this.warClaims)
            {
                if (claim.getOtherNation() == otherIndex)
                {
                    warClaimsOnThisNation.Add(claim);
                }
            }
            return warClaimsOnThisNation;
        }

        public Dictionary<int, Relation> getRelations()
        {
            return this.relations;
        }

        public Relation getRelationToThisPlayer(int otherIndex)
        {
            // Debug.Log("Index:" + otherIndex);
            return this.relations[otherIndex];
        }

        public Relation getRelationFromThisPlayer(int otherIndex)
        {
            Nation otherNation = State.getNations()[otherIndex];
            return otherNation.getRelations()[this.getIndex()];
        }


        public void addDiplomacyPoints(int amount)
        {
            this.diplomacyPoints += amount;
        }

        public int getDiplomacyPoints()
        {
            return this.diplomacyPoints;
        }

        public void useDiplomacyPoints(int amount)
        {
            this.diplomacyPoints -= amount;
        }

        public void addRecognizedClaim(int claimTarget, int claimMaker)
        {
            this.claimsRecognized[claimTarget] = claimMaker;
        }

        public int getRecognition(int claimTarget)
        {
            return claimsRecognized[claimTarget];
        }

        public AI getAI()
        {
            return ai;
        }

        public void setReliability(int score)
        {
            this.reliability = score;
        }

        public int getReliability()
        {
            return reliability;
        }

        public void adjustReliability(int amount)
        {
            this.reliability += amount;
        }

        public void adjustReputation(int amount)
        {
            this.reputation += amount;
        }

        public void setTotalScore()
        {
            this.totalScore = this.prestige + this.militaryScore + this.industrialScore;
        }

        public int getTotalScore()
        {
            return this.totalScore;
        }


        public void increaseUnemployment(float growth)
        {
            this.unemployed += growth;
        }

        public void reduceUnemployment(int recruited)
        {
            this.unemployed -= recruited;
        }


        public float getUnemployed()
        {
            return this.unemployed;
        }

        public void setFoodShortage(float shortage)
        {
            this.foodShortage = shortage;
        }

        public float getFoodShortage()
        {
            return this.foodShortage;
        }

        public void setMilitaryPOP(int amount)
        {
            this.militaryPOP = amount;
        }

        public void setRuralPOP(int amount)
        {
            this.ruralPOP = amount;
        }

        public int IsColonyOf()
        {
            return this.isColonyOf;
        }

        public void MakeColonyOf(int possessor)
        {
            this.isColonyOf = possessor;
        }

        public int IsSphereOf()
        {
            return this.isSphereOf;
        }

        public void MakeSphereOf(int possessor)
        {
            this.isSphereOf = possessor;
        }



        public List<int> getLoveToHateList()
        {
            return this.orderedRelationLoveToHate;
        }

        public void addToImportValue(float amount)
        {
            this.importValue += amount;
        }

        public float getImportValues()
        {
            return this.importValue;
        }

        public void clearImportAndExportValues()
        {
            this.importValue = 0;
            this.exportValue = 0;
        }

        public void addToExportValue(float amount)
        {
            this.exportValue += amount;
        }

        public float getExportValue()
        {
            return this.exportValue;
        }

        public void addImport(Signal signal)
        {
            this.imports.Add(signal);
        }

        public void addExport(Signal signal)
        {
            this.exports.Add(signal);
        }

        public List<Signal> getImports()
        {
            return imports;
        }

        public List<Signal> getExports()
        {
            return exports;
        }

        public float getOilNeeded()
        {
            return this.oilNeeded;
        }

        public void increaseOilNeeded()
        {
            this.oilNeeded += 0.2f;
        }

        public void increaseFoodShortage(float amount)
        {
            this.foodShortage += amount;
        }

        public void setFoodImblance(float amount)
        {
            this.foodImbalance = amount;
        }

        public void increaseFoodImbalance(float amount)
        {
            this.foodImbalance += amount;
        }

        public float getFoodImbalance()
        {
            return this.foodImbalance;
        }

        public void setNewNavyPosition(Vector2 position)
        {
            this.newNavyPosition = position;
        }

        public Vector2 getNewNavalPosition()
        {
            return this.newNavyPosition;
        }

       

    }

}

  


