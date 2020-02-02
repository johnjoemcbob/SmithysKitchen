using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oven : MonoBehaviour
{
	public Transform Door;
	public GameObject Light;
	public AudioSource SoundLoop;

	public bool Cooking = false;

	private void Start()
	{
		Door.parent.parent.parent.localEulerAngles = new Vector3( 42.584f, 0, 0 );
	}
	void Update()
    {
		//Debug.Log( Door.up );
		Cooking = ( Door.up.z <= 0.1f );
		if ( Cooking )
		{
			if ( !SoundLoop.isPlaying )
			{
				SoundLoop.Play();
			}
		}
		else
		{
			SoundLoop.Pause();
		}
		Light.SetActive( Cooking );
	}
}
