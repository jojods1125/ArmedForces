using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonTesting : MonoBehaviour
{
    //Press enter on the Button GameObject to trigger this Event
    public void OnSubmit(BaseEventData eventData)
    {
        //Output that the Button is in the submit stage
        Debug.Log("Submitted!");
    }
}
