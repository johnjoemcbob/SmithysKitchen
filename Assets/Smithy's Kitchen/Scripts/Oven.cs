using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oven : MonoBehaviour
{
	public Transform Door;
	public GameObject Light;
	public AudioSource SoundLoop;

    void Update()
    {
		Debug.Log( Door.up );
       // if ( Door.up)
    }
}
