using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingMark : MonoBehaviour {

	public float rotateVelocity = 90.0f;
	private bool isEnable		= false;

	// Use this for initialization
	void Start ()
	{
		SetEnable( false );
	}
	
	// Update is called once per frame
	void Update ()
	{
		if( isEnable )
		{
			// 回転.
			transform.Rotate( 0, rotateVelocity * Time.deltaTime, 0 );
		}
	}

	public void SetEnable( bool enable )
	{
		isEnable = enable;
		GetComponent<MeshRenderer>().enabled = enable;
	}
}
