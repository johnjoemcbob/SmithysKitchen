using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoupStirScript : MonoBehaviour
{
    public GameObject soupObj;
    SoupPotScript soupPotScript;

    // Start is called before the first frame update
    void Start()
    {
        soupPotScript = GetComponent<SoupPotScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "WoodenSpoon")
        {
            Debug.Log("WOODEN SPOON IS HERE");
        }
    }
}
