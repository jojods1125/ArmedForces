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
    //                    SINGLETON PATTERN
    // ===========================================================

    public List<Weapon> weapons;

    /// <summary>
    /// Gets a weapon from the existing list given a name
    /// </summary>
    /// <param name="name"> Name of the Weapon to find </param>
    /// <returns> Weapon with given name, null otherwise </returns>
    public Weapon getByName( string name )
	{
        foreach (Weapon w in weapons)
		{
            if (w.name.Equals(name))
			{
                return w;
			}
		}
        return null;
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
