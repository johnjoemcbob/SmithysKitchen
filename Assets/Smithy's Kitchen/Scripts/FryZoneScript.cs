using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FryZoneScript : MonoBehaviour
{
    List<FryableScript> fryableObjects;
    AudioSource audioSource;

    bool isHeated = false;

    // Start is called before the first frame update
    void Start()
    {
        fryableObjects = new List<FryableScript>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //Fry if heated
        if (isHeated)
        {
            if (fryableObjects.Count > 0)
            {
                foreach (FryableScript fry in fryableObjects)
                {
                    fry.Fry();
                }
            }
        }
    }

    public void SetHeated(bool val)
    {
        if (isHeated == val)
            return;

        isHeated = val;

        if (isHeated)
        {
            if (fryableObjects.Count > 0)
            {
                foreach (FryableScript fry in fryableObjects)
                {
                    fry.StartFry();
                }
            
                if (!audioSource.isPlaying)
                {
                    audioSource.Play();
                }
            }
        }
        else
        {
            if (fryableObjects.Count > 0)
            {
                foreach (FryableScript fry in fryableObjects)
                {
                    fry.StopFry();
                }
            }

            audioSource.Stop();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        FryableScript fryable = other.gameObject.GetComponentInParent<FryableScript>();

        if (fryable != null)
        {
            if (!fryableObjects.Contains(fryable))
            {
                fryableObjects.Add(fryable);

                //Only fry if heated
                if (isHeated)
                {
                    fryable.StartFry();

                    if (!audioSource.isPlaying)
                    {
                        audioSource.Play();
                    }
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        FryableScript fryable = other.gameObject.GetComponentInParent<FryableScript>();

        if (fryable != null)
        {
            if (fryableObjects.Contains(fryable))
            {
                fryable.StopFry();
                fryableObjects.Remove(fryable);

                if (fryableObjects.Count <= 0)
                {
                    audioSource.Stop();
                }
            }
        }
    }
}
