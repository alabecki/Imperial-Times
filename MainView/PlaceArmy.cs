using assemblyCsharp;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WorldMapStrategyKit;

public class PlaceArmy : MonoBehaviour {



    WMSK map;
    GameObjectAnimator army;  


    GameObjectAnimator placeArmy(Vector2 mapPosition)
    {
        //   GameObject tankGO = Instantiate(Resources.Load<GameObject>("Prefabs/Army"));
        // GameObject tankGO = Instantiate(Resources.Load<GameObject>("Tank/CompleteTank"));
        // GameObject tankGO = Instantiate(Resources.Load<GameObject>
        // ("SpartanKing/SpartanKing2"));
        GameObject tankGO = Instantiate(Resources.Load<GameObject>
                    ("ToonyTinyPeople/TT_ww1/EarlyEraArmy"));
        //("SD_Character/Character/4Hero/Prefabs/Chara_4Hero 1"));
        SkinnedMeshRenderer[] smrs = tankGO.GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach (SkinnedMeshRenderer smr in smrs)
        {
            if (smr.name.StartsWith("flagcl"))
            {
                Debug.Log("Does this happen?");
                smr.material = Resources.Load("Flags/Materials/" + State.getNations()[4].getNationName(), typeof(Material)) as Material;
            }
        }
        GameObjectAnimator tank = tankGO.WMSK_MoveTo(mapPosition);
        //tank.autoRotation = true;
        tank.preserveOriginalRotation = true;
        tank.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        tank.terrainCapability = TERRAIN_CAPABILITY.OnlyGround;
        tank.autoScale = true;
        tank.group = 1;
        tank.transform.localRotation= Quaternion.Euler(-30f, 180f, 0f);
        tank.BlocksRayCast = true;
       // tank.transform.rotation = Quaternion.Euler(-45.0f, 0f, -45.0f);
        //tank.transform.Rotate(0, -45, 0, Space.Self);
        // Set tank ownership
        tank.attrib["player"] = 1;
        return tank;

    }

    // Use this for initialization
    void Start () {
       // map = WMSK.instance;

       // WorldMapStrategyKit.Province prov = map.GetProvince(45);
       // Vector2 position = prov.center;
       // army = placeArmy(position);
       // army.name = "first unit";
       // army.GetComponent<ArmyController>().army = army;

    }

    
	public void placeArmyOnMap(int provIndex, Nation nation, int id)
    {

        map = WMSK.instance;
        GameObject armyPrefabInstance = Instantiate(Resources.Load<GameObject>
             ("ToonyTinyPeople/TT_ww1/EarlyEraArmy"));
        WorldMapStrategyKit.Province prov = map.GetProvince(provIndex);
        if(nation.getType() == MyEnum.NationType.major || nation.getType() == MyEnum.NationType.minor)
        {
            if (State.GerEra() == MyEnum.Era.Early)
            {
                armyPrefabInstance = Instantiate(Resources.Load<GameObject>
                // ("ToonyTinyPeople/TT_ww1/EarlyEraArmy"));
                ("ToonyTinyPeople/TT_ww1/EarlyEraArmy"));

            }
        }
        else
        {
            {
                armyPrefabInstance = Instantiate(Resources.Load<GameObject>
                 ("Model_Warrior/barbOne"));
            }
        }

        Vector2 position = prov.center;
        GameObjectAnimator army = armyPrefabInstance.WMSK_MoveTo(position);
        army.gameObject.AddComponent<GenericSoldierController>();
        army.name = id.ToString();



            SkinnedMeshRenderer[] smrs = armyPrefabInstance.GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach (SkinnedMeshRenderer smr in smrs)
        {
            if (smr.name.StartsWith("flagcl"))
            {
                Debug.Log("Army of " +  nation.getNationName());
                smr.material = Resources.Load("Flags/Materials/" + nation.getNationName(), typeof(Material)) as Material;
            }
        }
        if (nation.getType() == MyEnum.NationType.major || nation.getType() == MyEnum.NationType.minor)
        {
            army.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

        }
        else
        {
            army.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);

        }
        army.preserveOriginalRotation = true;
        army.terrainCapability = TERRAIN_CAPABILITY.OnlyGround;
        army.autoScale = true;
        army.group = 1;
        army.transform.localRotation = Quaternion.Euler(-30f, 180f, 0f);
        army.BlocksRayCast = true;

        army.attrib["owner"] = nation.getIndex();
        army.attrib["id"] = id;
        
    }


      /* if (type == MyEnum.ArmyUnits.infantry)
         {
            newArmy.attrib["infantry"] = 1;
          }
         if(type == MyEnum.ArmyUnits.artillery)
         {
            newArmy.attrib["artillery"] = 1;
        }
        if (type == MyEnum.ArmyUnits.cavalry)
        {
            newArmy.attrib["cavalry"] = 1;
        }
        if (type == MyEnum.ArmyUnits.cavalry)
        {
            newArmy.attrib["cavalry"] = 1;
        }
        if (type == MyEnum.ArmyUnits.fighter)
        {
            newArmy.attrib["fighter"] = 1;
        }
        if (type == MyEnum.ArmyUnits.tank)
        {
            newArmy.attrib["tank"] = 1;
        }
         */
}
