using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

/// <summary>
/// RETRIEVED FROM https://forum.unity.com/threads/scrollview-using-controller-arrowkeys.1008121/
/// USER : Havyx ( https://forum.unity.com/members/havyx.6067685/ )
/// Edited with topValue for own use
/// 
/// Automatically Scroll through the Dropdown list based on Selected object
/// </summary>
public class ScrollRectHorizontal : MonoBehaviour
{

    // Top Value of the items for calculations
    int topValue = 525;
    RectTransform scrollRectTransform;
    RectTransform contentPanel;
    RectTransform selectedRectTransform;
    GameObject lastSelected;
    void Start()
    {
        scrollRectTransform = GetComponent<RectTransform>();
        contentPanel = GetComponent<ScrollRect>().content;
    }
    void Update()
    {
        // Get the currently selected UI element from the event system.
        GameObject selected = EventSystem.current.currentSelectedGameObject;
        // Return if there are none.
        if (selected == null)
        {
            return;
        }
        // Return if the selected game object is not inside the scroll rect.
        if (selected.transform.parent != contentPanel.transform)
        {
            return;
        }
        // Return if the selected game object is the same as it was last frame,
        // meaning we haven't moved.
        if (selected == lastSelected)
        {
            return;
        }
        // Get the rect tranform for the selected game object.
        selectedRectTransform = selected.GetComponent<RectTransform>();
        // The position of the selected UI element is the absolute anchor position,
        // ie. the local position within the scroll rect + its height if we're
        // scrolling down. If we're scrolling up it's just the absolute anchor position.
        float selectedPositionX = Mathf.Abs(selectedRectTransform.anchoredPosition.x - topValue) + selectedRectTransform.rect.width;
        // The upper bound of the scroll view is the anchor position of the content we're scrolling.
        float scrollViewMinX = contentPanel.anchoredPosition.x;
        // The lower bound is the anchor position + the height of the scroll rect.
        float scrollViewMaxX = contentPanel.anchoredPosition.x + scrollRectTransform.rect.width;
        // If the selected position is below the current lower bound of the scroll view we scroll down.
        if (selectedPositionX > scrollViewMaxX)
        {
            float newX = selectedPositionX - scrollRectTransform.rect.width;
            contentPanel.anchoredPosition = new Vector2(newX, contentPanel.anchoredPosition.y);
        }
        // If the selected position is above the current upper bound of the scroll view we scroll up.
        else if (Mathf.Abs(selectedRectTransform.anchoredPosition.x - topValue) < scrollViewMinX)
        {
            contentPanel.anchoredPosition = new Vector2(Mathf.Abs(selectedRectTransform.anchoredPosition.x - topValue), contentPanel.anchoredPosition.y);
        }
        lastSelected = selected;
    }
}