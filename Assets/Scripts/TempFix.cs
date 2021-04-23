using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.UI;

public class TempFix : MonoBehaviour
{
    public GameObject scroller;
    public MultiplayerEventSystem mes;
    public PlayerInput pi;

    public int playerId;

    // Update is called once per frame
    void FixedUpdate()
    {
        // currently selected object
        GameObject curr = mes.currentSelectedGameObject;

        // is Selection active
        if (transform.Find("Selection").gameObject.activeSelf)
        {
            // scrollbar is not selected, curr->Weapons->Weapon Group Type->Content->Viewport->ScrollView->Player Input #
            if (curr != scroller && !curr.transform.parent.parent.parent.parent.parent.parent.parent.name.Contains(playerId.ToString()))
            {
                mes.SetSelectedGameObject(scroller);
            }
        }
/*        // if Selection not active, curr->(Back/Front) Arm Loadout->Loadouts->Player Input #
        else if (!curr.transform.parent.parent.parent.name.Contains(playerId.ToString()))
        {
            mes.SetSelectedGameObject(scroller);
        }*/
    }
}
