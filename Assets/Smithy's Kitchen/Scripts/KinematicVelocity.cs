using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zinnia.Tracking.Velocity;

// Had my own class so just try to wrap around it without needing to replace everything.......
public class KinematicVelocity : AverageVelocityEstimator
{
	[HideInInspector]
	public Vector3 Velocity
	{
		get
		{
			return DoGetVelocity();
		}
	}
	[HideInInspector]
	public Vector3 AngularVelocity
	{
		get
		{
			return DoGetAngularVelocity();
		}
	}

	// Quick fix ya
	private void Start()
	{
		Source = gameObject;
		RelativeTo = transform.parent.gameObject;
	}
}
