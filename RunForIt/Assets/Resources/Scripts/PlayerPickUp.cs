using UnityEngine;
using System.Collections;

public class PlayerPickUp : MonoBehaviour {

    // Use this for initialization
    void OnCollisionEnter(Collision other)
    {
        Debug.Log("collision");
        if (other.gameObject.name == "FlashLightInactive")
        {
            Destroy(other.gameObject);
            other.gameObject.SetActive(false);
        }
    }
}
