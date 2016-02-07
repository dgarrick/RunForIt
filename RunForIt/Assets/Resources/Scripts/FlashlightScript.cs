using UnityEngine;
using System.Collections;

public class FlashlightScript : MonoBehaviour
{

    private float charge;
    private Light beam;
    private bool isOn;
    private bool isBright;

    void Start() {
        charge = 40; //8 is max
        isOn = true;
        isBright = false;
        beam = gameObject.GetComponent("Light") as Light;
        InvokeRepeating("checkCharge", 0, 0.2f);
        beam.range = charge;
    }

    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            isBright = false;
            if (isOn) 
                beam.range = 0;
            isOn = !isOn;  
        }
        if (Input.GetMouseButtonDown(1))
        {
            isOn = false;
            if (isBright)
                beam.range = 0;
            isBright = !isBright;
        }
    }

    void checkCharge()
    {
        if (isOn && charge > 0)
        {
            charge -= 0.1f;
            beam.range = Mathf.Pow(charge, 2);
            beam.intensity = 3;
        }
        else if(isBright && charge > 0)
        {
            charge -= 0.3f;
            beam.range = Mathf.Pow(charge,2);
            beam.intensity = 8;
        }
        if (charge == 0)
        {
            beam.range = 0;
            beam.intensity = 0;
        }
    }
    public void increaseCharge(float n)
    {
        charge += n;
    }

}

