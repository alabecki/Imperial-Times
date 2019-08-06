using assemblyCsharp;
using ModelShark;
using System;
using System.Collections;
using System.Collections.Generic;
using UI.ThreeDimensional;
using UnityEngine;
using UnityEngine.UI;

public class EventRegisterObject : MonoBehaviour
{
    public Camera mainCamera;
    public Camera SeaCamera;

    public GameObject eventDecisionTwoPanel;
    public Text dualDecisionHeadline;
    public Text dualDecisionMessage;
    public Image dualDecisionImage;
    public Button optionA;
    public Button optionB;
   // private MyEnum.eventType eventType;
    //private Stack<int> riotingProvs;
    //private int otherMajorIndex;
   // private int otherMinor;
    private War war;
    private DecisionEvent currentEvent;
    private EventRegister eventLogic;

    public GameObject warPanel;
    public UIObject3D playerFlag;
    public UIObject3D aiFlag;

    public Text playerName;
    public Text aiName;
    public Text playerForces;
    public Text aiForces;

    public Text playerMorale;
    public Text playerReconValue;
    public Text playerIntelligenceValue;
    public Text playerJudgmentValue;
    public Text playerManeuverValue;

    public Text aiMorale;
    public Text playerAttackAttribute;
    public Text aiAttackAttribute;
    public Text playerShockAttribute;
    public Text aiShockAttribute;
    public Text aiReconValue;
    public Text aiIntelligenceValue;
    public Text aiJudgmentValue;
    public Text aiManeuverValue;

    public Text phase_or_round;
    public Toggle playerRecon;
    public Toggle aiRecon;
    public Toggle playerEsopinage;
    public Toggle aiEspionage;
    public Toggle playerJudgement;
    public Toggle aiJudgement;

    public Text playerTactics;
    public Text aiTactics;
    public Toggle playerManouver;
    public Toggle aiManeuver;
    public Text playerCombatModifier;
    public Text aiCombatModifier;

    public Text phaseRoundReport;
    public Button nextPhaseRound;

    public Button turnButton;

    private GameObject leftUnit;
    private GameObject rightUnit;

    private Animator leftUnitAnimator;
    private Animator rightUnitAnimator;

    public GameObject ZeroOne;
    public GameObject ZeroTwo;
    public GameObject ZeroThree;
    public GameObject OneOne;
    public GameObject OneTwo;
    public GameObject OneThree;
    public GameObject TwoOne;
    public GameObject TwoTwo;
    public GameObject TwoThree;
    public GameObject ThreeOne;
    public GameObject ThreeTwo;
    public GameObject ThreeThree;
    public GameObject FourOne;
    public GameObject FourTwo;
    public GameObject FourThree;
    public GameObject FiveOne;
    public GameObject FiveTwo;
    public GameObject FiveThree;

    public GameObject leftBattleTerrain;
    public GameObject rightBattleTerrain;

    public UnitController unitController;

    private AudioSource audioSource;

    public AudioClip thinking;
    public AudioClip marching;
    public AudioClip heavyDrums;
    public AudioClip cannonFire;
    public AudioClip boltLoad;

    // Naval Battle View
    public Button continueNavalBattle;
    public Text playerNameOcean;
    public Text AI_NameOcean;
    public UIObject3D playerFlagOcean;
    public UIObject3D aiFlagOcean;
    public Text playerFrigates;
    public Text aiFrigates;
    public Text playerIronclads;
    public Text aiIronclads;
    public Text playerDreadnoughts;
    public Text aiDreadnoughts;

    public Text playerNavyMorale;
    public Text aiNavyMorale;

    public Text navalBattleReport;

    private GameObject leftShip;
    private GameObject rightShip;

    public GameObject frigateModel;
    public GameObject ironcladModel;
    public GameObject dreadnoughtModel;


    // public GameObject generalReport;
    // public Text generalReportMessage;
    // public Image generalReportImage;
    // public Button generalReportContinue;

    void Start()
        {
        audioSource = nextPhaseRound.GetComponent<AudioSource>();
        eventLogic = State.eventRegister;
        eventDecisionTwoPanel.SetActive(false);
        warPanel.SetActive(false);
       // generalReport.SetActive(false);
        optionA.onClick.AddListener(delegate { processOptionA(); });
        optionB.onClick.AddListener(delegate { processOptionB(); });
        nextPhaseRound.onClick.AddListener(delegate { processNextWarPhaseRound(); });
      //  generalReportContinue.onClick.AddListener(delegate { continueAfterReport(); });
        }


    public void processOptionA()
    {
        Debug.Log("Option A");
        Debug.Log(currentEvent.EventType);
        eventDecisionTwoPanel.SetActive(false);
        closeToolTips();
        App app = UnityEngine.Object.FindObjectOfType<App>();
        int playerIndex = app.GetHumanIndex();
        Nation player = State.getNations()[playerIndex];
        Nation otherMajor = State.getNation(currentEvent.OtherMajor);
        EventRegister eventLogic = State.eventRegister;
        Queue<DecisionEvent> decisionevents = eventLogic.DecisionEvents;
        MyEnum.eventType eventType = currentEvent.EventType;
        Province disputedProvince = State.getProvince(currentEvent.Province);
        int minorNationIndex = currentEvent.OtherMinor;
        EventResponder responder = otherMajor.getAI().getEventResponder();

        if (eventType == MyEnum.eventType.riotResponce)
        {
            // Crackdown on riots to restore order
            crackdown();
       
        }
        else if (eventType == MyEnum.eventType.referendumDemanded)
        {
            // Refuse to hold a referendum 
            refuseReferendum(player, otherMajor, disputedProvince);
        }

        

        else if (eventType == MyEnum.eventType.AI_RejectsReferendum)
        {
            Debug.Log("Player Decides to Declare War");
            war = new War(player, otherMajor, disputedProvince.getIndex(), minorNationIndex);
            prepareWarPanel(player, otherMajor, war);
            return;
        }

        else if (currentEvent.EventType == MyEnum.eventType.notificationOfCrackdown)
        {
            DecisionEvent newEvent = new DecisionEvent();
            Debug.Log("We demand Referendum");
            Debug.Log("Player Name: " + player.getNationName());
            Debug.Log("Other Nation: " + otherMajor.getNationName());
            if (responder.acceptRefDemand(otherMajor, player, disputedProvince))
            {
                Debug.Log("Accepts");
                eventLogic.initalizeAI_AcceptsReferendumEvent(newEvent, currentEvent);   
            }
            else
            {
                Debug.Log("Rejects");
                eventLogic.initalizeAI_RejectsReferendumEvent(newEvent, currentEvent);
            }
            currentEvent = newEvent;
            showDecisionPanel(player);
        }

     

        else if (currentEvent.EventType == MyEnum.eventType.AI_RejectsReferendum)
        {
            // Declare war
            DecisionEvent newEvent = new DecisionEvent();
            war = new War(player, otherMajor, disputedProvince.getIndex(), minorNationIndex);
            prepareWarPanel(player, otherMajor, war);
        }

        else if (currentEvent.EventType == MyEnum.eventType.askIfBoycott)
        {
            player.addBoycott(otherMajor.getIndex());
        }

        else if(currentEvent.EventType == MyEnum.eventType.spreadDissentOppertunity)
        {
            Province prov = State.getProvince(currentEvent.Province);
            prov.adjustDiscontentment(1);
            player.InfulencePoints--;
            DecisionEvent newEvent = new DecisionEvent();

            if (eventLogic.spreadDiscontentDetected(player, otherMajor))
            {
                player.Relations[otherMajor.getIndex()] =- 10;
                eventLogic.initializeReportDissentCaughtEvent(newEvent, currentEvent);
            }
            else
            {
                eventLogic.initializeReportDissentInfluenceEvent(newEvent, currentEvent);
            }
            currentEvent = newEvent;
        }

        else if (currentEvent.EventType == MyEnum.eventType.navyIntercept)
        {
            // Player decides to intercept enemy navy

        }



        nextEventTurn(player);
        
    }

    public void prepareNavalView()
    {
        closeToolTips();
        App app = UnityEngine.Object.FindObjectOfType<App>();
        int playerIndex = app.GetHumanIndex();
        Nation player = State.getNations()[playerIndex];
        Nation otherMajor = State.getNation(currentEvent.OtherMajor);

        playerNameOcean.text = player.getName();
        AI_NameOcean.text = otherMajor.getName();

        GameObject playerFlagPrefab = Instantiate(Resources.Load<GameObject>("Flags/Prefabs/" + player.getNationName()));
        playerFlagOcean.ObjectPrefab = playerFlagPrefab.transform;
        GameObject aiFlagPrefab = Instantiate(Resources.Load<GameObject>("Flags/Prefabs/" + otherMajor.getNationName()));
        aiFlagOcean.ObjectPrefab = aiFlagPrefab.transform;

        playerFrigates.text = player.seaForces.frigate.Number.ToString();
        playerIronclads.text = player.seaForces.ironclad.Number.ToString();
        playerDreadnoughts.text = player.seaForces.dreadnought.ToString();
        aiFrigates.text = otherMajor.seaForces.frigate.Number.ToString();
        aiIronclads.text = otherMajor.seaForces.ironclad.Number.ToString();
        aiDreadnoughts.text = otherMajor.seaForces.dreadnought.ToString();

     //   playerNavyMorale.text = player.la
        


    }

    public void nextEventTurn(Nation player)
    {
        if (eventLogic.DecisionEvents.Count > 0)
        {
            currentEvent = eventLogic.DecisionEvents.Dequeue();
            showDecisionPanel(player);
        }
        else
        {
            Debug.Log("Make turn button interactable");
            turnButton.interactable = true;
            // .... next turn
        }

    }

    public void processOptionB()
    {
        eventDecisionTwoPanel.SetActive(false);
        closeToolTips();
        App app = UnityEngine.Object.FindObjectOfType<App>();
        int playerIndex = app.GetHumanIndex();
        Nation player = State.getNations()[playerIndex];
        Nation otherMajor = State.getNation(currentEvent.OtherMajor);

        Province disputedProvince = State.getProvince(currentEvent.Province);
        Nation otherNation = State.getNation(currentEvent.OtherMajor);
        if(currentEvent.OtherMinor > -1)
        {
            otherNation = State.getNation(currentEvent.OtherMinor);
        }
        if (currentEvent.EventType == MyEnum.eventType.riotResponce)
        {
            nextEventTurn(player);
        }
        else if (currentEvent.EventType == MyEnum.eventType.referendumDemanded)
        {
            // Accept....
            if(eventLogic.referendum(player, otherNation, currentEvent.OtherMinor, disputedProvince))
            {
                // Province Leaves!
                DecisionEvent newEvent = new DecisionEvent();
                eventLogic.initializeProvinceLeaveEvent(newEvent, currentEvent);
                currentEvent = newEvent;
                showDecisionPanel(player);
          
            }
            else
            {
                DecisionEvent newEvent = new DecisionEvent();
                eventLogic.initializeProvinceStaysEvent(newEvent, currentEvent);
                currentEvent = newEvent;
                showDecisionPanel(player);
            }
        }


        else if (currentEvent.EventType == MyEnum.eventType.AI_Attacks)
        {
            otherMajor = State.getNation(currentEvent.OtherMajor);
            war = new War(otherMajor, player, disputedProvince.getIndex(), currentEvent.OtherMinor);
            if (war.AmphibiousAttack)
            {
                DecisionEvent newEvent = new DecisionEvent();
                eventLogic.initializeDecideIfUseNavyDefend(newEvent, currentEvent);
                currentEvent = newEvent;
                showDecisionPanel(player);
            }
            prepareWarPanel(player, otherMajor, war);
        }

        else if (currentEvent.EventType == MyEnum.eventType.notificationOfCrackdown)
        {
            DecisionEvent newEvent = new DecisionEvent();
            eventLogic.initializeAskIfBoycott_AI_Event(newEvent, currentEvent);
            currentEvent = newEvent;
            showDecisionPanel(player);
        }

        else if(currentEvent.EventType == MyEnum.eventType.AI_RejectsReferendum)
        {
            // Don't declare war but maybe boycott
            DecisionEvent newEvent = new DecisionEvent();
        }

        else if (currentEvent.EventType == MyEnum.eventType.askIfBoycott)
        {
            PlayerPayer.loseFace(player);
        }

        else if (currentEvent.EventType == MyEnum.eventType.end)
        {
            nextEventTurn(player);
        }

        else if (currentEvent.EventType == MyEnum.eventType.AI_AcceptsReferendum)
        {
            DecisionEvent newEvent = new DecisionEvent();
            if (eventLogic.referendum(otherMajor, player, currentEvent.OtherMinor, disputedProvince))
            {
                eventLogic.initalizeAI_ReferendumVotesYes(newEvent, currentEvent);
                currentEvent = newEvent;
                showDecisionPanel(player);
            }
            else
            {
                eventLogic.initalizeAI_ReferendumVotesNo(newEvent, currentEvent);
                currentEvent = newEvent;
                showDecisionPanel(player);
            }
        }

        nextEventTurn(player);

    }

    /*public void continueAfterReport()
    {
        App app = UnityEngine.Object.FindObjectOfType<App>();
        int playerIndex = app.GetHumanIndex();
        Nation player = State.getNations()[playerIndex];
        // Might be after a war or after lossing a province in a referendum. Check if any more provinces are revolting 
        if (riotingProvs.Count > 0)
        {
            Province nextProv = State.getProvince(riotingProvs.Pop());
            showRiotEvent(player);
        }
        else
        {
            // Next Kind of event.......
        }
    } */

    public void playNewSound(AudioClip newClip)
    {
        audioSource.enabled = true;
       // audioSource.clip = newClip;
      //  audioSource.Play();
        audioSource.PlayOneShot(newClip);

    }

    public void processNextWarPhaseRound()
    {
     
        Debug.Log("Next War Phase...");
        Debug.Log("Current Phase... " + war.Phase);
        App app = UnityEngine.Object.FindObjectOfType<App>();
        int playerIndex = app.GetHumanIndex();
        Nation human = State.getNations()[playerIndex];
        if (war != null)
        {
            Nation attacker = State.getNation(war.AttackerIndex);
            Nation defender = State.getNation(war.DefenderIndex);
            bool over = war.checkIfWarIsOver(attacker, defender);
            if (over)
            {
                if (war.Phase == MyEnum.warPhase.engagement)
                {
                    war.Phase = MyEnum.warPhase.conclusion;
                }
            }
           
            if (war.Phase == MyEnum.warPhase.information)
            {
                Debug.Log("Should be Interacting");
                playNewSound(boltLoad);

                leftUnitAnimator.StopPlayback();
                rightUnitAnimator.StopPlayback();

                rightUnitAnimator.Play("Interacting");           
                leftUnitAnimator.Play("Interacting");
                war.knowledgePhase(attacker, defender);

            }
            if(war.Phase == MyEnum.warPhase.tactic)
            {
                Debug.Log("Should draw weapon");
                playNewSound(thinking);

                leftUnitAnimator.Play("WeaponDraw");
                rightUnitAnimator.Play("WeaponDraw");
                war.judgementPhase(attacker, defender);
            }
            if(war.Phase == MyEnum.warPhase.maneuver)
            {
                Debug.Log("Should be Walking");
                //nextWarPhaseSoundPlayer.clip = marching;
                playNewSound(marching);

                leftUnitAnimator.Play("Walking");
                rightUnitAnimator.Play("Walking");
                war.maneuverPhase(attacker, defender);
            }
            if(war.Phase == MyEnum.warPhase.engagement)
            {
                //  nextWarPhaseSoundPlayer.clip = cannonFire;
                playNewSound(cannonFire);
                Debug.Log("Should be Attacking");

                leftUnitAnimator.Play("Attacking");
                rightUnitAnimator.Play("Attacking");
                war.warRound(attacker, defender);
            }
            if (war.Phase == MyEnum.warPhase.conclusion)
            {
                if (war.DefenderMorale == 0 || war.DefenderForces == 0)
                {
                    if (playerIndex == war.DefenderIndex)
                    {
                        Debug.Log("Human should be running");
                        leftUnit.transform.Rotate(new Vector3(0, 180, 0));
                        leftUnitAnimator.Play("RunAway");
                        rightUnitAnimator.Play("Jumping");

                    }
                    else
                    {
                        Debug.Log("AI should be running");

                        rightUnit.transform.Rotate(new Vector3(0, 180, 0));
                        rightUnitAnimator.Play("RunAway");
                        rightUnitAnimator.Play("Jumping");

                    }
                }
                else if (war.AttackerMorale == 0 || war.AttackerForces == 0)

                {
                    if (playerIndex == war.AttackerIndex)
                    {
                        Debug.Log("Human should be running");

                        leftUnit.transform.Rotate(new Vector3(0, 180, 0));
                        leftUnitAnimator.Play("RunAway");
                        rightUnitAnimator.Play("Jumping");

                    }
                    else
                    {
                        Debug.Log("AI should be running");

                        rightUnit.transform.Rotate(new Vector3(0, 180, 0));
                        rightUnitAnimator.Play("RunAway");
                        rightUnitAnimator.Play("Jumping");

                    }
                }
                playNewSound(heavyDrums);
            }

            if(war.Phase == MyEnum.warPhase.over)
            {
                playNewSound(heavyDrums);
                Debug.Log("War should be over now");
                endWar(war, attacker, defender);
            }
       

            updateWarPanel(war);

            if (war.Phase == MyEnum.warPhase.information)
            {
                war.Phase = MyEnum.warPhase.tactic;
            }
           else if (war.Phase == MyEnum.warPhase.tactic)
            {
                war.Phase = MyEnum.warPhase.maneuver;
            }
            else if (war.Phase == MyEnum.warPhase.maneuver)
            {
            
                war.Phase = MyEnum.warPhase.engagement;
            }
            else if (war.Phase == MyEnum.warPhase.engagement)
            {         
               war.Phase = MyEnum.warPhase.maneuver;
            }
            else if(war.Phase == MyEnum.warPhase.conclusion)
            {
                war.Phase = MyEnum.warPhase.over;
            }
        }
    }


    public void processAndGenerateEvents()
    {
        Debug.Log("Time to create and process events!");
        App app = UnityEngine.Object.FindObjectOfType<App>();
        int playerIndex = app.GetHumanIndex();
        Nation player = State.getNations()[playerIndex];
        EventRegister eventLogic = State.eventRegister;
        //eventLogic.DecisionEvents.Clear();
        clearProvinceStatus();
        eventLogic.checkForProvinceRiots();
        eventLogic.generateSupportDissentEvents();
        if (eventLogic.DecisionEvents.Count > 0)
        {
            nextEventTurn(player);
        }


        // Other kinds of events to add:
        // Semi-random events, encourage dissent, diplomatic
        // Corruption, Stability, High Culture, Low Culture can increase the chance that
        // certain events happen.
    }


    public void clearProvinceStatus()
    {
        foreach(Province prov in State.getProvinces().Values)
        {
            prov.setRioting(false);
            prov.setDisaster(false);
            prov.Bonus = false;
            prov.setFunctioning();
        }
    }

    public void closeToolTips()
    {
        TooltipTrigger optionA_ToolTip = optionA.GetComponent<TooltipTrigger>();
        if (optionA_ToolTip != null) // Set the tooltip text.
            optionA_ToolTip.SetText("BodyText", currentEvent.OptionA_ToolTip);
        TooltipTrigger optionB_ToolTip = optionB.GetComponent<TooltipTrigger>();
        optionA_ToolTip.ForceHideTooltip();
        optionB_ToolTip.ForceHideTooltip();
    }
        
    public void showDecisionPanel(Nation player)
    {
        Text optionA_Text = optionA.GetComponentInChildren<Text>();
        optionA_Text.text = currentEvent.OptionA_Text;
        Text optionB_Text = optionB.GetComponentInChildren<Text>();
        optionB_Text.text = currentEvent.OptionB_Text;
        TooltipTrigger optionA_ToolTip = optionA.GetComponent<TooltipTrigger>();
        if (optionA_ToolTip != null) // Set the tooltip text.
            optionA_ToolTip.SetText("BodyText", currentEvent.OptionA_ToolTip);
        TooltipTrigger optionB_ToolTip = optionB.GetComponent<TooltipTrigger>();
        if (optionB_ToolTip != null)
        {
            optionB_ToolTip.SetText("BodyText", currentEvent.OptionB_ToolTip);
        }
        dualDecisionHeadline.text = currentEvent.HeadLine;
        dualDecisionMessage.text = currentEvent.Message;
        if (currentEvent.OptionA_Text.Length == 0)
        {
            optionA.transform.localScale = new Vector3(0, 0, 0);

        }
        else
        {
            optionA.transform.localScale = new Vector3(1, 1, 1);

        }
        if (currentEvent.OptionB_Text.Length == 0)
        {
            optionB.transform.localScale = new Vector3(0, 0, 0);

        }
        else
        {
            optionB.transform.localScale = new Vector3(1, 1, 1);

        }

        eventDecisionTwoPanel.SetActive(true);
    }


    public void endWar(War war, Nation attacker, Nation defender)
    {
        Debug.Log("End war");
        warPanel.SetActive(false);
        App app = UnityEngine.Object.FindObjectOfType<App>();
        int playerIndex = app.GetHumanIndex();
        Nation human = State.getNations()[playerIndex];
        Province prov = State.getProvince(0);
        EventRegister eventLogic = State.eventRegister;
        DecisionEvent newEvent = new DecisionEvent();

        if (war.ProvIndex != -1)
        {
            prov = State.getProvince(war.ProvIndex);
        }

        if (war.AttackerForces == 0 || war.AttackerMorale == 0)
        {
            Debug.Log("Attack fails");
            // Attack fails and defender wins
            attacker.decreasePrestige(2);
            defender.increasePrestige(2);
            attacker.landForces.Morale -= 10;
            defender.landForces.Morale += 10;
            int payment = (int)(attacker.getGold() / 3);
            PlayerPayer.payAnotherPlayer(attacker, defender, payment);

            if (war.AttackerIndex == human.getIndex())
            {
                eventLogic.initializeFailedInvasionEvent(newEvent, currentEvent, war);
            }
            else
            {
                eventLogic.initializeSucessfulDefenseEvent(newEvent, currentEvent, war);     
            }
            currentEvent = newEvent;
            showDecisionPanel(human);
        }

        else if (war.DefenderMorale == 0 || war.DefenderForces == 0)
        {
            // Attack succeeds 
            Debug.Log("Attack succeeds");
            attacker.increasePrestige(2);
            defender.decreasePrestige(2);
            attacker.landForces.Morale += 10;
            defender.landForces.Morale -= 10;
            if (war.ProvIndex > -1)
            {
                prov.changeProvinceControl(defender, attacker);
                MapChange newMapChange = new MapChange(defender.getIndex(), attacker.getIndex(), prov.getIndex());
                List<MapChange> mapChanges = State.MapChanges;
                mapChanges.Add(newMapChange);
            }
            else
            {
                int payment = (int)(defender.getGold() / 3);
                PlayerPayer.payAnotherPlayer(defender, attacker, payment);
            }

            if (war.AttackerIndex == human.getIndex())
            {
                eventLogic.initializeSucessfulInvasionEvent(newEvent, currentEvent, war);
                currentEvent = newEvent;
                showDecisionPanel(human);
            }
            else
            {
                eventLogic.initializeFailedDefenseEvent(newEvent, currentEvent, war);
                currentEvent = newEvent;
                showDecisionPanel(human);
            }
        }
    }
    

    public bool checkIfWarIsOver(War war)
    {
        if(war.AttackerForces == 0 || war.DefenderForces == 0 || war.DefenderMorale == 0 || war.AttackerMorale == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
   

    public void crackdown()
    {
        App app = UnityEngine.Object.FindObjectOfType<App>();
        int playerIndex = app.GetHumanIndex();
        Nation player = State.getNations()[playerIndex];
        
        eventLogic.playerCrackDownOnRiot(currentEvent.Province);
        Province disputedProvince = State.getProvince(currentEvent.Province);
        if (currentEvent.OtherMajor != -1 && !player.culture.Equals(disputedProvince.getCulture()))
        {
            Nation otherMajor = State.getNation(currentEvent.OtherMajor);
            EventResponder responder = otherMajor.getAI().getEventResponder();
            if (responder.demandReferendum(otherMajor, player, disputedProvince))
            {
                //Now you need to update and re-open decision panel
                DecisionEvent newEvent  = new DecisionEvent();
                eventLogic.initalizeAIDemandsreferendumEvent(newEvent, player, otherMajor, disputedProvince);
                currentEvent = newEvent;
                showDecisionPanel(player);
            }
            nextEventTurn(player);
        }
    }


    public void refuseReferendum(Nation player, Nation otherMajor, Province disputedProvince)
    {
        // Refuse to hold a referendum 
        EventResponder responder = otherMajor.getAI().getEventResponder();
        if (responder.warOverRejection(otherMajor, player, disputedProvince))
        {
            // The AI player responds by declaring war on the human player
            DecisionEvent newEvent = new DecisionEvent();
            eventLogic.initializeAI_DeclaresWarEvent(newEvent, player, otherMajor);
            currentEvent = newEvent;
            showDecisionPanel(player);
            //  war.warBetweenAI(player, otherMajor, prov.getIndex());
        }
        else if (responder.boycottOverRefDemandRejection(otherMajor))
        {
            // AI imposes boycott 
            DecisionEvent newEvent = new DecisionEvent();
            eventLogic.initializeAI_BoycottsHumanEvent(newEvent, player, currentEvent);
            currentEvent = newEvent;
            showDecisionPanel(player);
        }
        else
        {
            // AI does nothing and looses face
            otherMajor.decreasePrestige(2);
            nextEventTurn(player);
        }
    }



    public void prepareWarPanel(Nation human, Nation ai, War war)
    {
        Debug.Log("Prepare War Panel");
        Debug.Log("Human player name:" + human.getNationName());
        Debug.Log("AI player name: " + ai.getNationName());
        GameObject playerFlagPrefab = Instantiate(Resources.Load<GameObject>("Flags/Prefabs/" + human.getNationName()));
        playerFlag.ObjectPrefab = playerFlagPrefab.transform;

        GameObject aiFlagPrefab = Instantiate(Resources.Load<GameObject>("Flags/Prefabs/" + ai.getNationName()));
        aiFlag.ObjectPrefab = aiFlagPrefab.transform;

        playerName.text = human.getNationName();
        aiName.text = ai.getNationName();
        //UpdateWarPanelSideInfo(human, ai, war);
        InitializeSidePanelInfo(human, ai, war);

        phase_or_round.text = "Phase I: Information Gathering";
        playerRecon.isOn =  aiRecon.isOn = false;
        playerEsopinage.isOn = aiEspionage.isOn = false;
        playerJudgement.isOn = aiJudgement.isOn = false;
        playerTactics.text = aiTactics.text = "0";
        playerManouver.isOn = aiManeuver.isOn = false;
        playerCombatModifier.text = aiCombatModifier.text = "0";
        phaseRoundReport.text = "Press Continue to begin the War";

        leftUnit = Instantiate(ChooseUnitPrefab(human), new Vector3(500, 0, 45), Quaternion.Euler(0, 120, 0), leftBattleTerrain.transform);
        leftUnit.transform.localPosition = new Vector3(500, 0, 35);
        rightUnit = Instantiate(ChooseUnitPrefab(ai), new Vector3(500, 0, 45), Quaternion.Euler(0, -120, 0), rightBattleTerrain.transform);
        rightUnit.transform.localPosition = new Vector3(500, 0, 35);

        leftUnit.transform.localScale = new Vector3(16, 16, 16);
        rightUnit.transform.localScale = new Vector3(16, 16, 16);
        leftUnitAnimator = leftUnit.GetComponent<Animator>();
        rightUnitAnimator = rightUnit.GetComponent<Animator>();

        

        AudioSource nextWarPhaseSoundPlayer = nextPhaseRound.GetComponent<AudioSource>();
        warPanel.SetActive(true);
        nextWarPhaseSoundPlayer.clip = heavyDrums;
        nextWarPhaseSoundPlayer.Play();
        nextWarPhaseSoundPlayer.clip = boltLoad;


    }


    public GameObject ChooseUnitPrefab(Nation player)
    {
        MyEnum.Era era = State.era;
        List<int> majorIndexes = State.getMajorNations();
        int playerIndex = player.getIndex();
        if (era == MyEnum.Era.Early)
        {
            if (majorIndexes[0] == playerIndex)
            {
                return ZeroOne;
            }
            else if (majorIndexes[1] == playerIndex)
            {
                return OneOne;
            }
            else if (majorIndexes[2] == playerIndex)
            {
                return TwoOne;
            }
            else if (majorIndexes[3] == playerIndex)
            {
                return ThreeOne;
            }
            else if (majorIndexes[4] == playerIndex)
            {
                return FourOne;
            }
            else 
            {
                return FiveOne;
            }
        }
        else if (era == MyEnum.Era.Middle)
        {
            if (majorIndexes[0] == playerIndex)
            {
                return ZeroTwo;
            }
            else if (majorIndexes[1] == playerIndex)
            {
                return OneTwo;
            }
            else if (majorIndexes[2] == playerIndex)
            {
                return TwoTwo;
            }
            else if (majorIndexes[3] == playerIndex)
            {
                return ThreeTwo;
            }
            else if (majorIndexes[4] == playerIndex)
            {
                return FourTwo;
            }
            else 
            {
                return FiveTwo;
            }
        }
        else if (era == MyEnum.Era.Late)
        {
            if (majorIndexes[0] == playerIndex)
            {
                return ZeroThree;
            }
            else if (majorIndexes[1] == playerIndex)
            {
                return OneThree;
            }
            else if (majorIndexes[2] == playerIndex)
            {
                return TwoThree;
            }
            else if (majorIndexes[3] == playerIndex)
            {
                return ThreeThree;
            }
            else if (majorIndexes[4] == playerIndex)
            {
                return FourThree;
            }
            else 
            {
                return FiveThree;
            }
        }
        else
        {

            return ZeroOne;
        }



    }

  


    public void updateWarPanel(War war)
    {
        Debug.Log("Update war panel");
        App app = UnityEngine.Object.FindObjectOfType<App>();
        int playerIndex = app.GetHumanIndex();
        Nation human = State.getNations()[playerIndex];
        Nation attacker = State.getNation(war.AttackerIndex);
        Nation defender = State.getNation(war.DefenderIndex);
        if (war.AttackerIndex == human.getIndex())
        {
            Debug.Log("Human Player is the Attacker");
            _updateWarPanel(attacker, defender, war);
        }
        else
        {
            Debug.Log("AI Player is the Attacker");
            _updateWarPanel(defender, attacker, war);
        }
    }


    private void _updateWarPanel(Nation human, Nation ai, War war)
    {
        Nation attacker = State.getNation(war.AttackerIndex);
        Nation defender = State.getNation(war.DefenderIndex);
        Debug.Log("__Update war panel");
        if (war.Phase == MyEnum.warPhase.information)
        {
            phase_or_round.text = "Phase I: Information Gathering";
        }
        if (war.Phase == MyEnum.warPhase.tactic)
        {
            phase_or_round.text = "Phase II: Generals Decide on Tactics";
        }
        else if(war.Phase == MyEnum.warPhase.maneuver)
        {
            phase_or_round.text = "Phase III: Army Maneuvers to Execute Commands";
        }
        else if(war.Phase == MyEnum.warPhase.engagement)
        {
            phase_or_round.text = "Phase IV: Army Engages Enemy: Round: " + war.Round;
        }
        else if(war.Phase == MyEnum.warPhase.conclusion)
        {
            phase_or_round.text = "Phase V: Conclusion";

        }

        UpdateWarPanelSideInfo(human, ai, war);

        if (war.AttackerIndex == human.getIndex())
        {
            playerRecon.isOn = war.AttackerRecon;
            playerEsopinage.isOn = war.AttackerIntel;
            playerJudgement.isOn = war.AttackerJudgement;
            playerTactics.text = war.AttackerTactics.ToString();
            playerManouver.isOn = war.AttackerManeuver;
            playerCombatModifier.text = war.AttackerDmgModifier.ToString();

            aiRecon.isOn = war.DefenderRecon;
            aiEspionage.isOn = war.DefenderIntel;
            aiJudgement.isOn = war.DefenderJudgement;
            aiTactics.text = war.DefenderTactics.ToString();
            aiManeuver.isOn = war.DefenderManeuver;
            aiCombatModifier.text = war.DefenderDmgModifier.ToString();
        }
        else
        {
            playerRecon.isOn = war.DefenderRecon;
            playerEsopinage.isOn = war.DefenderIntel;
            playerJudgement.isOn = war.DefenderJudgement;
            playerTactics.text = war.DefenderTactics.ToString();
            playerManouver.isOn = war.DefenderManeuver;
            playerCombatModifier.text = war.DefenderDmgModifier.ToString();

            aiRecon.isOn = war.AttackerRecon;
            aiEspionage.isOn = war.AttackerIntel;
            aiJudgement.isOn = war.AttackerJudgement;
            aiTactics.text = war.AttackerTactics.ToString();
            aiManeuver.isOn = war.AttackerManeuver;
            aiCombatModifier.text = war.AttackerDmgModifier.ToString();
        }

        MyEnum.warPhase phase = war.Phase;
        string report = "";
        if(phase == MyEnum.warPhase.information)
        {
            if (playerRecon.isOn)
            {
                report += "Our reconnaissance has detected " + ai.getNationName() + "'s forces.\n";
            }
            else
            {
                report += "Our reconnaissance have been unable to detect the movements of " + ai.getNationName() + "'s forces.\n";
            }
            if (playerEsopinage.isOn)
            {
                report += human.getNationName() + "We have gained valuable information on the enemies’ plans.\n";
            }
            else
            {
                report += "Our useless intelligence operatives have failed to obtain any critical information regarding the" +
                    "enemies' plans.\n";
            }

            if (aiRecon.isOn)
            {
                report += "The movement our our forces have been detected by enemy reconnaissance.\n";
            }
            else
            {
                report += "Our forces were not detected by reconnaissance.\n";
            }

            if (aiEspionage.isOn)
            {
                report += "It seems that the enemy somehow got a hold of our battle plans.\n";
            }
            else
            {
                report += "The enemy appears to have no valuable information regarding our battle plans.\n";
            }
        }

        else if(phase == MyEnum.warPhase.tactic)
        {
            if (aiJudgement.isOn)
            {
                report += ai.getNationName() + "' generals have displayed tremendous tactical insight.\n";
            }
            else
            {
                report += ai.getNationName() + "' generals seem to have dipped a little too deep into the brandy bottle today.\n";
            }
            if (playerJudgement.isOn)
            {
                report += " Our generals have seized the moment and drawn up some excellent war plans. \n";
            }
            else
            {
                report += " Our generals have drawn up some underwhelming plans. \n";
            }

        }

        else if(phase == MyEnum.warPhase.maneuver)
        {
            if (playerManouver.isOn)
            {
                report += "We managed to out maneuver the enemie's forces!\n";
            }
            else if(aiManeuver.isOn)
            {
                report += "Drat! Our army has been out-maneuvered by those of " + ai.getName() + ".\n";
            }
            else
            {
                report += "Neither army has conducted is maneuver especially well.\n";

            }
        }

        else if(phase == MyEnum.warPhase.engagement)
        {
            if (war.AttackerIndex == human.getIndex())
            {
                report += "We destroyed " + war.AttackerAttackDamage + " enemy units during this round.\n";
                report += "We lost " + war.DefenderAttackDamage + " units this round.\n";
                report += "Our assult reduced enemy morale by " + war.AttackerShockDamage + " this round.\n";
                report += "Our army's morale was reduced by " + war.DefenderShockDamage + " this round.\n";
            }
            else
            {
                report += "We destroyed " + war.DefenderAttackDamage + " enemy units during this round.\n";
                report += "We lost " + war.AttackerAttackDamage + " units this round.\n";
                report += "Our assult reduced enemy morale by " + war.DefenderShockDamage + " this round.\n";
                report += "Our army's morale was reduced by " + war.AttackerShockDamage + " this round.\n";
            }
        }
        else if (phase == MyEnum.warPhase.conclusion)
        {
            bool defenderWon = war.defenderWon();
            if (war.AttackerIndex == human.getIndex())
            {
                // player is attacker
                if (defenderWon)
                {
                    if (war.AttackerForces == 0)
                    {
                        report += "Alas! Our forces have been completely destroyed! The battle is lost.\n";
                    }
                    else
                    {
                        report += "After being decisively outfought, our forces have been forced to retreat.";
                    }
                }
                else
                {
                    if (war.DefenderForces == 0)
                    {
                        report += "Our proud army had completely annihilated what had been " + defender.getName() + "'s pathetic forces!";
                    }
                    else
                    {
                        report += "After being outlcassed by our capable military, " + defender.getName() + "'s forces had no choice but to flee the field.";

                    }
                }
            }
            else
            {
                // Player is the defender
                if (defenderWon)
                {
                    if (war.AttackerForces == 0)
                    {
                        report += "Alas! Our forces have been completely destroyed! The battle is lost.\n";
                    }
                    else
                    {
                        report += "After being outlcassed by our capable military, " + defender.getName() + "'s forces had no choice but to flee the field.";
                    }
                }
                else
                {
                    if (war.DefenderForces == 0)
                    {
                        report += "Our proud army had completely annihilated what had been " + defender.getName() + "'s pathetic forces!";
                    }
                    else
                    {
                        report += "After being decisively outfought, our forces have been forced to retreat.";

                    }
                }
            }

        }
        phaseRoundReport.text = report;
        
     

    }
    


    public void InitializeSidePanelInfo(Nation human, Nation ai, War war)
    {
        playerShockAttribute.text = human.landForces.Shock.ToString();
        aiShockAttribute.text = ai.landForces.Shock.ToString();

        playerAttackAttribute.text = human.landForces.Attack.ToString();
        aiAttackAttribute.text = ai.landForces.Attack.ToString();

        playerReconValue.text = human.landForces.Recon.ToString();
        aiReconValue.text = ai.landForces.Recon.ToString();

        playerIntelligenceValue.text = human.landForces.Espionage.ToString();
        aiIntelligenceValue.text = ai.landForces.Espionage.ToString();

        playerJudgmentValue.text = human.landForces.Judgment.ToString();
        aiJudgmentValue.text = ai.landForces.Judgment.ToString();

        playerManeuverValue.text = human.landForces.Maneuver.ToString();
        aiManeuverValue.text = ai.landForces.Maneuver.ToString();

    }

    public void UpdateWarPanelSideInfo(Nation human, Nation ai, War war)
    {
        Debug.Log("Update Side Info (War)");

        playerShockAttribute.text = human.landForces.Shock.ToString();
        aiShockAttribute.text = ai.landForces.Shock.ToString();
        if (war.AttackerIndex == human.getIndex())
        {
            // Player is attacker
            playerForces.text = war.AttackerForces.ToString();
            aiForces.text = war.DefenderForces.ToString();
            playerMorale.text = war.AttackerMorale.ToString();
            aiMorale.text = war.DefenderMorale.ToString();
            playerAttackAttribute.text = human.landForces.Attack.ToString();
            aiAttackAttribute.text = ai.landForces.Defense.ToString();
        }
        else
        {
            // Player is defender
            playerForces.text = war.DefenderForces.ToString();
            aiForces.text = war.AttackerForces.ToString();
            playerMorale.text = war.DefenderMorale.ToString();
            aiMorale.text = war.DefenderMorale.ToString();
            playerAttackAttribute.text = human.landForces.Defense.ToString();
            aiAttackAttribute.text = ai.landForces.Attack.ToString();
        }
    }

    public void warPhaseOne(Nation attacker, Nation defender, War war)
    {
        Debug.Log("WarPhaseOne");
        war.knowledgePhase(attacker, defender);
        if (war.AttackerRecon)
        {
            playerRecon.isOn = true;
        }
        else
        {
            playerRecon.isOn = false;
        }
    }

}
