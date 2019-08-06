using System.Collections;
using System.Collections.Generic;
using System.IO;
using UI.ThreeDimensional;
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

       public UIObject3D nationFlag;
        public Text description;
        public Transform initialFlag;

        public class ScenarioList
        {
            public List<string> scenarios = new List<string>();
        }

        public class NationNames
        {
            public List<string> nations = new List<string>();
        }   



        void Start()
        {
            App app = Object.FindObjectOfType<App>();

            // Dropdown nationDropdown = FindObjectOfType<Dropdown>();
            Debug.Log("Starting Dropdown Value : " + nationDropdown.value);

            List<string> nation_choices = new List<string>()
            // { "Aerakrara", "An'rio", "Arozus", "Crystalice",
            //     "Dessak", "Feandra" };
            { "Bambaki",
                "Boreois",
                "Chaldea",
                "Sidero",
                "Sitari",
                "Wyvermount" };
            // foreach (string item in nation_choices)
            //{
            //  Debug.Log(item);
            //}
            //Aerakara 19
            //An'rio  17
            //Ditaeler 14 but not a major
            //Crystalice 16
            //Dessak 22
            //Feandra 21

            Populate(nation_choices, true, 0);
            ScenarioList scenarios = CreateScenarioList();
            app.SetMajorNations(nation_choices);
            Populate(scenarios.scenarios, false, 1);

            // Debug.Log("startNewGameScene");

       // GameObject flagPrefab = Instantiate(Resources.Load<GameObject>("Flags/Prefabs/" + nationName));
       GameObject flagPrefab = Instantiate(Resources.Load<GameObject>("Flags/Prefabs/Bambaki"));

            nationFlag.ObjectPrefab = flagPrefab.transform;
            //nationFlag.ObjectPrefab = flagPrefab.transform;
       nationFlag.UpdateDisplay();
           // nationFlag.ObjectPrefab = initialFlag;


            string nationDescriptionPath = Application.dataPath + "/StreamingAssets/Scenarios/Fictional/MajorNationsDescriptions.json";
        string descriptionListRaw = System.IO.File.ReadAllText(nationDescriptionPath);
        var NationDescriptions = Newtonsoft.Json.JsonConvert.DeserializeObject<NationNames>(descriptionListRaw);
        description.text = NationDescriptions.nations[0];
            
        }

        public void Populate(List<string> item_choices, bool flags, int fuckYou)
        {
            if (flags == true)
            {
                nationDropdown.options.Clear();
                foreach (string item in item_choices)
                {
                    nationDropdown.options.Add(new Dropdown.OptionData()
                    {
                        text = item,
                       
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

        }

        public ScenarioList CreateScenarioList()
        {
            Debug.Log("Starting to create Scenario List");
            string listPath = Application.streamingAssetsPath + "/Scenarios/ScenarioList.json";
            string dataAsJson = File.ReadAllText(listPath);
            var Scenarios = Newtonsoft.Json.JsonConvert.DeserializeObject<ScenarioList>(dataAsJson);
            return Scenarios;
        }

        void Update()
        {

        }
    }
}