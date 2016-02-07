using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.UI;

public class PlayerPickUp : MonoBehaviour {

    private GameObject floatLight;
    private bool follow;
	int messageWidth;
	int messageHeight;
	bool dead = false;
    GameObject attachedLight;

    void Start()
    {
        floatLight = Resources.Load("Prefabs/FlashLightActive") as GameObject;
		dead = false;
    }


    // Use this for initialization
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag != "Untagged")
          Debug.Log("collision with " +other.gameObject.tag);
        if (other.gameObject.tag == "Item")
        {
            Destroy(other.gameObject);
            other.gameObject.SetActive(false);
            follow = true;
            Transform camFor = transform.FindChild("MainCamera").transform;
            attachedLight = Instantiate(floatLight, new Vector3(camFor.position.x+1.0f,camFor.position.y-1.0f,camFor.position.z-1.0f), camFor.rotation) as GameObject;
            attachedLight.transform.SetParent(camFor);
        }
        if(other.gameObject.tag == "Battery")
        {
            Destroy(other.gameObject);
            other.gameObject.SetActive(false);
            // Check if they have a light first
            if (attachedLight != null)
              attachedLight.GetComponent<FlashlightScript>().increaseCharge(10f);
        }
    }

    public bool isDead()
    {
        return dead;
    }
}
