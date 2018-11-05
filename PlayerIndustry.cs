using assemblyCsharp;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIndustry  {

    private Dictionary<MyEnum.Goods, int> factories = new Dictionary<MyEnum.Goods, int>()
        {
            {MyEnum.Goods.arms, 0}, {MyEnum.Goods.auto, 0},
        {MyEnum.Goods.chemicals, 0}, {MyEnum.Goods.clothing, 0}, {MyEnum.Goods.fabric, 0},
        {MyEnum.Goods.fighter, 0}, {MyEnum.Goods.furniture, 0},
            { MyEnum.Goods.lumber, 0}, { MyEnum.Goods.gear, 0},
        {MyEnum.Goods.paper, 0}, {MyEnum.Goods.parts, 0},
        {MyEnum.Goods.steel, 0}, {MyEnum.Goods.tank, 0}, {MyEnum.Goods.telephone, 0}
        };

    private Dictionary<MyEnum.Goods, int> goodsProducing = new Dictionary<MyEnum.Goods, int>()
        {
            {MyEnum.Goods.arms, 0}, {MyEnum.Goods.auto, 0},
        {MyEnum.Goods.chemicals, 0}, {MyEnum.Goods.clothing, 0}, {MyEnum.Goods.fabric, 0},
        {MyEnum.Goods.fighter, 0}, {MyEnum.Goods.furniture, 0},
            { MyEnum.Goods.lumber, 0}, { MyEnum.Goods.gear, 0},
        {MyEnum.Goods.paper, 0}, {MyEnum.Goods.parts, 0},
        {MyEnum.Goods.steel, 0}, {MyEnum.Goods.tank, 0}, {MyEnum.Goods.telephone, 0}
        };

    public void setFactoryLevel(MyEnum.Goods type, int level)
    {
        this.factories[type] = level;
    }

    public int getFactoryLevel(MyEnum.Goods type)
    {
        return this.factories[type];
    }

    public int getGoodProducing(MyEnum.Goods good)
    {
        return this.goodsProducing[good];
    }

    public void setGoodProducing(MyEnum.Goods good, int number)
    {
        this.goodsProducing[good] = number;
    }

    public void upgradeFactory(MyEnum.Goods good)
    {
        this.factories[good] += 1;
    }


    public float determineCanProduce(MyEnum.Goods good, Nation player)
    {
        Dictionary<string, float> costs = ProductionCosts.GetCosts(good);
        float bottleNeck = 1000f;
        float nextComponent = 0.0f;
        float ratio;
        float materialMod = DetermineMaterialMod(player);
        foreach (string item in costs.Keys)
        {
            if (Enum.IsDefined(typeof(MyEnum.Goods), item))
            {
                MyEnum.Goods itemType = (MyEnum.Goods)System.
                  Enum.Parse(typeof(MyEnum.Goods), item);
                nextComponent = player.getNumberGood(itemType) * materialMod;
                ratio = nextComponent / costs[item];

            }
            else
            {
                MyEnum.Resources itemType = (MyEnum.Resources)System.
                Enum.Parse(typeof(MyEnum.Resources), item);
                nextComponent = player.getNumberResource(itemType) * materialMod;
                ratio = nextComponent / costs[item];
            }
            if (ratio < bottleNeck)
            {
                bottleNeck = ratio;
            }
        }
        int value = 0;
        int factoryLevel = getFactoryLevel(good);
        if (factoryLevel == 0)
        {
            value = 0;
        }
        if (factoryLevel == 1)
        {
            value = (int)Math.Min(4, bottleNeck);
        }
        if (factoryLevel == 2)
        {
            value = (int)Math.Min(8, bottleNeck);
        }
        float corruptionFactor = PlayerCalculator.getCorruptionFactor(player);
        float industrialExpFactor = PlayerCalculator.getIndustrialExperienceFactor(player);
        /*Later additional modifiers will affect this including:
         * Corruptuon, happiness, management level, and perhaps some 
         * technologies */
        value = (int)(value * corruptionFactor * industrialExpFactor);
        return value;
    }

    public bool CheckIfCanUpgradeFactory(MyEnum.Goods good)
    {
        App app = UnityEngine.Object.FindObjectOfType<App>();
        int humanIndex = app.GetHumanIndex();
        Nation player = State.getNations()[humanIndex];
        if (!player.GetTechnologies().Contains("high_power_steam_engine") || player.getAP() < 1 || getFactoryLevel(good) == 3 ||
            player.getNumberGood(MyEnum.Goods.parts) < 1 || player.getNumberGood(MyEnum.Goods.lumber) < 1 || player.getIP() < 1)
        {
            return false;
        }
        if (good == MyEnum.Goods.gear)
        {
            if (!player.GetTechnologies().Contains("electricity"))
            {
                return false;
            }
        }
        if (good == MyEnum.Goods.chemicals)
        {
            if (!player.GetTechnologies().Contains("chemistry"))
            {
                return false;
            }
        }
        if (good == MyEnum.Goods.telephone)
        {
            if (!player.GetTechnologies().Contains("telephone"))
            {
                return false;
            }
        }
        if (good == MyEnum.Goods.auto)
        {
            if (!player.GetTechnologies().Contains("automobile"))
            {
                return false;
            }
        }
        if (good == MyEnum.Goods.fighter)
        {
            if (!player.GetTechnologies().Contains("flight"))
            {
                return false;
            }
        }
        if (good == MyEnum.Goods.tank)
        {
            if (!player.GetTechnologies().Contains("mobile_warfare"))
            {
                return false;
            }
        }
        if (getFactoryLevel(good) == 1)
        {
            if (player.getNumberGood(MyEnum.Goods.lumber) < 2 || player.getNumberGood(MyEnum.Goods.parts) < 2 ||
                player.getIP() < 2)
            {
                return false;
            }
            if (good == MyEnum.Goods.parts || good == MyEnum.Goods.arms || good == MyEnum.Goods.steel)
            {
                if (!player.GetTechnologies().Contains("bessemer_process"))
                {
                    return false;
                }
            }
            if (good == MyEnum.Goods.paper)
            {
                if (!player.GetTechnologies().Contains("pulping"))
                {
                    return false;
                }
            }
            if (good == MyEnum.Goods.clothing)
            {
                if (!player.GetTechnologies().Contains("sewing_machine"))
                {
                    return false;
                }
            }
            if (good == MyEnum.Goods.fabric)
            {
                if (!player.GetTechnologies().Contains("spinning_jenny"))
                {
                    return false;
                }
            }
            if (good == MyEnum.Goods.lumber || good == MyEnum.Goods.furniture)
            {
                if (!player.GetTechnologies().Contains("saw_mill"))
                {
                    return false;
                }
            }
        }
        if (getFactoryLevel(good) == 2)
        {
            if (player.getNumberGood(MyEnum.Goods.gear) < 2 || player.getAP() < 2 || player.getIP() < 3)
            {
                return false;
            }

            if (good == MyEnum.Goods.parts || good == MyEnum.Goods.arms)
            {
                if (!player.GetTechnologies().Contains("advanced_iron_working"))
                {
                    return false;
                }
            }
            if (good == MyEnum.Goods.paper || good == MyEnum.Goods.clothing)
            {
                if (!player.GetTechnologies().Contains("pulping"))
                {
                    return false;
                }
            }

            if (good == MyEnum.Goods.fabric)
            {
                if (!player.GetTechnologies().Contains("power_loom"))
                {
                    return false;
                }
            }
        }
        return true;
    }

      
    public float DetermineMaterialMod(Nation player)
    {
        float CORRUPTION_NORMALIZER = 0.035f;
        float corruptionLevel = player.GetCorruption();
        float corruptionFactor = 1 + (corruptionLevel * CORRUPTION_NORMALIZER);

        return corruptionFactor;

    }

    public void consumeGoodsMaterial(MyEnum.Goods good, Nation player)
    {
        Debug.Log("Consume materials for " + good.ToString());
        Dictionary<string, float> costs = ProductionCosts.GetCosts(good);
        foreach (string item in costs.Keys)
        {
            if (Enum.IsDefined(typeof(MyEnum.Goods), item))
            {
                MyEnum.Goods itemType = (MyEnum.Goods)System.
                    Enum.Parse(typeof(MyEnum.Goods), item);
                player.consumeGoods(itemType, costs[item]);

            }
            else
            {
                MyEnum.Resources itemType = (MyEnum.Resources)System.
                     Enum.Parse(typeof(MyEnum.Resources), item);
                player.consumeResource(itemType, costs[item]);
            }
        }
    }

    public void  returnGoodsMaterial(MyEnum.Goods good, Nation player)
    {
        Dictionary<string, float> costs = ProductionCosts.GetCosts(good);
        foreach (string item in costs.Keys)
        {
            if (Enum.IsDefined(typeof(MyEnum.Goods), item))
            {
                MyEnum.Goods itemType = (MyEnum.Goods)System.
                    Enum.Parse(typeof(MyEnum.Goods), item);
                player.collectGoods(itemType, costs[item]);

            }
            else
            {
                MyEnum.Resources itemType = (MyEnum.Resources)System.
                     Enum.Parse(typeof(MyEnum.Resources), item);
                player.collectResource(itemType, costs[item]);
            }
        }

    }


    public int getTotalNumberFactoryLevels()
    {
        int number = 0;
        foreach (MyEnum.Goods good in Enum.GetValues(typeof(MyEnum.Goods)))
        {
            number += getFactoryLevel(good);
        }
        return number;
    }




}
