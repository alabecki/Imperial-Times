using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetNationChoice : MonoBehaviour {
    
   
   public List<string> nationList = new List<string>();
    public Dropdown nationsDropdown;

    public void recordChosenNation()
    {
        App app = Object.FindObjectOfType<App>();
       Debug.Log(nationsDropdown.value);

        int nationIndex = nationsDropdown.value;
        Debug.Log("Nation index is:" +  nationIndex);
        List<string> majorNations = app.GetMajorNations();
       
        app.SetHumanNation(majorNations[nationIndex]);
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
