using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basket : MonoBehaviour
{
	[Header( "Parameters" )]
	public float MaxDist = 1;

	[Header( "References" )]
	public GameObject Prefab;
	public Transform SpawnPoint;

	private GameObject Current;

    void Start()
    {
		Spawn();
	}

    void Update()
    {
        if ( Vector3.Distance( Current.transform.position, SpawnPoint.position ) > MaxDist )
		{
			Spawn();
		}
    }

	private void Spawn()
	{
		Current = Instantiate( Prefab, transform );
		Current.transform.position = SpawnPoint.position;
		Current.transform.eulerAngles = SpawnPoint.eulerAngles;
	}
}
