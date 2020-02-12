using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnZone : MonoBehaviour
{
	private void OnTriggerEnter( Collider other )
	{
		RespawnInsideVan.Instance.ZoneChange( other, false );
	}

	private void OnTriggerExit( Collider other )
	{
		RespawnInsideVan.Instance.ZoneChange( other, true );
	}
}
