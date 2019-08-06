using assemblyCsharp;
using EasyUIAnimator;
using System;
using System.Collections;
using System.Collections.Generic;
using UI.Tables;
using UnityEngine;
using UnityEngine.UI;

public class transportView : MonoBehaviour
{
    public Button transportButton;
    public GameObject transportPanel;
    public Text transportCapacity;
    public Text coalCapacity;
    public Button addTransportCapacityButton;
    public TableLayout resourceTableA;
    public TableLayout resourceTableB;

    public UI_Updater uiUpdater;

    public GameObject developmentPanel;
    public GameObject MarketTab;
    public GameObject productionPanel;

    public Camera mainCamera;
    public Camera cityCamera;
    public Camera diplomacyCamera;

    private UIAnimation progressExit;
    private UIAnimation marketExit;
    private UIAnimation productionExit;


    // Start is called before the first frame update
    void Start()
    {
        RectTransform progressRect = developmentPanel.GetComponent<RectTransform>();
        RectTransform marketRect = MarketTab.GetComponent<RectTransform>();
        RectTransform productionRect = productionPanel.GetComponent<RectTransform>();
        progressExit = UIAnimator.Move(progressRect, new Vector2(0.5f, 0.45f), new Vector2(0.5f, 1.2f), 1f).SetModifier(Modifier.Linear);
        marketExit = UIAnimator.Move(marketRect, new Vector2(0.5f, 0.45f), new Vector2(0.5f, 1.2f), 1f).SetModifier(Modifier.Linear);
        productionExit = UIAnimator.Move(productionRect, new Vector2(0.5f, 0.45f), new Vector2(0.5f, 1.2f), 1f).SetModifier(Modifier.Linear);


        transportPanel.SetActive(false);

        addTransportCapacityButton.onClick.AddListener(delegate { addTransportCapacity(); });

        transportButton.onClick.AddListener(delegate { showTransportPanel(); });

        Debug.Log("Begin start for transport sliders. #########################################################################");
        for (int i = 0; i < 6; i++)
        {
            Slider resSliderA = resourceTableA.Rows[i].Cells[2].GetComponentInChildren<Slider>();
            resSliderA.onValueChanged.AddListener(delegate { changeTransportSlider(); });
            Slider resSliderB = resourceTableB.Rows[i].Cells[2].GetComponentInChildren<Slider>();
            resSliderB.onValueChanged.AddListener(delegate { changeTransportSlider(); });
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void showTransportPanel()
    {
        if (diplomacyCamera.enabled)
        {
            diplomacyCamera.enabled = false;
            mainCamera.enabled = true;
        }
        if (transportPanel.activeSelf == true)
        {
            transportPanel.SetActive(false);
        }
        else
        {
            
            updateTransportPanel();
            transportPanel.SetActive(true);
        }

    }

    private void updateTransportPanel()
    {
        Debug.Log("Begin Updating Transport Panel");
        App app = UnityEngine.Object.FindObjectOfType<App>();
        Nation player = State.getNations()[app.GetHumanIndex()];
        int transportFlow = PlayerCalculator.calculateTransportFlow(player);
        int totalNumberProv = PlayerCalculator.getTotalNumberOfProvinces(player);
        int realTransportCapacity = PlayerCalculator.calculateMaxTransportFlow(player);
        Debug.Log("Transport Flow: " + transportFlow);
        Debug.Log("Current Transport Capacity: " + realTransportCapacity);
        // player.industry.setTransportFlow(PlayerCalculator.calculateTransportFlow(player));
        float coalUsedForTransport = (transportFlow - totalNumberProv) * 0.2f;
        transportCapacity.text = transportFlow.ToString() + "/" + realTransportCapacity.ToString();
        coalCapacity.text = player.getNumberResource(MyEnum.Resources.coal).ToString();
        int remainingFlow = realTransportCapacity - transportFlow;
        Debug.Log("Remaining Flow Capacity: " + remainingFlow);

        if (PlayerCalculator.canBuildTrain(player))
        {
            addTransportCapacityButton.interactable = true;
        }
        else
        {
            addTransportCapacityButton.interactable = false;
        }

        // Counter used for switching from first to second table in Production GUI
        int counter = 0;
        foreach (MyEnum.Resources res in Enum.GetValues(typeof(MyEnum.Resources)))
        {
            int producing = PlayerCalculator.getResourceProducing(player, res);
            int currentFlow = player.getResTransportFlow(res);
            Debug.Log("Resource is: " + res + "---------------------------------------------");
            Debug.Log("Producing: " + producing);
            Debug.Log("Current Flow: " + currentFlow);
            //int systemFlow = player.industry.getTransportFlow();
            int capacity = Math.Min(producing - currentFlow, remainingFlow);
            Debug.Log("Remaining " + res + " capacity: " + capacity);
            int max = currentFlow + capacity;
            Debug.Log("Max: " + max);
            if (counter <= 5)
            {
                Debug.Log("check 2553");
                Text resFlow = resourceTableA.Rows[counter].Cells[1].GetComponentInChildren<Text>();
                Slider resSlider = resourceTableA.Rows[counter].Cells[2].GetComponentInChildren<Slider>();
                resFlow.text = currentFlow + "/" + producing;
                if (max == currentFlow && max < producing)
                {
                    Debug.Log("poop");
                    resFlow.color = new Color32(170, 12, 12, 255);
                }
                else
                {
                    resFlow.color = new Color32(0, 0, 0, 255);
                }
                resSlider.maxValue = max;
                resSlider.minValue = 0;
                resSlider.value = currentFlow;
                Debug.Log("check 2569");
            }
            else
            {
                Debug.Log("check 2573");
                Text resFlow = resourceTableB.Rows[counter % 6].Cells[1].GetComponentInChildren<Text>();
                Slider resSlider = resourceTableB.Rows[counter % 6].Cells[2].GetComponentInChildren<Slider>();
                resFlow.text = currentFlow + "/" + producing;
                if (max == currentFlow && max < producing)
                {
                    Debug.Log("poop");
                    resFlow.color = new Color32(170, 12, 12, 255);
                }
                else
                {
                    resFlow.color = new Color32(0, 0, 0, 255);
                }
                resSlider.maxValue = max;
                resSlider.minValue = 0;
                resSlider.value = currentFlow;
            }
            counter++;
            Debug.Log("Counter: " + counter);
        }


    }


    private void changeTransportSlider()
    {
        App app = UnityEngine.Object.FindObjectOfType<App>();
        Nation player = State.getNations()[app.GetHumanIndex()];
        int oldFlow = PlayerCalculator.calculateTransportFlow(player);

        // int oldFlow = player.industry.getTransportFlow();
        updateFlow(player);
        int newFlow = PlayerCalculator.calculateTransportFlow(player);
        //If slider was reduced new flow will be less than old flow - return coal to player
        int difference = Math.Abs(oldFlow - newFlow);
        int totalNumberOfProvinces = PlayerCalculator.getTotalNumberOfProvinces(player);

        // In case we reduced the flow
        if (newFlow < oldFlow)
        {
            if (oldFlow <= totalNumberOfProvinces)
            {
                // do nothing
            }
            else if (oldFlow > totalNumberOfProvinces)
            {
                if (newFlow >= totalNumberOfProvinces)
                {
                    player.collectResource(MyEnum.Resources.coal, difference * 0.2f);

                }
                else
                {
                    player.collectResource(MyEnum.Resources.coal, (oldFlow - totalNumberOfProvinces) * 0.2f);

                }
            }
        }
        else
        // The case where we have increased the amount of flow
        {
            if (newFlow <= totalNumberOfProvinces)
            {
                // do nothing
            }
            else if (newFlow > totalNumberOfProvinces)
            {
                // The case where we were already making use of some coal for transport
                if (oldFlow >= totalNumberOfProvinces)
                {
                    player.consumeResource(MyEnum.Resources.coal, difference * 0.2f);
                }
                else
                {
                    player.consumeResource(MyEnum.Resources.coal, (newFlow - player.getProvinces().Count) * 0.2f);

                }
            }
        }
        updateTransportPanel();
    }

    private void updateFlow(Nation player)
    {
        //foreach (MyEnum.Resources res in Enum.GetValues(typeof(MyEnum.Resources)))
        //  {
        //      player.setResTransportFlow(res, 0);
        //  }

        int counter = 0;
        foreach (MyEnum.Resources res in Enum.GetValues(typeof(MyEnum.Resources)))
        {
            if (counter <= 5)
            {
                Slider resSlider = resourceTableA.Rows[counter].Cells[2].GetComponentInChildren<Slider>();
                player.setResTransportFlow(res, (int)resSlider.value);
            }
            else
            {
                Slider resSlider = resourceTableB.Rows[counter % 6].Cells[2].GetComponentInChildren<Slider>();
                player.setResTransportFlow(res, (int)resSlider.value);
            }
            counter++;
            //   Debug.Log(counter);
        }

    }


    private void addTransportCapacity()
    {
        App app = UnityEngine.Object.FindObjectOfType<App>();
        Nation player = State.getNations()[app.GetHumanIndex()];
        player.UseAP(1);
        player.useIP(1);
        player.consumeGoods(MyEnum.Goods.parts, 1);
        player.industry.addTrain();
        player.industry.increaseTransportCapacity(2);
        uiUpdater.updateUI();
        updateTransportPanel();
    }
}
