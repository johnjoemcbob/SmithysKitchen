using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Main class with helpers
public class SmithysKitchen : MonoBehaviour
{
    public static GameObject CreateGrabbable( GameObject from )
	{
		GameObject grabbable = Instantiate( Resources.Load( "Prefabs/BaseGrabbable" ), from.transform.parent ) as GameObject;
		{
			grabbable.transform.position = from.transform.position;
			grabbable.transform.rotation = from.transform.rotation;

			from.transform.SetParent( grabbable.transform.GetChild( 0 ) );

			from.transform.localPosition = Vector3.zero;
			from.transform.localEulerAngles = Vector3.zero;
		}
		return grabbable;
	}
}
