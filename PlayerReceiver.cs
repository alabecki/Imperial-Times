using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using assemblyCsharp;
using System;

public static class PlayerReceiver {

    public static void CollectResearchPoints(Nation player)
    {
        MyEnum.Era era = State.era;
        if (era == MyEnum.Era.Early)
        {
            player.AddResearchPoints(2);
        }
        if (era == MyEnum.Era.Middle)
        {
            player.AddResearchPoints(4);
        }
        if (era == MyEnum.Era.Late)
        {
            player.AddResearchPoints(6);
        }
    }

    public static void addNewTech(Nation player, Technology tech)
    {
        player.AddTechnology(tech.GetTechName());
        if (tech.GetDiscovered() == false)
        {
            player.addPattent();
            tech.SetDiscovered(true);
            tech.SetDiscoveredBy(player.getIndex());
        }

    }

    public static void registerTechChanges(Technology tech, Nation player)
    {
        string techName = tech.GetTechName();
        MilitaryForm militaryForm = player.GetMilitaryForm();

        if (techName == "flintlock")
        {
            militaryForm.infantry.ImproveAttackStrength(0.25f);
            militaryForm.infantry.ImproveDefenseStrength(0.1f);
            militaryForm.cavalry.ImproveAttackStrength(0.25f);
            militaryForm.cavalry.ImproveDefenseStrength(0.1f);
        }
        if(techName == "square_timbering")
        {
            player.IncreaseMaxWareHouseCapacity(8);

        }


        if (techName == "cement")
        {
            player.IncreaseMaxFort();
            player.IncreaseMaxWareHouseCapacity(8);
        }
        if (techName == "breech_loaded_arms")
        {
            militaryForm.infantry.ImproveAttackStrength(0.35f);
            militaryForm.infantry.ImproveDefenseStrength(0.2f);
            militaryForm.cavalry.ImproveAttackStrength(0.35f);
            militaryForm.cavalry.ImproveDefenseStrength(0.1f);
            militaryForm.artillery.ImproveAttackStrength(0.35f);
            militaryForm.artillery.ImproveDefenseStrength(0.2f);
            militaryForm.artillery.IncreaseShock(0.1f);
            militaryForm.artillery.IncreaseAmmoConsumption(0.05f);

            militaryForm.frigate.ImproveAttack(0.3f);
        }
        if (techName == "machine_guns")
        {
            militaryForm.infantry.ImproveAttackStrength(0.1f);
            militaryForm.infantry.ImproveAttackStrength(1.0f);
            militaryForm.cavalry.ImproveDefenseStrength(0.2f);
            militaryForm.ironclad.ImproveAttack(0.15f);
            militaryForm.infantry.IncreaseAmmoConsumption(0.075f);
        }
        if (techName == "indirect_fire")
        {
            militaryForm.artillery.ImproveAttackStrength(0.3f);
            militaryForm.artillery.ImproveDefenseStrength(0.6f);
            militaryForm.artillery.IncreaseShock(0.2f);
            militaryForm.ironclad.ImproveAttack(0.2f);
            militaryForm.artillery.IncreaseAmmoConsumption(0.1f);
            militaryForm.ironclad.IncreaseAmmoConsumption(0.1f);
        }
        if(techName == "advanced_iron_working")
        {
            player.IncreaseMaxWareHouseCapacity(8);

        }

        if(techName == "steel_armor")
        {
            militaryForm.ironclad.HitPoints = 3;
        }

        if(techName == "oil_powered_ships")
        {
            player.IncreaseMaxWareHouseCapacity(8);
        }

        if (techName == "bombers")
        {
            militaryForm.fighter.ImproveAttackStrength(0.25f);
            militaryForm.fighter.IncreaseShock(0.3f);
            militaryForm.fighter.ImproveDefenseStrength(0.1f);
            militaryForm.fighter.IncreaseAmmoConsumption(0.15f);
        }
        if (techName == "radar")
        {
            militaryForm.fighter.ImproveDefenseStrength(0.8f);
            militaryForm.dreadnought.ImproveAttack(0.5f);
        }
        if (techName == "bolt_action_rifles")
        {
            militaryForm.infantry.ImproveAttackStrength(0.5f);
            militaryForm.infantry.ImproveDefenseStrength(0.15f);
            militaryForm.cavalry.ImproveAttackStrength(0.25f);
            militaryForm.infantry.IncreaseShock(0.1f);
            militaryForm.cavalry.IncreaseShock(0.15f);


        }
        if (techName == "telegraph")
        {
            player.IncreaseFactoryThroughput(1);
            player.InceaseProductionModifier(0.15f);
            player.IncreaseOrgFactor(0.15f);
        }


        if (techName == "electricity")
        {
            player.IncreaseFactoryThroughput(1);
            player.InceaseProductionModifier(0.15f);
        }
        if (techName == "radio")
        {
            player.IncreaseOrgFactor(0.15f);
        }

        if (techName == "early_computers")
        {
            player.InceaseProductionModifier(0.15f);
            militaryForm.dreadnought.ImproveAttack(1);

        }

        if (techName == "atomic_bomb")
        {

        }

    }

    public static void collectTacticCards(Nation player)
    {
        player.increaseArmyLevel();
        if (player.getArmyLevel() < 3)
        {
            player.setMaximumTacticHandSize(6);
        }
        if (player.getArmyLevel() == 3)
        {
            player.setMaximumTacticHandSize(7);
        }
        if (player.getArmyLevel() == 5)
        {
            player.setMaximumTacticHandSize(8);
        }
        if (player.getArmyLevel() == 7)
        {
            player.setMaximumTacticHandSize(9);
        }
        if (player.getArmyLevel() == 9)
        {
            player.setMaximumTacticHandSize(10);
        }
        if (player.getArmyLevel() == 11)
        {
            player.setMaximumTacticHandSize(11);
        }
        if (player.getArmyLevel() == 13)
        {
            player.setMaximumTacticHandSize(12);
        }
        Debug.Log("Max number of card is now: " + player.getMaximumTacticHandSize());

        MyEnum.Era era = State.era;
        int numNewCards = 2;
        if(era == MyEnum.Era.Middle)
        {
            numNewCards = 3;
        }
        if(era == MyEnum.Era.Late)
        {
            numNewCards = 4;
        }
        int count = 0;
        Stack<TacticCard> deck = State.getTacticDeck();
        while(player.getTacticCards().Count <= player.getMaximumTacticHandSize() && count < numNewCards)
        {
            TacticCard newCard = deck.Pop();
            player.addTacticCard(newCard);
            Debug.Log("New Card is: " + newCard.type.ToString());
            count += 1;
        }

    }

    public static MyEnum.cultCard collectCultureCard(Nation player)
    {
        Stack<CultureCard> cultureDeck = State.getCultureDeck();
        CultureCard newCard = cultureDeck.Pop();
        if (player.getCultureCards().Contains(newCard.getCardName()))
        {

            Boolean newCardFound = false;
            while(newCardFound == false)
            {
                cultureDeck.Push(newCard);
                State.Shuffle<CultureCard>(cultureDeck);
                newCard = cultureDeck.Pop();
                if (!player.getCultureCards().Contains(newCard.getCardName()))
                {
                    newCardFound = true;
                }
            }
        }
        player.addCultureCard(newCard.getCardName());
        player.Stability += newCard.getHappinessBoost();
        player.InfulencePoints += newCard.getInfulenceBoost();
        player.increasePrestige(newCard.getPrestigeBoost());
        player.GetMilitaryForm().adjustMaxMorale(newCard.getMoraleBoost());
        player.adjustClothingQuality(newCard.getClothingQuality());
        player.adjustFurnitureQuality(newCard.getFurnQuality());
        int index = State.getCultureCardTypes().IndexOf(newCard);
        CultureCard thisCard = State.getCultureCardTypes()[index];
        Debug.Log("Current originator: " + thisCard.getOriginator());
        if(thisCard.getOriginator() == -1)
        {
            thisCard.setOriginator(player);
            player.increasePrestige(1);
        }
        Debug.Log("New Card is: " + newCard.getCardName().ToString());
        player.increaseCultureLevel();

        return thisCard.getCardName();

    }

    public static void increaseFactoryLevel(Nation player, MyEnum.Goods good)
    {
        player.industry.upgradeFactory(good);
    }

    public static void receiveIP(Nation player)
    {
        player.addIP(2);
        MyEnum.Era era = State.GerEra();
        if (era == MyEnum.Era.Middle)
        {
            player.addIP(2);
        }
        if (era == MyEnum.Era.Late)
        {
            player.addIP(2);
        }
        player.increaseInvestmentLevel();
    }



    public static void receiveFromCancelingProduction(Nation player, MyEnum.Goods good, int amount)
    {
        if (amount == 1)
        {
            player.addAP(1);
        }
        if (amount > 1 && amount <= 4)
        {
            player.addAP(2);
        }
        if (amount > 4 && amount <= 7)
        {
            player.addAP(3);
        }
        if (amount > 7)
        {
            player.addAP(4);
        }
        player.industry.setGoodProducing(good, amount);
        for (int k = 0; k < amount; k++)
        {
            player.industry.returnGoodsMaterial(good, player);
        }

    }

    public static void worsenRelations(Nation player, int target)
    {
        int adjustment = 1;
        Nation otherNation = State.getNations()[target];
        MyEnum.NationType NType = otherNation.getType();
        if (NType == MyEnum.NationType.major)
        {
            adjustment = -20;
        }
        if (NType == MyEnum.NationType.minor)
        {
            adjustment = -50;
        }
        if (NType == MyEnum.NationType.oldEmpire)
        {
            adjustment = -30;
        }
        Debug.Log("Adjustment is " + adjustment);
        player.getRelationFromThisPlayer(target).adjustAttude(adjustment);
        player.getRelationToThisPlayer(target).adjustAttude(adjustment);
        player.useDiplomacyPoints(1);
    }

   public static void improveRelations(Nation player, int target)
    {
        Nation otherNation = State.getNations()[target];

        int adjustment = 1;
        MyEnum.NationType NType = otherNation.getType();
        if (NType == MyEnum.NationType.major)
        {
            adjustment = 10;
            Debug.Log("here");
        }
        if (NType == MyEnum.NationType.minor)
        {
            adjustment = 30;
            Debug.Log("here");

        }
        if (NType == MyEnum.NationType.oldEmpire)
        {
            adjustment = 20;
            Debug.Log("here");

        }
        Debug.Log("Adjustment is " + adjustment);

        player.getRelationFromThisPlayer(target).adjustAttude(adjustment);
        player.getRelationToThisPlayer(target).adjustAttude(adjustment);
        player.useDiplomacyPoints(1);
    }


    public static void gainCB(Nation player, int target, WarClaim claim)
    {
        claim.toggleActive();
        Nation otherNation = State.getNations()[target];
        player.useDiplomacyPoints(1);
        Relation relationTo = player.getRelationToThisPlayer(target);
        Relation relationFrom = player.getRelationFromThisPlayer(target);
        relationTo.adjustAttude(-5);
        relationFrom.adjustAttude(-5);
        player.adjustReputation(-5);

    }

    public static void declareWar(Nation player, int target)
    {
        Relation relationTo = player.getRelationToThisPlayer(target);
        Relation relationFrom = player.getRelationFromThisPlayer(target);
        player.getRelationFromThisPlayer(target).makeWar();
        player.getRelationToThisPlayer(target).makeWar();
        player.useDiplomacyPoints(1);
        relationTo.adjustAttude(-15);
        relationFrom.adjustAttude(-15);
    }


}
