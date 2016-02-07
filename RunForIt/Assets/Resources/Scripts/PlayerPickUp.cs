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

		messageWidth = 200;
		messageHeight = 25;
    }


    // Use this for initialization
    void OnCollisionEnter(Collision other)
    {
        Debug.Log("collision with " +other.gameObject.tag);
        if (other.gameObject.name == "FlashLightInactive")
        {
            Destroy(other.gameObject);
            other.gameObject.SetActive(false);
            follow = true;
            Transform camFor = transform.FindChild("MainCamera").transform;
            attachedLight = Instantiate(floatLight, new Vector3(camFor.position.x+1.0f,camFor.position.y-1.0f,camFor.position.z-1.0f), camFor.rotation) as GameObject;
            attachedLight.transform.SetParent(camFor);
        }
        //This isn't exactly a pickup...
		if (other.gameObject.tag == "Monster")
		{
            kill();
		}
        if(other.gameObject.tag == "Battery")
        {
            Destroy(other.gameObject);
            other.gameObject.SetActive(false);
            attachedLight.GetComponent<FlashlightScript>().increaseCharge(10f);
        }
    }

    // This belongs in another player script, as it is not a pickup
    public void kill()
    {
        RigidbodyFirstPersonController controller = gameObject.GetComponent<RigidbodyFirstPersonController>();
        controller.disableMovement();
        controller.dead = true;
        dead = true;
    }

    public bool isDead()
    {
        return dead;
    }
}
