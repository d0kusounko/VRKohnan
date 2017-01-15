using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// アイテム：コーナンロゴ
/// </summary>
public class ItemKohnan : MonoBehaviour
{
	public float hitEnableTime = 3.0f;
	public float rotateVelocity	= 1.0f;
	private Rigidbody rigidbody;
	private float hitEnableCount;
	private MeshRenderer meshRender = null;

	/// <summary>
	/// 出現時処理.
	/// </summary>
	void Start ()
	{
		// MeshRenderer取得.
		GameObject modelObj = transform.FindChild( "Model" ).gameObject;
		meshRender = modelObj.GetComponent<MeshRenderer>();

		// ランダム方向発射.
		rigidbody = GetComponent<Rigidbody>();
		Vector3 forceVector = Quaternion.Euler( Random.Range( -45.0f, 45.0f ), 0, Random.Range( -45.0f, 45.0f ) ) * Vector3.up;
		rigidbody.AddForce( forceVector * 0.1f, ForceMode.Impulse );
		hitEnableCount = hitEnableTime;

		// カラー適用.
		ApplyColor();
	}

	// Update is called once per frame
	void Update ()
	{
		// Y軸回転.
		transform.Rotate( 0, rotateVelocity * Time.deltaTime, 0 );

		// 衝突可能カウント.
		if( hitEnableCount > 0.0f )
		{
			hitEnableCount -= Time.deltaTime;
		}

		// カラー更新.
		ApplyColor();
	}

	public bool IsHitEnable()
	{
		return ( hitEnableCount <= 0.0f );
	}

	/// <summary>
	/// カラー適用.
	/// </summary>
	private void ApplyColor()
	{
		Color setColor	= new Color( 1, 1, 1 );
		// 衝突不可の場合はグレーに.
		if( !IsHitEnable() )
		{
			setColor = new Color( 0.25f, 0.25f, 0.25f );
		}

		// 透明度設定.
		setColor.a	= 1.0f - ( hitEnableCount / hitEnableTime );
		meshRender.material.color = setColor;
	}
}
