using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public class LoadingLoadout : MonoBehaviour
{
    [Header("Weapon Loadout Display")]
    [Tooltip("Group Type Prefab")]
    public GameObject groupPrefab;
    [Tooltip("Weapon Display Prefab")]
    public GameObject weaponPrefab;

    [Header("Buttons")]
    [Tooltip("Back Button")]
    public Button backButton;
    [Tooltip("Start Button")]
    public Button startButton;

    private string[] currentArm = new string[4];
    private char[] currentWeapon = new char[4];

    // Dictionary of playerId to List of Weapon Buttons
    private Dictionary<int, List<GameObject>> weaponButtons = new Dictionary<int, List<GameObject>>();

    public void OnPlayerJoined(PlayerInput loadoutLayout)
    {
        int playerId = MenuManager.Instance().numPlayers++;

        loadoutLayout.GetComponent<TempFix>().playerId = playerId;

        // Put layout in correct spot
        loadoutLayout.transform.SetParent(transform, false);
        loadoutLayout.gameObject.name = "Player Loadout " + playerId;

        // set button onClick()'s
        // Back Arm
        Transform backArm = loadoutLayout.transform.Find("Loadouts").Find("Back Arm Loadout");
        backArm.Find("Weapon A").GetComponent<Button>().onClick.AddListener(() => DisplayWeapons(playerId + "BA"));
        backArm.Find("Weapon B").GetComponent<Button>().onClick.AddListener(() => DisplayWeapons(playerId + "BB"));
        backArm.Find("Weapon C").GetComponent<Button>().onClick.AddListener(() => DisplayWeapons(playerId + "BC"));
        backArm.Find("Weapon D").GetComponent<Button>().onClick.AddListener(() => DisplayWeapons(playerId + "BD"));
        // Front Arm
        Transform frontArm = loadoutLayout.transform.Find("Loadouts").Find("Front Arm Loadout");
        frontArm.Find("Weapon A").GetComponent<Button>().onClick.AddListener(() => DisplayWeapons(playerId + "FA"));
        frontArm.Find("Weapon B").GetComponent<Button>().onClick.AddListener(() => DisplayWeapons(playerId + "FB"));
        frontArm.Find("Weapon C").GetComponent<Button>().onClick.AddListener(() => DisplayWeapons(playerId + "FC"));
        frontArm.Find("Weapon D").GetComponent<Button>().onClick.AddListener(() => DisplayWeapons(playerId + "FD"));

        WeaponManager.Instance().LoadLoadout();

        // Update Loadouts
        for (int i = 0; i < WeaponManager.Instance().playerLoadouts[playerId][0].Length; i++)
        {
            backArm.GetChild(i + 1).Find("Image").GetComponent<Image>().sprite = WeaponManager.Instance().playerLoadouts[playerId][0][i].icon;
            frontArm.GetChild(i + 1).Find("Image").GetComponent<Image>().sprite = WeaponManager.Instance().playerLoadouts[playerId][1][i].icon;
        }

        Dictionary<string, Dictionary<WeaponRarity, List<Weapon>>> order = MenuManager.Instance().order;

        // Check that Display not already made
        if (weaponButtons.ContainsKey(playerId))
        {
            return;
        }
        // Construct the display
        List<GameObject> temp = new List<GameObject>();
        foreach (KeyValuePair<string, Dictionary<WeaponRarity, List<Weapon>>> type in order)
        {
            // Create group
            Transform weaponContent = loadoutLayout.transform.Find("Selection").Find("Scroll View").Find("Viewport").Find("Content");
            GameObject typeGroup = Instantiate(groupPrefab, weaponContent);

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
                    weapon.GetComponent<Button>().onClick.AddListener(() => ChooseWeapon(playerId, w));
                    if (!w.unlocked)
                    {
                        weapon.GetComponent<Button>().interactable = false;
                    }
                    temp.Add(weapon);

                    // Set Weapon & stats
                    WeaponButtonContatiner wbc = weapon.transform.GetComponent<WeaponButtonContatiner>();
                    wbc.weapon = w;
                    // assign attributes
                    if (w is W_AutoGun)
                    {
                        W_AutoGun auto = (W_AutoGun)w;
                        wbc.damage = auto.bulletDamage;
                        wbc.recoil = auto.recoil;
                        wbc.pushback = auto.bulletPushback;
                        wbc.variable = 1f / auto.fireRate;
                        wbc.ammoCapacity = auto.ammoCapacity;
                        wbc.reloadSpeed = 1f / auto.reloadRate;
                    }
                    else if (w is W_SemiGun)
                    {
                        W_SemiGun semi = (W_SemiGun)w;
                        wbc.damage = semi.bulletDamage;
                        wbc.recoil = semi.recoil;
                        wbc.pushback = semi.bulletPushback;
                        wbc.variable = 1f / semi.burstCount;
                        wbc.ammoCapacity = semi.ammoCapacity;
                        wbc.reloadSpeed = 1f / semi.reloadRate;
                    }
                    else if (w is W_Launcher)
                    {
                        W_Launcher launcher = (W_Launcher)w;
                        wbc.damage = launcher.coreDamage;
                        wbc.recoil = launcher.recoil;
                        wbc.pushback = launcher.corePushback;
                        wbc.variable = 1f / launcher.projectilePower;
                        wbc.ammoCapacity = launcher.ammoCapacity;
                        wbc.reloadSpeed = 1f / launcher.reloadRate;
                    }
                    else if (w is W_Sprayer)
                    {
                        W_Sprayer sprayer = (W_Sprayer)w;
                        wbc.damage = sprayer.bulletDamage;
                        wbc.recoil = sprayer.recoil;
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

        weaponButtons.Add(playerId, temp);
    }

    public void OnPlayerLeft (PlayerInput loadoutLayout)
    {
        Destroy(loadoutLayout.gameObject);
    }

    /// <summary>
    /// Enables the Weapon Selection Screen given the current Weapon slot
    /// </summary>
    /// <param name="weaponSlot"> Weapon Slot to modify </param>
    public void DisplayWeapons(string weaponSlot)
    {
        // Disable buttons
        backButton.interactable = false;
        startButton.interactable = false;

        int playerId = int.Parse(weaponSlot[0].ToString());
        char arm = weaponSlot[1];
        if (arm.Equals('B'))
        {
            currentArm[playerId] = "Back";
        }
        else if (arm.Equals('F'))
        {
            currentArm[playerId] = "Front";
        }
        currentWeapon[playerId] = weaponSlot[2];

        // Display Selection
        Transform current = transform.Find("Player Loadout " + playerId);
        current.Find("Loadouts").gameObject.SetActive(false);
        current.Find("Selection").gameObject.SetActive(true);

        // Set selected to scrollbar
        if (playerId != 0)
        {
            GameObject scrollbar = current.Find("Selection").Find("Scroll View").Find("Scrollbar Vertical").gameObject;
            current.transform.Find("Input Module & Multiplayer Holder").GetComponent<MultiplayerEventSystem>().SetSelectedGameObject(scrollbar);
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(current.Find("Selection").Find("Scroll View").Find("Scrollbar Vertical").gameObject);
        }

        // Update the display
        foreach (GameObject weaponButton in weaponButtons[playerId])
        {
            // If button not interactable
            if (!weaponButton.GetComponent<Button>().interactable)
            {
                // Check weapon for unlocked
                if (weaponButton.GetComponent<WeaponButtonContatiner>().weapon.unlocked)
                {
                    // Make interactable
                    weaponButton.GetComponent<Button>().interactable = true;
                }
            }

            // Disable buttons for weapons that you have equiped
            Weapon w = weaponButton.GetComponent<WeaponButtonContatiner>().weapon;
            int armIndex = -1;
            switch (currentArm[playerId])
            {
                case "Back":
                    armIndex = 0;
                    break;
                case "Front":
                    armIndex = 1;
                    break;
            }
            int weaponIndex = currentWeapon[playerId] - 65;
            string currName = WeaponManager.Instance().playerLoadouts[playerId][armIndex][weaponIndex].weaponName;
            foreach (Weapon wload in WeaponManager.Instance().playerLoadouts[playerId][armIndex])
            {
                if (!w.weaponName.Equals(currName) && wload.weaponName.Equals(w.weaponName))
                {
                    weaponButton.GetComponent<Button>().interactable = false;
                }
            }
        }

    }

    /// <summary>
    /// Selects the given weapon for the loadout based on currently selected
    /// </summary>
    /// <param name="playerId"> Id of the Player choosing a weapon </param>
    /// <param name="weapon"> Weapon selected </param>
    public void ChooseWeapon(int playerId, Weapon weapon)
    {
        Debug.Log("In ChooseWeapon for Player " + playerId + "\'s " + currentArm[playerId] + "\'s weapon " + currentWeapon + " to have weapon " + weapon.name);
        Transform current = transform.Find("Player Loadout " + playerId);

        // Change Loadout
        int index = currentWeapon[playerId] - 65;
        if (currentArm[playerId].Equals("Back"))
        {
            WeaponManager.Instance().playerLoadouts[playerId][0][index] = weapon;
        }
        else if (currentArm[playerId].Equals("Front"))
        {
            WeaponManager.Instance().playerLoadouts[playerId][1][index] = weapon;
        }

        // descriptionBox.SetActive(false);

        // Save loadout
        WeaponManager.Instance().SaveLoadout();

        // Update Loadouts
        Transform back = current.Find("Loadouts").Find("Back Arm Loadout");
        Transform front = current.Find("Loadouts").Find("Front Arm Loadout");
        for (int i = 0; i < WeaponManager.Instance().playerLoadouts[playerId][0].Length; i++)
        {
            back.GetChild(i + 1).Find("Image").GetComponent<Image>().sprite = WeaponManager.Instance().playerLoadouts[playerId][0][i].icon;
            front.GetChild(i + 1).Find("Image").GetComponent<Image>().sprite = WeaponManager.Instance().playerLoadouts[playerId][1][i].icon;
        }

        // Go back to Loadouts
        current.Find("Loadouts").gameObject.SetActive(true);
        current.Find("Selection").gameObject.SetActive(false);

        // Set selected to Weapon A
        if (playerId != 0)
        {
            GameObject wA = current.Find("Loadouts").Find("Back Arm Loadout").Find("Weapon A").gameObject;
            current.transform.Find("Input Module & Multiplayer Holder").GetComponent<MultiplayerEventSystem>().SetSelectedGameObject(wA);
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(current.Find("Loadouts").Find("Back Arm Loadout").Find("Weapon A").gameObject);
        }

        // Enable Buttons
        backButton.interactable = true;
        startButton.interactable = true;
    }

/*    void FixedUpdate()
    {

    }*/
}
