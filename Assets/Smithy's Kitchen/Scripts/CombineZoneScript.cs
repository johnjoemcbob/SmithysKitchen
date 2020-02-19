using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CombineZoneScript : MonoBehaviour
{
    public CombineZoneHiltScript HiltZone;
	public GameObject Canvas;

    public ParticleSystem ParticleSystem;

    private bool complete = false;

    void Update()
    {
   //     if ( HiltZone.IsComplete() )// && bladeZone.IsComplete())
   //     {
			//Complete();
   //     }
    }

	public void Complete()
	{
		if ( !complete )
		{
			ParticleSystem.Play();
			ParticleSystem.GetComponentInParent<AudioSource>().Play();

			// TODO replace this somehow
			// Maybe after the completed sword is given to the customer
			Canvas.SetActive( true );

			FindObjectOfType<Customer>().Impress( 8 );

			StartCoroutine( Reset() );
			complete = true;
		}
	}

	public IEnumerator Reset()
	{
		yield return new WaitForSeconds( 10 );

		SceneManager.LoadSceneAsync( 0 );
	}
}
