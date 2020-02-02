using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CombineZoneScript : MonoBehaviour
{
    public CombineZoneHiltScript hiltZone;
    public CombineZoneBladeScript bladeZone;
	public GameObject Canvas;

    public ParticleSystem particleSystem;

    bool complete = false;

    void Start()
    {
        
    }

    void Update()
    {
        if (hiltZone.IsComplete() && bladeZone.IsComplete())
        {
            if (complete == false)
            {
                particleSystem.Play();
				Canvas.SetActive( true );
				FindObjectOfType<Customer>().Impress( 8 );
				GetComponent<AudioSource>().Play();
				StartCoroutine( Reset() );
				complete = true;
            }
        }
    }

	public IEnumerator Reset()
	{
		yield return new WaitForSeconds( 10 );

		SceneManager.LoadSceneAsync( 0 );
	}
}
