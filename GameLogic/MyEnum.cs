using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MyEnum  {

        
        public enum Era { Early, Middle, Late};

		public enum NationType {major = 1, minor = 2 , oldEmpire = 3, oldMinor = 4};

		public enum Government {Despotism, AbsoluteMonarchy, Oligarchy};

		public enum Culture {};

		public enum Religion{};

		public enum Class{Upper, Lower, Middle};

		public enum Role{Farmers, Laborers, Soliders, Proletarian, Artisans, Bureaucrats, Researchers,
			Managers, Officers, Artists, Aristocrats, Capitalists};


        public enum Politics {Monarchist, Conservative, Liberal, Socialist, Communist, Fachist}
	public enum Specialist {Officer, Researcher, Manager, Bureaucrat, Artist};

        public enum Resources {wheat, meat, fruit, cotton, iron, wood, coal, spice,
        dyes, rubber, oil, gold};
    //Maybe replace {Food} with {Wheat, Fruit, Fish, Meat}
        public enum Goods {lumber, fabric, steel, clothing, paper, furniture,
        parts, arms, chemicals, gear, telephone, auto};

    public enum GamePhase{adminstration, trade, auction, events, end};

    public enum UnitTypes { infantry, cavalry, artillery, fighter, tank, frigates, ironclad, dreadnought};

    public enum ArmyUnits { infantry, cavalry, artillery, fighter, tank};
    public enum NavyUnits { frigates, ironclad, dreadnought};

    public enum TacticCardPhase {Intelligence, Raid, Engagement_Defend, Engagement_Attack, Escape}
    public enum TacticCards {
        Ambush, Breakout, CounterScheme, Deception, DemoralizingRaid,
        Evasion, FeignedRetreat, Flank, GrandDeception, IndirectApproach,
        LineDefense, Mastermind, MilitarySpy, Penetration, Recon,
        RepelRaid, SupplyRaid, TargetedRaid, TurtleDefense
    };

    // Doctrines can improve: morale, recon, intelligence, strategic judgment, attack damage (and evasion), hock damange, and Mobilization. 
    public enum ArmyDoctrines
    {
        MilitaryTradition, MilitaryScience, SkilledHussars, EarlyDetection, InterceptionPatrols, SpyNetwork, ExpertSpies, CounterIntelligence, 
        Professionalism, InsipiringLeaders, LockStep, DragoonCommanders, MassConscription,
        ExpertLogistics, ExpertRaiders, ShockAndAwe, ConcentratedArtillery, Evasion, end 
    };


    public enum OffBidLevels { high, medium, low, none};
    public enum marketChoice { pass, offer , bid};

    public enum cultCardType { literature, music, politics, philosophy, painting, design, none};

    public enum cultMovement { unique, romanticism, realism, impressionism, liberalism, existentialism, victorian, neoclassical};

    public enum cultCard
    {
        ArtNouveau, Communism, DecadentLiterature, EspritDeCorps, ExistentialistLiterature, ExistentialistPhilosophy,
        Expressionism, Fauvism, Idealism, ImpressionistArt, ImpressionistMusic, LateRomanticism, Nationalism,
        NeoclassicalArt, PostImpressionism, Primitivism, RealistLiterature, RealistPainting, RomanticLiterature,
        RomanticMusic, RomanticPainting, SocialDarwinism, Symbolism, TheRightsOfMan, ClassicalLiberalism, Feminism, Sociology,
        RomanticOpera, EmpireStyle, RevivalStyle, Aestheticism, VictorianStyle, VictorianLiterature
    };



    public enum Levels { low, medium, high};

    public enum LeftRightPriority { left, right, center};

    // 0 colonies, 1 industry, 2 research, 3 culture, 4 army 
    public enum staticPriorities {colonies, industry, research, culture, army}

  //  public enum macroPriorities
    //{
     //   colonies, spheres, buildFactory, research, developProvince,
     //   conquest, culture, navy, defense, increasePOP, manufactureGoods, railroads,
     //   stability, corruption
    //};

    public enum progressPriorities
    {
        research, investment, culture, doctrines, corruption
    }

    public enum developmentPriorities
    {
        buildFactory, developProvince, railroad, trains, shipyard, fortification, warehouse 
    }

    public enum productionPriorities
    {
        manifactureGoods, buildUnit, buildShip
    }

    public enum metaPriorities
    {
        progress, development, production
    }

    public enum nationalDevelopmentTypes { research, colonial, investment, culture, tactics};

    public enum claimType { province, colony, sphere, trade, gold};


    public enum auctionType {sphere, colony};

    public enum provinceType {core, colony};

    public enum fiveLevelLow_High { veryLow, low, normal, high, veryHigh};


    public enum difficulty { easy, normal, difficult};

    public enum diploIntrepretation { gift, demand, badDeal, goodDeal}

    public enum militancy { Hawk, Dove, Realist}

    public enum eventType { riotResponce, referendumDemanded, AI_Attacks, notificationOfCrackdown, AI_RejectsReferendum, AI_AcceptsReferendum,
        askIfBoycott, spreadDissentOppertunity, navyIntercept, end }

    public enum warPhase { information, tactic, maneuver, engagement, conclusion, over}

    public enum shipType { frigate, ironclad, dreadnought};
}

