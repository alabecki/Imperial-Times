using assemblyCsharp;
using ModelShark;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class DT_ToolTipController : MonoBehaviour
{

    TooltipTrigger toolTipTrigger; 
    // Start is called before the first frame update
    void Start()
    {
        toolTipTrigger = this.GetComponent<TooltipTrigger>();

    }

    public void controlToolTip()
    {
        toolTipTrigger = this.GetComponent<TooltipTrigger>();

        Debug.Log("Mouse Over Here");
        Debug.Log(toolTipTrigger.name);

        App app = UnityEngine.Object.FindObjectOfType<App>();
        int playerIndex = app.GetHumanIndex();
        Nation player = State.getNations()[playerIndex];
        if (PlayerCalculator.canAddDP(player))
        {

            toolTipTrigger.SetText("BodyText", "Press to gain more Development Points (DP)");
        }
        else
        {

            string message = "You requre ";
     
            if(player.getNumberResource(MyEnum.Resources.spice) < 1)
            {
                message += " 1 more spice ";

            }
            if (player.getNumberGood(MyEnum.Goods.paper) < 1)
            {
               
                    message += " 1 more paper ";
            }

            if(player.getNumberGood(MyEnum.Goods.furniture) < 1)
            {
                message += " 1 more furniture ";

            }
            // StringBuilder requirements = new StringBuilder();

            message += ".";

            toolTipTrigger.SetText("BodyText", message);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
