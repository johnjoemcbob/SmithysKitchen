using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Main class with helpers
public class SmithysKitchen : MonoBehaviour
{
	bool loading = false;

	public void ButtonResetScene()
	{
		if ( loading ) return;

		AudioClip clip = Resources.Load( "Sounds/phone" ) as AudioClip;
		AudioSource.PlayClipAtPoint( clip, transform.position, 1 );

		SceneManager.LoadSceneAsync( 0 );
		loading = true;
	}

	public void ButtonTestScene()
	{
		if ( loading ) return;

		AudioClip clip = Resources.Load( "Sounds/phone" ) as AudioClip;
		AudioSource.PlayClipAtPoint( clip, transform.position, 1 );

		SceneManager.LoadSceneAsync( 1 );
		loading = true;
	}

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

	public static GameObject EmitParticleImpact( Vector3 point )
	{
		GameObject particle = Instantiate( Resources.Load( "Prefabs/Particle Effect" ) ) as GameObject;
		{
			particle.transform.position = point;

			Destroy( particle, 1 );
		}
		return particle;
	}
}
