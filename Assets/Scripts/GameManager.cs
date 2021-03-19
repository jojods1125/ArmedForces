using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;


public class GameManager : NetworkBehaviour
{
    public GameObject spawnPointContainer;

    [Min(0)]
    public int playerRespawnTimer = 3;

    [Min(0)]
    public int startingGameTime = 180;

    [Range(0, 1)]
    public float serverGameTimeStep = 0.25f;

    public Player mainPlayer;

    public UIManager uiManager;

    private List<Vector3> spawnPoints = new List<Vector3>();


    private int playerCount = 0;

    [SyncVar]
    private float currentGameTime = 0;

    private Dictionary<int,int> kills = new Dictionary<int, int>();

    private Dictionary<int,int> deaths = new Dictionary<int, int>();


    public void Respawn(GameObject obj)
    {
        if (obj.layer == LayerMask.NameToLayer("Player"))
        {
            StartCoroutine(RespawnPlayer(obj));
        }
    }


    public IEnumerator RespawnPlayer(GameObject obj)
    {
        yield return new WaitForSeconds(playerRespawnTimer);
        obj.SetActive(true);
        obj.GetComponent<Player>().Respawn(spawnPoints[Random.Range(0, spawnPoints.Count)]);
    }


    private static GameManager instance;

    public static GameManager Instance()
    {
        return instance;
    }


    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }

        foreach (Transform child in spawnPointContainer.transform)
        {
            spawnPoints.Add(child.position);
        }
    }


    private void Start()
    {
        InvokeRepeating("IncrementTime", 0, serverGameTimeStep);

        if (!isServer)
            return;
    }

    [Command]
    public void CmdClientConnected()
    {
        //mainPlayer.
        //RpcDisplayTime(gameTime);
    }

    public int getID(){
        GameManager.Instance().playerCount++;
        GameManager.Instance().kills.Add(playerCount, 0);
        GameManager.Instance().deaths.Add(playerCount, 0);
        // What is the playerCount
        // Debug.Log(playerCount);
        return playerCount;
    }


    void IncrementTime(){
        
        if (isServer)
        {
            currentGameTime += serverGameTimeStep;
        }

        int mins = Mathf.Max((startingGameTime - (int)currentGameTime) / 60, 0);
        int secs = Mathf.Max((startingGameTime - (int)currentGameTime) % 60, 0);

        uiManager.matchClock.GetComponent<TMP_Text>().text = mins.ToString("D2") + ":" + secs.ToString("D2");


        if (isServer && currentGameTime >= startingGameTime)
        {
            ServerMessage("ROUND ENDED");
        }
    }










    public void TrackDeath(int killer, int deceased)
    {
        CmdTrackDeath(killer, deceased);

        ServerMessage("Player " + killer + " killed Player " + deceased + ". Player " + killer + "'s score is " + kills[killer]);
    }

    //[Command]
    public void CmdTrackDeath(int killer, int deceased)
    {
        Debug.LogError("I TRIED TO TRACK THE DEATH AS A COMMAND");

        // Who killed who
        // Debug.Log("Killer: " + killer + ", Deceased: " + deceased);
        deaths[deceased]++;
        if(killer != deceased && killer != -1){
            kills[killer]++;
        }

        RpcUpdateKDR(killer, kills[killer], deceased, deaths[deceased]);

        // Check counts
        /*Debug.Log("Deaths: " + deaths[killer]);
        Debug.Log("Kills: " + kills[killer]);*/
    }


    [ClientRpc]
    public void RpcUpdateKDR(int killer, int numKills, int deceased, int numDeaths)
    {
        kills[killer] = numKills;
        deaths[deceased] = numDeaths;

        if (killer == 1 || deceased == 1)
        {
            uiManager.p1_KDR.GetComponent<TMP_Text>().text = "1 - K: " + kills[1] + " D: " + deaths[1];
        }

        if (killer == 2 || deceased == 2)
        {
            uiManager.p2_KDR.GetComponent<TMP_Text>().text = "2 - K: " + kills[2] + " D: " + deaths[2];
        }

        if (killer == 3 || deceased == 3)
        {
            uiManager.p3_KDR.GetComponent<TMP_Text>().text = "3 - K: " + kills[3] + " D: " + deaths[3];
        }

        if (killer == 4 || deceased == 4)
        {
            uiManager.p4_KDR.GetComponent<TMP_Text>().text = "4 - K: " + kills[4] + " D: " + deaths[4];
        }
    }


    [Command]
    public void CmdUpdateAllKDR()
    {
        int[] killsArray = new int[playerCount];
        int[] deathsArray = new int[playerCount];

        for (int id = 1; id <= playerCount; id++)
        {
            killsArray[id - 1] = kills[id];
            deathsArray[id - 1] = deaths[id];
        }

        RpcUpdateAllKDR(killsArray, deathsArray);
    }

    [ClientRpc]
    public void RpcUpdateAllKDR(int[] killsArray, int[] deathsArray)
    {
        for (int i = 0; i < killsArray.Length; i++)
        {
            kills[i + 1] = killsArray[i];
        }

        for (int i = 0; i < deathsArray.Length; i++)
        {
            deaths[i + 1] = deathsArray[i];
        }
    }


    public void ServerMessage(string msg) //this will be serverside
    {
        if (!isServer)
            return;

        RpcServerMessage(msg);
    }

    [ClientRpc]
    private void RpcServerMessage(string msg)//this will be clientside
    {
        Debug.LogError("SERVER MSG: " + msg);
    }
}
