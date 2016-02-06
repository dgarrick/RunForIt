using UnityEngine;
using System.Collections;

public class PlayerPickUp : MonoBehaviour {

    private GameObject floatLight;
    private bool follow;
    GameObject attachedLight;
    void Start()
    {
       floatLight = Resources.Load("Prefabs/FlashLightActive") as GameObject;
    }


    // Use this for initialization
    void OnCollisionEnter(Collision other)
    {
        Debug.Log("collision");
        if (other.gameObject.name == "FlashLightInactive")
        {
            Destroy(other.gameObject);
            other.gameObject.SetActive(false);
            follow = true;
            Transform camFor = transform.FindChild("MainCamera").transform;
            attachedLight = Instantiate(floatLight, new Vector3(camFor.position.x+1.0f,camFor.position.y-1.0f,camFor.position.z-1.0f), camFor.rotation) as GameObject;
            attachedLight.transform.SetParent(camFor);
        }
    }
}
