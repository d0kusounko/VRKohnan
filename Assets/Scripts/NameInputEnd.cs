using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NameInputEnd : MonoBehaviour {

	private float       hitTime          = 0.0f;
	public float        hitEnableTime    = 2.0f;
	public float        noHitAlpha       = 0.2f;
	private bool        isEnable         = true;


	// Use this for initialization
	void Start()
	{
		UpdateMaterial();
	}

	// Update is called once per frame
	void Update()
	{
		bool isHit = false;

		if( isEnable )
		{
			Transform camera = Camera.main.transform;
			Ray ray = new Ray( camera.position, camera.rotation * Vector3.forward );
			RaycastHit hit;
			if( Physics.Raycast( ray, out hit ) )
			{
				// ヒット判定.
				if( hit.collider.gameObject == this.gameObject )
				{
					isHit = true;
					hitTime += Time.deltaTime;
					if( hitTime >= hitEnableTime )
					{
						hitTime = hitEnableTime;
					}
				}
				
			}
		}

		if( isHit == false )
		{
			hitTime = 0;
		}

		UpdateMaterial();
	}

	// マテリアル更新.
	void UpdateMaterial()
	{
		MeshRenderer mesh = GetComponent<MeshRenderer>();
		float setAlpha = noHitAlpha + ( ( 1.0f - noHitAlpha ) * hitTime / hitEnableTime );
		if( !isEnable )
		{
			setAlpha = 0.0f;
		}
		mesh.material.color = new Color( 1.0f, 0.0f, 0.0f, setAlpha );
	}

	// Endボタンが押されたか.
	public bool IsEnd()
	{
		return ( hitTime >= hitEnableTime );
	}

	// ボタン有効設定.
	public void SetEnable( bool enable )
	{
		isEnable = enable;
	}

}
