using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallColiderColor : MonoBehaviour
{
	public enum CheckAxis
	{
		X,
		Y,
		Z,
	};
	public Transform    checkPosition   = null;
	public CheckAxis	checkAxis	= CheckAxis.X;
	public float        range       = 10.0f;
	public Color        color		= new Color( 0, 255, 0, 0.5f );

	private MeshRenderer meshrender = null;

	void Start()
	{
		meshrender = GetComponent<MeshRenderer>();
	}

	// Update is called once per frame
	void Update ()
	{
		if( checkPosition == null )
		{
			return;
		}

		// checkAxisの軸方向のrange内にカメラが入った時は、
		// 壁に近づくほど透明度を薄くして壁を表示.
		float distance = range;
		switch( checkAxis )
		{
			case CheckAxis.X:
				distance = Mathf.Abs( transform.position.x - checkPosition.position.x );
				break;
			case CheckAxis.Y:
				distance = Mathf.Abs( transform.position.y - checkPosition.position.y );
				break;
			case CheckAxis.Z:
				distance = Mathf.Abs( transform.position.z - checkPosition.position.z );
				break;
		}
		if( distance > range )
		{
			distance = range;
		}

		// Alpha計算.
		float setAlpha = color.a - ( color.a * ( distance / range ) );
		meshrender.material.color = new Color( color.r, color.g, color.b, setAlpha );
	}
}
