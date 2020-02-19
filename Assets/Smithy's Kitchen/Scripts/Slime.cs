using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour
{
	public float WobbleMultiplier = 0.01f;

	public Transform[] Spheres;
	public Transform[] Eyes;

	void Start()
    {
        
    }

	private bool blownup = false;
    void Update()
    {
		if ( !blownup )
		{
			foreach ( var sphere in Spheres )
			{
				sphere.transform.localPosition += Vector3.one * Mathf.Sin( Time.time + sphere.transform.GetSiblingIndex() * 2 ) * WobbleMultiplier;
			}
		}

		if ( Input.GetKeyDown( KeyCode.Q ) )//&& !blownup )
		{
			BlowUp();
		}
    }

	private void OnTriggerEnter( Collider other )
	{
		if ( other.attachedRigidbody && other.attachedRigidbody.velocity.magnitude > 0.1f )
		{
			BlowUp();
		}
	}

	void BlowUp()
	{
		if ( blownup ) return;

		// BLOW UP
		Rigidbody body = null;
		foreach ( var sphere in Spheres )
		{
			body = sphere.gameObject.AddComponent<Rigidbody>();
			//body.drag = 10;
			body.AddExplosionForce( 200, transform.position, 50, 3 );
		}
		foreach ( var eye in Eyes )
		{
			eye.parent = null;
			eye.gameObject.AddComponent<Rigidbody>();
		}
		GetComponent<AudioSource>().Play();
		blownup = true;
	}
}
