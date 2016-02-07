using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System.Collections;

public class ObjectSpawner : NetworkBehaviour {

    bool initialSpawn = false;
    GameObject[] players;

	// Use this for initialization
	void Start () {
        players = GameObject.FindGameObjectsWithTag("Player");
	}

    // Update is called once per frame
    void Update() {
        if (!initialSpawn) { 
            players = GameObject.FindGameObjectsWithTag("Player");
        }
        if (!initialSpawn && players.Length != 0 && !initialSpawn)
            initialSpawns();
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
                if (isServer && GameObject.FindGameObjectsWithTag("Monster").Length == 0)
                {
                    Debug.Log("Spawning monster from ObjectSpawner!");
                    GameObject monsterPrefab = Resources.Load("Prefabs/Monster") as GameObject;
                    GameObject monster = (GameObject)Instantiate(monsterPrefab, new Vector3(50, 5, 50), new Quaternion());
                    NetworkServer.Spawn(monster);
                }
                    Debug.Log("Tryna make a flashlight");
                    GameObject flashlightPrefab = Resources.Load("Prefabs/FlashLightInactive") as GameObject;
                    GameObject flashLight = (GameObject)Instantiate(flashlightPrefab, new Vector3(Random.Range(-15, 15), 2, Random.Range(-15, 15)), new Quaternion());
                    NetworkServer.Spawn(flashLight);
            }
        }
    }


}
