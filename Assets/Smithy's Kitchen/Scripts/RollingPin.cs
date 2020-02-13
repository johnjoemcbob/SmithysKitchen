using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK.Prefabs.Interactions.Interactors;

public class RollingPin : MonoBehaviour
{
	public Transform OrientationHandle;

	private int Grabs = 0;
	private bool Right = true;

    public void OnGrabbed( InteractorFacade facade )
    {
		Grabs++;

		// If right then copy transform of second child
		if ( Grabs <= 1 && facade.name.Contains( "Right" ) )
		{
			Right = true;
			SetPos( true );
		}
		// If left then reset to zero
		else
		{
			if ( Grabs <= 1 )
			{
				Right = false;
			}
			SetPos( false );
		}
	}

	public void OnUnGrabbed()
	{
		Grabs = Mathf.Max( Grabs - 1, 0 );
		SetPos( Right );
	}

	private void SetPos( bool right )
	{
		int child = ( right == true ) ? 1 : 0;
		OrientationHandle.parent.localPosition		= OrientationHandle.parent.GetChild( child ).localPosition;
		OrientationHandle.parent.localEulerAngles	= OrientationHandle.parent.GetChild( child ).localEulerAngles;
	}
}
