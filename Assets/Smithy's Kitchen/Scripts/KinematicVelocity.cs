using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// For kinematic rigidbodies velocity will not be set, so calculate here by positional change
public class KinematicVelocity : MonoBehaviour
{
	[HideInInspector]
	public Vector3 Velocity;

	private Vector3 oldpos;

	void Start()
	{
		oldpos = transform.position;
	}

	void Update()
	{
		Vector3 newpos = transform.position;
		var media =  (newpos - oldpos);

		Velocity = media / Time.deltaTime;

		oldpos = newpos;
		newpos = transform.position;
	}
}
