using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;

public class LookMove : MonoBehaviour
{
	public float acceleration	= 10.0f;
	public float velocity		= 2.0f;

	private Rigidbody rigidbody;
	private bool    isExecute;

	// Use this for initialization
	void Start ()
	{
		rigidbody	= GetComponent<Rigidbody>();
		isExecute	= false;
	}

	public void Execute()
	{
		isExecute = true;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if( !isExecute )
		{
			return;
		}

		// 視線方向に移動.
		Vector3 targetDirection	= Camera.main.transform.forward * velocity;
		Vector3 moveDirection = targetDirection - rigidbody.velocity;
		moveDirection = moveDirection.normalized * acceleration * Time.deltaTime;

		if( Vector3.Distance( rigidbody.velocity, rigidbody.velocity + moveDirection ) >
			Vector3.Distance( targetDirection, rigidbody.velocity + moveDirection ) )
		{
			// 目標視線に近づいている場合は目標視線にそのまま設定.
			rigidbody.velocity = targetDirection;
		}
		else
		{
			rigidbody.velocity += moveDirection;
		}
	}
}
