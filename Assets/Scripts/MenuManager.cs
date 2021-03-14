using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
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
        SceneManager.LoadScene( "L_LoadoutMenu" );
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
        SceneManager.LoadScene( "L_MainMenu" );
	}
}
