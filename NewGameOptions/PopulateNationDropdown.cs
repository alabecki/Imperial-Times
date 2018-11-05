using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;


namespace AssemblyCSharp
{


    public class PopulateNationDropdown : MonoBehaviour
    {

        //public static PopulateNationDropdown populator;
        public List<string> nation_choices;

        public Dropdown nationDropdown;
        public Dropdown scenarioDropdown;

        public class ScenarioList
        {
            public List<string> scenarios = new List<string>();
        }


        void Start()
        {
            App app = Object.FindObjectOfType<App>();

            // Dropdown nationDropdown = FindObjectOfType<Dropdown>();
            Debug.Log("Starting Dropdown Value : " + nationDropdown.value);

            List<string> nation_choices = new List<string>()
            { "Aerakrara", "An'rio", "Ditaeler", "Crystalice",
                "Dessak", "Feandra" };
           // foreach (string item in nation_choices)
            //{
              //  Debug.Log(item);
            //}

            Populate(nation_choices, true, 0);
            ScenarioList scenarios = CreateScenarioList();
            app.SetMajorNations(nation_choices);
            Populate(scenarios.scenarios, false, 1);

            
        }

        public void Populate(List<string> item_choices, bool flags, int fuckYou)
        {
            if (flags == true)
            {
                nationDropdown.options.Clear();
                foreach (string item in item_choices)
                {

                    Debug.Log("Adding nation: " + item);

                   // Sprite temp = new Sprite();
                    nationDropdown.options.Add(new Dropdown.OptionData()
                    {
                        text = item,
                        image = Resources.Load("Flags/" + item, typeof(Sprite)) as Sprite
                        //image = Resources.Load<Sprite>(nation) as Sprite,

                    });
                }
            }
           
            
            else
            {
                scenarioDropdown.options.Clear();

                foreach (string item in item_choices)
                {
                    scenarioDropdown.options.Add(new Dropdown.OptionData(item));

                }

            }
          //  Debug.Log("Exiting Populate " + fuckYou);

        }

        public ScenarioList CreateScenarioList()
        {
            Debug.Log("Starting to create Scenario List");
            string listPath = Application.dataPath +
            "/StreamingAssets/Scenarios/ScenarioList.json";
            //  string[] file = Directory.GetFiles(listPath, "*.json");
            //string listRaw = File.ReadAllText(file[0]);
            string dataAsJson = File.ReadAllText(listPath);

            var Scenarios = Newtonsoft.Json.JsonConvert.DeserializeObject<ScenarioList>(dataAsJson);
            return Scenarios;

        }

        void Update()
        {

        }
    }
}