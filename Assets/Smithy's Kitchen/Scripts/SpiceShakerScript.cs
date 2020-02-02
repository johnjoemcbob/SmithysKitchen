using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiceShakerScript : MonoBehaviour
{
    bool isShaking = false;
    public MeshRenderer spiceRenderer;
    public ParticleSystem particleSprinkles;
    public Color particleColour = Color.white;

    // Start is called before the first frame update
    void Start()
    {
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
            StartShaking();
        }
        else
        {
            StopShaking();
        }
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
