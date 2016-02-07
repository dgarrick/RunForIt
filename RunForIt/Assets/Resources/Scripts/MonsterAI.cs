using UnityEngine;

public class MonsterAI : MonoBehaviour {

	private NavMeshAgent agent;
    private GameObject[] playerObjs;

    private GameObject targetObject;
    private Transform targetTransform;

    // Tracks frames for efficient computations
    private int tick;



    // Use this for initialization
    void Start () {
		agent = GetComponent<NavMeshAgent>();
        tick = 0;
    }
	
	// Update is called once per frame
	void Update () {
        tick += 1;

        //Look for new targets every 5 seconds (@ 60 FPS)
        if (tick % 300 == 0) {
            //TODO move this line to startup code when it is written
            playerObjs = GameObject.FindGameObjectsWithTag("Player");
            considerNewTargets();
        }

        //Recompute where the current target is every 10th frame
        if (tick % 10 == 0) {
            updateTargetTransform();
            agent.SetDestination(targetTransform.position);
        }

        // Reset tick if it gets huge. Just cleanup
        if (tick > 1000000000) {
            tick = 0;
        }

        //TODO check if game ended here.
	}

    void updateTargetTransform() {
        targetTransform = targetObject.transform;
    }

    void considerNewTargets() {
        float min = float.MaxValue;
        foreach (GameObject player in playerObjs) {
            // Distance between the monster and the player we are considering
            float thisDist = Vector3.Distance(transform.position, player.transform.position);
            //This is *so close* to being a perfect spot for a ternary operator
            if (thisDist < min) {
                min = thisDist;
                targetObject = player;
            }
        }
    }
}
