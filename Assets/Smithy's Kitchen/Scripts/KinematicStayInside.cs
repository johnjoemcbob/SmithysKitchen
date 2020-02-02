using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent( typeof(KinematicVelocity) )]
public class KinematicStayInside : MonoBehaviour
{
	private KinematicVelocity Physics;

	private float start = 0.1f;

	private void Start()
	{
		Physics = GetComponent<KinematicVelocity>();
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
		if ( other.tag == "Ingredient" )
		{
			if ( Physics.Velocity.magnitude > start )
			{
				other.attachedRigidbody.transform.parent = transform;
				other.attachedRigidbody.isKinematic = true;
			}
			if ( Physics.Velocity.magnitude <= start )
			{
				other.attachedRigidbody.transform.parent = null;
				other.attachedRigidbody.isKinematic = false;
			}
		}
		else if ( other.tag == "Water" )
		{
			if ( Physics.Velocity.magnitude > start )
			{
				other.attachedRigidbody.GetComponentInParent<MCBlob>().transform.parent = transform;
				other.attachedRigidbody.isKinematic = true;
			}
			if ( Physics.Velocity.magnitude <= start )
			{
				other.attachedRigidbody.GetComponentInParent<MCBlob>().transform.parent = null;
				other.attachedRigidbody.isKinematic = false;
			}
		}
	}

	private void OnTriggerExit( Collider other )
	{
		if ( other.tag == "Ingredient" )
		{
			other.attachedRigidbody.transform.parent = null;
			other.attachedRigidbody.isKinematic = false;
		}
		else if ( other.tag == "Water" )
		{
			other.attachedRigidbody.GetComponentInParent<MCBlob>().transform.parent = null;
			other.attachedRigidbody.isKinematic = false;
		}
	}
}
