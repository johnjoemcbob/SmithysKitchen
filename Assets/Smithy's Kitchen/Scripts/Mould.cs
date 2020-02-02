using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mould : MonoBehaviour
{
	public Transform Liquid;
	public Transform SpawnPoint;
	public GameObject SwordBlankPrefab;

	private float Level = 0;

	public void AddLiquid( float liquid )
	{
		Level += liquid;
		Level = Mathf.Max( Level, 0 );
		Level = Mathf.Min( Level, 1 );
		Liquid.localPosition = new Vector3( 0, Level * 0.0786f, 0 );
	}

	private void OnCollisionEnter( Collision collision )
	{
		if ( collision.collider.tag == "Water" )
		{
			if ( GetComponentInParent<MCBlob>() != null )
			{
				GetComponentInParent<MCBlob>().Remove( collision.collider as SphereCollider );
			}
			Destroy( collision.rigidbody.gameObject );
			Level += 0.1f;
			Level = Mathf.Min( Level, 1 );
			Liquid.localPosition = new Vector3( 0, Level * 0.0786f, 0 );
		}
	}

	private void OnTriggerEnter( Collider other )
	{
		if ( other.tag == "Water" )
		{
			if ( GetComponentInParent<MCBlob>() != null )
			{
				GetComponentInParent<MCBlob>().Remove( other as SphereCollider );
			}
			Destroy( other.attachedRigidbody.gameObject );
			Level += 0.1f;
			Level = Mathf.Min( Level, 1 );
			Liquid.localPosition = new Vector3( 0, Level * 0.0786f, 0 );
		}
		if ( other.tag == "Oven" )
		{
			if ( Level > 0 )
			{
				Level = 0;
				Liquid.localPosition = new Vector3( 0, Level * 0.0786f, 0 );

				GameObject sword = Instantiate( SwordBlankPrefab, SpawnPoint );
				sword.transform.localPosition = Vector3.zero;
				sword.transform.localEulerAngles = Vector3.zero;
			}
		}
	}
}
