using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundCollide : MonoBehaviour
{
	public Vector2 PitchRange;

	public void OnCollisionEnter( Collision collision )
	{
		GetComponentInChildren<AudioSource>().Play();
		GetComponentInChildren<AudioSource>().pitch = Random.Range( PitchRange.x, PitchRange.y );
	}
}
