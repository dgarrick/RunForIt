using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.UI;

public class PlayerPickUp : MonoBehaviour {

    private GameObject floatLight;
    private bool follow;
	int messageX;
	int messageY;
	int messageWidth;
	int messageHeight;
	bool dead = false;
    bool hasLight = false;
    GameObject attachedLight;

    void Start()
    {
        floatLight = Resources.Load("Prefabs/FlashLightActive") as GameObject;
		dead = false;
		messageX = Screen.width / 2 - 100;
		messageY = Screen.height / 2;
		messageWidth = 200;
		messageHeight = 25;
    }


    // Use this for initialization
    void OnCollisionEnter(Collision other)
    {
        Debug.Log("collision");
        //TODO fix Item tag to Light tag
        if (other.gameObject.tag == "Item" && hasLight == false)
        {
            hasLight = true;
            Destroy(other.gameObject);
            other.gameObject.SetActive(false);
            follow = true;
            Transform camFor = transform.FindChild("MainCamera").transform;
            attachedLight = Instantiate(floatLight, new Vector3(camFor.position.x+1.0f,camFor.position.y-1.0f,camFor.position.z-1.0f), camFor.rotation) as GameObject;
            attachedLight.transform.SetParent(camFor);
        }
		if (other.gameObject.name == "Monster")
		{
			Debug.Log ("monster");
			gameObject.GetComponent<RigidbodyFirstPersonController>().disableMovement();
			dead = true;
		}
        if(other.gameObject.tag == "Battery")
        {
            Destroy(other.gameObject);
            other.gameObject.SetActive(false);
            attachedLight.GetComponent<FlashlightScript>().increaseCharge(10f);
        }
    }

	void OnGUI() {
		if(dead) GUI.TextField(new Rect(messageX,messageY,messageWidth,messageHeight), "You have been brutally murdered.");
	}
}
