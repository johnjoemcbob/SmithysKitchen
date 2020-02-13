using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK.Prefabs.CameraRig.UnityXRCameraRig.Input;

public class InputDebug : MonoBehaviour
{
    void Update()
    {
        if ( Input.GetKeyDown( KeyCode.P ) )
		{
			GetComponent<UnityAxis1DAction>().Receive( 1 );
		}
    }
}
