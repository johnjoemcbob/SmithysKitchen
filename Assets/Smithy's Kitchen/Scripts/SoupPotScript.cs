using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoupPotScript : MonoBehaviour
{
    public GameObject soupObj;
    GameObject baseObj;
    GameObject flakeObj;

    public float maxSoupHeight = 0.275f;

    MeshRenderer soupRenderer;

    // Start is called before the first frame update
    void Start()
    {
        baseObj = soupObj.transform.Find("SoupObj").gameObject;
        baseObj.SetActive(false);

        flakeObj = soupObj.transform.Find("SoupFlakes").gameObject;
        flakeObj.SetActive(false);

        soupRenderer = baseObj.GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddBase(float amount = 0.005f)
    {
        //Activate soup
        if (!baseObj.activeSelf)
            baseObj.SetActive(true);

        //Add soup
        float newHeight = baseObj.transform.localPosition.y;
        newHeight += amount;

        if (newHeight > maxSoupHeight)
            newHeight = maxSoupHeight;

        baseObj.transform.localPosition = new Vector3(0.0f, newHeight, 0.0f);

        //Flakes need to be at the same height as the base at all times
        flakeObj.transform.localPosition = baseObj.transform.localPosition;
    }

    public void LerpBaseMaterial(Material newMaterial, float lerpRate = 0.05f)
    {
        if (!soupRenderer)
            return;

        soupRenderer.material.Lerp(soupRenderer.material, newMaterial, lerpRate);
    }

    public void AddChunks(Material chunkMaterial)
    {
        if (!flakeObj.activeSelf)
            flakeObj.SetActive(true);

        foreach(Transform flakeGroup in flakeObj.transform)
        {
            if (!flakeGroup.gameObject.activeSelf)
            {
                flakeGroup.gameObject.SetActive(true);

                foreach (Transform flake in flakeGroup.transform)
                {
                    flake.gameObject.GetComponent<MeshRenderer>().material = chunkMaterial;
                }

                break;
            }
        }
    }
}
