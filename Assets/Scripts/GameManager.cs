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

    public Player localPlayer;

    public UIManager uiManager;

    private List<Vector3> spawnPoints = new List<Vector3>();


    private int playerCount = 0;

    [SyncVar]
    private float currentGameTime = 0;


    private SyncDictionary<int,int> kills = new SyncDictionary<int, int>();

    private SyncDictionary<int,int> deaths = new SyncDictionary<int, int>();


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
        InvokeRepeating(nameof(IncrementTime), 0, serverGameTimeStep);
    }


    public int ClientConnected()
    {
        // THIS RUNS ON SERVER

        int newID = getID();

        RpcClientConnected(newID);

        kills.Add(newID, 0);
        deaths.Add(newID, 0);

        return newID;
    }

    [ClientRpc]
    public void RpcClientConnected(int newID)
    {
        Debug.LogError("RPC: Client Connected");

        localPlayer.UpdateAppearance();
    }

    public int getID()
    {
        // THIS RUNS ON SERVER

        playerCount++;

        // What is the playerCount
        // Debug.Log(playerCount);
        return playerCount;
    }


    void IncrementTime()
    {
        
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


    private void Update()
    {
        if (kills.ContainsKey(1) && deaths.ContainsKey(1))
            uiManager.p1_KDR.GetComponent<TMP_Text>().text = "1 - K: " + kills[1] + " D: " + deaths[1];

        if (kills.ContainsKey(2) && deaths.ContainsKey(2))
            uiManager.p2_KDR.GetComponent<TMP_Text>().text = "2 - K: " + kills[2] + " D: " + deaths[2];

        if (kills.ContainsKey(3) && deaths.ContainsKey(3))
            uiManager.p3_KDR.GetComponent<TMP_Text>().text = "3 - K: " + kills[3] + " D: " + deaths[3];

        if (kills.ContainsKey(4) && deaths.ContainsKey(4))
            uiManager.p4_KDR.GetComponent<TMP_Text>().text = "4 - K: " + kills[4] + " D: " + deaths[4];
    }


    public void TrackDeath(int killer, int deceased)
    {
        if (!isServer)
            return;

        deaths[deceased]++;
        if (killer != deceased && killer != -1)
        {
            kills[killer]++;
        }



        ServerMessage("Player " + killer + " killed Player " + deceased + ". Player " + killer + "'s score is " + kills[killer]);
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
