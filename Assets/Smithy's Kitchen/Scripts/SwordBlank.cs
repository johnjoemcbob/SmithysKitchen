using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordBlank : MonoBehaviour
{
	[Header( "Parameters" )]
	public float SpeedRequired = 5;

	[Header( "References" )]
	public Transform[] SegmentRotations;

	float hitdelay = 0;

	public void Start()
	{
		SegmentRotations = transform.FindChildren( "SegRotation" );
		Randomise();
	}

	public void OnCollisionEnter( Collision collision )
	{
		ContactPoint[] contacts = new ContactPoint[collision.contactCount];
		collision.GetContacts( contacts );

		foreach ( var contact in contacts )
		{
			if ( contact.otherCollider.tag == "Hammer" && CanStraighten( contact.otherCollider ) )
			{
				contact.thisCollider.transform.parent.localEulerAngles = Vector3.zero;
				GetComponent<AudioSource>().Play();
				GetComponent<AudioSource>().pitch = Random.Range( 0.8f, 1.2f );
				hitdelay = Time.time + 0.02f;
			}
		}

		CheckStraight();
	}

	private void OnTriggerEnter( Collider other )
	{
		if ( other.tag == "Hammer" && CanStraighten( other ) )
		{
			Transform closest = SegmentRotations[0];
			float maxdist = -1;
			foreach ( var seg in SegmentRotations )
			{
				float dist = Vector3.Distance( seg.position, other.transform.position );
				if ( maxdist == -1 || dist < maxdist )
				{
					closest = seg;
					maxdist = dist;
				}
			}

			closest.parent.localEulerAngles = Vector3.zero;
			GetComponent<AudioSource>().Play();
			GetComponent<AudioSource>().pitch = Random.Range( 0.8f, 1.2f );
			hitdelay = Time.time + 0.02f;
		}

		CheckStraight();
	}

	private bool CanStraighten( Collider hammer )
	{
		if ( hammer.attachedRigidbody.GetComponentInChildren<KinematicVelocity>().Velocity.magnitude >= SpeedRequired && hitdelay <= Time.time )
		{
			return true;
		}
		return false;
	}

	private void CheckStraight()
	{
		foreach ( var seg in SegmentRotations )
		{
			if ( seg.localEulerAngles != Vector3.zero )
			{
				return;
			}
		}

		Randomise();
	}

	private void Randomise()
	{
		foreach ( var seg in SegmentRotations )
		{
			seg.localEulerAngles = new Vector3( Random.Range( -20, 20.0f ), 0, 0 );
		}
	}
}
