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
        parts, arms, chemicals, gear, telephone, auto, fighter, tank};

    public enum GamePhase{adminstration, trade, auction, movement, end};

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


    public enum OffBidLevels { high, medium, low, none};
    public enum marketChoice { pass, offer , bid};

    public enum cultCardType { literature, music, politics, philosophy, painting, design};
    public enum cultMovement { unique, romanticism, realism, impressionism};
    public enum cultCard
    {
        ArtNouveau, Communism, DecadentLiterature, DesignerClothing, EspritDeCorps, Existentialism,
        Expressionism, Fauvism, Idealism, ImpressionistArt, ImpressionistMusic, LateRomanticism, Nationalism,
        NeoclassicalArt, PostImpressionism, Primitivism, RealistLiterature, RealistPainting, RevivalFurniture, RomanticLiterature,
        RomanticMusic, RomanticPainting, SocialDarwinism, Symbolism, TheRightsOfMan
    };


    public enum Loyality { low, medium, high};
    public enum Protectionism { low, medium, high};

    public enum macroPriorities {colonies, spheres, buildFactory, research, developProvince,
                                conquest, culture, navy, defense, corruption, increasePOP};

    public enum nationalDevelopmentTypes { research, colonial, investment, culture, tactics};

    public enum claimType { province, colony, sphere, trade, gold};


    public enum auctionType {sphere, colony};

    public enum provinceType {core, colony};


}

