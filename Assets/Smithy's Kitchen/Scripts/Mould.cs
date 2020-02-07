﻿using System.Collections;
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
			AddLiquidLevel( 1 );
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
		if ( other.tag == "Container" && other.transform.up.y <= 0.5f )
		{
			bool added = other.attachedRigidbody.GetComponentInParent<StirBowl>().PourOut();
			if ( added )
			{
				AddLiquidLevel( 0.1f );
			}
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

	private void BakeSword()
	{
		RemainingCookTime = CookTime;

		SetLiquidLevel( 0 );

		GameObject sword = Instantiate( SwordBlankPrefab, SpawnPoint );
		sword.transform.localPosition = Vector3.zero;
		sword.transform.localEulerAngles = Vector3.zero;
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
