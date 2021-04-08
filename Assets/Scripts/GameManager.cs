using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public enum MatchType
{
    Training,
    Online,
    Local
}

public class GameManager : MonoBehaviour
{
    [Header("Match Information")]

    [Tooltip("Type of match, affects instantiation")]
    public MatchType matchType;
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
    //[Tooltip("Local player within the scene [DO NOT SET IN EDITOR IN ONLINE SCENES]")]
    //[SerializeField]
    //public Player localPlayer;
    [Tooltip("Local players within the scene [ONLY SET IN LOCAL MULTIPLAYER]")]
    public Player[] localPlayers = new Player[4];

    public Player ai;
    private AI_Controller aiC;


    /// <summary> List of spawn point locations </summary>
    private List<Vector3> spawnPoints = new List<Vector3>();
    /// <summary> Number of players in the match </summary>
    private int playerCount = 0;

    /// <summary> Current number of seconds since match began </summary>
    private float currentGameTime = 0;

    /// <summary> Dictionary of player IDs to kills </summary>
    private Dictionary<int,int> kills = new Dictionary<int, int>();
    /// <summary> Dictionary of player IDs to deaths </summary>
    private Dictionary<int,int> deaths = new Dictionary<int, int>();



    // ===========================================================
    //                           START-UP
    // ===========================================================

    //Returns the closest spawn point to a position
    public Vector3 getCloseRespawnPoint(Vector3 pos)
    {
        Vector3 closestPoint = spawnPoints[0];
        float closestDist = int.MaxValue;
        foreach (Vector3 spawnPoint in spawnPoints){
            if(Vector3.Distance(pos, spawnPoint) < closestDist){
                closestPoint = spawnPoint;
                closestDist = Vector3.Distance(pos, spawnPoint);
            }
        }
        return closestPoint;
    }
       

    public Vector3 getRandomRespawnPoint()
    {
        return spawnPoints[Random.Range(0, spawnPoints.Count - 1)];
    }

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
        // Debug.LogError("I SHOULD BE FIRST");
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

        if (matchType == MatchType.Training)
        {
            ai.Activate();
            aiC = ai.GetComponent<AI_Controller>();
        }
        
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
        Debug.Log("Client Connected executed");

        // Retrieves the ID
        int newID = getID();

        Debug.Log("New ID is " + newID);

        // Tells other GameManagers that a player connected
        RpcClientConnected(newID);

        // Adds new key to kills and deaths dictionaries
        kills.Add(newID, 0);
        deaths.Add(newID, 0);

        uiManager.ui_Players[newID].gameObject.SetActive(true);

        // Gives ID to Player
        return newID;
    }

    public int AIConnected()
    {
        // Debug.LogError("CALLING AIID");
        // Retrieves the ID
        int newID = getAIID();
        // Debug.LogError(newID);

        // Tells other GameManagers that a player connected
        //RpcClientConnected(newID);

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
    public void RpcClientConnected(int newID)
    {
        // Refreshes arms so new player sees them
        //localPlayer.UpdateAppearance();

        if (matchType == MatchType.Training)
            aiC.Activate();
    }


    /// <summary>
    /// Updates playerCount on the server and retrieves it as the ID
    /// </summary>
    /// <returns> New ID for the player </returns>
    public int getID()
    {
        int newID = playerCount;
        playerCount++;
        return newID;
    }

    public int getAIID()
    {
        // Debug.LogError("IN GETAIID");
        playerCount++;
        return playerCount;
    }


    // ===========================================================
    //                     TIME AND UI UPDATES
    // ===========================================================

    /// <summary>
    /// Increments time
    /// </summary>
    public void IncrementTime()
    {
        if (matchType != MatchType.Online)
            currentGameTime += serverGameTimeStep;

        // Formats minutes and seconds remaining
        int mins = Mathf.Max((totalGameTime - (int)currentGameTime) / 60, 0);
        int secs = Mathf.Max((totalGameTime - (int)currentGameTime) % 60, 0);

        // Updates clock UI
        uiManager.matchClock.GetComponent<TMP_Text>().text = mins.ToString("D2") + ":" + secs.ToString("D2");

        /// TODO: ADD FUNCTIONALITY FOR ROUND ENDING
        // Server notification if round ends
        if (currentGameTime >= totalGameTime)
        {
            ServerMessage("ROUND ENDED");
        }
    }


    private void Update()
    {
        // Player score UI updates

        if (kills.ContainsKey(0) && deaths.ContainsKey(0))
            uiManager.p1_KDR.GetComponent<TMP_Text>().text = "P1 - K: " + kills[0] + " D: " + deaths[0];

        if (kills.ContainsKey(1) && deaths.ContainsKey(1))
            uiManager.p2_KDR.GetComponent<TMP_Text>().text = "P2 - K: " + kills[1] + " D: " + deaths[1];

        if (kills.ContainsKey(2) && deaths.ContainsKey(2))
            uiManager.p3_KDR.GetComponent<TMP_Text>().text = "P3 - K: " + kills[2] + " D: " + deaths[2];

        if (kills.ContainsKey(3) && deaths.ContainsKey(3))
            uiManager.p4_KDR.GetComponent<TMP_Text>().text = "P4 - K: " + kills[3] + " D: " + deaths[3];
    }



    // ===========================================================
    //                      DEATH AND REBIRTH
    // ===========================================================

    /// <summary>
    /// Updates the server's kills and deaths dictionaries
    /// </summary>
    /// <param name="killer"> ID of the player that killed </param>
    /// <param name="deceased"> ID of the player that died </param>
    /// /// <param name="weaponType"> Type of weapon that killed </param>
    public void TrackDeath(int killer, int deceased, WeaponType weaponType)
    {
        // Updates the deaths and kills for the deceased and killer
        deaths[deceased]++;
        if (killer != deceased && killer != -1)
        {
            kills[killer]++;
            // if the killer is the mainPlayer
            if (matchType != MatchType.Local && killer == localPlayers[0].GetPlayerID())
            {
                AchievementManager.Instance().OnEvent(AchievementType.kills, 1, weaponType);
            }
            else if (matchType == MatchType.Local)
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
        if (obj.GetComponent<Player_Networked>() != null)
            obj.GetComponent<Player_Networked>().Respawn(spawnPoints[Random.Range(0, spawnPoints.Count)]);
        else if (obj.GetComponent<Player>() != null)
            obj.GetComponent<Player>().Respawn(spawnPoints[Random.Range(0, spawnPoints.Count)]);
        //else if (obj.GetComponent<Player_AI>() != null)
        //    obj.GetComponent<Player_AI>().Respawn(spawnPoints[Random.Range(0, spawnPoints.Count)]);
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
        RpcServerMessage(msg);
    }


    /// <summary>
    /// Send a message across all clients
    /// </summary>
    /// <param name="msg"> Message to send </param>
    private void RpcServerMessage(string msg)
    {
        Debug.Log("SERVER MSG: " + msg);
    }


}
