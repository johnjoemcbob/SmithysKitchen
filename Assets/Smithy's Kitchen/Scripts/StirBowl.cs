using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StirBowl : MonoBehaviour
{
	[Header( "Parameters" )]
	public float WhiskTime = 0.4f;
	public float WhiskRequiredSpeed = 0.5f;
	public float LerpSpeed = 5;
	public float LerpAddSpeed = 5;
	public float LiquidGlobSpeed = 5;
	public float LiquidLevelSpeed = 5;
	public float LiquidCastMult = 0.75f;
	public float LiquidLevelPerIngredient = 0.1f;
	public Vector2 LiquidLevels = new Vector2( 0.05f, 0.17f );
	public Vector2 LiquidScales = new Vector2( 1.36f, 1.25f );

	[Header( "References" )]
	public Transform[] QuadrantArrows;
	//public Transform LiquidLevel;
	public Transform IngredientsParent;
	public Transform LiquidParent;
	public Transform WhiskLiquidGlob;

	[Header( "Assets" )]
	public GameObject LiquidPourPrefab;

	[HideInInspector]
	public Mould Mould;

	List<Transform> PossibleIngredients = new List<Transform>();

	private float LiquidLevel = 0;
	private int lastquad = -1;
	private int dir = 0;
	private float lastquadtime = -100;
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
		// Lerp ingredient adding
		// TODO could be better and not getcomponent every frame
		foreach ( Transform ingredient in IngredientsParent )
		{
			if ( ingredient.GetComponent<MeshRenderer>().enabled )
			{
				ingredient.localScale = Vector3.Lerp( ingredient.localScale, new Vector3( 0.1f, 0.05f, 0.1f ), Time.deltaTime * LerpAddSpeed );
			}
		}

		// Lerp whisk glob
		LiquidWhiskGlobTarget = new Vector3( LiquidWhiskGlobTarget.x, Mathf.Clamp( LiquidWhiskGlobTarget.y, 0.06f, 0.07f ), LiquidWhiskGlobTarget.z );
		WhiskLiquidGlob.localPosition = Vector3.Lerp( WhiskLiquidGlob.localPosition, LiquidWhiskGlobTarget, Time.deltaTime * LiquidGlobSpeed );
		//WhiskLiquidGlob.localPosition += new Vector3( 0, -LiquidWhiskGlobTarget.y + 0.07f, 0 ); // todo, do better

		// Lerp liquid level
		LerpLiquid();

		// Liquid level flat
		LiquidParent.rotation = Quaternion.Lerp( LiquidParent.rotation, Quaternion.Euler( Vector3.zero ), Time.deltaTime * LiquidLevelSpeed );

		// Testing
		if ( Input.GetKeyDown( KeyCode.KeypadPlus ) )
		{
			LiquidLevel += LiquidLevelPerIngredient;
			LiquidLevel = Mathf.Min( LiquidLevel, 1 );
		}
		else if ( Input.GetKeyDown( KeyCode.KeypadMinus ) )
		{
			LiquidLevel -= LiquidLevelPerIngredient;
			LiquidLevel = Mathf.Max( LiquidLevel, 0 );
		}

		// Pouring
		UpdatePour();
	}

	private void UpdatePour()
	{
		// Raycast for the liquid extents each frame
		if ( LiquidLevel > 0 )
		{
			int off = 1;
			Vector3[] dirs = new Vector3[]
			{
				new Vector3( 1, 0, 0 ), // North
				new Vector3( 1, 0, 1 ), // North East
				new Vector3( 0.1f, 0, 1 ), // East
				new Vector3( -0.1f, 0, 1 ), // East
				new Vector3( -1, 0, 1 ), // South East
				new Vector3( -1, 0, 0 ), // South
				new Vector3( -1, 0, -1 ), // South West
				new Vector3( -0.1f, 0, -1 ), // West
				new Vector3( 0.1f, 0, -1 ), // West
				new Vector3( 1, 0, -1 ), // North West
			};
			for ( int sphere = 0; sphere < dirs.Length; sphere++ )
			{
				RaycastHit info;
				// Don't collide with the liquid itself
				int layerMask = 1 << LayerMask.NameToLayer( "Bowl" );
				if ( Physics.Raycast( LiquidParent.position, dirs[sphere], out info, 10, layerMask ) )
				{
					LiquidParent.GetChild( sphere + off ).position = Vector3.Lerp( LiquidParent.position, info.point, LiquidCastMult );
				}
				else
				{
					Debug.Log( "pour!" );
					// Pour out
					//GameObject pour = Instantiate( LiquidPourPrefab );
					//pour.transform.position = LiquidParent.GetChild( sphere + off ).position;
					//pour.transform.eulerAngles = new Vector3( 0, transform.eulerAngles.y, 0 );
					//Destroy( pour, 5 );

					// TODO Detect if pouring into mould
					if ( Mould != null )
					{
						Mould.AddLiquidLevel( LiquidLevelPerIngredient );
					}

					// Reduce liquid level
					LiquidLevel -= LiquidLevelPerIngredient;

					break;
				}
			}
		}

		// Hide liquid if none
		LiquidParent.gameObject.SetActive( ( LiquidLevel > 0 ) );
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
		if ( PossibleIngredients.Count > 0 )
		{
			LiquidLevel += LiquidLevelPerIngredient;
			LiquidLevel = Mathf.Min( LiquidLevel, 1 );

			// Find last ingredient
			PossibleIngredients[PossibleIngredients.Count - 1].GetComponent<MeshRenderer>().enabled = false;
			PossibleIngredients.RemoveAt( PossibleIngredients.Count - 1 );
		}

		// Effects
		//SmithysKitchen.EmitParticleImpact( whisk.transform.position );
		//GetComponent<AudioSource>().pitch = Random.Range( 0.8f, 1.5f );
		//GetComponent<AudioSource>().Play();

		// Stir liquid
		LiquidWhiskGlobTarget = transform.InverseTransformPoint( whisk.transform.position );

		// Reduce liquid
		//GetComponentInChildren<MCBlob>().isoLevel += 0.4f;

		// Hide tutorial arrows
		if ( LiquidLevel >= 1 )
		{
			for ( int arrow = 0; arrow < QuadrantArrows.Length; arrow++ )
			{
				QuadrantArrows[arrow].gameObject.SetActive( false );
			}
		}
	}

	public void TrackIngredient( Collider other, bool track )
	{
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
				// Turn mesh renderer on for this "newly added" ingredient and colour correctly
				parent.GetComponent<MeshRenderer>().material.color = other.GetComponent<MeshRenderer>().material.color;
				parent.GetComponent<MeshRenderer>().enabled = true;
				parent.localScale = Vector3.zero;

				// Remove old grabbable objects
				Transform oldroot = other.attachedRigidbody.transform;
				Destroy( oldroot.gameObject );

				// Play rising pitch sound
				IngredientsParent.GetComponent<AudioSource>().pitch = Mathf.Lerp( 2, 3, parent.GetSiblingIndex() / IngredientsParent.childCount );
				IngredientsParent.GetComponent<AudioSource>().Play();

				PossibleIngredients.Add( parent );
			}
		}
	}

	public bool PourOut()
	{
		//if ( LiquidLevel > 0 )
		//{
		//	LiquidLevel -= LiquidLevelPerIngredient;
		//	UpdateLiquid();

		//	GetComponent<AudioSource>().pitch = Random.Range( 0.8f, 1.5f );
		//	GetComponent<AudioSource>().Play();

		//	return true;
		//}
		return false;
	}

	private void LerpLiquid()
	{
		float progress = LiquidLevel;
		float height = Mathf.Lerp( LiquidLevels.x, LiquidLevels.y, progress );
		float scale = Mathf.Lerp( LiquidScales.x, LiquidScales.y, progress );
		if ( progress == 0 )
		{
			height = 0;
			scale = 0;
		}

		// Start at least at the minimum so doesn't pour/leak out of base
		LiquidParent.localPosition = new Vector3( 0, Mathf.Max( LiquidLevels.x, LiquidParent.localPosition.y ), 0 );

		// Lerp
		//LiquidParent.localPosition = new Vector3( 0, height, 0 ); // Lerping causes issues since the pour point isn't updated in time
		LiquidParent.localPosition = Vector3.Lerp( LiquidParent.localPosition, new Vector3( 0, height, 0 ), Time.deltaTime * LerpSpeed );
		LiquidParent.GetComponent<MCBlob>().isoLevel = Mathf.Lerp( LiquidParent.GetComponent<MCBlob>().isoLevel, scale, Time.deltaTime * LerpSpeed );
	}
}
