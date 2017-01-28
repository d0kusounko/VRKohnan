using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NameInputSystem : MonoBehaviour {

	private static int	nameInputNum	= 3;
	private ArrayList	objNameInput	= new ArrayList();
	private GameObject  objEndButton    = null;
	private bool        isDecide        = false;

	// Use this for initialization
	void Start () {

		for( int i = 0; i < nameInputNum; ++i )
		{
			objNameInput.Add( transform.FindChild( "NameInput" + ( i + 1 ).ToString() ).gameObject );
		}
		objEndButton = transform.FindChild( "EndButton" ).gameObject;
	}
	
	// Update is called once per frame
	void Update ()
	{
		// 决定待ち.
		if( !isDecide && objEndButton.GetComponent<NameInputEnd>().IsEnd() )
		{
			// 名前入力を変更不可に.
			foreach( GameObject obj in objNameInput )
			{
				obj.GetComponent<NameInput>().SetChangeable( false );
			}
			objEndButton.GetComponent<NameInputEnd>().SetEnable( false );

			isDecide = true;
		}
	}

	// 决定したか.
	public bool IsDecide()
	{
		return isDecide;
	}

	// 名前を取得.
	public string GetName()
	{
		string name	= "";
		foreach( GameObject obj in objNameInput )
		{
			name	+= obj.GetComponent<NameInput>().GetNameOneLetter().ToString();
		}
		return name;
	}
}
