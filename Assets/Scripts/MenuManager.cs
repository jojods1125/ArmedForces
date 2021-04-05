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
    [Tooltip("Weapon Selection Screen Link")]
    public GameObject SelectionMenu;

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
    [Tooltip("First Selected Weapon Selection")]
    public GameObject selectionFirstButton;

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
    public GameObject achievementContent;
    [Tooltip("Display Prefab")]
    public GameObject achievementPrefab;
    [Tooltip("Tier Prefab")]
    public GameObject tierPrefab;
    [Tooltip("Progress Bar Prefab")]
    public GameObject progressBarPrefab;

    [Header("Weapon Loadout Display")]
    [Tooltip("List of Weapons")]
    public List<Weapon> weapons;
    [Tooltip("Content of the ScrollView")]
    public GameObject weaponContent;
    [Tooltip("Group Type Prefab")]
    public GameObject groupPrefab;
    [Tooltip("Weapon Display Prefab")]
    public GameObject weaponPrefab;
    [Tooltip("Weapon Description Box")]
    public GameObject descriptionBox;
    [Tooltip("Back Arm Loadout")]
    public Weapon[] backArm = new Weapon[4];
    [Tooltip("Front Arm Loadout")]
    public Weapon[] frontArm = new Weapon[4];

    // List of Weapon Buttons
    private List<GameObject> weaponButtons = new List<GameObject>();

    [Header("Max Bar Values")]
    [Tooltip("Max Damage")]
    public float maxDamage;
    [Tooltip("Max Recoil")]
    public float maxRecoil;
    [Tooltip("Max Pushback")]
    public float maxPushback;
    [Tooltip("Max Ammo")]
    public float maxAmmo;
    [Tooltip("Max Reload Speed")]
    public float maxReloadSpeed;

    [Header("Variable Attributes")]
    [Tooltip("Max Fire rate")]
    public float maxFireRate;
    [Tooltip("Max Scatter Count")]
    public float maxScatterCount;
    [Tooltip("Max Projectile Power")]
    public float maxProjectilePower;
    [Tooltip("Max Spray Distance")]
    public float maxSprayDistance;

    // Current Menu on
    private GameObject currentMenu;
    // Last visited Menu
    private GameObject lastVisitedMenu;

    // Is the user in a menu or playing
    private bool playing = false;

    // Map selected
    private string mapName;

    // Weapon Selected
    private char currentWeapon;
    // Arm Selected - String
    private string currentArm;

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
            GameObject curr = Instantiate(achievementPrefab, achievementContent.transform);

            MakeDisplay(curr, a);
        }

        // Set up Weapon loadout
        Dictionary<string, Dictionary<WeaponRarity, List<Weapon>>> order = new Dictionary<string, Dictionary<WeaponRarity, List<Weapon>>>();
        // Dictionary 1 - Type { Auto = 0, Semi = 1, Launcher = 2, Sprayer = 3, ... }
        for (int i = 0; i < System.Enum.GetValues(typeof(WeaponType)).Length - 1; i++)  // - 1 gets rid of "none" from enum
        {
            switch (i)
            {
                case 0:
                    order.Add("Automatic", new Dictionary<WeaponRarity, List<Weapon>>());
                    break;
                case 1:
                    order.Add("Semiautomatic", new Dictionary<WeaponRarity, List<Weapon>>());
                    break;
                case 2:
                    order.Add("Launcher", new Dictionary<WeaponRarity, List<Weapon>>());
                    break;
                case 3:
                    order.Add("Sprayer", new Dictionary<WeaponRarity, List<Weapon>>());
                    break;
                default:
                    Debug.LogError("Something went wrong");
                    break;
            }
            // Dictionary 2 - Rarity { Common = 0, Uncommon = 1, Rare = 2, Legenday = 3 }
            foreach (WeaponRarity rarity in System.Enum.GetValues(typeof(WeaponRarity)))
            {
                string type = "";
                switch (i)
                {
                    case 0:
                        type = "Automatic";
                        break;
                    case 1:
                        type = "Semiautomatic";
                        break;
                    case 2:
                        type = "Launcher";
                        break;
                    case 3:
                        type = "Sprayer";
                        break;
                    default:
                        Debug.LogError("Something went wrong");
                        break;
                }
                order[type].Add(rarity, new List<Weapon>());
            }
        }
        
        // Sort through existing Weapons
        foreach (Weapon w in weapons)
        {
            WeaponRarity wr = w.rarity;

            if (w is W_AutoGun) // ordering[0]
            {
                switch (wr)
                {
                    case WeaponRarity.Common:
                        order["Automatic"][WeaponRarity.Common].Add(w);
                        break;
                    case WeaponRarity.Uncommon:
                        order["Automatic"][WeaponRarity.Uncommon].Add(w);
                        break;
                    case WeaponRarity.Rare:
                        order["Automatic"][WeaponRarity.Rare].Add(w);
                        break;
                    case WeaponRarity.Legendary:
                        order["Automatic"][WeaponRarity.Rare].Add(w);
                        break;
                    default:
                        Debug.LogError("Auto weapon does not have listed rarity");
                        break;
                }
            }
            else if (w is W_SemiGun) // ordering[1]
            {
                switch (wr)
                {
                    case WeaponRarity.Common:
                        order["Semiautomatic"][WeaponRarity.Common].Add(w);
                        break;
                    case WeaponRarity.Uncommon:
                        order["Semiautomatic"][WeaponRarity.Uncommon].Add(w);
                        break;
                    case WeaponRarity.Rare:
                        order["Semiautomatic"][WeaponRarity.Rare].Add(w);
                        break;
                    case WeaponRarity.Legendary:
                        order["Semiautomatic"][WeaponRarity.Legendary].Add(w);
                        break;
                    default:
                        Debug.LogError("Semi weapon does not have listed rarity");
                        break;
                }
            }
            else if (w is W_Launcher) // ordering[2]
            {
                switch (wr)
                {
                    case WeaponRarity.Common:
                        order["Launcher"][WeaponRarity.Common].Add(w);
                        break;
                    case WeaponRarity.Uncommon:
                        order["Launcher"][WeaponRarity.Uncommon].Add(w);
                        break;
                    case WeaponRarity.Rare:
                        order["Launcher"][WeaponRarity.Rare].Add(w);
                        break;
                    case WeaponRarity.Legendary:
                        order["Launcher"][WeaponRarity.Legendary].Add(w);
                        break;
                    default:
                        Debug.LogError("Launcher weapon does not have listed rarity");
                        break;
                }
            }
            else if (w is W_Sprayer) // ordering[3]
            {
                switch (wr)
                {
                    case WeaponRarity.Common:
                        order["Sprayer"][WeaponRarity.Common].Add(w);
                        break;
                    case WeaponRarity.Uncommon:
                        order["Sprayer"][WeaponRarity.Uncommon].Add(w);
                        break;
                    case WeaponRarity.Rare:
                        order["Sprayer"][WeaponRarity.Rare].Add(w);
                        break;
                    case WeaponRarity.Legendary:
                        order["Sprayer"][WeaponRarity.Legendary].Add(w);
                        break;
                    default:
                        Debug.LogError("Sprayer weapon does not have listed rarity");
                        break;
                }
            }
        }   // Done with Sorting

        // Construct the display
        foreach (KeyValuePair<string, Dictionary<WeaponRarity, List<Weapon>>> type in order)
        {
            // Create group
            GameObject typeGroup = Instantiate(groupPrefab, weaponContent.transform);

            // Put name in group
            typeGroup.transform.Find("Weapon Type").Find("Text").GetComponent<Text>().text = type.Key;

            // Get area for Weapons
            Transform weaponList = typeGroup.transform.Find("Weapons");

            foreach (KeyValuePair<WeaponRarity, List<Weapon>> rarity in type.Value)
            {
                Color background;
                switch (rarity.Key)
                {
                    case WeaponRarity.Common:
                        background = Color.white;
                        break;
                    case WeaponRarity.Uncommon:
                        background = Color.green;
                        break;
                    case WeaponRarity.Rare:
                        background = Color.blue;
                        break;
                    case WeaponRarity.Legendary:
                        background = Color.magenta;
                        break;
                    default:
                        background = Color.black;
                        break;
                }

                foreach (Weapon w in rarity.Value)
                {
                    // Create Weapon Display
                    GameObject weapon = Instantiate(weaponPrefab, weaponList);
                    weapon.GetComponent<Button>().onClick.AddListener(() => ChooseWeapon(w));
                    weaponButtons.Add(weapon);

                    // Set Weapon & stats
                    WeaponButtonContatiner wbc = weapon.transform.GetComponent<WeaponButtonContatiner>();
                    wbc.weapon = w;
                    // assign attributes
                    if (w is W_AutoGun)
					{
                        W_AutoGun auto = (W_AutoGun)w;
                        wbc.damage = auto.bulletDamage;
                        wbc.recoil = auto.pushback;
                        wbc.pushback = auto.bulletPushback;
                        wbc.variable = 1f / auto.fireRate;
                        wbc.ammoCapacity = auto.ammoCapacity;
                        wbc.reloadSpeed = 1f / auto.reloadRate;
					}
                    else if (w is W_SemiGun)
					{
                        W_SemiGun semi = (W_SemiGun)w;
                        wbc.damage = semi.bulletDamage;
                        wbc.recoil = semi.pushback;
                        wbc.pushback = semi.bulletPushback;
                        wbc.variable = 1f / semi.burstCount;
                        wbc.ammoCapacity = semi.ammoCapacity;
                        wbc.reloadSpeed = 1f / semi.reloadRate;
                    }
                    else if (w is W_Launcher)
					{
                        W_Launcher launcher = (W_Launcher)w;
                        wbc.damage = launcher.coreDamage;
                        wbc.recoil = launcher.pushback;
                        wbc.pushback = launcher.corePushback;
                        wbc.variable = 1f / launcher.projectilePower;
                        wbc.ammoCapacity = launcher.ammoCapacity;
                        wbc.reloadSpeed = 1f / launcher.reloadRate;
                    }
                    else if (w is W_Sprayer)
					{
                        W_Sprayer sprayer = (W_Sprayer)w;
                        wbc.damage = sprayer.bulletDamage;
                        wbc.recoil = sprayer.pushback;
                        wbc.pushback = sprayer.bulletPushback;
                        wbc.variable = 1f / sprayer.sprayDistance;
                        wbc.ammoCapacity = sprayer.ammoCapacity;
                        wbc.reloadSpeed = 1f / sprayer.reloadRate;
                    }

                    // Set image
                    weapon.transform.Find("Image").GetComponent<Image>().sprite = w.icon;

                    // Set Rarity
                    weapon.GetComponent<Image>().color = background;
                }
            }
        }

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
        if (AchievementsMenu.activeSelf)
        {
            for (int i = 0; i < AchievementManager.Instance().achievements.Count; i++)
            {
                // Get Achievement
                Achievement a = AchievementManager.Instance().achievements[i];
                // Get Achievement Display
                Transform curr = achievementContent.transform.GetChild(i);
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

        // Update Weapon display
        if (SelectionMenu.activeSelf)
		{
            foreach (GameObject button in weaponButtons)
			{
                if (EventSystem.current.currentSelectedGameObject == button)
				{
                    WeaponButtonContatiner  wpc = button.GetComponent<WeaponButtonContatiner>();
                    Weapon weapon = wpc.weapon;
                    Transform nameAndDesc = descriptionBox.transform.Find("Name and Desc");
                    nameAndDesc.Find("Name").GetComponent<Text>().text = weapon.weaponName;
                    nameAndDesc.Find("Description").GetComponent<Text>().text = weapon.description;

                    // Get attribute holder
                    Transform attributes = descriptionBox.transform.Find("Attributes");
                    Transform dmg = attributes.Find("Damage");
                    Transform recoil = attributes.Find("Recoil");
                    Transform pushBack = attributes.Find("PushBack");
                    Transform variable = attributes.Find("VariableAttribute");
                    Transform ammoCap = attributes.Find("AmmoCap");
                    Transform reloadSpeed = attributes.Find("ReloadSpeed");

                    // Zero vector
                    Vector3 zeroed = new Vector3(0f, 0f, 0f);

                    // Set damage bar
                    Vector3 damageBar = new Vector3(wpc.damage / maxDamage, 1f, 1f);
                    if (wpc.damage > 0)
                    {
                        dmg.Find("BarBack").Find("Positive").localScale = damageBar;
                        dmg.Find("BarBack").Find("Negative").localScale = zeroed;
                    }
                    else if (wpc.damage < 0)
					{
                        dmg.Find("BarBack").Find("Negative").localScale = damageBar;
                        dmg.Find("BarBack").Find("Positive").localScale = zeroed;
                    }

                    // Set recoil bar
                    Vector3 recoilBar = new Vector3(wpc.recoil / maxRecoil, 1f, 1f);
                    if (wpc.recoil > 0)
                    {
                        recoil.Find("BarBack").Find("Positive").localScale = recoilBar;
                        recoil.Find("BarBack").Find("Negative").localScale = zeroed;
                    }
                    else if (wpc.recoil < 0)
                    {
                        recoil.Find("BarBack").Find("Negative").localScale = recoilBar;
                        recoil.Find("BarBack").Find("Positive").localScale = zeroed;
                    }

                    // Set pushback bar
                    Vector3 pushbackBar = new Vector3(wpc.pushback / maxPushback, 1f, 1f);
                    if (wpc.pushback > 0)
                    {
                        pushBack.Find("BarBack").Find("Positive").localScale = pushbackBar;
                        pushBack.Find("BarBack").Find("Negative").localScale = zeroed;
                    }
                    else if (wpc.pushback < 0)
                    {
                        pushBack.Find("BarBack").Find("Negative").localScale = pushbackBar;
                        pushBack.Find("BarBack").Find("Positive").localScale = zeroed;
                    }

                    // Set variable bar
                    string variableText = "{{INVALID}}";
                    float variableMax = 0;
                    if (weapon is W_AutoGun)
                    {
                        variableText = "Fire Rate";
                        variableMax = maxFireRate;
                    }
                    else if (weapon is W_SemiGun)
                    {
                        variableText = "Scatter Count";
                        variableMax = maxScatterCount;
                    }
                    else if (weapon is W_Launcher)
                    {
                        variableText = "Projectile Power";
                        variableMax = maxProjectilePower;
                    }
                    else if (weapon is W_Sprayer)
					{
                        variableText = "Sprayer Distance";
                        variableMax = maxSprayDistance;
					}

                    variable.Find("Text").GetComponent<Text>().text = variableText;
                    variable.Find("BarBack").Find("Positive").localScale = new Vector3(wpc.variable / variableMax, 1f, 1f);

                    // Set ammo capacity bar
                    ammoCap.Find("BarBack").Find("Positive").localScale = new Vector3(wpc.ammoCapacity / maxAmmo, 1f, 1f);

                    // Set reload speed bar
                    reloadSpeed.Find("BarBack").Find("Positive").localScale = new Vector3(wpc.reloadSpeed / maxReloadSpeed, 1f, 1f);

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
        // set inactive if active
        if (SelectionMenu.activeSelf)
        {
            SelectionMenu.SetActive(false);
        }

        if (!MainMenu.activeSelf)
		{
            MainMenu.SetActive(true);
		}

        // Set Loadout Images
        Transform back = LoadoutMenu.transform.Find("Back Arm Loadout");
        Transform front = LoadoutMenu.transform.Find("Front Arm Loadout");
        
        // Back
        for (int i = 0; i < backArm.Length; i++)
		{
            back.GetChild(i + 1).Find("Image").GetComponent<Image>().sprite = backArm[i].icon;
            front.GetChild(i + 1).Find("Image").GetComponent<Image>().sprite = frontArm[i].icon;
		}

        LoadoutMenu.SetActive( true );
        //setActiveMenu( LoadoutMenu );
        currentMenu = LoadoutMenu;

        // Clear selected object
        EventSystem.current.SetSelectedGameObject(null);
        // Set button to LoadoutFirst
        EventSystem.current.SetSelectedGameObject(loadoutFirstButton);
    }

    /// <summary>
    /// Enables the Weapon Selection Screen given the current Weapon slot
    /// </summary>
    /// <param name="weaponSlot"> Weapon Slot to modify </param>
    public void DisplayWeapons(string weaponSlot)
    {
        char arm = weaponSlot[0];
        if (arm.Equals('B'))
		{
            currentArm = "Back";
		}
        else if (arm.Equals('F'))
		{
            currentArm = "Front";
		}
        currentWeapon = weaponSlot[1];
        MainMenu.SetActive(false);
        LoadoutMenu.SetActive(false);

        SelectionMenu.SetActive(true);
        currentMenu = SelectionMenu;

        // Clear selected object
        EventSystem.current.SetSelectedGameObject(null);
        // Set button to LoadoutFirst
        EventSystem.current.SetSelectedGameObject(selectionFirstButton);
    }


    public void ChooseWeapon( Weapon weapon )
	{
        Debug.Log("In ChooseWeapon for " + currentArm + "\'s weapon " + currentWeapon + " to have weapon " + weapon.name);

        // Change Loadout
        int index = currentWeapon - 65;
        if (currentArm.Equals("Back"))
		{
            backArm[index] = weapon;
		}
        else if (currentArm.Equals("Front"))
		{
            frontArm[index] = weapon;
		}

        // go back to Loadout menu
        Loadout();
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
}
