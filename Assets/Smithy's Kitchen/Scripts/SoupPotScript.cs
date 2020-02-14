using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoupPotScript : MonoBehaviour
{
    public GameObject soupObj;
    GameObject childObj;

    public float maxSoupHeight = 0.275f;

    MeshRenderer soupRenderer;

    // Start is called before the first frame update
    void Start()
    {
        childObj = soupObj.transform.Find("SoupObj").gameObject;
        childObj.SetActive(false);

        soupRenderer = childObj.GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddBase(float amount = 0.005f)
    {
        //Activate soup
        if (!childObj.activeSelf)
            childObj.SetActive(true);

        //Add soup
        float newHeight = childObj.transform.localPosition.y;
        newHeight += amount;

        if (newHeight > maxSoupHeight)
            newHeight = maxSoupHeight;

        childObj.transform.localPosition = new Vector3(0.0f, newHeight, 0.0f);
    }

    public void LerpBaseMaterial(Material newMaterial, float lerpRate = 0.05f)
    {
        if (!soupRenderer)
            return;

        soupRenderer.material.Lerp(soupRenderer.material, newMaterial, lerpRate);
    }
}
