using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class MenuManager : MonoBehaviour
{

    [Header("Menus to link to")]
    [Tooltip("Main Menu link")]
    public GameObject MainMenu;
    [Tooltip("Offline Menu link")]
    public GameObject TrainingMenu;
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
    public GameObject trainingFirstButton;
    [Tooltip("First Selected Button on Pregame Menu")]
    public GameObject pregameFirstButton;

    [Header("Return to last Menu Button for each Menu")]
    [Tooltip("Return to Main Button on Achievements Menu")]
    public GameObject achievementsReturnButton;
    [Tooltip("Return to Main Button on Loadout Menu")]
    public GameObject loadoutReturnButton;
    [Tooltip("Return to Main Button on Offline Menu")]
    public GameObject trainingReturnButton;
    [Tooltip("Return to Map Selection Button on Pregame Menu")]
    public GameObject pregameReturnButton;

    [Header("Achievement Menu Display")]
    [Tooltip("Content of the ScrollView")]
    public GameObject content;
    [Tooltip("Display Prefab")]
    public GameObject achievementPrefab;
    [Tooltip("Tier Prefab")]
    public GameObject tierPrefab;
    [Tooltip("Progress Bar Prefab")]
    public GameObject progressBarPrefab;

    [Header("Weapon Loadout Display")]
    [Tooltip("List of Weapons")]
    public List<Weapon> weapons;

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

        // Go through all register Achievements
        foreach (Achievement a in AchievementManager.Instance().achievements)
        {
            // Create a GameObject based on achDisplay prefab
            GameObject curr = Instantiate(achievementPrefab, content.transform);

            MakeDisplay(curr, a);
        }

        // Set up Weapon loadout


    }

    private void FixedUpdate()
    {
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
            else if(currentMenu == TrainingMenu)
            {
                // Clear selected object
                EventSystem.current.SetSelectedGameObject(null);
                // Set button to OfflineFirst
                EventSystem.current.SetSelectedGameObject(trainingReturnButton);
            }

        }

        // update Achievements Menu
        for (int i = 0; i < AchievementManager.Instance().achievements.Count; i++)
        {
            // Get Achievement
            Achievement a = AchievementManager.Instance().achievements[i];
            // Get Achievement Display
            Transform curr = content.transform.GetChild(i);
            // Get Progress Bars for Display
            Transform progressBars = curr.Find("Progress Bars");

            // Update progress bars if tiered
            if (a is A_Tiered)
            {
                // cast 
                A_Tiered at = (A_Tiered)a;
                int currentValue = at.currentValue;
                for (int j = 0; j < at.activationValues.Length; j++)
                {
                    // Current working bar
                    Transform bar = progressBars.GetChild(j);

                    // This tiers max
                    int max = at.activationValues[j];

                    // Check if this bar needs scaled at all -> Zero the scale
                    if (j > 0 && currentValue < at.activationValues[j - 1])
                    {
                        bar.localScale = new Vector3(0f, bar.localScale.y, bar.localScale.z);
                    }
                    // Check if this bar has been met/exceeded -> Max the scale
                    else if (currentValue >= max)
                    {
                        bar.localScale = new Vector3(1f, bar.localScale.y, bar.localScale.z);
                    }
                    // Set the bar scale
                    else
                    {
                        float percent = 0f;
                        if (j > 0)
                        {
                            int delta = at.activationValues[j - 1];
                            percent = (currentValue - delta) / ((float)max - delta);
                        }
                        else
                        {
                            percent = currentValue / (float)max;
                        }
                        /*float percent = currentValue / (float)max;*/
                        bar.localScale = new Vector3(percent, bar.localScale.y, bar.localScale.z);
                    }
                }
            }
            else if (a is A_Repeatable)
            {
                A_Repeatable ar = (A_Repeatable)a;
                int currentValue = ar.currentValue % ar.repeatValue;
                for (int j = 0; j < ar.repeatValue; j++)
                {
                    // Current working bar
                    Transform bar = progressBars.GetChild(j);

                    // set bar to full if past or at progress
                    if (j < currentValue)
                    {
                        bar.localScale = new Vector3(1f, bar.localScale.y, bar.localScale.z);
                    }
                    else
                    {
                        bar.localScale = new Vector3(0f, bar.localScale.y, bar.localScale.z);
                    }
                }
            }
            /** Set fields in display */
            // Name
            curr.transform.Find("Name").gameObject.GetComponent<TMP_Text>().text = a.achievementMessage;
            // Description
            curr.transform.Find("Description").gameObject.GetComponent<TMP_Text>().text = a.ToString();
            // Current Count
            curr.transform.Find("Current Count").gameObject.GetComponent<TMP_Text>().text = "Current Count: " + a.currentValue.ToString();
            // Next Count & set finished
            if (a is A_Tiered)
            {
                A_Tiered at = (A_Tiered)a;
                if (!at.IsComplete() && at.nextTier < at.activationValues.Length)
                {
                    curr.transform.Find("Next Count").gameObject.GetComponent<TMP_Text>().text = "Next Count: " + at.activationValues[at.nextTier].ToString();
                    curr.transform.Find("Finished").gameObject.SetActive(false);
                }
                else
                {
                    curr.transform.Find("Next Count").gameObject.GetComponent<TMP_Text>().text = "Achieved";
                    curr.transform.Find("Finished").gameObject.SetActive(true);
                }
            }
        }

    }

    /// <summary>
    /// Turns the OfflineMenu active 
	/// Clears and sets 1st button to active
    /// </summary>
    public void TrainingMode()
    {
        // set inactive if active
        if (PregameMenu.activeSelf)
		{
            PregameMenu.SetActive( false );
		}

        TrainingMenu.SetActive( true );
        //setActiveMenu( OfflineMenu );
        currentMenu = TrainingMenu;

        // Clear selected object
        EventSystem.current.SetSelectedGameObject(null);
        // Set button to OfflineFirst
        EventSystem.current.SetSelectedGameObject(trainingFirstButton);
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
        // SceneManager.LoadScene( "L_LobbyMenu" );
        SceneManager.LoadScene("OfflineScene");
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

    /// <summary>
    /// Helper method to fill in an Achievement Display given an Achievement
    /// </summary>
    /// <param name="curr"> Current Display object </param>
    /// <param name="a"> Achievement to use </param>
    public void MakeDisplay(GameObject curr, Achievement a)
    {
        // Get the Tiers child
        Transform tiers = curr.transform.Find("Tiers");
        // Get the Progress Bar child
        Transform progressBars = curr.transform.Find("Progress Bars");

        // Create a tier for every activation value
        if (a is A_Tiered)
        {
            A_Tiered at = (A_Tiered)a;
            for (int i = 0; i < at.activationValues.Length; i++)
            {
                Instantiate(tierPrefab, tiers);
                Instantiate(progressBarPrefab, progressBars);
            }

            // Set size of tier
            GridLayoutGroup tglg = tiers.GetComponent<GridLayoutGroup>();
            tglg.cellSize = new Vector2(tglg.cellSize.x / at.activationValues.Length, tglg.cellSize.y);
            // Set size of progress bars
            GridLayoutGroup pbglg = progressBars.GetComponent<GridLayoutGroup>();
            pbglg.cellSize = new Vector2(pbglg.cellSize.x / at.activationValues.Length, pbglg.cellSize.y);

            // Set progress bars
            int currentValue = at.currentValue;
            for (int i = 0; i < at.activationValues.Length; i++)
            {
                // Current working bar
                Transform bar = progressBars.GetChild(i);

                // This tiers max
                int max = at.activationValues[i];

                // Check if this bar needs scaled at all, break if not
                if (i > 0 && currentValue < at.activationValues[i - 1])
                {
                    break;
                }
                // Check if this bar has been met/exceeded
                else if (currentValue >= max)
                {
                    bar.localScale = new Vector3(1f, bar.localScale.y, bar.localScale.z);
                }
                // Set the bar scale
                else
                {
                    bar.localScale = new Vector3(currentValue / max, bar.localScale.y, bar.localScale.z);
                }
            }
        }
        else if (a is A_Repeatable)
        {
            A_Repeatable ar = (A_Repeatable)a;
            // create tiers and progress bars
            for (int i = 0; i < ar.repeatValue; i++)
            {
                Instantiate(tierPrefab, tiers);
                Instantiate(progressBarPrefab, progressBars);
            }

            // Set size of tier
            GridLayoutGroup tglg = tiers.GetComponent<GridLayoutGroup>();
            tglg.cellSize = new Vector2(tglg.cellSize.x / ar.repeatValue, tglg.cellSize.y);
            // Set size of progress bars
            GridLayoutGroup pbglg = progressBars.GetComponent<GridLayoutGroup>();
            pbglg.cellSize = new Vector2(pbglg.cellSize.x / ar.repeatValue, pbglg.cellSize.y);

            int currentValue = ar.currentValue % ar.repeatValue;
            for (int j = 0; j < ar.repeatValue; j++)
            {
                // Current working bar
                Transform bar = progressBars.GetChild(j);

                // set bar to full if past or at progress
                if (j < currentValue)
                {
                    bar.localScale = new Vector3(1f, bar.localScale.y, bar.localScale.z);
                }
                else
                {
                    break;
                }
            }
        }

        /** Set fields in display */
        // Name
        curr.transform.Find("Name").gameObject.GetComponent<TMP_Text>().text = a.achievementMessage;
        // Description
        curr.transform.Find("Description").gameObject.GetComponent<TMP_Text>().text = a.ToString();
        // Current Count
        curr.transform.Find("Current Count").gameObject.GetComponent<TMP_Text>().text = "Current Count: " + a.currentValue.ToString();
        // Next Count & set finished
        if (a is A_Tiered)
        {
            A_Tiered at = (A_Tiered)a;
            if (!at.IsComplete())
            {
                curr.transform.Find("Next Count").gameObject.GetComponent<TMP_Text>().text = "Next Count: " + at.activationValues[at.nextTier].ToString();
                curr.transform.Find("Finished").gameObject.SetActive(false);
            }
            else
            {
                curr.transform.Find("Next Count").gameObject.GetComponent<TMP_Text>().text = "Achieved";
                curr.transform.Find("Finished").gameObject.SetActive(true);
            }
        }
    }


    public void displayWeapons (char weaponSlot)
    {
        
    }
}
