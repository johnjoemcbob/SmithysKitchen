using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsSound : MonoBehaviour
{
	public static Vector2 VelocityRange = new Vector2( -1, 5 );
	public static float MaxVolume = 0.3f;
	public static float CooldownTime = 0.1f;

	public enum Type
	{
		Organic,
		Wood,
		Metal,
		Glass // Shakers maybe?
	}
	public Type SelfType;

	private KinematicVelocity Kinematic;
	private Dictionary<string, float> CollisionCooldown = new Dictionary<string, float>();

	private void Start()
	{
		Kinematic = GetComponent<KinematicVelocity>();
	}

	private void OnCollisionEnter( Collision collision )
	{
		if ( Kinematic != null )
		{
			foreach ( var contact in collision.contacts )
			{
				HandleCollider( contact.otherCollider );
			}
		}
	}

	private void OnTriggerEnter( Collider other )
	{
		if ( Kinematic != null )
		{
			HandleCollider( other );
		}
	}

	private void HandleCollider( Collider other )
	{
		//Debug.Log( gameObject + " " + other.gameObject );
		if ( Kinematic == null ) return;

		// Work out custom collision ID for cooldown
		// Always in the same ID order in case the other physics object calls this too.
		int id_other	= other.GetInstanceID();
		int id_self     = gameObject.GetInstanceID();
		string cooldownid = ( id_other < id_self ) ? ( id_other.ToString() + "|" + id_self.ToString() ) : ( id_self.ToString() + "|" + id_other.ToString() );

		// If cooling then return
		if ( CollisionCooldown.ContainsKey( cooldownid ) && CollisionCooldown[cooldownid] > Time.time ) return;

		PhysicsSound othersound = other.GetComponentInParent<PhysicsSound>();
		float velocity = Mathf.Max( Kinematic.Velocity.magnitude, Kinematic.AngularVelocity.magnitude );
		if ( othersound && ( velocity >= VelocityRange.x ) )
		{
			// Get sound
			string name = SelfType.ToString() + othersound.SelfType.ToString();
			AudioClip clip = Resources.Load( "Sounds/" + name ) as AudioClip;
			//Debug.Log( "Try forwards; " + name );
			if ( clip == null )
			{
				name = othersound.SelfType.ToString() + SelfType.ToString();
				clip = Resources.Load( "Sounds/" + name ) as AudioClip;
				//Debug.Log( "Try backwards; " + name );
			}
			if ( clip != null )
			{
				//Debug.Log( "has clip!!" );
				float range = VelocityRange.y - VelocityRange.x;
				float volume = ( velocity - VelocityRange.x ) / range;
				//Debug.Log( velocity + " " + volume );
				AudioSource.PlayClipAtPoint( clip, transform.position, MaxVolume * volume );
			}

			// Apply cooldown
			if ( !CollisionCooldown.ContainsKey( cooldownid ) )
			{
				CollisionCooldown.Add( cooldownid, 0 );
			}
			CollisionCooldown[cooldownid] = Time.time + CooldownTime;
		}
	}
}
