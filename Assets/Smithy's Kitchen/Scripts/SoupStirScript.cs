using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoupStirScript : MonoBehaviour
{
    public GameObject soupObj;
    SoupPotScript soupPotScript;

    int previousQuad = -1;
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
            //Lerp the rotation to try and get that fun resistance effect...
        }
        else if (currentQuad > quad)
        {
            //Anticlockwise?
        }

        previousQuad = currentQuad;
        currentQuad = quad;
        Debug.Log("CURRENT QUAD: " + quad);
    }

    public void QuadExited(int quad)
    {
        previousQuad = currentQuad;
        currentQuad = -1;
        //if currentquad is equal to previous quad?
    }
}
