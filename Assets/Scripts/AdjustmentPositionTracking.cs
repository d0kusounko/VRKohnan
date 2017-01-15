using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;

public class AdjustmentPositionTracking : MonoBehaviour
{
	public float scale  = 1.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		Vector3 basePos = Vector3.zero;

		// VR.InputTracking から hmd の位置を取得.
		Vector3 trackingPos = InputTracking.GetLocalPosition( VRNode.CenterEye );

		trackingPos -= trackingPos * scale;

		// 固定したい位置から hmd の位置を
		// 差し引いて実質 hmd の移動を無効化する
		transform.localPosition = basePos - trackingPos;

		Vector3 debugTrackingPos = -trackingPos;
		Debug.Log( "HeadTrankingPos: X(" + debugTrackingPos.x +
			"), Y(" + debugTrackingPos.y +
			"), Z(" + debugTrackingPos.z );
	}
}
