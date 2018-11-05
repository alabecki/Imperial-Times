using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WorldMapStrategyKit;

public class GenericSoldierController : MonoBehaviour {

    WMSK map;
    GameObjectAnimator army;
    //GameObject earlyArmy;
    Ray ray;
    RaycastHit hit;

    Animator animator;
    private const string key_isWalking = "IsWalking";
    private const string key_isIdle = "IsIdle";
   // private const string key_weaponDrawn = "weaponDrawn";
    private const string key_is_attacking = "IsAttacking";
    


   // int drawWeapon = Animator.StringToHash("m_weapon_draw");
    int walk = Animator.StringToHash("m_weapon_walk_rm");
    int shootBurst = Animator.StringToHash("m_weapon_shoot_burst");
    int defendHash = Animator.StringToHash("m_weapon_damage");
    int idle = Animator.StringToHash("m_weapon_idle_C");
    bool selected = false;
    bool wait = false;
    int provinceClicked;
    bool flag = false;
  

    // Use this for initialization
    void Start() {
        animator = GetComponent<Animator>();
        map = WMSK.instance;
        army = GetComponent<GameObjectAnimator>();


        this.animator.SetBool(key_isWalking, false);
        this.animator.SetBool(key_is_attacking, false);
        // this.animator.SetBool(key_weaponDrawn, false);
        this.animator.SetBool(key_isIdle, true);
        // animator.Play(idle);
        map.pathFindingEnableCustomRouteMatrix = true;

        /* map.OnVGOPointerEnter = delegate (GameObjectAnimator army) {
             Debug.Log("GLOBAL EVENT: Mouse entered " + army.name);
             if (army.GetComponent<Animator>() != null)
         };


         map.OnVGOPointerExit = delegate (GameObjectAnimator army)
         {
             Debug.Log("GLOBAL EVENT: Mouse exited " + army.name);
             if (army.GetComponentInChildren<Renderer>().material.color == Color.yellow)
             {
                 if (army.GetComponent<Animator>() != null)
                     makeIdle(army);
             }
         }; */

        map.OnClick += (float x, float y, int buttonIndex) => {
            flag = true;
            if (selected == false)
            {
                return;
            }
            if (provinceClicked == map.provinceLastClicked)
            {
                //   wait = false;
                return;
            }
            Debug.Log("Name" + army.name);
           // Debug.Log("selected " + selectedName);


            if (buttonIndex == 1)
            {
                Debug.Log("Righ Click on Destination");


                //  makeWalk(army);
                //  System.Threading.Thread.Sleep(500);
                //  Invoke("MoveUnitWithPathFinding", 4);

                MoveUnitWithPathFinding(new Vector2(x, y));
            }
        };

        map.OnVGOCountryEnter += makeAttack;
        map.OnVGOMoveEnd += makeIdle;
        map.OnVGOMoveStart += makeWalk;

        map.OnVGOPointerDown += selectArmy;
    }

    void Update()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            if (Input.GetMouseButtonDown(0))
            {
                print(hit.collider.name);
                Collider col = hit.collider;
                animator = col.gameObject.GetComponent<Animator>();
            }
                
        }
    }



    private void selectArmy(GameObjectAnimator clickedArmy) { 
            army = clickedArmy;
          //  animator = army.gameObject.GetComponent<Animator>();
            Debug.Log("GLOBAL EVENT: Left button pressed on " + army.name);
          
           // ArmyMouseDown(army);
            int provIndex = map.provinceLastClicked;
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

        
    }






    void makeWalk(GameObjectAnimator army)
    {
        Debug.Log("Name: " + army.name);
    
        this.animator.SetBool(key_isIdle, false);

        army.autoRotation = true;
        // army.preserveOriginalRotation = false;
        // army.GetComponent<Animator>().SetBool(key_isRun, true);
      //  this.animator.SetBool(key_weaponDrawn, true);
       // army.GetComponent<Animator>().SetBool(key_weaponDrawn, true);

       // animator.Play(drawWeapon);
       //System.Threading.Thread.Sleep(400);
        this.animator.SetBool(key_isWalking, true);
        this.animator.SetBool(key_isWalking, false);
        //   army.GetComponent<Animator>().SetBool(key_weaponDrawn, false);
        //   army.GetComponent<Animator>().SetBool(key_isWalking, true);
        // animator.Play(walk);
        //this.animator.SetBool(key_weaponDrawn, false);
        animator.Play(walk);


    }

    void MoveUnitWithPathFinding(Vector2 destination)
    {

        army = map.VGOLastClicked;
        bool canMove = false;
        Debug.Log("Are we going to move now?");
        animator.SetBool(key_isWalking, true);
       // animator.Play(walk);

        // army.GetComponent<Animator>().SetBool(key_isRun, true);
        canMove = army.MoveTo(destination, 0.33f);
        Debug.Log(canMove);
        selected = false;
        if (!canMove)
        {
            Debug.Log("Can't move to destination!");
        }
       // animator.Play(walk);

    }

    void makeAttack(GameObjectAnimator earlyArmy)
    {
        if (flag == true)
        {
            Vector2 armyLocation = earlyArmy.currentMap2DLocation;
            earlyArmy.autoRotation = false;
            earlyArmy.preserveOriginalRotation = true;
            this.animator.SetBool(key_is_attacking, true);
            this.animator.SetBool(key_isWalking, false);
          //  animator.Play(shootBurst);
        }
    }

    void makeIdle(GameObjectAnimator earlyArmy)
    {
        animator.StopPlayback();

      //  Debug.Log("Why do you suck so much?");
        earlyArmy.autoRotation = true;
        earlyArmy.preserveOriginalRotation = true;
        this.animator.SetBool(key_is_attacking, false);
        this.animator.SetBool(key_isWalking, false);
        this.animator.SetBool(key_is_attacking, false);
        this.animator.SetBool(key_isIdle, true);
       
        this.animator.StopPlayback();
        animator.Play(idle);

    }



    IEnumerator Wait(float duration)
    {
        yield return new WaitForSeconds(duration);
        Debug.Log("After Waiting 2 Seconds");
    }
}
