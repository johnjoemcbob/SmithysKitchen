using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiceShakerScript : MonoBehaviour
{
    bool isShaking = false;
    public MeshRenderer spiceRenderer;
    public ParticleSystem particleSprinkles;
    public Color particleColour = Color.white;

    VRTK.Prefabs.Interactions.Interactables.InteractableFacade facade;

    // Start is called before the first frame update
    void Start()
    {
        facade = GetComponent<VRTK.Prefabs.Interactions.Interactables.InteractableFacade>();

        particleSprinkles.Stop();

        ParticleSystem.MainModule ma = particleSprinkles.main;
        ma.startColor = particleColour;

        foreach (Material mat in spiceRenderer.materials)
        {
            if (mat.name.Contains("Spice"))
            {
                mat.color = particleColour;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.up.y <= 0.0f)
        {
            if (facade.IsGrabbed)
                StartShaking();
        }
        else
        {
            StopShaking();
        }

        if (!facade.IsGrabbed)
            StopShaking();
    }

    //----------------------------------------------------------------
    void StartShaking()
    {
        if (isShaking)
            return;

        isShaking = true;
        particleSprinkles.Play();
    }

    //----------------------------------------------------------------
    void StopShaking()
    {
        if (!isShaking)
            return;

        isShaking = false;
        particleSprinkles.Stop();
    }
}
