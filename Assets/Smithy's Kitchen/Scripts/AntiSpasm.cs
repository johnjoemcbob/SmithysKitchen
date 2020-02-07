using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent( typeof( Rigidbody ) )]
public class AntiSpasm : MonoBehaviour
{
	private float CollideDrag = 100;
	private float LerpSpeed = 5;

	private float BaseDrag;

	private Rigidbody Body;

	//private void Awake()
	//{
	//	Body = GetComponent<Rigidbody>();
	//	BaseDrag = Body.drag;
	//}

	//private void Update()
	//{
	//	Body.drag = Mathf.Lerp( Body.drag, BaseDrag, Time.deltaTime * LerpSpeed );
	//}

	//private void OnTriggerEnter( Collider other )
	//{
	//	Body.drag = CollideDrag;
	//}

	//private void OnTriggerExit( Collider other )
	//{
	//}

	//private void OnCollisionEnter( Collision collision )
	//{
	//	Body.drag = CollideDrag;
	//}

	//private void OnCollisionExit( Collision collision )
	//{
		
	//}
}
