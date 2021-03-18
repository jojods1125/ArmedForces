using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class GameManager : NetworkBehaviour
{
    public GameObject spawnPointContainer;

    [Min(0)]
    public int playerRespawnTimer = 3;

    public Player mainPlayer;

    public UIManager uiManager;

    private List<Vector3> spawnPoints = new List<Vector3>();


    private int playerCount = 0;

    [SyncVar]
    private int gameTime = 0;

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
        obj.GetComponent<Player>().Respawn(spawnPoints[Random.Range(0, spawnPoints.Count - 1)]);
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
        if (!isServer)
            return;

        InvokeRepeating("incrementTime", 0, 1);
    }

    public int getID(){
        GameManager.Instance().playerCount++;
        GameManager.Instance().kills.Add(playerCount, 0);
        GameManager.Instance().deaths.Add(playerCount, 0);
        // What is the playerCount
        // Debug.Log(playerCount);
        return playerCount;
    }

    void incrementTime(){
        gameTime++;
        ServerMessage("Game Time is " + gameTime);
    }


    public void trackDeath(int killer, int deceased){
        // Who killed who
        // Debug.Log("Killer: " + killer + ", Deceased: " + deceased);
        deaths[deceased]++;
        if(killer != deceased && killer != -1){
            kills[killer]++;
        }

        ServerMessage("Player " + killer + " killed Player " + deceased + ". Player " + killer + "'s score is " + kills[killer]);
        // Check counts
        /*Debug.Log("Deaths: " + deaths[killer]);
        Debug.Log("Kills: " + kills[killer]);*/
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
