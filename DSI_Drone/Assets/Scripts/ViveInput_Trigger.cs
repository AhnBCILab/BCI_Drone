using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Valve.VR;

public class ViveInput_Trigger : MonoBehaviour
{
    public GameObject ControlPanel;
    public SteamVR_Action_Boolean TriggerAction;
    public SteamVR_Input_Sources inputSource = SteamVR_Input_Sources.Any;

    private void Update()
    {
        if (TriggerAction.GetLastStateDown(inputSource))
        {
            Debug.Log("Trigger pressed");
            if (!ControlPanel.activeSelf)
            {
                Move_with_ControlPanel_Trigger.ControlPanel_On_Off = 1;
                Move_with_ControlPanel_Trigger.outpNum = 0;
                ControlPanel.SetActive(true);
            }

        }
    }

}
