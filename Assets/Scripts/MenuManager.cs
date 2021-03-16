using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MenuManager : MonoBehaviour
{

    [Header("Menus to link to")]
    [Tooltip("Main Menu link")]
    public GameObject MainMenu;
    [Tooltip("Achievements Menu link")]
    public GameObject AchievementsMenu;
    [Tooltip("Loadout Menu link")]
    public GameObject LoadoutMenu;
    [Tooltip("Offline Menu link")]
    public GameObject OfflineMenu;

    [Header("First Selected Button for each Menu")]
    [Tooltip("First Selected Button on Main Menu")]
    public GameObject mainFirstButton;
    [Tooltip("First Selected Button on Achievements Menu")]
    public GameObject achievementsFirstButton;
    [Tooltip("First Selected Button on Loadout Menu")]
    public GameObject loadoutFirstButton;
    [Tooltip("First Selected Button on Offline Menu")]
    public GameObject offlineFirstButton;

    [Header("Return to last Menu Button for each Menu")]
    [Tooltip("Return to Main Button on Achievements Menu")]
    public GameObject achievementsReturnButton;
    [Tooltip("Return to Main Button on Loadout Menu")]
    public GameObject loadoutReturnButton;
    [Tooltip("Return to Main Button on Offline Menu")]
    public GameObject offlineReturnButton;

    // Current Menu on
    private GameObject currentMenu;
    // Last visited Menu
    private GameObject lastVisitedMenu;

    // Is the user in a menu or playing
    private bool playing = false;

    // Start is called before the first frame update
    void Start()
    {
        lastVisitedMenu = MainMenu;
    }

    private void FixedUpdate()
    {
        // Pause the Game
        if ( Gamepad.current[GamepadButton.Start].isPressed )
        {
            // Toggle Pause menu
            // PauseUnpause();
        }

        // Select "Return to Main" button based on screen
        if ( !playing && Gamepad.current[GamepadButton.B].isPressed && currentMenu != MainMenu )
        {
            if ( currentMenu == AchievementsMenu )
            {
                // Clear selected object
                EventSystem.current.SetSelectedGameObject(null);
                // Set button to OfflineFirst
                EventSystem.current.SetSelectedGameObject(achievementsReturnButton);
            }
            else if (currentMenu == LoadoutMenu)
            {
                // Clear selected object
                EventSystem.current.SetSelectedGameObject(null);
                // Set button to OfflineFirst
                EventSystem.current.SetSelectedGameObject(loadoutReturnButton);
            }
            else if(currentMenu == OfflineMenu)
            {
                // Clear selected object
                EventSystem.current.SetSelectedGameObject(null);
                // Set button to OfflineFirst
                EventSystem.current.SetSelectedGameObject(offlineReturnButton);
            }

        }

    }

    /// <summary>
    /// Turns the OfflineMenu active 
    /// </summary>
    public void OfflineMode()
    {
        OfflineMenu.SetActive( true );
        currentMenu = OfflineMenu;

        // Clear selected object
        EventSystem.current.SetSelectedGameObject(null);
        // Set button to OfflineFirst
        EventSystem.current.SetSelectedGameObject(offlineFirstButton);
    }

    /// <summary>
    /// Turns the OfflineMenu active 
    /// </summary>
    public void Achievements()
    {
        AchievementsMenu.SetActive(true);
        currentMenu = AchievementsMenu;

        // Clear selected object
        EventSystem.current.SetSelectedGameObject(null);
        // Set button to AchievementFirst
        EventSystem.current.SetSelectedGameObject(achievementsFirstButton);

    }

    /// <summary>
	/// Change to the Map Selection scene
	/// </summary>
    public void MapSelection()
	{
        SceneManager.LoadScene( "L_MapSelectionMenu" );
	}

    /// <summary>
	/// Change to the Weapon Loadout scene
	/// </summary>
    public void Loadout()
	{
        // SceneManager.LoadScene( "L_LoadoutMenu" );
        LoadoutMenu.SetActive( true );
        currentMenu = LoadoutMenu;

        // Clear selected object
        EventSystem.current.SetSelectedGameObject(null);
        // Set button to LoadoutFirst
        EventSystem.current.SetSelectedGameObject(loadoutFirstButton);
    }

    /// <summary>
	/// Close the game
	/// </summary>
    public void Exit()
	{
        Application.Quit();
	}

    public void ReturnToMain()
	{
        // SceneManager.LoadScene( "L_MainMenu" );
        if ( LoadoutMenu.activeSelf )
        {
            LoadoutMenu.SetActive( false );
        }
        if ( AchievementsMenu.activeSelf )
        {
            AchievementsMenu.SetActive( false );
        }
        if ( OfflineMenu.activeSelf )
        {
            OfflineMenu.SetActive( false );
        }

        currentMenu = MainMenu;

        // Clear selected object
        EventSystem.current.SetSelectedGameObject(null);
        // Set button to MainFirst
        EventSystem.current.SetSelectedGameObject(mainFirstButton);
    }
}
