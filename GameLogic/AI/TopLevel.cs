using assemblyCsharp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TopLevel {

    private MyEnum.Loyality loyality;
    private MyEnum.Protectionism protectionism;

    private Dictionary<MyEnum.macroPriorities, float> macroPriorities = new Dictionary<MyEnum.macroPriorities, float>();
    private Dictionary<MyEnum.Resources, float> resourcePriorities = new Dictionary<MyEnum.Resources, float>();
    private Dictionary<MyEnum.Goods, float> factoryPriorities = new Dictionary<MyEnum.Goods, float>();
    private Dictionary<MyEnum.Goods, float> productionPriority = new Dictionary<MyEnum.Goods, float>();
    private Dictionary<MyEnum.ArmyUnits, float> armyUnitPriority = new Dictionary<MyEnum.ArmyUnits, float>();
    private Dictionary<string, float> technologyPriority = new Dictionary<string, float>()
    {
        { "advanced_iron_working", 1.2f },
        { "automobile", 1.2f},
        {"barbed_wire", 1.0f },
        {"bessemer_process", 1.5f},
        {"biology", 1.0f},
        {"bolt_action_rifles", 1.25f},
        {"bombers", 1.0f },
        {"breech_loaded_arms", 1.2f},
        {"cement", 1.0f },
        {"chemistry", 1.2f },
        {"combustion", 1.2f },
        {"compound_steam_engin", 1.3f},
        {"cotton_gin", 1.0f },
        {"dynamite",  1.1f},
        {"electricity", 1.15f },
        {"fertilizer", 1.0f },
        {"flight", 1.0f },
        {"flintlock", 1.2f },
        {"heavy_armament", 1.1f },
        {"high_power_steam_engine", 1.3f },
        {"indirect_fire", 1.2f },
        {"ironclad", 1.2f },
        {"machine_guns", 1.2f },
        {"mechanical_reaper", 1.0f },
        {"medicine", 1.0f },
        {"mobile_warfare", 1.2f },
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
        {"steel_armor", 1.1f },
        {"steel_plows", 1.0f },
        {"synthetic_dyes", 1.0f },
        {"synthetic_oil", 1.0f },
        {"synthetic_rubber", 1.0f },
        {"telegraph", 1.1f },
        {"telephone", 1.15f },
    };

    private Dictionary<MyEnum.Resources, int> resourcesToKeep = new Dictionary<MyEnum.Resources, int>();

    private Dictionary<MyEnum.nationalDevelopmentTypes, float> nationalDevelopmentPriorities =
        new Dictionary<MyEnum.nationalDevelopmentTypes, float>();


    public MyEnum.Loyality GetLoyality()
    {
        return this.loyality;
    }

    public MyEnum.Protectionism GetProtectionism()
    {
        return this.protectionism;
    }

    public void SetLoyality(MyEnum.Loyality level)
    {
        this.loyality = level;
    }

    public void GetProtectionism(MyEnum.Protectionism level)
    {
        this.protectionism = level;
    }



    public void calculateProductionPriority(Nation player)
    {
        Market market = State.market;
        int turn = State.turn;
        foreach (MyEnum.Goods good in Enum.GetValues(typeof(MyEnum.Goods)))
        {
            this.productionPriority[good] = 0.0f;
        }
        foreach (MyEnum.Goods good in Enum.GetValues(typeof(MyEnum.Goods)))
        {
            if (market.getNumberGoodsOffered(good) < (4 + turn) && player.getNumberGood(good) < (2 + turn * 0.2f))
            {
                this.productionPriority[good] += 1;
            }
            if (market.getNumberGoodsOffered(good) < (3 + turn) * 0.66f && player.getNumberGood(good) < (1 + turn * 0.15f))
            {
                this.productionPriority[good] += 1;
            }
            if (market.getNumberGoodsOffered(good) < (2 + turn) * 0.4f && player.getNumberGood(good) < (1 + turn * 0.1f))
            {
                this.productionPriority[good] += 1;
            }
            if (market.getNumberGoodsOffered(good) < (1 + turn) * 0.1f && player.getNumberGood(good) < turn * 0.1f)
            {
                this.productionPriority[good] += 1;
            }
            if (good == MyEnum.Goods.tank || good == MyEnum.Goods.arms)
            {
                this.productionPriority[good] += 1;
            }

        }

    }


    public void setMacroPriority(Nation player, MyEnum.macroPriorities type, float value)
    {
        this.macroPriorities[type] = value;

    }

    public void alterMacroPriority(Nation player, MyEnum.macroPriorities type, float value)
    {
        this.macroPriorities[type] += value;
    }

    public float getMacroPriority(Nation player, MyEnum.macroPriorities type)
    {
        return this.macroPriorities[type];
    }

    public List<MyEnum.macroPriorities> getMacroPriorites(Nation player)
    {
       List<MyEnum.macroPriorities> priorities = new List<MyEnum.macroPriorities>();       
       var ordered = macroPriorities.OrderByDescending(x => x.Value);
        foreach (var item in ordered)
        {
            priorities.Add(item.Key);
        }
        return priorities;
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

    public void alterProductionPriority(Nation player, MyEnum.Goods good, float value)
    {
        this.productionPriority[good] += value;
    }

   






}
