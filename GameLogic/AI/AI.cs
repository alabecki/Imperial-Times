using assemblyCsharp;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI {

    private TopLevel topLevel = new TopLevel();
    private Diplomatic diploMod = new Diplomatic();
    private Tactical tacticsMod = new Tactical();
    private AdminAI adminMod = new AdminAI();
    private DevelopmentAI developmentAI = new DevelopmentAI();

    private MyEnum.macroPriorities primaryFocus;
    private MyEnum.macroPriorities secondaryFocus;

    private bool usedAllAPLastTurn = false;

  
    public TopLevel GetTopLevel()
    {
        return this.topLevel;
    }

    public Diplomatic GetDiplomatic()
    {
        return diploMod;
    }

    public Tactical GetTactical()
    {
        return tacticsMod;
    }

    public AdminAI GetAdmin()
    {
        return adminMod;
    }

    public DevelopmentAI getDevelopmentAI()
    {
        return this.developmentAI;
    }


    public void processAdminstrationPhase(Nation player)
    {


        topLevel.calculateProductionPriority(player);
        adminMod.considerMilitaryNeed(player);

        /* General Priorities (ordinal ranking): Colonies - Spheres - Technology - Conquest - Industry 
         * 
         * 1. First priority - ensure sufficient food, coal, and oil for projected needs
         * 2. Update resource priorities
         * 2. Update action priorities
         *         1 Increasing POP
         *         2 Reduce Corruption
         *         3 Build Army Unit
         *         4 Build Naval Unit
         *         5 Tactic Cards
         *         6 Culture Cards
         *         7 Research Points
         *         8 Colonial Points
         *         9 Investment Points
         *         10 Build/Upgrade Factory
         *         11 Improve Province
         *         12 Upgrade Shipyard
         *         13 Upgrade Warehouse
         *         14 Manifacture Goods
         * 
         * Increase POP if it had insufficient action points previous turn (so need to note that)
         * Reduce corruption if it is high (range 0 - 10)
         * Build army unit if believes it might be targeted by a stronger player or if it aggressive and wishes to target another
         * Gain tatic cards if army is big enough but wants to gain qualitative edge
         * Culture if unhappiness is high or if it its prestige rank is low or if it wants spheres 
         * Research - always values - esp. if falling behind - choice of tech will be determined partly by other priorities
         * Colonial Points - valued if colonies are values or if behind on colonies (unless colonies is least valued)
         * Investment Points - valued based on value of industry, as well as comparative ind. rank. Always somewhat valued (unless
         * points are not being used)
         * 
         * Sell surplus good and resources
         */
        AdminAI admin = GetAdmin();
        admin.handleEachResrouceNeed(player);
        if (this.usedAllAPLastTurn)
        {
            admin.tryIncreasePOP(player);
            player.getAI().GetTopLevel().alterMacroPriority(player, MyEnum.macroPriorities.increasePOP, -0.25f);
        }

        List<MyEnum.macroPriorities> macroPriorities = GetTopLevel().getMacroPriorites(player);

        for(int i = 0; i < macroPriorities.Count; i++)
        {
            MyEnum.macroPriorities macro = macroPriorities[i];
        
            if(player.getAP() == 0)
            {
                return;
            }

            tryMacro(player, macro, admin);

            if (i < 2 && macro == MyEnum.macroPriorities.conquest)
            {
                admin.tryImproveMilitary(player);
            }
            if (i < 2 && macro == MyEnum.macroPriorities.defense)
            {
                tryMacro(player, MyEnum.macroPriorities.defense, admin);
            }

            if (i >= 4)
            {
                MyEnum.macroPriorities macro2 = macroPriorities[0];
                tryMacro(player, macro2, admin); 
            }
            if(i >= 6)
            {
                MyEnum.macroPriorities macro2 = macroPriorities[1];
                tryMacro(player, macro2, admin);
            }
        }
  
    }

    public void tryMacro(Nation player, MyEnum.macroPriorities macro, AdminAI admin)
    {
        if (macro == MyEnum.macroPriorities.colonies)
        {
            admin.tryColonial(player);
        }
        else if (macro == MyEnum.macroPriorities.conquest)
        {
            admin.tryImproveMilitary(player);
        }
        else if(macro == MyEnum.macroPriorities.defense)
            {
                admin.tryImproveDefense(player);
            }
        
        else if (macro == MyEnum.macroPriorities.culture)
        {
            admin.tryCulture(player);
        }
        else if (macro == MyEnum.macroPriorities.buildFactory)
        {
            admin.tryFactory(player);
        }
        else if (macro == MyEnum.macroPriorities.research)
        {
            admin.tryResearch(player);

        }
        else if (macro == MyEnum.macroPriorities.spheres)
        {
            admin.tryGainInfulence(player);
        }
        else if (macro == MyEnum.macroPriorities.navy)
        {
            admin.tryBuildNavy(player);
        }
        else if(macro == MyEnum.macroPriorities.corruption)
        {
            admin.tryReduceCorruption(player);
        }
    }

    public void processMovements()
    {
        return;
    }

    public int makeBidSphere(Nation nation)
    {
        int bid = -1;



        return bid;
    }

    public int makeBidColony(Nation nation)
    {
        int bid = -1;


        return bid;
    }

    public TopLevel getTopLevel()
    {
        return topLevel;
    }

    public MyEnum.macroPriorities getPrimaryFocus()
    {
        return this.primaryFocus;
    }

    public MyEnum.macroPriorities getSecondaryFocus()
    {
        return this.secondaryFocus;
    }

    public void setPrimaryFocus(MyEnum.macroPriorities priority)
    {
        this.primaryFocus = priority;
    }

    public void setSecondaryFocus(MyEnum.macroPriorities priority)
    {
        this.secondaryFocus = priority;
    }


}
