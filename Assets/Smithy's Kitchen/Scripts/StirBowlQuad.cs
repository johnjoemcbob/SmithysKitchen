using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StirBowlQuad : MonoBehaviour
{
	private void OnTriggerEnter( Collider other )
	{
		if ( other.tag == "Whisk" )
		{
			GetComponentInParent<StirBowl>().QuadEntered( other, transform.GetSiblingIndex() );
		}
	}

	private void OnTriggerStay( Collider other )
	{
		if ( other.tag == "Whisk" )
		{
			GetComponentInParent<StirBowl>().QuadEntered( other, transform.GetSiblingIndex() );
		}
		if ( other.tag == "Ingredient" )
		{
			GetComponentInParent<StirBowl>().TrackIngredient( other, true );
		}
	}

	private void OnTriggerExit( Collider other )
	{
		if ( other.tag == "Ingredient" )
		{
			GetComponentInParent<StirBowl>().TrackIngredient( other, false );
		}
		if ( other.tag == "Whisk" )
		{
			GetComponentInParent<StirBowl>().QuadExited( other, transform.GetSiblingIndex() );
		}
	}
}
