using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiquidSplat : MonoBehaviour
{
	private float time = 0;

	private void OnCollisionStay( Collision collision )
	{
		if ( collision.collider.tag != "Container" && collision.collider.tag != "Water" && collision.collider.tag != "Utensil" && collision.collider.tag != "Knife" && collision.collider.tag != "Whisk" )
		{
			time += Time.deltaTime;
			if ( time >= 0.5f )
			{
				Debug.Log( collision.collider );
				GetComponentInParent<MCBlob>().Remove( GetComponentInChildren<SphereCollider>() );
				Destroy( GetComponentInParent<Rigidbody>().gameObject, 1 );
				GetComponent<AudioSource>().Play();
			}
		}
		else
		{
			time = 0;
		}
	}

	private void OnCollisionExit( Collision collision )
	{
		time = 0;
	}
}
