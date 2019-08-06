using assemblyCsharp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TopLevel {

    private MyEnum.staticPriorities topPriority;
    private MyEnum.staticPriorities secondPriory;
    private MyEnum.militancy militancy;


  //  public Dictionary<MyEnum.progressPriorities, int> metaProgressPriorities = new Dictionary<MyEnum.progressPriorities, int>();

    public Dictionary<MyEnum.progressPriorities, int> progressPriorities = new Dictionary<MyEnum.progressPriorities, int>();
    public Dictionary<MyEnum.developmentPriorities, int> developmentPriorities = new Dictionary<MyEnum.developmentPriorities, int>();
    public Dictionary<MyEnum.productionPriorities, int> productTypePriorities = new Dictionary<MyEnum.productionPriorities, int>();
    public Dictionary<MyEnum.metaPriorities, int> metaPriorities = new Dictionary<MyEnum.metaPriorities, int>();

    private Dictionary<MyEnum.Resources, float> resourcePriorities = new Dictionary<MyEnum.Resources, float>();
    private Dictionary<MyEnum.Goods, float> factoryPriorities = new Dictionary<MyEnum.Goods, float>();
    private Dictionary<MyEnum.Goods, float> productionPriorities = new Dictionary<MyEnum.Goods, float>();
    private Dictionary<MyEnum.ArmyUnits, float> armyUnitPriority = new Dictionary<MyEnum.ArmyUnits, float>();
    private Dictionary<string, float> technologyPriority = new Dictionary<string, float>()
    {
        {"advanced_metalworking", 1.0f },
        { "automobile", 1.2f},
        {"barbed_wire", 1.0f },
        {"bessemer_process", 1.5f},
        {"biology", 1.0f},
        {"bolt_action_rifles", 1.25f},
        {"breech_loaded_arms", 1.2f},
        {"cement", 1.0f },
        {"chemistry", 1.2f },
        {"compound_steam_engine", 1.4f},
        {"combustion", 1.2f },
        {"cotton_gin", 1.0f },
        {"dynamite",  1.1f},
        {"electricity", 1.15f },
        {"fertilizer", 1.0f },
        {"flight", 1.0f },
        {"flintlock", 4.5f },
        {"heavy_armament", 1.1f },
        {"indirect_fire", 1.2f },
        {"ironclad", 1.2f },
        {"interchangeable_parts", 4.5f},
        {"machine_guns", 1.2f },
        {"mechanical_reaper", 1.0f },
        {"medicine", 1.0f },
       // {"mobile_warfare", 1.2f },
        {"oil_drilling", 1.2f },
        {"oil_powered_ships", 1.2f },
        {"physics", 1.0f },
        {"power_loom", 1.1f },
        {"premodern", 1.2f },
        {"pulping", 1.1f },
        {"radio", 1.0f },
        {"rotary_drilling", 1.0f },
        {"saw_mill", 1.0f },
        {"scientific_revolution", 1.1f },
        {"sewing_machine", 1.0f },
        {"spinning_jenny", 1.0f },
        {"square_timbering", 1.1f },
        {"steam_engine", 1.9f},
        //{ "steel_armor", 1.1f },
        {"steel_plows", 1.0f },
        {"steam_locomotive", 2.5f},
        {"synthetic_dyes", 1.0f },
        {"telegraph", 1.1f },
        {"telephone", 1.15f },
    };

    private Dictionary<MyEnum.Resources, int> resourcesToKeep = new Dictionary<MyEnum.Resources, int>();
    private Dictionary<MyEnum.Goods, int> goodsToKeep = new Dictionary<MyEnum.Goods, int>();

    private Dictionary<MyEnum.nationalDevelopmentTypes, float> nationalDevelopmentPriorities =
        new Dictionary<MyEnum.nationalDevelopmentTypes, float>();


    public void UpdateProgressPriorities()
    {
        if(this.topPriority == MyEnum.staticPriorities.army)
        {
            this.progressPriorities[MyEnum.progressPriorities.doctrines] += 2;
        }
        else if(this.secondPriory == MyEnum.staticPriorities.army)
            {
                this.progressPriorities[MyEnum.progressPriorities.doctrines] += 1;
            }
        else
        {
            float rand = UnityEngine.Random.Range(0, 1);
            if(rand > 0.5f)
            {
                this.progressPriorities[MyEnum.progressPriorities.doctrines] += 1;
            }
        }
        
    
        if(this.topPriority == MyEnum.staticPriorities.culture)
        {
            this.progressPriorities[MyEnum.progressPriorities.culture] += 3;
        }
        else if(this.secondPriory == MyEnum.staticPriorities.culture)
        {
            this.progressPriorities[MyEnum.progressPriorities.culture] += 2;
        }
        else
        {
            this.progressPriorities[MyEnum.progressPriorities.culture] += 1;
        }

        if(this.topPriority == MyEnum.staticPriorities.industry)
        {
            this.progressPriorities[MyEnum.progressPriorities.investment] += 3;
        }
        else if(this.secondPriory == MyEnum.staticPriorities.industry)
        {
            this.progressPriorities[MyEnum.progressPriorities.investment] += 2;
        }
        else
        {
            this.progressPriorities[MyEnum.progressPriorities.investment] += 1;
        }

        if(this.topPriority == MyEnum.staticPriorities.research)
        {
            this.progressPriorities[MyEnum.progressPriorities.research] += 3;
        }
        else if(this.secondPriory == MyEnum.staticPriorities.research)
        {
            this.progressPriorities[MyEnum.progressPriorities.research] += 2;
        }
        else
        {
            this.progressPriorities[MyEnum.progressPriorities.research] += 1;
        }
    }

    public void adjustDevelopmentPriorityInLightOfPotential(Nation player)
    {
        if (player.getIP() < 1 && State.era == MyEnum.Era.Early)
        {
            if (PlayerCalculator.canMakeDevelopmentAction(player))
            {
                PlayerPayer.payForDevelopmentAction(player, 1);
                player.addIP(2);
            }
            else
            {
                this.progressPriorities[MyEnum.progressPriorities.investment]++;
            }
        }

        if (player.getIP() < 2 && State.era != MyEnum.Era.Early)
        {
            if (PlayerCalculator.canMakeDevelopmentAction(player))
            {
                PlayerPayer.payForDevelopmentAction(player, 1);
                player.addIP(2);
            }
            else
            {
                this.progressPriorities[MyEnum.progressPriorities.investment]++;
            }
        }
    }

    public void UpdateDevelopmentPriorities(Nation player)
    {
        AdminAI admin = player.getAI().GetAdmin();
        int turn = State.turn;
        int numFactories = PlayerCalculator.getNumberOfFactories(player);
      //  int d = 5 - this.metaProgressPriorities[MyEnum.progressPriorities.investment];
        int possFact = admin.getPotentialFactoryUpgradeOptions(player).Count;

        if(possFact > 0)
        {
            adjustDevelopmentPriorityInLightOfPotential(player);
        }
        else
        {
            this.developmentPriorities[MyEnum.developmentPriorities.buildFactory]--;

        }

        if (numFactories / turn > 5 && possFact > 0)
        {
            this.developmentPriorities[MyEnum.developmentPriorities.buildFactory]++;
            this.metaPriorities[MyEnum.metaPriorities.development]++;
        }

        int provDevOpts = admin.getProviceDevelopmentOptions(player).Count;
        if (provDevOpts > 0)
        {
            adjustDevelopmentPriorityInLightOfPotential(player);
            this.developmentPriorities[MyEnum.developmentPriorities.developProvince] ++;
            this.metaPriorities[MyEnum.metaPriorities.development]++;


            if (this.topPriority == MyEnum.staticPriorities.industry)
            {
                this.developmentPriorities[MyEnum.developmentPriorities.developProvince] ++;
            }
            else if (this.secondPriory == MyEnum.staticPriorities.industry)
            {
                this.developmentPriorities[MyEnum.developmentPriorities.developProvince] ++;
            }
        }
        else
        {
            this.developmentPriorities[MyEnum.developmentPriorities.developProvince]--;
        }


        if (PlayerCalculator.canUpgradeFort(player))
        {
            {
                this.developmentPriorities[MyEnum.developmentPriorities.fortification]++;
                this.metaPriorities[MyEnum.metaPriorities.development]++;
            }
        }
        else
        {
            this.developmentPriorities[MyEnum.developmentPriorities.fortification]--;

        }
        if (player.GetTechnologies().Contains("steam_locomotive"))
        {
            adjustDevelopmentPriorityInLightOfPotential(player);
            int numberRailroads = PlayerCalculator.getNumberProvRailRoads(player);
            if (numberRailroads < (player.getProvinces().Count + player.getColonies().Count))
                this.metaPriorities[MyEnum.metaPriorities.development]++;
            {
                this.developmentPriorities[MyEnum.developmentPriorities.railroad]++;
                if (this.topPriority == MyEnum.staticPriorities.industry)
                {
                    this.developmentPriorities[MyEnum.developmentPriorities.railroad]++;
                }
                else if (this.secondPriory == MyEnum.staticPriorities.industry)
                {
                    this.developmentPriorities[MyEnum.developmentPriorities.railroad]++;
                }
            }
        }
    
        if (PlayerCalculator.canUpgradeShipyard(player))
        {
            this.developmentPriorities[MyEnum.developmentPriorities.shipyard]++;
            if(this.topPriority == MyEnum.staticPriorities.colonies)
            {
                this.developmentPriorities[MyEnum.developmentPriorities.shipyard]++;
            }
        }

        if ((PlayerCalculator.calculateTotalResourceProduction(player) - (player.getProvinces().Count + player.getColonies().Count)) > player.industry.getNumberOfTrains())
        {
            this.developmentPriorities[MyEnum.developmentPriorities.trains]++;
            if (this.topPriority == MyEnum.staticPriorities.industry)
            {
                this.developmentPriorities[MyEnum.developmentPriorities.trains]++;
            }
            else if (this.secondPriory == MyEnum.staticPriorities.industry)
            {
                this.developmentPriorities[MyEnum.developmentPriorities.trains]++;
            }
        }
      

        if (player.numberOfResourcesAndGoods() + PlayerCalculator.calculateTotalResourceProduction(player) > player.GetCurrentWarehouseCapacity())
        {
            this.developmentPriorities[MyEnum.developmentPriorities.warehouse] += 3;
        }
        else
        {
            this.developmentPriorities[MyEnum.developmentPriorities.warehouse]--;
        }
    }

    public void UpdateProductTypePriorities(Nation player)
    {
     if(this.topPriority == MyEnum.staticPriorities.colonies)
        {
            this.productTypePriorities[MyEnum.productionPriorities.buildShip] += 2;
        }
        else if (this.secondPriory == MyEnum.staticPriorities.colonies)
        {
            this.productTypePriorities[MyEnum.productionPriorities.buildShip] += 1;
        }
        else
        {
            float random = UnityEngine.Random.Range(0, 1);
            if(random <= 0.5f)
            {
                this.productTypePriorities[MyEnum.productionPriorities.buildShip] += 1;

            }
        }

        if (this.topPriority == MyEnum.staticPriorities.army)
        {
            this.productTypePriorities[MyEnum.productionPriorities.buildUnit]+=2;
        }
        else if(this.secondPriory == MyEnum.staticPriorities.army)
        {
            this.productTypePriorities[MyEnum.productionPriorities.buildUnit] ++;
        }
        float randNum2 = UnityEngine.Random.Range(0, 1);
        if (randNum2 > 0.5f)
        {
            this.productTypePriorities[MyEnum.productionPriorities.buildUnit]++;
        }

        this.productTypePriorities[MyEnum.productionPriorities.manifactureGoods]++;
    }

    public void UpdateMetaPriorities(Nation player)
    {
        if (this.topPriority == MyEnum.staticPriorities.army)
        {
            this.metaPriorities[MyEnum.metaPriorities.production] += 2;
        }
        if (this.secondPriory == MyEnum.staticPriorities.army)
        {
            this.metaPriorities[MyEnum.metaPriorities.production] += 1;
        }
        else
        {
            float rand = UnityEngine.Random.Range(0, 1);
            if (rand > 0.5f)
            {
                this.metaPriorities[MyEnum.metaPriorities.production] += 1;
            }
        }

        if(this.topPriority == MyEnum.staticPriorities.colonies)
            {
                this.metaPriorities[MyEnum.metaPriorities.production] += 2;
            }
            if (this.secondPriory == MyEnum.staticPriorities.colonies)
            {
                this.metaPriorities[MyEnum.metaPriorities.production] += 1;
            }
            else
            {
                float rand = UnityEngine.Random.Range(0, 1);
                if (rand > 0.5f)
                {
                    this.metaPriorities[MyEnum.metaPriorities.production] += 1;
                }
            }

        if(this.topPriority == MyEnum.staticPriorities.culture)
        {
            this.metaPriorities[MyEnum.metaPriorities.progress] += 2;
        }
        else if(this.secondPriory == MyEnum.staticPriorities.culture)
        {
            this.metaPriorities[MyEnum.metaPriorities.progress] += 1;
        }
        else
        {
            float rand = UnityEngine.Random.Range(0, 1);
            if (rand > 0.5f)
            {
                this.metaPriorities[MyEnum.metaPriorities.progress] += 1;
            }
        }

        if(this.topPriority == MyEnum.staticPriorities.industry)
        {
            this.metaPriorities[MyEnum.metaPriorities.development] += 2;
        }
        else if(this.secondPriory == MyEnum.staticPriorities.industry)
        {
            this.metaPriorities[MyEnum.metaPriorities.development] += 1;
        }
        else
        {
            float rand = UnityEngine.Random.Range(0, 1);
            if (rand > 0.5f)
            {
                this.metaPriorities[MyEnum.metaPriorities.development] += 1;
            }
        }

        
        if (this.topPriority == MyEnum.staticPriorities.research)
        {
            this.metaPriorities[MyEnum.metaPriorities.progress] += 2;
        }
        else if (this.secondPriory == MyEnum.staticPriorities.research)
        {
            this.metaPriorities[MyEnum.metaPriorities.progress] += 1;
        }
        else
        {
            float rand = UnityEngine.Random.Range(0, 1);
            if (rand > 0.5f)
            {
                this.metaPriorities[MyEnum.metaPriorities.progress] += 1;
            }
        }
     

    }



    public void updateProductionPriorities(Nation player)
    {
        Market market = State.market;
        int turn = State.turn;
       // foreach (MyEnum.Goods good in Enum.GetValues(typeof(MyEnum.Goods)))
       // {
       //     this.productionPriorities[good] = 0.0f;
      //  }
        foreach (MyEnum.Goods good in Enum.GetValues(typeof(MyEnum.Goods)))
        { 
            if(market.getDifferenceBetweenSupplyAndDemandGood(good) > 0)
            {
                this.productionPriorities[good] -= 0.1f;
            }
            if (market.getDifferenceBetweenSupplyAndDemandGood(good) < 0)
            {
                this.productionPriorities[good] += 0.1f;
            }

            if (market.getDifferenceBetweenSupplyAndDemandGood(good) > 2)
            {
                this.productionPriorities[good] -= 0.1f;
            }
            if (market.getDifferenceBetweenSupplyAndDemandGood(good) < -2)
            {
                this.productionPriorities[good] += 0.1f;
            }

            if (market.getDifferenceBetweenSupplyAndDemandGood(good) > 5)
            {
                this.productionPriorities[good] -= 0.1f;
            }
            if (market.getDifferenceBetweenSupplyAndDemandGood(good) < -5)
            {
                this.productionPriorities[good] += 0.1f;
            }
        }
    }


    public void setResPriority(Nation player, MyEnum.Resources type, float value)
    {
        this.resourcePriorities[type] = value;

    }

    public void alterResPriority(Nation player, MyEnum.Resources type, float value)
    {
        this.resourcePriorities[type] += value;
    }

    public float getResPriority(Nation player, MyEnum.Resources type)
    {
        return this.resourcePriorities[type];
    }

   

    public void setFactPriority(Nation player, MyEnum.Goods type, float value)
    {
        this.factoryPriorities[type] = value;

    }

    public void alterFactPriority(Nation player, MyEnum.Goods type, float value)
    {
        this.factoryPriorities[type] += value;
    }

    public float getFactPriority(Nation player, MyEnum.Goods type)
    {
        return this.factoryPriorities[type];
    }

    public List<MyEnum.Goods> getFactoryPriorities()
    {
        List<MyEnum.Goods> priorities = new List<MyEnum.Goods>();
        var ordered = factoryPriorities.OrderByDescending(x => x.Value);
        foreach (var item in ordered)
        {
            priorities.Add(item.Key);
        }
        return priorities;
    }

    public List<MyEnum.progressPriorities> getProgressPriorities()
    {
        List<MyEnum.progressPriorities> priorities = new List<MyEnum.progressPriorities>();
        var ordered = progressPriorities.OrderByDescending(x => x.Value);
        foreach (var item in ordered)
        {
            priorities.Add(item.Key);
        }
        return priorities;
    }

    public MyEnum.progressPriorities getTopProgressPriority()
    {
        List<MyEnum.progressPriorities> priorities = new List<MyEnum.progressPriorities>();
        var ordered = progressPriorities.OrderByDescending(x => x.Value);
        foreach (var item in ordered)
        {
            priorities.Add(item.Key);
        }
        return priorities[0];
    }



    public void increaseProgressPriority(MyEnum.progressPriorities priority)
    {
        progressPriorities[priority]++;
    }

    public MyEnum.developmentPriorities getTopDevelopmentPriority()
    {
        List<MyEnum.developmentPriorities> priorities = new List<MyEnum.developmentPriorities>();
        var ordered = developmentPriorities.OrderByDescending(x => x.Value);
        foreach (var item in ordered)
        {
            priorities.Add(item.Key);
        }
        return priorities[0];
    }

    public MyEnum.productionPriorities getTopProductionTypePriority()
    {
        List<MyEnum.productionPriorities> priorities = new List<MyEnum.productionPriorities>();
        var ordered = productTypePriorities.OrderByDescending(x => x.Value);
        foreach (var item in ordered)
        {
            priorities.Add(item.Key);
        }
        return priorities[0];
    }

    public MyEnum.metaPriorities getTopMetaPriority()
    {
        List<MyEnum.metaPriorities> priorities = new List<MyEnum.metaPriorities>();
        var ordered = metaPriorities.OrderByDescending(x => x.Value);
        foreach (var item in ordered)
        {
            priorities.Add(item.Key);
        }
        return priorities[0];
    }


    public MyEnum.productionPriorities getTopProductPriority()
    {
        List<MyEnum.productionPriorities> priorities = new List<MyEnum.productionPriorities>();
        var ordered = productTypePriorities.OrderByDescending(x => x.Value);
        foreach (var item in ordered)
        {
            priorities.Add(item.Key);
        }
        return priorities[0];
    }

    public MyEnum.Goods getTopFactoryPriority()
    {
        List<MyEnum.Goods> priorities = new List<MyEnum.Goods>();
        var ordered = factoryPriorities.OrderByDescending(x => x.Value);
        foreach (var item in ordered)
        {
            priorities.Add(item.Key);
        }
        return priorities[0];
    }

    public MyEnum.Goods getFactoryPriority(int ord)
    {
        List<MyEnum.Goods> priorities = new List<MyEnum.Goods>();
        var ordered = factoryPriorities.OrderByDescending(x => x.Value);
        foreach (var item in ordered)
        {
            priorities.Add(item.Key);
        }
        return priorities[ord];
    }


    public MyEnum.Resources getTopResourcePriority()
    {
        List<MyEnum.Resources> priorities = new List<MyEnum.Resources>();
        var ordered = resourcePriorities.OrderByDescending(x => x.Value);
        foreach (var item in ordered)
        {
            priorities.Add(item.Key);
        }
        return priorities[0];

    }

    public MyEnum.Resources getTopResourcePriorityN(int order)
    {
        List<MyEnum.Resources> priorities = new List<MyEnum.Resources>();
        var ordered = resourcePriorities.OrderByDescending(x => x.Value);
        foreach (var item in ordered)
        {
            priorities.Add(item.Key);
        }
        return priorities[order];

    }


    public void setArmyUnitPriority(Nation player, MyEnum.ArmyUnits type, float value)
    {
        this.armyUnitPriority[type] = value;

    }

    public void alterArmyUnitPriority(Nation player, MyEnum.ArmyUnits type, float value)
    {
        this.armyUnitPriority[type] += value;
    }

    public float getArmyUnitPriority(Nation player, MyEnum.ArmyUnits type)
    {
        return this.armyUnitPriority[type];
    }

    public void setProductionPriority(Nation player, MyEnum.Goods good, float value)
    {
        productionPriorities[good] = value;
    }

    public void alterProductionPriorities(Nation player, MyEnum.Goods good, float value)
    {
        this.productionPriorities[good] += value/20;
    }

    public MyEnum.Goods getTopProductionPriority(Nation player)
    {
        List<MyEnum.Goods> priorities = new List<MyEnum.Goods>();
        var ordered = productionPriorities.OrderByDescending(x => x.Value);
        foreach (var item in ordered)
        {
            priorities.Add(item.Key);
        }
        return priorities[0];
    }

    public List<MyEnum.Goods> getProductionPriorities(Nation player)
    {
        List<MyEnum.Goods> priorities = new List<MyEnum.Goods>();
        var ordered = productionPriorities.OrderByDescending(x => x.Value);
        foreach (var item in ordered)
        {
            priorities.Add(item.Key);
        }
        return priorities;
    }


    public void setResToKeep(MyEnum.Resources res, int value)
    {
        resourcesToKeep[res] = value;
    }

    public void adjustResToKeep(MyEnum.Resources res, int amount)
    {
        resourcesToKeep[res] += amount;
    }

    public float getResToKeep(MyEnum.Resources res)
    {
        return resourcesToKeep[res];
    }


    public void setGoodToKeep(MyEnum.Goods good, int value)
    {
        goodsToKeep[good] = value;
    }

    public void adjustGoodToKeep(MyEnum.Goods good, int amount)
    {
        goodsToKeep[good] += amount;
    }

    public float getGoodToKeep(MyEnum.Goods res)
    {
        return goodsToKeep[res];
    }


    public void alterPrioritiesBasedOnProvinceGain(Nation player, MyEnum.Resources res)
    {
        TopLevel aiTopLevel = player.getAI().GetTopLevel();
        aiTopLevel.alterResPriority(player, res, -0.65f);


        if(res == MyEnum.Resources.coal)
        {

            aiTopLevel.alterFactPriority(player, MyEnum.Goods.arms, 0.15f);
            aiTopLevel.alterFactPriority(player, MyEnum.Goods.steel, 0.25f);
            aiTopLevel.alterFactPriority(player, MyEnum.Goods.parts, 0.15f);
            aiTopLevel.alterFactPriority(player, MyEnum.Goods.chemicals, 0.5f);

            aiTopLevel.alterProductionPriorities(player, MyEnum.Goods.arms, 0.25f);
            aiTopLevel.alterProductionPriorities(player, MyEnum.Goods.steel, 0.4f);
            aiTopLevel.alterProductionPriorities(player, MyEnum.Goods.parts, 0.3f);
            aiTopLevel.alterProductionPriorities(player, MyEnum.Goods.chemicals, 0.4f);

            aiTopLevel.alterTechnologyPriority(player, "chemistry", 0.65f);
            aiTopLevel.alterTechnologyPriority(player, "fertilizer", 0.5f);
            aiTopLevel.alterTechnologyPriority(player, "medicine", 0.4f);
            aiTopLevel.alterTechnologyPriority(player, "synthetic_dyes", 0.5f);
           aiTopLevel.alterTechnologyPriority(player, "bessemer_process", 0.33f);
            aiTopLevel.alterTechnologyPriority(player, "steel_plows", 0.45f);
            aiTopLevel.alterTechnologyPriority(player, "square_timbering", 0.45f);
            aiTopLevel.alterTechnologyPriority(player, "steam_engine", 0.45f);
            aiTopLevel.alterTechnologyPriority(player, "compound_steam_engine", 0.45f);
           
        }

        if (res == MyEnum.Resources.cotton)
        {
            aiTopLevel.alterFactPriority(player, MyEnum.Goods.fabric, 0.6f);
            aiTopLevel.alterFactPriority(player, MyEnum.Goods.clothing, 0.45f);
            aiTopLevel.alterFactPriority(player, MyEnum.Goods.furniture, 0.25f);

            aiTopLevel.alterProductionPriorities(player, MyEnum.Goods.clothing, 0.5f);
            aiTopLevel.alterProductionPriorities(player, MyEnum.Goods.fabric, 0.55f);

            aiTopLevel.alterProductionPriorities(player, MyEnum.Goods.furniture, 0.25f);
            aiTopLevel.alterTechnologyPriority(player, "cotton_gin", 0.5f);
            aiTopLevel.alterTechnologyPriority(player, "power_loom", 0.5f);
            aiTopLevel.alterTechnologyPriority(player, "sewing_machine", 0.5f);
            aiTopLevel.alterTechnologyPriority(player, "spinning_jenny", 0.5f);
        }

        if(res == MyEnum.Resources.dyes)
        {
            aiTopLevel.alterFactPriority(player, MyEnum.Goods.clothing, 0.4f);
            aiTopLevel.alterProductionPriorities(player, MyEnum.Goods.clothing, 0.35f);

            aiTopLevel.alterTechnologyPriority(player, "synthetic_dyes", -0.5f);
            aiTopLevel.alterTechnologyPriority(player, "sewing_machine", 0.25f);
            aiTopLevel.alterTechnologyPriority(player, "spinning_jenny", 0.25f);
            aiTopLevel.alterTechnologyPriority(player, "fertilizer", 0.2f);

        }

        if (res == MyEnum.Resources.iron)
        {
            aiTopLevel.alterFactPriority(player, MyEnum.Goods.arms, 0.25f);
            aiTopLevel.alterFactPriority(player, MyEnum.Goods.steel, 0.4f);
            aiTopLevel.alterFactPriority(player, MyEnum.Goods.parts, 0.25f);
            aiTopLevel.alterFactPriority(player, MyEnum.Goods.gear, 0.2f);

            aiTopLevel.alterProductionPriorities(player, MyEnum.Goods.arms, 0.35f);
            aiTopLevel.alterProductionPriorities(player, MyEnum.Goods.steel, 0.6f);
            aiTopLevel.alterProductionPriorities(player, MyEnum.Goods.parts, 0.3f);
            aiTopLevel.alterProductionPriorities(player, MyEnum.Goods.gear, 0.3f);

            aiTopLevel.alterTechnologyPriority(player, "bessemer_process", 0.55f);
           aiTopLevel.alterTechnologyPriority(player, "advanced_metalworking", 0.33f);
            aiTopLevel.alterTechnologyPriority(player, "steam_engine", 0.34f);
            aiTopLevel.alterTechnologyPriority(player, "automobile", 0.25f);
            aiTopLevel.alterTechnologyPriority(player, "flight", 0.25f);
            aiTopLevel.alterTechnologyPriority(player, "square_timbering", 0.45f);
            aiTopLevel.alterTechnologyPriority(player, "dynamite", 0.25f);
            aiTopLevel.alterTechnologyPriority(player, "steam_locomotive", 0.2f);
            

        }

        if (res == MyEnum.Resources.oil)
        {


            aiTopLevel.alterFactPriority(player, MyEnum.Goods.auto, 0.5f);

            aiTopLevel.alterProductionPriorities(player, MyEnum.Goods.auto, 0.6f);

            aiTopLevel.alterTechnologyPriority(player, "oil_drilling", 0.75f);
            aiTopLevel.alterTechnologyPriority(player, "automobile", 0.35f);
            aiTopLevel.alterTechnologyPriority(player, "flight", 0.35f);


        }

        if (res == MyEnum.Resources.rubber)
        {
            aiTopLevel.alterFactPriority(player, MyEnum.Goods.gear, 0.6f);
            aiTopLevel.alterFactPriority(player, MyEnum.Goods.telephone, 0.45f);
            aiTopLevel.alterFactPriority(player, MyEnum.Goods.auto, 0.4f);

            aiTopLevel.alterProductionPriorities(player, MyEnum.Goods.gear, 0.85f);
            aiTopLevel.alterProductionPriorities(player, MyEnum.Goods.telephone, 0.65f);
            aiTopLevel.alterProductionPriorities(player, MyEnum.Goods.auto, 0.6f);

            aiTopLevel.alterTechnologyPriority(player, "electricity", 0.75f);
            aiTopLevel.alterTechnologyPriority(player, "automobile", 0.35f);
            aiTopLevel.alterTechnologyPriority(player, "flight", 0.25f);
            aiTopLevel.alterTechnologyPriority(player, "telephone", 0.55f);
            aiTopLevel.alterTechnologyPriority(player, "fertilizer", 0.2f);


        }

        if (res == MyEnum.Resources.wood)
        {
            aiTopLevel.alterFactPriority(player, MyEnum.Goods.lumber, 0.65f);
            aiTopLevel.alterFactPriority(player, MyEnum.Goods.paper, 0.55f);
            aiTopLevel.alterFactPriority(player, MyEnum.Goods.furniture, 0.35f);
            aiTopLevel.alterFactPriority(player, MyEnum.Goods.telephone, 0.2f);

            aiTopLevel.alterProductionPriorities(player, MyEnum.Goods.paper, 1.0f);
            aiTopLevel.alterProductionPriorities(player, MyEnum.Goods.lumber, 1.1f);

            aiTopLevel.alterProductionPriorities(player, MyEnum.Goods.furniture, 0.65f);
            aiTopLevel.alterProductionPriorities(player, MyEnum.Goods.telephone, 0.35f);


            aiTopLevel.alterTechnologyPriority(player, "pulping", 0.5f);
            aiTopLevel.alterTechnologyPriority(player, "compound_steam_engine", 0.35f);
            aiTopLevel.alterTechnologyPriority(player, "steam_engine", 0.35f);
        }

        if(res == MyEnum.Resources.wheat)
        {
            aiTopLevel.alterTechnologyPriority(player, "steel_plows", 0.35f);
            aiTopLevel.alterTechnologyPriority(player, "mechanical_reaper", 0.35f);
            aiTopLevel.alterTechnologyPriority(player, "fertilizer", 0.15f);
        }

        if (res == MyEnum.Resources.meat)
        {
            aiTopLevel.alterTechnologyPriority(player, "biology", 0.5f);
            aiTopLevel.alterTechnologyPriority(player, "barbed_wire", 0.5f);
        }

        if (res == MyEnum.Resources.fruit)
        {
            aiTopLevel.alterTechnologyPriority(player, "steel_plows", 0.35f);
            aiTopLevel.alterTechnologyPriority(player, "fertilizer", 0.25f);
        }


    }


    public void alterResourcesAndGoodsToKeepAfterBuildFactory(Nation player, MyEnum.Goods good)
    {

        if (good == MyEnum.Goods.arms || good == MyEnum.Goods.parts)
        {
            resourcesToKeep[MyEnum.Resources.iron] += 1;
            resourcesToKeep[MyEnum.Resources.coal] += 1;
            goodsToKeep[MyEnum.Goods.steel] += 4;
        }
        if (good == MyEnum.Goods.auto)
        {
            resourcesToKeep[MyEnum.Resources.iron] += 2;
            resourcesToKeep[MyEnum.Resources.rubber] += 2;
            resourcesToKeep[MyEnum.Resources.oil] += 6;
            goodsToKeep[MyEnum.Goods.gear] += 2;
            goodsToKeep[MyEnum.Goods.parts] += 2;
        }

        if (good == MyEnum.Goods.chemicals)
        {
            resourcesToKeep[MyEnum.Resources.coal] += 3;
            resourcesToKeep[MyEnum.Resources.dyes] -= 1;
        }

        if (good == MyEnum.Goods.clothing)
        {
            resourcesToKeep[MyEnum.Resources.cotton] += 1;
            resourcesToKeep[MyEnum.Resources.dyes] += 2;
            goodsToKeep[MyEnum.Goods.fabric] += 3;
        }

        if (good == MyEnum.Goods.fabric)
        {
            resourcesToKeep[MyEnum.Resources.cotton] += 3;
        }


        if (good == MyEnum.Goods.furniture)
        {
            resourcesToKeep[MyEnum.Resources.wood] += 1;
            goodsToKeep[MyEnum.Goods.lumber] += 2;
            goodsToKeep[MyEnum.Goods.fabric] += 1;
        }

        if (good == MyEnum.Goods.gear)
        {
            resourcesToKeep[MyEnum.Resources.rubber] += 5;
            resourcesToKeep[MyEnum.Resources.iron] += 1;

            goodsToKeep[MyEnum.Goods.steel] += 1;
        }

        if (good == MyEnum.Goods.lumber)
        { 
            resourcesToKeep[MyEnum.Resources.wood] += 4;
        }

        if(good == MyEnum.Goods.paper)
        {
            resourcesToKeep[MyEnum.Resources.wood] += 1;
            goodsToKeep[MyEnum.Goods.lumber] += 3;

        }

        if (good == MyEnum.Goods.steel)
        {
            resourcesToKeep[MyEnum.Resources.coal] += 2;
            resourcesToKeep[MyEnum.Resources.iron] += 4;

        }

        if (good == MyEnum.Goods.telephone)
        {
            resourcesToKeep[MyEnum.Resources.iron] += 1;
            resourcesToKeep[MyEnum.Resources.rubber] += 2;

            goodsToKeep[MyEnum.Goods.gear] += 4;
            goodsToKeep[MyEnum.Goods.lumber] += 2;
        }



    }


    public float getPriorityLevelOfTech(Nation player, string techName)
    {
        Debug.Log(techName);
        return technologyPriority[techName];
    }

    public void alterTechnologyPriority(Nation player, string name, float change)
    {
      Debug.Log(name);
        this.technologyPriority[name] += change;
    }

    public void setTechnologyPriority(Nation player, string name, float value)
    {
        this.technologyPriority[name] = value;
    }

    public MyEnum.staticPriorities getPrimaryFocus()
    {
        return this.topPriority;
    }

    public MyEnum.staticPriorities getSecondaryFocus()
    {
        return this.secondPriory;
    }

    public void setPrimaryFocus(MyEnum.staticPriorities priority)
    {
        this.topPriority = priority;
    }

    public void setSecondaryFocus(MyEnum.staticPriorities priority)
    {
        this.secondPriory = priority;
    }


    public void updateResourcesToKeepNewEra()
    {
        foreach (MyEnum.Resources res in Enum.GetValues(typeof(MyEnum.Resources)))
        {
            adjustResToKeep(MyEnum.Resources.wheat, 2);
            adjustResToKeep(MyEnum.Resources.fruit, 1);
            adjustResToKeep(MyEnum.Resources.meat, 1);
            adjustResToKeep(MyEnum.Resources.coal, 1);
            adjustResToKeep(MyEnum.Resources.oil, 2);
            adjustResToKeep(MyEnum.Resources.spice, 1);
        }
    }

    public void updateGoodsToKeepNewEra()
    {
        adjustGoodToKeep(MyEnum.Goods.arms, 2);
        adjustGoodToKeep(MyEnum.Goods.clothing, 2);
        adjustGoodToKeep(MyEnum.Goods.furniture, 1);
        adjustGoodToKeep(MyEnum.Goods.paper, 1);
        adjustGoodToKeep(MyEnum.Goods.telephone, 1);
        adjustGoodToKeep(MyEnum.Goods.auto, 1);
        adjustGoodToKeep(MyEnum.Goods.chemicals, 2);
    }
}
