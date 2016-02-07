﻿using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class ObjectSpawner : NetworkBehaviour {

    bool initialSpawn = false;
    int playerCount = 0;
    private int tick = 0;
    HashSet<GameObject> players;
    GameObject batteryPrefab = Resources.Load("Prefabs/Batteries") as GameObject;

    // Use this for initialization
    void Start () {
        players = new HashSet<GameObject>();
	}

    // Update is called once per frame
    void Update() {
        
        // Check for new players to spawn stuff for
        GameObject[] playersScraped = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject obj in playersScraped)
        {
            if (!players.Contains(obj))
            {
                Debug.Log("Discovered new player!");
                players.Add(obj);
                spawnEssentials(obj);
            }
        }

        // Update tick
        if (tick > 100000000)
            tick = 0;
        tick += 1;

        // Consider spawning batteries
        // Every 40th seconds @ 60 FPS for each player (2 players = 1 per 20s, 3 players = 1 per 13s)
        if (tick % (4800 / players.Count) == 0)
        {
            // TODO Definitely too small of a spawn range...
            GameObject batteryToSpawn = (GameObject)Instantiate(batteryPrefab, new Vector3(Random.Range(-20, 220), 10, Random.Range(-5, 120)), new Quaternion());
            NetworkServer.Spawn(batteryToSpawn);
            Debug.Log("Battery spawned at " + batteryToSpawn.transform.position);
        }
        


    if (players.Count != 0 && !initialSpawn)
            initialSpawns();
    }

    // Spawn Essentials for this player
    void spawnEssentials(GameObject player)
    {
        // Spawn a flashlight within 21.2 units of player
        GameObject flashlightPrefab = Resources.Load("Prefabs/FlashLightInactive") as GameObject;
        GameObject flashLight = (GameObject)Instantiate(flashlightPrefab, new Vector3(player.transform.position.x + Random.Range(-15, 15), player.transform.position.y+1, player.transform.position.z + Random.Range(-15, 15)), new Quaternion());
        NetworkServer.Spawn(flashLight);
    }

    void initialSpawns()
    {
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("MainGame") && GameObject.FindGameObjectsWithTag("Player").Length != 0)
        {
            if (!initialSpawn)
            {
                initialSpawn = true;
                Debug.Log("Spawning objects...");
                //Do Initial spawning
               if (GameObject.FindGameObjectsWithTag("Monster").Length == 0)
                {
                    Debug.Log("Spawning monster from ObjectSpawner!");
                    GameObject monsterPrefab = Resources.Load("Prefabs/Monster") as GameObject;
                    GameObject monster = (GameObject)Instantiate(monsterPrefab, new Vector3(50, 5, 50), new Quaternion());
                    NetworkServer.Spawn(monster);
                }
            }
        }
    }


}
