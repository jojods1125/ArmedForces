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

    // Update is called once per frame
    void Update()
    {
        if (Gamepad.current[GamepadButton.RightTrigger].isPressed && Gamepad.current[GamepadButton.LeftTrigger].isPressed)
        {
            mes.SetSelectedGameObject(scroller);
        }
    }
}
