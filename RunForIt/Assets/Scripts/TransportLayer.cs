using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class TransportLayer : NetworkBehaviour {

    public string owner;
    private HashSet<GameObject> players;
	private GameObject lightPrefab;    
	private GameObject syringePrefab;
	GameObject[] scrapedPlayers;
	public enum ITEMTYPES {FLASHLIGHT, SYRINGE, NONE}


    // Use this for initialization
    void Start () {
        players = new HashSet<GameObject>();
        lightPrefab = Resources.Load("Prefabs/FlashLightActive") as GameObject;
		syringePrefab = Resources.Load ("Prefabs/SyringeActive") as GameObject;
    }

    // Update is called once per frame
    void Update()
    {
        scrapedPlayers = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in scrapedPlayers)
        {
            if (!players.Contains(player))
            {
                players.Add(player);
            }

        }
    }

    [Command]
    public void CmdChangeLightStatus(NetworkInstanceId id, int status)
    {
        RpcChangeLightStatus(id, status);
    }

    [Command]
	public void CmdPickupItem(NetworkInstanceId id, string name)
    {
		ITEMTYPES type = ITEMTYPES.NONE;
		if (name.Equals ("FlashLight"))
			type = ITEMTYPES.FLASHLIGHT;
		else if (name.Equals ("Syringe"))
			type = ITEMTYPES.SYRINGE;
		RpcAttachItem (id, type);
    }

	[Command]
	public void CmdPickupBattery(NetworkInstanceId id) 
	{
		RpcPickupBattery (id);
	}

    [ClientRpc]
    public void RpcChangeLightStatus(NetworkInstanceId id, int status)
    {
        foreach (GameObject player in players)
        {
            if (player.GetComponent<NetworkIdentity>().netId == id)
            {
                FlashlightScript theirLight = player.GetComponentInChildren<FlashlightScript>();
                if (theirLight != null)
                    theirLight.changeStatus(status);
            }
        }
    }

    [ClientRpc]
	public void RpcAttachItem(NetworkInstanceId id, ITEMTYPES type)
    {
        foreach (GameObject player in players)
        {
            if (player.GetComponent<NetworkIdentity>().netId == id)
            {
                Vector3 spawnPos = player.transform.position + player.transform.forward + player.transform.right;
				GameObject item;
				switch (type) {
					case ITEMTYPES.FLASHLIGHT:
						item = Instantiate (lightPrefab, spawnPos, player.transform.rotation) as GameObject;
						break;
					case ITEMTYPES.SYRINGE:
						item = Instantiate (syringePrefab, spawnPos, player.transform.rotation) as GameObject;
						break;
					default:
						item = null;
						break;
				}
                item.transform.SetParent(player.transform);
            }
        }
    }

	[ClientRpc]
	public void RpcPickupBattery(NetworkInstanceId id)
	{
		foreach (GameObject player in players)
		{
			if (player.GetComponent<NetworkIdentity> ().netId == id) {
				FlashlightScript theirLight = player.GetComponentInChildren<FlashlightScript> ();
				if (theirLight != null) {
					Debug.Log ("Increasing charge");
					theirLight.increaseCharge (10);
				}
			}
		}
	}
}
