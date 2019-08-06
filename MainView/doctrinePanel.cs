using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UI.Tables;
using assemblyCsharp;
using System;
using UnityEngine.EventSystems;

public class doctrinePanel: MonoBehaviour 
{

    public GameObject doctrineTab;
    public TableLayout doctrineTable;
    public Button openDoctrineTabButton;
    public UI_Updater uiUpdater;



    private void Start()
    {
        doctrineTab.SetActive(false);
        openDoctrineTabButton.onClick.AddListener(delegate { openDoctrineTab(); });

        App app = UnityEngine.Object.FindObjectOfType<App>();
        int playerIndex = app.GetHumanIndex();
        Nation player = State.getNations()[playerIndex];
        int rowIndex = 0;
        LandForces landForces = player.landForces;

        foreach (MyEnum.ArmyDoctrines doctrine in Enum.GetValues(typeof(MyEnum.ArmyDoctrines)))
        {
            TableRow row = doctrineTable.Rows[rowIndex];

            Button addButton = row.GetComponentInChildren<Button>();
            addButton.onClick.AddListener(delegate { addDoctrine(); });
            if (landForces.hasDoctrine(doctrine))
            {
                Toggle toggle = row.GetComponentInChildren<Toggle>();
                toggle.isOn = true;
            }
            rowIndex++;
        }
    }

    private void openDoctrineTab()
    {
        App app = UnityEngine.Object.FindObjectOfType<App>();
        int playerIndex = app.GetHumanIndex();
        Nation player = State.getNations()[playerIndex];
        LandForces landForces = player.landForces;
        int rowIndex = 0;

        foreach (MyEnum.ArmyDoctrines doctrine in Enum.GetValues(typeof(MyEnum.ArmyDoctrines)))
        {
            TableRow row = doctrineTable.Rows[rowIndex];

            Button addButton = row.GetComponentInChildren<Button>();
            if (landForces.hasDoctrine(doctrine))
            {
                Toggle toggle = row.GetComponentInChildren<Toggle>();
                toggle.isOn = true;
                addButton.interactable = false;
            }
            rowIndex++;
        }
        doctrineTab.SetActive(true);
    }


    private void addDoctrine()
    {
        App app = UnityEngine.Object.FindObjectOfType<App>();
        int playerIndex = app.GetHumanIndex();
        Nation player = State.getNations()[playerIndex];
        string name = EventSystem.current.currentSelectedGameObject.name;
        Debug.Log(name);
        MyEnum.ArmyDoctrines newDoctrine = (MyEnum.ArmyDoctrines)Enum.Parse(typeof(MyEnum.ArmyDoctrines), name);
        Debug.Log(newDoctrine);
        player.landForces.addDoctrine(newDoctrine);
        uiUpdater.updateUI();
    }






}
