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
    [Tooltip("Offline Menu link")]
    public GameObject OfflineMenu;
    [Tooltip("Achievements Menu link")]
    public GameObject AchievementsMenu;
    [Tooltip("Loadout Menu link")]
    public GameObject LoadoutMenu;
    [Tooltip("Pregame Menu Link")]
    public GameObject PregameMenu;

    [Header("Menus")]
    [Tooltip("Group of all Menus")]
    public GameObject menuGroup;

    // internal list of menus
    private List<GameObject> menuList = new List<GameObject>();

    [Header("First Selected Button for each Menu")]
    [Tooltip("First Selected Button on Main Menu")]
    public GameObject mainFirstButton;
    [Tooltip("First Selected Button on Achievements Menu")]
    public GameObject achievementsFirstButton;
    [Tooltip("First Selected Button on Loadout Menu")]
    public GameObject loadoutFirstButton;
    [Tooltip("First Selected Button on Offline Menu")]
    public GameObject offlineFirstButton;
    [Tooltip("First Selected Button on Pregame Menu")]
    public GameObject pregameFirstButton;

    [Header("Return to last Menu Button for each Menu")]
    [Tooltip("Return to Main Button on Achievements Menu")]
    public GameObject achievementsReturnButton;
    [Tooltip("Return to Main Button on Loadout Menu")]
    public GameObject loadoutReturnButton;
    [Tooltip("Return to Main Button on Offline Menu")]
    public GameObject offlineReturnButton;
    [Tooltip("Return to Map Selection Button on Pregame Menu")]
    public GameObject pregameReturnButton;

    // Current Menu on
    private GameObject currentMenu;
    // Last visited Menu
    private GameObject lastVisitedMenu;

    // Is the user in a menu or playing
    private bool playing = false;

    // Map selected
    private string mapName;

    // Start is called before the first frame update
    void Start()
    {
        lastVisitedMenu = MainMenu;

        foreach (Transform child in menuGroup.transform)
		{
            menuList.Add( child.gameObject );
		}

        // Clear selected object
        EventSystem.current.SetSelectedGameObject(null);
        // Set button to MainFirst
        EventSystem.current.SetSelectedGameObject(mainFirstButton);

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
	/// Clears and sets 1st button to active
    /// </summary>
    public void OfflineMode()
    {
        // set inactive if active
        if (PregameMenu.activeSelf)
		{
            PregameMenu.SetActive( false );
		}

        OfflineMenu.SetActive( true );
        //setActiveMenu( OfflineMenu );
        currentMenu = OfflineMenu;

        // Clear selected object
        EventSystem.current.SetSelectedGameObject(null);
        // Set button to OfflineFirst
        EventSystem.current.SetSelectedGameObject(offlineFirstButton);
    }

    /// <summary>
    /// Turns the AchievementsMenu active 
	/// Clears and sets 1st button to active
    /// </summary>
    public void Achievements()
    {
        AchievementsMenu.SetActive(true);
        //setActiveMenu( AchievementsMenu );
        currentMenu = AchievementsMenu;

        // Clear selected object
        EventSystem.current.SetSelectedGameObject(null);
        // Set button to AchievementFirst
        EventSystem.current.SetSelectedGameObject(achievementsFirstButton);

    }

    /// <summary>
	/// Change to the Lobby scene (new menu layout)
	/// </summary>
    public void LoadLobby()
	{
        SceneManager.LoadScene( "L_LobbyMenu" );
	}

    /// <summary>
	/// Change to the Weapon Loadout scene
	/// Clears and sets 1st button to active
	/// </summary>
    public void Loadout()
	{
        LoadoutMenu.SetActive( true );
        //setActiveMenu( LoadoutMenu );
        currentMenu = LoadoutMenu;

        // Clear selected object
        EventSystem.current.SetSelectedGameObject(null);
        // Set button to LoadoutFirst
        EventSystem.current.SetSelectedGameObject(loadoutFirstButton);
    }

    /// <summary>
    /// Changes the active menu to the Pregame menu
    /// Sets the mapName to the given Map name
    /// </summary>
    /// <param name="mapName"> Name of the Map choosen </param>
    public void Pregame( string mapName )
	{
        PregameMenu.SetActive( true );
        //setActiveMenu( PregameMenu );
        currentMenu = PregameMenu;

        this.mapName = mapName;

        // Clear selected object
        EventSystem.current.SetSelectedGameObject(null);
        // Set button to LoadoutFirst
        EventSystem.current.SetSelectedGameObject(pregameFirstButton);
    }

    /// <summary>
	/// Close the game
	/// </summary>
    public void Exit()
	{
        Application.Quit();
	}

    /// <summary>
	/// Returns to the Main Menu
	/// Deactivates other menus and activates Main with its 1st button
	/// </summary>
    public void ReturnToMain()
	{
        setActiveMenu( MainMenu );

        currentMenu = MainMenu;

        // Clear selected object
        EventSystem.current.SetSelectedGameObject(null);
        // Set button to MainFirst
        EventSystem.current.SetSelectedGameObject(mainFirstButton);
    }

    /// <summary>
    /// Loads the map that was selected
    /// </summary>
    public void LoadMap()
	{
        LoadScene( mapName );
	}

    /// <summary>
    /// Load the Scene for {{Stage 1}}
    /// </summary>
    /// <param name="name"> Name of the Scene to load </param>
    public void LoadScene( string name )
    {
        // load scene
        SceneManager.LoadScene( name );
        // clear selected
        EventSystem.current.SetSelectedGameObject( null );
    }

    /// <summary>
    /// Helper method to deactivate any menu that is not supposed to be displayed
    /// </summary>
    /// <param name="menu"> Menu to display </param>
    private void setActiveMenu( GameObject menu )
	{
        foreach (GameObject curr in menuList)
		{
            if (curr.Equals( menu ))
			{
                curr.SetActive( true );
			}
            else if (menu.activeSelf)
			{
                curr.SetActive( false );
			}
		}
    }
}
