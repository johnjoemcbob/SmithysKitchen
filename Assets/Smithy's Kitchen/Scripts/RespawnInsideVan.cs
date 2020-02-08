using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnInsideVan : MonoBehaviour
{
	public string Help = "This is only for the van trigger";
	public float LeaveTime = 5;

	public struct PosRot
	{
		public PosRot( Vector3 pos, Vector3 rot )
		{
			Pos = pos;
			Rot = rot;
		}

		public Vector3 Pos;
		public Vector3 Rot;
	}

	private Dictionary<GameObject, PosRot> Tracked = new Dictionary<GameObject, PosRot>();
	private Dictionary<GameObject, float> Leaving = new Dictionary<GameObject, float>();

	private void Update()
	{
		foreach ( var leave in Leaving )
		{
			if ( leave.Value + LeaveTime <= Time.time )
			{
				Rigidbody body = leave.Key.GetComponent<Rigidbody>();
				body.transform.position = Tracked[leave.Key].Pos;
				body.transform.eulerAngles = Tracked[leave.Key].Rot;

				// Stop physics-ing
				body.velocity = Vector3.zero;
				body.angularVelocity = Vector3.zero;
				body.Sleep();
			}
		}
	}

	private void OnTriggerEnter( Collider other )
	{
		if ( !Tracked.ContainsKey( other.attachedRigidbody.gameObject ) && ( other.attachedRigidbody.GetComponentInChildren<ShouldRespawnInsideVan>() || other.attachedRigidbody.GetComponentInParent<ShouldRespawnInsideVan>() ) )
		{
			Tracked.Add( other.attachedRigidbody.gameObject, new PosRot( other.attachedRigidbody.transform.position, other.attachedRigidbody.transform.eulerAngles ) );
		}
		if ( Leaving.ContainsKey( other.attachedRigidbody.gameObject ) )
		{
			Leaving.Remove( other.attachedRigidbody.gameObject );
		}
	}

	private void OnTriggerExit( Collider other )
	{
		if ( Tracked.ContainsKey( other.attachedRigidbody.gameObject ) && !Leaving.ContainsKey( other.attachedRigidbody.gameObject ) )
		{
			Leaving.Add( other.attachedRigidbody.gameObject, Time.time );
		}
	}
}
