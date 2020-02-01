using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StirBowl : MonoBehaviour
{
	[Header( "Parameters" )]
	public float WhiskTime = 0.4f;
	public int RequiredWhisks = 10;

	[Header( "References" )]
	public Transform[] QuadrantArrows;
	public Transform LiquidLevel;

	private Vector2 LiquidLevels = new Vector2( 0.072f, 0.211f );
	private Vector2 LiquidScales = new Vector2( 0.85f, 1 );
	private int lastquad = -1;
	private int dir = 0;
	private float lastquadtime = -100;
	private int whiskcount = 0;

	private void Start()
	{
		//QuadEntered( 0 );
	}

	private void Update()
	{
		if ( Input.GetKeyDown( KeyCode.Alpha1 ) )
		{
			int next = lastquad + 1;
			if ( next >= QuadrantArrows.Length )
			{
				next = 0;
			}
			QuadEntered( next );
		}
		if ( Input.GetKeyDown( KeyCode.Alpha2 ) )
		{
			int next = lastquad - 1;
			if ( next < 0 )
			{
				next = QuadrantArrows.Length - 1;
			}
			QuadEntered( next );
		}
	}

	public void QuadEntered( int quad )
	{
		if ( whiskcount >= RequiredWhisks ) return;

		if ( lastquad != quad && lastquadtime + WhiskTime <= Time.time )
		{
			Debug.Log( "Enter quad; " + quad );

			Whisk();

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
				QuadrantArrows[arrow].gameObject.SetActive( arrow == next );
				QuadrantArrows[arrow].localEulerAngles = new Vector3( 0, 0, 90 * ( arrow + 1 ) + ( ( dir == 1 ) ? 90 : 0 ) + ( arrow % 2 != 0 ? 180 : 0 ) );
				QuadrantArrows[arrow].localScale = new Vector3( ( ( dir == 1 ) ? 0.5f : -0.5f ), 0.5f, 0.5f );
			}

			lastquad = quad;
			lastquadtime = Time.time;
		}
	}

	private void Whisk()
	{
		// Reduce any vegetables inside


		if ( whiskcount >= RequiredWhisks ) return;

		whiskcount++;

		float height = Mathf.Lerp( LiquidLevels.x, LiquidLevels.y, (float) whiskcount / RequiredWhisks );
		float scale = Mathf.Lerp( LiquidScales.x, LiquidScales.y, (float) whiskcount / RequiredWhisks );
		Debug.Log( (float) whiskcount / RequiredWhisks + " " + height + " " + scale );
		LiquidLevel.localPosition = new Vector3( 0, height, 0 );
		LiquidLevel.localScale = Vector3.one * scale;

		// Hide tutorial arrows
		for ( int arrow = 0; arrow < QuadrantArrows.Length; arrow++ )
		{
			QuadrantArrows[arrow].gameObject.SetActive( false );
		}
	}
}
