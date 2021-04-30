using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;

public class TempFix : MonoBehaviour
{
    public GameObject scroller;
    public MultiplayerEventSystem mes;
    public PlayerInput pi;

    public int playerId;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (mes == null)
        {
            mes = SceneManager.GetActiveScene().GetRootGameObjects()[2].GetComponent<MultiplayerEventSystem>();
        }

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
        // Selection not active
/*        else
        {
            pi.devices[0]
        }*/

    }
}
