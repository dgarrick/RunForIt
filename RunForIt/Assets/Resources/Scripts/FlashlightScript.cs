using UnityEngine;
using System.Collections;

public class FlashlightScript : MonoBehaviour
{

    private int charge;
    private Light beam;
    private bool isOn;
    void Start()
    {
        charge = 100;
        isOn = true;
        beam = gameObject.GetComponent("Light") as Light;
        InvokeRepeating("checkCharge", 0, 0.2f);
        beam.intensity = charge;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("CLICKY CLICK");
            if (isOn)
            {
                isOn = false;
                beam.intensity = 0;
            }
            else
            {
                isOn = true;
            }
        }
    }

    void checkCharge()
    {
        if (isOn && charge > 0)
        {
            charge -= 1;
            beam.intensity = charge;
            Debug.Log("OH NO");
        }
        if (charge == 0)
        {
            beam.intensity = 0;
        }
    }

}

