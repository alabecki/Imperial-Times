using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using assemblyCsharp;
using UI.ThreeDimensional;
using TMPro;
using EasyUIAnimator;

public class AssignHeaderValues : MonoBehaviour {

    public GameObject mainHeader;

    public Image headerFlag;
    public TextMeshProUGUI nationName;
   
    public Text turn;
    public UIObject3D flag;
    private bool flagFlag = false;
    bool header_Flag = false;
    private UIAnimation headerEnter;
    private UIAnimation headerExit;
    // public Toggle headerSwitcher;

 

    // Use this for initialization
    void Start () {
        assignHeaderValues();
     //   headerSwitcher.onValueChanged.AddListener(delegate { toggleHeader(); });

        //   mainHeader.SetActive(false);

        RectTransform marketRect = mainHeader.GetComponent<RectTransform>();

        headerEnter = UIAnimator.Move(marketRect, new Vector2(0.5f, 1f), new Vector2(0.5f, 0.93f), 1f).SetModifier(Modifier.Linear); ;
        headerExit = UIAnimator.Move(marketRect, new Vector2(0.5f, 0.93f), new Vector2(0.5f, 1f), 1f).SetModifier(Modifier.Linear); ;

    }

    private void toggleHeader()
    {
        if (mainHeader.activeSelf == false || header_Flag == false)
        {
            assignHeaderValues();

            mainHeader.SetActive(true);
            headerEnter.Play();
            header_Flag = true;
            return;
        }
        else
        {
            if (header_Flag == true)
            {
                headerExit.Play();
                header_Flag = false;
                //  inventoryPanel.SetActive(false);
            }
        }
    }

    public void assignHeaderValues()
    {
        App app = UnityEngine.Object.FindObjectOfType<App>();
        Debug.Log("HUman Index is " + app.GetHumanIndex());
        Nation player = State.getNations()[app.GetHumanIndex()];
        //  headerFlag.sprite = Resources.Load
        //    ("Flags/" + player.getNationName(), typeof(Sprite)) as Sprite;
        nationName.text = player.getNationName();
        //APValue.text = player.getAP().ToString();
        //goldValue.text = player.getGold().ToString();
        //displayStabilityFace();
        //urbanPOP.text = player.getUrbanPOP().ToString();
        turn.text = State.turn.ToString();
        if (flagFlag == false)
        {

            GameObject threeDObject = Instantiate(Resources.Load<GameObject>("Flags/Prefabs/" + player.getNationName()));
            flag.RefreshTarget();

            flag.ObjectPrefab = threeDObject.transform;
            flagFlag = true;
            flag.RenderScale = 0;
            flag.ObjectPrefab = threeDObject.transform;
            flag.RefreshTarget();

            // RenderTask renderTask = new RenderTask(renderTexture, gameObjectToPreview);
            //ObjectPreviewRenderer.current.RenderPreview(renderTask);

        }
    }

    // Update is called once per frame
    void Update () {

    }

   /* private void displayStabilityFace()
    {

        App app = UnityEngine.Object.FindObjectOfType<App>();
        int playerIndex = app.GetHumanIndex();
        Nation player = State.getNations()[playerIndex];

        float nationStab = player.Stability;

        if (nationStab < -2.6)
        {
            stability.sprite = Resources.Load("Sprites/Stability/-3", typeof(Sprite)) as Sprite;
        }
        else if (nationStab < -1.6)
        {
            stability.sprite = Resources.Load("Sprites/Stability/-2", typeof(Sprite)) as Sprite;
        }
        else if (nationStab < -0.6)
        {
            stability.sprite = Resources.Load("Sprites/Stability/-1", typeof(Sprite)) as Sprite;
        }
        else if (nationStab < 0.45)
        {
            stability.sprite = Resources.Load("Sprites/Stability/0", typeof(Sprite)) as Sprite;
        }
        else if (nationStab < 1.45)
        {
            stability.sprite = Resources.Load("Sprites/Stability/1", typeof(Sprite)) as Sprite;
        }
        else if (nationStab < 2.45)
        {
            stability.sprite = Resources.Load("Sprites/Stability/2", typeof(Sprite)) as Sprite;
        }
        else
        {
            stability.sprite = Resources.Load("Sprites/Stability/3", typeof(Sprite)) as Sprite;
        }
    } */
}
