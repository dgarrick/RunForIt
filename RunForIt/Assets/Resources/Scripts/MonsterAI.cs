using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class MonsterAI : MonoBehaviour {

	private NavMeshAgent agent;
    private Transform[] playerLocs;
    private GameObject[] playerObjs;



    // Use this for initialization
    void Start () {
		agent = GetComponent<NavMeshAgent>();


    }
	
	// Update is called once per frame
	void Update () {
        playerObjs = GameObject.FindGameObjectsWithTag("Player");
        playerLocs = new Transform[playerObjs.Length];
        //TODO change to 1 for actual game
        if (playerObjs.Length == 0)
        {
            SceneManager.LoadScene(1);
        }
        updatePlayerLocs();
        Transform target = findClosestTransform();
        agent.SetDestination(target.position);
	}

    void updatePlayerLocs()
    {
        for (int i = 0; i < playerObjs.Length; ++i)
        {
            playerLocs[i] = playerObjs[i].transform;
        }
    }

    Transform findClosestTransform()
    {
        float min = 10000000000000;
        Transform closest = null;
        foreach (Transform otherTrans in playerLocs)
        {
            float thisDist = Vector3.Distance(transform.position, otherTrans.position);
            if (thisDist < min) {
                min = thisDist;
                closest = otherTrans;
            }
        }
        return closest;
    }



    //Updates the Monster's current target
    void setTarget(Transform t) {

	}
}
