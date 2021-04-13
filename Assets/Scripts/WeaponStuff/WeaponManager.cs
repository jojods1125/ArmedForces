using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    // ===========================================================
    //                    SINGLETON PATTERN
    // ===========================================================
    private static WeaponManager instance;
    public static WeaponManager Instance()
    {
        return instance;
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    // ===========================================================
    //                  Accessing and Saving
    // ===========================================================

    public List<Weapon> weapons;
    public Weapon[] defaultBack = new Weapon[4];
    public Weapon[] defaultFront = new Weapon[4];

    // Dictionary of playerId to loadout?
    public Dictionary<int, Weapon[][]> playerLoadouts = new Dictionary<int, Weapon[][]>();


    /// <summary>
    /// Gets a weapon from the existing list given a name
    /// </summary>
    /// <param name="name"> Name of the Weapon to find </param>
    /// <returns> Weapon with given name, null otherwise </returns>
    public Weapon getByName( string name )
	{
        foreach (Weapon w in weapons)
		{
            if (w.weaponName.Equals(name))
			{
                return w;
			}
		}
        return null;
	}

    /// <summary>
    /// Saves the Loadout into PlayerPrefs
    /// </summary>
    public void SaveLoadout()
	{
        // Weapon slot
        char slot = 'A';

        for (int playerId = 0; playerId < playerLoadouts.Count; playerId++)
        {
            // Rest slot
            slot = 'A';

            Weapon[] currBack = playerLoadouts[playerId][0];
            // Go through back loadout
            foreach (Weapon w in currBack)
            {
                PlayerPrefs.SetString(playerId + "Back" + slot++, w.weaponName);
            }

            // Reset slot
            slot = 'A';

            Weapon[] currFront = playerLoadouts[playerId][1];
            // Go through front loadout
            foreach (Weapon w in currFront)
            {
                PlayerPrefs.SetString(playerId + "Front" + slot++, w.weaponName);
            }
        }
    }

    /// <summary>
    /// Loads the Loadouts into the corresponding arms from PlayerPrefs
    /// </summary>
    public void LoadLoadout()
	{
        // Weapon slot
        char slot = 'A';

        for (int playerId = 0; playerId < playerLoadouts.Count; playerId++)
        {
            Weapon[] currBack = playerLoadouts[playerId][0];
            // Go through back loadout
            for (int i = 0; i < currBack.Length; i++)
            {
                string file = playerId + "Back" + (char)(slot + i);
                if (PlayerPrefs.HasKey(file))
                {
                    string data = PlayerPrefs.GetString(playerId + "Back" + (char)(slot + i));
                    currBack[i] = getByName(data);
                }
            }

            Weapon[] currFront = playerLoadouts[playerId][1];
            // Go through front loadout
            for (int i = 0; i < currFront.Length; i++)
            {
                string file = playerId + "Front" + (char)(slot + i);
                if (PlayerPrefs.HasKey(file))
                {
                    string data = PlayerPrefs.GetString(playerId + "Front" + (char)(slot + i));
                    currFront[i] = getByName(data);
                }
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        /*// Create 4 players with base loadout
        Weapon[][] loadout = { (Weapon[])defaultBack.Clone(), (Weapon[])defaultFront.Clone() };
        playerLoadouts = new Dictionary<int, Weapon[][]>();
        // Player 1
        playerLoadouts.Add(0, (Weapon[][])loadout.Clone());
        // Player 2
        playerLoadouts.Add(1, (Weapon[][])loadout.Clone());
        // Player 3
        playerLoadouts.Add(2, (Weapon[][])loadout.Clone());
        // Player 4
        playerLoadouts.Add(3, (Weapon[][])loadout.Clone());*/
        
        // for all 4 possible players...
        for (int i = 0; i < 4; i++)
        {
            playerLoadouts.Add(i, new Weapon[2][]);
            // for back and front loadouts
            for (int j = 0; j < 2; j++)
            {
                playerLoadouts[i][j] = new Weapon[4];
                // for each of the 4 weapons
                for (int k = 0; k < 4; k++)
                {
                    playerLoadouts[i][j][k] = getByName("Machine Gun");
                }
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
