using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StirBowl : MonoBehaviour
{
	[Header( "Parameters" )]
	public float WhiskTime = 0.4f;
	public int RequiredWhisks = 30;

	[Header( "References" )]
	public Transform[] QuadrantArrows;
	public Transform LiquidLevel;

	List<Collider> PossibleIngredients = new List<Collider>();

	private Vector2 LiquidLevels = new Vector2( 0.072f, 0.211f );
	private Vector2 LiquidScales = new Vector2( 0.85f, 1.05f );

	private int lastquad = -1;
	private int dir = 0;
	private float lastquadtime = -100;
	private int whiskcount = 0;

	private void Start()
	{
		//QuadEntered( 0 );
		//toadd.Add( transform.position + new Vector3( 0, 0.1f ) );
		//toadd.Add( transform.position + new Vector3( 0, 0.2f ) );
		//toadd.Add( transform.position + new Vector3( 0, 0.3f ) );
		//toadd.Add( transform.position + new Vector3( 0, 0.4f ) );
		//toadd.Add( transform.position + new Vector3( 0, 0.5f ) );
		//toadd.Add( transform.position + new Vector3( 0, 0.6f ) );
	}

	List<Vector3> toadd = new List<Vector3>();
	float nextadd = 0;
	float betweenadd = 0.2f;
	private void Update()
	{
		//if ( toadd.Count > 0 && nextadd <= Time.time )
		//{
		//	FindObjectOfType<MCBlob>().Add( toadd[0] );
		//	toadd.RemoveAt( 0 );
		//	nextadd = Time.time + betweenadd;
		//}

		//if ( Input.GetKeyDown( KeyCode.Alpha1 ) )
		//{
		//	int next = lastquad + 1;
		//	if ( next >= QuadrantArrows.Length )
		//	{
		//		next = 0;
		//	}
		//	QuadEntered( next );
		//}
		//if ( Input.GetKeyDown( KeyCode.Alpha2 ) )
		//{
		//	int next = lastquad - 1;
		//	if ( next < 0 )
		//	{
		//		next = QuadrantArrows.Length - 1;
		//	}
		//	QuadEntered( next );
		//}
	}

	private void OnTriggerStay( Collider other )
	{
		// Bowl/mould collision (not blobs)
		if ( other != null && other.attachedRigidbody != null && other.attachedRigidbody.tag == "Container" && other.transform.up.y <= 0.5f )
		{
			PourOut();

			Mould mould = other.GetComponentInParent<Mould>();
			if ( mould )
			{
				other.GetComponentInParent<Mould>().AddLiquidLevel( 0.1f );
			}
		}
	}

	public void QuadEntered( Collider whisk, int quad )
	{
		//if ( whiskcount >= RequiredWhisks ) return;

		if ( lastquad != quad && lastquadtime + WhiskTime <= Time.time )
		{
			Whisk( whisk );

			// Show correct arrow
			// Direction either moving up (so positive change or just looped to 0) and ISN'T looping the other way or otherwise is down
			dir = -1;
			if ( ( lastquad < quad || ( quad == 0 && lastquad != 1 ) ) && !( quad == 3 && lastquad == 0 ) )
			{
				dir = 1;
			}
			int next = quad + dir;
			if ( next >= QuadrantArrows.Length )
			{
				next = 0;
			}
			if ( next < 0 )
			{
				next = QuadrantArrows.Length - 1;
			}
			for ( int arrow = 0; arrow < QuadrantArrows.Length; arrow++ )
			{
				//QuadrantArrows[arrow].gameObject.SetActive( arrow == next );
				QuadrantArrows[arrow].localEulerAngles = new Vector3( 0, 0, 90 * ( arrow + 1 ) + ( ( dir == 1 ) ? 90 : 0 ) + ( arrow % 2 != 0 ? 180 : 0 ) );
				QuadrantArrows[arrow].localScale = new Vector3( ( ( dir == 1 ) ? 0.5f : -0.5f ), 0.5f, 0.5f );
			}

			lastquad = quad;
			lastquadtime = Time.time;
		}
	}

	private void Whisk( Collider whisk )
	{
		//if ( whiskcount >= RequiredWhisks ) return;

		// Reduce any vegetables inside
		// And convert to liquid
		foreach ( var ingredient in PossibleIngredients )
		{
			if ( ingredient != null )
			{
				whiskcount++;

				ingredient.attachedRigidbody.transform.localScale -= Vector3.one * 0.4f;
				if ( ingredient.attachedRigidbody.transform.localScale.x <= 0 )
				{
					Destroy( ingredient.attachedRigidbody.gameObject );
					//toadd.Add( ingredient.transform.position + new Vector3( 0, 0.1f, 0 ) );
				}
			}
		}

		UpdateLiquid();

		SmithysKitchen.EmitParticleImpact( whisk.transform.position );
		GetComponent<AudioSource>().pitch = Random.Range( 0.8f, 1.5f );
		GetComponent<AudioSource>().Play();

		// Stir liquid
		//foreach ( var blob in FindObjectOfType<MCBlob>().BlobObjectsLocations )
		//{
		//	blob.attachedRigidbody.AddForce( whisk.GetComponentInParent<KinematicVelocity>().Velocity * 100 );
		//}

		// Reduce liquid
		//GetComponentInChildren<MCBlob>().isoLevel += 0.4f;

		// Hide tutorial arrows
		if ( whiskcount >= RequiredWhisks )
		{
			for ( int arrow = 0; arrow < QuadrantArrows.Length; arrow++ )
			{
				QuadrantArrows[arrow].gameObject.SetActive( false );
			}
		}
	}

	public void TrackIngredient( Collider other, bool track )
	{
		//Debug.Log( "Track; " + other + " " + track );
		if ( track && !PossibleIngredients.Contains( other ) )
		{
			PossibleIngredients.Add( other );
		}
		else if ( !track && PossibleIngredients.Contains( other ) )
		{
			PossibleIngredients.Remove( other );
		}
	}

	public bool PourOut()
	{
		if ( whiskcount > 0 )
		{
			whiskcount -= Mathf.Min( 1, RequiredWhisks / 10 );
			UpdateLiquid();

			GetComponent<AudioSource>().pitch = Random.Range( 0.8f, 1.5f );
			GetComponent<AudioSource>().Play();

			return true;
		}
		return false;
	}

	private void UpdateLiquid()
	{
		whiskcount = Mathf.Max( whiskcount, 0 );
		whiskcount = Mathf.Min( whiskcount, RequiredWhisks );
		float progress = (float) whiskcount / RequiredWhisks;
		float height = Mathf.Lerp( LiquidLevels.x, LiquidLevels.y, progress );
		float scale = Mathf.Lerp( LiquidScales.x, LiquidScales.y, progress );
		if ( progress == 0 )
		{
			height = 0;
			scale = 0;
		}

		LiquidLevel.localPosition = new Vector3( 0, height, 0 );
		LiquidLevel.localScale = Vector3.one * scale;
	}
}
