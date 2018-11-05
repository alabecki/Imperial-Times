using System.Collections;
using System.Collections.Generic;
using UI.Tables;
using UnityEngine;
using assemblyCsharp;
using UnityEngine.UI;

public class TarrifView : MonoBehaviour {

    public TableLayout tariffTable;
    public TableRow tarrifRow;
    public RectTransform scrollviewContent;


    public void CreateTariffTable()
    {
        App app = UnityEngine.Object.FindObjectOfType<App>();
        Dictionary<int, Nation> nations = State.getNations();
        for (int i = 0; i < nations.Count; i++)
        {
             var newRow = Instantiate<TableRow>(tarrifRow);
            //var fieldGameObject = new GameObject("Field", typeof(RectTransform));

            newRow.gameObject.SetActive(true);
            newRow.preferredHeight = 30;
                            var textGameObject = new GameObject("Text", typeof(RectTransform));                

            tariffTable.AddRow(newRow);
            scrollviewContent.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (tariffTable.transform as RectTransform).rect.height);
            List<TableCell> cells = newRow.Cells;
            Text nationName = cells[0].GetComponent<Text>();
            nationName.text = nations[i].getNationName();
            for (int j = 1; j < cells.Count; j++)
            {
                InputField tarrif = cells[j].GetComponent<InputField>();
                tarrif.text = "0";

            }
        }

    }



	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
