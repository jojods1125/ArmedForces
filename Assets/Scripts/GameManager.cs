﻿using System.Collections;
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

    //Returns the closest spawn point to a position
    public Vector3 getCloseRespawnPoint(Vector3 pos)
    {
        Vector3 closestPoint = spawnPoints[0];
        float closestDist = int.MaxValue;
        foreach (Vector3 spawnPoint in spawnPoints){
            if(Vector3.Distance(pos, spawnPoint) < closestDist){
                closestDist = Vector3.Distance(pos, spawnPoint);
                closestPoint = spawnPoint;
            }
        }
        return closestPoint;
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
        // What is the playerCount
        // Debug.Log(playerCount);
        return playerCount;
    }

    void incrementTime(){
        gameTime++;
    }

    public void trackDeath(int killer, int deceased, WeaponType weaponType){
        // Who killed who
        // Debug.Log("Killer: " + killer + ", Deceased: " + deceased);
        deaths[deceased]++;
        if(killer != deceased && killer != -1) 
        {
            kills[killer]++;
            // if the killer is the mainPlayer
            if (killer == mainPlayer.playerID)
            {
                AchievementManager.Instance().OnEvent(AchievementType.kills, 1, weaponType);
            }
        }

        // Check counts
        /*Debug.Log("Deaths: " + deaths[killer]);
        Debug.Log("Kills: " + kills[killer]);*/
    }


}
