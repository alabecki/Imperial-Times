using assemblyCsharp;
using System;
using System.Collections;
using System.Collections.Generic;
using UI.Tables;
using UnityEngine;
using UnityEngine.UI;
using EasyUIAnimator;

public class SelectNationButton : MonoBehaviour {

   public Button SelectionButton;
 //  public TableLayout nationList;
    public Image nationFlag;
    public Text nationName;
    public Text prestigePoints;
    public Text prestigeRank;
    public Text industrialPoints;
    public Text industrialRank;
    public Text militaryPoints;
    public Text militaryRank;
    public Text nationPopValue;
    public Text nationGoldValue;
    public Image nationStability;

    public GameObject currentWars;
    public GameObject currentDefensiveAlliances;
    public GameObject currentFullAlliances;
    public GameObject currentCollonies;
    public GameObject currentSpheres;
    public GameObject currentEmbargos;

    public Text ourRelationsText;
    public Button improveRelations;
    public Button worsenRelations;
    public Button declareWar;
    public Button gainCB;
    public Button offerAliance;
    public Button embargo;
    public Button negotiate;
    public Button openBorders;
    public Button leaveAlliance;

    public GameObject warClaimPanel;
    public TableLayout warClaimListTable;
    public TableRow claimRow;
    public Button selectClaim;
    public Button confirmWarClaim;
    private UIAnimation claimOpen;
    private UIAnimation claimClose;
    public TableLayout warClaimDetails;
    private string currentClaimID;
    private bool warClaimFlag = false;

    public Text reasonRejectAlliance;

    private bool diplomacyFromProv;

    public Text selectedNationName;
    public GameObject DiplomacyTab;
    public Button diplomacyButton;
    private bool diploFlag;

    private UIAnimation diplomacyFade;
    private UIAnimation diplomacyResize;

    private Graphic[] diploReact;
    private bool nationHasBeenSelected = false;
    public Button diplomacyProvinceButton;


    // Use this for initialization
    void Start () {

      //  diploReact = DiplomacyTab.GetComponentsInChildren<Graphic>();

     //   warClaimPanel.SetActive(false);
        SelectionButton.onClick.AddListener(delegate { selectNation(); });

     //   improveRelations.onClick.AddListener(delegate { improveNationRelations(); });
      //  worsenRelations.onClick.AddListener(delegate { worsenNationRelations(); });
      //  declareWar.onClick.AddListener(delegate { declareNationWar(); });
      //  gainCB.onClick.AddListener(delegate { gainNationCB(); });
      //  offerAliance.onClick.AddListener(delegate { alterAlliance(); });
      //  embargo.onClick.AddListener(delegate { alterEmbargo(); });
      //  negotiate.onClick.AddListener(delegate { negotiateNation(); });
      //  openBorders.onClick.AddListener(delegate { openBordersNation(); });
      //  leaveAlliance.onClick.AddListener(delegate { leaveAllianceNation(); });
       // diplomacyButton.onClick.AddListener(delegate { showDiplomacyPanel(); });
      //  diplomacyProvinceButton.onClick.AddListener(delegate { showDiplomacyPanelProv(); });

    }

    private void diploEnterTop()
    {
        foreach (Graphic graphic in diploReact)
        {
            diplomacyResize = UIAnimator.Scale
              (DiplomacyTab.GetComponent<RectTransform>(), new Vector3(0, 0, 0), new Vector3(1, 1, 1), 0.1f);
            diplomacyFade = UIAnimator.ChangeColor(graphic, new Color(1, 1, 1, 0), new Color(1, 1, 1, 1), 1.2f);
            diplomacyResize.SetDelay(0f);
            diplomacyResize.Play();
            diplomacyFade.Play();
        }
    }

    private void diploExitTop()
    {
        foreach (Graphic graphic in diploReact)
        {
            diplomacyFade = UIAnimator.ChangeColor(graphic, new Color(1, 1, 1, 1), new Color(1, 1, 1, 0), 1.2f);
            diplomacyResize = UIAnimator.Scale
                (DiplomacyTab.GetComponent<RectTransform>(), new Vector3(1, 1, 1), new Vector3(0, 0, 0), 0.1f);
            diplomacyResize.SetDelay(1.25f);
            diplomacyFade.Play();
            diplomacyResize.Play();
        }
    }

    private void showDiplomacyPanel()
    {
        Debug.Log("Clicked on Diplo button");
        diplomacyFromProv = false;
        nationHasBeenSelected = false;

        if (DiplomacyTab.activeSelf == false || diploFlag == false)
        {
            fetchNationForDiplomacy();
            diploFlag = true;
            DiplomacyTab.SetActive(true);
            // diplomacyEnter.Play();
            diploEnterTop();
        }
        else
        {
            diploFlag = false;
            diploExitTop();
            // diplomacyExit.Play();
            //   DiplomacyTab.SetActive(false);
        }
    }

    private void showDiplomacyPanelProv()
    {
        {
            Debug.Log("Clicked on Diplo button");
            diplomacyFromProv = true;
            nationHasBeenSelected = false;
            if (DiplomacyTab.activeSelf == false || diploFlag == false)
            {

                fetchNationForDiplomacy();
                DiplomacyTab.SetActive(true);
                diploEnterTop();
            }
            else
            {
                diploFlag = false;
                diploExitTop();
                //  diplomacyExit.Play();
            }
        }
    }

    private void selectNation()
    {
       diplomacyFromProv = false;
        nationHasBeenSelected = true;
        fetchNationForDiplomacy();
    }


    private void fetchNationForDiplomacy()
    {
        App app = UnityEngine.Object.FindObjectOfType<App>();
        int playerIndex = app.GetHumanIndex();
        Nation player = State.getNations()[playerIndex];
        Nation chosenNation = State.getNations()[0];
        int otherIndex = 0;
        if (diplomacyFromProv == true)
        {

            chosenNation = State.GetNationByName(selectedNationName.text);
            Debug.Log("Chosen nation: " + chosenNation.getNationName() + " index: " + chosenNation.getIndex());
            State.setCurrentSelectedNationDiplomacy(chosenNation.getIndex());
        }
        else if (nationHasBeenSelected == true)
        {
            Debug.Log("Row name is: " + SelectionButton.transform.parent.parent.name);
            otherIndex = Int32.Parse(SelectionButton.transform.parent.parent.name);
            State.setCurrentSelectedNationDiplomacy(otherIndex);
            chosenNation = State.getNations()[State.getCurrentSlectedNationDiplomacy()];
            Debug.Log("Chosen nation: " + chosenNation.getNationName() + " index: " + chosenNation.getIndex());

        }
        else if (nationHasBeenSelected == false)
        {
            for (int i = 0; i < State.getNations().Count; i++)
            {
                Nation thisNation = State.getNations()[i];
                Debug.Log("Chosen nation: " + chosenNation.getNationName() + " index: " + chosenNation.getIndex());

                if (thisNation.getType() == MyEnum.NationType.major && thisNation.getIndex() != player.getIndex())
                {
                    chosenNation = State.getNations()[i];
                    State.setCurrentSelectedNationDiplomacy(chosenNation.getIndex());
                }
            }
        }

        otherIndex = chosenNation.getIndex();
        int relation = player.Relations[otherIndex];

        ourRelationsText.text = relation.ToString();

        if (relation < 20)
        {
            ourRelationsText.color = Color.red;
        }
        else if (relation < 40)
        {
            ourRelationsText.color = new Color(255, 165, 0);
        }
        else if (relation < 60)
        {
            ourRelationsText.color = Color.yellow;
        }
        else if (relation < 80)
        {
            ourRelationsText.color = Color.green;
        }
        else
        {
            ourRelationsText.color = Color.blue;
        }
        nationFlag.sprite = Resources.Load("Flags/" + chosenNation.getNationName().ToString(), typeof(Sprite)) as Sprite;
        nationName.text = chosenNation.getNationName().ToString();
        prestigePoints.text = chosenNation.getPrestige().ToString();
        prestigeRank.text = State.history.getPrestigeRanking(otherIndex).ToString();
        // industrialPoints.text = chosenNation.getIndustrialScore().ToString();
        industrialRank.text = State.history.getMilitaryRanking(otherIndex).ToString();
        // militaryPoints.text = chosenNation.getMilitaryScore().ToString();
        militaryRank.text = State.history.getMilitaryRanking(otherIndex).ToString();
        nationGoldValue.text = chosenNation.getGold().ToString();
        int nationStab = chosenNation.Stability;
        if (nationStab == -3)
        {
            nationStability.sprite = Resources.Load("Sprites/Stability/-3", typeof(Sprite)) as Sprite;
        }
        else if (nationStab == -2)
        {
            nationStability.sprite = Resources.Load("Sprites/Stability/-2", typeof(Sprite)) as Sprite;
        }
        else if (nationStab == -1)
        {
            nationStability.sprite = Resources.Load("Sprites/Stability/-1", typeof(Sprite)) as Sprite;
        }
        else if (nationStab == 0)
        {
            nationStability.sprite = Resources.Load("Sprites/Stability/0", typeof(Sprite)) as Sprite;
        }
        else if (nationStab == 1)
        {
            nationStability.sprite = Resources.Load("Sprites/Stability/1", typeof(Sprite)) as Sprite;
        }
        else if (nationStab == 2)
        {
            nationStability.sprite = Resources.Load("Sprites/Stability/2", typeof(Sprite)) as Sprite;
        }
        else
        {
            nationStability.sprite = Resources.Load("Sprites/Stability/3", typeof(Sprite)) as Sprite;
        }

        int warCount = 1;
        int defAllianceCount = 1;
        int embargoCount = 1;
        //foreach (Nation nation in State.getNations().Values)
        //{
        //   if (nation.getType() == MyEnum.NationType.oldMinor || nation.getIndex() == otherIndex)
        //    {
        //       continue;
        //   }
        //  Debug.Log("Initial Nation is: " + chosenNation.getNationName().ToString());
        // Debug.Log("Other Nation is: " + nation.getNationName().ToString() + " index: " + nation.getIndex());

        //  int relation = chosenNation.Relations[nation.getIndex()];
        //  if (relationToThisNation.isAtWar())
        /*   {
               if (warCount < 5)
               {

                   currentWars.transform.GetChild(warCount).GetChild(0).GetComponent<Image>().sprite =
                        Resources.Load("Flags/" + nation.getNationName(), typeof(Sprite)) as Sprite;
                   warCount += 1;
               }
           }
           if (relationToThisNation.isDefensiveAlliance())
           {
               if (defAllianceCount < 5)
               {

                   currentDefensiveAlliances.transform.GetChild(defAllianceCount).GetChild(0).GetComponent<Image>().sprite =
                   Resources.Load("Flags/" + nation.getNationName(), typeof(Sprite)) as Sprite;
                   defAllianceCount += 1;
               }
           }

           if (relationToThisNation.isEmbargoing())
           {
               if (embargoCount < 5)
               {
                   currentEmbargos.transform.GetChild(embargoCount).GetChild(0).GetComponent<Image>().sprite =
                  Resources.Load("Flags/" + nation.getNationName(), typeof(Sprite)) as Sprite;
                   embargoCount += 1;
               }
           }
       }

       for (int col = 1; col <= chosenNation.getColonies().Count; col++)
       {
           Nation thisColony = State.getNations()[chosenNation.getColonies()[col]];
           currentCollonies.transform.GetChild(col).GetChild(0).GetComponent<Image>().sprite =
               Resources.Load("Flags/" + thisColony.getNationName(), typeof(Sprite)) as Sprite;
           if (col > 4)
           {
               break;
           }
       }

       for (int sp = 1; sp <= chosenNation.getSpheres().Count; sp++)
       {
           Nation thisSphere = State.getNations()[chosenNation.getColonies()[sp]];
           currentSpheres.transform.GetChild(sp).GetChild(0).GetComponent<Image>().sprite =
               Resources.Load("Flags/" + thisSphere.getNationName(), typeof(Sprite)) as Sprite;
           if (sp > 4)
           {
               break;
           }

       }

       improveRelations.interactable = true;
       worsenRelations.interactable = true;
       gainCB.interactable = true;
       declareWar.interactable = true;
       offerAliance.interactable = true;
       embargo.interactable = true;
       negotiate.interactable = true;
       openBorders.interactable = true;
       leaveAlliance.interactable = true;

       if (player.InfulencePoints < 1)
       {
           improveRelations.interactable = false;
           worsenRelations.interactable = false;
           gainCB.interactable = false;
           offerAliance.interactable = false;
       }

       if(relationFromChosenPlayer.getAttitude() == 100)
       {
           improveRelations.interactable = false;

       }

       if (relationFromChosenPlayer.getAttitude() == 0)
       {
           worsenRelations.interactable = false;

       }
       bool possCB = false;



       if (relationFromChosenPlayer.getAttitude() < 75)
       {
           offerAliance.interactable = false;
       }

       if(relationToChosenPlayer.getAttitude() > 35)
       {
           embargo.interactable = false;
       }



       if(relationFromChosenPlayer.getAttitude() < 80)
       {
           openBorders.interactable = false;
       }


       if (relationToChosenPlayer.isAtWar())
       {
           declareWar.GetComponentInChildren<Text>().text = "Offer Peace";
           declareWar.interactable = true;

       }

       if (relationToChosenPlayer.isDefensiveAlliance())
       {
           offerAliance.GetComponentInChildren<Text>().text = "Offer Full Alliance";
           if (relationFromChosenPlayer.getAttitude() > 88)
           {
               offerAliance.interactable = true;
           }
           else
           {
               offerAliance.interactable = false;
           }
       }

   }

   private void improveNationRelations()
   {
      int otherIndex = State.getCurrentSlectedNationDiplomacy();
       App app = UnityEngine.Object.FindObjectOfType<App>();
       int playerIndex = app.GetHumanIndex();
      Nation player = State.getNations()[playerIndex];
       Debug.Log("Other index: " + otherIndex);
       Nation otherNation = State.getNations()[otherIndex];
       PlayerReceiver.improveRelations(player, otherIndex);

       ourRelationsText.text = player.getRelationFromThisPlayer(otherIndex).getAttitude().ToString();
   }

   private void worsenNationRelations()
   {
       int otherIndex = State.getCurrentSlectedNationDiplomacy();
       App app = UnityEngine.Object.FindObjectOfType<App>();
       int playerIndex = app.GetHumanIndex();
       Nation player = State.getNations()[playerIndex];
       Nation otherNation = State.getNations()[otherIndex];
       PlayerReceiver.worsenRelations(player, otherIndex);
       ourRelationsText.text = player.getRelationFromThisPlayer(otherIndex).getAttitude().ToString();
   }

   private void declareNationWar()
   {
       App app = UnityEngine.Object.FindObjectOfType<App>();
       int playerIndex = app.GetHumanIndex();
      Nation player = State.getNations()[playerIndex];
     int  otherIndex = State.getCurrentSlectedNationDiplomacy();
       Relation relationTo = player.getRelationToThisPlayer(otherIndex);
       if (relationTo.isAtWar() == false)
       {
           PlayerReceiver.declareWar(player, otherIndex);
           declareWar.GetComponent<Text>().text = "Make Peace";
           declareWar.interactable = false;
       }
       else
       {
           //Must check if other player is willing to make peace
       }
   }




   private void alterAlliance()
   {
       App app = UnityEngine.Object.FindObjectOfType<App>();
       int playerIndex = app.GetHumanIndex();
     Nation  player = State.getNations()[playerIndex];
     int   otherIndex = State.getCurrentSlectedNationDiplomacy();
       Nation otherNation = State.getNations()[otherIndex];

   }

   private void alterEmbargo()
   {
       App app = UnityEngine.Object.FindObjectOfType<App>();
       int playerIndex = app.GetHumanIndex();
      Nation player = State.getNations()[playerIndex];
      int otherIndex = State.getCurrentSlectedNationDiplomacy();

       Relation relationTo = player.getRelationToThisPlayer(otherIndex);
       Relation relationFrom = player.getRelationFromThisPlayer(otherIndex);
       if (!relationTo.isEmbargoing())
       {
           relationTo.startEmbargo();
           relationTo.adjustAttude(-10);
       }
       else
       {
           relationTo.endEmbargo();
           relationTo.adjustAttude(5);
       }

   }

   private void negotiateNation()
   {

   }


 */
    }
    }




