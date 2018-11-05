using assemblyCsharp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UI.Tables;
using UnityEngine;
using UnityEngine.UI;

public class NationListDynamic : MonoBehaviour {

    public TableLayout nationList;
    public TableRow nationRow;
    public RectTransform scrollviewContent;
    public Button selectByFlag;
    public Button selectByName;

    public bool dontUseTableCellBackground = true;

    // Use this for initialization
    void Start () {

        CreateNationList();


    }

    // Update is called once per frame
    void Update () {
		
	}


    private void orderTableByColumValue(TableLayout table, int colNum)
    {
        TableLayout tempTable = table;
        Dictionary<int, float> rowsToColValues = new Dictionary<int, float>();
        for(int i = 0; i < table.Rows.Count; i++)
        {
            TableRow currentRow = table.Rows[i];
            string stringValue = currentRow.Cells[colNum].GetComponentInChildren<Text>().text;
            float  floatValue = float.Parse(stringValue, CultureInfo.InvariantCulture.NumberFormat);
          //  int intValue = (int)Math.Floor(floatValue);
            rowsToColValues[i] = floatValue;
        }

        Dictionary<int, int> rowMapper = new Dictionary<int, int>();
        var ordered = rowsToColValues.OrderBy(x => x.Value);
        int index = 0;
        foreach (var item in ordered)
        {
            rowMapper[index] = item.Key;
            index++;
        }
        table.ClearRows();
        for(int i = 0; i < tempTable.Rows.Count; i++)
        {
            table.Rows[i] = tempTable.Rows[rowMapper[i]];
        }
        // Destroy(tempTable);
        table.CalculateLayoutInputVertical();
        table.UpdateLayout();
    }

    private void CreateNationList()
    {

        
        App app = UnityEngine.Object.FindObjectOfType<App>();
        int playerIndex = app.GetHumanIndex();
        Nation player = State.getNations()[playerIndex];
        nationList.ClearRows();
        foreach(Nation nation in State.getNations().Values)
        {
            Debug.Log(nation.getType());
            if (nation.getType() == MyEnum.NationType.oldMinor || nation.getIndex() == player.getIndex())
            {
                continue;
            }
            TableRow newRow = Instantiate<TableRow>(nationRow);
            newRow.gameObject.SetActive(true);
            newRow.preferredHeight = 30;
            newRow.name = nation.getIndex().ToString();
            Debug.Log("Row Name:" + newRow.name);
            nationList.AddRow(newRow);
            scrollviewContent.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (nationList.transform as RectTransform).rect.height);

            Transform res = newRow.Cells[0].transform.GetChild(0);
            Image resImg = res.GetComponent<Image>();
            resImg.preserveAspect = true;
            resImg.sprite = Resources.Load("Flags/" + nation.getNationName().ToString(), typeof(Sprite)) as Sprite;
            //newRow.Cells[0].GetComponentInChildren<Image>().sprite = Resources.Load("Resource/" + resource.ToString(), typeof(Sprite)) as Sprite;
            newRow.Cells[1].GetComponentInChildren<Text>().text = nation.getNationName().ToString();

            if (nation.getType() == MyEnum.NationType.major)
            {

            }
            if (nation.getType() == MyEnum.NationType.minor)
            {

            }
            if (nation.getType() == MyEnum.NationType.oldEmpire)
            {

            }

        }
        scrollviewContent.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (nationList.transform as RectTransform).rect.height);


    }
}
