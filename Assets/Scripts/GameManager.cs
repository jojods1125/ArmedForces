using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject spawnPointContainer;

    [Min(0)]
    public int playerRespawnTimer = 3;

    public Player mainPlayer;

    public UIManager uiManager;

    private List<Vector3> spawnPoints = new List<Vector3>();

    private int playerCount = 0;

    private int gameTime = 0;

    private Dictionary<int,int> kills;

    private Dictionary<int,int> deaths;



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

        InvokeRepeating("incrementTime", 0, 1);
        kills = new Dictionary<int, int>();
        deaths = new Dictionary<int, int>();
    }

    public int getID(){
        GameManager.Instance().playerCount++;
        GameManager.Instance().kills.Add(playerCount, 0);
        GameManager.Instance().deaths.Add(playerCount, 0);
        return playerCount;
    }

    void incrementTime(){
        gameTime++;
    }

    public void trackDeath(int killer, int deceased){
        deaths[deceased]++;
        if(killer != deceased && killer != -1){
            kills[killer]++;
        }
    }


}
