using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class CollisionDetection : NetworkBehaviour {

    TransportLayer transportLayer;

	// Use this for initialization
	void Start () {
        transportLayer = GameObject.FindObjectOfType<TransportLayer>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnCollisionEnter(Collision other)
    {
        if (isServer)
        {
            if (other.gameObject.tag == "Player")
			{
				Debug.Log ("Collision with player!" + gameObject.name);
				// Currently flashlights (inactive) are "Item"
				if (gameObject.tag == "Item") {
					Destroy (gameObject);
					NetworkIdentity theirId = other.gameObject.GetComponent<NetworkIdentity> ();
					transportLayer.CmdPickupLight (theirId.netId);
				}
				else if (gameObject.tag == "Battery") {
					Destroy (gameObject);
					NetworkIdentity theirId = other.gameObject.GetComponent<NetworkIdentity> ();
					transportLayer.CmdPickupBattery (theirId.netId);
				}
            }
        }
    }
}
