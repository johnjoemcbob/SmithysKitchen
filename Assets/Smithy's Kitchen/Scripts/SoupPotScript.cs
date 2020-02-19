using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoupPotScript : MonoBehaviour
{
    public GameObject burnerObj;
    public GameObject soupObj;
    GameObject baseObj;
    GameObject flakeObj;
    ParticleSystem bubbleSystem;
    MeshRenderer soupRenderer;

    public float maxSoupHeight = 0.275f;

    bool isSnapped = false;
    bool isHeated = false;
    float soupTemperature = 0.0f;
    float soupTempMAX = 20.0f;

    // Start is called before the first frame update
    void Start()
    {
        baseObj = soupObj.transform.Find("SoupObj").gameObject;
        baseObj.SetActive(false);

        flakeObj = soupObj.transform.Find("SoupFlakes").gameObject;
        flakeObj.SetActive(false);

        bubbleSystem = soupObj.transform.Find("BubbleParticles").GetComponent<ParticleSystem>();

        soupRenderer = baseObj.GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isSnapped && (burnerObj != null))
        {
            if (isHeated != burnerObj.activeSelf)
            {
                SetHeated(burnerObj.activeSelf);
            }
        }


        //INCREASE SOUP TEMP ON HEAT / DECREASE ON COOL
        if (isHeated)
        {
            soupTemperature += Time.deltaTime;

            if (soupTemperature > soupTempMAX)
            {
                soupTemperature = soupTempMAX;
            }
        }
        else
        {
            soupTemperature -= Time.deltaTime;

            if (soupTemperature <= 0.0f)
            {
                soupTemperature = 0.0f;
            }
        }

        //Modify with temp
        if ((bubbleSystem != null) && (bubbleSystem.gameObject.activeSelf))
        {
            ParticleSystem.EmissionModule em = bubbleSystem.emission;
            em.rateOverTimeMultiplier = soupTemperature;

            //Disable if cold enough
            if (soupTemperature == 0.0f)
            {
                bubbleSystem.gameObject.SetActive(false);
            }
        }
    }

    //----------------------------------------------------------------
    public void OnSnapped()
    {
        isSnapped = true;

        if (burnerObj != null)
        {
            SetHeated(burnerObj.activeSelf);
        }
    }

    //----------------------------------------------------------------
    public void OnUnsnapped()
    {
        isSnapped = false;

        SetHeated(false);
    }

    //----------------------------------------------------------------
    void SetHeated(bool val)
    {
        if (isHeated == val)
            return;

        isHeated = val;

        if (isHeated)
        {
            if (bubbleSystem != null)
            {
                bubbleSystem.gameObject.SetActive(true);
            }
        }
    }

    //----------------------------------------------------------------
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

    //----------------------------------------------------------------
    public void LerpBaseMaterial(Material newMaterial, float lerpRate = 0.05f)
    {
        if (!soupRenderer)
            return;

        soupRenderer.material.Lerp(soupRenderer.material, newMaterial, lerpRate);

        //Change the bubbles!
        if (bubbleSystem != null)
        {
            Color soupColour = soupRenderer.material.color;
            soupColour.a = 1.0f;
            ParticleSystem.MainModule pm = bubbleSystem.main;
            pm.startColor = soupColour;
        }
    }

    //----------------------------------------------------------------
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
