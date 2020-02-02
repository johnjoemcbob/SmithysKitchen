using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsSound : MonoBehaviour
{
    public enum Type
	{
		Organic,
		Wood,
		Metal,
		Glass // Shakers maybe?
	}
	public Type SelfType;

	private void OnCollisionEnter( Collision collision )
	{
		foreach ( var contact in collision.contacts )
		{
			PhysicsSound othersound = contact.otherCollider.GetComponentInParent<PhysicsSound>();
			if ( othersound )
			{
				// Get sound
				string name = SelfType.ToString() + othersound.SelfType.ToString();
				AudioClip clip = Resources.Load( "Sounds/" + name ) as AudioClip;
				Debug.Log( "Try forwards; " + name );
				if ( clip == null )
				{
					name = othersound.SelfType.ToString() + SelfType.ToString();
					clip = Resources.Load( "Sounds/" + name ) as AudioClip;
					Debug.Log( "Try backwards; " + name );
				}
				if ( clip != null )
				{
					Debug.Log( "has clip!!" );
					AudioSource.PlayClipAtPoint( clip, transform.position, 0.2f );
				}
			}
		}
	}
}
