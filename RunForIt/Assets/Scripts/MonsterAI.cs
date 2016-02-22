using UnityEngine;
using UnityEngine.Networking;

public class MonsterAI : NetworkBehaviour {

	// -------------------------------
	// Monster attributes and objects
	// -------------------------------
	private NavMeshAgent agent;
    private GameObject[] playerObjs;

    private GameObject targetObject;
    private Transform targetTransform;

    // Tracks frames for efficient computations
    private int tick;

	private int stunFrames;	//# of frames left that monster is stunned for

	// ------------------------
	// Start & Update functions
	// ------------------------
    void Start () {
        // Default values to prevent excess errors at startup
        playerObjs = new GameObject[0];
        targetObject = new GameObject();
		agent = GetComponent<NavMeshAgent>();
        tick = 0;
    }

	void Update () {
		// Reset tick if it gets huge. Just cleanup
		if (tick > 1000000000) {
			tick = 0;
		} else tick += 1;


		//If stunned, set target to self. Else, find closest player
		if (stunFrames > 0) {
			stunFrames--;
			agent.SetDestination(transform.position);
		} else {

			//Look for new targets every 5 seconds (@ 60 FPS)
			if (tick % 300 == 0) {
				//TODO move this line to startup code when it is written
				playerObjs = GameObject.FindGameObjectsWithTag("Player");
				considerNewTargets();
			}

			//Recompute where the current target is every 10th frame
			if (tick % 10 == 0 && playerObjs.Length > 0) {
				updateTargetTransform();
				agent.SetDestination(targetTransform.position);
			}
		}

        //TODO check if game ended here.
	}

	// ---------------
	// Monster methods
	// ---------------
    void updateTargetTransform() {
        if (targetObject == null)
            considerNewTargets();
        targetTransform = targetObject.transform;

        //Hack for non-moving players surviving the monster being right on top of them
        if (Vector3.Distance(transform.position, targetObject.transform.position) < 1.0)
        {
            targetObject.GetComponent<RigidbodyFirstPersonController>().kill();
        }
    }

    void considerNewTargets() {
        float min = float.MaxValue;
        foreach (GameObject player in playerObjs) {
            // Distance between the monster and the player we are considering
            float thisDist = Vector3.Distance(transform.position, player.transform.position);
            if (thisDist < min) {
                //Ensure that the player we are considering is alive
                RigidbodyFirstPersonController controller = player.GetComponent<RigidbodyFirstPersonController>();
                if (!controller.dead) { 
                    min = thisDist;
                    targetObject = player;
                }
            }
        }
    }

	void stun(int duration) {
		stunFrames = duration;
	}

}
