using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombineZoneScript : MonoBehaviour
{
    public CombineZoneHiltScript hiltZone;
    public CombineZoneBladeScript bladeZone;

    public ParticleSystem particleSystem;

    bool complete = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (hiltZone.IsComplete() && bladeZone.IsComplete())
        {
            if (complete == false)
            {
                particleSystem.Play();
                complete = true;
            }
        }
    }
}
