using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NameInput : MonoBehaviour
{
	private static char[] nameTable = new char[]{ 'A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P','Q','R','S','T','U','V','W','X','Y','Z',' ', };

	private int nameIndex = 0;
	private GameObject  objArrowUp		 = null;
	private GameObject  objArrowDown     = null;
	private GameObject  objText			 = null;
	private float       hitTimeArrowUp	 = 0.0f;
	private float       hitTimeArrowDown = 0.0f;
	public float		hitEnableTime	 = 0.5f;
	public float        noHitAlpha		 = 0.2f;
	private bool        changeable       = true;

	// Use this for initialization
	void Start ()
	{
		objArrowUp = transform.FindChild( "ArrowUp" ).gameObject;
		objArrowDown = transform.FindChild( "ArrowDown" ).gameObject;
		{
			GameObject objCanvas = transform.FindChild( "Canvas" ).gameObject;
			objText	= objCanvas.transform.FindChild( "Text" ).gameObject;
		}

		UpdateArrowMaterial();
		UpdateText();
	}
	
	// Update is called once per frame
	void Update ()
	{
		bool isHitArrowUp = false;
		bool isHitArrowDown = false;

		if( changeable )
		{
			Transform camera = Camera.main.transform;
			Ray ray = new Ray( camera.position, camera.rotation * Vector3.forward );
			RaycastHit hit;
			if( Physics.Raycast( ray, out hit ) )
			{
				// 上矢印のヒット判定.
				if( hit.collider.gameObject == objArrowUp )
				{
					isHitArrowUp = true;
					hitTimeArrowUp += Time.deltaTime;
					if( hitTimeArrowUp >= hitEnableTime )
					{
						// 次の文字へ.
						++nameIndex;
						if( nameIndex >= nameTable.Length )
						{
							nameIndex = 0;
						}
						hitTimeArrowUp = 0;
					}
				}

				// 下矢印のヒット判定.
				if( hit.collider.gameObject == objArrowDown )
				{
					isHitArrowDown = true;
					hitTimeArrowDown += Time.deltaTime;
					if( hitTimeArrowDown >= hitEnableTime )
					{
						// 前の文字へ.
						--nameIndex;
						if( nameIndex < 0 )
						{
							nameIndex = nameTable.Length - 1;
						}
						hitTimeArrowDown = 0;
					}
				}
			}
		}


		if( isHitArrowUp == false )
		{
			hitTimeArrowUp = 0;
		}
		if( isHitArrowDown == false )
		{
			hitTimeArrowDown = 0;
		}

		UpdateArrowMaterial();
		UpdateText();
	}

	// 矢印マテリアル更新.
	void UpdateArrowMaterial()
	{
		// 上矢印.
		{
			MeshRenderer arrowMesh = null;
			if( objArrowUp )
			{
				arrowMesh = objArrowUp.GetComponent<MeshRenderer>();
			}

			float setAlpha = noHitAlpha + ( ( 1.0f - noHitAlpha ) * hitTimeArrowUp / hitEnableTime );
			if( !changeable )
			{
				setAlpha = 0.0f;
			}
			arrowMesh.material.color = new Color( 1.0f, 1.0f, 1.0f, setAlpha );
		}

		// 下矢印.
		{
			MeshRenderer arrowMesh = null;
			if( objArrowDown )
			{
				arrowMesh = objArrowDown.GetComponent<MeshRenderer>();
			}
			float setAlpha = noHitAlpha + ( ( 1.0f - noHitAlpha ) * hitTimeArrowDown / hitEnableTime );
			if( !changeable )
			{
				setAlpha = 0.0f;
			}
			arrowMesh.material.color = new Color( 1.0f, 1.0f, 1.0f, setAlpha );
		}
	}

	// 名前のテキスト表示更新.
	void UpdateText()
	{
		Text nameText = objText.GetComponent<Text>();
		nameText.text = nameTable[ nameIndex ].ToString();
	}

	// 選択中の一文字を取得.
	public char GetNameOneLetter()
	{
		return nameTable[ nameIndex ];
	}

	// 名前変更可能状態設定.
	public void SetChangeable( bool enable )
	{
		changeable = enable;
	}
}
