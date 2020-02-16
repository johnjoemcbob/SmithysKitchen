using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoupStirScript : MonoBehaviour
{
    public GameObject soupObj;
    SoupPotScript soupPotScript;

    int currentQuad = -1;

    // Start is called before the first frame update
    void Start()
    {
        soupPotScript = GetComponent<SoupPotScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void QuadEntered(int quad)
    {
        if (currentQuad == -1)
        {
            //Begin
        }
        else if (currentQuad < quad)
        {
            //Clockwise?
        }
        else if (currentQuad > quad)
        {
            //Anticlockwise?
        }

        currentQuad = quad;
        Debug.Log("CURRENT QUAD: " + quad);
    }

    public void QuadExited(int quad)
    {

    }
}
