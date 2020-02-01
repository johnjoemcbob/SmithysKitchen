using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StirBowlQuad : MonoBehaviour
{
	private void OnTriggerEnter( Collider other )
	{
		if ( other.tag == "Whisk" )
		{
			GetComponentInParent<StirBowl>().QuadEntered( transform.GetSiblingIndex() );
		}
	}

	private void OnTriggerStay( Collider other )
	{
		if ( other.tag == "Whisk" )
		{
			GetComponentInParent<StirBowl>().QuadEntered( transform.GetSiblingIndex() );
		}
	}
}
