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
    }

    void Update() {

    }

    public void changeStatus(int status)
    {
        if (status == 0) {
            isOn = false;
            isBright = false;
        }
        else if (status == 1) {
            isOn = true;
            isBright = false;
        }
        else if (status == 2)
        {
            isOn = false;
            isBright = true;
        }
    }

    void checkCharge()
    {
        if (!isOn && !isBright)
        {
			beam.intensity = 0;
        }
        if (isOn && charge > 0)
        {
            charge -= 0.12f;
			beam.intensity = Mathf.Log (charge, 3.2f);
        }
        else if(isBright && charge > 0)
        {
            charge -= 0.36f;
			beam.intensity = Mathf.Log (charge, 1.8f);
        }
        if (charge == 0)
        {
            beam.intensity = 0;
        }
    }
    public void increaseCharge(float n)
    {
        charge += n;
    }

}

