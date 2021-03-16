using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LobbyMenuManager : MonoBehaviour
{
    [Header("Menus to link to")]
    [Tooltip("Lobby Menu link")]
    public GameObject LobbyMenu;
    [Tooltip("Host Menu link")]
    public GameObject HostMenu;
    [Tooltip("Join Menu link")]
    public GameObject JoinMenu;
    [Tooltip("PreGame Menu link")]
    public GameObject PreGameMenu;

    [Header("First Selected Button for each Menu")]
    [Tooltip("First Selected Button on Lobby Menu")]
    public GameObject lobbyFirstButton;
    [Tooltip("First Selected Button on Host Menu")]
    public GameObject hostFirstButton;
    [Tooltip("First Selected Button on Join Menu")]
    public GameObject joinFirstButton;
    [Tooltip("First Selected Button on PreGame Menu")]
    public GameObject pregameFirstButton;

    [Header("Input Field")]
    [Tooltip("Server Code")]
    public GameObject serverInput;

    // Code to join the server
    private string serverCode = "";

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Starts the Server for others to join
    /// </summary>
    public void HostServer()
	{
        // Joseph's networking stuff
	}

    /// <summary>
    /// Assigns the server code based on the inputField
    /// </summary>
    /// <param name="code"> User typed code </param>
    public void InputServerCode()
	{
        serverCode = serverInput.GetComponent<Text>().text;
        // Debug.Log(serverCode);
	}

    /// <summary>
    /// Join the Server based on the server code
    /// </summary>
    public void JoinServer()
	{
        if (serverCode != null && serverCode != "")
		{
            // Joseph's networking stuff
            Debug.Log("Joingin Server");
		}
	}
}
