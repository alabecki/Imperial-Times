using System.Collections;
using System.Collections.Generic;
using UI.Tables;
using UnityEngine;
using UnityEngine.UI;



public class GameStarter : MonoBehaviour {

    public static List<string> majorNations = new List<string>();
    public ToggleGroup scenario;
    public Dropdown nation;
    public ToggleGroup autoSave;

    public TableLayout resourceTable;
    public TableLayout goodsTable;

    public void primaryFactors() {

    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
