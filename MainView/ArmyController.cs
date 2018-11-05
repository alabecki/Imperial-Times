using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WorldMapStrategyKit;
using assemblyCsharp;



public class ArmyController : MonoBehaviour {

    WMSK map;
   // GameObject army;
    public GameObjectAnimator army { get; set; }
    private Animator animator;
    private const string key_isRun = "m_weapon_walk_rm";
    private const string key_isIdle = "m_weapon_draw";

    int drawWeapon = Animator.StringToHash("m_weapon_draw");
    int walk = Animator.StringToHash("m_weapon_walk_rm");
    int shootBurst = Animator.StringToHash("m_weapon_shoot_burst");
    int defendHash = Animator.StringToHash("m_weapon_damage");


    bool selected = false;
    bool wait = false;
    int provinceClicked;

    int id;



  //  public int id;

	// Use this for initialization
	void Start () {


        map = WMSK.instance;



        map.OnClick += (float x, float y, int buttonIndex) => {
            if (selected == false)
            {
                return;
            }
            if (provinceClicked == map.provinceLastClicked)
            {
             //   wait = false;
                return;
            }
            else if (buttonIndex == 1)
            {
                Debug.Log("Righ Click on Destination");
                // army.autoRotation = true;
                // army.preserveOriginalRotation = false;
                // army.GetComponent<Animator>().SetBool(key_isRun, true);
                makeWalk(army);
                System.Threading.Thread.Sleep(350);
                MoveTankWithPathFinding(new Vector2(x, y));
            }
        };

       map.OnVGOMoveEnd += makeIdle;
      // map.OnVGOMoveStart += makeWalk;
     

        map.pathFindingEnableCustomRouteMatrix = true;

     //   Vector2 position = map.GetProvince("Jansera", "Dallael").center;

     //   army = PlaceArmy(position);
       // army.name = "first unit";

        map.OnVGOPointerDown = delegate (GameObjectAnimator army) {
            Debug.Log("GLOBAL EVENT: Left button pressed on " + army.name);
            ArmyMouseDown(army);
           id = army.uniqueId;
           // int provIndex = map.provinceLastClicked;
           ///// Debug.Log(provIndex);
          //  assemblyCsharp.Province province = State.getProvinces()[provIndex];
          //  province.SetRayBlocked(true);
          //  IEnumerator wait = Wait(2f);
            if (selected == false)
            {
                selected = true;

            }
            else
            {
                selected = false;
            }
            
        };

        map.OnVGOPointerEnter = delegate (GameObjectAnimator army) {
            Debug.Log("GLOBAL EVENT: Mouse entered " + army.name);
            if (army.GetComponent<Animator>() != null)
            ArmyHover(army);
           
        };

        map.OnVGOPointerExit = delegate (GameObjectAnimator army)
        {
            Debug.Log("GLOBAL EVENT: Mouse exited " + army.name);
            if (army.GetComponentInChildren<Renderer>().material.color == Color.yellow)
            {
                if (army.GetComponent<Animator>() != null)
                    RestoreArmyColor(army);

            }
        };


       // UpdatePathFindingMatrixCost();


    }

    void makeIdle(GameObjectAnimator army)
    {
        army.autoRotation = false;
        army.preserveOriginalRotation = true;
        army.GetComponent<Animator>().SetBool(key_isIdle, true);

    }

    void makeWalk(GameObjectAnimator army)
    {
            army.autoRotation = true;
           // army.preserveOriginalRotation = false;
            army.GetComponent<Animator>().SetBool(key_isRun, true);
    }

    GameObjectAnimator PlaceArmy(Vector2 mapPosition)
    {
        //   GameObject tankGO = Instantiate(Resources.Load<GameObject>("Prefabs/Army"));
        // GameObject tankGO = Instantiate(Resources.Load<GameObject>("Tank/CompleteTank"));
         // GameObject tankGO = Instantiate(Resources.Load<GameObject>
           // ("SpartanKing/SpartanKing2"));
        GameObject tankGO = Instantiate(Resources.Load<GameObject>
               ("SD_Character/Character/4Hero/Prefabs/Chara_4Hero"));

        GameObjectAnimator tank = tankGO.WMSK_MoveTo(mapPosition);
        tank.autoRotation = true;
      
        tank.autoScale = true;
        tank.transform.localScale = new Vector3(1, 1, 1);
        tank.terrainCapability = TERRAIN_CAPABILITY.OnlyGround;
        // Set tank ownership
        tank.attrib["player"] = 1;
        float degrees = 45;
        Vector3 to = new Vector3(degrees, 0, 0);

        tank.transform.localRotation = Quaternion.Euler(-45.0f, -45.0f, -45.0f);
        return tank;

    }




    void ArmyMouseDown(GameObjectAnimator army)
    {
        // Changes tank color to white
       Renderer renderer = army.GetComponentInChildren<Renderer>();
       renderer.sharedMaterial.color = Color.white;
       // Sprite newSprite = Resources.Load<Sprite>("/Sprites/AustrianInfantry1.png");
       // obj.GetComponent<SpriteRenderer>().sprite = newSprite;
    }

    void ArmyHover(GameObjectAnimator army)
    {
        // Changes tank color - but first we store original color inside its attribute bag
         Renderer renderer = army.GetComponentInChildren<Renderer>();
         army.attrib["color"] = renderer.sharedMaterial.color;
        // army.GetComponent<Animator>().SetTrigger("MakeAttack");
      // army.GetComponent<Animator>().SetBool(key_isAttack01, true);

        //Sprite newSprite = Resources.Load<Sprite>("/Sprites/AustrianInfantry1.png");
        //obj.GetComponent<SpriteRenderer>().color = Color.white;
        renderer.material.color = Color.yellow; // notice how I use material and not sharedmaterial - this is to prevent affecting all clone instances - we just want to color this one, so we need to make this material unique.
    }


    void RestoreArmyColor(GameObjectAnimator army)
    {
        // Restores original tank color
         Renderer renderer = army.GetComponentInChildren<Renderer>();
         Color tankColor = army.attrib["color"];  // get back the original color from attribute bag
        renderer.sharedMaterial.color = tankColor;
        // army.GetComponent<Animator>().SetTrigger("MakeIdle");
      //  army.GetComponent<Animator>().SetBool(key_isRun, false);
        //army.GetComponent<Animator>().SetBool(key_isAttack01, false);

        // obj.GetComponentInChildren<SpriteRenderer>().color = Color.blue;

    }

    void MoveTankWithPathFinding(Vector2 destination)
    {
     //   army.autoRotation = true;
      //  army.preserveOriginalRotation = false;

        bool canMove = false;
        Debug.Log("Are we going to move now?");
       // army.GetComponent<Animator>().SetTrigger("MakeWalk");
       // army.GetComponent<Animator>().SetBool(key_isRun, true);

        canMove = army.MoveTo(destination, 0.2f);
        selected = false;
        RestoreArmyColor(army);

        if (!canMove)
        {
            Debug.Log("Can't move to destination!");
        }
    }

    IEnumerator Wait(float duration)
    {
        yield return new WaitForSeconds(duration);
        Debug.Log("After Waiting 2 Seconds");
    }




    // Update is called once per frame
    void Update () {
		
	}

   
}
