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
            DontDestroyOnLoad(gameObject);
        }
    }

    // ===========================================================
    //                  Accessing and Saving
    // ===========================================================

    public List<Weapon> weapons;
    public Weapon[] back = new Weapon[4];
    public Weapon[] front = new Weapon[4];

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

        // Go through back loadout
        foreach (Weapon w in back)
		{
            PlayerPrefs.SetString("Back" + slot++, w.weaponName);
		}

        // Reset slot
        slot = 'A';

        // Go through front loadout
        foreach (Weapon w in front)
        {
            PlayerPrefs.SetString("Front" + slot++, w.weaponName);
        }
    }

    public void LoadLoadout()
	{
        // Weapon slot
        char slot = 'A';

        // Go through back loadout
        for (int i = 0; i < back.Length; i++)
		{
            string file = "Back" + (char)(slot + i);
            if (PlayerPrefs.HasKey(file))
			{
                string data = PlayerPrefs.GetString("Back" + (char)(slot + i));
                back[i] = getByName(data);
			}
		}

        // Go through front loadout
        for (int i = 0; i < front.Length; i++)
        {
            string file = "Front" + (char)(slot + i);
            if (PlayerPrefs.HasKey(file))
            {
                string data = PlayerPrefs.GetString("Front" + (char)(slot + i));
                front[i] = getByName(data);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
