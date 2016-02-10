using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class TransportLayer : NetworkBehaviour {

    public string owner;
    private HashSet<GameObject> players;
    private GameObject lightPrefab;


    // Use this for initialization
    void Start () {
        players = new HashSet<GameObject>();
        lightPrefab = Resources.Load("Prefabs/FlashLightActive") as GameObject;
    }

    // Update is called once per frame
    void Update()
    {
        GameObject[] scrapedPlayers = GameObject.FindGameObjectsWithTag("Player");
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
    public void CmdPickupLight(NetworkInstanceId id)
    {
        RpcAttachLight(id);
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
    public void RpcAttachLight(NetworkInstanceId id)
    {
        Debug.Log("Rpc received. Trying to attach a light to " + id);
        foreach (GameObject player in players)
        {
            if (player.GetComponent<NetworkIdentity>().netId == id)
            {
                Vector3 spawnPos = player.transform.position + player.transform.forward + player.transform.right;
                GameObject attachedLight = Instantiate(lightPrefab, spawnPos, player.transform.rotation) as GameObject;
                Debug.Log(player.transform);
                attachedLight.transform.SetParent(player.transform);
            }
        }
    }
}
