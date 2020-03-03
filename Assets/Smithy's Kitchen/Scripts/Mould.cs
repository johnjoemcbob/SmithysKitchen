using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mould : MonoBehaviour
{
	[Header( "References" )]
	public Transform Liquid;
	public Transform SpawnPoint;
	public Collider LiquidInsideTrigger;

	[Header( "Assets ")]
	public GameObject SwordBlankPrefab;

	private float Level = 0;
	private float CookTime = 120;
	private float RemainingCookTime;

	private SwordBlank LastSwordBaked;

	private void Start()
	{
		RemainingCookTime = CookTime;
		Level = 0;
		if ( Application.isEditor )
		{
		//	AddLiquidLevel( 1 );
			CookTime = 1;
		}
	}

	private void Update()
	{
		if ( transform.up.y < 0.5f && LastSwordBaked != null )
		{
			LastSwordBaked.OnGrabbed();
		}
	}

	private void OnTriggerEnter( Collider other )
	{
		// Bowl based collision
		if ( other.tag == "Container" )
		{
			other.attachedRigidbody.GetComponentInParent<StirBowl>().Mould = this;
		}
	}

	private void OnTriggerStay( Collider other )
	{
		if ( other.tag == "Oven" )
		{
			if ( Level > 0.5f && other.GetComponentInParent<Oven>().Cooking )
			{
				RemainingCookTime -= Time.deltaTime;

				if ( RemainingCookTime <= 0 )
				{
					BakeSword();
				}
			}
		}
	}

	private void OnTriggerExit( Collider other )
	{
		// Bowl based collision
		if ( other.tag == "Container" )
		{
			other.attachedRigidbody.GetComponentInParent<StirBowl>().Mould = null;
		}
	}

	private void BakeSword()
	{
		RemainingCookTime = CookTime;

		SetLiquidLevel( 0 );

		GameObject sword = Instantiate( SwordBlankPrefab );
		sword.transform.position = SpawnPoint.position;
		sword.transform.eulerAngles = SpawnPoint.eulerAngles;
		LastSwordBaked = sword.GetComponentInChildren<SwordBlank>();
		LastSwordBaked.Mould = this;

		GetComponent<AudioSource>().Play();

		LiquidInsideTrigger.enabled = false;
	}

	public void AddLiquidLevel( float liquid )
	{
		Level += liquid;
		SetLiquidLevel( Level );
	}

	private void SetLiquidLevel( float level )
	{
		Level = level;
		Level = Mathf.Max( Level, 0 );
		Level = Mathf.Min( Level, 1 );
		Liquid.localPosition = new Vector3( 0, Level * 0.0786f, 0 );
	}

	public void OnSwordGrabbed()
	{
		LiquidInsideTrigger.enabled = true;
		LastSwordBaked = null;
	}
}
