using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoppableVeg : MonoBehaviour
{
	[Header( "Parameters" )]
	public float SpeedRequired = 3;
	public float MaxPeelAngle = 60;

	[Header( "References" )]
	public Transform[] Segments;
	public Transform Head;
	public Transform[] Peels;

	// temp, do in order for now - no matter where chopped // "temp" hahahahaha
	int nextchop = 0;

	float chopdelay = 0;
	float peeldelay = 0;
	private float[] PeelProgress = new float[] { 0, 0, 0, 0 };
	private int CompletedPeels = 0;

	private bool Grabbed = false;
	List<GameObject> DestroyLater = new List<GameObject>();

	public enum State
	{
		Spawning,
		Peeling,
		Chopping,
	}
	private State CurrentState = State.Spawning;

	public void OnCollisionEnter( Collision collision )
	{
		if ( CurrentState == State.Spawning ) return;

		ContactPoint[] contacts = new ContactPoint[collision.contactCount];
		collision.GetContacts( contacts );

		switch ( CurrentState )
		{
			case State.Spawning:
				break;
			case State.Peeling:
				foreach ( var contact in contacts )
				{
					if ( contact.otherCollider.tag == "Peel" && CanPeel( contact.otherCollider ) )
					{
						Transform closest = Peels[0];
						float maxdist = -1;
						foreach ( var seg in Peels )
						{
							if ( seg != null )
							{
								float dist = Vector3.Distance( seg.GetChild( 0 ).position, contact.otherCollider.transform.position );
								if ( maxdist == -1 || dist < maxdist )
								{
									closest = seg;
									maxdist = dist;
								}
							}
						}

						Peel( closest );
						GetComponent<AudioSource>().Play();
						GetComponent<AudioSource>().pitch = Random.Range( 0.8f, 1.2f );
						SmithysKitchen.EmitParticleImpact( contact.otherCollider.transform.position );
					}
				}
				break;
			case State.Chopping:
				foreach ( var contact in contacts )
				{
					if ( contact.otherCollider.tag == "Knife" && CanChop( contact.otherCollider ) )
					{
						ChopOff();
						GetComponent<AudioSource>().Play();
						GetComponent<AudioSource>().pitch = Random.Range( 0.8f, 1.2f );
						SmithysKitchen.EmitParticleImpact( contact.otherCollider.transform.position );
					}
				}
				break;
			default:
				break;
		}
	}

	private void OnTriggerEnter( Collider other )
	{
		if ( CurrentState == State.Spawning ) return;

		switch ( CurrentState )
		{
			case State.Spawning:
				break;
			case State.Peeling:
				if ( other.tag == "Peel" )
				{
					Transform closest = Peels[0];
					float maxdist = -1;
					foreach ( var seg in Peels )
					{
						if ( seg != null )
						{
							float dist = Vector3.Distance( seg.GetChild( 0 ).position, other.transform.position );
							if ( maxdist == -1 || dist < maxdist )
							{
								closest = seg;
								maxdist = dist;
							}
						}
					}

					if ( CanPeel( other ) )
					{
						Peel( closest );
						GetComponent<AudioSource>().Play();
						GetComponent<AudioSource>().pitch = Random.Range( 1.8f, 2.2f );
						SmithysKitchen.EmitParticleImpact( other.transform.position );
					}
				}
				break;
			case State.Chopping:
				if ( other.tag == "Knife" )
				{
					Transform closest = Segments[0];
					float maxdist = -1;
					foreach ( var seg in Segments )
					{
						float dist = Vector3.Distance( seg.position, other.transform.position );
						if ( maxdist == -1 || dist < maxdist )
						{
							closest = seg;
							maxdist = dist;
						}
					}

					if ( CanChop( other ) )
					{
						ChopOff();
						GetComponent<AudioSource>().Play();
						GetComponent<AudioSource>().pitch = Random.Range( 1.8f, 2.2f );
						SmithysKitchen.EmitParticleImpact( other.transform.position );
					}
				}
				break;
			default:
				break;
		}
	}

	private bool CanChop( Collider knife )
	{
		if ( CurrentState == State.Chopping && knife.attachedRigidbody.GetComponentInChildren<KinematicVelocity>().Velocity.magnitude >= SpeedRequired && chopdelay <= Time.time )
		{
			return true;
		}
		return false;
	}

	int impress = 0;
	int impressgoal = 5;
	private void ChopOff()
	{
		if ( CurrentState != State.Chopping ) return;

		if ( nextchop >= Segments.Length - 1 )
		{
			Head.GetComponentInParent<Rigidbody>().isKinematic = false;
		}
		if ( nextchop >= Segments.Length ) return;

		Segments[nextchop].SetParent( null );
		Segments[nextchop].tag = "Ingredient";
		//Segments[nextchop].gameObject.AddComponent<Rigidbody>();
		GameObject grab = SmithysKitchen.CreateGrabbable( Segments[nextchop].gameObject );
		var sound = grab.AddComponent<PhysicsSound>();
		sound.SelfType = PhysicsSound.Type.Organic;

		// temp // hahahaha
		impress++;
		if ( impress >= impressgoal )
		{
			FindObjectOfType<Customer>().Impress();
			impress = 0;
			impressgoal = Random.Range( 5, 15 );
		}

		nextchop++;
		chopdelay = Time.time + 0.02f;
	}

	private bool CanPeel( Collider knife )
	{
		if ( CurrentState == State.Peeling && peeldelay <= Time.time )
		{
			return true;
		}
		return false;
	}

	private void Peel( Transform peel )
	{
		if ( CurrentState != State.Peeling ) return;

		// Find which index this peel is
		int peelID = System.Array.IndexOf( Peels, peel );

		// Add progress to that one
		PeelProgress[peelID] += 0.1f;
		Transform basebone = peel.GetChild( 0 ).GetChild( 0 );
		Transform[] bones = new Transform[5];
		bones[0] = basebone.GetChild( 0 );
		bones[1] = bones[0].GetChild( 0 );
		bones[2] = bones[1].GetChild( 0 );
		bones[3] = bones[2].GetChild( 0 );
		bones[4] = bones[3].GetChild( 0 );

		// Invert the effect so the top bone has the most progress always
		for ( int bone = 0; bone < bones.Length; bone++ )
		{
			float effect = ( (float) bone / bones.Length );
			float inverse_effect = ( 1 - effect );
			float mult = bones.Length;
			float localprogress = Mathf.Clamp( ( PeelProgress[peelID] * mult ) - ( mult - 1 - ( 1 / mult ) * bone * mult ), 0.0f, 1.0f );
			bones[bone].localEulerAngles = new Vector3( 1, 0, 0 ) * MaxPeelAngle * localprogress;
		}

		// If progress > 1 then pop off
		if ( PeelProgress[peelID] >= 1 )
		{
			GameObject grab = Disconnect( Peels[peelID] );
			var sound = grab.AddComponent<PhysicsSound>();
			sound.SelfType = PhysicsSound.Type.Organic;
			grab.GetComponentInChildren<Rigidbody>().AddExplosionForce( 10000, transform.position, 10, 5 );
			Destroy( grab, 5 );

			Peels[peelID] = null;

			// Detect done peeling
			CompletedPeels++;
			if ( CompletedPeels >= 4 )
			{
				CurrentState = State.Chopping;
			}
		}

		peeldelay = Time.time + 0.02f;
	}

	public void FirstGrabbed()
	{
		if ( CurrentState == State.Spawning )
		{
			CurrentState = State.Peeling;
			Head.GetComponent<Collider>().enabled = false;
			GetComponentInChildren<VRTK.Prefabs.Interactions.Interactables.Grab.Action.GrabInteractableFollowAction>().GrabOffset = VRTK.Prefabs.Interactions.Interactables.Grab.Action.GrabInteractableFollowAction.OffsetType.PrecisionPoint;
		}
		OnGrabbed();
	}

	public void OnGrabbed()
	{
		Grabbed = true;
	}

	public void OnUnGrabbed()
	{
		Grabbed = false;
		DestroyQueue();
	}

	private GameObject Disconnect( Transform trans )
	{
		if ( Grabbed )
		{
			Debug.Log( "Grabbed! will duplicate..." );
			return DisconnectWhileGrabbed( trans );
		}
		else
		{
			Debug.Log( "Not Grabbed! as normal..." );
			trans.SetParent( null );
			return SmithysKitchen.CreateGrabbable( trans.gameObject );
		}
	}

	private GameObject DisconnectWhileGrabbed( Transform trans )
	{
		// Duplicate the real one
		GameObject dup = Instantiate( trans.gameObject );

		// Unparent
		dup.transform.parent = null;
		dup.transform.position = trans.position;
		dup.transform.rotation = trans.rotation;

		// Add to destroy later
		DestroyLater.Add( trans.gameObject );

		// Remove visuals
		foreach ( var render in trans.GetComponentsInChildren<Renderer>() )
		{
			render.enabled = false;
		}

		// Return the duplicate
		return SmithysKitchen.CreateGrabbable( dup );
	}

	private void DestroyQueue()
	{
		foreach ( var destroy in DestroyLater )
		{
			Destroy( destroy );
		}
		DestroyLater.Clear();
	}
}
