using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GoHome : Button
{
    public override void OnSubmit(BaseEventData eventData)
    {
        MenuManager.Instance().ReadyUp(this.GetComponent<Button>());
    }
}
