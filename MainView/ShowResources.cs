using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WorldMapStrategyKit;
using assemblyCsharp;

public class ShowResources : MonoBehaviour {
    public Toggle resourceTogle;
    WMSK map;


    public void ToggleProvinceResources()
    {
        map = WMSK.instance;

        App app = UnityEngine.Object.FindObjectOfType<App>();
        Dictionary<int, assemblyCsharp.Province> provinces = State.getProvinces();
        if (State.GetResourceIcons().Count == 0)
        {
            for (int k = 0; k < map.provinces.Length; k++)
            {

                GameObject resourceIcon = Instantiate
                    (Resources.Load<GameObject>("ResourceMap/" + provinces[k].getResource()));
                Vector2 position = map.GetProvince(k).center;
                GameObjectAnimator icon = resourceIcon.WMSK_MoveTo(position, true, 1f);

                //  icon.WMSK_MoveTo(position, true, 1.6f);
                icon.transform.localScale = new Vector3(0.5f, 0.5f, 1.0f);
                // resourceIcon.SetActive(false);
                icon.autoScale = true;
                icon.group = 0;
                //   icon.GetComponent<SpriteRenderer>().enabled = false;
                icon.visible = false;
                State.addResourceIcon(icon);
            }
            map.VGOToggleGroupVisibility(0, false);

        }
        else if (!resourceTogle.isOn)
        {
            map.VGOToggleGroupVisibility(0, false);
        }

        else if(resourceTogle.isOn)
        {
            map.VGOToggleGroupVisibility(0, true);

            // State.GetResourceIcons()[k].GetComponent<SpriteRenderer>().enabled = true;
            // State.GetResourceIcons()[k].SetActive(true);

        }
         
     
        
        // Instantiate the sprite, face it to up and position it into the map
        //goldIcon.transform.localRotation = Quaternion.Euler(90, 180, 180);
        //goldIcon.transform.localScale = Misc.Vector3one * 1f;
        // GameObject star = Instantiate(Resources.Load<GameObject>("Sprites/StarSprite"));
        
    }


    // Use this for initialization
    void Start () {
//map.VGOToggleGroupVisibility(0, false);


    }

    // Update is called once per frame
    void Update () {
		
	}
}
