using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class ObjectSpawner : NetworkBehaviour {

    // Max spawn limits placed here
    public static int MAX_SPAWNED_BATTERIES = 40;

    private bool initialSpawn = false;
    private int tick = 0;
    private HashSet<GameObject> players;
    private GameObject batteryPrefab;
    private GameObject flashlightPrefab;
    private GameObject monsterPrefab;

    private LinkedList<GameObject> batteries;

    // Use this for initialization
    void Start () {
        players = new HashSet<GameObject>();
        batteries = new LinkedList<GameObject>();
        loadResources();
	}

    // Update is called once per frame
    void Update() {

        if (!initialSpawn)
            initialSpawns();

        // Update tick
        tick += 1;
        if (tick > int.MaxValue-1)
            tick = 0;

        // Check for new players to spawn essentials for every half second @ 60FPS
        if (tick % 30 == 0)
            checkForNewPlayers();

        // Every 40th seconds @ 60 FPS for each player (2 players = 1 per 20s, 3 players = 1 per 13s)
        if (players.Count > 0 && tick % (4800 / players.Count) == 0)
            spawnBattery();
    }

    void loadResources()
    {
         batteryPrefab = Resources.Load("Prefabs/Batteries") as GameObject;
         flashlightPrefab = Resources.Load("Prefabs/FlashLightInactive") as GameObject;
         monsterPrefab = Resources.Load("Prefabs/Monster") as GameObject;
    }

    void checkForNewPlayers()
    {
        GameObject[] playersScraped = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in playersScraped)
        {
            if (!players.Contains(player))
            {
                Debug.Log("Discovered new player! there are now " + players.Count + " in the game");
                players.Add(player);
                spawnEssentialsNear(player);
            }
        }
    }

    void spawnEssentialsNear(GameObject player)
    {
        // Spawn a flashlight within 21.2 units of player
        GameObject flashLight = (GameObject)Instantiate(flashlightPrefab, 
                                                        new Vector3(player.transform.position.x + Random.Range(-15, 15),
                                                                    player.transform.position.y+1,
                                                                    player.transform.position.z + Random.Range(-15, 15)),
                                                        new Quaternion());
        NetworkServer.Spawn(flashLight);
    }

    void initialSpawns()
    {
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("MainGame") && GameObject.FindGameObjectsWithTag("Player").Length != 0)
        {
            initialSpawn = true;
            if (GameObject.FindGameObjectsWithTag("Monster").Length == 0)
            {
                Debug.Log("Spawning monster from ObjectSpawner!");
                GameObject monster = (GameObject)Instantiate(monsterPrefab, new Vector3(50, 5, 50), new Quaternion());
                NetworkServer.Spawn(monster);
            }
        }
    }

    void spawnBattery()
    {
        // TODO Definitely too small of a spawn range...
        GameObject batteryToSpawn = (GameObject)Instantiate(batteryPrefab, new Vector3(Random.Range(-20, 220), 10, Random.Range(-5, 120)), new Quaternion());
        batteries.AddLast(batteryToSpawn);
        if (batteries.Count > MAX_SPAWNED_BATTERIES)
        {
            GameObject batteryToDestroy = batteries.First.Value;
            batteries.RemoveFirst();
            Destroy(batteryToDestroy);
        }
        NetworkServer.Spawn(batteryToSpawn);
        Debug.Log("Battery spawned at " + batteryToSpawn.transform.position);
    }
}
