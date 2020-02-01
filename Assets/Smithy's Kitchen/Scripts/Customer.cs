using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customer : MonoBehaviour
{
	[Header( "References" )]
	public GameObject Popup;

	[Header( "Assets" )]
	public AudioClip[] ImpressClips;

    void Start()
	{
		Popup.SetActive( false );
	}

    void Update()
    {
        if ( Input.GetKeyDown( KeyCode.Q ) )
		{
			Impress();
		}

		// TODO Lerp?
    }

	public void Impress()
	{
		// Toggle on the popup
		Popup.SetActive( true );

		// Start moving
		StartCoroutine( Animate() );

		// Make sound
		GetComponent<AudioSource>().clip = ImpressClips[Random.Range( 0, ImpressClips.Length )];
		GetComponent<AudioSource>().pitch = Random.Range( 1, 1.6f );
		GetComponent<AudioSource>().Play();
	}

	public IEnumerator Animate()
	{
		float duration = 1.5f;
		float horizontal = 0.4f;
		float vertical = 0.2f;
		float angle = 30;
		float vertspeed = 5;
		float speed = 5;

		Vector3 startpos = transform.localPosition;
		Vector3 startrot = transform.localEulerAngles;

		float stop = Time.time + duration;
		while ( true )
		{
			yield return new WaitForEndOfFrame();

			transform.localPosition = startpos + transform.forward * Mathf.Sin( Time.time * speed ) * horizontal + transform.up * Mathf.Cos( Time.time * speed ) * vertical;
			transform.localEulerAngles = startrot + new Vector3( 1, 0, 0 ) * Mathf.Sin( Time.time * vertspeed ) * angle;

			if ( Time.time >= stop )
			{
				break;
			}
		}

		transform.localPosition = startpos;
		transform.localEulerAngles = startrot;

		// toggle off popup
		Popup.SetActive( false );
	}
}
