using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent( typeof(KinematicVelocity) )]
public class KinematicStayInside : MonoBehaviour
{
	private KinematicVelocity Physics;

	private float start = 0.1f;
	private float exitdelay = 0.2f;

	Dictionary<Rigidbody,float> HeldInside = new Dictionary<Rigidbody,float>();

	private void Awake()
	{
		Physics = GetComponent<KinematicVelocity>();
	}

	private void Update()
	{
		List<Rigidbody> toremove = new List<Rigidbody>();
		foreach ( var held in HeldInside )
		{
			if ( held.Value <= Time.time )
			{
				held.Key.transform.parent = null;
				held.Key.isKinematic = false;
				toremove.Add( held.Key );
			}
		}
		foreach ( var remove in toremove )
		{
			HeldInside.Remove( remove );
		}
	}

	private void OnTriggerEnter( Collider other )
	{
		if ( other.tag == "Water" || other.tag == "Ingredient" )
		{
			//other.attachedRigidbody.drag = 1000;
			//other.isTrigger = true;
		}
	}

	private void OnTriggerStay( Collider other )
	{
		if ( other.attachedRigidbody == null || other.attachedRigidbody.isKinematic ) return;

		if ( other.tag == "Ingredient" )
		{
			if ( Physics == null )
			{
				Physics = GetComponent<KinematicVelocity>();
			}
			if ( Physics.Velocity.magnitude > start )
			{
				other.attachedRigidbody.transform.parent = transform;
				other.attachedRigidbody.isKinematic = true;

				if ( !HeldInside.ContainsKey( other.attachedRigidbody ) )
				{
					HeldInside.Add( other.attachedRigidbody, 0 );
				}
				HeldInside[other.attachedRigidbody] = Time.time + exitdelay;
			}
			if ( Physics.Velocity.magnitude <= start )
			{
				//other.attachedRigidbody.transform.parent = null;
				//other.attachedRigidbody.isKinematic = false;
				//HeldInside.Remove( other.attachedRigidbody );
			}
		}
	}

	private void OnTriggerExit( Collider other )
	{
		if ( other.attachedRigidbody == null || !HeldInside.ContainsKey( other.attachedRigidbody ) ) return;

		if ( other.tag == "Ingredient" )
		{
			//other.attachedRigidbody.transform.parent = null;
			//other.attachedRigidbody.isKinematic = false;
			//HeldInside.Remove( other.attachedRigidbody );
		}
	}
}
