using assemblyCsharp;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventResponder 
{
    public void standardProvinceResponse(Nation nation, Province prov, int otherNationIndex, int minorNationIndex)
    {
        Debug.Log("Standard Province Response");
        App app = UnityEngine.Object.FindObjectOfType<App>();
        int humanIndex = app.GetHumanIndex();
        Nation human = State.getNations()[humanIndex];
        int numberOfDisruptedProvinces = PlayerCalculator.getNumberOfDisrputedProvinces(nation);
        EventRegister eventLogic = State.eventRegister;
       // if (numberOfDisruptedProvinces > 0)
       // {
            if (otherNationIndex > -1)
            {
                Debug.Log("Crack down");
                Nation otherNation = State.getNation(otherNationIndex);
                //crack down on riots
                nation.InfulencePoints--;
                prov.adjustDiscontentment(1);
                nation.adjustRelation(otherNation, -10);
                if(otherNationIndex == humanIndex)
                {
                    DecisionEvent newEvent = new DecisionEvent();
                    if (minorNationIndex == -1)
                    {
                        eventLogic.initalizeInformedOfCrackdownEvent(newEvent, human, nation, prov);
                    }
                    else
                    {
                        eventLogic.initalizeInformedOfCrackdownEvent(newEvent, human, nation, prov, minorNationIndex);

                    }
                    eventLogic.DecisionEvents.Enqueue(newEvent);
                }
            }
       // }
            else
            {
                int roll = Random.Range(1, 100);
                Debug.Log("Roll: " + roll);
                if (roll < 50)
                {
                    nation.InfulencePoints--;
                    prov.adjustDiscontentment(1);
                    if (otherNationIndex > -1)
                    {
                        Nation otherNation = State.getNation(otherNationIndex);
                        //crack down on riots
                        nation.InfulencePoints--;
                        prov.adjustDiscontentment(1);
                        nation.adjustRelation(otherNation, -10);
                        if (otherNationIndex == humanIndex)
                        {
                            DecisionEvent newEvent = new DecisionEvent();
                            if (minorNationIndex == -1)
                            {
                                eventLogic.initalizeInformedOfCrackdownEvent(newEvent, human, nation, prov);
                            }
                            else
                            {
                                eventLogic.initalizeInformedOfCrackdownEvent(newEvent, human, nation, prov, minorNationIndex);

                            }
                            eventLogic.DecisionEvents.Enqueue(newEvent);
                        }
                    }
                }
                else
                {
                    prov.setRioting(true);
                }
            }
    }

  
    public void respondToProvinceRiots(Nation nation, Province prov)
    {
        Debug.Log("Respond to Province Riots");
        int numberOfDisruptedProvinces = PlayerCalculator.getNumberOfDisrputedProvinces(nation);
        bool sameCulture = true;
        int nationWithSameCulture = nation.getIndex();
        EventRegister eventLogic = State.eventRegister;

        App app = UnityEngine.Object.FindObjectOfType<App>();
        int humanIndex = app.GetHumanIndex();
        Nation human = State.getNations()[humanIndex];

        if (!nation.culture.Equals(prov.getCulture()))
        {
            sameCulture = false;
            nationWithSameCulture = Utilities.findNationWithThisCulture(prov.getCulture());
        }

        if (prov.isColony)
        {
            if (nation.GetColonialPoints() > 0)
            {
                //crack down on riots
                nation.SpendColonialPoints(1);
            }
            else
            {
                prov.setRioting(true);
            }
        }
        else if (!prov.isColony)
        {
            if (sameCulture)
            {
                if (nation.InfulencePoints > 0)
                {
                    standardProvinceResponse(nation, prov, -1, -1);
                }
                else
                {
                    prov.setRioting(true);
                }
            }
            // Now consider cases where the prov is not a colony and does not share the same culture as its owner

            else if (!prov.isColony && !sameCulture)
            {
                if (nation.InfulencePoints > 0)
                {
                    Debug.Log("Not colony and not same culture");
                    //Consider who might get angry with the crackdown
                    int otherNationIndex = Utilities.findNationWithThisCulture(prov.getCulture());
                    Nation otherNation = State.getNation(otherNationIndex);
                    Debug.Log("Other Nation is: " + otherNation.getName());
                    int minorNationIndex = -1;
                    Nation minorNation = new Nation();
                    if (otherNation.getType() == MyEnum.NationType.minor)
                    {
                        minorNationIndex = otherNation.getIndex();
                        minorNation = otherNation;
                        otherNationIndex = PlayerCalculator.getMostFavouredMajorNation(otherNation);
                        // The other nation is the guardian, not the owner of the nation
                        otherNation = State.getNation(otherNationIndex);
                    }
                    int otherStrength = 0;
                    int selfStrength = PlayerCalculator.CalculateArmyScore(nation);
                    // Will anger another great power
                    int relations = nation.Relations[otherNationIndex];
                    //This value is just for tests
                    if (relations < 85)
                    {
                        Debug.Log("here");
                        standardProvinceResponse(nation, prov, otherNationIndex, minorNationIndex);
                    }
                    else
                    {
                        if (State.mapUtilities.shareLandBorder(nation, otherNation))
                        {
                            otherStrength = PlayerCalculator.CalculateArmyScore(otherNation);
                        }
                        else
                        {
                            otherStrength = PlayerCalculator.CalculateNavalProjection(otherNation);
                        }

                        if (otherStrength * 1.15 < selfStrength)
                        {
                            //Not too afraid
                            standardProvinceResponse(nation, prov, otherNationIndex, minorNationIndex);
                            if (demandReferendum(otherNation, nation, prov))
                            {
                                if (acceptRefDemand(nation, otherNation, prov))
                                {
                                    referendum(nation, otherNation, prov);
                                }
                                else
                                {
                                    //nation refuses to hold a referendum
                                    if (warOverRejection(otherNation, nation, prov))
                                    {
                                        War war = new War(otherNation, nation, prov.getIndex(), minorNationIndex);
                                        war.warBetweenAI(otherNation, nation);
                                    }
                                    else if (boycottOverRefDemandRejection(otherNation))
                                    {
                                        otherNation.addBoycott(nation.getIndex());
                                        //....................

                                    }
                                    else
                                    {
                                        // Backdown
                                        PlayerPayer.loseFace(otherNation);
                                    }
                                }
                            }
                        }

                        if (otherStrength * 1.3 < selfStrength)
                        {
                            // Prefer not to provicate other nation, but might do so if situation calls for it

                            int roll = Random.Range(1, 100);
                            if (roll < 50)
                            {
                                nation.InfulencePoints--;
                                prov.adjustDiscontentment(1);
                                if (otherNation.getIndex() == humanIndex)
                                {
                                    DecisionEvent newEvent = new DecisionEvent();
                                    if (minorNationIndex == -1)
                                    {
                                        eventLogic.initalizeInformedOfCrackdownEvent(newEvent, human, nation, prov);
                                    }
                                    else
                                    {
                                        eventLogic.initalizeInformedOfCrackdownEvent(newEvent, human, nation, prov, minorNationIndex);

                                    }
                                    eventLogic.DecisionEvents.Enqueue(newEvent);
                                }
                                else if (demandReferendum(otherNation, nation, prov))
                                {
                                    if (acceptRefDemand(nation, otherNation, prov))
                                    {
                                        referendum(nation, otherNation, prov);
                                    }
                                    else
                                    {
                                        if (warOverRejection(otherNation, nation, prov))
                                        {
                                            War war = new War(otherNation, nation, prov.getIndex(), minorNationIndex);
                                            war.warBetweenAI(otherNation, nation);
                                        }
                                    }
                                }
                                else
                                {
                                    prov.setRioting(true);
                                }
                            }

                            else
                            {
                                prov.setRioting(true);
                            }
                        }
                        else
                        {
                            // Don't want to fuck with these guys
                            prov.setRioting(true);
                        }

                    }
                }
                else
                {
                    Debug.Log("No Influence Points");
                    prov.setRioting(true);
                    // Just for now
                    nation.InfulencePoints++;
                }
            }
        }
    }


    public bool demandReferendum(Nation responder, Nation cracker, Province prov)
    {
        // sanity check: make sure responder and prov share same culture
        if (!responder.culture.Equals(cracker.culture))
        {
            return false;
        }
        responder.adjustRelation(cracker, -15);
        int relations = responder.Relations[cracker.getIndex()];
        if(relations > 50)
        {
            return false;
        }
        int selfStrength = 0;
        int otherStrength = PlayerCalculator.CalculateArmyScore(cracker);
    
        if (State.mapUtilities.shareLandBorder(responder, cracker))
        {
            selfStrength = PlayerCalculator.CalculateArmyScore(responder);
        }
        else
        {
            selfStrength = PlayerCalculator.CalculateNavalProjection(responder);
        }
        if (selfStrength * 1 > otherStrength)
        {
            if (selfStrength * 1.2 < otherStrength)
            {
                int roll = Random.Range(1, 100);
                if (roll < 35)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return true;
            }
        }
        else
        {
            return false;
        }
    }

    public bool acceptRefDemand(Nation owner, Nation demander, Province prov)
    {
        Debug.Log("Will AI accept refferendum demand?");
        int roll = Random.Range(0, 100);
        Debug.Log(roll);
        int otherStrength = 0;
        int selfStrength = PlayerCalculator.CalculateArmyScore(owner);
        Debug.Log("Self strength: " + selfStrength);

        if (State.mapUtilities.shareLandBorder(owner, demander))
        {
            Debug.Log("By land");
            otherStrength = PlayerCalculator.CalculateArmyScore(demander);
        }
        else
        {
            Debug.Log("By sea");
            otherStrength = PlayerCalculator.CalculateNavalProjection(demander);
        }
        Debug.Log("Other strength: " + otherStrength);

        if (otherStrength < selfStrength * 1.15)
        {
            Debug.Log("Here");
            return false;
        }
        else if(otherStrength  < selfStrength * 1.25)
        {
            Debug.Log("Here");

            if (roll < 50)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        else if(otherStrength < selfStrength * 1.35)
        {
            Debug.Log("Here");

            if (roll < 25)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        else
        {
            Debug.Log("Here");

            return true;
        }
    }

    public bool referendum(Nation nation, Nation otherNation, Province prov)
    {
        int discontentLevel = prov.getDiscontentment();
        // Idea: Discontment Level of 12 warrents %50 chance of yes vote
        int yesStrength = discontentLevel * 4;
        int roll = Random.Range(1, 100);
        if(roll < yesStrength)
        {
            // province votes to leave
            nation.decreasePrestige(2);

            prov.changeProvinceControl(nation, otherNation);
            MapChange newMapChange = new MapChange(nation.getIndex(), otherNation.getIndex(), prov.getIndex());
            List <MapChange> mapChanges = State.MapChanges;
            Debug.Log("Adding Map Change");
            mapChanges.Add(newMapChange);
            return true;
        }
        else
        {
            nation.increasePrestige(1);
            prov.adjustDiscontentment(-1);
            return false;
        }
    }

    public bool warOverRejection(Nation nation, Nation otherNation, Province prov)
    {
        int otherStrength = 0;
        int selfStrength = PlayerCalculator.CalculateArmyScore(nation);
        int roll = Random.Range(1, 100);

        if (State.mapUtilities.shareLandBorder(nation, otherNation))
        {
            otherStrength = PlayerCalculator.CalculateArmyScore(otherNation);
        }
        else
        {
            otherStrength = PlayerCalculator.CalculateNavalProjection(otherNation);
        }

        if(selfStrength * 1.5 > otherStrength)
        {
            return true;
        }
        else if(selfStrength * 1.33 > otherStrength)
        {
            if(roll < 60)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else if(selfStrength * 1.25 > otherStrength)
        {
            if (roll < 35)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else if(selfStrength * 1.15 > otherStrength)
        {
            if(roll < 20)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;    
    }

    public bool boycottOverRefDemandRejection(Nation nation)
    {
        int numberOfBoycotts = nation.getBoycotts().Count;
        if (numberOfBoycotts > 1)
        {
            return false;
        }
        if(numberOfBoycotts == 1)
        {
            int roll = Random.Range(1, 100);
                if(roll < 50)
                {
                    return true;
                }
                else
                {
                    return false;
                }
        }
        else
        {
            // Currently not boycotting anyone
            return true;
        }
      
        
    }

    public void decideOnSpreadDisscentOpportunity(Nation decider, Nation otherMajor, Province prov)
    {
        if(spreadDiscontent(decider, otherMajor, prov))
        {
            if(spreadDiscontentDetected(decider, otherMajor))
            {
                decider.Relations[otherMajor.getIndex()] = -10;
            }
            prov.adjustDiscontentment(1);
            otherMajor.InfulencePoints--;
        }

    }

    public bool spreadDiscontent(Nation decider, Nation otherMajor, Province prov)
    {
        if(decider.InfulencePoints < 1)
        {
            return false;
        }
        if(prov.getCulture() == decider.culture)
        {
            return true;
        }
        else
        {
            if(decider.Relations[otherMajor.getIndex()] < 58 && decider.InfulencePoints > 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public bool spreadDiscontentDetected(Nation spreader, Nation spredee)
    {
        int corruption = spredee.Corruption;
        int DetectionChance = 60 - corruption * 60;
        int roll = UnityEngine.Random.Range(1, 100);
        if (roll < DetectionChance)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool sendNavyToIntercept(Nation defender, Nation attacker)
    {
        //int defenderNavalStrength = PlayerCalculator.nav
            return true;
    }

}
