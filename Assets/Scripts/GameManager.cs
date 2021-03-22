using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;


public class GameManager : NetworkBehaviour
{
    [Header("Match Information")]

    [Tooltip("Number of seconds before respawn after player dies")] [Min(0)] 
    public int playerRespawnTime = 3;
    [Tooltip("Number of seconds the match will last for")] [Min(0)]
    public int totalGameTime = 180;
    [Tooltip("Number of seconds between UI server refreshes")][Range(0, 1)]
    public float serverGameTimeStep = 0.25f;


    [Header("Scene References")]

    [Tooltip("Parent GameObject for all spawn point GameObjects")]
    public GameObject spawnPointContainer;
    [Tooltip("UI Manager within the scene")]
    public UIManager uiManager;
    [Tooltip("Local player within the scene [DO NOT SET IN EDITOR IN ONLINE SCENES]")]
    public Player localPlayer;


    /// <summary> List of spawn point locations </summary>
    private List<Vector3> spawnPoints = new List<Vector3>();
    /// <summary> Number of players in the match </summary>
    private int playerCount = 0;

    /// <summary> Current number of seconds since match began </summary>
    [SyncVar]
    private float currentGameTime = 0;

    /// <summary> Dictionary of player IDs to kills </summary>
    private SyncDictionary<int,int> kills = new SyncDictionary<int, int>();
    /// <summary> Dictionary of player IDs to deaths </summary>
    private SyncDictionary<int,int> deaths = new SyncDictionary<int, int>();



    // ===========================================================
    //                           START-UP
    // ===========================================================

    /// <summary> Singleton instance </summary>
    private static GameManager instance;

    /// <summary>
    /// Retrieve singleton instance
    /// </summary>
    /// <returns> Instance of GameManager </returns>
    public static GameManager Instance()
    {
        return instance;
    }


    void Awake()
    {
        // Set singleton
        if (instance != null && instance != this)
            Destroy(this.gameObject);
        else
            instance = this;

        // Set up spawn points
        foreach (Transform child in spawnPointContainer.transform)
        {
            spawnPoints.Add(child.position);
        }
    }


    private void Start()
    {
        // Increase game time every serverGameTimeStep
        InvokeRepeating(nameof(IncrementTime), 0, serverGameTimeStep);
    }



    // ===========================================================
    //                      CLIENT CONNECTIONS
    // ===========================================================

    /// <summary>
    /// Called on server when a player connects to the game
    /// </summary>
    /// <returns> New ID for the player </returns>
    public int ClientConnected()
    {
        // THIS RUNS ON SERVER
        if (!isServer)
            return -1;

        // Retrieves the ID
        int newID = getID();

        // Tells other GameManagers that a player connected
        RpcClientConnected(newID);

        // Adds new key to kills and deaths dictionaries
        kills.Add(newID, 0);
        deaths.Add(newID, 0);

        // Gives ID to Player
        return newID;
    }


    /// <summary>
    /// Tells every instance of GameManager that a player connected
    /// </summary>
    /// <param name="newID"> ID of the new player </param>
    [ClientRpc]
    public void RpcClientConnected(int newID)
    {
        // Refreshes arms so new player sees them
        localPlayer.UpdateAppearance();
    }


    /// <summary>
    /// Updates playerCount on the server and retrieves it as the ID
    /// </summary>
    /// <returns> New ID for the player </returns>
    int getID()
    {
        // THIS RUNS ON SERVER
        if (!isServer)
            return -1;

        playerCount++;
        return playerCount;
    }



    // ===========================================================
    //                     TIME AND UI UPDATES
    // ===========================================================

    /// <summary>
    /// Increments time
    /// </summary>
    void IncrementTime()
    {
        // Increases the server's time by serverGameTimeStep, which is a SyncVar
        if (isServer)
        {
            currentGameTime += serverGameTimeStep;
        }

        // Formats minutes and seconds remaining
        int mins = Mathf.Max((totalGameTime - (int)currentGameTime) / 60, 0);
        int secs = Mathf.Max((totalGameTime - (int)currentGameTime) % 60, 0);

        // Updates clock UI
        uiManager.matchClock.GetComponent<TMP_Text>().text = mins.ToString("D2") + ":" + secs.ToString("D2");

        /// TODO: ADD FUNCTIONALITY FOR ROUND ENDING
        // Server notification if round ends
        if (isServer && currentGameTime >= totalGameTime)
        {
            ServerMessage("ROUND ENDED");
        }
    }


    private void Update()
    {
        // Player score UI updates

        if (kills.ContainsKey(1) && deaths.ContainsKey(1))
            uiManager.p1_KDR.GetComponent<TMP_Text>().text = "P1 - K: " + kills[1] + " D: " + deaths[1];

        if (kills.ContainsKey(2) && deaths.ContainsKey(2))
            uiManager.p2_KDR.GetComponent<TMP_Text>().text = "P2 - K: " + kills[2] + " D: " + deaths[2];

        if (kills.ContainsKey(3) && deaths.ContainsKey(3))
            uiManager.p3_KDR.GetComponent<TMP_Text>().text = "P3 - K: " + kills[3] + " D: " + deaths[3];

        if (kills.ContainsKey(4) && deaths.ContainsKey(4))
            uiManager.p4_KDR.GetComponent<TMP_Text>().text = "P4 - K: " + kills[4] + " D: " + deaths[4];
    }



    // ===========================================================
    //                      DEATH AND REBIRTH
    // ===========================================================

    /// <summary>
    /// Updates the server's kills and deaths dictionaries
    /// </summary>
    /// <param name="killer">  </param>
    /// <param name="deceased"></param>
    public void TrackDeath(int killer, int deceased)
    {
        // THIS RUNS ON SERVER
        if (!isServer)
            return;

        // Updates the deaths and kills for the deceased and killer
        deaths[deceased]++;
        if (killer != deceased && killer != -1)
        {
            kills[killer]++;
            // if the killer is the mainPlayer
            if (killer == mainPlayer.playerID)
            {
                AchievementManager.Instance().OnEvent(AchievementType.kills, 1, weaponType);
            }
        }

        /// TODO: MAKE THIS WHEN THE UI GETS UPDATED (?)  ==> ALSO UPDATE ON CONNECTION IF DOING THIS <==
        // Sends a server-wide message about the kill
        ServerMessage("Player " + killer + " killed Player " + deceased);
    }


    /// <summary>
    /// Respawn the specified GameObject
    /// </summary>
    /// <param name="obj"> GameObject to respawn </param>
    public void Respawn(GameObject obj)
    {
        if (obj.layer == LayerMask.NameToLayer("Player"))
        {
            StartCoroutine(RespawnPlayer(obj));
        }
    }


    /// <summary>
    /// Respawn the player with a playerRespawnTimer delay
    /// </summary>
    /// <param name="obj"> Player GameObject to respawn </param>
    /// <returns> Respawn delay </returns>
    public IEnumerator RespawnPlayer(GameObject obj)
    {
        yield return new WaitForSeconds(playerRespawnTime);
        obj.SetActive(true);
        obj.GetComponent<Player>().Respawn(spawnPoints[Random.Range(0, spawnPoints.Count)]);
    }



    // ===========================================================
    //                       SERVER MESSAGING
    // ===========================================================

    /// <summary>
    /// Tell the server to send a message across all clients
    /// </summary>
    /// <param name="msg"> Message to send </param>
    public void ServerMessage(string msg) //this will be serverside
    {
        if (!isServer)
            return;

        RpcServerMessage(msg);
    }


    /// <summary>
    /// Send a message across all clients
    /// </summary>
    /// <param name="msg"> Message to send </param>
    [ClientRpc]
    private void RpcServerMessage(string msg)
    {
        Debug.LogError("SERVER MSG: " + msg);
    }


}
