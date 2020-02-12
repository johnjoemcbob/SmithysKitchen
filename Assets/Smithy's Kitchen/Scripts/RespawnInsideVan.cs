using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnInsideVan : MonoBehaviour
{
	public static RespawnInsideVan Instance;

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

	public struct Leave
	{
		public Leave( float time, bool prio )
		{
			Time = time;
			HighPriority = prio;
		}

		public float Time;
		public bool HighPriority;
	}

	private Dictionary<GameObject, PosRot> Tracked = new Dictionary<GameObject, PosRot>();
	private Dictionary<GameObject, Leave> Leaving = new Dictionary<GameObject, Leave>();
	List<GameObject> Grabbed = new List<GameObject>();

	private void Awake()
	{
		Instance = this;
	}

	private void Update()
	{
		Grabbed.Clear();
		foreach ( var leave in Leaving )
		{
			// If grabbed then extend respawn timer
			if ( leave.Key.GetComponent<Rigidbody>().isKinematic )
			{
				Grabbed.Add( leave.Key );
			}

			// TODO cleanup list better
			if ( leave.Value.Time + LeaveTime <= Time.time && leave.Key != null )
			{
				Rigidbody body = leave.Key.GetComponent<Rigidbody>();
				body.transform.position = Tracked[leave.Key].Pos;
				body.transform.eulerAngles = Tracked[leave.Key].Rot;

				// Stop physics-ing
				body.velocity = Vector3.zero;
				body.angularVelocity = Vector3.zero;
				body.Sleep();

				// Play sound
				AudioSource source = GetComponentInChildren<AudioSource>();
				source.pitch = Random.Range( 0.8f, 1.4f );
				source.Play();
				source.transform.position = body.transform.position;
			}
		}
		foreach ( var grab in Grabbed )
		{
			Leave leave = Leaving[grab];
			{
				leave.Time = Time.time;
			}
			Leaving[grab] = leave;
		}
	}

	private void OnTriggerEnter( Collider other )
	{
		ZoneChange( other, true, true );
	}

	private void OnTriggerExit( Collider other )
	{
		ZoneChange( other, false, true );
	}

	// Priority is so anything outside of the van definitely respawns,
	// and takes precedence over the RespawnZones inside the counter etc
	public void ZoneChange( Collider other, bool correct, bool priority = false )
	{
		if ( correct )
		{
			if ( !Tracked.ContainsKey( other.attachedRigidbody.gameObject ) && ( other.attachedRigidbody.GetComponentInChildren<ShouldRespawnInsideVan>() || other.attachedRigidbody.GetComponentInParent<ShouldRespawnInsideVan>() ) )
			{
				Tracked.Add( other.attachedRigidbody.gameObject, new PosRot( other.attachedRigidbody.transform.position, other.attachedRigidbody.transform.eulerAngles ) );
			}
			if ( Leaving.ContainsKey( other.attachedRigidbody.gameObject ) && ( priority || !Leaving[other.attachedRigidbody.gameObject].HighPriority ) )
			{
				Leaving.Remove( other.attachedRigidbody.gameObject );
			}
		}
		else
		{
			if ( Tracked.ContainsKey( other.attachedRigidbody.gameObject ) && !Leaving.ContainsKey( other.attachedRigidbody.gameObject ) )
			{
				Leaving.Add( other.attachedRigidbody.gameObject, new Leave( Time.time, priority ) );
			}
		}
	}
}
