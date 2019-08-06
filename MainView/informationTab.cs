using assemblyCsharp;
using EasyUIAnimator;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class informationTab : MonoBehaviour
{
    public GameObject informationPanel;
    public Toggle informationToggle;

    public Button moreDetails;

    private UIAnimation informationEnter;
    private UIAnimation informationExit;
    private bool informationFlag;

    public Text prestige;
    public Text AP;
    public Text PP;
    public Text gold;
    public Text researchPoints;
    public Text colonialPoints;
    public Text infulencePoints;
    public Text corruption;
    public Text stability;
   // public Text morale;
    public Text IP;
    public Text savings;
    public Text interestReceived;
    public Text debt;
    public Text interestPayed;


    // Start is called before the first frame update
    void Start()
    {
        informationPanel.SetActive(false);
        RectTransform informationRect = informationPanel.GetComponent<RectTransform>();

        informationToggle.onValueChanged.AddListener(delegate { togleInformationPanel(); });
        informationEnter = UIAnimator.Move(informationRect, new Vector2(-0.2f, 0.48f), new Vector2(0.073f, 0.48f), 1.1f).SetModifier(Modifier.PolyIn);
        informationExit = UIAnimator.Move(informationRect, new Vector2(0.073f, 0.48f), new Vector2(-0.2f, 0.48f), 1.1f).SetModifier(Modifier.PolyOut);

        moreDetails.onClick.AddListener(delegate { showMoreInformationTab(); });

    }

    private void togleInformationPanel()
    {
        if (informationPanel.activeSelf == false || informationFlag == false)
        {
            updateInformationPanel();
            informationPanel.SetActive(true);
            informationEnter.Play();
            informationFlag = true;
            return;
        }
        else
        {
            if (informationFlag == true)
            {
                informationExit.Play();
                informationFlag = false;
                //  inventoryPanel.SetActive(false);
            }
        }
    }

    private void updateInformationPanel()
    {
        App app = UnityEngine.Object.FindObjectOfType<App>();
        int playerIndex = app.GetHumanIndex();
        Nation player = State.getNations()[playerIndex];
        prestige.text = player.getPrestige().ToString("0.0");
        gold.text = player.getGold().ToString("0.0");
        WorldBank bank = State.bank;
        int bondSize = bank.BondSize;
        savings.text = (bank.getDeposits(player) * bondSize) .ToString();
        interestReceived.text = player.InterestCollectedLastTurn.ToString("0.0");
        debt.text = (bank.getDebt(player) * bondSize).ToString();
        interestPayed.text = player.InterestPayedLastTurn.ToString("0.0");
        AP.text = player.getAP().ToString("0.0");
        PP.text = player.getDP().ToString("0.0");
        researchPoints.text = player.Research.ToString("0.0");
        colonialPoints.text = player.ColonialPoints.ToString("0.0");
        infulencePoints.text = player.InfulencePoints.ToString("0.0");
        corruption.text = player.GetCorruption().ToString("G");
        stability.text = player.Stability.ToString("G");

       // morale.text = player.landForces.Morale.ToString("0.0");
       
        IP.text = player.getIP().ToString();
    }

    private void showMoreInformationTab()
    {

        App app = UnityEngine.Object.FindObjectOfType<App>();
        int playerIndex = app.GetHumanIndex();
        Nation player = State.getNations()[playerIndex];

        prestige.text = player.getPrestige().ToString("0.0");
        AP.text = player.getAP().ToString("0.0");
        PP.text = player.getDP().ToString("0.0");

        gold.text = player.getGold().ToString("0.0");
        researchPoints.text = player.Research.ToString("0.0");

        colonialPoints.text = player.ColonialPoints.ToString("0.0");
        infulencePoints.text = player.InfulencePoints.ToString("G");
        corruption.text = player.GetCorruption().ToString("G");
        stability.text = player.Stability.ToString("0.0");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
