using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// For kinematic rigidbodies velocity will not be set, so calculate here by positional change
public class KinematicVelocity : MonoBehaviour
{
	[HideInInspector]
	public Vector3 Velocity;
	[HideInInspector]
	public Vector3 AngularVelocity;

	private Vector3 oldpos;
	private Vector3 oldeuler;

	void Start()
	{
		oldpos = transform.position;
		oldeuler = transform.eulerAngles;
	}

	void Update()
	{
		// Pos
		Vector3 newpos = transform.position;
		var media =  (newpos - oldpos);

		Velocity = media / Time.deltaTime;

		oldpos = newpos;
		newpos = transform.position;

		// Ang
		Vector3 neweuler = transform.eulerAngles;
		media =  (neweuler - oldeuler);

		AngularVelocity = media / Time.deltaTime;

		oldeuler = neweuler;
		neweuler = transform.eulerAngles;
	}
}
