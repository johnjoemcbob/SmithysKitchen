using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoppableVeg : MonoBehaviour
{
	[Header( "Parameters" )]
	public float SpeedRequired = 3;

	[Header( "References" )]
	public Transform[] Segments;

	// temp, do in order for now - no matter where chopped
	int nextchop = 0;

	float chopdelay = 0;

	public void OnCollisionEnter( Collision collision )
	{
		ContactPoint[] contacts = new ContactPoint[collision.contactCount];
		collision.GetContacts( contacts );

		foreach ( var contact in contacts )
		{
			if ( contact.otherCollider.tag == "Knife" && CanChop( contact.otherCollider ) )
			{
				ChopOff( contact.thisCollider.transform );
				GetComponent<AudioSource>().Play();
				GetComponent<AudioSource>().pitch = Random.Range( 0.8f, 1.2f );
			}
		}
	}

	private void OnTriggerEnter( Collider other )
	{
		if ( other.tag == "Knife" )
		{
			Transform closest = Segments[0];
			float maxdist = -1;
			foreach ( var seg in Segments )
			{
				float dist = Vector3.Distance( seg.position, other.transform.position );
				if ( maxdist == -1 || dist < maxdist )
				{
					closest = seg;
					maxdist = dist;
				}
			}

			if ( CanChop( other ) )
			{
				ChopOff( closest );
				GetComponent<AudioSource>().Play();
				GetComponent<AudioSource>().pitch = Random.Range( 1.8f, 2.2f );
			}
		}
	}

	private bool CanChop( Collider knife )
	{
		if ( knife.attachedRigidbody.GetComponentInChildren<KinematicVelocity>().Velocity.magnitude >= SpeedRequired && chopdelay <= Time.time )
		{
			return true;
		}
		return false;
	}

	private void ChopOff( Transform segment )
	{
		//segment.SetParent( null );

		Segments[nextchop].SetParent( null );
		//Segments[nextchop].gameObject.AddComponent<Rigidbody>();
		SmithysKitchen.CreateGrabbable( Segments[nextchop].gameObject );

		nextchop++;
		chopdelay = Time.time + 0.02f;
	}
}
