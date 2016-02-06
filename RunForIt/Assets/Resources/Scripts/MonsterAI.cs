using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
        Debug.Log(playerObjs.Length);
        playerLocs = new Transform[playerObjs.Length];
        updatePlayerLocs();
        Transform target = findClosestTransform();
        //Debug.Log(target);
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
        float min = 1000000000;
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
