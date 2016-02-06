using UnityEngine;
using System.Collections;

public class MonsterAI : MonoBehaviour {

	public Transform target;
	private NavMeshAgent agent;

	// Use this for initialization
	void Start () {
		agent = GetComponent<NavMeshAgent>();
	}
	
	// Update is called once per frame
	void Update () {
		agent.SetDestination(target.transform.position);
	}

	//Updates the Monster's current target
	void setTarget(Transform t) {

	}
}
