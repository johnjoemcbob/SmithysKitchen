using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoppableVeg : MonoBehaviour
{
	public Transform[] Segments;

	// temp, do in order for now - no matter where chopped
	int nextchop = 0;

	public void OnCollisionEnter( Collision collision )
	{
		ContactPoint[] contacts = new ContactPoint[collision.contactCount];
		collision.GetContacts( contacts );

		foreach ( var contact in contacts )
		{
			if ( contact.otherCollider.tag == "Knife" )
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

			ChopOff( closest );
			GetComponent<AudioSource>().Play();
			GetComponent<AudioSource>().pitch = Random.Range( 1.8f, 2.2f );
		}
	}

	private void ChopOff( Transform segment )
	{
		//segment.SetParent( null );

		Segments[nextchop].SetParent( null );
		Segments[nextchop].gameObject.AddComponent<Rigidbody>();
		nextchop++;
	}
}
