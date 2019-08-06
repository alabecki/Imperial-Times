using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class GetScenarioChoice : MonoBehaviour {

    public Dropdown nationDropdown;
    public Dropdown scenarioDropdown;
    private List<string> scenarioNames = new List<string>();

  

    public class NationNames
    {
        public List<string> nations = new List<string>();
    }

    public void  getScenarioChoice()
    {
        int choice = scenarioDropdown.value;
        string scenario = scenarioDropdown.options[choice].text;

        App app = Object.FindObjectOfType<App>();

        app.SetScenario(scenario);
        Debug.Log("Choice: " + choice);
        Debug.Log("opts: " + scenarioDropdown.options);
        List<Dropdown.OptionData> options = scenarioDropdown.options;
        List<string> scenarioNames = new List<string>();
        foreach (Dropdown.OptionData option in options)
        {
            scenarioNames.Add(option.text);
        }
        Debug.Log("nameA: " + scenarioNames[0]);

        string scenarioName = scenarioNames[choice];
        Debug.Log("Scenario Name: "  + scenarioName);
        string nationListPath = Application.dataPath +
            "/StreamingAssets/Scenarios/" + scenarioName + "/MajorNationsNames.json";

        FileStream stream = File.OpenRead(nationListPath);

        string listRaw = System.IO.File.ReadAllText(nationListPath);

        var NationNames = Newtonsoft.Json.JsonConvert.DeserializeObject<NationNames>(listRaw);   
        Populate(nationDropdown, NationNames.nations, true);

    }

    public void Populate(Dropdown dropdown, List<string> item_choices, bool flags)
    {
        dropdown.options.Clear();
        Debug.Log("Number of Major Nations in Scenario: " + item_choices.Count);
        foreach (string text in item_choices)
        {
            //Sprite temp = new Sprite();
            Debug.Log(text);
            dropdown.options.Add(new Dropdown.OptionData()
            {
                text = text,        
                image = Resources.Load("Flags/" + text, typeof(Sprite)) as Sprite

            });

            Sprite labelSprite = Resources.Load("Flags/" + item_choices[0], typeof(Sprite)) as Sprite;
            Canvas.ForceUpdateCanvases();
        }
    }


 
}
