using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StirBowl : MonoBehaviour
{
	[Header( "Parameters" )]
	public float WhiskTime = 0.4f;
	public float WhiskRequiredSpeed = 0.5f;
	public float LiquidGlobSpeed = 5;
	public float LiquidLevelSpeed = 5;
	public int RequiredWhisks = 30;

	[Header( "References" )]
	public Transform[] QuadrantArrows;
	public Transform LiquidLevel;
	public Transform IngredientsParent;
	public Transform LiquidParent;
	public Transform WhiskLiquidGlob;

	List<Transform> PossibleIngredients = new List<Transform>();

	private Vector2 LiquidLevels = new Vector2( 0.072f, 0.211f );
	private Vector2 LiquidScales = new Vector2( 0.85f, 1.05f );

	private int lastquad = -1;
	private int dir = 0;
	private float lastquadtime = -100;
	private int whiskcount = 0;
	private Vector3 LiquidWhiskGlobTarget;

	private void Start()
	{
		foreach ( var mesh in IngredientsParent.GetComponentsInChildren<MeshRenderer>() )
		{
			mesh.enabled = false;
		}
	}

	private void Update()
	{
		// Lerp whisk glob pos
		WhiskLiquidGlob.localPosition = Vector3.Lerp( WhiskLiquidGlob.localPosition, LiquidWhiskGlobTarget, Time.deltaTime * LiquidGlobSpeed );
		WhiskLiquidGlob.localPosition += new Vector3( 0, -WhiskLiquidGlob.localPosition.y + 0.17f, 0 ); // todo, do better

		// Liquid level flat
		//LiquidParent.rotation = Quaternion.Lerp( LiquidParent.rotation, Quaternion.Euler( Vector3.zero ), Time.deltaTime * LiquidLevelSpeed );
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
		if ( lastquad != quad && lastquadtime + WhiskTime <= Time.time && ( whisk.GetComponentInParent<KinematicVelocity>().Velocity.magnitude >= WhiskRequiredSpeed || whisk.GetComponentInParent<KinematicVelocity>().AngularVelocity.magnitude >= WhiskRequiredSpeed ) )
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
				QuadrantArrows[arrow].localEulerAngles = new Vector3( 0, 0, 90 * ( arrow + 1 ) + ( ( dir == 1 ) ? 90 : 0 ) + ( arrow % 2 != 0 ? 180 : 0 ) );
				QuadrantArrows[arrow].localScale = new Vector3( ( ( dir == 1 ) ? 0.5f : -0.5f ), 0.5f, 0.5f );
			}

			lastquad = quad;
			lastquadtime = Time.time;
		}
	}

	private void Whisk( Collider whisk )
	{
		// Reduce any vegetables inside
		// And convert to liquid
		{
			if ( PossibleIngredients.Count > 0 )
			{
				whiskcount++;

				// Find last ingredient
				PossibleIngredients[PossibleIngredients.Count - 1].GetComponent<MeshRenderer>().enabled = false;
				PossibleIngredients.RemoveAt( PossibleIngredients.Count - 1 );
			}
		}

		UpdateLiquid();

		//SmithysKitchen.EmitParticleImpact( whisk.transform.position );
		//GetComponent<AudioSource>().pitch = Random.Range( 0.8f, 1.5f );
		//GetComponent<AudioSource>().Play();

		// Stir liquid
		Vector3 target = transform.InverseTransformPoint( whisk.transform.position );
		//target += new Vector3( 0, -WhiskLiquidGlob.localPosition.y + 0.17f, 0 ); // todo, do better
		LiquidWhiskGlobTarget = target;

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
		if ( track )
		{
			// Find any free spots to add to
			Transform parent = null;
			foreach ( Transform ingredient in IngredientsParent )
			{
				if ( !ingredient.GetComponent<MeshRenderer>().enabled )
				{
					parent = ingredient;
					break;
				}
			}

			// If free spot then add
			if ( parent != null )
			{
				// Get only visual
				Transform oldroot = other.attachedRigidbody.transform;
				//Transform visual = oldroot.transform.Find( "Meshes" );
				//foreach ( var collider in visual.GetComponentsInChildren<Collider>() )
				//{
				//	collider.enabled = false;
				//}

				//// Move ingredient to be a child of the bowl
				//visual.SetParent( parent );
				//visual.localPosition = Vector3.zero;
				//visual.localEulerAngles = Vector3.zero;
				parent.GetComponent<MeshRenderer>().material.color = other.GetComponent<MeshRenderer>().material.color;
				parent.GetComponent<MeshRenderer>().enabled = true;

				// Remove other logics (just model)
				Destroy( oldroot.gameObject );

				// Play rising pitch sound
				IngredientsParent.GetComponent<AudioSource>().pitch = Mathf.Lerp( 2, 3, parent.GetSiblingIndex() / IngredientsParent.childCount );
				IngredientsParent.GetComponent<AudioSource>().Play();

				PossibleIngredients.Add( parent );
			}
		}
		//else if ( !track && PossibleIngredients.Contains( other ) )
		//{
		//	PossibleIngredients.Remove( other );
		//}
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
