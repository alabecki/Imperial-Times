using System.Collections;
using System.Collections.Generic;
using System.IO;
using UI.ThreeDimensional;
using UnityEngine;
using UnityEngine.UI;

public class GetNationChoice : MonoBehaviour {
    
   
   public List<string> nationList = new List<string>();
    public Dropdown nationsDropdown;
    public Dropdown scenarioDropdown;
    public Text nationDescription;
    public UIObject3D nationFlag;
    public Image previewMap;


    public class NationNames
    {
        public List<string> nations = new List<string>();
    }

    public void recordChosenNation()
    {
        App app = Object.FindObjectOfType<App>();
       Debug.Log(nationsDropdown.value);

        int nationIndex = nationsDropdown.value;
        Debug.Log("Nation index is:" +  nationIndex);
        List<string> majorNations = app.GetMajorNations();
        app.SetHumanNation(majorNations[nationIndex]);
        int choice = scenarioDropdown.value;
        string scenario = scenarioDropdown.options[choice].text;

        string nationListPath = Application.dataPath +
             "/StreamingAssets/Scenarios/" + scenario + "/MajorNationsNames.json";
        string listRaw = System.IO.File.ReadAllText(nationListPath);
        var NationNames = Newtonsoft.Json.JsonConvert.DeserializeObject<NationNames>(listRaw);
        string nationName = NationNames.nations[nationIndex];

        string nationDescriptionPath = Application.dataPath + "/StreamingAssets/Scenarios/" + scenario + "/MajorNationsDescriptions.json";
        string descriptionListRaw = System.IO.File.ReadAllText(nationDescriptionPath);
        var NationDescriptions = Newtonsoft.Json.JsonConvert.DeserializeObject<NationNames>(descriptionListRaw);
        Debug.Log("Nation Index: " + nationIndex);
        nationDescription.text = NationDescriptions.nations[nationIndex];

        previewMap.sprite = Resources.Load("SelectionMaps/" + nationName, typeof(Sprite)) as Sprite;

        GameObject flagPrefab = Instantiate(Resources.Load<GameObject>("Flags/Prefabs/" + nationName));
        nationFlag.ObjectPrefab = flagPrefab.transform;
        nationFlag.RenderScale = 2;
    }
    
    // Use this for initialization
    void Start () {
        //flagPreview.objectProperties.gameObjectToPreview = Instantiate(Resources.Load<GameObject>("Flags/Prefabs/Bambaki"));
      //  flagPreview.Render();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
