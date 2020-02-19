using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoupStirScript : MonoBehaviour
{
    public GameObject soupObj;
    SoupPotScript soupPotScript;

    int previousQuad = -1;
    int currentQuad = -1;

    float targetRotateStr = 0.0f;
    float currentRotateStr = 0.0f;
    float strMAX = 40.0f;

    float exitTimer = 0.0f;
    float exitTimerMAX = 3.0f;

    // Start is called before the first frame update
    void Start()
    {
        soupPotScript = GetComponent<SoupPotScript>();
    }

    // Update is called once per frame
    void Update()
    {
        currentRotateStr = Mathf.Lerp(currentRotateStr, targetRotateStr, 0.01f);
        soupObj.transform.Rotate(0.0f, currentRotateStr * 0.1f, 0.0f);

        //Reset if time runs out
        exitTimer -= Time.deltaTime;
        if (exitTimer <= 0.0f)
        {
            currentQuad = -1;
            previousQuad = -1;
            targetRotateStr = Mathf.Lerp(targetRotateStr, 0.0f, 0.01f);
        }
    }

    public void QuadEntered(int quad)
    {
        if (currentQuad < quad)
        {
            //Clockwise?
            //Lerp the rotation to try and get that fun resistance effect...
            targetRotateStr += 5.0f;
        }
        else if (currentQuad > quad)
        {
            //Anticlockwise?
            targetRotateStr -= 5.0f;
        }

        if (targetRotateStr > strMAX)
            targetRotateStr = strMAX;

        if (targetRotateStr < -strMAX)
            targetRotateStr = strMAX;

        currentQuad = quad;
        exitTimer = exitTimerMAX;

        //Debug.Log("CURRENT QUAD: " + quad);
    }

    public void QuadExited(int quad)
    {
        previousQuad = quad;
        //if currentquad is equal to previous quad?
    }
}
