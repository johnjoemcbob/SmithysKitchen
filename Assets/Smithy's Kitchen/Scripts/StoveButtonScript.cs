using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveButtonScript : MonoBehaviour
{
    public GameObject burner;
    bool isBeingTouched = false;
    bool isOn = false;

    // Start is called before the first frame update
    void Start()
    {
        if (burner != null)
        {
            burner.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTouch()
    {
        if (isBeingTouched)
            return;

        isBeingTouched = true;

        //Toggle the hob
        isOn = !isOn;
        if (burner != null)
        {
            burner.SetActive(isOn);
        }
    }

    public void OnUntouch()
    {
        if (!isBeingTouched)
            return;

        isBeingTouched = false;
    }
}
